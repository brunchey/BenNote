using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BenNote.Security
{
    public class SiteIdentity : IIdentity
    {
        private bool isAuthenticated;
        private string userName;

        public SiteIdentity(bool isAuthenticated, string userName)
        {
            this.isAuthenticated = isAuthenticated;
            this.userName = userName;
        }

        public string AuthenticationType
        {
            get { return "FormsAuthentication"; }
        }

        public bool IsAuthenticated
        {
            get
            {
                return this.isAuthenticated;
            }
        }

        public string Name
        {
            get { return this.userName; }
        }
    }
}