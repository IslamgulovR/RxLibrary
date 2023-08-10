namespace RxGroup.Features.Dto.Books;

public class BookReaderHistoryDto
{
    public BookDto Book { get; set; }
    
    public int Quantity { get; set; }
    
    public DateTimeOffset IssueDate { get; set; }
    
    public DateTimeOffset? ReturnDate { get; set; }
}