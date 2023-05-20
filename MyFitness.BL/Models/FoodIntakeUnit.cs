using Librarian.DAL.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.BL.Models
{
    public class FoodIntakeUnit : Entity
    {
        /// <summary>
        /// Meal.
        /// </summary>
        public Meal? Meal { get; set; }

        /// <summary>
        /// Meal weight.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Create new food intake unit.
        /// </summary>
        /// <param name="meal">Meal.</param>
        /// <param name="weight">Weight.</param>
        public FoodIntakeUnit(Meal? meal, double weight)
        {
            #region Data validation

            if (meal is null) throw new ArgumentNullException(nameof(Meal)); 

            #endregion

            Meal = meal;
            Weight = weight;
        }

        /// <summary>
        /// No parameterless constructor for EtityFramework. 
        /// </summary>
        public FoodIntakeUnit() { }
    }
}
