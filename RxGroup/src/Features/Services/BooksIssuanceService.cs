using Microsoft.EntityFrameworkCore;
using RxGroup.Configurations;
using RxGroup.Models;

namespace RxGroup.Features.Services;

public class BooksIssuanceService
{
    private readonly PgSqlContext _database;

    public BooksIssuanceService(PgSqlContext database)
    {
        _database = database;
    }

    /// <summary>
    ///     Книга выдана
    /// </summary>
    /// <param name="bookId"> Идентификатор книги </param>
    public Task<bool> IsBookIssuedAsync(Guid bookId)
    {
        return _database.Set<BookIssuance>()
            .AnyAsync(issuance => issuance.BookId == bookId && issuance.ReturnDate == null);
    }

    /// <summary>
    ///     У читателя есть выданные книги
    /// </summary>
    /// <param name="readerId"> Идентификатор книги </param>
    public Task<bool> IsReaderHasBooksAsync(Guid readerId)
    {
        return _database.Set<BookIssuance>()
            .AnyAsync(issuance => issuance.ReaderId == readerId && issuance.ReturnDate == null);
    }

    /// <summary>
    ///     Количество книг на руках у читателей
    /// </summary>
    /// <param name="bookId"> Идентификатор книги </param>
    public Task<int> CountIssuedBooksAsync(Guid bookId)
    {
        return _database.Set<BookIssuance>()
            .Where(issuance => issuance.ReturnDate == null)
            .Select(issuance => issuance.Quantity)
            .CountAsync();
    }

    /// <summary>
    ///     Книга доступна для выдачи
    /// </summary>
    /// <param name="bookId"> Идентификатор книги </param>
    public async Task<bool> IsIssueAvailableAsync(Guid bookId)
    {
        var availableBooks = await _database.Set<Book>()
            .Where(book => book.Id == bookId && book.Deleted == false)
            .Select(book => book.CopiesNumber - book.BooksIssuance.Count(issuance => issuance.ReturnDate == null))
            .FirstOrDefaultAsync();

        return availableBooks > 0;
    }
}