using Microsoft.AspNetCore.Mvc;
using FlightBooking_CaseStudy_sameer.DTOs;
using FlightBooking_CaseStudy_sameer.Models;
using Microsoft.EntityFrameworkCore;
using FlightBooking_CaseStudy_sameer.AppDbContext;
using Microsoft.AspNetCore.Authorization;
using FlightBooking_CaseStudy_sameer.Sevices;
using System.Security.Claims;




namespace FlightBooking_CaseStudy_sameer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BookingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailBooking _email;

        public BookingsController(ApplicationDbContext context,EmailBooking email)
        {
            _context = context;
            _email = email;
        }
      
        [Authorize(Policy = "PassengersOnly")]
        [HttpGet("current")]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings()
         {
             var userIdClaim = int.Parse(User.FindFirst("UserId")?.Value);
            
            if (userIdClaim == null)
            {
            return Unauthorized("UserId claim is missing.");
            }
                    //
              var bookings = await _context.Bookings.Where(b=>b.UserId==userIdClaim)
                  .Select(b => new BookingDTO
                  {
                      BookingId = b.BookingId,
                      UserId = b.UserId,
                      ScheduleId = b.ScheduleId,
                      Name = b.Name,
                      No_of_Sets=b.No_of_Sets,
                      CheckInStatus = b.CheckInStatus,
                      BookingDate = b.BookingDate,
                      TotalAmount = b.TotalAmount,
                      Email=b.Email

                  })
                  .ToListAsync();


             return Ok(bookings);
         }
        
       //main 
        // GET: api/Bookings/5
        // [Authorize(Policy = "PassengersOnly")]
        // [HttpGet("{id}")]
        // public async Task<ActionResult<BookingDTO>> GetBooking(int id)
        // {
        //     var booking = await _context.Bookings
        //         .Where(b => b.BookingId == id)
        //         .Select(b => new BookingDTO
        //         {
        //             BookingId = b.BookingId,
        //             UserId = b.UserId,
        //             ScheduleId = b.ScheduleId,
        //             Name= b.Name,
        //             No_of_Sets= b.No_of_Sets,
        //             CheckInDate = b.CheckInDate,
        //             SeatNumber = b.SeatNumber,
        //             CheckInStatus = b.CheckInStatus,
        //             Email = b.Email
        //         })
        //         .FirstOrDefaultAsync();

        //     if (booking == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok(booking);
        // }
       
        
        [Authorize(Policy = "PassengersOnly")]
        [HttpPost("Ticket_Booking")]
        public async Task<ActionResult<BookingDTO>> PostBooking(BookingDTO bookingDto)
        {
            
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.schedule_id == bookingDto.ScheduleId);

            if (schedule == null)
            {
                return NotFound("Need to given Schedule id.");
            }

            // Calculate total amount
            var totalAmount = schedule.Fare * bookingDto.No_of_Sets;
            

            // Create the Booking entity
            var booking = new Booking
            {
                UserId = bookingDto.UserId,
                ScheduleId = bookingDto.ScheduleId,
                Name = bookingDto.Name,
                No_of_Sets = bookingDto.No_of_Sets,
                BookingDate = bookingDto.BookingDate,
                CheckInStatus = bookingDto.CheckInStatus,
                Email = bookingDto.Email,
                TotalAmount = totalAmount 
            };

            // save changes
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            //Email sending
            _email.SendEmail(bookingDto.Email, bookingDto.Name);

            
            bookingDto.BookingId = booking.BookingId;
            bookingDto.TotalAmount = totalAmount;

            

            return CreatedAtAction(nameof(GetBookings), new { id = bookingDto.BookingId }, bookingDto);
        }

       
        // PUT: api/Bookings/5
           [HttpPut("{id}")]
           public async Task<IActionResult> PutBooking(int id, BookingDTO bookingDto)
           {
               var booking = await _context.Bookings.FindAsync(id);
               if (booking == null)
               {
                   return NotFound();
               }

               booking.UserId = bookingDto.UserId;
               booking.ScheduleId = bookingDto.ScheduleId;
               booking.Name= bookingDto.Name;
               booking.No_of_Sets= bookingDto.No_of_Sets;
               booking.BookingDate = bookingDto.BookingDate;
               booking.CheckInStatus = bookingDto.CheckInStatus;
               booking.Email = bookingDto.Email;
               booking.TotalAmount=bookingDto.TotalAmount;

               _context.Entry(booking).State = EntityState.Modified;
               await _context.SaveChangesAsync();

               return NoContent();
           }
        //  [Authorize(Policy = "PassengersOnly")]

        // DELETE: api/Bookings/5
        [Authorize(Policy = "PassengersOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
