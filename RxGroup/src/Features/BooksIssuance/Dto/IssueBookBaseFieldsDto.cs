using System.ComponentModel.DataAnnotations;

namespace RxGroup.Features.BooksIssuance.Dto;

public class IssueBookBaseFieldsDto
{
    [Required]
    public Guid BookId { get; set; }
    
    [Required]
    public Guid ReaderId { get; set; }
}