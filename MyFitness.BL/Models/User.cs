using Librarian.DAL.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// User.
    /// </summary>
    [DataContract]
    public class User : NamedEntity
    {
        /// <summary>
        /// Gender.
        /// </summary>
        [DataMember(Name = "gender")]
        public Gender? Gender { get; set; }

        /// <summary>
        /// Birth date.
        /// </summary>
        [Column(TypeName = "date")]
        [DataMember(Name = "date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        [DataMember(Name = "weight")]
        public double Weight { get; set; }

        /// <summary>
        /// Height.
        /// </summary>
        [DataMember(Name = "height")]
        public double Height { get; set; }

        /// <summary>
        /// Age.
        /// </summary>
        [DataMember(Name = "age")]
        public int Age 
        { 
            get
            {
                var age = DateTime.Now.Year - DateOfBirth.Year;
                if (DateTime.Now < DateOfBirth.AddYears(age)) age--;
                return age;
            }
            set { } 
        }

        /// <summary>
        /// Age.
        /// </summary>
        [DataMember(Name = "bmi")]
        public double BMI 
        {
            get => Math.Round(Weight / (Height / 100 * (Height / 100)), 2); 
            set { } 
        }

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
        public User(string? name, Gender gender, DateTime dateOfBirth, double weight, double height)
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

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public User(string name)
        {
            #region Parameters validation

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            #endregion

            Name = name;
            DateOfBirth = DateTime.Now;
        }

        /// <summary>
        /// No parameterless constructor for EtityFramework. 
        /// </summary>
        public User() { }

        public override string ToString()
        {
            return $"{Name}, {Age} y.o.";
        }
    }
}
