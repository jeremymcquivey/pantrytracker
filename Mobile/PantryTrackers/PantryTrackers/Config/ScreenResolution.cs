namespace PantryTrackers.Config
{
    public class ScreenResolution
    {
        public double Width { set; get; }

        public double Height { set; get; }

        public double DPI { get; set; }

        public override string ToString()
        {
            return $"{Width} x {Height}";
        }
    }
}
