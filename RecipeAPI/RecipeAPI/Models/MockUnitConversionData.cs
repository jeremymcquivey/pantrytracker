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
                },
                new UnitConversionRate
                {
                    ConversionScale = 4,
                    PrimaryUnit = "gallon",
                    SecondaryUnit = "quart"
                },
                new UnitConversionRate
                {
                    ConversionScale = 8,
                    PrimaryUnit = "gallon",
                    SecondaryUnit = "pint"
                },
                new UnitConversionRate
                {
                    ConversionScale = 16,
                    PrimaryUnit = "gallon",
                    SecondaryUnit = "cup"
                },
                new UnitConversionRate
                {
                    ConversionScale = 2,
                    PrimaryUnit = "quart",
                    SecondaryUnit = "pint",
                },
                new UnitConversionRate
                {
                    ConversionScale = 4,
                    PrimaryUnit = "quart",
                    SecondaryUnit = "cup"
                },
                new UnitConversionRate
                {
                    ConversionScale = 2,
                    PrimaryUnit = "pint",
                    SecondaryUnit = "cup"
                }
            });
        }
    }
}
