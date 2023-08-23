using RxGroup.Features.Books.Dto;

namespace RxGroup.Features.Readers.Dto;

public class ReaderIssuedBooksDto : ReaderDto
{
    public List<BookDto> IssuedBooks { get; set; }
}