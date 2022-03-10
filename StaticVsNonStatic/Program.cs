namespace Demo;

public class Program
{
    public static void Main(string[] args)
    {
        Person Matt = new Person("Matt");
        Person Cameron = new Person("Cameron");
        Cameron.Name = "test";
        Console.WriteLine();

    }

}

public class Person
{

    public Person(string name)
    {
        Name = name;
    }
    public string Name { get; init; }
    public static string color { get; set; }    
}
