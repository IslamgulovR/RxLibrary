using RxGroup.Features.Dto.Readers;

namespace RxGroup.Features.Dto.Books;

public class BookBookHistoryDto : BookDto
{
    public List<ReaderBookHistoryDto> ReadersList { get; set; }
}