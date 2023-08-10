using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RxGroup.Configurations;
using RxGroup.Features.Dto.Books;
using RxGroup.Features.Dto.Issues;
using RxGroup.Features.Dto.Readers;
using RxGroup.Models;

namespace RxGroup.Features.Services;

public class ReadersService : CrudService<Reader>
{
    private readonly BooksIssuanceService _issuanceService;
    private readonly IMapper _mapper;
    
    public ReadersService(PgSqlContext database, BooksIssuanceService issuanceService, IMapper mapper) : base(database)
    {
        _issuanceService = issuanceService;
        _mapper = mapper;
    }

    public async Task<ReaderIssuedBooksDto> GetReaderWithIssuedBooksAsync(Guid id)
    {
        var reader = await Database.Set<Reader>()
            .Where(reader => reader.Id == id)
            .Include(reader => reader.BooksIssuance.Where(issuance => issuance.ReturnDate == null))
            .ThenInclude(issuance => issuance.Book)
            .FirstOrDefaultAsync();

        if (reader == null)
            throw new Exception($"Reader {id} not found");
        
        var booksList = reader.BooksIssuance?
            .Select(issuance => issuance.Book)
            .ToList();

        var readerDto = _mapper.Map<ReaderIssuedBooksDto>(reader);
        readerDto.IssuedBooks = _mapper.Map<List<BookDto>>(booksList);

        return readerDto;
    }

    public async Task<List<ReaderIssuedBooksDto>> GetReadersByNameAsync(string name = "")
    {
        var readersList = await Database.Set<Reader>()
            .Where(reader => reader.Name.ToLower().Contains(name.ToLower()))
            .Include(reader => reader.BooksIssuance.Where(issuance => issuance.ReturnDate == null))
            .ThenInclude(issuance => issuance.Book)
            .AsNoTracking()
            .ToListAsync();

        var readersWithBooksList = new List<ReaderIssuedBooksDto>();
        
        foreach (var reader in readersList)
        {
            var booksList = reader.BooksIssuance?
                .Select(issuance => issuance.Book)
                .ToList();
            
            var readerDto = _mapper.Map<ReaderIssuedBooksDto>(reader);
            readerDto.IssuedBooks = _mapper.Map<List<BookDto>>(booksList);
            
            readersWithBooksList.Add(readerDto);
        }

        return readersWithBooksList;
    }

    public async Task<ReaderReaderHistoryDto> GetReaderHistoryAsync(Guid id)
    {
        var reader = await Database.Set<Reader>()
            .Where(reader => reader.Id == id)
            .Include(reader => reader.BooksIssuance)
            .ThenInclude(issuance => issuance.Book)
            .FirstOrDefaultAsync();

        if (reader == null)
            throw new Exception($"Reader {id} not found");

        return _mapper.Map<ReaderReaderHistoryDto>(reader);
    }
    
    public override async Task DeleteAsync(Guid id)
    {
        if (await _issuanceService.IsReaderHasBooksAsync(id))
            throw new Exception($"Reader {id} has issued books");
        
        await base.DeleteAsync(id);
    }

    public async Task<BookIssuance> IssueBooksAsync(IssueBookDto dto)
    {
        if (!await _issuanceService.IsIssueAvailableAsync(dto.BookId))
            throw new Exception("Book issuance is not available"); 
        
        var bookIssuance = _mapper.Map<BookIssuance>(dto);

        var entry = await Database.Set<BookIssuance>()
            .AddAsync(bookIssuance);
        await Database.SaveChangesAsync();

        return entry.Entity;
    }

    public async Task ReturnBookAsync(ReturnBookDto dto)
    {
        var bookIssuance = await Database.Set<BookIssuance>()
            .Where(issuance => issuance.BookId == dto.BookId
                               && issuance.ReaderId == dto.ReaderId
                               && issuance.ReturnDate == null)
            .FirstOrDefaultAsync();

        if (bookIssuance == null)
            throw new Exception("BookIssuance not found");

        bookIssuance.ReturnDate = DateTimeOffset.UtcNow;

        await Database.SaveChangesAsync();
    }
    
    public Task<bool> IsDeletedAsync(Guid id)
    {
        return Database.Set<Reader>()
            .AnyAsync(reader => reader.Id == id && reader.Deleted);
    }
}