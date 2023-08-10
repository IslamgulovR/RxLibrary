namespace RxGroup.Features.Dto.Books;

public class BookDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public bool Deleted { get; set; }
    
    public string Author { get; set; }
    
    public string VendorCode { get; set; }
    
    public int PublishingYear { get; set; }
    
    public int CopiesNumber { get; set; }
}