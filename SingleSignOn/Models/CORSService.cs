using IdentityServer4.Services;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace PantryTrackers.STS.Models
{
    public class CORSService : ICorsPolicyService
    {
        private readonly AppSettings _settings;

        public CORSService(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            return await Task.Run(() => _settings.CorsOrigins.Contains(origin));
        }
    }
}
