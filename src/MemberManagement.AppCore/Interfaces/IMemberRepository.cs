using System.Threading.Tasks;
using MemberManagement.AppCore.Entities;

namespace MemberManagement.AppCore.Interfaces
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<bool> IsUserNameUniqueAsync(string userName);
    }
}
