using RxGroup.Features.Readers.Dto;

namespace RxGroup.Features.Books.Dto;

public class BookBookHistoryDto : BookDto
{
    public List<ReaderBookHistoryDto> ReadersList { get; set; }
}