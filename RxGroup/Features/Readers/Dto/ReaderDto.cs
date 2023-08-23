namespace RxGroup.Features.Readers.Dto;

public class ReaderDto
{
    public Guid Id { get; set; }
    
    public bool Deleted { get; set; }
    
    public string Name { get; set; }
    
    public DateOnly BirthDate { get; set; }
}