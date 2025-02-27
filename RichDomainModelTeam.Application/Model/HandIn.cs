using Castle.Core.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichDomainModelTeam.Application.Model
{
    [Table("HandIn")]
    public class HandIn
    {
        public HandIn(Student student, DateTime date)
        {
            Student = student;
            StudentId = student.Id;
            Date = date;
            Guid = Guid.NewGuid();
            Task = default!;
        }

        protected HandIn(HandIn handIn)
        {
            Id = handIn.Id;
            Guid = handIn.Guid;
            Student = handIn.Student;
            StudentId = handIn.StudentId;
            Task = handIn.Task;
            TaskId = handIn.TaskId;
            Date = handIn.Date;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected HandIn() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int Id { get; private set; }
        public Guid Guid { get; private set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int TaskId { get; set; }
        public virtual Task Task { get; private set; }

        public DateTime Date { get; set; }
    }
}