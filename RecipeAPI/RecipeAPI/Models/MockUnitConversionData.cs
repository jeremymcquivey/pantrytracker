using System.Collections.Generic;
using PantryTracker.Model;

namespace RecipeAPI.MockData
{
    public class MockUnitConversionData : List<UnitConversionRate>
    {
        public MockUnitConversionData()
        {
            AddRange(new List<UnitConversionRate>
            {
                new UnitConversionRate
                {
                    ConversionScale = 16,
                    PrimaryUnit = "lb",
                    SecondaryUnit = "oz",
                    ProductId = null
                },
                new UnitConversionRate
                {
                    ConversionScale = .39,
                    PrimaryUnit = "cup",
                    SecondaryUnit = "lb",
                    ProductId = 34
                },
                new UnitConversionRate
                {
                    ConversionScale = .25,
                    PrimaryUnit = "cup",
                    SecondaryUnit = "lb",
                    ProductId = 44
                },
                new UnitConversionRate
                {
                    ConversionScale = 4,
                    PrimaryUnit = "cup",
                    SecondaryUnit = "oz",
                    ProductId = 44
                }
            });
        }
    }
}
