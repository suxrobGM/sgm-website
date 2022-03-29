using System.ComponentModel.DataAnnotations;

namespace SGM.Domain.Entities;

public abstract class Entity : IAggregateRoot
{
    [Display(Name = "ID")]
    public string Id { get; set; } = Generator.NewGuid();

    [Display(Name = "Timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.Now;
}