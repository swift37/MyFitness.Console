namespace MyFitness.BL.Models
{
    /// <summary>
    /// User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Gender.
        /// </summary>
        public Gender? Gender { get; }

        /// <summary>
        /// Birth date.
        /// </summary>
        public DateTime BirthDate { get; }

        /// <summary>
        /// Weight
        /// </summary>
        public double Weight { get; }

        /// <summary>
        /// Height.
        /// </summary>
        public double Height { get; }

        /// <summary>
        /// Create new user.
        /// </summary>
        /// <param name="name">User name.</param>
        /// <param name="gender">User gender.</param>
        /// <param name="birthDate">User birthday.</param>
        /// <param name="weight">User weight.</param>
        /// <param name="height">User height.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public User(string name, Gender gender, DateTime birthDate, double weight, double height)
        {
            #region Parameters validation

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (gender == null)
                throw new ArgumentNullException(nameof(gender));
            if (birthDate < DateTime.Parse("01.01.1900") || birthDate > DateTime.Now)
                throw new InvalidDataException(nameof(birthDate));
            if (weight < 0.0)
                throw new InvalidDataException(nameof(weight));
            if (height < 0.0)
                throw new InvalidDataException(nameof(height));
            
            #endregion

            Name = name;
            Gender = gender;
            BirthDate = birthDate;
            Weight = weight;
            Height = height;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
