using Microsoft.AspNetCore.Identity;

namespace SGM.EntityFramework.Repositories;

public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    private readonly DatabaseContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager, 
        DatabaseContext context) : base(context)
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
