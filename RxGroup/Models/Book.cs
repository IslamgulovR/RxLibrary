using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RxGroup.Models.Interfaces;

namespace RxGroup.Models;

/// <summary>
///     Книга
/// </summary>
public class Book : ITimeStamps, IDeletable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    /// <summary> Название </summary>
    public string Name { get; set; }
    
    public bool Deleted { get; set; }
    
    /// <summary> Автор </summary>
    public string Author { get; set; }
    
    /// <summary> Артикул </summary>
    public string VendorCode { get; set; }
    
    /// <summary> Год издания </summary>
    public int PublishingYear { get; set; }
    
    /// <summary> Количество копий </summary>
    public int CopiesNumber { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public List<BookIssuance> BooksIssuance { get; set; } = new();
}