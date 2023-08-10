using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome3.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        [Required]
        public string AuthorImg { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Role { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
