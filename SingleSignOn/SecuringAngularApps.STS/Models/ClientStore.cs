using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace PantryTracker.SingleSignOn.STS.Models
{
    public class ClientStore : IClientStore
    {
        private readonly AppSettings _settings;

        public ClientStore(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return await Task.Run(() =>
            {
                var client = _settings.Clients.FirstOrDefault(c => c.ClientId == clientId);
                return client;
            });
        }
    }
}