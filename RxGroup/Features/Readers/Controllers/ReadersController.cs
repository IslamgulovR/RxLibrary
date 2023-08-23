using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RxGroup.Features.Books.Services;
using RxGroup.Features.BooksIssuance.Dto;
using RxGroup.Features.Readers.Dto;
using RxGroup.Features.Readers.Services;
using RxGroup.Models;

namespace RxGroup.Features.Readers.Controllers;

[ApiController]
[Route("api/readers")]
public class ReadersController : ControllerBase
{
    private readonly ReadersService _readersService;
    private readonly BooksService _booksService;
    private readonly IMapper _mapper;
    
    public ReadersController(ReadersService readersService, BooksService booksService, IMapper mapper)
    {
        _readersService = readersService;
        _booksService = booksService;
        _mapper = mapper;
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReaderIssuedBooksDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _readersService.GetReaderWithIssuedBooksAsync(id));
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReaderIssuedBooksDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string name = "")
    {
        return Ok(await _readersService.GetReadersByNameAsync(name));
    }

    [HttpGet("{id:guid}/history")]
    [ProducesResponseType(typeof(ReaderReaderHistoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHistory(Guid id)
    {
        return Ok(await _readersService.GetReaderHistoryAsync(id));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Reader), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(ReaderDto dto)
    {
        var entity = _mapper.Map<Reader>(dto);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var entry = await _readersService.AddAsync(entity);

        return CreatedAtAction("Get", entry.Id, entry);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Reader), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, ReaderDto dto)
    {
        var entity = _mapper.Map<Reader>(dto);
        
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        await _readersService.UpdateAsync(id, entity);

        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _readersService.DeleteAsync(id);
        
        return NoContent();
    }

    [HttpPost("issue-book")]
    [ProducesResponseType(typeof(BookIssuance), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> IssueBook(IssueBookDto dto)
    {
        if (await _readersService.IsDeletedAsync(dto.ReaderId))
            return BadRequest($"Reader {dto.ReaderId} is deleted or not found");
        
        if (await _booksService.IsDeletedAsync(dto.BookId))
            return BadRequest($"Book {dto.BookId} is deleted or not found");

        if (dto.Quantity == 0)
            return BadRequest("Quantity must be greater than 0");
        
        return Ok(await _readersService.IssueBooksAsync(dto));
    }

    [HttpPost("return-book")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReturnBook(ReturnBookDto dto)
    {
        if (await _readersService.IsDeletedAsync(dto.ReaderId))
            return BadRequest($"Reader {dto.ReaderId} is deleted or not found");
        
        if (await _booksService.IsDeletedAsync(dto.BookId))
            return BadRequest($"Book {dto.BookId} is deleted or not found");

        await _readersService.ReturnBookAsync(dto);
        
        return Ok();
    }
}