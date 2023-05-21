using Librarian.DAL.Entities.Base;
using System.Runtime.Serialization;
using System.Xml.Linq;

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
        /// Total number of kilocalories per food intake.
        /// </summary>
        [DataMember(Name = "total_kilocalories")]
        public double TotalKilocalories
        {
            get => ((Meal?.Kilocalories ?? 0) / 100) * Weight;
            set { }
        }

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
