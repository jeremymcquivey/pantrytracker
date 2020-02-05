using PantryTracker.Model.Products;
using System.Threading.Tasks;

namespace PantryTracker.Model
{
    public interface IProductSearch
    {
        Task<ProductCode> SearchByCodeAsync(string code); 
    }
}
