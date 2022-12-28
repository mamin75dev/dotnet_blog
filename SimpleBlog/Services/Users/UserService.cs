using ErrorOr;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Context;
using SimpleBlog.Models;

namespace SimpleBlog.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DBContext dBContext;

        public UserService(DBContext dBContext)
        {
            this.dBContext = dBContext;
        }

        public async Task<ErrorOr<Created>> InsertUser(ApplicationUser user)
        {
            try
            {
                dBContext.Users.Add(user);
                await dBContext.SaveChangesAsync();

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
                var user = await dBContext.Users.Select(user => new ApplicationUser
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