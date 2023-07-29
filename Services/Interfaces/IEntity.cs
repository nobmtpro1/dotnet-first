namespace Blog.Services.Interfaces
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
