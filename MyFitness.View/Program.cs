using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using MyFitness.BL.Services;
using MyFitness.BL.Services.Interfaces;
using System.Globalization;
using System.Resources;
using System.Text;

namespace MyFitness.View
{
    internal class Program
    {
        private static IDataIOService? _dataService;
        private static CultureInfo? _culture;
        private static ResourceManager? _rm;

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            _dataService = new DatabaseService("MyFitness.db");
            //_dataService = new SerializationService();

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
                Console.WriteLine(_rm?.GetString("UserProfile", _culture));
                Console.WriteLine(_rm?.GetString("AddFoodIntake", _culture));
                Console.WriteLine(_rm?.GetString("AddExercise", _culture));
                Console.WriteLine(_rm?.GetString("UserFoodIntakes", _culture));
                Console.WriteLine(_rm?.GetString("UserExercises", _culture));
                Console.WriteLine(_rm?.GetString("AllMeals", _culture));
                Console.WriteLine(_rm?.GetString("AllActivities", _culture));
                Console.WriteLine(_rm?.GetString("Settings", _culture));
                Console.WriteLine(_rm?.GetString("Exit", _culture));
                var key = Console.ReadKey();
                Console.Clear();

                switch (key.Key)
                {
                    case ConsoleKey.U:
                        Console.WriteLine(_rm?.GetString("Profile_UserProfile", _culture) + "\n");

                        Console.WriteLine(_rm?.GetString("Profile_Username", _culture) 
                            + userController.CurrentUser?.Name ?? "none");

                        Console.WriteLine(_rm?.GetString("Profile_Age", _culture) 
                            + userController.CurrentUser?.Age + " " + _rm?.GetString("yo", _culture));

                        Console.WriteLine(_rm?.GetString("Profile_Gender", _culture) 
                            + userController.CurrentUser?.Gender?.Name ?? "no data");

                        Console.WriteLine(_rm?.GetString("Profile_DateOfBirth", _culture) 
                            + userController.CurrentUser?.DateOfBirth.ToShortDateString());

                        Console.WriteLine(_rm?.GetString("Profile_Height", _culture) 
                            + userController.CurrentUser?.Height + " " + _rm?.GetString("cm", _culture));

                        Console.WriteLine(_rm?.GetString("Profile_Weight", _culture) 
                            + userController.CurrentUser?.Weight + " " + _rm?.GetString("kg", _culture));

                        Console.WriteLine(_rm?.GetString("Profile_BMI", _culture) + userController.CurrentUser?.BMI);

                        Console.Write("\n" + _rm?.GetString("ReturnMainMenu", _culture));
                        Console.ReadKey();
                        Console.Clear();
                        continue;

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
                            Console.WriteLine($"{foodIntake.Moment}\n\t" +
                                _rm?.GetString("TotalKcalFoodIntake", _culture) + $"{foodIntake.TotalKilocalories}");
                            if (foodIntake.Meals is null) continue;
                            foreach (var FoodIntakeUnit in foodIntake.Meals)
                            {
                                if (FoodIntakeUnit.Meal is null) continue;
                                var units = FoodIntakeUnit.Meal.IsLiquid ? 
                                    _rm?.GetString("ml", _culture) : _rm?.GetString("g", _culture);
                                Console.WriteLine(
                                    $"\t\t{FoodIntakeUnit.Meal?.Name} - {FoodIntakeUnit.Weight} {units}, {foodIntake.TotalKilocalories} " +
                                    _rm?.GetString("kcals", _culture));
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
                                $"\n" + _rm?.GetString("Carbs", _culture) + $"{meal.Carbohydrates}" + _rm?.GetString("g", _culture) +
                                $"\n" + _rm?.GetString("Fats", _culture) + $"{meal.Fats}" + _rm?.GetString("g", _culture) +
                                $"\n" + _rm?.GetString("Proteins", _culture) + $"{meal.Proteins}" + _rm?.GetString("g", _culture) + "\n");
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
                            Console.WriteLine(
                                $"{activity}, {activity.KilocaloriesPerHour} " + _rm?.GetString("kcalPerHour", _culture) + "\n");
                        }
                        Console.Write(_rm?.GetString("ReturnMainMenu", _culture));
                        Console.ReadKey();
                        Console.Clear();
                        continue;

