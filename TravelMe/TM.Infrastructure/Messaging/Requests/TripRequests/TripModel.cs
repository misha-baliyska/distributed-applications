using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Requests.TripRequests
{
    public class TripModel
    {
        public int Id { get; set; }

        public decimal Price { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Less than 20 characters!")]

        required public string Destination { get; set; }
        required public DateTime DateOfDeparture { get; set; }
        required public DateTime DateOfReturn { get; set; }
        required public int Seats { get; set; }

        [Required]
        [MaxLength(300, ErrorMessage = "Less than 300 characters!")]
        required public string Description { get; set; }
    }
}
