using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichDomainModelTeam.Application.Model
{
    
    public class ReviewedHandIn : HandIn
    {
        public ReviewedHandIn(HandIn handIn, DateTime reviewDate, int points): base(handIn)
        {
            ReviewDate = reviewDate;
            Points = points;
        }
        
        protected ReviewedHandIn() { } 
        public DateTime ReviewDate { get; set; }
        public int Points { get; set; }
    }
}
