namespace RxGroup.Features.Readers.Dto;

public class ReaderBookHistoryDto
{
    public ReaderDto Reader { get; set; }
    
    public int Quantity { get; set; }
    
    public DateTimeOffset IssueDate { get; set; }
    
    public DateTimeOffset? ReturnDate { get; set; }
}