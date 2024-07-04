using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalManagementSystem.Controllers;
using HospitalManagementSystem.Models;
using Microsoft.AspNetCore.Identity; // Replace with your user model namespace
namespace HospitalManagementSystem.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAdminDataAsync();
        Task<IdentityResult> CreateUserAsync(UserCreationModel model);
        Task<IdentityResult> UpdateUserAsync(string userId, UserUpdateModel model);
        Task<IdentityResult> DeleteUserAsync(string userId);
    }
}