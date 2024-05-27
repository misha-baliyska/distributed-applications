using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Data.Entities
{
    public class Trip: BaseEntity
    {
        public Trip()
        {
            Reservations = new HashSet<Reservation>();
        }

        [Required]
        [MaxLength(20)]
        public string Destination { get; set; }

        public decimal Price { get; set; }

        [Required]
        public DateTime DateOfDeparture { get; set; }
        [Required]
        public DateTime DateOfReturn { get; set; }
        [Required]
        public int Seats { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        public virtual ICollection<Reservation>? Reservations { get; set; }
    }
}
