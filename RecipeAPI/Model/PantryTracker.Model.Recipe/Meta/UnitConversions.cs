using System.Collections.Generic;

namespace PantryTracker.Model.Meta
{
    public class UnitConversions : List<UnitConversionRate>
    {
        public const int DecimalPrecision = 1;

        public UnitConversions()
        {
            AddRange(new List<UnitConversionRate>
            {
                // **** Units of Weight **** \\
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Gram,
                    ConversionScale = 1000,
                    DestinationUnit = UnitsOfWeight.Milligram
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Ounce,
                    ConversionScale = 28349.5,
                    DestinationUnit = UnitsOfWeight.Milligram
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Ounce,
                    ConversionScale = 28.5,
                    DestinationUnit = UnitsOfWeight.Gram
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Pound,
                    ConversionScale = 453592,
                    DestinationUnit = UnitsOfWeight.Milligram
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Pound,
                    ConversionScale = 453.6,
                    DestinationUnit = UnitsOfWeight.Gram
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Pound,
                    ConversionScale = 16,
                    DestinationUnit = UnitsOfWeight.Ounce
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Kilogram,
                    ConversionScale = 1000000,
                    DestinationUnit = UnitsOfWeight.Milligram
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Kilogram,
                    ConversionScale = 1000,
                    DestinationUnit = UnitsOfWeight.Gram
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Kilogram,
                    ConversionScale = 35.3,
                    DestinationUnit = UnitsOfWeight.Ounce
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Kilogram,
                    ConversionScale = 2.2,
                    DestinationUnit = UnitsOfWeight.Pound
                },

                // **** Other Conversions **** \\
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Teaspoon,
                    ConversionScale = 8,
                    DestinationUnit = OtherUnits.Pinch
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Teaspoon,
                    ConversionScale = 8,
                    DestinationUnit = OtherUnits.Dash
                },
                new UnitConversionRate
                {
                    SourceUnit = OtherUnits.Jigger,
                    ConversionScale = 1.5,
                    DestinationUnit = UnitsOfVolume.FluidOunce
                },

                // **** Units of Volume **** \\
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Teaspoon,
                    ConversionScale = 5.9,
                    DestinationUnit = UnitsOfVolume.Milliliter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Tablespoon,
                    ConversionScale = 17.8,
                    DestinationUnit = UnitsOfVolume.Milliliter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Tablespoon,
                    ConversionScale = 3,
                    DestinationUnit = UnitsOfVolume.Teaspoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.FluidOunce,
                    ConversionScale = 28.4,
                    DestinationUnit = UnitsOfVolume.Milliliter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.FluidOunce,
                    ConversionScale = 4.8,
                    DestinationUnit = UnitsOfVolume.Teaspoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.FluidOunce,
                    ConversionScale = 1.6,
                    DestinationUnit = UnitsOfVolume.Tablespoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Cup,
                    ConversionScale = 284.1,
                    DestinationUnit = UnitsOfVolume.Milliliter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Cup,
                    ConversionScale = 48,
                    DestinationUnit = UnitsOfVolume.Teaspoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Cup,
                    ConversionScale = 16,
                    DestinationUnit = UnitsOfVolume.Tablespoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Cup,
                    ConversionScale = 10,
                    DestinationUnit = UnitsOfVolume.FluidOunce
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Pint,
                    ConversionScale = 568.3,
                    DestinationUnit = UnitsOfVolume.Milliliter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Pint,
                    ConversionScale = 96,
                    DestinationUnit = UnitsOfVolume.Teaspoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Pint,
                    ConversionScale = 32,
                    DestinationUnit = UnitsOfVolume.Tablespoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Pint,
                    ConversionScale = 20,
                    DestinationUnit = UnitsOfVolume.FluidOunce
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Pint,
                    ConversionScale = 2,
                    DestinationUnit = UnitsOfVolume.Cup
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Liter,
                    ConversionScale = 1000,
                    DestinationUnit = UnitsOfVolume.Milliliter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Liter,
                    ConversionScale = 168.9,
                    DestinationUnit = UnitsOfVolume.Teaspoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Liter,
                    ConversionScale = 56.3,
                    DestinationUnit = UnitsOfVolume.Tablespoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Liter,
                    ConversionScale = 35.2,
                    DestinationUnit = UnitsOfVolume.FluidOunce
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Liter,
                    ConversionScale = 3.5,
                    DestinationUnit = UnitsOfVolume.Cup
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Liter,
                    ConversionScale = 1.8,
                    DestinationUnit = UnitsOfVolume.Pint
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Quart,
                    ConversionScale = 1136.5,
                    DestinationUnit = UnitsOfVolume.Milliliter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Quart,
                    ConversionScale = 192,
                    DestinationUnit = UnitsOfVolume.Teaspoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Quart,
                    ConversionScale = 64,
                    DestinationUnit = UnitsOfVolume.Tablespoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Quart,
                    ConversionScale = 40,
                    DestinationUnit = UnitsOfVolume.FluidOunce
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Quart,
                    ConversionScale = 4,
                    DestinationUnit = UnitsOfVolume.Cup
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Quart,
                    ConversionScale = 2,
                    DestinationUnit = UnitsOfVolume.Pint
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Quart,
                    ConversionScale = 1.1,
                    DestinationUnit = UnitsOfVolume.Liter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Gallon,
                    ConversionScale = 4546.1,
                    DestinationUnit = UnitsOfVolume.Milliliter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Gallon,
                    ConversionScale = 768,
                    DestinationUnit = UnitsOfVolume.Teaspoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Gallon,
                    ConversionScale = 256,
                    DestinationUnit = UnitsOfVolume.Tablespoon
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Gallon,
                    ConversionScale = 460,
                    DestinationUnit = UnitsOfVolume.FluidOunce
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Gallon,
                    ConversionScale = 16,
                    DestinationUnit = UnitsOfVolume.Cup
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Gallon,
                    ConversionScale = 8,
                    DestinationUnit = UnitsOfVolume.Pint
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Gallon,
                    ConversionScale = 4.5,
                    DestinationUnit = UnitsOfVolume.Liter
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Gallon,
                    ConversionScale = 4,
                    DestinationUnit = UnitsOfVolume.Quart
                },

                // **** Product Specific Conversions **** \\
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Pound,
                    ConversionScale = 4,
                    DestinationUnit = UnitsOfVolume.Cup,
                    Products = new int?[0] //Cheese
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Pound,
                    ConversionScale = 4,
                    DestinationUnit = OtherUnits.Cube,
                    Products = new int?[0] //Butter, Margarine
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfVolume.Cup,
                    ConversionScale = 2,
                    DestinationUnit = OtherUnits.Cube,
                    Products = new int?[0] //Butter, Margarine
                },
                new UnitConversionRate
                {
                    SourceUnit = UnitsOfWeight.Ounce,
                    ConversionScale = 1,
                    DestinationUnit = OtherUnits.Square,
                    Products = new int?[0] //Baking Chocolate
                }
            });
        }
    }
}
