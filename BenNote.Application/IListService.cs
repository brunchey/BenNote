using System;
using BenNote.Model;
using System.Collections.Generic;

namespace BenNote.Application
{
    interface IListService
    {
        List Get(List listToShare);
        List Get(int listId);
        void Save(List listToSave);
        void ShareList(List listToShare, IEnumerable<ListSecurity> listSecurity);
    }
}
