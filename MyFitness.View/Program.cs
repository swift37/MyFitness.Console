using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using MyFitness.View.Languages;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Transactions;

namespace MyFitness.View
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var culture = CultureInfo.CurrentCulture;
            var rm = new ResourceManager("MyFitness.View.Languages.Lang", typeof(Program).Assembly);

            Console.WriteLine(rm.GetString("AppStarted", culture));

            Console.Write(rm.GetString("EnterUsername", culture));
            var username = Console.ReadLine();
            
            var userController = new UserController(username); 

            if (userController.IsNewUser)
            {
                Console.Write(rm.GetString("EnterGender", culture));
                var gender = Console.ReadLine();

                Console.Write(rm.GetString("EnterDateOfBirth", culture));
                var dateOfBirth = GetParsedValue<DateTime>("date of birth");

                Console.Write(rm.GetString("EnterWeight", culture));
                var weight = GetParsedValue<double>("weight");

                Console.Write(rm.GetString("EnterHeight", culture));
                var height = GetParsedValue<double>("height");

                userController.CreateUserData(gender, dateOfBirth, weight, height);

                Console.Clear();
            }

            while (true)
            {
                Console.WriteLine("\nCurrent user: {0}", userController.CurrentUser);
                Console.WriteLine("\nSelect an action:");
                Console.WriteLine("F - add food intake");
                Console.WriteLine("E - add exercise");
                Console.WriteLine("Q - exit");
                var key = Console.ReadKey();
                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.F:
                        AddFoodIntake(userController.CurrentUser);
                        continue;

                    case ConsoleKey.E:
                        AddExercise(userController.CurrentUser);
                        continue;

                    case ConsoleKey.Q:
                        break;

                    default:
                        continue;
                }
                break;
            }
        }

        private static void AddExercise(User? user)
        {
            var exerciseController = new ExerciseController(user);

            Console.WriteLine("\nEnter the name of activity: ");
            var activityName = Console.ReadLine();

            var activity = exerciseController.Activities?.SingleOrDefault(a => a.Name == activityName);

            if (activity is null)
            {
                Console.WriteLine("Enter the number of calories burned per minute: ");
                var caloriesPerMinute = GetParsedValue<double>("calories per minute");

                activity = new Activity(activityName, caloriesPerMinute);
            }

            Console.WriteLine("Enter the start time of the exercise: ");
            var start = GetParsedValue<DateTime>("start time of the exercise");
            Console.WriteLine("Enter the end time of the exercise: ");
            var finish = GetParsedValue<DateTime>("end time of the exercise");

            exerciseController.Add(activity, start, finish);

            if (exerciseController.Exercises is null) throw new ArgumentNullException(nameof(exerciseController.Exercises));

            foreach (var item in exerciseController.Exercises)
            {
                #region Item validation

                if (user?.Name is null) continue;
                if (item.User?.Name != user.Name) continue; 

                #endregion

                Console.WriteLine($"\t{item.Activity?.Name}: {item.Start} - {item.Finish}");
            }
        }

        private static void AddFoodIntake(User? user)
        {
            var foodIntakeController = new FoodIntakeController(user, DateTime.UtcNow);
            while (true)
            {
                var newMeal = EnterMeal();
                foodIntakeController.Add(newMeal.Meal, newMeal.Weight);

                if (foodIntakeController.FoodIntake?.Meals is null) break;

                foreach (var item in foodIntakeController.FoodIntake.Meals)
                {
                    Console.WriteLine($"\t{item.Key} - {item.Value} gr.");
                }

                Console.WriteLine("Would you like to add another meal? (Y/N)\n");
                if (Console.ReadKey().Key != ConsoleKey.Y) break;
            }
        }

        private static (Meal Meal, double Weight) EnterMeal()
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