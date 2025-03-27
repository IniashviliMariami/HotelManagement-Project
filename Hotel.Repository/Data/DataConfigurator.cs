using Microsoft.EntityFrameworkCore;
using Hotels.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Repository.Data
{
    public static class DataConfigurator
    {
        public static void ConfigureGuests(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

                entity.Property(g => g.FirstName)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(g => g.LastName)
               .IsRequired()
               .HasMaxLength(50);

                entity.Property(g => g.PersonalNumber)
                .IsRequired()
                .HasColumnType("CHAR(11)")
                .HasMaxLength(11)
                .IsUnicode(false);

                entity.Property(g => g.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnType("VARCHAR(15)")
                .IsUnicode(false);

                entity
                .HasMany(g => g.GuestReservations)
                .WithOne(g => g.Guest)
                .HasForeignKey(g => g.GuestId);

            });
        }

        public static void ConfigureGuestReservations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuestReservation>(entity =>
            {
                entity
                .HasKey(gr => new { gr.GuestId, gr.ReservationId });

                entity
                .HasOne(gr => gr.Guest)
                .WithMany(g => g.GuestReservations)
                .HasForeignKey(gr => gr.GuestId);

                entity
                .HasOne(gr => gr.Reservation)
                .WithMany(r => r.GuestReservations)
                .HasForeignKey(gr => gr.ReservationId);
            });
        }

        public static void ConfigureHotels(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity
                .HasKey(h => h.Id);

                entity.Property(h => h.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

                entity.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(h => h.Rating)
                .IsRequired()
                .HasDefaultValue(1)
                .HasColumnType("TINYINT");


                entity.Property(h => h.Country)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(h => h.City)
               .IsRequired()
               .HasMaxLength(50);

                entity.Property(h => h.Address)
               .IsRequired()
               .HasMaxLength(50);

                entity.HasOne(h => h.Manager)
                .WithOne(m => m.Hotel)
                .HasForeignKey<Hotel>(h => h.ManagerId);

                entity.HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId);

            });
        }

        public static void ConfigureManagers(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>(entity =>
            {
                entity
               .HasKey(m => m.Id);

                entity.Property(m => m.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

                entity.Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(50);

                entity.Property(m => m.LastName)
               .IsRequired()
               .HasMaxLength(50);

                entity.Property(m => m.PersonalNumber)
                .IsRequired()
                .HasColumnType("CHAR(11)")
                .HasMaxLength(11)
                .IsUnicode(false);

                entity.Property(m => m.Email)
                .IsRequired()
                .HasColumnType("VARCHAR(50)")
                .HasMaxLength(50);

                entity.Property(m => m.PhoneNumber)
                .IsRequired()
                .HasMaxLength(15)
                .HasColumnType("VARCHAR(15)")
                .IsUnicode(false);

            });
        }

        public static void ConfigureReservations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(r => r.CheckIn)
                    .IsRequired();

                entity.Property(r => r.CheckOut)
                    .IsRequired();

                entity.HasOne(r => r.Room)
                    .WithMany(room => room.Reservations)
                    .HasForeignKey(r => r.RoomId);

                entity.HasMany(r => r.GuestReservations)
                    .WithOne(gr => gr.Reservation)
                    .HasForeignKey(gr => gr.ReservationId);
            });
        }

        public static void ConfigureRooms(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                    .ValueGeneratedOnAdd()
                    .IsRequired();

                entity.Property(r => r.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(r => r.IsAvailable)
                    .IsRequired();

                entity.Property(r => r.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(r => r.Hotel)
                    .WithMany(h => h.Rooms)
                    .HasForeignKey(r => r.HotelId);

                entity.HasMany(r => r.Reservations)
                   .WithOne(reservation => reservation.Room)
                   .HasForeignKey(reservation => reservation.RoomId);
            });
        }



        public static void SeedManager(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manager>().HasData(
            new Manager()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "123456789",
                PersonalNumber = "11111111111"
            },
            new Manager()
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                PhoneNumber = "987654321",
                PersonalNumber = "22222222222"
            }
            );
        }

        public static void SeedHotel(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel()
                {
                    Id = 1,
                    Name = "Grand Palace",
                    Country = "USA",
                    City = "New York",
                    Address = "123 Main St",
                    Rating = 5,
                    ManagerId = 1
                },
                new Hotel()
                {
                    Id = 2,
                    Name = "Ocean View",
                    Country = "UK",
                    City = "London",
                    Address = "45 Beach Ave",
                    Rating = 4,
                    ManagerId = 2
                }
            );
        }

        public static void SeedRoom(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasData(
                new Room()
                {
                    Id = 1,
                    HotelId = 1,
                    Name = "Luxury Suite",
                    Price = 250,
                    IsAvailable = true
                },
                new Room()
                {
                    Id = 2,
                    HotelId = 1,
                    Name = "Standard Room",
                    Price = 150,
                    IsAvailable = true
                },
                new Room()
                {
                    Id = 3,
                    HotelId = 2,
                    Name = "Deluxe Room",
                    Price = 200,
                    IsAvailable = false
                }
            );
        }

        public static void SeedGuest(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>().HasData(
                new Guest()
                {
                    Id = 1,
                    FirstName = "Alice",
                    LastName = "Brown",
                    PersonalNumber = "33333333333",
                    PhoneNumber = "555666777"
                },
                new Guest()
                {
                    Id = 2,
                    FirstName = "Bob",
                    LastName = "White",
                    PersonalNumber = "44444444444",
                    PhoneNumber = "888999000"
                }
            );
        }

        public static void SeedReservation(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>().HasData(
                new Reservation()
                {
                    Id = 1,
                    RoomId = 1,
                    CheckIn = DateTime.UtcNow.AddDays(1),
                    CheckOut = DateTime.UtcNow.AddDays(5)
                },
                new Reservation()
                {
                    Id = 2,
                    RoomId = 2,
                    CheckIn = DateTime.UtcNow.AddDays(3),
                    CheckOut = DateTime.UtcNow.AddDays(7)
                }
            );

        }
    }
    
}
