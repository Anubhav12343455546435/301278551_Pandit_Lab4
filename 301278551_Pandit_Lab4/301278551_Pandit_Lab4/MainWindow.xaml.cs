using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _301278551_Pandit_Lab4
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Product> products = new ObservableCollection<Product>();
        private ObservableCollection<Product> billItems = new ObservableCollection<Product>();
        private double subtotal = 0.0;
        private const double TaxRate = 0.13;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged(nameof(Products));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();

            products.CollectionChanged += Products_CollectionChanged;

            cmbBeverage.ItemsSource = Product.GetByCategory("Beverage");
            cmbAppetizer.ItemsSource = Product.GetByCategory("Appetizer");
            cmbMainCourse.ItemsSource = Product.GetByCategory("Main Course");
            cmbDessert.ItemsSource = Product.GetByCategory("Dessert");
        }

        private void LoadData()
        {
            foreach (var product in Product.GetAll())
            {
                products.Add(product);
            }
        }

        private void Products_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateBill();
        }

        private void cmbBeverage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddOrUpdateItem(cmbBeverage.SelectedItem as Product);
        }

        private void cmbAppetizer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddOrUpdateItem(cmbAppetizer.SelectedItem as Product);
        }

        private void cmbMainCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddOrUpdateItem(cmbMainCourse.SelectedItem as Product);
        }

        private void cmbDessert_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddOrUpdateItem(cmbDessert.SelectedItem as Product);
        }

        private void AddOrUpdateItem(Product item)
        {
            if (item == null)
                return;

            var existingItem = Products.FirstOrDefault(p => p.Name == item.Name && p.Category == item.Category);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                item.Quantity = 1;
                Products.Add(item);
            }

            var billItem = billItems.FirstOrDefault(p => p.Name == item.Name && p.Category == item.Category);
            if (billItem != null)
            {
                billItem.Quantity++;
                UpdateItemTotalPrice(billItem);
            }
            else
            {
                var newItem = new Product(item.Id, item.Name, item.Category, item.Price) { Quantity = 1 };
                billItems.Add(newItem);
                UpdateItemTotalPrice(newItem);
            }

            UpdateBill();
            RefreshBill();
        }

        private void QuantityChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.DataContext is Product product)
                {
                    if (int.TryParse(textBox.Text, out int newQuantity))
                    {
                        product.Quantity = newQuantity;
                        product.TotalPrice = product.Price * newQuantity; 
                        UpdateBill(); 
                    }
                }
            }
        }




        private void UpdateItemTotalPrice(Product product)
        {
            product.TotalPrice = product.Price * product.Quantity;
        }


        private void DataGridRow_Delete(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Product product)
            {
                Products.Remove(product);
                billItems.Remove(product);
                UpdateBill();
            }
        }


        private void UpdateBill()
        {
            subtotal = billItems.Sum(item => item.TotalPrice);
            double tax = subtotal * TaxRate;
            double total = subtotal + tax;

            txtSubtotal.Text = subtotal.ToString("C");
            txtTax.Text = tax.ToString("C");
            txtTotal.Text = total.ToString("C");
        }

        private void RefreshBill()
        {

            StockGrid.ItemsSource = null;

            StockGrid.ItemsSource = billItems;

            subtotal = billItems.Sum(item => item.TotalPrice);
            double tax = subtotal * TaxRate;
            double total = subtotal + tax;

            txtSubtotal.Text = subtotal.ToString("C");
            txtTax.Text = tax.ToString("C");
            txtTotal.Text = total.ToString("C");
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ClearBillButton_Click(object sender, RoutedEventArgs e)
        {
            ClearBill();
        }

        private void ClearBill()
        {

            billItems.Clear();
            UpdateBill();

            cmbBeverage.ItemsSource = Product.GetByCategory("Beverage");
            cmbAppetizer.ItemsSource = Product.GetByCategory("Appetizer");
            cmbMainCourse.ItemsSource = Product.GetByCategory("Main Course");
            cmbDessert.ItemsSource = Product.GetByCategory("Dessert");
        }

        private void OpenWebsite(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                startInfo.Arguments = "https://www.centennialcollege.ca/";
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while opening website: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StockGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


}

