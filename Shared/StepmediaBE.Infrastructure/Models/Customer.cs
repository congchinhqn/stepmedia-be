using System;
using System.Collections.Generic;

namespace FAI.Domain.Models
{
    public partial class Customer
    {
        public long CustomerId { get; protected set; }
        public string FullName { get; protected  set; }
        public DateTime DOB { get; protected  set; }
        public string Email { get; protected  set; }
    }
}
