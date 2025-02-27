using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichDomainModelTeam.Application.Model
{
    public class Student
    {
        public Student(Name name)
        {
            Name = name;
            Guid = Guid.NewGuid();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected Student() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public int Id { get; set; }
        public Guid Guid { get; private set; }

        public Name Name { get; set; }
        public DateTime BirthDate{ get; set; }

        public virtual ICollection<HandIn> HandIns { get; } = new List<HandIn>(); //virtual for lazy loading
        public bool IsAdult() 
        {
            return DateTime.Now.AddYears(-18) > BirthDate;
        }
    }
}
