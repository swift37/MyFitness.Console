namespace MyFitness.BL.Models
{
    /// <summary>
    /// Gender.
    /// </summary>
    public class Gender
    {
        /// <summary>
        /// Name.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// Create new gender.
        /// </summary>
        /// <param name="name">Gender name.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Gender(string name)
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
