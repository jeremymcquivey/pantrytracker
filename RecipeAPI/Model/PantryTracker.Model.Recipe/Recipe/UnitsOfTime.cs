using System;
using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.Model.Recipes
{
    public class UnitsOfTime
    {
        private readonly Dictionary<string, List<string>> _units =
            new Dictionary<string, List<string>>
            {
                {"Hours", new List<string> {"hours", "hour", "h"} },
                {"Minutes", new List<string> {"minutes", "minute", "min", "mins", "m"} },
                {"Seconds", new List<string> {"seconds", "second", "sec", "secs", "s"} }
            };

        public bool isUnitOfTime(string word)
        {
            if (string.IsNullOrEmpty(word))
                return false;

            var unit = _units.Where(m => m.Value.Contains(word.ToLower()));
            return unit.Any();
        }
    }
}
