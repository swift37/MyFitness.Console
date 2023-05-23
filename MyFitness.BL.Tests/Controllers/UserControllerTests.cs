using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using MyFitness.BL.Services;

namespace MyFitness.BL.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        public void InitializationNewUserTest()
        {
            // Arrange
            var dataService = new DatabaseService("MyFitnessTest.db");
            var username = Guid.NewGuid().ToString();

            // Act
            var userContrDb = new UserController(username, dataService);

            // Assert
            Assert.AreEqual(username, userContrDb.CurrentUser?.Name);
            Assert.AreEqual(true, userContrDb.IsNewUser);
        }

        [TestMethod()]
        public void CreateUserDataDatabaseTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService("MyFitnessTest.db");

            var username = Guid.NewGuid().ToString();
            var gender = Guid.NewGuid().ToString();
            var dateOfBirth = DateTime.Now.AddYears(-20);
            var weight = 70;
            var height = 180;

            var userContrDb = new UserController(username, dataServiceDb);

            // Act
            userContrDb.CreateUserData(gender, dateOfBirth, weight, height);
            var userContrDb2 = new UserController(username, dataServiceDb);

            // Assert
            Assert.AreEqual(username, userContrDb2.CurrentUser?.Name);
            Assert.AreEqual(gender, userContrDb2.CurrentUser?.Gender?.Name);
            Assert.AreEqual(dateOfBirth.ToString(), userContrDb2.CurrentUser?.DateOfBirth.ToString());
            Assert.AreEqual(weight, userContrDb2.CurrentUser?.Weight);
            Assert.AreEqual(height, userContrDb2.CurrentUser?.Height);
            Assert.AreEqual(userContrDb.CurrentUser?.Age, userContrDb2.CurrentUser?.Age);
        }

        [TestMethod()]
        public void CreateUserDataSerializationTest()
        {
            // Arrange
            var dataServiceSer = new SerializationService();

            var username = Guid.NewGuid().ToString();
            var gender = Guid.NewGuid().ToString();
            var dateOfBirth = DateTime.Now.AddYears(-20);
            var weight = 70;
            var height = 180;

            var userContrSer = new UserController(username, dataServiceSer);

            // Act
            userContrSer.CreateUserData(gender, dateOfBirth, weight, height);
            var userContrSer2 = new UserController(username, dataServiceSer);

            // Assert
            Assert.AreEqual(username, userContrSer2.CurrentUser?.Name);
            Assert.AreEqual(gender, userContrSer2.CurrentUser?.Gender?.Name);
            Assert.AreEqual(dateOfBirth.ToString(), userContrSer2.CurrentUser?.DateOfBirth.ToString());
            Assert.AreEqual(weight, userContrSer2.CurrentUser?.Weight);
            Assert.AreEqual(height, userContrSer2.CurrentUser?.Height);
            Assert.AreEqual(userContrSer.CurrentUser?.Age, userContrSer2.CurrentUser?.Age);
        }

        [TestMethod()]
        public void DeleteCurrentUserTest()
        {
            // Arrange
            var dataService = new DatabaseService("MyFitnessTest.db");
            var username = Guid.NewGuid().ToString();
            var gender = Guid.NewGuid().ToString();
            var dateOfBirth = DateTime.Now.AddYears(-20);
            var weight = 70;
            var height = 180;

            var userContrDb = new UserController(username, dataService);

            // Act
            userContrDb.CreateUserData(gender, dateOfBirth, weight, height);
            userContrDb.DeleteCurrentUser();

            // Assert
            Assert.IsNotNull(dataService.LoadData<User>());
            Assert.IsNull(dataService.LoadData<User>()?.SingleOrDefault(user => user.Name == username));
        }
    }
}