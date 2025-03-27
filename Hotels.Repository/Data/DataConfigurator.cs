using Hotels.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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



        public static void SeedUsers(this ModelBuilder modelBuilder)
        {
            PasswordHasher<ApplicationUser> hasher = new();
            modelBuilder.Entity<ApplicationUser>().HasData
                (
                 new ApplicationUser()
                 {
                     Id = "8716071C-1D9B-48FD-B3D0-F059C4FB8031",
                     UserName = "admin@gmail.com",
                     NormalizedUserName = "ADMIN@GMAIL.COM",
                     Email = "admin@gmail.com",
                     NormalizedEmail = "ADMIN@GMAIL.COM",
                     EmailConfirmed = false,
                     PasswordHash = hasher.HashPassword(null, "Admin123!"),
                     PhoneNumber = "555337681",
                     PhoneNumberConfirmed = false,
                     TwoFactorEnabled = false,
                     LockoutEnd = null,
                     LockoutEnabled = true,
                     AccessFailedCount = 0,
                     FullName = "Administrator"
                 },
                  new ApplicationUser()
                  {
                      Id = "D514EDC9-94BB-416F-AF9D-7C13669689C9",
                      UserName = "manager@gmail.com",
                      NormalizedUserName = "MANAGER@GMAIL.COM",
                      Email = "manager@gmail.com",
                      NormalizedEmail = "MANAGER@GMAIL.COM",
                      EmailConfirmed = false,
                      PasswordHash = hasher.HashPassword(null, "Manager123!"),
                      PhoneNumber = "558558866",
                      PhoneNumberConfirmed = false,
                      TwoFactorEnabled = false,
                      LockoutEnd = null,
                      LockoutEnabled = true,
                      AccessFailedCount = 0,
                      FullName = "Manager"
                  },
                   new ApplicationUser()
                   {
                       Id = "87746F88-DC38-4756-924A-B95CFF3A1D8A",
                       UserName = "mari@gmail.com",
                       NormalizedUserName = "MARI@GMAIL.COM",
                       Email = "mari@gmail.com",
                       NormalizedEmail = "MARI@GMAIL.COM",
                       EmailConfirmed = false,
                       PasswordHash = hasher.HashPassword(null, "mari123!"),
                       PhoneNumber = "551446622",
                       PhoneNumberConfirmed = false,
                       TwoFactorEnabled = false,
                       LockoutEnd = null,
                       LockoutEnabled = true,
                       AccessFailedCount = 0,
                       FullName = "Mariam Iniashvili"
                   }
                );
        }

        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole { Id = "33B7ED72-9434-434A-82D4-3018B018CB87", Name = "Admin", NormalizedName = "ADMIN" },
               new IdentityRole { Id = "477340A8-A64A-4F6F-816F-9066D38548A6", Name = "Manager", NormalizedName = "MANAGER" },
               new IdentityRole { Id = "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", Name = "Guest", NormalizedName = "GUEST" }
           );
        }

        public static void SeedUserRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                    new IdentityUserRole<string> { RoleId = "33B7ED72-9434-434A-82D4-3018B018CB87", UserId = "8716071C-1D9B-48FD-B3D0-F059C4FB8031" },
                    new IdentityUserRole<string> { RoleId = "477340A8-A64A-4F6F-816F-9066D38548A6", UserId = "D514EDC9-94BB-416F-AF9D-7C13669689C9" },
                    new IdentityUserRole<string> { RoleId = "9C07F9F6-D3B0-458A-AB7F-218AA622FA5B", UserId = "87746F88-DC38-4756-924A-B95CFF3A1D8A" }
                );
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
            CheckIn = new DateTime(2025, 04, 01),
            CheckOut = new DateTime(2025, 04, 05)
        },
        new Reservation()
        {
            Id = 2,
            RoomId = 2,
            CheckIn = new DateTime(2025, 04, 03),
            CheckOut = new DateTime(2025, 04, 07)
        }
    );

        }
        public static void GuestReservation(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GuestReservation>().HasData(
                new GuestReservation { GuestId = 1, ReservationId = 1},
                new GuestReservation { GuestId = 2, ReservationId = 1},
                new GuestReservation { GuestId = 1, ReservationId = 2}
            );
        }



        
    }
}
