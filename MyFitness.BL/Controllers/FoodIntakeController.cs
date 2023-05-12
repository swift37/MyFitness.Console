using MyFitness.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyFitness.BL.Controllers
{
    public class FoodIntakeController : ControllerBase
    {
        private const string MEALS_FILE_PATH = "meals.json";
        private const string FOOD_INTAKES_FILE_PATH = "food_intakes.json";

        private readonly User? _user;

        public List<FoodIntake>? FoodIntakes { get; }

        public FoodIntake? FoodIntake { get; }

        public List<Meal>? Meals { get; } 

        public FoodIntakeController(User? user, DateTime foodIntakeMoment)
        {
            _user = user ?? throw new ArgumentNullException(nameof(user));
            FoodIntakes = LoadFoodIntakes();
            Meals = LoadMeals();

            FoodIntake = new FoodIntake(_user, foodIntakeMoment);
        }

        public void Add(Meal meal, double weight)
        {
            #region Data validation

            if (FoodIntake is null) throw new ArgumentNullException(nameof(FoodIntake));
            if (FoodIntakes is null) throw new ArgumentNullException(nameof(FoodIntakes));
            if (Meals is null) throw new ArgumentNullException(nameof(Meals));

            #endregion
   
            if (Meals.SingleOrDefault(m => m.Name == meal.Name) is null) Meals.Add(meal);

            if (FoodIntakes.SingleOrDefault(f => f.Moment.ToString() == FoodIntake.Moment.ToString()) is null) 
                FoodIntakes.Add(FoodIntake);

            FoodIntake.Add(meal, weight);

            Save();
        }

        /// <summary>
        /// Load meals.
        /// </summary>
        /// <returns>Meals list.</returns>
        private List<Meal>? LoadMeals()
        {
            return LoadData<Meal>(MEALS_FILE_PATH)?.ToList() ?? new List<Meal>();
        }

        /// <summary>
        /// Load food intakes.
        /// </summary>
        /// <returns>Food intakes list.</returns>
        private List<FoodIntake>? LoadFoodIntakes()
        {
            return LoadData<FoodIntake>(FOOD_INTAKES_FILE_PATH)?.ToList() ?? new List<FoodIntake>();
        }

        /// <summary>
        /// Save meals.
        /// </summary>
        public void Save()
        {
            #region Data validation

            if (FoodIntakes is null) throw new ArgumentNullException(nameof(FoodIntakes)); 
            if (Meals is null) throw new ArgumentNullException(nameof(Meals));

            #endregion

            SaveData(FOOD_INTAKES_FILE_PATH, FoodIntakes);
            SaveData(MEALS_FILE_PATH, Meals);
        }
    }
}
