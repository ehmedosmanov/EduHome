using System.Collections.Generic;

namespace EduHome3.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
        public decimal Price { get; set; }

        public SkillLevel SkillLevel { get; set; }
        public int? SkillLevelId { get; set; }

        public Language Language { get; set; }
        public int? LanguageId { get; set; }

        public List<TagCourse> TagCourses { get; set; }
        public bool IsDeactive { get; set; }

        public CourseDetail CourseDetail { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
