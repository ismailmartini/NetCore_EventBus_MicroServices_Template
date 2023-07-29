using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntitiyConfigurations
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders", OrderDbContext.DEFAULT_SCHEMA);
            builder.HasKey(o => o.Id);
            builder.Property(i => i.Id).ValueGeneratedOnAdd();
            builder.Ignore(i => i.DomainEvents); //domaing events ignore

            //dont need address table ex: Order_street,Order_city in Order Table
            builder
            .OwnsOne(o => o.Address, a =>
            {
                a.WithOwner();

            });

            //backing field 
            builder
            .Property<int>("orderStatusId")
            .UsePropertyAccessMode(PropertyAccessMode.Field).HasColumnName("OrderStatusId")
            .IsRequired();



            var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));            
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(o => o.Buyer)
            .WithMany().HasForeignKey(i
            => i.BuyerId);

            builder.HasOne(o => o.OrderStatus)
            .WithMany()
            .HasForeignKey("orderStatusId");
        }
    }
}
