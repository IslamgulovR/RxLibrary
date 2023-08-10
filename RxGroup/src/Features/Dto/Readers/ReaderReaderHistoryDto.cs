using RxGroup.Features.Dto.Books;

namespace RxGroup.Features.Dto.Readers;

/// <summary>
///     История выдачи книг читателю
/// </summary>
public class ReaderReaderHistoryDto : ReaderDto
{
    public List<BookReaderHistoryDto> BooksIssueList { get; set; } 
}