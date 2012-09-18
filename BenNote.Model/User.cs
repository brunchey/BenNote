using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Model
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public IEnumerable<SiteRoleType> Roles { get; set; }

        public DateTime LastLogin { get; set; }
        public DateTime Created { get; set; }
    }
}
