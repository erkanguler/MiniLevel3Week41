using System.Globalization;

namespace WeeklyMiniProject3
{
    class Tracker
    {
        private List<Asset> assets = new List<Asset>();

        public void AddAsset(Asset asset)
        {
            assets.Add(asset);
        }

        public List<Asset> getAssets() => assets;
    }

    class App
    {
        public const int THREE_YEAR = 365 * 3;

        public static void Main()
        {
            var app = new App();
            Tracker tracker = new Tracker();

            if (tracker.getAssets().Count < 1)
            {
                App.AddTestDataToTracker(tracker);
            }

            App.PrintAssets(tracker.getAssets());

            while (true)
            {
                App.PrintTextWithColor("\nEnter 'q' to quit.\n", ConsoleColor.Blue);
                int? productClass;
                while (true)
                {
                    string m = "Enter '1' or '2' to choose a Computer or Smartphone as a asset: \n1 = Computer\n2 = Smartphone\n";
                    App.PrintTextWithColor(m, ConsoleColor.Blue);
                    string? _productClass = App.GetInput();

                    if (_productClass is null)
                        continue;

                    App.exitIfUserWants(_productClass);

                    productClass = App.GetInputAsInteger(_productClass, 1, 2);

                    if (productClass is null)
                        continue;

                    break;
                }

                float price;
                while (true)
                {
                    App.PrintTextWithColor("Enter a price for your asset: ", ConsoleColor.Blue);
                    string? _price = App.GetInput();

                    if (_price is null)
                        continue;

                    App.exitIfUserWants(_price);

                    float? price2 = App.GetPriceAsFloat(_price);

                    if (price2 is null)
                        continue;

                    price = (float)price2;

                    break;
                }

                int? currency;
                while (true)
                {
                    string m = "Enter '1', '2' or '3' to choose a currency: \n1 = EUR for Germany\n2 = SEK for Sweden\n3 = USA for United States\n";
                    App.PrintTextWithColor(m, ConsoleColor.Blue);
                    string? _currency = App.GetInput();

                    if (_currency is null)
                        continue;

                    App.exitIfUserWants(_currency);

                    currency = App.GetInputAsInteger(_currency, 1, 3);

                    if (currency is null)
                        continue;

                    break;
                }

                DateTime purchaseDate;
                while (true)
                {
                    App.PrintTextWithColor("Enter a date (yyyy-MM-dd): ", ConsoleColor.Blue);
                    string? _purchaseDate = App.GetInput();

                    if (_purchaseDate is null)
                        continue;

                    App.exitIfUserWants(_purchaseDate);

                    if (!DateTime.TryParse(_purchaseDate, out DateTime purchaceDate2))
                        continue;

                    purchaseDate = Convert.ToDateTime(purchaceDate2);

                    break;
                }

                string? brand;
                while (true)
                {
                    App.PrintTextWithColor("Enter a brand name: ", ConsoleColor.Blue);
                    brand = App.GetInput();

                    if (brand is null)
                        continue;

                    App.exitIfUserWants(brand);

                    break;
                }

                string? model;
                while (true)
                {
                    App.PrintTextWithColor("Enter a model name: ", ConsoleColor.Blue);
                    model = App.GetInput();

                    if (model is null)
                        continue;

                    App.exitIfUserWants(model);

                    break;
                }

                int? country = null;
                while (true)
                {
                    App.PrintTextWithColor("\nEnter '1', '2' or '3' to choose a country:\n1 = Germany\n2 = Sweden\n3 = USA\n", ConsoleColor.Blue);
                    string? _country = App.GetInput();

                    if (_country is null)
                        continue;

                    App.exitIfUserWants(_country);

                    country = App.GetInputAsInteger(_country, 1, 3);

                    if (country is null)
                        continue;

                    break;
                }

                if (productClass == 1)
                    tracker.AddAsset(new Computer(new Price(price, (Currency)(currency - 1)), purchaseDate, brand, model, (Country)(country - 1)));
                else
                    tracker.AddAsset(new Smartphone(new Price(price, (Currency)(currency - 1)), purchaseDate, brand, model, (Country)(country - 1)));

                App.PrintAssets(tracker.getAssets());
                App.PrintTextWithColor("\nEnter 'q' to quit or press 'Enter' key to add more assets.", ConsoleColor.Blue);
                string? input = App.GetInput();

                if (input is null)
                    continue;

                App.exitIfUserWants(input);
            }
        }

        public static void exitIfUserWants(string input)
        {
            if (IsTokenEqual(input, "q"))
                Environment.Exit(0);
        }
        public static List<Asset> SortAssets(List<Asset> assets)
        {
            return assets.OrderBy(asset => (asset as Product)!.Country).ThenBy(asset => (asset as Product)!.PurchaceDate).ToList();
        }

        public static string FormatAssetData(string[] properties)
        {
            string s = "";
            foreach (var name in properties)
                s += name.PadRight(20);

            return s;
        }

