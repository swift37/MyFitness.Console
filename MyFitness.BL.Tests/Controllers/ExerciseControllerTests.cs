using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using MyFitness.BL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.BL.Controllers.Tests
{
    [TestClass()]
    public class ExerciseControllerTests
    {
        [TestMethod()]
        public void AddTest()
        {
            // Arrange
            var dataService = new DatabaseService();
            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var userContr = new UserController(username, dataService);
            var exContr = new ExerciseController(userContr.CurrentUser, dataService);
            var activity = new Activity("Runing", 7);

            // Act
            exContr.Add(activity, DateTime.Now.AddMinutes(-60), DateTime.Now);

            // Assert
            Assert.AreEqual(activity.Name, exContr.Exercises?.First().Activity?.Name);
        }
    }
}