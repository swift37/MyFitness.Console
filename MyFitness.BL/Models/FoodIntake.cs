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
        public Dictionary<Meal, double>? Meals { get; set; }

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
            Meals = new Dictionary<Meal, double>();
        }

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

            var product = Meals.Keys.FirstOrDefault(m => m.Name.Equals(meal.Name));

            if (product is null)
                Meals.Add(meal, weight);
            else
                Meals[product] += weight;
        }
    }
}
