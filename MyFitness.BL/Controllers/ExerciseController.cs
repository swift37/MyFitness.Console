using MyFitness.BL.Context;
using MyFitness.BL.Models;
using MyFitness.BL.Services;
using MyFitness.BL.Services.Interfaces;

namespace MyFitness.BL.Controllers
{
    public class ExerciseController : IDisposable
    {
        private readonly IDataIOService _dataService;
        private readonly User? _user;

        public List<Activity>? Activities { get; }

        public List<Exercise>? Exercises { get; }

        public ExerciseController(User? user, IDataIOService? dataService)
        {
            #region Data validation

            if (dataService is null)
                throw new ArgumentNullException(nameof(dataService));

            #endregion

            _dataService = dataService;

            _user = user ?? throw new ArgumentNullException(nameof(user));

            Activities = LoadActivities();
            Exercises = LoadExercises();
        }

        public void Add(Activity? activity, DateTime start, DateTime finish)
        {
            #region Data validation

            if (Exercises is null) throw new ArgumentNullException(nameof(Exercises));
            if (Activities is null) throw new ArgumentNullException(nameof(Activities));
            if (activity is null) throw new ArgumentNullException(nameof(activity));

            #endregion

            if (Activities.SingleOrDefault(a => a.Name == activity.Name) is null) Activities.Add(activity);

            var newExercise = new Exercise(start, finish, activity, _user);
            Exercises.Add(newExercise);

            Save();
        }

        /// <summary>
        /// Load activities.
        /// </summary>
        /// <returns>Activities list.</returns>
        private List<Activity>? LoadActivities()
        {
            return _dataService.LoadData<Activity>()?.ToList() ?? new List<Activity>();
        }

        /// <summary>
        /// Load exercises.
        /// </summary>
        /// <returns>Exercises list.</returns>
        private List<Exercise>? LoadExercises()
        {
            return _dataService.LoadData<Exercise>()?.ToList() ?? new List<Exercise>();
        }

        /// <summary>
        /// Save activities and exercises.
        /// </summary>
        private void Save()
        {
            _dataService.SaveData(Exercises);
            if (!(_dataService is DatabaseService)) _dataService.SaveData(Activities);
        }

        public void Dispose() 
        {
            GC.SuppressFinalize(this);
        }
    }
}
