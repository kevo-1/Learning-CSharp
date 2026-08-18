namespace Library.Core;

public class Member {
    public string name;
    public string membershipID;
    public List<Book> borrowedBooks;
    public double currentDebt;

    public Member(string name, string Id)
    {
        this.name = name;
        membershipID = Id;
        borrowedBooks = [];
    }

    public Member(string name, string Id, List<Book> currentBooks)
    {
        this.name = name;
        membershipID = Id;
        //this.loans = new List<Loan>(oldLoans);
        borrowedBooks = currentBooks;
    }

    public void Borrow(Book book)
    {
        if(currentDebt >= 100.0)
        {
            throw new ArgumentException("Member's current debt is higher than 100$, Can't borrow book");
        }

        borrowedBooks.Add(book);
    }
}
