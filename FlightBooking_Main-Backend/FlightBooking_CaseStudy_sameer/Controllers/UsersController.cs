using Microsoft.AspNetCore.Mvc;
using FlightBooking_CaseStudy_sameer.DTOs;
using FlightBooking_CaseStudy_sameer.Models;
using Microsoft.EntityFrameworkCore;
using FlightBooking_CaseStudy_sameer.AppDbContext;
using FlightBooking_CaseStudy_sameer.Sevices;
using FlightBooking_CaseStudy_sameer.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sending_Emails_in_Asp.Net_Core;
using Microsoft.AspNetCore.Authorization;

namespace FlightBooking_CaseStudy_sameer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly EmailSending _email;


        public UsersController(ApplicationDbContext context, PasswordHasher passwordHasher, IConfiguration configuration, EmailSending email)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _email = email;
        }
       
        [HttpPost("Login")]
        public async Task<ActionResult<string>> PostLogin([FromBody] LoginDTO loginDto)
        {

            if (loginDto == null)
            {
                return BadRequest("User data is null");
            }
            // Fetch the user based on email
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);

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
                bool isPasswordValid = _passwordHasher.VerifyPassword(user.Password, loginDto.Password);

                if (isPasswordValid)
                {

                    ////////////////
                    // user.FailedLoginAttempts = 0;
                    // user.LastFailedLoginAttempt = null;
                    //////////////////////
                    ///

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim("UserName",user.UserName.ToString()),
                        new Claim("Email",user.Email.ToString()),
                        new Claim(ClaimTypes.Role,user.Role.ToString()),
                        //new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim("UserId",user.UserId.ToString())
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
                    return Ok(new { Token = tokenValue, User_id = user.UserId,Role=user.Role });
                    
                }
                else
                {
                    ////////////////////////
                    // user.FailedLoginAttempts++;
                    // user.LastFailedLoginAttempt = DateTime.UtcNow;
                    // await _context.SaveChangesAsync();
                    ////////////////////////////
                    ///
                    return Unauthorized("Invalid email or password");
                }

            }
            else
            {
                return Unauthorized("Invalid email or password");
            }
           
        }

        [Authorize(Policy = "SoftwareDeveloperOnly")]

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Gender = u.Gender,
                    DOB = u.DOB,
                    Age = u.Age,
                    UserName = u.UserName,
                    Password = u.Password,
                    Email = u.Email,
                    MobileNumber = u.MobileNumber,
                    Role = u.Role
                })
                .ToListAsync();

            return Ok(users);
        }

        // [Authorize(Policy = "SoftwareDeveloperOnly")]
        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Gender = u.Gender,
                    DOB = u.DOB,
                    Age = u.Age,
                    UserName = u.UserName,
                    Password = u.Password,
                    ConfirmPassword=u.ConfirmPassword,
                    Email = u.Email,
                    MobileNumber = u.MobileNumber
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/Users
        /*[HttpPost]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDto)
        {
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Gender = userDto.Gender,
                DOB = userDto.DOB,
                Age = userDto.Age,
                UserName = userDto.UserName,
                Password = userDto.Password,
                Email = userDto.Email,
                MobileNumber = userDto.MobileNumber
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            userDto.UserId = user.UserId;

            return CreatedAtAction(nameof(GetUser), new { id = userDto.UserId }, userDto);
        }*/
     
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> PostUser(UserDTO userDto)
        {
            if (userDto == null)
            {
                return BadRequest("User data is null");
            }

            PasswordHasher a = new PasswordHasher();
            string hashedPassword = a.HashPassword(userDto.Password);

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Gender = userDto.Gender,
                DOB = userDto.DOB,
                Age = userDto.Age,
                UserName = userDto.UserName,
                Password = hashedPassword,
                ConfirmPassword=hashedPassword,// Ensure the password is hashed
                Email = userDto.Email,
                MobileNumber = userDto.MobileNumber,
                Role=userDto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _email.SendEmail(userDto.Email, userDto.UserName);



            userDto.UserId = user.UserId;

            return CreatedAtAction(nameof(GetUser), new { id = userDto.UserId }, userDto);
        }


        // [Authorize(Policy = "SoftwareDeveloperOnly")]
        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Gender = userDto.Gender;
            user.DOB = userDto.DOB;
            user.Age = userDto.Age;
            user.UserName = userDto.UserName;
            user.Password = userDto.Password;
            user.Email = userDto.Email;
            user.MobileNumber = userDto.MobileNumber;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Policy = "SoftwareDeveloperOnly")]

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

