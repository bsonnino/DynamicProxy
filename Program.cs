Console.WriteLine("***\r\n Begin program\r\n");
var customerRepository = new Repository<Customer>();
// var customerRepositoryLogger = new RepositoryLogger<IRepository<Customer>>().Create(customerRepository);
// if (customerRepositoryLogger == null)
// {
//     return;
// }
var customerRepositoryLogger = new DynamicProxy<IRepository<Customer>>().Create(customerRepository,
    s => Log($"Entering {s.Name}"),
    s => Log($"Exiting {s.Name}"),
    s => Log($"Error {s.Name}"),
    s => s.Name != "GetAll");
if (customerRepositoryLogger == null)
{
    return;
}
var customer = new Customer(1, "John Doe", "1 Main Street");
customerRepositoryLogger.Add(customer);
customerRepositoryLogger.GetAll();
customerRepositoryLogger.Delete(customer);
customerRepositoryLogger.GetAll();
Console.WriteLine("\r\nEnd program\r\n***");

static void Log(string msg)
{
    Console.ForegroundColor = msg.StartsWith("Entering") ? ConsoleColor.Blue :
        msg.StartsWith("Exiting") ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(msg);
    Console.ResetColor();
}
record Customer(int Id, string Name, string Address);
