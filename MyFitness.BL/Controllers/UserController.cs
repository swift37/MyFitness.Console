using MyFitness.BL.Models;
using MyFitness.BL.Services;
using MyFitness.BL.Services.Interfaces;

namespace MyFitness.BL.Controllers
{
    /// <summary>
    /// User controller.
    /// </summary>
    public class UserController : IDisposable
    {
        private readonly IDataIOService _dataService;

        /// <summary>
        /// Genders collection.
        /// </summary>
        private List<Gender> _genders;

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
        public UserController(string? username, IDataIOService? dataService)
        {
            #region Username validation

            if (username is null) 
                throw new ArgumentNullException(nameof(username));
            if (dataService is null)
                throw new ArgumentNullException(nameof(dataService));
            if (string.IsNullOrWhiteSpace(username) && username.Length <= 0 && username.Length > 16)
                throw new ArgumentException("Incorrect username.", nameof(username));

            #endregion

            _dataService = dataService;
            _genders = LoadGenders();
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
            #region Data validation

            if (CurrentUser is null)
                throw new ArgumentNullException(nameof(CurrentUser));
            if (Users is null)
                throw new ArgumentNullException(nameof(Users));
            if (dateOfBirth < DateTime.Parse("01.01.1900") || dateOfBirth > DateTime.Now)
                throw new InvalidDataException(nameof(dateOfBirth));

            #endregion

            CurrentUser.Gender = _genders.SingleOrDefault(g => g.Name == gender) ?? new Gender(gender);
            CurrentUser.DateOfBirth = dateOfBirth;
            CurrentUser.Weight = weight;
            CurrentUser.Height = height;
            CurrentUser.SetUserAge();
            Users.Add(CurrentUser);
            SaveData();
        }

        public void DeleteCurrentUser()
        {
            if (CurrentUser is null) return;

            var removableUser = Users?.SingleOrDefault(user => user.Name == CurrentUser.Name);
            if (removableUser is null) return;

            _dataService.Remove<User>(removableUser.Id);
        }

        /// <summary>
        /// Load users data.
        /// </summary>
        /// <returns>Users list.</returns>
        /// <exception cref="FileLoadException"></exception>
        private List<User>? LoadUsers()
        {
            return _dataService.LoadData<User>()?.ToList() ?? new List<User>();
        }

        /// <summary>
        /// Load genders.
        /// </summary>
        /// <returns>Users list.</returns>
        /// <exception cref="FileLoadException"></exception>
        private List<Gender> LoadGenders()
        {
            return _dataService.LoadData<Gender>()?.ToList() ?? new List<Gender>();
        }

        /// <summary>
        /// Save user data.
        /// </summary>
        public void SaveData()
        {
            _dataService.SaveData(Users);
            if (!(_dataService is DatabaseService)) _dataService.SaveData(_genders);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
