namespace SGM.Domain.Interfaces.Entities
{
    public interface ISlugifiedEntity : IEntity<string>
    {
        string Slug { get; set; }
    }
}
