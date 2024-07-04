using HospitalManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
  
        public class AdminController : ControllerBase
        {
            private readonly IUserService _userService; // Assuming IUserService for user operations

            public AdminController(IUserService userService)
            {
                _userService = userService;
            }

            // GET: api/Admin
            [HttpGet]
            public IActionResult Get()
            {
                try
                {
                    var adminData = _userService.GetAdminDataAsync(); // Example: Fetch admin-specific data from a service method
                    return Ok(adminData);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            //POST: api/Admin
            [HttpPost]
            public async Task<IActionResult> Post([FromBody] UserCreationModel model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var result = await _userService.CreateUserAsync(model); // Example: Create user via service method

                    if (result.Succeeded)
                    {
                        return Ok("User created successfully");
                    }
                    else
                    {
                        return BadRequest(new { Errors = result.Errors });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            // PUT: api/Admin/5
            [HttpPut("{id}")]
            public async Task<IActionResult> Put(string id, [FromBody] UserUpdateModel model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                try
                {
                    var result = await _userService.UpdateUserAsync(id, model); // Example: Update user via service method

                    if (result.Succeeded)
                    {
                        return Ok($"User updated successfully for ID: {id}");
                    }
                    else
                    {
                        return BadRequest(new { Errors = result.Errors });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }

            // DELETE: api/Admin/5
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(string id)
            {
                try
                {
                    var result = await _userService.DeleteUserAsync(id); // Example: Delete user via service method

                    if (result.Succeeded)
                    {
                        return Ok($"User deleted successfully for ID: {id}");
                    }
                    else
                    {
                        return BadRequest(new { Errors = result.Errors });
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }

        public class UserCreationModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string Email { get; set; }
        }

        public class UserUpdateModel
        {
            public string FullName { get; set; }
            public string Email { get; set; }
        }
    }
