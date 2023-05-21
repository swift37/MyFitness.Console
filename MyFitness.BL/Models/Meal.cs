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
        /// Kilocalories per 100 grams of product.
        /// </summary>
        [DataMember(Name = "calories")]
        public double Kilocalories { get; set; }

        /// <summary>
        /// Is the dish a liquid.
        /// </summary>
        [DataMember(Name = "is_liquid")]
        public bool IsLiquid { get; set; }

        /// <summary>
        /// Create new meal.
        /// </summary>
        /// <param name="name">Meal name.</param>
        public Meal(string? name, double kilocalories, bool isLiquid) : this(name, kilocalories, isLiquid, 0, 0, 0) { }

        /// <summary>
        /// Create new meal.
        /// </summary>
        /// <param name="name">Meal name.</param>
        /// <param name="kilocalories">Kilocalories.</param>
        /// <param name="proteins">Proteins.</param>
        /// <param name="fats">Fats.</param>
        /// <param name="carbohydrates">Carbohydrates.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Meal(string? name, double kilocalories, bool isLiquid, double proteins, double fats, double carbohydrates)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Kilocalories = kilocalories;
            IsLiquid = isLiquid;
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
            return $"{Name}, {Kilocalories} kcal. per 100 grams";
        }
    }
}
