namespace PantryTracker.Model
{
    public class UnitConversionRate
    {
        public string SourceUnit { get; set; }

        public string DestinationUnit { get; set; }

        public double ConversionScale { get; set; }

        public int[] Products { get; set; }
    }
}
