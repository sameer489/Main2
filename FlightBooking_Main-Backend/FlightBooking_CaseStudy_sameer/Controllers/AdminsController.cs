using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightBooking_CaseStudy_sameer.AppDbContext;
using FlightBooking_CaseStudy_sameer.Models;
using FlightBooking_CaseStudy_sameer.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlightBooking_CaseStudy_sameer.Sevices;
using FlightBooking_CaseStudy_sameer.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace FlightBooking_CaseStudy_sameer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher _password_Hash;
        private readonly IConfiguration _configuration;
        

        public AdminsController(ApplicationDbContext context, PasswordHasher passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _password_Hash = passwordHasher;
            _configuration = configuration;
           
        }

        [HttpPost("Admin-Login")]
        public async Task<ActionResult<string>> PostLogin([FromBody] LoginDTO adminDto)
        {


            if (adminDto == null)
            {
                return BadRequest("User data is null");
            }

            // Fetch the user based on email
            var user = await _context.Admins
                .FirstOrDefaultAsync(u => u.AdminName == adminDto.UserName);
            if (user != null)
            {
                ////////////login time///////////////
                // if (user.FailedLoginAttempts >= 3 && user.LastFailedLoginAttempt != null)
                // {
                //     var timeSinceLastAttempt = DateTime.UtcNow - user.LastFailedLoginAttempt.Value;

                //     // If less than 1 minute has passed, deny login
                //     if (timeSinceLastAttempt < TimeSpan.FromMinutes(1))
                //     {
                //         return BadRequest("Too many failed attempts. Please wait 1 minute before trying again.");
                //     }
                // }
                /////////////////////////////////////////

                // Verify the provided password with the stored hashed password
                bool isPasswordValid = _password_Hash.VerifyPassword(user.AdminPassword, adminDto.Password);

                if (isPasswordValid)
                {
                    ////////////////
                    // user.FailedLoginAttempts = 0;
                    // user.LastFailedLoginAttempt = null;
                    //////////////////////


                    await _context.SaveChangesAsync();

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim("AdminName",user.AdminName.ToString()),   
                        new Claim("AdminEmail",user.AdminEmail.ToString()),
                        //new Claim()
                        new Claim(ClaimTypes.Role,user.Role.ToString())
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(60),
                        signingCredentials: signIn
                    );
                    string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
                    return Ok(new { Token = tokenValue, role = user.Role,adminEmail=user.AdminEmail,adminName=user.AdminName });
                }
                else
                {
                    ////////////////////////
                    // user.FailedLoginAttempts++;
                    // user.LastFailedLoginAttempt = DateTime.UtcNow;
                    // await _context.SaveChangesAsync();
                    ////////////////////////////

                    return Unauthorized("Invalid password");
                }

            }
            else
            {
                
                return Unauthorized("Invalid username");
            }

            
        }

        // GET: api/Admins
        [Authorize(Policy = "SoftwareDeveloperOnly")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        // GET: api/Admins/5
        /*  [HttpGet("{id}")]
          public async Task<ActionResult<Admin>> GetAdmin(int id)
          {
              var admin = await _context.Admins.FindAsync(id);

              if (admin == null)
              {
                  return NotFound();
              }

              return admin;
          }*/

        // PUT: api/Admins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
         /* [HttpPut("{id}")]
           public async Task<IActionResult> PutAdmin(int id, Admin admin)
           {
               if (id != admin.AdminId)
               {
                   return BadRequest();
               }

               _context.Entry(admin).State = EntityState.Modified;

               try
               {
                   await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!AdminExists(id))
                   {
                       return NotFound();
                   }
                   else
                   {
                       throw;
                   }
               }

               return NoContent();
           }*/

        // POST: api/Admins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       //[Authorize(Policy = "SoftwareDeveloperOnly")]
        [HttpPost("Registration")]
        public async Task<ActionResult<Admin>> PostAdmin(AdminDTO adminDto)
        {
            PasswordHasher a = new PasswordHasher();
            string hashedPassword = a.HashPassword(adminDto.AdminPassword);

            var admin = new Admin
            {
                AdminId = adminDto.AdminId,
                AdminName = adminDto.AdminName,
                AdminEmail=adminDto.AdminEmail,
              

                AdminPassword = hashedPassword, // Ensure the password is hashed
                Role=adminDto.Role
               
            };
            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();



            adminDto.AdminId = admin.AdminId;

            return CreatedAtAction("GetAdmins", new { id = admin.AdminId }, admin);
        }
      
        // DELETE: api/Admins/5
       [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }

}
