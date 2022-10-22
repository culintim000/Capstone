using System.Collections;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using LoginSQL;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace auth_service
{
    // [EnableCors("MyAllowSpecificOrigins")]
    [ApiController]
    [Route("auth")]
    public class UserController : ControllerBase
    {
        private readonly UserDb _db;
        private readonly IConfiguration _configuration;

        public UserController(ILogger<UserController> logger, UserDb db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpGet("abc")]
        public ActionResult<string> abc()
        {
            return "hello from abc";
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserReturnDto>> Register(UserRegisterDto request)
        {
            var checkEmail = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (checkEmail != null)
            {
                return BadRequest("User already exists");
            }

            var user = new User()
            {
                Name = request.Name,
                Number = request.Number,
                Email = request.Email,
                Password = SecurePasswordHasher.Hash(request.Password),
                isAdmin = false,
                isWorker = false
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            UserReturnDto outUser = new UserReturnDto(user.Name, user.Email, CreateToken(user), user.isAdmin, user.isWorker);
            return Ok(outUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserReturnDto>> Login(UserLoginDto request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }

            // validate password
            if (!SecurePasswordHasher.Verify(request.Password, user.Password))
            {
                return BadRequest("Wrong password.");
            }

            // return JWT token based on user
            UserReturnDto outUser = new UserReturnDto(user.Name, user.Email, CreateToken(user), user.isAdmin, user.isWorker);
            return Ok(outUser);
        }

        // TODO update user

        private string CreateToken(User user)
        {
            //set a string named role to the user's role
            string role = user.isAdmin ? "Admin" : user.isWorker ? "Worker" : "User";

            List<Claim> claims = new List<Claim>
            {
                new Claim("Name", user.Name),
                new Claim("Role", role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}