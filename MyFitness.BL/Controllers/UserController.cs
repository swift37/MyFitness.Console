using MyFitness.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyFitness.BL.Controllers
{
    /// <summary>
    /// User controller.
    /// </summary>
    public class UserController
    {
        /// <summary>
        /// User.
        /// </summary>
        public User? User { get; }

        public List<User>? Users { get; } = new List<User>();

        /// <summary>
        /// Create new username controller.
        /// </summary>
        /// <param name="username"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserController(string? username, string? gender, DateTime birthday, double weight, double height)
        {
            #region Username validation

            if (username is null || gender is null) 
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username) && username.Length <= 0 && username.Length > 16)
                throw new ArgumentException("Incorrect username.", nameof(username));

            #endregion

            var userGender = new Gender(gender);

            var newUser = new User(username, userGender, birthday, weight, height);

            Users?.Add(newUser);
        }

        /// <summary>
        /// Load username data.
        /// </summary>
        /// <returns>User.</returns>
        /// <exception cref="FileLoadException"></exception>
        public UserController()
        {
            var serializer = new DataContractJsonSerializer(typeof(List<User>));

            using (var stream = new FileStream("users.json", FileMode.OpenOrCreate))
            {
                if (serializer.ReadObject(stream) is List<User> users)
                    Users = users;
                else
                    Debug.WriteLine("Users not found");
            }
        }

        /// <summary>
        /// Save user data.
        /// </summary>
        public void SaveUser()
        {
            var serializer = new DataContractJsonSerializer(typeof(List<User>));

            using (var stream = new FileStream("users.json", FileMode.OpenOrCreate))
            {
                serializer.WriteObject(stream, Users);
            }
        }
    }
}
