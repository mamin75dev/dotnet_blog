using ErrorOr;
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

        /*public async Task<ErrorOr<User>> GetUser(string email, string password)
        {

          
        }*/
    }
}
