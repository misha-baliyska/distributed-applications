using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TM.Data.Entities.Enums;

namespace TM.Infrastructure.Messaging.Responses.ReservationsResponses
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TripId { get; set; }
        required public decimal Price { get; set; }
        required public LuggageSize LuggageSize { get; set; }
        public string? Note { get; set; }
    }
}
