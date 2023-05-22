using MyFitness.BL.Models;
using MyFitness.BL.Services;

namespace MyFitness.BL.Controllers.Tests
{
    [TestClass()]
    public class ExerciseControllerTests
    {
        [TestMethod()]
        public void AddDatabaseTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService();

            var username = Guid.NewGuid().ToString();
            var activityName = Guid.NewGuid().ToString();
            var activityKcals = 799.81;
            var rnd = new Random();

            var userContrDb = new UserController(username, dataServiceDb);
            userContrDb.CreateUserData("test", DateTime.Now.AddYears(-20), 180, 70);

            var exContrDb = new ExerciseController(userContrDb.CurrentUser, dataServiceDb);

            var activity = new Activity("TestActivity", activityKcals);

            // Act
            exContrDb.Add(activity, DateTime.Now.AddMinutes(-61), DateTime.Now.AddMinutes(-1));

            // Assert
            Assert.IsNotNull(exContrDb.Exercises?.FirstOrDefault(a => a.Activity?.Name == activity.Name));
        }

        [TestMethod()]
        public void AddSerializationTest()
        {
            // Arrange
            var dataServiceSer = new SerializationService();

            var username = Guid.NewGuid().ToString();
            var activityName = Guid.NewGuid().ToString();
            var activityKcals = 799.81;
            var rnd = new Random();

            var userContrSer = new UserController(username, dataServiceSer);

            var exContrSer = new ExerciseController(userContrSer.CurrentUser, dataServiceSer);

            var activity = new Activity("Runing", activityKcals);

            // Act
            exContrSer.Add(activity, DateTime.Now.AddMinutes(-60), DateTime.Now);

            // Assert
            Assert.AreEqual(activity.Name, exContrSer.Exercises?.First().Activity?.Name);
        }
    }
}