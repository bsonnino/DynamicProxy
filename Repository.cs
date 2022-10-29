public interface IRepository<T>
{
    void Add(T entity);
    void Delete(T entity);
    IEnumerable<T> GetAll();
}

public class Repository<T> : IRepository<T> 
{
    private readonly List<T> _entities = new List<T>();
    public void Add(T entity)
    {
        _entities.Add(entity);
        Console.WriteLine("Adding {0}", entity);
    }

    public void Delete(T entity)
    {
        _entities.Remove(entity);
        Console.WriteLine("Deleting {0}", entity);
    }

    public IEnumerable<T> GetAll()
    {
        Console.WriteLine("Getting entities");
        foreach (var entity in _entities)
        {
            Console.WriteLine($"  {entity}");
        }
        return _entities;
    }
}
