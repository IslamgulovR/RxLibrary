using RxGroup.Features.Readers.Dto;

namespace RxGroup.Features.Books.Dto;

public class IssuedBookDto : BookDto
{
    public List<ReaderDto> Readers { get; set; }
}