using FlightBooking_CaseStudy_sameer.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightBooking_CaseStudy_sameer.AppDbContext
{


    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for each class
        // public DbSet<Flight> Flights { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }

    }

}
