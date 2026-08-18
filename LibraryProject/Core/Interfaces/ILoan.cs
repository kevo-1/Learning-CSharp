namespace Library.Core;

public interface ILoan
{
    public void Loan(Member borrower, Book book, DateTime dueTime);
    public double Return(Member borrower, Book book, DateTime returnTime);
}