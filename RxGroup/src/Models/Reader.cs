using RxGroup.Models.Interfaces;

namespace RxGroup.Models;

/// <summary>
///     Читатель
/// </summary>
public class Reader : Entity<Guid>, ITimeStamps, IDeletable
{
    /// <summary> Фио </summary>
    public string Name { get; set; }

    public bool Deleted { get; set; }
    
    /// <summary> Дата рождения </summary>
    public DateTimeOffset BirthDate { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public ICollection<BookIssuance> BooksIssuance { get; set; }
}