namespace My_Library.Core.Data
{
    public interface IEntity
    {
        int Id { get; set; }
    }

    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}
