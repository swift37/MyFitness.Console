using MyFitness.BL.Services;

namespace MyFitness.BL.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        [TestMethod()]
        public void CreateUserDataTest()
        {
            // Arrange
            var dataService = new DatabaseService();
            var username = Guid.NewGuid().ToString();
            var gender = "testtest";
            var dateOfBirth = DateTime.Now.AddYears(-20);
            var weight = 70;
            var height = 180;
            var contr = new UserController(username, dataService);

            // Act
            contr.CreateUserData(gender, dateOfBirth, weight, height);
            var contr2 = new UserController(username, dataService);

            // Assert
            Assert.AreEqual(username, contr2.CurrentUser?.Name);
            Assert.AreEqual(gender, contr2.CurrentUser?.Gender?.Name);
            Assert.AreEqual(dateOfBirth.ToString(), contr2.CurrentUser?.DateOfBirth.ToString());
            Assert.AreEqual(weight, contr2.CurrentUser?.Weight);
            Assert.AreEqual(height, contr2.CurrentUser?.Height);
            Assert.AreEqual(contr.CurrentUser?.Age, contr2.CurrentUser?.Age);
        }

        [TestMethod()]
        public void SaveUsersTest()
        {
            // Arrange
            var dataService = new DatabaseService();
            var username = Guid.NewGuid().ToString();

            // Act
            var contr = new UserController(username, dataService);

            // Assert
            Assert.AreEqual(username, contr.CurrentUser?.Name);
            Assert.AreEqual(true, contr.IsNewUser);
        }
    }
}