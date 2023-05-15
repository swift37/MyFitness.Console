using Librarian.DAL.Entities.Base;
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
        /// <summary>
        /// Load user data.
        /// </summary>
        /// <typeparam name="T">T : Entity.</typeparam>
        /// <returns>Entities enumerable.</returns>
        public IEnumerable<T>? LoadData<T>() where T : Entity
        {
            using (var db = new FitnessDb())
            {
                return db.Set<T>().ToList();
            }
        }

        /// <summary>
        /// Save user data.
        /// </summary>
        /// <typeparam name="T">T : Entity.</typeparam>
        /// <param name="entities">Entities enumerable.</param>
        public void SaveData<T>(IEnumerable<T> entities) where T : Entity
        {
            using (var db = new FitnessDb())
            {
                db.Set<T>().AddRange(entities);
                db.SaveChanges();
            }
        }

        //public void Remove<T>(int id) where T : Entity, new()
        //{
        //    using (var db = new FitnessDb())
        //    {
        //        var entity = db.Set<T>().Local.FirstOrDefault(entity => entity.Id == id) ?? new T { Id = id };

        //        db.Remove(entity);
        //        db.SaveChanges();
        //    }
        //}
    }
}
