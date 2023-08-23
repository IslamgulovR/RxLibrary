using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RxGroup.Configurations;
using RxGroup.Features.Books.Dto;
using RxGroup.Features.BooksIssuance.Dto;
using RxGroup.Features.BooksIssuance.Services;
using RxGroup.Features.Readers.Dto;
using RxGroup.Models;

namespace RxGroup.Features.Readers.Services;

public class ReadersService
{
    private readonly BooksIssuanceService _issuanceService;
    private readonly IMapper _mapper;
    private readonly PgSqlContext _database;
    
    public ReadersService(PgSqlContext database, BooksIssuanceService issuanceService, IMapper mapper)
    {
        _database = database;
        _issuanceService = issuanceService;
        _mapper = mapper;
    }

    public async Task<ReaderIssuedBooksDto> GetReaderWithIssuedBooksAsync(Guid id)
    {
        var reader = await _database.Set<Reader>()
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
        var readersList = await _database.Set<Reader>()
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
        var reader = await _database.Set<Reader>()
            .Where(reader => reader.Id == id)
            .Include(reader => reader.BooksIssuance)
            .ThenInclude(issuance => issuance.Book)
            .FirstOrDefaultAsync();

        if (reader == null)
            throw new Exception($"Reader {id} not found");

        return _mapper.Map<ReaderReaderHistoryDto>(reader);
    }

    public async Task<Reader> AddAsync(Reader entity)
    {
        var entry = await _database.Set<Reader>()
            .AddAsync(entity);
        await _database.SaveChangesAsync();

        return entry.Entity;
    }
    
    public async Task<Reader> UpdateAsync(Guid id, Reader entity)
    {
        var entry = await _database.Set<Reader>()
            .FindAsync(id);

        if (entry == null)
            throw new Exception($"Entity with Id {id} not found");
        
        _database.Entry(entry).CurrentValues.SetValues(entity);
        await _database.SaveChangesAsync();

        return entry;
    }
    
    public async Task DeleteAsync(Guid id)
    {
        if (await _issuanceService.IsReaderHasBooksAsync(id))
            throw new Exception($"Reader {id} has issued books");
        
        var entry = await _database.Set<Reader>()
            .FindAsync(id);
        
        if (entry == null)
            throw new Exception($"Entity with Id {id} not found");

        _database.Remove(entry);
        await _database.SaveChangesAsync();
    }

    public async Task<BookIssuance> IssueBooksAsync(IssueBookDto dto)
    {
        if (!await _issuanceService.IsIssueAvailableAsync(dto.BookId))
            throw new Exception("Book issuance is not available"); 
        
        var bookIssuance = _mapper.Map<BookIssuance>(dto);

        var entry = await _database.Set<BookIssuance>()
            .AddAsync(bookIssuance);
        await _database.SaveChangesAsync();

        return entry.Entity;
    }

    public async Task ReturnBookAsync(ReturnBookDto dto)
    {
        var bookIssuance = await _database.Set<BookIssuance>()
            .Where(issuance => issuance.BookId == dto.BookId
                               && issuance.ReaderId == dto.ReaderId
                               && issuance.ReturnDate == null)
            .FirstOrDefaultAsync();

        if (bookIssuance == null)
            throw new Exception("BookIssuance not found");

        bookIssuance.ReturnDate = DateTimeOffset.UtcNow;

        await _database.SaveChangesAsync();
    }
    
    public Task<bool> IsDeletedAsync(Guid id)
    {
        return _database.Set<Reader>()
            .AnyAsync(reader => reader.Id == id && reader.Deleted);
    }
}