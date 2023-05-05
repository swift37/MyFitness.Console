using System.Runtime.Serialization;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// User.
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// Name.
        /// </summary>
        [DataMember]
        public string? Name { get; set; }

        /// <summary>
        /// Gender.
        /// </summary>
        [DataMember]
        public Gender? Gender { get; set; }

        /// <summary>
        /// Birth date.
        /// </summary>
        [DataMember]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        [DataMember]
        public double Weight { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        [DataMember]
        public double Height { get; set; }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <param name="gender">User gender.</param>
        /// <param name="birthDate">User date of birth.</param>
        /// <param name="weight">User weight.</param>
        /// <param name="height">User height.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public User(string name, Gender gender, DateTime dateOfBirth, double weight, double height)
        {
            #region Parameters validation

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (gender == null)
                throw new ArgumentNullException(nameof(gender));
            if (dateOfBirth < DateTime.Parse("01.01.1900") || dateOfBirth > DateTime.Now)
                throw new InvalidDataException(nameof(dateOfBirth));
            if (weight < 0.0)
                throw new InvalidDataException(nameof(weight));
            if (height < 0.0)
                throw new InvalidDataException(nameof(height));
            
            #endregion

            Name = name;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            Weight = weight;
            Height = height;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
