using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Dto.Requests;
using SimpleBlog.Dto.Responses;
using SimpleBlog.Models;
using SimpleBlog.Services.Users;

namespace SimpleBlog.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IUserService userService;

        public AuthController(IUserService userService)
        {
            this.userService = userService;
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

            ApplicationUser.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            ErrorOr<Created> createUserResult = await userService.InsertUser(user);

            if (createUserResult.IsError)
            {
                return Problem(createUserResult.Errors);
            }

            var registerResponseDto = new RegisterResponseDto(user.Id, user.UserName, user.Email, user.PhoneNumber);

            /*var response = new GenericResponse<RegisterResponseDto>(201, "User Created", registerResponseDto);*/

            return Ok(registerResponseDto);
        }
    }
}
