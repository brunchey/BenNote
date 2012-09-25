using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;


namespace BenNote.Security
{
    public class FormsAuthenticationProvider : IAuthenticationProvider
    {
        private readonly HttpContextBase httpContext;
        private readonly ISitePrincipalRepository sitePrincipalRepository;

        public FormsAuthenticationProvider(HttpContextBase httpContext, ISitePrincipalRepository userRepository)
        {
            this.httpContext = httpContext;
            this.sitePrincipalRepository = userRepository;
        }

        #region public IAuthenticationProvider implementation
        void IAuthenticationProvider.Authorize()
        {
            this.Authorize();
        }

        bool IAuthenticationProvider.Login(string userName, string password)
        {
            return this.Login(userName, password);
        }

        void IAuthenticationProvider.Logout()
        {
            FormsAuthentication.SignOut();
        }

        ISitePrincipal IAuthenticationProvider.CreateAccount(string userName, string password)
        {
            return this.CreateAccount(userName, password);
        }



        #endregion


        #region private IAuthenticationProvider logic
        private void Authorize()
        {
            if (this.httpContext.User != null)
            {
                if (this.httpContext.User.Identity.IsAuthenticated)
                {
                    if (this.httpContext.User.Identity is FormsIdentity)
                    {

                        FormsIdentity id =
                                    (FormsIdentity)this.httpContext.User.Identity;
                        FormsAuthenticationTicket ticket = id.Ticket;

                        // Get the stored user-data, in this case, our roles
                        string userData = ticket.UserData;
                        string[] roles = userData.Split(',');
                        ISitePrincipal principle = new SitePrincipal(ticket.Name, roles);
                        Thread.CurrentPrincipal = principle as IPrincipal;
                        this.httpContext.User = principle as IPrincipal;

                    }
                }
            }
        }

        private bool Login(string userName, string password)
        {
            ISitePrincipal principal = this.sitePrincipalRepository.GetByUserName(userName);

            if (principal == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(password, principal.Password))
                return false;

            string roles = string.Join(",", principal.Roles);


            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                       1, // Ticket version
                       userName, // Username associated with ticket
                       DateTime.Now, // Date/time issued
                       DateTime.Now.AddMinutes(30), // Date/time to expire
                       false, // "true" for a persistent user cookie
                       roles, // User-data, in this case the roles
                       FormsAuthentication.FormsCookiePath);// Path cookie valid for

            // Encrypt the cookie using the machine key for secure transport
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(
               FormsAuthentication.FormsCookieName, // Name of auth cookie
               hash); // Hashed ticket

            // Set the cookie's expiration time to the tickets expiration time
            if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

            // Add the cookie to the list for outgoing response
            this.httpContext.Response.Cookies.Add(cookie);

            return true;
        }

        private void Logout()
        {
            FormsAuthentication.SignOut();
        }

        private ISitePrincipal CreateAccount(string userName, string password)
        {

            string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));

            ISitePrincipal newPrincipal = this.sitePrincipalRepository.CreateUser(userName, encryptedPassword);

            return newPrincipal;

        }


        #endregion

    }
}