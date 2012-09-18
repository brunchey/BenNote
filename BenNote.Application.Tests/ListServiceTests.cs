using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BenNote.Data;
using BenNote.Model;

namespace BenNote.Application.Tests
{
    [TestClass]
    public class ListServiceTests
    {
        

        [TestMethod]
        public void Ctor_Success()
        {
            var unitOfWork = new Mock<IUnitOfWork>();
            var listRepo = new Mock<IListRepository>();
            var user = new User();

            var listService = new ListService(unitOfWork.Object, listRepo.Object, user);

            Assert.IsNotNull(listService);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullUnitOfWork_ThrowsArgumentNullException()
        {
            IUnitOfWork unitOfWork = null;
            var listRepo = new Mock<IListRepository>();
            var user = new User();

            var listService = new ListService(unitOfWork, listRepo.Object, user);

        }
    }
}
