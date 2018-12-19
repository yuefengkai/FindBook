using System.Threading.Tasks;

namespace User.Identity.Services
{
    public interface IUserService
    {
        Task<int> CheckOrCreate(string phone);
    }
}
