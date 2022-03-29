namespace SGM.Domain
{
    public interface ISlugifiedEntity : IEntity<string>
    {
        string Slug { get; set; }
    }
}
