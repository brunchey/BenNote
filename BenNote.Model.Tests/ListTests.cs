using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using BenNote.Model.Exceptions;
using System.Linq;

namespace BenNote.Model.Tests
{
    [TestClass]
    public class ListTests
    {

        #region Create methods

        [TestMethod]
        public void CreateList_Successfully()
        {
            var owner = new User() { UserName = "Ben" };
            string listName = "Bens List";

            var list = new List(listName, owner);
            Assert.IsNotNull(list);
            Assert.AreEqual(list.Name, listName);
            Assert.IsTrue(list.Security.ToList<ListSecurity>().Count() == 1);
            Assert.IsTrue(list.Security.Where(s => s.Role == ListRoleType.Owner).Count() == 1);
            Assert.AreSame(list.Security.Where(s => s.Role == ListRoleType.Owner).First().User, owner);
            Assert.IsNotNull(list.CurrentVersion);
            Assert.AreSame(list.CurrentVersion.CreatedBy, owner);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateList_WithNullName_Throws_ArgumentNullException()
        {
            var owner = new User() { UserName = "Ben" };
            var list = new List(null, owner);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateList_WithNullOwner_Throws_ArgumentNullException()
        {
            var owner = new User() { UserName = "Ben" };
            var list = new List(null, owner);
        }

        #endregion

        #region ShareList methods
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShareList_NullListSecurityParam_Throws_ArgumentNullException()
        {
            var list = new List();
            list.Share(null); 
        }


        [TestMethod]
        [ExpectedException(typeof(ShareListException))]
        public void ShareList_DuplicateUserInList_Throws_UnableToShareListException()
        {
            //arrange
            var existingListSecurity = new List<ListSecurity>
            {
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Samuel"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Owner, User = new User() { UserName = "Jennifer"}},
            };

            var proposedListSecurity = new List<ListSecurity>
            {
                new ListSecurity() { IsActive = true, Role = ListRoleType.Contributor, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Solomon"}},
            };

            //act
            List testList = new List();
            testList.Security = existingListSecurity;
            testList.Share(proposedListSecurity);

        }


        [TestMethod]
        [ExpectedException(typeof(ShareListException))]
        public void ShareList_ChangeToExistingOwner_Throws_UnableToShareListException()
        {
            //arrange
            var existingListSecurity = new List<ListSecurity>
            {
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Samuel"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Owner, User = new User() { UserName = "Jennifer"}},
            };

            var addToListSecurity = new List<ListSecurity>
            {
                new ListSecurity() { IsActive = false, Role = ListRoleType.Viewer, User = new User() { UserName = "Samuel"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Contributor, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Solomon"}},
                new ListSecurity() { IsActive = false, Role = ListRoleType.Contributor, User = new User() { UserName = "Jennifer"}},
            };

            List testList = new BenNote.Model.List();
            testList.Security = existingListSecurity;

            //act
            testList.Share(addToListSecurity);
        }


        [TestMethod]
        public void ShareList_VerifyOwnerRoleIsRemovedFromProposedList()
        {
            //arrange
            var existingListSecurity = new List<ListSecurity>
            {
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Samuel"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Owner, User = new User() { UserName = "Jennifer"}},
            };

            var addToListSecurity = new List<ListSecurity>
            {
                new ListSecurity() { IsActive = false, Role = ListRoleType.Viewer, User = new User() { UserName = "Samuel"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Contributor, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Owner, User = new User() { UserName = "Solomon"}},
            };

            List testList = new BenNote.Model.List();
            testList.Security = existingListSecurity;

            //act
            testList.Share(addToListSecurity);

            //Assert
            Assert.IsTrue(testList.Security.ToList<ListSecurity>().Count() == 3, "");
            Assert.IsTrue(testList.Security.ToList<ListSecurity>().Where(s => s.User.UserName == "Solomon").FirstOrDefault() == null, "");
        }



        [TestMethod]
        public void ShareList_SecurityListUpdated_Successfully()
        {
            //arrange
            var existingListSecurity = new List<ListSecurity>
            {
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Samuel"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Owner, User = new User() { UserName = "Jennifer"}},
            };

            var addToListSecurity = new List<ListSecurity>
            {
                new ListSecurity() { IsActive = false, Role = ListRoleType.Viewer, User = new User() { UserName = "Samuel"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Contributor, User = new User() { UserName = "Ben"}},
                new ListSecurity() { IsActive = true, Role = ListRoleType.Viewer, User = new User() { UserName = "Solomon"}},
                new ListSecurity() { IsActive = false, Role = ListRoleType.Owner, User = new User() { UserName = "Jennifer"}},
            };

            List testList = new BenNote.Model.List();
            testList.Security = existingListSecurity;

            //act
            testList.Share(addToListSecurity);

            //Assert
            Assert.IsTrue(testList.Security.ToList<ListSecurity>().Count() == 4, "");
            Assert.IsTrue(testList.Security.ToList<ListSecurity>().Where(s => s.User.UserName == "Samuel").FirstOrDefault().IsActive == false, "");

        }
        #endregion

        

    }
}
