using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using System.Net;

namespace SKA.Helpers
{
    public class ReportCredential:IReportServerCredentials
    {
        private string _userName;
        private string _password;
        private string _domain;

        public Uri ReportServerUrl { get; set; }

        public ReportCredential(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }

        public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string password, out string authority)
        {
            userName = _userName;
            password = _password;
            authority = _domain;
            authCookie = new System.Net.Cookie(".ASPXAUTH", ".ASPXAUTH", "/", "Domain");

            return true;
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {
            get { return null; }
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get { return new NetworkCredential(_userName, _password, _domain); }
        }
    }
}