        public static bool IsAssetGettingOlder(Asset asset)
        {
            // More than 3 years and less then 3 months
            var today = DateTime.Now;
            TimeSpan diff = today - (asset as Product)!.PurchaceDate;
            TimeSpan threeMonths = today.AddMonths(3) - today;

            if ((diff.Days > App.THREE_YEAR) & (diff.Days < App.THREE_YEAR + threeMonths.Days))
                return true;

            return false;
        }

        public static bool IsAssetTooOld(Asset asset)
        {
            // More than 3 years and more than 3 months
            var today = DateTime.Now;
            TimeSpan diff = today - (asset as Product)!.PurchaceDate;
            TimeSpan threeMonths = today.AddMonths(3) - today;

            if ((diff.Days > App.THREE_YEAR) & (diff.Days > App.THREE_YEAR + threeMonths.Days))
                return true;

            return false;
        }

        public static string? GetInput()
        {
            string? input = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(input))
                return null;

            input = input!.Trim();

            return input;
        }

        public static float? GetPriceAsFloat(string? input)
        {
            float price;
            var style = NumberStyles.AllowDecimalPoint;
            var culture = CultureInfo.CreateSpecificCulture("en-US");

            if (!float.TryParse(input, style, culture, out price) | price < 0)
            {
                PrintTextWithColor("\nProvide a price like 100 or 11.99.\n", ConsoleColor.Red);
                return null;
            }

            return price;
        }

        public static int? GetInputAsInteger(string input, int min, int max)
        {
            int number;
            if (!int.TryParse(input, out number) | number < min | number > max)
            {
                PrintTextWithColor($"\nProvide a number between {min} and {max}.\n", ConsoleColor.Red);
                return null;
            }

            return number;
        }

