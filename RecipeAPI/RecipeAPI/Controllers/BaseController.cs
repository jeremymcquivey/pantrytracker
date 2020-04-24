using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Defines behaviors which are common amongst multiple controllers, or
    /// not relating to the actual data transfer process
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// Retrieves the current user's SSO id
        /// </summary>
        protected string AuthenticatedUser { get => User.FindFirst(ClaimTypes.NameIdentifier).Value; }

        protected IEnumerable<string> UserRoles { get => User.FindAll(ClaimTypes.Role).Select(p => p.Value); }
        /// <summary>
        /// Retrieves all roles associated with the user in the current context.
        /// </summary>
    }
}
