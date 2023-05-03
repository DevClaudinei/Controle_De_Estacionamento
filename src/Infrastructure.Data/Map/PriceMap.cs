using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Map
{
    public class PriceMap : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.ToTable("Price");

            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Parkings)
                .WithOne(x => x.Price)
                .HasForeignKey(x => x.PriceId);

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.CurrentValue)
                .IsRequired();

            builder.Property(x => x.InitialTerm)
                .IsRequired();

            builder.Property(x => x.FinalTerm)
                .IsRequired();
        }
    }
}
