namespace Library.Services;

using Library.Core;

public class LoanService: ILoan
{
    public void Loan(Member borrower, Book book, DateTime dueTime)
    {
        book.Borrow();
        borrower.Borrow(book);
        
        
    }

    public double Return(Member borrower, Book book, DateTime returnTime)
    {
        throw new NotImplementedException();
    }
}