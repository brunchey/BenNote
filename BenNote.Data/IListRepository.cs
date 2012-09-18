using BenNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Data
{
    public interface IListRepository
    {
        void Save(List list);
        List Get(List list);
        List Get(int listId);
        List GetAll(User user);
        ListVersion GetVersion(ListVersion listVersion);
            
    }
}
