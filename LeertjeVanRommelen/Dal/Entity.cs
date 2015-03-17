using System.ComponentModel.DataAnnotations;

namespace LeertjeVanRommelen.Dal
{
    internal abstract class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}