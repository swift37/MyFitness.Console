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
            userContrDb.DeleteCurrentUser();

            // Assert
            Assert.IsNotNull(dataServiceDb.LoadData<FoodIntake>()?.FirstOrDefault(
                fI => fI.Moment.ToString() == foodIntakeMoment.ToString()));
            Assert.IsNotNull(dataServiceDb.LoadData<Meal>()?.FirstOrDefault(
                m => m.Name == mealName && m.Kilocalories == mealKcals));
            Assert.IsNotNull(dataServiceDb.LoadData<FoodIntakeUnit>()?.FirstOrDefault(u => u.Weight == mealWeight));
        }

        [TestMethod()]
        public void AddSerializationTest()
        {
            // Arrange
            var dataServiceSer = new SerializationService();

            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var mealWeight = rnd.NextDouble() * 1000;

            var userContrSer = new UserController(username, dataServiceSer);

            var foodIntakeContrSer = new FoodIntakeController(userContrSer.CurrentUser, DateTime.UtcNow, dataServiceSer);

            var meal = new Meal(mealName, rnd.Next(900), false);

            // Act
            foodIntakeContrSer.Add(meal, mealWeight);

            // Assert
            Assert.AreEqual(meal.Name, foodIntakeContrSer.FoodIntake?.Meals?.First().Meal?.Name);
            Assert.AreEqual(meal.Kilocalories, foodIntakeContrSer.FoodIntake?.Meals?.First().Meal?.Kilocalories);
            Assert.AreEqual(mealWeight, foodIntakeContrSer.FoodIntake?.Meals?.First().Weight);
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
            userContrDb.DeleteCurrentUser();
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
            foodIntakeContrDb.DeleteFoodIntake(foodIntakeMoment);
            foodIntakeContrDb.DeleteMeal(mealName);
            userContrDb.DeleteCurrentUser();

            // Assert
            Assert.IsNotNull(dataServiceDb.LoadData<Meal>());
            Assert.IsNull(dataServiceDb.LoadData<Meal>()?.SingleOrDefault(
                m => m.Name == mealName && m.Kilocalories == mealCalories));
        }
    }
}