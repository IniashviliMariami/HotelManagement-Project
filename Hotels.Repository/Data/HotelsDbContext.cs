using Hotels.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotels.Repository.Data
{
    public class HotelsDbContext:IdentityDbContext<ApplicationUser>
    {
        public HotelsDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<GuestReservation> GuestReservations { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.ConfigureGuests();
            modelBuilder.ConfigureHotels();
            modelBuilder.ConfigureManagers();
            modelBuilder.ConfigureReservations();
            modelBuilder.ConfigureRooms();
            modelBuilder.ConfigureGuestReservations();

            modelBuilder.SeedUsers();
            modelBuilder.SeedRoles();
            modelBuilder.SeedUserRoles();
            modelBuilder.SeedGuest();
            modelBuilder.SeedHotel();
            modelBuilder.SeedReservation();
            modelBuilder.SeedRoom();
            modelBuilder.SeedManager();
        }


    }
}
