using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace RichDomainModelTeam.Application.Model
{
    [Table("Team")]
    public class Team
    {
        public Team(string name, string schoolclass)
        {
            Name = name;
            Schoolclass = schoolclass;
        }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        protected Team() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(64)]
        public string Name { get; private set; }

        [MaxLength(16)]
        public string Schoolclass { get; set; }

        public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
        public virtual IReadOnlyCollection<Task> GetActiveTasks(DateTime date)
        {
            // Filter tasks where expiration date is greater than the provided date
            return Tasks.Where(task => task.ExpirationDate > date).ToList(); ;
        }
    }
}