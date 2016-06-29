using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Utils;
using System.Web.Security;
using SKA.Models;
using SKA.Areas.Setting.Models.ViewModels;

namespace SKA.Controllers
{
    [HandleError]
    [Authorize]

    public class BaseController : Controller
    {
        private ILogger logger;
        private SKAEntities entities;
        public BaseController()
        {
            entities = new SKAEntities();
            logger = new FileLogger();
        }

        public BaseController(ILogger log)
        {
            logger = log ?? new FileLogger();
        }

        public ILogger Logger
        {
            get
            {
                return logger;
            }
        }

        
        public MembershipUser CurrentUser
        {
            get
            {
                return Membership.GetUser(User.Identity.Name);
            }
        }

        public void SetErrorMessageViewData(string message)
        {
            ViewData["ErrorMessage"] = message;
        }

        
        public BranchViewModel GetCurrentUserBranchId()
        {
            using (var entities = new SKAEntities())
            {
                var model = (from c in entities.UserProfiles
                            where c.Username == User.Identity.Name
                             select new BranchViewModel
                            {
                                Id = c.BranchId,
                                Name = c.Branch.Name,
                                Code = c.Branch.Code,
                                ShortName = c.Branch.ShortName,
                                Status = c.SystemStatusUserProfile.Status,
                                StatusId = c.SystemStatusUserProfileId
                            }).FirstOrDefault();
                
                return model;
            };
        }

        public Guid GetCurrentUserId()
        {
            var username = User.Identity.Name;
            var userid = (from a in entities.UserProfiles
                          where a.Username == username
                          select a).FirstOrDefault();

            return userid.Id;
        }

        public void saveHistory(string number = "")
        {

            var id = GetCurrentUserId();
            var userbranch = GetCurrentUserBranchId();
            var newHistory = new UserHistory();
            newHistory.Id = Guid.NewGuid();
            newHistory.UserId = id;
            newHistory.BranchId = userbranch.Id;
            newHistory.Activity = (number != "") ? Request.Url.ToString() + " [Number: " + number + "]" : Request.Url.ToString();
            newHistory.Date = DateTime.Now;

            entities.UserHistories.AddObject(newHistory);
            entities.SaveChanges();
        }
    }
}
