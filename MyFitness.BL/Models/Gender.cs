using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// Gender.
    /// </summary>
    [DataContract]
    public class Gender
    {
        /// <summary>
        /// Name.
        /// </summary>
        [DataMember]
        public string? Name { get; set; }

        /// <summary>
        /// Create new gender.
        /// </summary>
        /// <param name="name">Gender name.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Gender(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentNullException(nameof(name));
            
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
