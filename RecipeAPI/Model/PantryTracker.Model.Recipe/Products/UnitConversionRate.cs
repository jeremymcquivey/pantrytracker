namespace PantryTracker.Model
{
    public class UnitConversionRate
    {
        public string PrimaryUnit { get; set; }

        public string SecondaryUnit { get; set; }

        public double ConversionScale { get; set; }

        public int? ProductId { get; set; }
    }
}
