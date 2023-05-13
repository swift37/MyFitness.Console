using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.BL.Models
{
    /// <summary>
    /// Physical exercise.
    /// </summary>
    [DataContract]
    public class Exercise
    {
        /// <summary>
        /// Physical exercise start.
        /// </summary>
        [DataMember(Name = "start")]
        public DateTime Start { get; set; }

        /// <summary>
        /// Physical exercise finish.
        /// </summary>
        [DataMember(Name = "finish")]
        public DateTime Finish { get; set; }

        /// <summary>
        /// Type of physical activity.
        /// </summary>
        [DataMember(Name = "activity")]
        public Activity? Activity { get; set; }

        /// <summary>
        /// User.
        /// </summary>
        [DataMember(Name = "user")]
        public User? User { get; set; }

        /// <summary>
        /// Create new physical exercise.
        /// </summary>
        /// <param name="start">Physical exercise start.</param>
        /// <param name="finish">Physical exercise finish.</param>
        /// <param name="activity">Type of physical activity.</param>
        /// <param name="user">User.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Exercise(DateTime start, DateTime finish, Activity? activity, User? user)
        {
            #region Data validation

            if (activity is null) throw new ArgumentNullException(nameof(activity));
            if (user is null) throw new ArgumentNullException(nameof(user)); 

            #endregion

            Start = start;
            Finish = finish;
            Activity = activity;
            User = user;
        }
    }
}
