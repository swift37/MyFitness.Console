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
   
            if (Meals.SingleOrDefault(m => m.Name == meal.Name) is null) Meals.Add(meal);

            if (FoodIntakes.SingleOrDefault(f => f.Moment.ToString() == FoodIntake.Moment.ToString()) is null) 
                FoodIntakes.Add(FoodIntake);

            FoodIntake.Add(meal, weight);
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
            if (!(_dataService is DatabaseService)) _dataService.SaveData(Meals);
        }

        public void Dispose() 
        {
            GC.SuppressFinalize(this);
        }
    }
}
