using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Model
{
    public class ListSecurity
    {
        public int Id { get; set; }
        public User User { get; set; }
        public ListRoleType Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdated { get; set; }

        public ListSecurity()
        {
            IsActive = true;
        }

    }
}
