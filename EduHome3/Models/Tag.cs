using System.Collections.Generic;

namespace EduHome3.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeactive { get; set; }
        public List<TagCourse> TagCourses { get; set; }

    }
}
