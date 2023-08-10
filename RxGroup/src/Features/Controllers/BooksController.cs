using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RxGroup.Features.Dto.Books;
using RxGroup.Features.Services;
using RxGroup.Models;

namespace RxGroup.Features.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;
    private readonly IMapper _mapper;
    
    public BooksController(BooksService booksService, IMapper mapper)
    {
        _booksService = booksService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(string name = "")
    {
        return Ok(await _booksService.GetAsync(name));
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _booksService.GetAsync(id));
    }

    [HttpGet("issued")]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIssuedBooks()
    {
        return Ok(await _booksService.GetIssuedBooksAsync());
    }

    [HttpGet("available")]
    [ProducesResponseType(typeof(List<AvailableBookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAvailableBooks()
    {
        return Ok(await _booksService.GetAvailableBooksAsync());
    }
    
    [HttpGet("{id:guid}/history")]
    [ProducesResponseType(typeof(List<Book>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBooksHistory(Guid id)
    {
        return Ok(await _booksService.GetIssueHistoryAsync(id));
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Book), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create(BookDto dto)
    {
        var entity = _mapper.Map<Book>(dto);
        
        if (!ModelState.IsValid)
            return BadRequest();

        var entry = await _booksService.AddAsync(entity);
        
        return Created($"{entry.Id}", entry);
    }
    
    [HttpPut("{id:guid}")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(Guid id, BookDto dto)
    {
        var entity = _mapper.Map<Book>(dto);
        
        await _booksService.UpdateAsync(id, entity);
        
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _booksService.DeleteAsync(id);
        
        return NoContent();
    }
}