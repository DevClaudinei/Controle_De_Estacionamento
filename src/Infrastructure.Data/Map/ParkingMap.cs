using DomainModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Map
{
    public class ParkingMap : IEntityTypeConfiguration<Parking>
    {
        public void Configure(EntityTypeBuilder<Parking> builder)
        {
            builder.ToTable("Parking");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.HasOne(x => x.Price)
                .WithMany(x => x.Parkings)
                .HasForeignKey(x => x.PriceId);

            builder.Property(x => x.Plate)
                .IsRequired();

            builder.Property(x => x.ArrivalTime)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.DepartureTime);

            builder.Property(x => x.ParkingTime);

            builder.Property(x => x.TimeToBeBilled);

            builder.Property(x => x.AmountToPay);
        }
    }
}
