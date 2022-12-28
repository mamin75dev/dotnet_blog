using ErrorOr;
using SimpleBlog.Models;

namespace SimpleBlog.Services.Users
{
    public interface IUserService
    {
        Task<ErrorOr<Created>> InsertUser(ApplicationUser user);
        Task<ErrorOr<ApplicationUser>> GetUser(string userName);
    }
}
