﻿using Librarian.DAL.Entities.Base;
using System.Runtime.Serialization;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// Gender.
    /// </summary>
    [DataContract]
    public class Gender : NamedEntity
    {
        /// <summary>
        /// Create new gender.
        /// </summary>
        /// <param name="name">Gender name.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Gender(string? name)
        {
            #region Data validation
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name)); 
            #endregion

            Name = name;
        }

        /// <summary>
        /// No parameterless constructor for EtityFramework. 
        /// </summary>
        public Gender() { }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
