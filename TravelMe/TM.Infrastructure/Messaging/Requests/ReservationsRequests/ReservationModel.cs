using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Data.Entities.Enums;
using TM.Data.Entities;

namespace TM.Infrastructure.Messaging.Requests.ReservationsRequests
{
    public class ReservationModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TripId { get; set; }
        required public decimal Price { get; set; }

        [Required]
        required public LuggageSize LuggageSize { get; set; }

        [MaxLength(100, ErrorMessage = "Less than 100 characters!")]
        public string? Note { get; set; }
    }
}
