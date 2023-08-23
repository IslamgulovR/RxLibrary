using RxGroup.Features.Books.Dto;

namespace RxGroup.Features.Readers.Dto;

/// <summary>
///     История выдачи книг читателю
/// </summary>
public class ReaderReaderHistoryDto : ReaderDto
{
    public List<BookReaderHistoryDto> BooksIssueList { get; set; } 
}