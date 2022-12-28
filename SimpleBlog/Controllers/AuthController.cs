using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleBlog.Dto.Requests;
using SimpleBlog.Dto.Responses;
using SimpleBlog.Models;
using SimpleBlog.Services.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SimpleBlog.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            ErrorOr<Created> createUserResult = await _userService.InsertUser(user);

            if (createUserResult.IsError)
            {
                return Problem(createUserResult.Errors);
            }

            var registerResponseDto = new RegisterResponseDto(user.Id, user.UserName, user.Email, user.PhoneNumber);

            return Ok(registerResponseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            ErrorOr<ApplicationUser> appUserResult = await _userService.GetUser(request.UserName);

            if (appUserResult.IsError) return Problem(appUserResult.Errors);

            var appUser = appUserResult.Value;

            if (!CompareUserPasswords(request.Password, appUser.PasswordHash, appUser.PasswordSalt))
            {
                return Unauthorized();
            }

            string token = CreateToken(appUser);

            return Ok(token);
        }

        [HttpGet("me"), Authorize]
        public IActionResult GetMe()
        {
            // var userName = User?.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            // var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userName = _userService.GetUserIdFromRequest();

            return Ok(userName);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool CompareUserPasswords(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}