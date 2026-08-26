public class Animal
{
    public void eat()
    {
        Console.WriteLine("Base eat");
    }
}

public class Mammal: Animal
{
    public new void eat()
    {
        Console.WriteLine("Mammal eat");
    } 
}



public class App()
{
    public static void Main(string[] args)
    {
        Animal a = new Mammal();
        a.eat();

        Mammal m = new Mammal();
        m.eat();
    }
}