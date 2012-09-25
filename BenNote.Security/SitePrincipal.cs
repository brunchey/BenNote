using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BenNote.Security
{
    public class SitePrincipal : ISitePrincipal
    {
        public SitePrincipal(string userName, string password,  string[] roles)
        {
            this.Identity = new SiteIdentity(true, userName);
            this.Username = userName;
            this.Roles = roles;
            this.Password = password;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            if ((Roles == null)  || (this.Roles.Count() == 0))
                return false;

            foreach (string roleItem in this.Roles)
	        {
                if (string.Equals(roleItem, role, StringComparison.OrdinalIgnoreCase))
                    return true;
	        }

            return false;
            
        }

        public string Username
        {
            get;
            private set;
        }

        public string Password
        {
            get;
            private set;
        }

        public string[] Roles
        {
            get;
            private set;
        }
    }
}