using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BenNote.Security
{
    public interface IAuthenticationProvider
    {
        void Authorize();
        ISitePrincipal CreateAccount(string userName, string password);
        bool Login(string userName, string password);
        void Logout();
    }
}
