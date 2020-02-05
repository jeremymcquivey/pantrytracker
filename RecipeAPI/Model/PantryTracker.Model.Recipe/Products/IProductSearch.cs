using PantryTracker.Model.Products;
using System.Threading.Tasks;

namespace PantryTracker.Model
{
    public interface IProductSearch
    {
        string Name { get; }

        Task<ProductCode> SearchByCodeAsync(string code); 
    }
}
