using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduHome3.Models
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public DateTime CourseStart { get; set; }
        public TimeSpan CourseDuration { get; set; }
        public TimeSpan ClassDuration { get; set; }

        public int StudentsCount { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
