using BenNote.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BenNote.Web.Controllers
{
    public class BaseController : Controller
    {
        public IAuthenticationProvider AuthenticationProvider { get; set; }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.AuthenticationProvider.Authorize();
            base.OnAuthorization(filterContext);
        }
    }
}
