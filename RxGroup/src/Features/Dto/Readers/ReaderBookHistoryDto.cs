namespace RxGroup.Features.Dto.Readers;

public class ReaderBookHistoryDto
{
    public ReaderDto Reader { get; set; }
    
    public int Quantity { get; set; }
    
    public DateTimeOffset IssueDate { get; set; }
    
    public DateTimeOffset? ReturnDate { get; set; }
}