using System.ComponentModel.DataAnnotations;

namespace RxGroup.Features.Dto.Issues;

public class IssueBookBaseFieldsDto
{
    [Required]
    public Guid BookId { get; set; }
    
    [Required]
    public Guid ReaderId { get; set; }
}