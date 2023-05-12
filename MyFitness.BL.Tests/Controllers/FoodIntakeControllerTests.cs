using MyFitness.BL.Models;

namespace MyFitness.BL.Controllers.Tests
{
    [TestClass()]
    public class FoodIntakeControllerTests
    {
        [TestMethod()]
        public void AddTest()
        {
            // Arrange
            var username = Guid.NewGuid().ToString();
            var mealName = Guid.NewGuid().ToString();
            var rnd = new Random();
            var userContr = new UserController(username);
            var foodIntakeContr = new FoodIntakeController(userContr.CurrentUser, DateTime.UtcNow);
            var meal = new Meal(mealName, rnd.Next(500), rnd.Next(50), rnd.Next(50), rnd.Next(50));

            // Act
            foodIntakeContr.Add(meal, 300);

            // Assert
            Assert.AreEqual(meal.Name, foodIntakeContr.FoodIntake?.Meals?.First().Key.Name);    
        }
    }
}