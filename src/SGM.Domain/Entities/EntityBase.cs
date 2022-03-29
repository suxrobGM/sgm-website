using System.ComponentModel.DataAnnotations;
using SuxrobGM.Sdk.Utils;

namespace SGM.Domain.Entities
{
    public abstract class EntityBase : IEntity<string>
    {
        protected EntityBase()
        {
            Id = GeneratorId.GenerateLong();
            Timestamp = DateTime.Now;
        }

        [Display(Name = "ID")]
        public string Id { get; set; }

        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
