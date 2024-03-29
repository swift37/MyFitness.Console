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
        /// Kilocalories per hour.
        /// </summary>
        [DataMember(Name = "kilocalories_per_hour")]
        public double KilocaloriesPerHour { get; set; }

        /// <summary>
        /// Create new physical activity.
        /// </summary>
        /// <param name="name">Physical activity name.</param>
        /// <param name="caloriesPerHour">Kilocalories per hour.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public Activity(string? name, double caloriesPerHour)
        {
            #region Data validation

            if (name is null) throw new ArgumentNullException(nameof(name));
            if (caloriesPerHour <= 0.0) throw new InvalidDataException(nameof(caloriesPerHour)); 

            #endregion

            Name = name;
            KilocaloriesPerHour = caloriesPerHour;
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
