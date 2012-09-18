using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Model
{
    public class ListItem
    {
        public int Id { get; set; }
        public string ItemText { get; set; }
        public int OrderBy { get; set; }
        public DateTime Created { get; set; }

        public ListItem()
        {
            this.Created = DateTime.Now;
        }
    }
}
