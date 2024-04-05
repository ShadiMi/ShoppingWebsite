﻿using System.ComponentModel.DataAnnotations;

namespace ShoppingWebsite.Models
{
    public class Customer
    {
        [Key]
        public string CustomerID { get; set; }
        public string ContactName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
