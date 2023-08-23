using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RxGroup.Models.Interfaces;

namespace RxGroup.Models;

/// <summary>
///     Читатель
/// </summary>
public class Reader : ITimeStamps, IDeletable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    /// <summary> Фио </summary>
    public string Name { get; set; }

    public bool Deleted { get; set; }
    
    /// <summary> Дата рождения </summary>
    public DateOnly BirthDate { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public List<BookIssuance> BooksIssuance { get; set; } = new();
}