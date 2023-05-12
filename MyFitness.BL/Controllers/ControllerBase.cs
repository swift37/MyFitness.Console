using System.Runtime.Serialization.Json;

namespace MyFitness.BL.Controllers
{
    /// <summary>
    /// Controller base.
    /// </summary>
    public abstract class ControllerBase
    {
        /// <summary>
        /// Save user data.
        /// </summary>
        protected void SaveData<T>(string filePath, IEnumerable<T> entities)
        {
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                serializer.WriteObject(stream, entities);
            }
        }

        /// <summary>
        /// Load entities data.
        /// </summary>
        /// <returns>Entities enumerable.</returns>
        /// <exception cref="FileLoadException"></exception>
        protected IEnumerable<T>? LoadData<T>(string filePath)
        {
            var serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                if (stream.Length > 0 && serializer.ReadObject(stream) is IEnumerable<T> entities)
                    return entities;

                return null;
            }
        }
    }
}
