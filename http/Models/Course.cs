using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace http.Models
{
    [Table("courses")]
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string? CourseProvider { get; set; }

        public DateTime? DateCompleted { get; set; }

        public bool Completed { get; set; } = false;

        // Method to mark a course as completed
        public void MarkAsCompleted(DateTime? completedDate = null)
        {
            if (completedDate == null)
            {
                throw new ArgumentException("A completion date must be provided for past courses.");
            }

            Completed = true;
            DateCompleted = completedDate.Value;
        }
    }
}