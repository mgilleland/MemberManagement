using MemberManagement.AppCore.Entities;
using MemberManagement.AppCore.Interfaces;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MemberManagement.Data
{
    public class MemberRepository : EfRepository<Member>, IMemberRepository
    {
        public MemberRepository(MemberManagementContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsUserNameUniqueAsync(string userName)
        {
            return !await _dbContext.Members.AnyAsync(m => m.UserName == userName);
        }
    }
}
