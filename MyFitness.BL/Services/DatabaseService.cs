﻿using Librarian.DAL.Entities.Base;
using Microsoft.VisualBasic.FileIO;
using MyFitness.BL.Context;
using MyFitness.BL.Services.Interfaces;
using System.Data.Entity;

namespace MyFitness.BL.Services
{
    /// <summary>
    /// Database service.
    /// </summary>
    public class DatabaseService : IDataIOService
    {
        private readonly FitnessDbContext _dbContext;

        public DatabaseService(string connectionString)
        {
            _dbContext = new FitnessDbContext(connectionString);
        }

        /// <summary>
        /// Load user data.
        /// </summary>
        /// <typeparam name="T">T : Entity.</typeparam>
        /// <returns>Entities enumerable.</returns>
        public IEnumerable<T>? LoadData<T>() where T : Entity
        {
            return _dbContext.Set<T>().ToArray();
        }

        public void SaveData<T>(IEnumerable<T>? entities) where T : Entity
        {
            if (entities is null) throw new ArgumentNullException(nameof(entities));

            var entity = entities.LastOrDefault();
            if (entity is null) return;

            _dbContext.Entry(entity).State = EntityState.Added;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Remove entity.
        /// </summary>
        /// <typeparam name="T">T : Entity.</typeparam>
        /// <param name="id">Entity id.</param>
        public void Remove<T>(int id) where T : Entity, new()
        {
            var entity = _dbContext.Set<T>().Local.FirstOrDefault(entity => entity.Id == id) ?? new T { Id = id };
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}
