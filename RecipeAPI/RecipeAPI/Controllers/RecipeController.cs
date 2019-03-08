using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using RecipeAPI.Helpers;
using RecipeAPI.Model;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    /// <summary>
    /// Manages the creation and updating of all recipes for the current user.
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class RecipeController : BaseController
    {
        private const string RecipeDatabase = "PT_RecipeAPI";
        private const string RecipeCollection = "PT_Recipes";

        private readonly DocumentClient _documents;

#pragma warning disable 1591
        public RecipeController(IOptions<AppSettings> config)
#pragma warning restore 1591
        {
            _documents = new DocumentClient(new System.Uri(config.Value.ConnectionStrings.CosmosConnection),
                                                           config.Value.ConnectionStrings.CosmosPassword);
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
                var collectionLink = UriFactory.CreateDocumentCollectionUri(RecipeDatabase, RecipeCollection);
                var result = _documents.CreateDocumentQuery<Recipe>(collectionLink)
                                       .Where(so => so.OwnerId == AuthenticatedUser)
                                       .AsEnumerable();

                return Ok(result);
            });
        }

        /// <summary>
        /// Returns the desired recipe.
        /// </summary>
        //[Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var doc = await _documents.ReadDocument<Recipe>(RecipeDatabase, RecipeCollection, id);

                if(doc?.OwnerId != AuthenticatedUser)
                {
                    return NotFound();
                }

                return Ok(doc);
            }
            catch (DocumentClientException ex)
            {
                //TODO: Log to app insights.
                throw;
            }
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
                recipe.OwnerId = AuthenticatedUser;
                return Ok(await _documents.AddOrUpdateDocument(RecipeDatabase, RecipeCollection, recipe));
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
        //[HttpPatch]
        public async Task<IActionResult> Update([FromBody]Recipe recipe)
        {
            //TODO: Validate model. -- This needs to have an ID, and it needs to belong to the correct owner.
            return BadRequest("This method is not implemented yet.");
        }
    }
}