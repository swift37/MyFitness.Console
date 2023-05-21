using Librarian.DAL.Entities.Base;
using System.Runtime.Serialization;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// Food intake.
    /// </summary>
    [DataContract]
    public class FoodIntake : Entity
    {
        /// <summary>
        /// Food intake foodIntakeMoment.
        /// </summary>
        [DataMember(Name = "foodIntakeMoment")]
        public DateTime Moment { get; set; }

        /// <summary>
        /// Foods and foods weight list.
        /// </summary>
        [DataMember(Name = "meals")]
        public ICollection<FoodIntakeUnit> Meals { get; set; } = new HashSet<FoodIntakeUnit>();

        /// <summary>
        /// Total number of kilocalories per food intake.
        /// </summary>
        [DataMember(Name = "total_kilocalories")]
        public double TotalKilocalories 
        { 
            get
            {
                double totalKcal = 0;
                foreach (var foodIntakeUnit in Meals)
                {
                    totalKcal += foodIntakeUnit.TotalKilocalories;
                }
                return totalKcal;
            }
            set { } 
        }

        /// <summary>
        /// User.
        /// </summary>
        [DataMember(Name = "user")]
        public User? User { get; set; }

        /// <summary>
        /// Create new food intake.
        /// </summary>
        /// <param name="user">Username.</param>
        /// <param name="foodIntakeMoment">Food intake foodIntakeMoment.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FoodIntake(User? user, DateTime foodIntakeMoment)
        {
            User = user ?? throw new ArgumentNullException(nameof(user)); 
            Moment = foodIntakeMoment;
        }

        /// <summary>
        /// No parameterless constructor for EtityFramework. 
        /// </summary>
        public FoodIntake() { }

        /// <summary>
        /// Add new meal to food intake.
        /// </summary>
        /// <param name="food">Meal.</param>
        /// <param name="weight">Meal weight.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(Meal meal, double weight)
        {
            #region Data validation

            if (meal.Name is null) throw new ArgumentNullException(nameof(meal.Name));
            if (Meals is null) throw new ArgumentNullException(nameof(Meals)); 

            #endregion

            var product = Meals.FirstOrDefault(m => m.Meal?.Name == meal.Name );

            if (product is null)
                Meals.Add(new FoodIntakeUnit(meal, weight));
            else
                product.Weight += weight;
        }
    }
}
