using System;
using System.Threading.Tasks;

namespace PantryTrackers.Security
{
    public class MetadataService
    {
        public event EventHandler VersionCompatible;
        public event EventHandler VersionIncompatible;

        public async Task CheckVersion()
        {
            //VersionIncompatible?.Invoke(this, new EventArgs());
            VersionCompatible?.Invoke(this, new EventArgs());
        }
    }
}
