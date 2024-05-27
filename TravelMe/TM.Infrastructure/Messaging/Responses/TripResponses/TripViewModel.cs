using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TM.Infrastructure.Messaging.Responses.TripResponses
{
    public class TripViewModel
    {
        public int Id { get; set; }
        required public string Destination { get; set; }
        public decimal Price { get; set; }
        required public DateTime DateOfDeparture { get; set; }
        required public DateTime DateOfReturn { get; set; }
        required public int Seats { get; set; }
        required public string Description { get; set; }
    }
}
