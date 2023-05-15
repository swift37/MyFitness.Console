using System.Runtime.Serialization.Json;
using Librarian.DAL.Entities.Base;
using MyFitness.BL.Services.Interfaces;

namespace MyFitness.BL.Services
{
    /// <summary>
    /// Serialize service.
    /// </summary>
    public class SerializeService : IDataIOService
    {
        /// <summary>
        /// Deserialize user data.
        /// </summary>
        /// <returns>Entities enumerable.</returns>
        public IEnumerable<T>? LoadData<T>() where T : Entity
        {
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));

            var filePath = $"{typeof(T).Name}.json";

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                if (stream.Length > 0 && serializer.ReadObject(stream) is IEnumerable<T> entities)
                    return entities;

                return null;
            }
        }

        /// <summary>
        /// Serialize user data.
        /// </summary>
        /// <param name="entities">Entities enumerable.</param>
        public void SaveData<T>(IEnumerable<T> entities) where T : Entity
        {
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));

            var filePath = $"{typeof(T).Name}.json";

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                serializer.WriteObject(stream, entities);
            }
        }
    }
}
