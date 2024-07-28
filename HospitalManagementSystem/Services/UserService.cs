using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagementSystem.Controllers;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAdminDataAsync()
        {
            // Example: Return all users with Admin role
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");
            return adminUsers;
        }

        public async Task<IdentityResult> CreateUserAsync(UserCreationModel model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Assign role to the new user
                await _userManager.AddToRoleAsync(user, "Admin"); // Example: Assigning "Admin" role
            }

            return result;
        }

        public async Task<IdentityResult> UpdateUserAsync(string userId, UserUpdateModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User not found with ID: {userId}");
            }

            // Update user properties
            //user.FullName = model.FullName;
            user.Email = model.Email;

            // Update user using UserManager
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User not found with ID: {userId}");
            }

            var result = await _userManager.DeleteAsync(user);
            return result;
        }
    }
}
