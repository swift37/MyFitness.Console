using MyFitness.BL.Models;
using MyFitness.BL.Services;

namespace MyFitness.BL.Controllers.Tests
{
    [TestClass()]
    public class FoodIntakeControllerTests
    {
        [TestMethod()]
        public void AddDatabaseTest()
        {
            // Arrange
            var dataServiceDb = new DatabaseService();

            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var mealWeight = 299.81;
            var rnd = new Random();

            var userContrDb = new UserController(username, dataServiceDb);

            var foodIntakeContrDb = new FoodIntakeController(userContrDb.CurrentUser, DateTime.UtcNow, dataServiceDb);

            var meal = new Meal(mealName, rnd.Next(500), false, rnd.Next(50), rnd.Next(50), rnd.Next(50));

            // Act
            foodIntakeContrDb.Add(meal, mealWeight);

            // Assert
            Assert.AreEqual(meal.Name, foodIntakeContrDb.FoodIntake?.Meals?.First().Meal?.Name);    
            Assert.AreEqual(meal.Kilocalories, foodIntakeContrDb.FoodIntake?.Meals?.First().Meal?.Kilocalories);    
            Assert.AreEqual(mealWeight, foodIntakeContrDb.FoodIntake?.Meals?.First().Weight);
        }

        [TestMethod()]
        public void AddSerializationTest()
        {
            // Arrange
            var dataServiceSer = new SerializationService();

            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var mealWeight = 299.81;
            var rnd = new Random();

            var userContrSer = new UserController(username, dataServiceSer);

            var foodIntakeContrSer = new FoodIntakeController(userContrSer.CurrentUser, DateTime.UtcNow, dataServiceSer);

            var meal = new Meal(mealName, rnd.Next(500), false, rnd.Next(50), rnd.Next(50), rnd.Next(50));

            // Act
            foodIntakeContrSer.Add(meal, mealWeight);

            // Assert
            Assert.AreEqual(meal.Name, foodIntakeContrSer.FoodIntake?.Meals?.First().Meal?.Name);
            Assert.AreEqual(meal.Kilocalories, foodIntakeContrSer.FoodIntake?.Meals?.First().Meal?.Kilocalories);
            Assert.AreEqual(mealWeight, foodIntakeContrSer.FoodIntake?.Meals?.First().Weight);
        }
    }
}