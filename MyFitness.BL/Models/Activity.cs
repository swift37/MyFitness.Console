using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// Physical activity.
    /// </summary>
    [DataContract]
    public class Activity
    {
        /// <summary>
        /// Physical activity name.
        /// </summary>
        [DataMember(Name = "name")]
        public string? Name { get; set; }

        /// <summary>
        /// Calories per minut.
        /// </summary>
        [DataMember(Name = "calories_per_minute")]
        public double CaloriesPerMinute { get; set; }

        /// <summary>
        /// Create new physical activity.
        /// </summary>
        /// <param name="name">Physical activity name.</param>
        /// <param name="caloriesPerMinute">Calories per minut.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public Activity(string? name, double caloriesPerMinute)
        {
            #region Data validation

            if (name is null) throw new ArgumentNullException(nameof(name));
            if (caloriesPerMinute <= 0) throw new InvalidDataException(nameof(caloriesPerMinute)); 

            #endregion

            Name = name;
            CaloriesPerMinute = caloriesPerMinute;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
