using System;
using System.Collections.Generic;

namespace Program
{
    public struct SProducts
    {
        public SProducts(int id, string name, double price)
        {
            this.id = id;
            this.name = name;
            this.price = price;
        }

        public int id { get; }
        public string name { get; }
        public double price { get; }
    }

    public class MyShop
    {
        static public void Main(string[] args)
        {
            var orderManager = new OrderManager();
            var random = new Random();
            orderManager.InitializeProducts(random.Next(10, 15), random.Next(5, 10));

            Console.Clear();
            while (true)
            {
                try
                {
                    Console.WriteLine("All Products:");
                    foreach (var product in orderManager.GetProducts())
                    {
                        Console.WriteLine($"ID: {product.id}, Name: {product.name}, Price: {product.price}");
                    }

                    Console.WriteLine("\nAll Discount Codes:");
                    foreach (var code in orderManager.GetDiscountCodes())
                    {
                        Console.WriteLine($"Code: {code}");
                    }

                    Console.Write("\nEnter Product ID: ");
                    if (!int.TryParse(Console.ReadLine(), out int productId))
                    {
                        throw new ArgumentException("Invalid Product ID");
                    }

                    Console.Write("Enter Discount Code: ");
                    string discountCode = Console.ReadLine()?.Trim() ?? string.Empty;

                    double finalPrice = orderManager.ProcessOrder(productId, discountCode);
                    Console.WriteLine($"Final Price: {finalPrice}");
                    Console.WriteLine($"Total Earned Money: {orderManager.GetEarnedMoney()}");

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Press any key to try again...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
