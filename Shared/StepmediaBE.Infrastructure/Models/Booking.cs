using System;
using System.Collections.Generic;

namespace FAI.Domain.Models
{
    public partial class Booking
    {
        public long BookingId { get; set; }
        public long CustomerId { get; set; }
        public long RestaurantId { get; set; }
        public long TableId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
