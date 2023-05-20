using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using MyFitness.BL.Services;
using MyFitness.BL.Services.Interfaces;
using System.Globalization;
using System.Resources;

namespace MyFitness.View
{
    internal class Program
    {
        private static IDataIOService? _dataService;

        static void Main(string[] args)
        {
            _dataService = new SerializeService();

            var culture = CultureInfo.CurrentCulture;
            var rm = new ResourceManager("MyFitness.View.Languages.Lang", typeof(Program).Assembly);

            Console.WriteLine(rm.GetString("AppStarted", culture));

            Console.Write(rm.GetString("EnterUsername", culture));
            var username = Console.ReadLine();
            
            var userController = new UserController(username, _dataService); 

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
                Console.WriteLine("\n" + rm.GetString("CurrentUser", culture) + userController.CurrentUser);
                Console.WriteLine("\n" + rm.GetString("SelectAction", culture));
                Console.WriteLine(rm.GetString("AddFoodIntake", culture));
                Console.WriteLine(rm.GetString("AddExercise", culture));
                Console.WriteLine(rm.GetString("UserFoodIntakes", culture));
                Console.WriteLine(rm.GetString("UserExercises", culture));
                Console.WriteLine(rm.GetString("AllMeals", culture));
                Console.WriteLine(rm.GetString("AllActivities", culture));
                //Console.WriteLine(rm.GetString("DelCurUser", culture));
                Console.WriteLine(rm.GetString("Exit", culture));
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

                    case ConsoleKey.I:
                        var foodIntakes = _dataService.LoadData<FoodIntake>();
                        _dataService.LoadData<Meal>();
                        _dataService.LoadData<FoodIntakeUnit>();
                        if (foodIntakes is null) continue;
                        foreach (var foodIntake in foodIntakes)
                        {
                            if(foodIntake.User is null) continue;
                            if (foodIntake.User.Id != userController.CurrentUser?.Id) continue; 
                            Console.WriteLine(foodIntake.Moment);
                            if (foodIntake.Meals is null) continue;
                            foreach (var meal in foodIntake.Meals)
                                Console.WriteLine($"\t{meal.Meal?.Name} - {meal.Weight} gr.");
                            Console.WriteLine();
                        }
                        Console.ReadLine();
                        Console.Clear();
                        continue;

                    case ConsoleKey.X:
                        var exercises = _dataService.LoadData<Exercise>();
                        _dataService.LoadData<Activity>();
                        if (exercises is null) continue;
                        foreach (var exercise in exercises)
                        {
                            if (exercise.User is null) continue;
                            if (exercise.User.Id != userController.CurrentUser?.Id) continue;
                            Console.WriteLine($"{exercise.Activity?.Name}: {exercise.Start} - {exercise.Finish}\n");
                        }
                        Console.ReadLine();
                        Console.Clear();
                        continue;

                    case ConsoleKey.M:
                        var meals = _dataService.LoadData<Meal>();
                        if (meals is null) continue;
                        foreach (var meal in meals)
                        {
                            Console.WriteLine($"{meal}" +
                                $"\nCarbohydrates: {meal.Carbohydrates} gr." +
                                $"\nFats: {meal.Fats} gr." +
                                $"\nProteins: {meal.Proteins} gr.\n");
                        }
                        Console.ReadLine();
                        Console.Clear();
                        continue;

                    case ConsoleKey.A:
                        var activities = _dataService.LoadData<Activity>();
                        if (activities is null) continue;
                        foreach (var activity in activities)
                        {
                            Console.WriteLine($"{activity.Name} - {activity.CaloriesPerMinute * 60} kcal. per hour\n");
                        }
                        Console.ReadLine();
                        Console.Clear();
                        continue;

                    case ConsoleKey.D:
                        userController.DeleteCurrentUser();
                        break;

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
            Console.WriteLine("Enter the name of activity: ");
            var activityName = Console.ReadLine();

            using (var exerciseController = new ExerciseController(user, _dataService))
            {
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

                Console.WriteLine();

                foreach (var item in exerciseController.Exercises)
                {
                    #region Item validation

                    if (user?.Name is null) continue;
                    if (item.User?.Name != user.Name) continue;

                    #endregion

                    Console.WriteLine($"\t{item.Activity?.Name}: {item.Start} - {item.Finish}");
                } 
            }

            Console.ReadLine();
            Console.Clear();
        }

        private static void AddFoodIntake(User? user)
        {
            using (var foodIntakeController = new FoodIntakeController(user, DateTime.UtcNow, _dataService))
            {
                while (true)
                {
                    var newMeal = EnterMeal(foodIntakeController);
                    foodIntakeController.Add(newMeal.Meal, newMeal.Weight);

                    if (foodIntakeController.FoodIntake?.Meals is null) break;

                    Console.WriteLine();

                    foreach (var meal in foodIntakeController.FoodIntake.Meals)
                    {
                        Console.WriteLine($"\t{meal.Meal?.Name} - {meal.Weight} gr.");
                    }

                    Console.WriteLine("\nWould you like to add another meal? (Y/N)\n");
                    if (Console.ReadKey().Key != ConsoleKey.Y)
                    {
                        foodIntakeController.Save();
                        Console.Clear();
                        break;
                    }
                    Console.Clear();
                } 
            }
        }

        private static (Meal Meal, double Weight) EnterMeal(FoodIntakeController foodIntakeController)
        {
            Console.Write("Enter meal name: ");
            var mealName = Console.ReadLine();

            Console.Write("Enter meal weight: ");
            var mealWeight = GetParsedValue<double>("meal weight");

            var meal = foodIntakeController.Meals?.SingleOrDefault(m => m.Name == mealName);

            if (meal is null)
            {
                Console.WriteLine("Would you like to enter additional information for a meal? (Y/N)");
                if (Console.ReadKey().Key == ConsoleKey.Y)
                {
                    Console.Write("\nEnter the amount of kilocalories per 100 g. of the meal: ");
                    var calories = GetParsedValue<double>("calories");

                    Console.Write("Enter the amount of proteins per 100 g. of the meal: ");
                    var proteins = GetParsedValue<double>("proteins");

                    Console.Write("Enter the amount of fats per 100 g. of the meal: ");
                    var fats = GetParsedValue<double>("fats");

                    Console.Write("Enter the amount of carbohydrates per 100 g. of the meal: ");
                    var carbohydrates = GetParsedValue<double>("carbohydrates");

                    return (new Meal(mealName, calories, proteins, fats, carbohydrates), mealWeight);
                }
                meal = new Meal(mealName);
            }                

            return (meal, mealWeight);
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