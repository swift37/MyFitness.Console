using MyFitness.BL.Controllers;
using MyFitness.BL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFitness.BL.Context
{
    public class FitnessDb : DbContext
    {
        public FitnessDb() : base("DBConnection") { }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<FoodIntake> FoodIntakes { get; set; }

        public DbSet<Gender> Genders { get; set; }

        public DbSet<Meal> Meals { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
