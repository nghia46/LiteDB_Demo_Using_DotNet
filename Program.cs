using LiteDB;
using System.Linq.Expressions;

public interface IRepository<T>
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    void Insert(T entity);
    void Update(T entity);
    void Delete(int id);
}

public class LiteDbRepository<T> : IRepository<T>
{
    private readonly LiteDatabase _db;
    private readonly LiteCollection<T> _collection;

    public LiteDbRepository(string connectionString)
    {
        _db = new LiteDatabase(connectionString);
        _collection = (LiteCollection<T>?)_db.GetCollection<T>(typeof(T).Name.ToLower() + "s");
    }

    public IEnumerable<T> GetAll()
    {
        return _collection.FindAll();
    }

    public T GetById(int id)
    {
        return _collection.FindById(id);
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _collection.Find(predicate);
    }

    public void Insert(T entity)
    {
        _collection.Insert(entity);
    }

    public void Update(T entity)
    {
        _collection.Update(entity);
    }

    public void Delete(int id)
    {
        _collection.Delete(id);
    }
}

class Program
{
    static void Main(string[] args)
    {
        string connectionString = @"DemoLiteDB";
        var repository = new LiteDbRepository<Customer>(connectionString);

        // Insert 100 custmmer using generic repositoy
        //for (int i = 0; i < 100; i++)
        //{
        //    var newCus = new Customer{Id=Guid.NewGuid().ToString(),Name="Nghia",Age=99,Address = new Address{Id= Guid.NewGuid().ToString(),Name="HCM"}};
        //    repository.Insert(newCus);
        //}
        
        // Get all customers
        var allCustomers = repository.GetAll();
        foreach (var customer in allCustomers)
        {
            Console.WriteLine($"Name: {customer.Name}, Age: {customer.Age}, Address: {customer.Address.Name}");
        }
    }

    public class Customer
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public required Address Address { get; set; }
    }
    public class Address
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
