namespace RxGroup.Models.Interfaces;

public interface IDeletable
{
    /// <summary> Объект удален </summary>
    public bool Deleted { get; set; }
}