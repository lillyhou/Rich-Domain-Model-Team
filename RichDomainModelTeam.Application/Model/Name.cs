using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichDomainModelTeam.Application.Model
{
    public record Name ([MaxLength(80)] string Firstname, [MaxLength(80)]string Lastname, [MaxLength(280)]string Email);
    
}
