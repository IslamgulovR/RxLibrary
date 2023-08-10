using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RxGroup.Configurations;
using RxGroup.Features.Dto.Books;
using RxGroup.Features.Dto.Readers;
using RxGroup.Models;

namespace RxGroup.Features.Services;

public class BooksService : CrudService<Book>
{
    private readonly BooksIssuanceService _booksIssuanceService;
    private readonly IMapper _mapper;
    
    public BooksService(PgSqlContext database, BooksIssuanceService booksIssuanceService, IMapper mapper) : base(database)
    {
        _booksIssuanceService = booksIssuanceService;
        _mapper = mapper;
    }

    public override async Task<Book> GetAsync(Guid id)
    {
        var entry = await Database.Set<Book>()
            .Where(book => book.Id == id)
            .Include(book => book.BooksIssuance)
            .ThenInclude(issuance => issuance.Reader)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (entry == null)
            throw new Exception($"Book {id} not found");
        
        return entry;
    }

    public async Task<List<Book>> GetAsync(string name)
    {
        return await Database.Set<Book>()
            .Where(book => book.Name.ToLower().Contains(name.ToLower()))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<IssuedBookDto>> GetIssuedBooksAsync(string name = "")
    {
        var issuedBooksQuery = Database.Set<Book>()
            .Include(book => book.BooksIssuance)
            .ThenInclude(issuance => issuance.Reader)
            .Where(book => book.Deleted == false 
                           && book.BooksIssuance.Any(issuance => issuance.ReturnDate == null))
            .AsNoTracking();

        if (!string.IsNullOrEmpty(name))
            issuedBooksQuery = issuedBooksQuery
                .Where(book => book.Name.ToLower().Contains(name.ToLower()));
        
        var issuedBooksList = await issuedBooksQuery
            .ToListAsync();

        var issuedBooksListDto = new List<IssuedBookDto>();
        
        foreach (var book in issuedBooksList)
        {
            var readersList = book.BooksIssuance
                .Where(issuance => issuance.ReturnDate == null)
                .Select(issuance => issuance.Reader)
                .ToList();
            
            var bookDto = _mapper.Map<IssuedBookDto>(book);
            bookDto.Readers = _mapper.Map<List<ReaderDto>>(readersList);

            issuedBooksListDto.Add(bookDto);
        }

        return issuedBooksListDto;
    }

    public async Task<List<AvailableBookDto>> GetAvailableBooksAsync()
    {
        var availableBooksList = new List<AvailableBookDto>();
        
        var booksList = await Database.Set<Book>()
            .Where(book => book.Deleted == false)
            .AsNoTracking()
            .ToListAsync();

        foreach (var book in booksList)
        {
            var availableBook = _mapper.Map<AvailableBookDto>(book);
            availableBook.AvailableQuantity =
                book.CopiesNumber - await _booksIssuanceService.CountIssuedBooksAsync(book.Id);
            
            availableBooksList.Add(availableBook);
        }

        return availableBooksList;
    }

    public async Task<BookBookHistoryDto> GetIssueHistoryAsync(Guid id)
    {
        var book = await Database.Set<Book>()
            .Where(book => book.Id == id)
            .Include(book => book.BooksIssuance)
            .ThenInclude(issuance => issuance.Reader)
            .FirstOrDefaultAsync();

        if (book == null)
            throw new Exception($"Book {id} not found");

        return _mapper.Map<BookBookHistoryDto>(book);
    } 
    
    public override async Task<Book> UpdateAsync(Guid id, Book entity)
    {
        var book = await Database.Set<Book>()
            .FirstOrDefaultAsync(book => book.Id == id);

        if (book == null)
            throw new Exception($"Book {id} not found");

        if (book.Deleted && entity.Deleted)
            throw new Exception($"Book {id} is deleted");
        
        if (await _booksIssuanceService.IsBookIssuedAsync(id))
            throw new Exception($"Book {id} is issued");
        
        return await base.UpdateAsync(id, entity);
    }

    public override async Task DeleteAsync(Guid id)
    {
        if (await _booksIssuanceService.IsBookIssuedAsync(id))
            throw new Exception($"Book {id} is issued");
        
        await base.DeleteAsync(id);
    }
    
    public Task<bool> IsDeletedAsync(Guid id)
    {
        return Database.Set<Book>()
            .AnyAsync(reader => reader.Id == id && reader.Deleted);
    }
}