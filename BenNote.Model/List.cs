using BenNote.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenNote.Model
{
    public class List
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ListVersion> Versions { get; set; }
        public IEnumerable<ListSecurity> Security { get; set; }

        public List()
        {
            this.Versions = new List<ListVersion>();
            this.Security = new List<ListSecurity>();
        }


        //create a new list
        public List(string name, User owner)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Guard.NotNull<User>(() => owner, owner);

            this.Name = name;
            this.Security = new List<ListSecurity>
            {
                new ListSecurity { IsActive = true, Role = ListRoleType.Owner, User= owner, LastUpdated = DateTime.Now }
            };
            this.Versions = new List<ListVersion>
            {
                new ListVersion { CreatedBy = owner }
            };
        }


        /// <summary>
        /// Method used to initially share a list, update who the list is shared with, update how it is shared with other users
        /// </summary>
        /// <param name="listSecurity"></param>
        public void Share(IEnumerable<ListSecurity> listSecurity)
        {
            Guard.NotNull<IEnumerable<ListSecurity>>(() => listSecurity, listSecurity);

            //see if there is an owner specified.  If so, remove it, this is supported through the TransferOwnership method.
            List<ListSecurity> listSecurityUpdates = listSecurity.Where(s => s.Role != ListRoleType.Owner).ToList();

            this.VerifyUniqueUserNamesInSecurityList(listSecurityUpdates);
            this.VerifyExistingOwnerNotChanged(listSecurityUpdates);
            this.UpdateExistingSecurityList(listSecurityUpdates);

            //TODO: Raise event to bus here for sending out notifications about new invites 
            //or updates to existing security.

        }

        
        /// <summary>
        /// Method used to accept a proposed revision.  This will make the revision (an any edits) the current version of the list.
        /// </summary>
        /// <param name="acceptedVersion"></param>
        public void AcceptProposedVersion(ListVersion acceptedVersion)
        {
        }


        /// <summary>
        /// Returns the current version of the list
        /// </summary>
        public ListVersion CurrentVersion 
        { 
            get
            {
                return this.Versions.Where(v => v.State == ListVersionStateType.Current).FirstOrDefault();
            } 
        }

        /// <summary>
        /// Returns the ListItems for the current version of the list
        /// </summary>
        public IList<ListItem> Items
        {
            get
            {
                return this.CurrentVersion.Items;
            }
        }


        /// <summary>
        /// This method will transfer ownership of a list to a new user. And update the security roles of the current owner
        /// </summary>
        /// <param name="proposedOwner"></param>
        /// <param name="proposedListSecurityForCurrentOwner"></param>
        public void TransferOwnership(User proposedOwner, ListSecurity proposedListSecurityForCurrentOwner)
        {

        }

        #region Private Methods
        private void VerifyUniqueUserNamesInSecurityList(List<ListSecurity> listSecurityUpdates)
        {
            //verify that listSecurity contains only unique list of users
            var userGroups =
                from s in listSecurityUpdates
                group s by s.User.UserName into u
                where u.Count() > 1
                select new { UserName = u.Key, Something = u };

            if (userGroups.FirstOrDefault() != null)
                throw new ShareListException(string.Format("The user {0} is specified twice", userGroups.First().UserName));
        }

        private void VerifyExistingOwnerNotChanged(List<ListSecurity> listSecurityUpdates)
        {

            var existingOwner = 
                from s in this.Security
                where s.Role == ListRoleType.Owner
                select s.User;

            var proposedOwnerResult =
                from s in listSecurityUpdates
                where s.User.UserName == existingOwner.FirstOrDefault().UserName
                select s;


            var proposedOwnerChanges = proposedOwnerResult.FirstOrDefault();

            if (proposedOwnerChanges != null)
                if (proposedOwnerChanges.Role != ListRoleType.Owner)
                    throw new ShareListException(string.Format("Cannot change the security role of the existing owner", proposedOwnerChanges.User.UserName));
            
        }

        private void UpdateExistingSecurityList(List<ListSecurity> listSecurityUpdates)
        {
            //add/update security based on UserName
            var CurrentListSecurity = this.Security.ToList<ListSecurity>();

            foreach (var securityItem in listSecurityUpdates)
            {

                var existingItem = CurrentListSecurity.Where(s => s.User.UserName == securityItem.User.UserName).FirstOrDefault();
                if (existingItem == null)
                {
                    CurrentListSecurity.Add(securityItem);
                }
                else
                {
                    existingItem.Role = securityItem.Role;
                    existingItem.IsActive = securityItem.IsActive;
                    existingItem.LastUpdated = DateTime.Now;
                }
            }

            this.Security = CurrentListSecurity;

        }
        #endregion

    }
}
