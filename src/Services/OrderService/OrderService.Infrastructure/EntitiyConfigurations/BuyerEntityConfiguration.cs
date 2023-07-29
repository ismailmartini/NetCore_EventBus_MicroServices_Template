using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.AggregateModels.BuyerAggregate;
using OrderService.Domain.AggregateModels.OrderAggregate;
using OrderService.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.EntitiyConfigurations
{
    internal class BuyerEntityConfiguration : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> buyerConfiguration)
        {

            buyerConfiguration.ToTable("buyers", OrderDbContext.DEFAULT_SCHEMA);

            buyerConfiguration.HasKey(b => b.Id);

            buyerConfiguration.Ignore(b => b.DomainEvents);

            buyerConfiguration.Property(b => b.Id).ValueGeneratedOnAdd();

            buyerConfiguration.Property(b => b.Name).HasColumnType("name").HasColumnType("varchar").HasMaxLength(100);

            buyerConfiguration.HasMany(b => b.PaymentMethods)
            .WithOne()
            .HasForeignKey(i => i.Id)
            .OnDelete(DeleteBehavior.Cascade);

            var navigation = buyerConfiguration.Metadata.FindNavigation(nameof(Buyer.PaymentMethods));
            //private filed use buyer.paymentMethods
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
