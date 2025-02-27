using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using RichDomainModelTeam.Application.Model;
using Task = RichDomainModelTeam.Application.Model.Task;

namespace RichDomainModelTeam.Application.Infrastructure
{
    public class TeamsContext : DbContext
    {
        public TeamsContext(DbContextOptions opt) : base(opt) { }

        public DbSet<HandIn> HandIns => Set<HandIn>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<RichDomainModelTeam.Application.Model.Task> Tasks => Set<RichDomainModelTeam.Application.Model.Task>();
        public DbSet<Teacher> Teachers => Set<Teacher>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<ReviewedHandIn> ReviewedHandIns => Set<ReviewedHandIn>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HandIn>().ToTable("HandIn"); // here is the alternative way of " [Table("HandIn")]" in the class
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<Student>().ToTable("Student");
           

            modelBuilder.Entity<HandIn>().HasAlternateKey(h => h.Guid);
            modelBuilder.Entity<Teacher>().HasAlternateKey(t => t.Guid);
            modelBuilder.Entity<Student>().HasAlternateKey(s => s.Guid);
            modelBuilder.Entity<RichDomainModelTeam.Application.Model.Task>().HasAlternateKey(t => t.Guid);

            modelBuilder.Entity<RichDomainModelTeam.Application.Model.Task>().Property(t => t.Guid).ValueGeneratedOnAdd();
            modelBuilder.Entity<Teacher>().Property(t => t.Guid).ValueGeneratedOnAdd();
            modelBuilder.Entity<Student>().Property(s => s.Guid).ValueGeneratedOnAdd();

            modelBuilder.Entity<Student>().OwnsOne(s => s.Name);
            modelBuilder.Entity<Teacher>().OwnsOne(t => t.Name);

        }


        public void Seed()
        {
            Randomizer.Seed = new Random(2145);

            var teachers = new Faker<Teacher>("de").CustomInstantiator(f => new Teacher(
                name: new Name(f.Name.FirstName(), f.Name.LastName(), f.Internet.Email())))
                .Generate(10)
                .ToList();
            Teachers.AddRange(teachers); SaveChanges();

            var students = new Faker<Student>("de").CustomInstantiator(f => new Student(
                name: new Name(f.Name.FirstName(), f.Name.LastName(), f.Internet.Email())))
                .Generate(10)
                .ToList();
            Students.AddRange(students); SaveChanges();

            var teams = new Faker<Team>("de").CustomInstantiator(f => new Team(
                name: f.Commerce.ProductName(),
                schoolclass: $"{f.Random.Int(1, 5)}{f.Random.String2(1, "ABC")}HIF"))
                .Generate(10)
                .ToList();
            Teams.AddRange(teams); SaveChanges();

            var tasks = new Faker<Task>("de").CustomInstantiator(f => new Task(
                subject: f.Commerce.ProductMaterial(),
                title: f.Commerce.ProductAdjective(),
                team: f.Random.ListItem(teams),
                teacher: f.Random.ListItem(teachers),
                expirationDate: new DateTime(2024, 1, 1).AddDays(f.Random.Int(0, 4 * 30))))
                .Rules((f, t) => t.MaxPoints = f.Random.Int(16, 48).OrNull(f, 0.5f))
                .Generate(10)
                .ToList();
            Tasks.AddRange(tasks); SaveChanges();
        }
    }
}
