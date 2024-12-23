using haberPortali1.Models;
using haberPortali1.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RefikHaber.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefikHaber_Portal.Repositories
{
    public class RoleManagerRepository
    {
        private UygulamaDbContext _context;

        public RoleManagerRepository(UygulamaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppRole>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Select(role => new AppRole
                {
                    Id = role.Id,
                    Name = role.Name
                    // Gerekli diğer özellikleri burada eşleştirin
                })
                .ToListAsync();
        }


        public async Task AddRoleAsync(AppRole role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                })
                .ToListAsync();
        }




        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            return await _context.Users
                .Select(user => new ApplicationUser
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                })
                .ToListAsync();
        }

        public async Task<Dictionary<string, List<string>>> GetUserRolesGroupedAsync()
        {
            var userRoles = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();

            return userRoles
                .GroupBy(ur => ur.UserId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(ur => roles.FirstOrDefault(r => r.Id == ur.RoleId)?.Id).Where(roleId => roleId != null).ToList()
                );
        }

        public async Task UpdateUserRolesAsync(string userId, IEnumerable<string> newRoles)
        {
            var currentRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            _context.UserRoles.RemoveRange(currentRoles);

            var newUserRoles = newRoles.Select(roleId => new IdentityUserRole<string>
            {
                UserId = userId,
                RoleId = roleId
            });

            await _context.UserRoles.AddRangeAsync(newUserRoles);
            await _context.SaveChangesAsync();
        }
    }
}
