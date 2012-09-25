using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenNote.Model;
using BenNote.Security;

namespace BenNote.Data.EF
{
    public class UserRepository : IUserRepository, ISitePrincipalRepository
    {

        void IUserRepository.Save(User user)
        {
            throw new NotImplementedException();
        }

        void IUserRepository.Get(string userName)
        {
            throw new NotImplementedException();
        }

        ISitePrincipal ISitePrincipalRepository.GetByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        ISitePrincipal ISitePrincipalRepository.CreateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
