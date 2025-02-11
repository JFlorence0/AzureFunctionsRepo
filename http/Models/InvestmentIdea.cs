using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace http.Models
{
    [Table("investment_ideas")]
    public class InvestmentIdea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        [MaxLength(10)]
        public string Ticker { get; set; } = "N/A";

        [Required]
        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key to link to InvestmentTheme
        [ForeignKey("InvestmentTheme")]
        public int InvestmentThemeId { get; set; }
        
        public required InvestmentTheme InvestmentTheme { get; set; }
    }
}
