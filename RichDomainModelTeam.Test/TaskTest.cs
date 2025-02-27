using RichDomainModelTeam.Application.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace RichDomainModelTeam.Test
{
    public class TaskTests
    {
        public TaskTests()
        {
            var teacher = new Teacher(new Name("Name", "Surname", "email"));
            var student = new Student(new Name("Name", "Surname", "email"));
            var handIn = new HandIn(student, new DateTime(2021, 1, 1));
            var team = new Team("Team Name", "Welcome");
            var task = new RichDomainModelTeam.Application.Model.Task("Test Subject", "Test Title",
                team, teacher,
                new DateTime(2021, 2, 1), 100);
        }

        [Fact]
        public void CreateTaskSuccessTest()
        {
            var teacher = new Teacher(new Name("Name", "Surname", "email"));
            var student = new Student(new Name("Name", "Surname", "email"));
            var handIn = new HandIn(student, new DateTime(2021, 1, 1));
            var team = new Team("Team Name", "Welcome");
            var task = new RichDomainModelTeam.Application.Model.Task("Test Subject", "Test Title",
                 team, teacher,
                 new DateTime(2021, 2, 1), 100);

            Assert.NotNull(task);
        }

        [Fact]
        public void TryHandInSuccessTest()
        {
            var teacher = new Teacher(new Name("Name", "Surname", "email"));
            var student = new Student(new Name("Name", "Surname", "email"));
            var handIn = new HandIn(student, new DateTime(2021, 1, 1));
            var team = new Team("Team Name", "Welcome");
            var task = new RichDomainModelTeam.Application.Model.Task("Test Subject", "Test Title",
                   team, teacher,
                   new DateTime(2026, 12, 1), 100);

            var result = task.TryHandIn(handIn);

            Assert.True(result);
        }

        [Fact]
        public void TryHandInFailureTest()
        {
            var teacher = new Teacher(new Name("Name", "Surname", "email"));
            var student = new Student(new Name("Name", "Surname", "email"));
            var handIn = new HandIn(student, new DateTime(2021, 3, 1));
            var team = new Team("Team Name", "Welcome");
            var task = new RichDomainModelTeam.Application.Model.Task("Test Subject", "Test Title",
                  team, teacher,
                  new DateTime(2021, 2, 1), 100);

            var result = task.TryHandIn(handIn);

            Assert.False(result);
        }

        [Fact]
        public void ReviewHandInSuccessTest()
        {
            var teacher = new Teacher(new Name("Name", "Surname", "email"));
            var student = new Student(new Name("Name", "Surname", "email"));
            var handIn = new HandIn(student, new DateTime(2021, 1, 1));
            var team = new Team("Team Name", "Welcome");
            var task = new RichDomainModelTeam.Application.Model.Task("Test Subject", "Test Title",
                  team, teacher,
                  new DateTime(2021, 2, 1), 100);

            task.ReviewHandIn(handIn, new DateTime(2021, 1, 2), 10);

            Assert.True(task.HandIns.Count == 1);
        }

      

        [Fact]
        public void FirstHandInDateSuccessTest()
        {
            var teacher = new Teacher(new Name("Name", "Surname", "email"));
            var student = new Student(new Name("Name", "Surname", "email"));
            var handIn = new HandIn(student, new DateTime(2021, 1, 1));
            var team = new Team("Team Name", "Welcome");
            var task = new RichDomainModelTeam.Application.Model.Task("Test Subject", "Test Title",
                 team, teacher,
                 new DateTime(2021, 2, 1), 100);
            var HandIn2 = new HandIn(student, new DateTime(2021, 1, 25));

            task.ReviewHandIn(handIn, new DateTime(2021, 1, 2), 10);
            task.ReviewHandIn(HandIn2, new DateTime(2021, 1, 2), 10);

            Assert.True(task.FirstHandInDate == new DateTime(2021, 1, 1));
        }


    }
}
