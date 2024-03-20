using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace _301278551_Pandit_Lab4
{
    public class Product : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public double Price { get; set; }

        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (quantity != value)
                {
                    quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(TotalPrice)); 
                }
            }
        }

        public double TotalPrice { get; set; }

        public Product(int id, string name, string category, double price)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Quantity = 0;
        }

        public static IEnumerable<Product> GetAll()
        {
            return new List<Product>
            {
                new Product(1, "Soda", "Beverage", 1.95),
                new Product(2, "Tea", "Beverage", 1.50),
                new Product(3, "Coffee", "Beverage", 1.25),
                new Product(4, "Mineral Water", "Beverage", 2.95),
                new Product(5, "Juice", "Beverage", 2.50),
                new Product(6, "Milk", "Beverage", 1.50),

                new Product(7, "Buffalo Wings", "Appetizer", 5.95),
                new Product(8, "Buffalo Fingers", "Appetizer", 6.95),
                new Product(9, "Potato Skins", "Appetizer", 8.95),
                new Product(10, "Nachos", "Appetizer", 8.95),
                new Product(11, "Mushroom Caps", "Appetizer", 10.95),
                new Product(12, "Shrimp Cocktail", "Appetizer", 12.95),
                new Product(13, "Chips and Salsa", "Appetizer", 6.95),

                new Product(14, "Seafood Alfredo", "Main Course", 15.95),
                new Product(15, "Chicken Alfredo", "Main Course", 13.95),
                new Product(16, "Chicken Picatta", "Main Course", 13.95),
                new Product(17, "Turkey Club", "Main Course", 11.95),
                new Product(18, "Lobster Pie", "Main Course", 19.95),
                new Product(19, "Prime Rib", "Main Course", 20.95),
                new Product(20, "Shrimp Scampi", "Main Course", 18.95),
                new Product(21, "Turkey Dinner", "Main Course", 13.95),
                new Product(22, "Stuffed Chicken", "Main Course", 14.95),

                new Product(23, "Apple Pie", "Dessert", 5.95),
                new Product(24, "Sundae", "Dessert", 3.95),
                new Product(25, "Carrot Cake", "Dessert", 5.95),
                new Product(26, "Mud Pie", "Dessert", 4.95),
                new Product(27, "Apple Crisp", "Dessert", 5.95)
            };
        }

        public static Product GetById(int id)
        {
            return GetAll().FirstOrDefault(p => p.Id == id);
        }

        public static List<Product> GetByCategory(string category)
        {
            return GetAll().Where(p => p.Category == category).ToList();
        }

        public override string ToString()
        {
            return $"{Name} - {Category} - {Price:C}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
