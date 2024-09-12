using Microsoft.AspNetCore.Mvc;
using FlightBooking_CaseStudy_sameer.DTOs;
using FlightBooking_CaseStudy_sameer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightBooking_CaseStudy_sameer.AppDbContext;
using Microsoft.AspNetCore.Authorization;

namespace FlightBooking_CaseStudy_sameer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class SchedulesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SchedulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Schedules
        [HttpGet]
       [Authorize(Policy = "SoftwareDeveloperOnly")]
            public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            var schedules = await _context.Schedules
                .Select(s => new ScheduleDTO
                {
                    ScheduleId = s.schedule_id,
                    FlightName=s.Flight_Name,
                    SeatCapacity=s.Seat_Capacity,
                    StartLocation = s.Start_location,
                    Destination = s.Destination,
                    TravelDate = s.Travel_date,
                    ArrivalTime=TimeOnly.FromTimeSpan(s.Arrival_Time),
                    DepartureTime=TimeOnly.FromTimeSpan(s.Depature_Time),
                    Fare = s.Fare

                })
                .ToListAsync();

            return Ok(schedules);
        }

        // GET: api/Schedules/5
          [HttpGet("{id}")]
          public async Task<ActionResult<ScheduleDTO>> GetSchedule(int id)
          {
              var schedule = await _context.Schedules
                  .Where(s => s.schedule_id == id)
                  .Select(s => new ScheduleDTO
                  {

                    ScheduleId = s.schedule_id,
                    FlightName=s.Flight_Name,
                    SeatCapacity=s.Seat_Capacity,
                    StartLocation = s.Start_location,
                    Destination = s.Destination,
                    TravelDate = s.Travel_date,
                    ArrivalTime=TimeOnly.FromTimeSpan(s.Arrival_Time),
                    DepartureTime=TimeOnly.FromTimeSpan(s.Depature_Time),
                    Fare = s.Fare

                  })
                  .FirstOrDefaultAsync();

              if (schedule == null)
              {
                  return NotFound();
              }

              return Ok(schedule);
          }

          ////////////////////////////////////////////////////////
          





          
     //   [Authorize(Policy = "SoftwareDeveloperOnly")]
    //    [Authorize(Policy = "PassengersOnly")]
        [HttpGet("Source/Destination")]
        public async Task<ActionResult<IEnumerable<ScheduleDTO>>> GetSchedule(
      [FromQuery] string startLocation,
      [FromQuery] string destination,
      [FromQuery] DateOnly travelDate)
        {
            if (string.Equals(startLocation, destination, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Source and Destination cannot be same.");
            }

            // Step 2: Validate if travelDate is in the past
             if (travelDate < DateOnly.FromDateTime(DateTime.Now))
            {
            return BadRequest("Travel date cannot be in the past.");
            }
            var schedule = await _context.Schedules
                .Where(s => s.Start_location == startLocation
                            && s.Destination == destination
                            && s.Travel_date == travelDate)
                .Select(s => new ScheduleDTO
                {
                    ScheduleId =s.schedule_id,
                    FlightName=s.Flight_Name,
                    SeatCapacity=s.Seat_Capacity,
                    StartLocation =s.Start_location,
                    Destination = s.Destination,
                    TravelDate = s.Travel_date,
                    ArrivalTime=TimeOnly.FromTimeSpan(s.Arrival_Time),
                    DepartureTime=TimeOnly.FromTimeSpan(s.Depature_Time),
                    Fare = s.Fare
                })
                .ToListAsync();

            if (schedule == null)
            {
                return NotFound("There is no Flights for this Source and Destination");
            }

            return Ok(schedule);
        }


        // POST: api/Schedules
        [Authorize(Policy = "SoftwareDeveloperOnly")]
        [HttpPost]

        public async Task<ActionResult<ScheduleDTO>> PostSchedule(ScheduleDTO scheduleDto)
        {
            var schedule = new Schedule
            {
                schedule_id = scheduleDto.ScheduleId,
                Flight_Name=scheduleDto.FlightName,
                Seat_Capacity=scheduleDto.SeatCapacity,
                Start_location = scheduleDto.StartLocation,
                Destination = scheduleDto.Destination,
                Travel_date = scheduleDto.TravelDate,
                Arrival_Time=scheduleDto.ArrivalTime.ToTimeSpan(),
                Depature_Time=scheduleDto.DepartureTime.ToTimeSpan(),
                Fare = scheduleDto.Fare
            };

            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            scheduleDto.ScheduleId = schedule.schedule_id;

            return CreatedAtAction(nameof(GetSchedule), new { id = scheduleDto.ScheduleId }, scheduleDto);
        }
        [Authorize(Policy = "SoftwareDeveloperOnly")]

        // PUT: api/Schedules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, ScheduleDTO scheduleDto)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            // schedule.FlightId = scheduleDto.FlightId;
            // schedule.StartLocation = scheduleDto.StartLocation;
            // schedule.Destination = scheduleDto.Destination;
            // schedule.TravelDate = scheduleDto.TravelDate;
            // schedule.Fare = scheduleDto.Fare;
                schedule.schedule_id = scheduleDto.ScheduleId;
                schedule.Flight_Name=scheduleDto.FlightName;
                schedule.Seat_Capacity=scheduleDto.SeatCapacity;
                schedule.Start_location = scheduleDto.StartLocation;
                schedule.Destination = scheduleDto.Destination;
                schedule.Travel_date = scheduleDto.TravelDate;
                schedule.Arrival_Time=scheduleDto.ArrivalTime.ToTimeSpan();
                schedule.Depature_Time=scheduleDto.DepartureTime.ToTimeSpan();
                schedule.Fare = scheduleDto.Fare;

            _context.Entry(schedule).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [Authorize(Policy = "SoftwareDeveloperOnly")]
        // DELETE: api/Schedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
    
}
