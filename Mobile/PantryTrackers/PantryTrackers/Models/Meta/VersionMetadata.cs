using System;
using System.Collections.Generic;
using System.Text;

namespace PantryTrackers.Models.Meta
{
    public class VersionMetadata
    {
        public long MinVersionCode { get; set; }

        public string MinVersionName { get; set; }

        public string UpdateUrl { get; set; }
    }
}
