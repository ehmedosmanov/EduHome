using System.Collections.Generic;

namespace EduHome3.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Course> Courses { get; set; } 
        public bool IsDeactive { get; set; }
    }
}