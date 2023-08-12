namespace EduHome3.Models
{
    public class TagCourse
    {
        public int Id { get; set; }
        public Course Course { get; set;}
        public int CourseId { get; set;}

        public Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
