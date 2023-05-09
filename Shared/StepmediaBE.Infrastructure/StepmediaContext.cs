using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FAI.Domain.Models;

namespace StepmediaBE.Infrastructure
{
    public partial class StepmediaContext : DbContext
    {
        public StepmediaContext()
        {
        }

        public StepmediaContext(DbContextOptions<StepmediaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => new { e.BookingId, e.CustomerId, e.RestaurantId, e.TableId });

                entity.Property(e => e.BookingDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.DOB).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Table>(entity => { entity.Property(e => e.Name).HasMaxLength(30); });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}