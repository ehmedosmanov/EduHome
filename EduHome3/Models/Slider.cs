using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome3.Models
{
    public class Slider
    {
        public int Id { get; set; }
        [Required]
        public string Img { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsDeactive { get; set; }
        [NotMapped] //SQL DE bele tipde data saxlaya bilmiriy deye not mapped ediriy
        public IFormFile Photo { get; set; }
    }
}
