namespace RxGroup.Models.Interfaces;

public interface ITimeStamps
{
    /// <summary> Дата и время создания </summary>
    public DateTimeOffset? CreatedAt { get; set; }
    
    /// <summary> Дата и время редактирования </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}