using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RecipeAPI.Models;
using PantryTracker.Model.Auth;
using System.Security.Claims;
using IdentityModel;
using System.Linq;
using System;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Manages user accounts
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class UserController: BaseController
    {
        private readonly RecipeContext _context;

        /// <summary>
        /// </summary>
        public UserController(RecipeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the user's core profile
        /// </summary>
        [HttpGet]
        [Route("AuthContext")]
        public IActionResult GetAuthContext()
        {
            var identity = User.Claims;
            long.TryParse(identity.FirstOrDefault(c => c.Type == JwtClaimTypes.Expiration)?.Value, out long expTicks);

            return Ok(new AuthContext
            {
                Expires = new DateTime(expTicks),
                UserProfile = new UserProfile
                {
                    Id = identity.FirstOrDefault(c => c.Type == JwtClaimTypes.Id)?.Value,
                    FirstName = User.FindFirst(ClaimTypes.GivenName)?.Value,
                    Email = User.FindFirst(JwtClaimTypes.PreferredUserName)?.Value,
                    LastName = User.FindFirst(ClaimTypes.Surname)?.Value
                },
                Roles = UserRoles
            }); ;
        }
    }
}