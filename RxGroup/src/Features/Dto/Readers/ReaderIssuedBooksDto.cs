using RxGroup.Features.Dto.Books;

namespace RxGroup.Features.Dto.Readers;

public class ReaderIssuedBooksDto : ReaderDto
{
    public List<BookDto> IssuedBooks { get; set; }
}