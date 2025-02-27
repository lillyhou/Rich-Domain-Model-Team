using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichDomainModelTeam.Application.Model
{
    [Table("Task")]
    public class Task
    {
        public Task(string subject, string title, Team team, Teacher teacher, DateTime expirationDate, int? maxPoints = null)
        {
            Subject = subject;
            Title = title;
            Team = team;
            TeamName = team.Name;
            Teacher = teacher;
            TeacherId = teacher.Id;
            ExpirationDate = expirationDate;
            MaxPoints = maxPoints;
            Guid = Guid.NewGuid();
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected Task() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int Id { get; private set; }
        public Guid Guid { get; private set; }
        [MaxLength(255)]
        public string Subject { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }

        public string TeamName { get; set; }
        public virtual Team Team { get; set; }

        public int TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }

        public DateTime ExpirationDate { get; set; }
        public int? MaxPoints { get; set; }

        protected List<HandIn> _handIns = new();
        public virtual IReadOnlyCollection<HandIn> HandIns => _handIns;

        public decimal AveragePoints => (decimal)HandIns.OfType<ReviewedHandIn>().Average(h => h.Points);
        public DateTime FirstHandInDate => HandIns.Min(h => h.Date);

        public bool TryHandIn(HandIn handIn)
        {
            if (handIn.Date <= ExpirationDate)
            {
                _handIns.Add(handIn);
                return true;
            }
            return false;
        }

        public void ReviewHandIn(HandIn handIn, DateTime reviewDate, int points)
        {
            var reviewedHandIn = handIn as ReviewedHandIn;
            if (reviewedHandIn is not null)
            {
                reviewedHandIn.ReviewDate = reviewDate;
                reviewedHandIn.Points = points;
            }
            else
            {
                reviewedHandIn = new ReviewedHandIn(handIn, reviewDate, points);
                _handIns.Remove(handIn);
                _handIns.Add(reviewedHandIn);
            }
        }
    }
}