                    case ConsoleKey.S:
                        while (true)
                        {
                            if (userController.CurrentUser != null && userController.CurrentUser.Name == "admin")
                                Console.WriteLine(_rm?.GetString("Settings_DelActivity", _culture));

                            Console.WriteLine(_rm?.GetString("Settings_DelExercise", _culture));

                            if (userController.CurrentUser != null && userController.CurrentUser.Name == "admin")
                                Console.WriteLine(_rm?.GetString("Settings_DelMeal", _culture));

                            Console.WriteLine(_rm?.GetString("Settings_DelFoodIntake", _culture));

                            Console.WriteLine(_rm?.GetString("Settings_DelCurUser", _culture));

                            if (userController.CurrentUser != null && userController.CurrentUser.Name == "admin") 
                                Console.WriteLine(_rm?.GetString("Settings_SwitchDataService", _culture));

                            Console.WriteLine(_rm?.GetString("Settings_ReturnMainMenu", _culture));

                            var settingsKey = Console.ReadKey();
                            Console.Clear();

                            switch (settingsKey.Key)
                            {
                                case ConsoleKey.A:
                                    if (userController.CurrentUser is null || userController.CurrentUser.Name != "admin")
                                    {
                                        Console.Clear();
                                        continue;
                                    }

                                    Console.Write(_rm?.GetString("Del_EnterActivityName", _culture));
                                    var activityName = Console.ReadLine();

                                    using (
                                        var exerciseContr = new ExerciseController(userController.CurrentUser, _dataService))
                                    {
                                        if (exerciseContr.DeleteActivity(activityName))
                                            Console.WriteLine(_rm?.GetString("ActivitySucDel", _culture));
                                        else
                                            Console.WriteLine(_rm?.GetString("ActivityNotFound", _culture));
                                    }
                                    Console.Write("\n" + _rm?.GetString("ReturnSettingsMenu", _culture));
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;

                                case ConsoleKey.E:
                                    Console.Write(_rm?.GetString("Del_EnterExerciseStartTime", _culture));
                                    var exerciseStartTime = GetParsedValue<DateTime>(
                                        _rm?.GetString("exerciseStartTime", _culture) ?? "time or date and start time of the exercise");

                                    using (
                                        var exerciseContr = new ExerciseController(userController.CurrentUser, _dataService))
                                    {
                                        if (exerciseContr.DeleteExercise(exerciseStartTime))
                                            Console.WriteLine(_rm?.GetString("ExerciseSucDel", _culture));
                                        else
                                            Console.WriteLine(_rm?.GetString("ExerciseNotFound", _culture));
                                    }
                                    Console.Write("\n" + _rm?.GetString("ReturnSettingsMenu", _culture));
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;

                                case ConsoleKey.M:
                                    if (userController.CurrentUser is null || userController.CurrentUser.Name != "admin") 
                                    {
                                        Console.Clear();
                                        continue;
                                    }
    
                                    Console.Write(_rm?.GetString("Del_EnterMealName", _culture));
                                    var mealName = Console.ReadLine();

                                    using (
                                        var foodIntakeContr = new FoodIntakeController(userController.CurrentUser,
                                        DateTime.Now,
                                        _dataService))
                                    {
                                        if (foodIntakeContr.DeleteMeal(mealName))
                                            Console.WriteLine(_rm?.GetString("MealSucDel", _culture));
                                        else
                                            Console.WriteLine(_rm?.GetString("MealNotFound", _culture));
                                    }
                                    Console.Write("\n" + _rm?.GetString("ReturnSettingsMenu", _culture));
                                    Console.ReadKey();
                                    Console.Clear();
                                    continue;

                                case ConsoleKey.F:
                                    Console.Write(_rm?.GetString("Del_EnterFoodIntakeMoment", _culture));
                                    var foodIntakeMoment = GetParsedValue<DateTime>(
                                        _rm?.GetString("foodIntakeMoment", _culture) ?? "moment of the food intake");

                                    using (
                                        var foodIntakeContr = new FoodIntakeController(userController.CurrentUser,
                                        DateTime.Now, 
                                        _dataService))
                                    {
                                        if (foodIntakeContr.DeleteFoodIntake(foodIntakeMoment))
                                            Console.WriteLine(_rm?.GetString("FoodIntakeSucDel", _culture));
                                        else
                                            Console.WriteLine(_rm?.GetString("FoodIntakeNotFound", _culture));
                                    }
                                    Console.Write("\n" + _rm?.GetString("ReturnSettingsMenu", _culture));
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
                                                Console.ReadKey();
                                                Environment.Exit(0);
                                                break;

                                            case ConsoleKey.N:
                                                Console.WriteLine(_rm?.GetString("ThankForStaying", _culture));
                                                Console.Write("\n" + _rm?.GetString("ReturnSettingsMenu", _culture));
                                                Console.ReadKey();
                                                Console.Clear();
                                                break;
                                        }
                                        break;
                                    }
                                    continue;

