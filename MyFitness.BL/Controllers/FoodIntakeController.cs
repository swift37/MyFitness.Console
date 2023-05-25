using Microsoft.VisualBasic.FileIO;
using MyFitness.BL.Models;
using MyFitness.BL.Services;
using MyFitness.BL.Services.Interfaces;

namespace MyFitness.BL.Controllers
{
    public class FoodIntakeController : IDisposable
    {
        private readonly IDataIOService _dataService;
        private readonly User? _user;

        public List<Meal>? Meals { get; } 

        public List<FoodIntake>? FoodIntakes { get; }

        public FoodIntake? FoodIntake { get; }

        public FoodIntakeController(User? user, DateTime foodIntakeMoment, IDataIOService? dataService)
        {
            #region Data validation

            if (dataService is null)
                throw new ArgumentNullException(nameof(dataService));

            #endregion

            _dataService = dataService;

            _user = user ?? throw new ArgumentNullException(nameof(user));
            FoodIntakes = LoadFoodIntakes();
            Meals = LoadMeals();

            FoodIntake = new FoodIntake(_user, foodIntakeMoment);

            var newFoodIntakeId = FoodIntakes?.LastOrDefault()?.Id ?? 0;
            //FoodIntakes?.Add(FoodIntake);
            FoodIntake.Id = ++newFoodIntakeId;
        }

        /// <summary>
        /// Add a meal to your current meal.
        /// </summary>
        /// <param name="meal">Meal.</param>
        /// <param name="weight">Meal weight.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(Meal? meal, double weight)
        {
            #region Data validation

            if (FoodIntake is null) throw new ArgumentNullException(nameof(FoodIntake));
            if (FoodIntakes is null) throw new ArgumentNullException(nameof(FoodIntakes));
            if (Meals is null) throw new ArgumentNullException(nameof(Meals));
            if (meal is null) throw new ArgumentNullException(nameof(meal));

            #endregion
            
            if (Meals.SingleOrDefault(m => m.Name == meal.Name) is null)
            {
                var newMealId = Meals.LastOrDefault()?.Id ?? 0;
                Meals.Add(meal);
                meal.Id = ++newMealId;
            }

            if (FoodIntakes.SingleOrDefault(f => f.Moment.ToString() == FoodIntake.Moment.ToString()) is null)
                FoodIntakes.Add(FoodIntake);

            FoodIntake.Add(meal, weight);
        }

        public bool DeleteFoodIntake(DateTime foodIntakeMoment)
        {
            #region Data validation

            if (_user is null) throw new ArgumentNullException(nameof(_user)); 

            #endregion

            var foodIntake = FoodIntakes?.FirstOrDefault(
                fI => fI.Moment.ToString() == foodIntakeMoment.ToString() && fI.User?.Name == _user.Name);
            if (foodIntake is null) return false;

            var foodIntakeUnitsIds = foodIntake.Meals.Select(u => u.Id);
            foreach (var item in foodIntakeUnitsIds)
            {
                _dataService.Remove<FoodIntakeUnit>(item);
            }

            _dataService.Remove<FoodIntake>(foodIntake.Id);
            FoodIntakes?.Remove(foodIntake);
            return true;
        }

        public bool DeleteMeal(string? name)
        {
            #region Data validation

            if (name is null) throw new ArgumentNullException(nameof(name)); 

            #endregion

            var meal = Meals?.SingleOrDefault(meal => meal.Name == name);
            if (meal is null) return false;

            _dataService.Remove<Meal>(meal.Id);
            Meals?.Remove(meal);
            return true;
        }

        /// <summary>
        /// Load meals.
        /// </summary>
        /// <returns>Meals list.</returns>
        private List<Meal>? LoadMeals()
        {
            return _dataService.LoadData<Meal>()?.ToList() ?? new List<Meal>();
        }

        /// <summary>
        /// Load food intakes.
        /// </summary>
        /// <returns>Food intakes list.</returns>
        private List<FoodIntake>? LoadFoodIntakes()
        {
            return _dataService.LoadData<FoodIntake>()?.ToList() ?? new List<FoodIntake>();
        }

        /// <summary>
        /// Save food intakes and meals.
        /// </summary>
        public void Save()
        {
            _dataService.SaveData(FoodIntakes);
            if (!(_dataService is DatabaseService))
            {
                _dataService.SaveData(Meals);
            }    
        }

        public void Dispose() 
        {
            GC.SuppressFinalize(this);
        }
    }
}
