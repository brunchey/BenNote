using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BenNote.Model.Tests
{
    [TestClass]
    public class ListVersionTests
    {
        #region Modify ListItems
        [TestMethod]
        public void ListItems_AddNew()
        {
            var owner = new User() { UserName = "Ben" };
            var list = new List("Ben List", owner);

            var listItems = list.CurrentVersion.Items;
            listItems.Add(new ListItem { ItemText = "First Item", OrderBy = 1 });
            listItems.Add(new ListItem { ItemText = "Second Item", OrderBy = 2 });
            listItems.Add(new ListItem { ItemText = "Third Item", OrderBy = 3 });

            Assert.IsTrue(list.CurrentVersion.Items.Count == 3);

        }

        #endregion
    }
}
