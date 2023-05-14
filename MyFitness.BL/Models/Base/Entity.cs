using System.Runtime.Serialization;

namespace Librarian.DAL.Entities.Base
{
    [DataContract]
    public abstract class Entity
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
    }
}
