using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatCore.Entities;

namespace TalabatRepository.Data.Configerations
{
    public class ProductConfigeration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //relation 1 to many with product type, product brand
            builder.HasOne(p => p.ProductType).WithMany();
            builder.HasOne(p => p.ProductBrand).WithMany();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p=> p.Price).HasColumnType("decimal(18,2)");
        }
    }
}
