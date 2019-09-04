using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using PantryTracker.Model.Recipe;
using PantryTracker.RecipeReader;
using System;
using System.Threading.Tasks;
using PantryTracker.ExternalServices;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Manages the creation and updating of all recipes for the current user.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class RecipeController : BaseController
    {
        private const char EndOfLinbeDelimiter = '';
        private IOCRService _ocr;

        public RecipeController(IOCRService ocrService)
        {
            _ocr = ocrService;
        }

        /// <summary>
        /// Returns all recipes belonging to the current user.
        /// </summary>
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await Task.Run(() =>
            {
                return Ok();
            });
        }

        /// <summary>
        /// Returns the desired recipe.
        /// </summary>
        //[Authorize]
        [HttpPost]
        [Route("preview/image")]
        public async Task<IActionResult> PreviewFromImage([FromBody]string imageText)
        {
            if(string.IsNullOrEmpty(imageText))
            {
                return Ok(new Recipe());
            }

            try
            {
                var ocrText = _ocr.ImageToText(imageText);
                return await Preview(string.Join('\n', ocrText));
            }
            catch (Exception ex)
            {
                //TODO: Log to app insights.
                throw;
            }
        }

        /// <summary>
        /// Creates a recipe from a raw text block (preview only)
        /// </summary>
        /// <param name="rawText"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("preview/text")]
        public async Task<IActionResult> Preview([FromBody]string rawText)
        {
            if(string.IsNullOrEmpty(rawText))
            {
                return Ok(new Recipe());
            }

            var parser = new MetadataParser();
            var lines = rawText.Split(EndOfLinbeDelimiter);
            var output = parser.ExtractRecipe(lines);

            // Returns just the object representation of the recipe. 
            // Future feature: return a list of possible "duplicates" of this recipe.
            // i.e. similar ingredients/ratios.
            return Ok(output);
        }

        /// <summary>
        /// Creates a new recipe within the current user's collection.
        /// </summary>
        [HttpPost]
        //[Authorize(Roles = "AddRecipe")]
        public async Task<IActionResult> Create([FromBody]Recipe recipe)
        {
            //TODO: Validate model.
            try
            {
                return await Task.Run(() =>
                {
                    return Ok();
                });
            }
            catch(DocumentClientException ex)
            {
                //TODO: Log to app insights.
                throw;
            }
        }

        /// <summary>
        /// Updates an existing recipe within the current user's collection.
        /// </summary>
        //[Authorize]
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody]Recipe recipe)
        {
            //TODO: Validate model. -- This needs to have an ID, and it needs to belong to the correct owner.
            return BadRequest("This method is not implemented yet.");
        }
    }
}
