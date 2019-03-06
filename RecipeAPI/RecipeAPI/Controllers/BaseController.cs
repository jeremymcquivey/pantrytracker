using Microsoft.AspNetCore.Mvc;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Defines behaviors which are common amongst multiple controllers, or
    /// not relating to the actual data transfer process
    /// </summary>
    public class BaseController : Controller
    {
        private const string UserId = "1234ABC";

        /// <summary>
        /// Retrieves the current user's SSO id
        /// </summary>
        protected string AuthenticatedUser { get => UserId; }
    }
}
