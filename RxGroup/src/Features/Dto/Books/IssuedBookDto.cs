using RxGroup.Features.Dto.Readers;

namespace RxGroup.Features.Dto.Books;

public class IssuedBookDto : BookDto
{
    public List<ReaderDto> Readers { get; set; }
}