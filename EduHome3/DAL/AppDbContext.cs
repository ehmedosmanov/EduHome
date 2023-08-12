using EduHome3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduHome3.DAL
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<VideoTour> VideoTours { get; set; }
        public DbSet<NoticeBoard> NoticeBoards { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Bio> Bios { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CourseDetail> CourseDetails { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<SkillLevel> SkillLevels { get; set; }
        public DbSet<TagCourse> TagCourses { get; set; }
    }
}
