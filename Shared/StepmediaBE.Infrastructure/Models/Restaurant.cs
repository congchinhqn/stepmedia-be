using System;
using System.Collections.Generic;

namespace FAI.Domain.Models
{
    public partial class Restaurant
    {
        public long RestaurantId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
