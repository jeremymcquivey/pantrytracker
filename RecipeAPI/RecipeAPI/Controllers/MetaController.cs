using Microsoft.AspNetCore.Mvc;

namespace RecipeAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MetaController : BaseController
    {
        [HttpGet]
        public IActionResult GetMetaData()
        {
            return Ok(new 
            { 
                ApiVersion = "1.0.0",
                Android = new 
                {
                    MinimumAppVersion = "1",
                    MinimumAppVersionName = "1.0"
                },
                iOS = new
                {
                    MinimumAppVersion = "1.0",
                    MinimumAppVersionName = "1.0"
                },
                STSUrl = "",
            });
        }
    }
}