        public static bool IsTokenEqual(string str1, string str2)
        {
            return str1.Trim().Equals(str2.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static void PrintAssets(List<Asset> assets)
        {
            printHeader();

            foreach (Product asset in SortAssets(assets))
            {
                if (App.IsAssetTooOld(asset))
                {
                    App.PrintTextWithColor(App.FormatAssetData(asset.GetPropertyValues()), ConsoleColor.Red);
                }
                else if (App.IsAssetGettingOlder(asset))
                {
                    App.PrintTextWithColor(App.FormatAssetData(asset.GetPropertyValues()), ConsoleColor.Yellow);
                }
                else
                    Console.WriteLine(App.FormatAssetData(asset.GetPropertyValues()));
            }
        }
        public static void printHeader()
        {
            string header = "";
            var columns = new string[] {
                "Office",
                "Asset",
                "Brand",
                "Model",
                "Price (USD)",
                "Price (Local)",
                "Purchase Date"
            };

            foreach (var name in columns)
                header += name.PadRight(20);

            PrintTextWithColor($"{header}", ConsoleColor.Cyan);
        }

        public static void PrintTextWithColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void AddTestDataToTracker(Tracker tracker)
        {
            tracker.AddAsset(new Smartphone(new Price(200, Currency.USD), DateTime.Now.AddMonths(-36 + 4), "Motorola", "X3", Country.USA));
            tracker.AddAsset(new Smartphone(new Price(400, Currency.USD), DateTime.Now.AddMonths(-36 + 5), "Motorola", "X3", Country.USA));
            tracker.AddAsset(new Smartphone(new Price(400, Currency.USD), DateTime.Now.AddMonths(-36 + 10), "Motorola", "X2", Country.USA));
            tracker.AddAsset(new Smartphone(new Price(4500, Currency.SEK), DateTime.Now.AddMonths(-36 + 6), "Samsung", "Galaxy 10", Country.SWEDEN));
            tracker.AddAsset(new Smartphone(new Price(4500, Currency.SEK), DateTime.Now.AddMonths(-36 + 7), "Samsung", "Galaxy 10", Country.SWEDEN));
            tracker.AddAsset(new Smartphone(new Price(3000, Currency.SEK), DateTime.Now.AddMonths(-36 + 4), "Sony", "XPeria 7", Country.SWEDEN));
            tracker.AddAsset(new Smartphone(new Price(3000, Currency.SEK), DateTime.Now.AddMonths(-36 + 5), "Sony", "XPeria 7", Country.SWEDEN));
            tracker.AddAsset(new Smartphone(new Price(220, Currency.EUR), DateTime.Now.AddMonths(-36 + 12), "Siemens", "Brick", Country.GERMANY));
            tracker.AddAsset(new Computer(new Price(100, Currency.USD), DateTime.Now.AddMonths(-38), "Dell", "Desktop 900", Country.USA));
            tracker.AddAsset(new Computer(new Price(100, Currency.USD), DateTime.Now.AddMonths(-37), "Dell", "Desktop 900", Country.USA));
            tracker.AddAsset(new Computer(new Price(300, Currency.USD), DateTime.Now.AddMonths(-36 + 1), "Lenovo", "X100", Country.USA));
            tracker.AddAsset(new Computer(new Price(300, Currency.USD), DateTime.Now.AddMonths(-36 + 4), "Lenovo", "X200", Country.USA));
            tracker.AddAsset(new Computer(new Price(500, Currency.USD), DateTime.Now.AddMonths(-36 + 9), "Lenovo", "X300", Country.USA));
            tracker.AddAsset(new Computer(new Price(1500, Currency.SEK), DateTime.Now.AddMonths(-36 + 7), "Dell", "Optiplex 100", Country.SWEDEN));
            tracker.AddAsset(new Computer(new Price(1400, Currency.SEK), DateTime.Now.AddMonths(-36 + 8), "Dell", "Optiplex 200", Country.SWEDEN));
            tracker.AddAsset(new Computer(new Price(1300, Currency.SEK), DateTime.Now.AddMonths(-36 + 9), "Dell", "Optiplex 300", Country.SWEDEN));
            tracker.AddAsset(new Computer(new Price(1600, Currency.EUR), DateTime.Now.AddMonths(-36 + 14), "Asus", "ROG 600", Country.GERMANY));
            tracker.AddAsset(new Computer(new Price(1200, Currency.EUR), DateTime.Now.AddMonths(-36 + 4), "Asus", "ROG 500", Country.GERMANY));
            tracker.AddAsset(new Computer(new Price(1200, Currency.EUR), DateTime.Now.AddMonths(-36 + 3), "Asus", "ROG 500", Country.GERMANY));
            tracker.AddAsset(new Computer(new Price(1300, Currency.EUR), DateTime.Now.AddMonths(-36 + 2), "Asus", "ROG 500", Country.GERMANY));

            tracker.AddAsset(new Computer(new Price(1300, Currency.EUR), DateTime.Now.AddMonths(-36 - 5), "Asus", "ROG 500", Country.USA));
            tracker.AddAsset(new Computer(new Price(1300, Currency.EUR), DateTime.Now.AddMonths(-36 - 5), "Asus", "ROG 500", Country.USA));

            tracker.AddAsset(new Computer(new Price(1300, Currency.EUR), DateTime.Now.AddMonths(-36 - 2), "Asus", "ROG 500", Country.USA));
            tracker.AddAsset(new Computer(new Price(1300, Currency.EUR), DateTime.Now.AddMonths(-36 - 2), "Asus", "ROG 500", Country.USA));
        }
    }

    interface Asset { }

    class Product : Asset
    {
        public float PriceUSD { get; set; }
        public Price Price { get; set; }
        public DateTime PurchaceDate { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public Country Country { get; set; }

        public Product(Price price, DateTime purchaceDate, string brand, string model, Country country)
        {
            this.PriceUSD = Price.ConvertToUSA(price);
            this.Price = price;
            this.PurchaceDate = purchaceDate;
            this.Brand = brand;
            this.Model = model;
            this.Country = country;
        }

        public string GetAssetName() => this.GetType().Name;

        public string GetPriceInDollarAsString() => Math.Round(PriceUSD, 2).ToString();

        public string GetFormattedLocalPrice() => $"{(Price.Currency).ToString()} {(Price.getPrice()).ToString()}";

        public string GetDateAsString() => DateOnly.FromDateTime(this.PurchaceDate).ToString();

        public string[] GetPropertyValues()
        {
            return [
                Country.ToString(),
                GetAssetName(),
                Brand,
                Model,
                GetPriceInDollarAsString(),
                GetFormattedLocalPrice(),
                GetDateAsString()
            ];
        }
    }

    class Computer : Product
    {
        public Computer(
            Price price,
            DateTime purchaceDate,
            string brand,
            string model,
            Country country
        ) : base(price, purchaceDate, brand, model, country)
        { }
    }

    class Smartphone : Product
    {
        public Smartphone(
            Price price,
            DateTime purchaceDate,
            string brand,
            string model,
            Country country
        ) : base(price, purchaceDate, brand, model, country)
        { }
    }

    struct Price
    {
        private float _price;
        public Currency Currency { get; init; }

        public Price(float price, Currency currency)
        {
            this._price = price;
            this.Currency = currency;
        }

        public float getPrice()
        {
            return this._price;
        }

        public static float ConvertToUSA(Price price)
        {
            float convertedValue = -1;

            if (price.Currency == Currency.USD)
            {
                convertedValue = price.getPrice();
            }

            if (price.Currency == Currency.SEK)
            {
                convertedValue = Convert.ToSingle(price.getPrice() / 10.39);
            }

            if (price.Currency == Currency.EUR)
            {
                convertedValue = Convert.ToSingle(price.getPrice() * 1.0957);
            }

            return convertedValue;
        }
    }

    enum Currency
    {
        EUR,
        SEK,
        USD
    }

    enum Country
    {
        GERMANY,
        SWEDEN,
        USA
    }
}
