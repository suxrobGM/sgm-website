using System.Collections.Generic;
using System.Threading.Tasks;
using SGM.Domain.Entities.UserEntities;

namespace SGM.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task UpdateUserRolesAsync(ApplicationUser user, IEnumerable<string> roles);

        /// <summary>
        /// Deeply deletes all records of the user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns></returns>
        Task DeleteDeeplyAsync(ApplicationUser user);
    }
}
