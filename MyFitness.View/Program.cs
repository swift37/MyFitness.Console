using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Transactions;

namespace MyFitness.View
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MyFitness was started!");

            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            
            var userController = new UserController(username); 

            if (userController.IsNewUser)
            {
                Console.Write("Enter gender: ");
                var gender = Console.ReadLine();

                Console.Write("Enter date of birth: ");
                var dateOfBirth = GetParsedValue<DateTime>("date of birth");

                Console.Write("Enter weight: ");
                var weight = GetParsedValue<double>("weight");

                Console.Write("Enter height: ");
                var height = GetParsedValue<double>("height");

                userController.CreateUserData(gender, dateOfBirth, weight, height);
            }

            Console.WriteLine("\nCurrent user: {0}", userController.CurrentUser);
            Console.WriteLine("\nSelect an action:");
            Console.WriteLine("F - introduce food intake");
            var key = Console.ReadKey();

            switch (key.Key)
            {
                default:
                    break;
                case ConsoleKey.F:
                    var foodIntakeController = new FoodIntakeController(userController.CurrentUser, DateTime.UtcNow);
                    while (true)
                    {
                        var newMeal = EnterFoodIntake();
                        foodIntakeController.Add(newMeal.Meal, newMeal.Weight);

                        if (foodIntakeController.FoodIntake?.Meals is null) break;

                        foreach (var item in foodIntakeController.FoodIntake.Meals)
                        {
                            Console.WriteLine($"\t{item.Key} - {item.Value} gr.");
                        }

                        Console.WriteLine("Would you like to add another meal? (Y/N)\n");
                        if (Console.ReadKey().Key != ConsoleKey.Y) break;
                    }
                    break;
            }
        }

        private static (Meal Meal, double Weight) EnterFoodIntake()
        {
            Console.Write("\nEnter meal name: ");
            var mealName = Console.ReadLine();

            Console.Write("Enter meal weight: ");
            var mealWeight = GetParsedValue<double>("meal weight");

            Console.WriteLine("Would you like to enter additional information for a meal? (Y/N)");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.Write("\nEnter the amount of calories in the meal:");
                var calories = GetParsedValue<double>("calories");

                Console.Write("Enter the amount of proteins in the meal:");
                var proteins = GetParsedValue<double>("proteins");

                Console.Write("Enter the amount of fats in the meal:");
                var fats = GetParsedValue<double>("fats");

                Console.Write("Enter the amount of carbohydrates in the meal:");
                var carbohydrates = GetParsedValue<double>("carbohydrates");

                return (new Meal(mealName, calories, proteins, fats, carbohydrates), mealWeight);
            }                

            return (new Meal(mealName), mealWeight);
        }

        private static T? GetParsedValue<T>(string paramName)
        {
            while (true)
            {
                var value = Console.ReadLine();
                if (value is null) continue;
                if (TryParse(value, out T? newValue)) return newValue;
                Console.WriteLine($"Incorrect {paramName}. Please, try again:");
            }
        }

        private static bool TryParse<T>(string value, out T? newValue)
        {
            while (true)
            {
                newValue = default;
                try
                {
                    newValue = (T)Convert.ChangeType(value, typeof(T));
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        } 
    }
}