using System;
using System.Linq;
using Xunit;
using RichDomainModelTeam.Application.Model;
using RichDomainModelTeam.Test;
using Microsoft.EntityFrameworkCore;

namespace RichDomainModelTeam.Test
{
    public class HandInTests : DatabaseTest
    {
        /// <summary>
        /// This class should be deleted! Logic could be wrong
        /// </summary>
        public HandInTests()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            // Create a student and add it to the database
            var student = new Student(new Name("John", "Doe", "john.doe@example.com"));
            _db.Students.Add(student);

            // Create a HandIn for the student and add it to the database
            var handIn = new HandIn(student, new DateTime(2021, 5, 1));
            _db.HandIns.Add(handIn);

            _db.SaveChanges();
        }

        [Fact]
        public void HandInCreationTest()
        {
            var handIn = _db.HandIns.First();

            // Assert that the handIn has been created successfully
            Assert.NotNull(handIn);
            Assert.Equal(1, _db.HandIns.Count());  // Verify only one HandIn exists
            Assert.Equal("John", handIn.Student.Name.Firstname);
            Assert.Equal(new DateTime(2021, 5, 1), handIn.Date);  // Verify the handIn date
        }

        [Fact]
        public void StudentAssociatedWithHandInTest()
        {
            // Retrieve the handIn from the database with the associated Student loaded
            var handIn = _db.HandIns.Include(h => h.Student).FirstOrDefault();

            // Assert that the HandIn is associated with a Student
            Assert.NotNull(handIn);
            Assert.NotNull(handIn.Student);  // Ensure the Student is loaded
            Assert.Equal("John", handIn.Student.Name.Firstname);  // Verify the student's name
        }



        [Fact]
        public void HandInDateTest()
        {
            var handIn = _db.HandIns.First();
            Assert.Equal(new DateTime(2021, 5, 1), handIn.Date);
        }
    }
}
