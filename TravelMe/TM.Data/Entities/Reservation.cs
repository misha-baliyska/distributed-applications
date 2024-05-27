using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TM.Data.Entities.Enums;

namespace TM.Data.Entities
{
    public class Reservation: BaseEntity
    {
        public Reservation()
        {
                
        }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TripId { get; set; }
        public virtual Trip Trip { get; set; }

        public  decimal Price { get; set; }
        //public decimal Price
        //{
        //    get { return price; }
        //    set 
        //    {
        //        if(this.LuggageSize == LuggageSize.Medium)
        //        {
        //            price = this.Trip.Price*(decimal)0.25;
        //        }
        //        else if (this.LuggageSize == LuggageSize.Large)
        //        {
        //            price = this.Trip.Price * (decimal)0.5;
        //        }
        //        else
        //        {
        //            price = this.Trip.Price;
        //        }

        //    }
        //}


        [Required]
        public LuggageSize LuggageSize { get; set; }

        [MaxLength(100)]
        public string? Note { get; set; }
    }
}
