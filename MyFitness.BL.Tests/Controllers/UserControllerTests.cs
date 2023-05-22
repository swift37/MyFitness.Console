using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFitness.BL.Controllers;
using MyFitness.BL.Services;

namespace MyFitness.BL.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [Priority(0)]
        [TestMethod()]
        public void InitializationNewUserTest()
        {
            // Arrange
            var dataService = new DatabaseService();
            var username = Guid.NewGuid().ToString();

            // Act
            var userContrDb = new UserController(username, dataService);

            // Assert
            Assert.AreEqual(username, userContrDb.CurrentUser?.Name);
            Assert.AreEqual(true, userContrDb.IsNewUser);
        }

        [Priority(1)]
        [TestMethod()]
        public void CreateUserDataDatabaseTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService();

            var username = "5D098A9C-EBCE-4FD5-B0AE-8162133E6578";
            var gender = "testGender";
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
            var gender = "testGender";
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

        [Priority(2)]
        [TestMethod()]
        public void DeleteCurrentUserTest()
        {
            // Arrange
            var dataService = new DatabaseService();
            var username = "5D098A9C-EBCE-4FD5-B0AE-8162133E6578";
            var tempUsername = "temp";

            // Act
            var userContrDb = new UserController(username, dataService);
            userContrDb.DeleteCurrentUser();

            var userContrDb2 = new UserController(tempUsername, dataService);

            // Assert
            Assert.IsNull(userContrDb2.Users?.SingleOrDefault(user => user.Name == "5D098A9C-EBCE-4FD5-B0AE-8162133E6578"));
        }
    }
}