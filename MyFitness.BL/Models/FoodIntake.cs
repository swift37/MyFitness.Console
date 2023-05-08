using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// Food intake.
    /// </summary>
    [DataContract]
    public class FoodIntake
    {
        /// <summary>
        /// Food intake moment.
        /// </summary>
        [DataMember(Name = "moment")]
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
        /// <param name="moment">Food intake moment.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FoodIntake(User? user, DateTime moment)
        {
            User = user ?? throw new ArgumentNullException(nameof(user)); 
            Moment = moment;
            Meals = new Dictionary<Meal, double>();
        }

        /// <summary>
        /// Add new meal to food intake.
        /// </summary>
        /// <param name="food">Meal.</param>
        /// <param name="weight">Meal weight.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(Meal food, double weight)
        {
            if( Meals is null || food.Name is null) throw new ArgumentNullException(nameof(food));

            var product = Meals.Keys.FirstOrDefault(f => f.Name.Equals(food.Name));

            if (product is null)
                Meals.Add(food, weight);
            else
                Meals[product] += weight;
        }
    }
}
