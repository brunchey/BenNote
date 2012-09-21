using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Security
{
    public interface ISitePrincipal : IPrincipal
    {
        string Username
        {
            get;
        }

        string[] Roles
        {
            get;
        }
    }
}
