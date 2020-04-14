using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityModel;
using RecipeAPI.Models;
using PantryTracker.Model.Auth;
using System.Security.Claims;

namespace RecipeAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UserController: BaseController
    {
        private readonly RecipeContext _context;

        public UserController(RecipeContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("AuthContext")]
        public IActionResult GetAuthContext()
        {
            return Ok(new AuthContext
            {
                UserProfile = new UserProfile
                {
                    Id = AuthenticatedUser,
                    FirstName = User.FindFirst(ClaimTypes.GivenName)?.Value,
                },
                Roles = UserRoles
            });
        }
    }
}