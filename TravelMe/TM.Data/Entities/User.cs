using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TM.Data.Entities.Enums;

namespace TM.Data.Entities
{
    public class User : BaseEntity
    {
        public User()
        {
            Reservations = new HashSet<Reservation>();  
        }

        [Required]
        [MaxLength(25)]
        public string Username { get; set; }

        [Required]
        [MaxLength(25)]
        [MinLength(6)]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        [Required]
        [MaxLength(20)]
        required public string FirstName { get; set; }

        [MaxLength(20)]
        [Required]
        required public string LastName { get; set; }

        [MaxLength(25)]
        [Required]
        required public string Email { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Reservation>? Reservations { get; set; }

    }
}
