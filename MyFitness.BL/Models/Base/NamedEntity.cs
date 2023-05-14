using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Librarian.DAL.Entities.Base
{
    [DataContract]
    public class NamedEntity : Entity
    {
        [DataMember(Name = "name")]
        [Required]
        public string? Name { get; set; }
    }
}
