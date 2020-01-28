using System;
using System.Collections.Generic;

namespace RecipeAPI.MockData
{
    public class MockUnitAliasData : List<Tuple<string, string>>
    {
        public MockUnitAliasData()
        {
            AddRange(new List<Tuple<string, string>>
            {
                new Tuple<string, string>("lb", "lbs"),
                new Tuple<string, string>("oz", "ozs"),
                new Tuple<string, string>("cup", "cups"),
                new Tuple<string, string>("cup", "c")
            });
        }
    }
}
