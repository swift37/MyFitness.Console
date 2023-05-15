using Librarian.DAL.Entities.Base;
using System.Runtime.Serialization;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// Meal.
    /// </summary>
    [DataContract]
    public class Meal : NamedEntity
    {
        private double ProteinsOneGr => Proteins / 100;

        private double FatsOneGr => Fats / 100;

        private double CarbohydratesOneGr => Carbohydrates / 100;

        private double CaloriesOneGr => Calories / 100;

        /// <summary>
        /// Proteins per 100 grams of product.
        /// </summary>
        [DataMember(Name = "proteins")]
        public double Proteins { get; set; }

        /// <summary>
        /// Fats per 100 grams of product.
        /// </summary>
        [DataMember(Name = "fats")]
        public double Fats { get; set; }

        /// <summary>
        /// Carbohydrates per 100 grams of product.
        /// </summary>
        [DataMember(Name = "carbohydrates")]
        public double Carbohydrates { get; set; }

        /// <summary>
        /// Calories per 100 grams of product.
        /// </summary>
        [DataMember(Name = "calories")]
        public double Calories { get; set; }

        /// <summary>
        /// Create new meal.
        /// </summary>
        /// <param name="name">Meal name.</param>
        public Meal(string? name) : this(name, 0, 0, 0, 0) { }

        /// <summary>
        /// Create new meal.
        /// </summary>
        /// <param name="name">Meal name.</param>
        /// <param name="calories">Calories.</param>
        /// <param name="proteins">Proteins.</param>
        /// <param name="fats">Fats.</param>
        /// <param name="carbohydrates">Carbohydrates.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Meal(string? name, double calories, double proteins, double fats, double carbohydrates)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Calories = calories;
            Proteins = proteins;
            Fats = fats;
            Carbohydrates = carbohydrates;
        }

        /// <summary>
        /// No parameterless constructor for EtityFramework. 
        /// </summary>
        public Meal() { }

        public override string ToString()
        {
            return $"{Name}, {Calories} kcal. per 100 grams";
        }
    }
}
