using ErrorOr;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Context;
using SimpleBlog.Models;
using System.Security.Claims;

namespace SimpleBlog.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetUserIdFromRequest()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return result;
        }
        public async Task<ErrorOr<Created>> InsertUser(ApplicationUser user)
        {
            try
            {
                _dataContext.Users.Add(user);
                await _dataContext.SaveChangesAsync();

                return Result.Created;
            }
            catch (Exception e)
            {
                return Error.Failure(code: "Database Error", description: e.Message);
            }
        }

        public async Task<ErrorOr<ApplicationUser>> GetUser(string userName)
        {
            try
            {
                var user = await _dataContext.Users.Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    PasswordHash = user.PasswordHash,
                    PasswordSalt = user.PasswordSalt
                }).FirstOrDefaultAsync(user => user.UserName == userName);

                return user;
            }
            catch (Exception e)
            {
                return Error.Failure(code: "Database Error", description: e.Message);
            }
        }
    }
}