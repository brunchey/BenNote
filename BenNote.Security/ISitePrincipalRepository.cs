using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Security
{
    public interface ISitePrincipalRepository
    {
        ISitePrincipal GetByUserName(string userName);
        ISitePrincipal CreateUser(string userName, string password);
    }
}
