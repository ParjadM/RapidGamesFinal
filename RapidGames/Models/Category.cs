using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RapidGames.Models 
{
    public class Category
    {
        [Key] 
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public virtual ICollection<CategoryGames> CategoryGames { get; set; } = new List<CategoryGames>();
    }
}