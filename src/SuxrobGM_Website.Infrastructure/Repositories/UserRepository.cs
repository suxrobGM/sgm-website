using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SuxrobGM_Website.Core.Entities.BlogEntities;
using SuxrobGM_Website.Core.Entities.UserEntities;
using SuxrobGM_Website.Core.Interfaces.Repositories;
using SuxrobGM_Website.Infrastructure.Data;

namespace SuxrobGM_Website.Infrastructure.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context) : base(context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task UpdateUserRolesAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var actualRoles = roles.ToList();
            var previousRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, previousRoles);
            
            foreach (var role in actualRoles)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        public async Task DeleteDeeplyAsync(ApplicationUser user)
        {
            if (user == null)
            {
                return;
            }

            var deletedUserAccount = await _userManager.FindByNameAsync("DELETED_USER");
            var articles =  _context.Set<Blog>().Where(i => i.Author.Id == user.Id);
            var comments =  _context.Set<Comment>().Where(i => i.Author.Id == user.Id);

            foreach (var article in articles)
            {
                article.Author = deletedUserAccount;
            }

            foreach (var comment in comments)
            {
                comment.Author = deletedUserAccount;
            }

            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(user);
        }
    }
}
