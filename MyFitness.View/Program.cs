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
        private static CultureInfo? _culture;
        private static ResourceManager? _rm;

        static void Main(string[] args)
        {
            _dataService = new SerializationService();
            _culture = CultureInfo.CurrentCulture;
            _rm = new ResourceManager("MyFitness.View.Languages.Lang", typeof(Program).Assembly);

            Console.WriteLine(_rm.GetString("AppStarted", _culture));

            Console.Write(_rm.GetString("EnterUsername", _culture));
            var username = Console.ReadLine();
            
            var userController = new UserController(username, _dataService); 

            if (userController.IsNewUser)
            {
                Console.Write(_rm.GetString("EnterGender", _culture));
                var gender = Console.ReadLine();

                Console.Write(_rm.GetString("EnterDateOfBirth", _culture));
                var dateOfBirth = GetParsedLimitedDateTimeValue(
                    _rm.GetString("dateOfBirth", _culture) ?? "date of birth", 
                    DateTime.Parse("01.01.1900"), 
                    DateTime.Now.AddYears(-12));

                Console.Write(_rm.GetString("EnterWeight", _culture));
                var weight = GetParsedLimitedDoubleValue(_rm.GetString("weight", _culture) ?? "weight", 30, 250);

                Console.Write(_rm.GetString("EnterHeight", _culture));
                var height = GetParsedLimitedDoubleValue(_rm.GetString("height", _culture) ?? "height", 100, 250);

                userController.CreateUserData(gender, dateOfBirth, weight, height);

                Console.Clear();
            }

            while (true)
            {
                Console.WriteLine("\n" + _rm?.GetString("CurrentUser", _culture) + userController.CurrentUser);
                Console.WriteLine("\n" + _rm?.GetString("SelectAction", _culture));
                Console.WriteLine(_rm?.GetString("AddFoodIntake", _culture));
                Console.WriteLine(_rm?.GetString("AddExercise", _culture));
                Console.WriteLine(_rm?.GetString("UserFoodIntakes", _culture));
                Console.WriteLine(_rm?.GetString("UserExercises", _culture));
                Console.WriteLine(_rm?.GetString("AllMeals", _culture));
                Console.WriteLine(_rm?.GetString("AllActivities", _culture));
                Console.WriteLine(_rm?.GetString("DelCurUser", _culture));
                Console.WriteLine(_rm?.GetString("Exit", _culture));
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
                            Console.WriteLine($"{foodIntake.Moment}" +
                                _rm?.GetString("TotalKcalFoodIntake", _culture) + $"{foodIntake.TotalKilocalories}");
                            if (foodIntake.Meals is null) continue;
                            foreach (var FoodIntakeUnit in foodIntake.Meals)
                            {
                                if (FoodIntakeUnit.Meal is null) continue;
                                var units = FoodIntakeUnit.Meal.IsLiquid ? 
                                    _rm?.GetString("ml", _culture) : _rm?.GetString("gr", _culture);
                                Console.WriteLine($"\t{FoodIntakeUnit.Meal?.Name} - {FoodIntakeUnit.Weight} {units}, {foodIntake.TotalKilocalories}");
                            }    
                        }
                        Console.Write("\n" + _rm?.GetString("ReturnMainMenu", _culture));
                        Console.ReadKey();
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
                            Console.WriteLine($"{exercise.Activity?.Name}: {exercise.Start} - {exercise.Finish}" +
                                $"\n\t" + _rm?.GetString("Duration", _culture) + $"{exercise.Duration}" +
                                $"\n\t" + _rm?.GetString("AmountBurnedKcal", _culture) + $"{exercise.BurnedCalories}\n");
                        }
                        Console.Write(_rm?.GetString("ReturnMainMenu", _culture));
                        Console.ReadKey();
                        Console.Clear();
                        continue;

                    case ConsoleKey.M:
                        var meals = _dataService.LoadData<Meal>();
                        if (meals is null) continue;
                        foreach (var meal in meals)
                        {
                            Console.WriteLine($"{meal}" +
                                $"\n" + _rm?.GetString("Carbs", _culture) + $"{meal.Carbohydrates}" + _rm?.GetString("gr", _culture) +
                                $"\n" + _rm?.GetString("Fats", _culture) + $"{meal.Fats}" + _rm?.GetString("gr", _culture) +
                                $"\n" + _rm?.GetString("Proteins", _culture) + $"{meal.Proteins}" + _rm?.GetString("gr", _culture) + "\n");
                        }
                        Console.Write(_rm?.GetString("ReturnMainMenu", _culture));
                        Console.ReadKey();
                        Console.Clear();
                        continue;

                    case ConsoleKey.A:
                        var activities = _dataService.LoadData<Activity>();
                        if (activities is null) continue;
                        foreach (var activity in activities)
                        {
                            Console.WriteLine($"{activity}\n");
                        }
                        Console.Write(_rm?.GetString("ReturnMainMenu", _culture));
                        Console.ReadKey();
                        Console.Clear();
                        continue;

                    case ConsoleKey.D:
                        while (true)
                        {
                            Console.WriteLine(_rm?.GetString("DeleteAccount?", _culture) + "\n" + _rm?.GetString("Confirmation", _culture));
                            var delConfirmation = Console.ReadKey();

                            switch (delConfirmation.Key)
                            {
                                default:
                                    continue;

                                case ConsoleKey.Y:
                                    userController.DeleteCurrentUser();
                                    Console.WriteLine(_rm?.GetString("SucsDel", _culture));
                                    break;

                                case ConsoleKey.N:
                                    Console.WriteLine(_rm?.GetString("ThankForStaying", _culture));
                                    break;
                            }
                            break;
                        }
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
            Console.WriteLine(_rm?.GetString("EnterActivityName", _culture));
            var activityName = Console.ReadLine();

            using (var exerciseController = new ExerciseController(user, _dataService))
            {
                var activity = exerciseController.Activities?.SingleOrDefault(a => a.Name == activityName);

                if (activity is null)
                {
                    Console.WriteLine(_rm?.GetString("EnterKcalPerHour", _culture));
                    var caloriesPerMinute = GetParsedLimitedDoubleValue(
                        _rm?.GetString("kcalPerHour", _culture) ?? "kilocalories per hour", 0, 3000);

                    activity = new Activity(activityName, caloriesPerMinute);
                }

                Console.WriteLine(_rm?.GetString("EnterExStartTime", _culture));
                var start = GetParsedLimitedDateTimeValue(
                    _rm?.GetString("startExTime", _culture) ?? "start time of the exercise", 
                    DateTime.Now.AddYears(-1), 
                    DateTime.Now.AddMinutes(-1));

                Console.WriteLine(_rm?.GetString("EnterExEndTime", _culture));
                var finish = GetParsedLimitedDateTimeValue(_rm?.GetString("endExTime", _culture) ?? "end time of the exercise", start, DateTime.Now);

                exerciseController.Add(activity, start, finish);

                if (exerciseController.Exercises is null) throw new ArgumentNullException(nameof(exerciseController.Exercises));

                Console.WriteLine();

                foreach (var exercise in exerciseController.Exercises)
                {
                    #region Item validation

                    if (user?.Name is null) continue;
                    if (exercise.User?.Name != user.Name) continue;

                    #endregion

                    Console.WriteLine($"{exercise.Activity?.Name}: {exercise.Start} - {exercise.Finish}" + 
                        $"\n\t" + _rm?.GetString("Duration", _culture) + $"{exercise.Duration}" +
                        $"\n\t" + _rm?.GetString("AmountBurnedKcal", _culture) + $"{exercise.BurnedCalories}\n");
                } 
            }

            Console.Write(_rm?.GetString("ReturnMainMenu", _culture));
            Console.ReadKey();
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

                    foreach (var foodIntakeUnit in foodIntakeController.FoodIntake.Meals)
                    {
                        if (foodIntakeUnit.Meal is null) continue;
                        var units = foodIntakeUnit.Meal.IsLiquid ? 
                            _rm?.GetString("ml", _culture) : _rm?.GetString("gr", _culture);
                        Console.WriteLine($"\t{foodIntakeUnit.Meal?.Name} - {foodIntakeUnit.Weight} {units}, {foodIntakeUnit.TotalKilocalories} kcal.");
                    }

                    Console.WriteLine("\n" + _rm?.GetString("AddAnotherMeal?", _culture) + _rm?.GetString("Confirmation", _culture));
                    if (Console.ReadKey().Key != ConsoleKey.Y)
                    {
                        foodIntakeController.Save();

                        Console.Write("\n" + _rm?.GetString("TotalKcalCurFoodIntake", _culture) + 
                            $"{foodIntakeController.FoodIntake.TotalKilocalories}\n");

                        Console.Write(_rm?.GetString("ReturnMainMenu", _culture));
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
                    Console.Clear();
                } 
            }
        }

        private static (Meal Meal, double Weight) EnterMeal(FoodIntakeController foodIntakeController)
        {
            Console.Write(_rm?.GetString("EnterMealName", _culture));
            var mealName = Console.ReadLine();

            var meal = foodIntakeController.Meals?.SingleOrDefault(m => m.Name == mealName);

            var isLiquid = meal != null ? meal.IsLiquid : GetParsedBoolValue(
                _rm?.GetString("IsLiquid?", _culture) ?? "Is the product a liquid?");

            double mealQuantity;

            if (isLiquid)
            {
                Console.Write(_rm?.GetString("EnterProductVolume", _culture));
                mealQuantity = GetParsedLimitedDoubleValue(_rm?.GetString("productVolume", _culture) ?? "product volume", 0, 3000);
            }
            else
            {
                Console.Write(_rm?.GetString("EnterMealWeight", _culture));
                mealQuantity = GetParsedLimitedDoubleValue(_rm?.GetString("mealWeight", _culture) ?? "meal weight", 0, 2000);
            }

            if (meal != null) return (meal, mealQuantity);

            Console.Write("\n" + _rm?.GetString("EnterKcal", _culture));
            var kilocalories = GetParsedLimitedDoubleValue(_rm?.GetString("kcals", _culture) ?? "kilocalories", 0, 900);

            Console.WriteLine(_rm?.GetString("AddInform?", _culture) + "\n" + _rm?.GetString("Confirmation", _culture));
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.Write(_rm?.GetString("EnterProteins", _culture));
                var proteins = GetParsedLimitedDoubleValue(_rm?.GetString("prts", _culture) ?? "proteins", 0, 50);

                Console.Write(_rm?.GetString("EnterFats", _culture));
                var fats = GetParsedLimitedDoubleValue(_rm?.GetString("fts", _culture) ?? "fats", 0, 100);

                Console.Write(_rm?.GetString("EnterCarbs", _culture));
                var carbohydrates = GetParsedLimitedDoubleValue(_rm?.GetString("crbs", _culture) ?? "carbohydrates", 0, 100);

                return (new Meal(mealName, kilocalories, isLiquid, proteins, fats, carbohydrates), mealQuantity);
            }
            return (new Meal(mealName, kilocalories, isLiquid), mealQuantity);
        }

        private static bool GetParsedBoolValue(string question)
        {
            while (true)
            {
                Console.WriteLine(question);
                Console.WriteLine(_rm?.GetString("Confirmation", _culture));
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    default:
                        continue;

                    case ConsoleKey.Y:
                        return true;

                    case ConsoleKey.N:
                        return false;
                } 
            }
        }

        private static DateTime GetParsedLimitedDateTimeValue(string paramName, DateTime lowerBound, DateTime upperBound)
        {
            while (true)
            {
                var value = GetParsedValue<DateTime>(paramName);
                if (value > lowerBound && value < upperBound) return value;
                Console.WriteLine(
                    _rm?.GetString("Incorrect", _culture) + 
                    $" {paramName}. " + 
                    _rm?.GetString("PleaseTryAgain", _culture));
            }
        }

        private static double GetParsedLimitedDoubleValue(string paramName, double lowerBound, double upperBound)
        {
            while (true)
            {
                var value = GetParsedValue<double>(paramName);
                if (value >= lowerBound && value <= upperBound) return value;
                Console.WriteLine(
                    _rm?.GetString("Incorrect", _culture) +
                    $" {paramName}. " +
                    _rm?.GetString("PleaseTryAgain", _culture));
            }
        }

        private static T? GetParsedValue<T>(string paramName)
        {
            while (true)
            {
                var value = Console.ReadLine();
                if (value is null) continue;
                if (TryParse(value, out T? newValue)) return newValue;
                Console.WriteLine(
                    _rm?.GetString("Incorrect", _culture) +
                     $" {paramName}. " +
                    _rm?.GetString("PleaseTryAgain", _culture));
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