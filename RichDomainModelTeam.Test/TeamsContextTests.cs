using Microsoft.EntityFrameworkCore;
using System.Linq;
using RichDomainModelTeam.Application.Infrastructure;
using Xunit;
using Bogus;
using RichDomainModelTeam.Application.Model;
using Task = RichDomainModelTeam.Application.Model.Task;
using System.Data;

namespace RichDomainModelTeam.Test
{
    [Collection("Sequential")]
    public class TeamsContextTests
    {
        private TeamsContext GetDatabase(bool deleteDb = false)
        {
            var db = new TeamsContext(new DbContextOptionsBuilder()
                 .UseSqlite("Data Source=RichDomainModelTeam.db")
                 .UseLazyLoadingProxies()
                 .Options);
            if (deleteDb)
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
            return db;
        }
        [Fact]
        public void CreateDatabaseSuccessTest()
        {
            using var db = GetDatabase(deleteDb: true);
        }

        [Fact]
        public void SeedDatabaseTest()
        {
            using var db = GetDatabase(deleteDb: true);
            db.Seed();
            // Multiple assert statements should be avoided in real unit tests, but in this case
            // the database is tested, not the program logic.
            Assert.True(db.Students.Count() == 10);
            Assert.True(db.Teams.Count() == 10);
            Assert.True(db.Teachers.Count() == 10);
           // Assert.True(db.HandIns.Count() == 20);
            Assert.True(db.Tasks.Count() == 10);
        }

        [Fact]
        public void GetActiveTasksSuccessTest()
        {
            {
                using var db = GetDatabase(deleteDb: true);
                db.Seed();
                var task1 = new RichDomainModelTeam.Application.Model.Task("Task1", "Title1",
                                    db.Teams.First(), db.Teachers.First(), new DateTime(2024, 7, 15), 100);
                db.Tasks.Add(task1);
                var task2 = new RichDomainModelTeam.Application.Model.Task("Task2", "Title2",
                                    db.Teams.First(), db.Teachers.First(), new DateTime(2024, 8, 1), 100);
                db.Tasks.Add(task2);
                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
            {
                using var db = GetDatabase(deleteDb: false);
                Assert.True(db.Teams.First().GetActiveTasks(new DateTime(2024, 7, 1)).Count == 2);
            }
        }

        [Fact]
        public void TryHandInSuccessTest()
        {
            {
                using var db = GetDatabase(deleteDb: true);
                db.Seed();
                var task = db.Tasks.First();
                var handIn = new HandIn(db.Students.First(), new DateTime(2023, 12, 1));
                var result = task.TryHandIn(handIn);
                db.SaveChanges();
                db.ChangeTracker.Clear();
                Assert.True(result);
            }
        }

        [Fact]
        public void TryHandInFailureTest()
        {
            {
                using var db = GetDatabase(deleteDb: true);
                db.Seed();
                var task = db.Tasks.First();
                var handIn = new HandIn(db.Students.First(), new DateTime(2024, 6, 1));
                var result = task.TryHandIn(handIn);
                db.SaveChanges();
                db.ChangeTracker.Clear();
                Assert.False(result);
            }
        }

        [Fact]
        public void ReviewHandInSuccessTest()
        {
            {
                using var db = GetDatabase(deleteDb: true);
                db.Seed();
                var task = db.Tasks.First();
                var handIn = new HandIn(db.Students.First(), new DateTime(2023, 12, 1));
                if (task.TryHandIn(handIn))
                    task.ReviewHandIn(handIn, new DateTime(2023, 12, 8), task.MaxPoints ?? 0);
                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
            {
                using var db = GetDatabase(deleteDb: false);
                Assert.True(db.Tasks.First().HandIns.OfType<ReviewedHandIn>().Count() == 1);
            }
        }

        [Fact]
        public void AveragePointsSuccessTest()
        {
            {
                using var db = GetDatabase(deleteDb: true);
                db.Seed();
                var task = db.Tasks.First();
                var handIn1 = new HandIn(db.Students.First(), new DateTime(2023, 12, 1));
                if (task.TryHandIn(handIn1))
                    task.ReviewHandIn(handIn1, new DateTime(2023, 12, 8), 10);
                var handIn2 = new HandIn(db.Students.Skip(1).First(), new DateTime(2023, 12, 15));
                if (task.TryHandIn(handIn2))
                    task.ReviewHandIn(handIn2, new DateTime(2023, 12, 8), 14);
                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
            {
                using var db = GetDatabase(deleteDb: false);
                Assert.True(db.Tasks.First().AveragePoints == 12);
            }
        }

        [Fact]
        public void FirstHandInDateSuccessTest()
        {
            {
                using var db = GetDatabase(deleteDb: true);
                db.Seed();
                var task = db.Tasks.First();
                var handIn1 = new HandIn(db.Students.First(), new DateTime(2023, 12, 1));
                task.TryHandIn(handIn1);
                var handIn2 = new HandIn(db.Students.Skip(1).First(), new DateTime(2023, 11, 30));
                task.TryHandIn(handIn2);
                var handIn3 = new HandIn(db.Students.Skip(2).First(), new DateTime(2023, 12, 5));
                task.TryHandIn(handIn3);
                db.SaveChanges();
                db.ChangeTracker.Clear();
            }
            {
                using var db = GetDatabase(deleteDb: false);
                var firstHandInDate = db.Tasks.First().FirstHandInDate;
                Assert.True(firstHandInDate.Equals(new DateTime(2023, 11, 30)));
            }
        }

        [Fact]
        public void InsertHandInFailureTest()
        {
            var updateException = false;
            try
            {
                using var db = GetDatabase(deleteDb: true);
                db.Seed();
                var task = db.Tasks.First();
                var handIn = new HandIn(db.Students.First(), new DateTime(2023, 12, 1));
                //handIn.Task = task;     // not possible because of private set
                db.HandIns.Add(handIn);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                updateException = true;
            }
            Assert.True(updateException);
        }

        [Fact]
        public void IsAdultSuccessTest()
        {
            using var db = GetDatabase(deleteDb: true);
            db.Seed();
            var student = db.Students.First();
            student.BirthDate = new DateTime(2000, 1, 1);
            Assert.True(student.IsAdult());
        }
    }
}
