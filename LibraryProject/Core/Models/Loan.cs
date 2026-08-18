namespace Library.Core;

public class Loan
{
    public Member borrower;
    public Book book;
    public DateTime borrowTime;
    public DateTime deadline;

    public Loan(Member borrower, Book book, DateTime deadline)
    {
        borrowTime = DateTime.Now;
        this.borrower = borrower;
        this.book = book;
        this.deadline = deadline;
    }
}
