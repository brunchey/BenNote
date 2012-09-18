using BenNote.Data;
using BenNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Application
{
    public class ListService : IListService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IListRepository listRepository;
        private readonly User currentUser;

        public ListService(IUnitOfWork unitOfWork, IListRepository listRepository, User currentUser)
        {
            Guard.NotNull<IUnitOfWork>(() => unitOfWork, unitOfWork);
            Guard.NotNull<IListRepository>(() => listRepository, listRepository);
            Guard.NotNull<User>(() => currentUser, currentUser);

            this.unitOfWork = unitOfWork;
            this.listRepository = listRepository;
            this.currentUser = currentUser;
        }

        #region IListServiceImplementation
        List IListService.Get(List listToShare)
        {
            return this.Get(listToShare);
        }

        List IListService.Get(int listId)
        {
            return this.Get(listId);
        }

        void IListService.Save(List listToSave)
        {
            this.Save(listToSave);
        }

        void IListService.ShareList(List listToShare, IEnumerable<ListSecurity> listSecurity)
        {
            this.ShareList(listToShare, listSecurity);
        }

        #endregion

        #region Private implementation
        private List Get(List listToShare)
        {
            Guard.NotNull<List>(() => listToShare, listToShare);
            return this.Get(listToShare.Id);
        }

        private List Get(int listId)
        {
            return this.listRepository.Get(listId);
        }

        private void Save(List listToSave)
        {
            if (listToSave.Id > 0)
                UpdateExistingList(listToSave);
            else
                SaveNewList(listToSave);

        }

        private void ShareList(List listToShare, IEnumerable<ListSecurity> listSecurity)
        {
            Guard.NotNull<List>(() => listToShare, listToShare);
            Guard.NotNull<IEnumerable<ListSecurity>>(() => listSecurity, listSecurity);

            var list = this.listRepository.Get(listToShare.Id);

            list.Share(listSecurity);

            try
            {
                this.listRepository.Save(list);
            }
            catch (Exception ex)
            {
                throw;
            }

            //TODO: Raise event to bus here for sending out notifications about new invites 
            //or updates to existing security.

        }
        #endregion

        #region Private methods
        private void UpdateExistingList(List listToSave)
        {
            var existingList = this.listRepository.Get(listToSave);

            existingList.CurrentVersion.Items = listToSave.Items;

            //todo use fluent validations here instead.
            existingList.CurrentVersion.ValidateItems();

            try
            {
                this.listRepository.Save(existingList);
            }
            catch (Exception ex)
            {
                throw;
            }

            //TODO: Raise event to bus here for sending out notifications about new invites 
            //or updates to existing security.

        }

        private void SaveNewList(List listToSave)
        {
            var newList = new List(listToSave.Name, this.currentUser);

            newList.CurrentVersion.Items = listToSave.Items;

            //TODO use fluent validation here.
            newList.CurrentVersion.ValidateItems();

            try
            {
                this.listRepository.Save(newList);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        #endregion




    }
}