                                case ConsoleKey.S:
                                    while (true)
                                    {
                                        if (userController.CurrentUser is null || userController.CurrentUser.Name != "admin") break;

                                        Console.WriteLine(_rm?.GetString("SelectDataService", _culture) + "\n");
                                        Console.WriteLine(_rm?.GetString("Service_Database", _culture));
                                        Console.WriteLine(_rm?.GetString("Service_Serialization", _culture));
                                        Console.WriteLine(_rm?.GetString("R_ReturnSettingsMenu", _culture));
                                        var selectService = Console.ReadKey();

                                        switch (selectService.Key)
                                        {
                                            case ConsoleKey.D:
                                                _dataService = new DatabaseService("MyFitness.db");
                                                Console.Write(_rm?.GetString("ServiceSucChangedDb", _culture));

                                                Console.Write("\n" + _rm?.GetString("AppExit", _culture));
                                                Console.ReadKey();
                                                Environment.Exit(0);
                                                break;

                                            case ConsoleKey.S:
                                                _dataService = new SerializationService();
                                                Console.Write(_rm?.GetString("ServiceSucChangedSer", _culture));

                                                Console.Write("\n" + _rm?.GetString("AppExit", _culture));
                                                Console.ReadKey();
                                                Environment.Exit(0);
                                                break;

                                            case ConsoleKey.R:
                                                break;

                                            default:
                                                Console.Clear();
                                                continue;
                                        }
                                        break;
                                    }
                                    Console.Clear();
                                    continue;

                                case ConsoleKey.R:
                                    Console.Clear();
                                    break;

                                default:
                                    Console.Clear();
                                    continue;
                            }
                            break;
                        }
                        continue;

                    case ConsoleKey.Q:
                        break;

                    default:
                        Console.Clear();
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
                            _rm?.GetString("ml", _culture) : _rm?.GetString("g", _culture);
                        Console.WriteLine(
                            $"\t{foodIntakeUnit.Meal?.Name} - {foodIntakeUnit.Weight} {units}, {foodIntakeUnit.TotalKilocalories} " +
                            _rm?.GetString("kcal", _culture));
                    }

                    Console.WriteLine("\n" + _rm?.GetString("AddAnotherMeal?", _culture) + "\n" + _rm?.GetString("Confirmation", _culture));
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
                "\n" + _rm?.GetString("IsLiquid?", _culture) ?? "Is the product a liquid?" + "\n");

            string per100units;

            double mealQuantity;

            if (isLiquid)
            {
                Console.Write("\n" + _rm?.GetString("EnterProductVolume", _culture));
                mealQuantity = GetParsedLimitedDoubleValue(_rm?.GetString("productVolume", _culture) ?? "product volume", 0, 3000);
                per100units = _rm?.GetString("Per100mlProduct", _culture) ?? "per 100 ml of the product (just a number in grams)";
            }
            else
            {
                Console.Write("\n" + _rm?.GetString("EnterMealWeight", _culture));
                mealQuantity = GetParsedLimitedDoubleValue(_rm?.GetString("mealWeight", _culture) ?? "meal weight", 0, 2000);
                per100units = _rm?.GetString("Per100gMeal", _culture) ?? "per 100 g of the meal (just a number in grams)";
            }

            if (meal != null) return (meal, mealQuantity);

            Console.Write("\n" + _rm?.GetString("EnterKcal", _culture) + _rm?.GetString("EnterKcal", _culture) + per100units);
            var kilocalories = GetParsedLimitedDoubleValue(_rm?.GetString("kcals", _culture) ?? "kilocalories", 0, 900);

            Console.WriteLine(_rm?.GetString("AddInform?", _culture) + "\n" + _rm?.GetString("Confirmation", _culture));
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.Write(_rm?.GetString("EnterProteins", _culture) + per100units);
                var proteins = GetParsedLimitedDoubleValue(_rm?.GetString("prts", _culture) ?? "proteins", 0, 50);

                Console.Write(_rm?.GetString("EnterFats", _culture) + per100units);
                var fats = GetParsedLimitedDoubleValue(_rm?.GetString("fts", _culture) ?? "fats", 0, 100);

                Console.Write(_rm?.GetString("EnterCarbs", _culture) + per100units);
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
                if (value >= lowerBound && value <= upperBound) return value;
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

        private static string GetParsedLimitedStringValue(string paramName, int lowerBound, int upperBound)
        {
            while (true)
            {
                var value = GetParsedValue<string>(paramName);
                if (!string.IsNullOrWhiteSpace(value) && value.Count() >= lowerBound && value.Count() <= upperBound) return value;
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