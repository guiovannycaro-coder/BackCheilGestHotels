
using BackCheilGestHotels.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BackCheilGestHotels.infra.Persistence
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }

        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("geshhotels", "public");

                entity.HasKey(h => h.HotId);

                entity.Property(h => h.HotId).HasColumnName("hotid");
                entity.Property(h => h.HotName).HasColumnName("hotname");
                entity.Property(h => h.HotDescription).HasColumnName("hotdescription");
                entity.Property(h => h.HotAddress).HasColumnName("hotaddress");
                entity.Property(h => h.HotPhone).HasColumnName("hotphone");
                entity.Property(h => h.HotEmail).HasColumnName("hotemail");
                entity.Property(h => h.HotCityId).HasColumnName("hotcityid");
                entity.Property(h => h.HotRating).HasColumnName("hotrating");
                entity.Property(h => h.HotRateSign).HasColumnName("hotratesign");
                entity.Property(h => h.HotIsActive).HasColumnName("hotisactive");
                entity.Property(h => h.hotImages).HasColumnName("hotimages");

                entity.HasOne(h => h.City)
                    .WithMany()
                    .HasForeignKey(h => h.HotCityId);
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("geshcity", "public");

                entity.HasKey(c => c.CityId);

                entity.Property(c => c.CityId).HasColumnName("cityid");
                entity.Property(c => c.CityName).HasColumnName("cityname");
                entity.Property(c => c.CityidDepartament)
                    .HasColumnName("cityiddepartament");
            });
        }
    }
}