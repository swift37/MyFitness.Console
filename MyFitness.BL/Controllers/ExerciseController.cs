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

            if (Activities.SingleOrDefault(a => a.Name == activity.Name) is null)
            {
                var newActivityId = Activities.LastOrDefault()?.Id ?? 0;
                Activities.Add(activity);
                activity.Id = ++newActivityId;
            }

            var newExercise = new Exercise(start, finish, activity, _user);
            var newExerciseId = Exercises.LastOrDefault()?.Id ?? 0;
            newExercise.Id = ++newExerciseId;

            Exercises.Add(newExercise);

            Save();
        }

        public bool DeleteExercise(DateTime exerciseStartDateTime)
        {
            #region Data validation

            if (_user is null) throw new ArgumentNullException(nameof(_user));

            #endregion

            var exercise = Exercises?.FirstOrDefault(
                ex => ex.Start.ToString() == exerciseStartDateTime.ToString() && ex.User?.Name == _user.Name);
            if (exercise is null) return false;

            _dataService.Remove<Exercise>(exercise.Id);
            Exercises?.Remove(exercise);
            return true;
        }

        public bool DeleteActivity(string? name)
        {
            #region Data validation

            if (name is null) throw new ArgumentNullException(nameof(name));

            #endregion

            var activity = Activities?.SingleOrDefault(activity => activity.Name == name);
            if (activity is null) return false;

            _dataService.Remove<Activity>(activity.Id);
            Activities?.Remove(activity);
            return true;
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
