namespace Library.Core;

public class Book
{
    public string name;
    public string author;
    public LoanStatus status;

    Book(string name, string author)
    {
        this.name = name;
        this.author = author;
        status = LoanStatus.Available;
    }

    Book(string name, string author, LoanStatus status)
    {
        this.name = name;
        this.author = author;
        this.status = status;
    }

    public void Borrow()
    {
        if (status != LoanStatus.Available)
        {
            throw new ArgumentException("This book is not available.");
        }
        status = LoanStatus.Loaned;
    }
}
