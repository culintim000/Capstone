using System.Collections;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LoginSQL;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using RabbitMQ.Client;

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
                new Claim("Role", role),
                new Claim("Email", user.Email)
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
        
        [HttpPost]
        [Route("getUser")]
        public async Task<ActionResult> GetUser(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.Contains(email));
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }
            
            var userDto = new UserReturnView(user.Name, user.Email, user.Number, user.isWorker);

            return Ok(userDto);
        }
        
        [HttpPost]
        [Route("makeWorker")]
        public async Task<ActionResult> MakeWorker(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }

            user.isWorker = true;
            await _db.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPost]
        [Route("removeWorker")]
        public async Task<ActionResult> RemoveWorker(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }

            user.isWorker = false;
            await _db.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<ActionResult> RecoverPassword(string email)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }

            string verificationCode = CreatePassword(15);

            user.Password = SecurePasswordHasher.Hash(verificationCode);
            await _db.SaveChangesAsync();

            var factory = new ConnectionFactory() { HostName = "message-queue" };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                string message = email + " " + verificationCode;
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);
                Console.WriteLine(" [x] Sent {0}", message);
            }

            return Ok("Verification code sent to your email.");
        }
        
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<ActionResult> ChangePassword(string email, string verificationCode, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }
            
            if (!SecurePasswordHasher.Verify(verificationCode, user.Password))
            {
                return BadRequest("Wrong Verification Code.");
            }

            user.Password = SecurePasswordHasher.Hash(password);
            await _db.SaveChangesAsync();
            
            return Ok("Changed password");
        }
        
    }
}