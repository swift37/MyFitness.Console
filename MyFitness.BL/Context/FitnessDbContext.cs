﻿using MyFitness.BL.Models;
using System.Data.Entity;

namespace MyFitness.BL.Context
{
    public class FitnessDbContext : DbContext
    {
        public DbSet<Activity>? Activities { get; set; }

        public DbSet<Exercise>? Exercises { get; set; }

        public DbSet<FoodIntake>? FoodIntakes { get; set; }

        public DbSet<FoodIntakeUnit>? FoodIntakeUnits { get; set; }

        public DbSet<Gender>? Genders { get; set; }

        public DbSet<Meal>? Meals { get; set; }

        public DbSet<User>? Users { get; set; }

        public FitnessDbContext(string connectrionString) : base(connectrionString) { }

        /// <summary>
        /// No parameterless constructor for creating migrations
        /// </summary>
        public FitnessDbContext () : base () { }
    }
}
