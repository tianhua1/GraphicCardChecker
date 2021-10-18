using System.Globalization;

namespace GraphicCardChecker.Model
{
    public class GraphicCard
    {
        private const string Card3090 = "3090";
        private const int Price3090 = 2200;
        private const string Card3080 = "3080";
        private const int Price3080 = 1500;
        private const string Card3070 = "3070";
        private const int Price3070 = 900;
        private const string Card3060 = "3060";
        private const int Price3060 = 500;
        public GraphicCard(string name, string brand, string price)
        {
            Price = price;
            Name = name;
            Brand = brand;
            if (CheckPrice(Name, Price))
            {
                SendAlert = true;
            }

        }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Price { get; set; }
        public bool SendAlert { get; set; }

        private bool CheckPrice(string name, string price)
        {
            var pricedecimal = int.Parse(price.Remove(price.Length -3).Replace("€", "").Replace(",", ""));
            if (name.Contains(Card3090) && pricedecimal <= Price3090) return true;
            if (name.Contains(Card3080) && pricedecimal <= Price3080) return true;
            if (name.Contains(Card3070) && pricedecimal <= Price3070) return true;
            if (name.Contains(Card3060) && pricedecimal <= Price3060) return true;
            return false;
        }

        public string FormatMessage() {
            return Name + " " + Price + "\n";
        }
    }
}
