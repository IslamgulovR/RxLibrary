namespace RxGroup.Models;

/// <summary>
///     Выдача книги
/// </summary>
public class BookIssuance : Entity<Guid>
{
    /// <summary> Ссылка на книгу </summary>
    public Guid BookId { get; set; }
    public Book Book { get; set; }
    
    /// <summary> Ссылка на читателя </summary>
    public Guid ReaderId { get; set; }
    public Reader Reader { get; set; }
    
    /// <summary> Количество </summary>
    public int Quantity { get; set; }
    
    /// <summary> Дата и время получения </summary>
    public DateTimeOffset IssueDate { get; set; }
    
    /// <summary> Дата и время возврата </summary>
    /// <remarks> Если не заполнена, то книга у читателя </remarks>
    public DateTimeOffset? ReturnDate { get; set; }
}