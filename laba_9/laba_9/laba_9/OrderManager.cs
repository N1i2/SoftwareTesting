using Program;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    public class OrderManager
    {
        const string PRODUCT_NAME = "Boot";
        const int COUNT_SYMBLE_PASSWORD = 8;

        private const double PROCENT_OF_PRICE_WITH_DISCOUNT = 0.75;
        private List<SProducts> products;
        private List<string> discountCodes;
        private double earnedMoney;

        public OrderManager()
        {
            products = new List<SProducts>();
            discountCodes = new List<string>();
            earnedMoney = 0.0;
        }

        public void InitializeProducts(int productCount, int discountCount)
        {
            var random = new Random();
            for (int i = 0; i < productCount; i++)
            {
                products.Add(new SProducts(i, $"{PRODUCT_NAME} {i + 1}", Math.Round(random.Next(100, 1000) / 10.0, 2)));
            }

            for (int i = 0; i < discountCount; i++)
            {
                discountCodes.Add(GetRandomString(COUNT_SYMBLE_PASSWORD, random));
            }
        }

        public List<SProducts> GetProducts() => products;

        public List<string> GetDiscountCodes() => discountCodes;

        public double GetEarnedMoney() => earnedMoney;

        public double? FindPriceById(int id)
        {
            if (products.FindIndex(p => p.id == id) == -1)
            {
                return null;
            }
            return products.Find(p => p.id == id).price;
        }

        public bool ApplyDiscountCode(string code)
        {
            if (!discountCodes.Contains(code))
            {
                return false;
            }

            discountCodes.Remove(code);
            return true;
        }

        public double ProcessOrder(int productId, string discountCode)
        {
            double? price = FindPriceById(productId);
            if (price == null)
            {
                throw new ArgumentException("Product not found");
            }

            bool isDiscountApplied = ApplyDiscountCode(discountCode);
            double finalPrice = isDiscountApplied
                ? Math.Round(price.Value * PROCENT_OF_PRICE_WITH_DISCOUNT, 2)
                : price.Value;

            earnedMoney += finalPrice;
            return finalPrice;
        }

        private static string GetRandomString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] buffer = new char[length];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = chars[random.Next(chars.Length)];
            }
            return new string(buffer);
        }
    }
}
