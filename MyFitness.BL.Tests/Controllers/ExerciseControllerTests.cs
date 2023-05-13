using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
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
            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var userContr = new UserController(username);
            var exContr = new ExerciseController(userContr.CurrentUser);
            var activity = new Activity("Runing", 7);

            // Act
            exContr.Add(activity, DateTime.Now.AddMinutes(-60), DateTime.Now);

            // Assert
            Assert.AreEqual(activity.Name, exContr.Exercises?.First().Activity?.Name);
        }
    }
}