namespace SuxrobGM_Website.Core.Interfaces.Entities
{
    /// <summary>
    /// Interface to define set of entity classes
    /// </summary>
    public interface IEntity<TKey> 
    {
        TKey Id { get; set; }
    }
}
