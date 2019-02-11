using System;

namespace PriceCalculator.Settings
{
    [Serializable]
    public sealed class PriceCalcSettings
    {
        public double BTC_Price { get; set; }
        public double SEK_USD_Rate { get; set; }
        public double Commision { get; set; }
        public bool Topmost { get; set; }
        public bool AutofocusPrice { get; set; }
        public Coordinate StartPosition { get; set; }

        public static PriceCalcSettings GetDefaultSettings()
        {
            return new PriceCalcSettings {BTC_Price = 100, Commision = 0, SEK_USD_Rate = 7, Topmost = false};
        }
    }

    [Serializable]
    public sealed class Coordinate
    {
        public Coordinate() { }

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
