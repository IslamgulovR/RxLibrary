namespace RxGroup.Features.Dto.Readers;

public class ReaderDto
{
    public Guid Id { get; set; }
    
    public bool Deleted { get; set; }
    
    public string Name { get; set; }
    
    public DateTimeOffset BirthDate { get; set; }
}