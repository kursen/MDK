using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using SKA.Models;
using Telerik.Web.Mvc;
using SKA.Areas.Setting.Models.ViewModels;
using System.Data.SqlClient;
namespace SKA.Controllers
{
    public class AccountController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        private SKAEntities entities;
        private BaseController home;
        public AccountController()
        {
            entities = new SKAEntities();
            home = new BaseController();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            using (var entities = new SKAEntities())
            {
                var model = (from up in entities.UserProfiles
                             where up.Username == User.Identity.Name
                             select new UserDetailModel
                             {
                                 Username = up.Username,
                                 Address = up.Address,
                                 BranchName = up.Branch.Name,
                                 Status = up.SystemStatusUserProfile.Status,
                                 Email = up.Email
                             }).FirstOrDefault();
                ViewData.Model = model;
            }
            return View();
        }

        [GridAction]
        public ActionResult GetListUser()
        {
            var getBranch = (from c in entities.UserProfiles
                             where c.Username == User.Identity.Name
                             select new BranchViewModel
                             {
                                 Id = c.BranchId,
                                 Code = c.Branch.Code
                             }).FirstOrDefault();
            
            var dataUser = (from c in entities.UserProfiles
                            where c.Username != User.Identity.Name
                            orderby c.Username descending   
                            select new RegisterModel
                            {
                                Id = c.Id,
                                UserName = c.Username,
                                Address = c.Address,
                                BranchName = c.Branch.Name,
                                Email = c.Email,
                                Status = c.SystemStatusUserProfile.Status,
                                BranchId = c.BranchId
                            });

            if (getBranch.Code != "00")
            {
                dataUser = dataUser.Where(a => a.BranchId == getBranch.Id);
            }
            return View(new GridModel<RegisterModel>(dataUser));
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            var getAutentikasi = entities.UserProfiles.Where(a => a.Username == model.UserName).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    if (getAutentikasi != null)
                    {
                        FormsService.SignIn(model.UserName, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            var name = User.Identity.Name;
            var branchid = (from c in entities.UserProfiles
                            where c.Username == name
                            select c).FirstOrDefault();

            var lastHistory = (from c in entities.TimesLogOuts
                               where c.UserId == branchid.Id && c.BranchId == branchid.BranchId
                               select c).FirstOrDefault();

            if (lastHistory != null)
            {
                lastHistory.LogoutDate = DateTime.Now;
            }
            else
            {
                var historyLogout = new TimesLogOut();
                historyLogout.Id = Guid.NewGuid();
                historyLogout.UserId = branchid.Id;
                historyLogout.BranchId = branchid.BranchId;
                historyLogout.LogoutDate = DateTime.Now;

                entities.TimesLogOuts.AddObject(historyLogout);
            }
            
            entities.SaveChanges();
            FormsService.SignOut();
            //Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************
        public ActionResult Register()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            

            using (var entities = new SKAEntities())
            {
                var model = from c in entities.Branches
                             select c;

                var getBranch = (from c in entities.UserProfiles
                                 where c.Username == User.Identity.Name
                                 select new BranchViewModel
                                 {
                                     Id = c.BranchId,
                                     Code = c.Branch.Code
                                 }).FirstOrDefault();

                if (getBranch.Code != "00")
                {
                    model = model.Where(a => a.Id == getBranch.Id);
                }

                var branchCode = new SelectList(model.ToList(), "Id", "Name");
                ViewBag.Branch = branchCode;
            };
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            var getUser = (from c in entities.UserProfiles
                           where c.Username == model.UserName
                           select new BranchViewModel
                           {
                               Id = c.BranchId,
                               Code = c.Branch.Code
                           }).FirstOrDefault();

            var branches = (from c in entities.Branches
                           select c).ToList();

            var branchCode = new SelectList(branches, "Id", "Name");
            ViewBag.Branch = branchCode;
           

            if (getUser == null)
            {
                if (ModelState.IsValid)
                {
                    // Attempt to register the user
                    MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        var role = Request.Form["role"];
                        //FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                        Roles.AddUserToRole(model.UserName, role);

                        var getStatus = entities.SystemStatusUserProfiles.Where(a => a.Status == role).FirstOrDefault();
                        var newUser = new UserProfile();
                        newUser.Id = Guid.NewGuid();
                        newUser.Username = model.UserName;
                        newUser.Email = model.Email;
                        newUser.BranchId = model.BranchId;
                        newUser.SystemStatusUserProfileId = getStatus.Id;
                        entities.UserProfiles.AddObject(newUser);
                        
                        //-------------save history----------------//
                        var name = User.Identity.Name;
                        var user = (from c in entities.UserProfiles
                                        where c.Username == name
                                        select c).FirstOrDefault();
                        var history = new UserHistory();
                        history.Id = Guid.NewGuid();
                        history.UserId = user.Id;
                        history.BranchId = user.BranchId;
                        history.Date = DateTime.Now;
                        history.Activity = Request.Url.ToString() + "[Username: " + model.UserName + "]";

                        entities.UserHistories.AddObject(history);
                        entities.SaveChanges();
                        //home.saveHistory("Nama : " + model.UserName + ", Cabang: " + model.BranchName);
                        return RedirectToAction("Index", "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Nama pengguna yang dimasukkan telah digunakan, silahkan masukkan nama pengguna lainnya.");
                return View(model);
            }
            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var model = (from c in entities.UserProfiles
                        where c.Id == id
                        select new RegisterModel
                        {
                            UserName = c.Username,
                            Address = c.Address,
                            BranchId = c.BranchId,
                            BranchName = c.Branch.Name,
                            Email = c.Email,
                            Status = c.SystemStatusUserProfile.Status,
                            StatusId = c.SystemStatusUserProfileId
                        }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RegisterModel model)
        {
            var getUpdateUser = (from c in entities.UserProfiles
                                        where c.Id == model.Id
                                        select c).FirstOrDefault();

            var branches = from c in entities.Branches
                            select c;

            var getBranch = (from c in entities.UserProfiles
                             where c.Username == User.Identity.Name
                             select new BranchViewModel
                             {
                                 Id = c.BranchId,
                                 Code = c.Branch.Code
                             }).FirstOrDefault();

            if (getBranch.Code != "00")
            {
                branches = branches.Where(a => a.Id == getBranch.Id);
            }

            var branchCode = new SelectList(branches.ToList(), "Id", "Name");
            ViewBag.Branch = branchCode;

            if (getUpdateUser == null)
            {
                return RedirectToAction("Index");
            }

            var membership = Membership.GetUser(getUpdateUser.Username);
           
            var role = Request.Form["role"];
            var getStatus = entities.SystemStatusUserProfiles.Where(a => a.Status == role).FirstOrDefault();
            if (membership != null)
            { 
                try
                {
                    if (model.Email != membership.Email)
                    {
                        var getEmail = Membership.GetUserNameByEmail(model.Email);
                        if (getEmail == null)
                        {
                            membership.Email = model.Email;
                            Membership.UpdateUser(membership);
                        }
                        else
                        {

                            ModelState.AddModelError("", "Email yang dimasukkan telah digunakan, silahkan masukkan alamat email lainnya.");
                        }
                    }
                    Roles.RemoveUserFromRole(getUpdateUser.Username, getUpdateUser.SystemStatusUserProfile.Status);
                    Roles.AddUserToRole(model.UserName, role);


                    if (!model.Password.Equals("") && !model.ConfirmPassword.Equals(""))
                    {
                        if (membership.IsLockedOut == true)
                        {
                            membership.UnlockUser();
                        }

                        MembershipService.ChangePassword(getUpdateUser.Username, membership.ResetPassword(), model.Password);
                    }
                    getUpdateUser.Username = model.UserName;
                    getUpdateUser.Address = model.Address;
                    getUpdateUser.SystemStatusUserProfileId = getStatus.Id;
                    getUpdateUser.Email = model.Email;

                    var name = User.Identity.Name;
                    var user = (from c in entities.UserProfiles
                                where c.Username == name
                                select c).FirstOrDefault();
                    
                    var history = new UserHistory();
                    history.Id = Guid.NewGuid();
                    history.UserId = user.Id;
                    history.BranchId = user.BranchId;
                    history.Date = DateTime.Now;
                    history.Activity = Request.Url.ToString() + "[Username: " + model.UserName + "]";

                    entities.UserHistories.AddObject(history);
                    entities.SaveChanges();
                    //home.saveHistory("Nama : " + model.UserName + ", Cabang : " + model.BranchName);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Data tidak bisa diubah");
                }
            }
            model.UserName = getUpdateUser.Username;
            model.Address = getUpdateUser.Address;
            model.BranchName = getUpdateUser.Branch.Name;
            model.StatusId = getUpdateUser.SystemStatusUserProfileId;
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(Guid id)
        {

            var model = (from c in entities.UserProfiles
                         where c.Id == id
                         select new RegisterModel
                         {
                             UserName = c.Username,
                             Address = c.Address,
                             BranchId = c.BranchId,
                             BranchName = c.Branch.Name,
                             Email = c.Email,
                             Status = c.SystemStatusUserProfile.Status,
                             StatusId = c.SystemStatusUserProfileId
                         }).FirstOrDefault();
            return View(model);
        }
        
        [HttpPost]
        public ActionResult Delete(RegisterModel model)
        {
            var getDeleteUser = (from c in entities.UserProfiles
                                 where c.Id == model.Id
                                 select c).FirstOrDefault();
            if (getDeleteUser == null)
            {
                return RedirectToAction("Index");
            }
            var membership = Membership.GetUser(getDeleteUser.Username);
            if (membership != null)
            {
                try
                {
                    entities.UserProfiles.DeleteObject(getDeleteUser);
                    Membership.DeleteUser(getDeleteUser.Username);
                    
                    var name = User.Identity.Name;
                    var user = (from c in entities.UserProfiles
                                where c.Username == name
                                select c).FirstOrDefault();

                    var history = new UserHistory();
                    history.Id = Guid.NewGuid();
                    history.UserId = user.Id;
                    history.BranchId = user.BranchId;
                    history.Date = DateTime.Now;
                    history.Activity = Request.Url.ToString() + "[Username: " + getDeleteUser.Username + "]";

                    entities.UserHistories.AddObject(history);
                    entities.SaveChanges();
                    //home.saveHistory("Nama : " + getDeleteUser.Username + ", Cabang + " + getDeleteUser.Branch.Name);
                    return RedirectToAction("Index");
                        
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Data tidak bisa dihapus");
                }
            }
            model.UserName = getDeleteUser.Username;
            model.Address = getDeleteUser.Address;
            model.BranchName = getDeleteUser.Branch.Name;
            model.StatusId = getDeleteUser.SystemStatusUserProfileId;
            return View(model);
        }
        
        // **************************************
        // URL: /Account/ChangePassword
        // **************************************
        
        
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    var name = User.Identity.Name;
                    var user = (from c in entities.UserProfiles
                                where c.Username == name
                                select c).FirstOrDefault();

                    var history = new UserHistory();
                    history.Id = Guid.NewGuid();
                    history.UserId = user.Id;
                    history.BranchId = user.BranchId;
                    history.Date = DateTime.Now;
                    history.Activity = Request.Url.ToString();

                    entities.UserHistories.AddObject(history);
                    entities.SaveChanges();
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form

            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //[Authorize(Roles="Administrator")]
        //[GridAction]
        //public ActionResult GetListUser()
        //{
        //    var getBranch = baseController.GetCurrentUserBranchId();
        //    var dataUser = (from c in entities.UserProfiles
        //                    where c.BranchId == getBranch.Id
        //                    select new RegisterModel 
        //                    { 
        //                        Id = c.Id,
        //                        UserName = c.Username,
        //                        Address = c.Address,
        //                        BranchName = c.Branch.Name,
        //                        Email = c.Email
        //                    });
        //    return View(new GridModel<RegisterModel>(dataUser));
        //}
        
    }
}
