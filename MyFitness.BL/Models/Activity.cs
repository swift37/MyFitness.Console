﻿using Librarian.DAL.Entities.Base;
using System.Runtime.Serialization;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// Physical activity.
    /// </summary>
    [DataContract]
    public class Activity : NamedEntity
    {
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

        /// <summary>
        /// No parameterless constructor for EtityFramework. 
        /// </summary>
        public Activity() { }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
