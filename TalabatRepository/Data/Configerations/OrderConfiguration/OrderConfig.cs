using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities.Order;

namespace TalabatRepository.Data.Configerations.OrderConfiguration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status);

            builder.Property(o => o.Status)
                    .HasConversion(
                        Ostatus => Ostatus.ToString(),
                        Ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), Ostatus)
                    );

            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

            builder.Property(p => p.Subtotal)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
