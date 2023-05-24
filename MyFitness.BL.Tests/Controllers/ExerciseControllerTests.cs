using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFitness.BL.Controllers;
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
            var dataServiceDb = new DatabaseService("MyFitnessTest.db");

            var username = Guid.NewGuid().ToString();
            var activityName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var activityKcals = rnd.NextDouble() * 1000;
            var exStartTime = DateTime.UtcNow.AddMinutes(-60);
            var exEndTime = DateTime.UtcNow;

            var userContrDb = new UserController(username, dataServiceDb);
            userContrDb.CreateUserData(Guid.NewGuid().ToString(), DateTime.Now.AddYears(-20), 180, 70);

            var exContrDb = new ExerciseController(userContrDb.CurrentUser, dataServiceDb);

            var activity = new Activity(activityName, activityKcals);

            // Act
            exContrDb.Add(activity, exStartTime, exEndTime);

            // Assert
            Assert.IsNotNull(dataServiceDb.LoadData<Exercise>()?.SingleOrDefault(
                ex => ex.Start.ToString() == exStartTime.ToString() && ex.User?.Name == username));
            Assert.IsNotNull(dataServiceDb.LoadData<Activity>()?.SingleOrDefault(
                a => a.Name == activityName && a.KilocaloriesPerHour == activityKcals));
        }

        [TestMethod()]
        public void AddSerializationTest()
        {
            // Arrange
            var dataServiceSer = new SerializationService();

            var username = Guid.NewGuid().ToString();
            var activityName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var activityKcals = rnd.NextDouble() * 1000;
            var exStartTime = DateTime.UtcNow.AddMinutes(-60);
            var exEndTime = DateTime.UtcNow;

            var userContrSer = new UserController(username, dataServiceSer);

            var exContrSer = new ExerciseController(userContrSer.CurrentUser, dataServiceSer);

            var activity = new Activity(activityName, activityKcals);

            // Act
            exContrSer.Add(activity, exStartTime, exEndTime);

            // Assert
            Assert.IsNotNull(dataServiceSer.LoadData<Exercise>()?.SingleOrDefault(
                ex => ex.Start.ToString() == exStartTime.ToString() && ex.User?.Name == username));
            Assert.IsNotNull(dataServiceSer.LoadData<Activity>()?.SingleOrDefault(
                a => a.Name == activityName && a.KilocaloriesPerHour == activityKcals));
        }

        [TestMethod()]
        public void DeleteExerciseTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService("MyFitnessTest.db");

            var username = Guid.NewGuid().ToString();
            var activityName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var activityKcals = rnd.NextDouble() * 1000;
            var exStartTime = DateTime.UtcNow.AddMinutes(-60);
            var exEndTime = DateTime.UtcNow;

            var userContrDb = new UserController(username, dataServiceDb);
            userContrDb.CreateUserData(Guid.NewGuid().ToString(), DateTime.Now.AddYears(-20), 180, 70);

            var exContrDb = new ExerciseController(userContrDb.CurrentUser, dataServiceDb);

            var activity = new Activity(activityName, activityKcals);

            // Act
            exContrDb.Add(activity, exStartTime, exEndTime);
            exContrDb.DeleteExercise(exStartTime);

            // Assert
            Assert.IsNotNull(dataServiceDb.LoadData<Exercise>());
            Assert.IsNull(dataServiceDb.LoadData<Exercise>()?.FirstOrDefault(
                ex => ex.Start.ToString() == exStartTime.ToString() && ex.User?.Name == username));
        }

        [TestMethod()]
        public void DeleteActivityTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService("MyFitnessTest.db");

            var username = Guid.NewGuid().ToString();
            var activityName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var activityKcals = rnd.NextDouble() * 1000;
            var exStartTime = DateTime.UtcNow.AddMinutes(-60);

            var userContrDb = new UserController(username, dataServiceDb);
            userContrDb.CreateUserData(Guid.NewGuid().ToString(), DateTime.Now.AddYears(-20), 180, 70);

            var exContrDb = new ExerciseController(userContrDb.CurrentUser, dataServiceDb);

            var activity = new Activity(activityName, activityKcals);

            // Act
            exContrDb.Add(activity, DateTime.UtcNow.AddMinutes(-60), DateTime.UtcNow);

            exContrDb.DeleteActivity(activityName);
            exContrDb.DeleteExercise(exStartTime);
            userContrDb.DeleteCurrentUser();

            // Assert
            Assert.IsNotNull(dataServiceDb.LoadData<Activity>());
            Assert.IsNull(dataServiceDb.LoadData<Activity>()?.FirstOrDefault(
                a => a.Name == activityName && a.KilocaloriesPerHour == activityKcals));
        }
    }
}