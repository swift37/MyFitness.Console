using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using MyFitness.BL.Services;
using System.ComponentModel.DataAnnotations;

namespace MyFitness.BL.Controllers.Tests
{
    [TestClass()]
    public class FoodIntakeControllerTests
    {
        [TestMethod()]
        public void AddDatabaseTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService("MyFitnessTest.db");

            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var mealKcals = rnd.Next(900);
            var mealWeight = rnd.NextDouble() * 1000;
            var foodIntakeMoment = DateTime.UtcNow;

            var userContrDb = new UserController(username, dataServiceDb);
            userContrDb.CreateUserData(Guid.NewGuid().ToString(), DateTime.Now.AddYears(-20), 180, 70);

            var foodIntakeContrDb = new FoodIntakeController(userContrDb.CurrentUser, foodIntakeMoment, dataServiceDb);

            var meal = new Meal(mealName, mealKcals, false);

            // Act
            foodIntakeContrDb.Add(meal, mealWeight);
            foodIntakeContrDb.Save();

            // Assert
            Assert.IsNotNull(dataServiceDb.LoadData<FoodIntake>()?.SingleOrDefault(
                fI => fI.Moment.ToString() == foodIntakeMoment.ToString() && fI.User?.Name == username));
            Assert.IsNotNull(dataServiceDb.LoadData<Meal>()?.SingleOrDefault(
                m => m.Name == mealName && m.Kilocalories == mealKcals));
            Assert.IsNotNull(dataServiceDb.LoadData<FoodIntakeUnit>()?.SingleOrDefault(
                u => u.Weight == mealWeight && u.Meal?.Name == mealName));
        }

        [TestMethod()]
        public void AddSerializationTest()
        {
            // Arrange
            var dataServiceSer = new SerializationService();

            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var mealKcals = rnd.Next(900);
            var mealWeight = rnd.NextDouble() * 1000;
            var foodIntakeMoment = DateTime.UtcNow;

            var userContrSer = new UserController(username, dataServiceSer);

            var foodIntakeContrSer = new FoodIntakeController(userContrSer.CurrentUser, foodIntakeMoment, dataServiceSer);

            var meal = new Meal(mealName, mealKcals, false);

            // Act
            foodIntakeContrSer.Add(meal, mealWeight);
            foodIntakeContrSer.Save();

            // Assert
            Assert.IsNotNull(dataServiceSer.LoadData<FoodIntake>()?.SingleOrDefault(
                fI => fI.Moment.ToString() == foodIntakeMoment.ToString() && fI.User?.Name == username));
            Assert.IsNotNull(dataServiceSer.LoadData<Meal>()?.SingleOrDefault(
                m => m.Name == mealName && m.Kilocalories == mealKcals));
        }

        [TestMethod()]
        public void DeleteFoodIntakeTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService("MyFitnessTest.db");

            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var mealWeight = rnd.NextDouble() * 1000;
            var foodIntakeMoment = DateTime.UtcNow;

            var userContrDb = new UserController(username, dataServiceDb);
            userContrDb.CreateUserData(Guid.NewGuid().ToString(), DateTime.Now.AddYears(-20), 180, 70);

            var foodIntakeContrDb = new FoodIntakeController(userContrDb.CurrentUser, foodIntakeMoment, dataServiceDb);

            var meal = new Meal(mealName, rnd.Next(900), false);

            // Act
            foodIntakeContrDb.Add(meal, mealWeight);
            foodIntakeContrDb.Save();

            foodIntakeContrDb.DeleteFoodIntake(foodIntakeMoment);

            // Assert
            Assert.IsNotNull(dataServiceDb.LoadData<FoodIntake>());
            Assert.IsNull(dataServiceDb.LoadData<FoodIntake>()?.SingleOrDefault(
                fI => fI.Moment.ToString() == foodIntakeMoment.ToString() && fI.User?.Name == username));
        }

        [TestMethod()]
        public void DeleteMealTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService("MyFitnessTest.db");

            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var mealCalories = rnd.Next(900);
            var mealWeight = rnd.NextDouble() * 1000;
            var foodIntakeMoment = DateTime.UtcNow;

            var userContrDb = new UserController(username, dataServiceDb);
            userContrDb.CreateUserData(Guid.NewGuid().ToString(), DateTime.Now.AddYears(-20), 180, 70);

            var foodIntakeContrDb = new FoodIntakeController(userContrDb.CurrentUser, foodIntakeMoment, dataServiceDb);

            var meal = new Meal(mealName, mealCalories, false, rnd.Next(50), rnd.Next(100), rnd.Next(100));

            // Act
            foodIntakeContrDb.Add(meal, mealWeight);
            foodIntakeContrDb.Save();

            foodIntakeContrDb.DeleteMeal(mealName);
            foodIntakeContrDb.DeleteFoodIntake(foodIntakeMoment);
            userContrDb.DeleteCurrentUser();

            // Assert
            Assert.IsNotNull(dataServiceDb.LoadData<Meal>());
            Assert.IsNull(dataServiceDb.LoadData<Meal>()?.SingleOrDefault(
                m => m.Name == mealName && m.Kilocalories == mealCalories));
        }
    }
}