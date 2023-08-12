using System.Collections.Generic;

namespace EduHome3.Models
{
    public class Category
    {      
            public int Id { get; set; }
            public string Name { get; set; }
            public int CategoryCount { get; set; }
            public bool IsDeactive { get; set; }
            public List<Course> Courses { get; set; }
    }
}
