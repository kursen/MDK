using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SKA.Models;

namespace SKA.Filters
{
    public class BranchCodeFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User == null)
            {
                filterContext.HttpContext.Session["UserBranchCode"] = null;
                filterContext.HttpContext.Session["ThresholdValue"]= null;
            }
            else
            {
                try
                {
                    using (var entities = new SKAEntities())
                    {
                        var branchCode = (from c in entities.UserProfiles
                                          where c.Username == filterContext.HttpContext.User.Identity.Name
                                          select c.Branch.Code).FirstOrDefault();

                        filterContext.HttpContext.Session["UserBranchCode"] = branchCode;

                        string setupValue = "";

                        var getValue = entities.SystemVoucherSetups.Where(a => a.Id == 1).FirstOrDefault();
                        setupValue = Convert.ToString(getValue.Value);
                        if (setupValue.Length <= 9 && setupValue.Length >= 7) //million : 999000000
                        {
                            if (setupValue.Length == 7)
                            {
                                setupValue = setupValue.Substring(0, 1);
                            }
                            else if (setupValue.Length == 8)
                            {
                                setupValue = setupValue.Substring(0, 2);
                            }
                            else if (setupValue.Length == 9)
                            {
                                setupValue = setupValue.Substring(0, 3);
                            }
                            setupValue = setupValue + " Juta";
                        }

                        filterContext.HttpContext.Session["ThresholdValue"] = setupValue;
                    }
                }
                catch (Exception ex)
                {
                    filterContext.HttpContext.Session["UserBranchCode"] = null;
                    filterContext.HttpContext.Session["ThresholdValue"] = null;
                }
            }
        }
    }
}