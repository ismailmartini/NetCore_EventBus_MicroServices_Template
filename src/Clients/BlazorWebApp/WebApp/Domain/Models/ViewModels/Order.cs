﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Domain.Models.ViewModels
{
    public class Order
    {

        public string OrderNumber { get; set; }
        public DateTime Date { get; set; }

        public string Status { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; }

        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        public string ZipCode { get; set; }

        [Required]
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }
        [DisplayName("Card Holder Name")]
        public string CardHolderName { get; set; }

        public DateTime CardExpiration { get; set; }

        [DisplayName("Card Expiration")]
        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
        public string CardExpirationShort { get; set; }


        [Required]
        [DisplayName("Card Security Number")]
        public string CardSecurityNumber { get; set; }


        [Required]
        public int CardTypeId { get; set; }



    
        public string Buyer { get; set; }

        public List<OrderItem> OrderItems { get; } = new List<OrderItem>();

        public void CardExpirationShortFormat()
        {
            CardExpirationShort = CardExpiration.ToString("MM/yy");
        }
        public void CardExpirationApiFormat()
        {
            var month = CardExpirationShort.Split('/')[0];
            var year = $"20{CardExpirationShort.Split('/')[1]}";
            CardExpiration = new DateTime(int.Parse(year), int.Parse(month), 1);
        }
    }
}
