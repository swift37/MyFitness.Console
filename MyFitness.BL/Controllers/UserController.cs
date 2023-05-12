﻿using MyFitness.BL.Models;
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
    public class UserController : ControllerBase
    {
        private const string FILE_PATH = "users.json";

        /// <summary>
        /// Users list.
        /// </summary>
        public List<User>? Users { get; } 

        /// <summary>
        /// Current user.
        /// </summary>
        public User? CurrentUser { get; }

        /// <summary>
        /// A field that determines whether the user is new or has already been registered.
        /// </summary>
        public bool IsNewUser { get; }

        /// <summary>
        /// Create new username controller.
        /// </summary>
        /// <param name="username"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserController(string? username)
        {
            #region Username validation

            if (username is null) 
                throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(username) && username.Length <= 0 && username.Length > 16)
                throw new ArgumentException("Incorrect username.", nameof(username));

            #endregion

            Users = LoadUsers();

            CurrentUser = Users?.SingleOrDefault(user => user.Name == username);

            if (CurrentUser is null)
            {
                IsNewUser = true;
                CurrentUser = new User(username);
            }
                
        }

        public void CreateUserData(string? gender, DateTime dateOfBirth, double weight, double height)
        {
            if (CurrentUser is null) throw new ArgumentNullException(nameof(CurrentUser));

            CurrentUser.Gender = new Gender(gender);
            CurrentUser.DateOfBirth = dateOfBirth;
            CurrentUser.Weight = weight;
            CurrentUser.Height = height;
            CurrentUser.SetUserAge();
            Users?.Add(CurrentUser);
            SaveUsers();
        }

        /// <summary>
        /// Load users data.
        /// </summary>
        /// <returns>Users list.</returns>
        /// <exception cref="FileLoadException"></exception>
        private List<User>? LoadUsers()
        {
            return LoadData<User>(FILE_PATH)?.ToList() ?? new List<User>();
        }

        /// <summary>
        /// Save user data.
        /// </summary>
        public void SaveUsers()
        {
            if (Users is null) throw new ArgumentNullException(nameof(Users));
            SaveData(FILE_PATH, Users);
        }
    }
}
