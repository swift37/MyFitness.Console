using System.Runtime.Serialization.Json;
using MyFitness.BL.Services.Interfaces;

namespace MyFitness.BL.Services
{
    /// <summary>
    /// Serialize service.
    /// </summary>
    public class SerializeService : IDataIOService
    {
        /// <summary>
        /// Serialize user data.
        /// </summary>
        /// <param name="filePath">Path to data file.</param>
        /// <returns>Entities list.</returns>
        public IEnumerable<T>? LoadData<T>(string filePath)
        {
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                if (stream.Length > 0 && serializer.ReadObject(stream) is IEnumerable<T> entities)
                    return entities;

                return null;
            }
        }

        /// <summary>
        /// Deserialize user data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">Path to save data file.</param>
        /// <param name="entities">Entities list.</param>
        public void SaveData<T>(string filePath, IEnumerable<T> entities)
        {
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                serializer.WriteObject(stream, entities);
            }
        }
    }
}
