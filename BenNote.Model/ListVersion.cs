using BenNote.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Model
{
    public class ListVersion
    {
        public int Id { get; set; }
        public ListVersionStateType State { get; set; }
        public IList<ListItem> Items { get; set; }

        public DateTime Created { get; set; }
        public User CreatedBy { get; set; }


        public ListVersion()
        {
            this.Created = DateTime.Now;
            this.Items = new List<ListItem>();
        }

        public void ValidateItems()
        {
            //verify that no two items have the same orderby number.  
            var withoutOrderBy =
                from li in this.Items
                where li.OrderBy == 0
                select li;

            if (withoutOrderBy.FirstOrDefault() != null)
                throw new InvalidListVersionException("Items list is missing OrderBy property");

        }


    }
}
