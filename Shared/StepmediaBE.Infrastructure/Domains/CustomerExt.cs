using System;
using System.Collections.Generic;

namespace FAI.Domain.Models
{
    public partial class Customer
    {
        public Customer()
        {
        }
        
        public void SetFullName(string value)
        {
            FullName = value;
        }
        
        public void SetDOB(DateTime value)
        {
            DOB = value;
        }
        
        public void SetEmail(string value)
        {
            Email = value;
        }
    }
}
