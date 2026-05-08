using Microsoft.EntityFrameworkCore;
using StudentCrudAppWithEFCoreCodeFirst.Dto;
using StudentCrudAppWithEFCoreCodeFirst.Models;

namespace StudentCrudAppWithEFCoreCodeFirst.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Subject> Subjects { get; set; }
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasIndex(s => s.StudentName);
            modelBuilder.Entity<Student>().HasIndex(s => s.StudentMarks);
            modelBuilder.Entity<Student>().HasIndex(s => new {s.StudentMarks, s.StudentAge});

            modelBuilder.Entity<Course>().HasIndex(s => s.StudentId);

            modelBuilder.Entity<Address>().HasIndex(s => s.StudentId);

            modelBuilder.Entity<Subject>().HasIndex(s => s.SubjectName);

          
            //One to many
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.StudentId);

            //One to one
            modelBuilder.Entity<Student>()
                .HasOne(s=>s.Address)
                .WithOne(a=>a.Student)
                .HasForeignKey<Address>(a=>a.StudentId);

            //Many to many
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Subjects)
                .WithMany(su => su.Students);

            base.OnModelCreating(modelBuilder);
        }
    }
}
