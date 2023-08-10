using System.ComponentModel.DataAnnotations;

namespace EduHome3.Models
{
    public class Service
    {
        public int Id { get; set; }
        [Required, StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDeactive { get; set; } //default false active deyl

    }
}
