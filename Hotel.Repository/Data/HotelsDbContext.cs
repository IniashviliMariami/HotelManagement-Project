using Hotels.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Repository.Data
{
    public class HotelsDbContext : DbContext
    {
        public HotelsDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<GuestReservation> GuestReservations { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.ConfigureGuests();
            modelBuilder.ConfigureHotels();
            modelBuilder.ConfigureManagers();
            modelBuilder.ConfigureReservations();
            modelBuilder.ConfigureRooms();
            modelBuilder.ConfigureGuestReservations();


            modelBuilder.SeedGuest();
            modelBuilder.SeedHotel();
            modelBuilder.SeedReservation();
            modelBuilder.SeedRoom();
            modelBuilder.SeedManager();
        }
    }
}
