﻿using OrderService.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;
namespace OrderService.Domain.AggregateModels.OrderAggregate
{
    public class OrderItem : BaseEntity//, IValidatableObject
    {

        public int ProductId { get;set; }
        public string ProductName { get;set; }
        public string PictureUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }



        public OrderItem()
        {
            
        }

        public OrderItem(int productId,string productName,decimal unitPrice,string pictureUrl,int units=1)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            PictureUrl = pictureUrl;
            Units = units;

        }


    
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var results = new List<ValidationResult>();

        //    if (Units <= 0)
        //    {
        //        results.Add(new ValidationResult("Invalid numver of units", new[] { "Units" }));
        //        return results;
        //    }
        //}

        
    }
}
