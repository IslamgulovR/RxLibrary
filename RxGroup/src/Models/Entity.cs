using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RxGroup.Models;

public class Entity<TIdType>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TIdType Id { get; set; }   
}