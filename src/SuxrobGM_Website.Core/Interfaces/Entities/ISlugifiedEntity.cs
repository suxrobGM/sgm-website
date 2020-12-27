namespace SuxrobGM_Website.Core.Interfaces.Entities
{
    public interface ISlugifiedEntity : IEntity<string>
    {
        string Slug { get; set; }
    }
}
