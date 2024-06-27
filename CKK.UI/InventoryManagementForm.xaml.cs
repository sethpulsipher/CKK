using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CKK.Logic.Models;
using Microsoft.Win32;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CKK.DB.Repository;
using CKK.DB.Interfaces;
using CKK.DB.UOW;
using static Dapper.SqlMapper;
using System.Windows.Controls.Primitives;
using System.Net.Http;

namespace CKK.UI
{
    /// <summary>
    /// Interaction logic for InventoryManagementForm.xaml
    /// </summary>
    public partial class InventoryManagementForm : Window
    {
        private readonly IConnectionFactory _connectionFactory;
        IUnitOfWork Services;
        public bool descendingOrder;
        IEnumerable<Product> products;
        public InventoryManagementForm(IConnectionFactory Conn)
        {
            InitializeComponent();
            _connectionFactory = Conn;
            Services = new UnitOfWork(_connectionFactory);
        }
        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Clear the items in output box
                outputBox.Items.Clear();
                //Get all products 
                products = Services.Products.GetAll();
                
                //Enabling sortby options, while making sure descending order is off
                sortBy.IsEnabled = true;
                descCheckBox.IsEnabled = true;
                descCheckBox.IsChecked = false;
                //Populates data grid
                PopulateDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Assigning values from inputboxes
                decimal price = decimal.Parse(priceBox.Text);
                int quantity = int.Parse(quantityBox.Text);
                string name = nameBox.Text;
                
                //Make new product with user inputs
                var prod = new Product() { Price = price, Quantity = quantity, Name = name };
                Services.Products.Add(prod);
                
                ResetText();
            }
            catch(Exception ex) 
            { 
                MessageBox.Show(ex.Message); 
            }
        }
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                int id = int.Parse(idBox.Text);

                Services.Products.Delete(id);

                idBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id = int.Parse(idBox.Text);
                outputBox.Items.Clear();
                Product productReturned = Services.Products.GetById(id);

                outputBox.Items.Add(productReturned);

                ResetText();
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                int id = int.Parse(idBox.Text);
                decimal? price = decimal.Parse(priceBox.Text);
                int? quantity = int.Parse(quantityBox.Text);
                string? name = nameBox.Text;

                var prod = new Product() { Id = id, Price = (decimal)price, Quantity = (int)quantity, Name = name };
                Services.Products.Update(prod);

                ResetText();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void resetTextButton_Click(object sender, RoutedEventArgs e)
        {
            ResetText();
        }

        private void nameSearchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = nameBox.Text;
                outputBox.Items.Clear();
                List<Product> productsReturned = Services.Products.GetByName(name);
                foreach (Product product in productsReturned)
                {
                    outputBox.Items.Add(product);
                }
                ResetText();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString();

            switch (selected)
            {
                case "Id":
                    products = Services.Products.GetAll().OrderBy(product => product.Id);
                    break;
                case "Name":
                    products = Services.Products.GetAll().OrderBy(product => product.Name);
                    break;
                case "Quantity":
                    products = Services.Products.GetAll().OrderBy(product => product.Quantity);
                    break;
                case "Price":
                    products = Services.Products.GetAll().OrderBy(product => product.Price);
                    break;
                default:
                    products = products; // No sorting
                    break;
            }

            if(descendingOrder == true)
            {
                var reversed = products.Reverse();
                products = reversed;
            }

            PopulateDataGrid();
        }

        private void descCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            descendingOrder = true;
            var reversed = products.Reverse();
            products = reversed;
            PopulateDataGrid();
        }

        private void descCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            descendingOrder = false;
            var reversed = products.Reverse();
            products = reversed;
            PopulateDataGrid();
        }

        public void PopulateDataGrid()
        {
            outputBox.Items.Clear();

            foreach (var item in products)
            {
                outputBox.Items.Add(item);
            }
        }
        public void ResetText()
        {
            idBox.Clear();
            priceBox.Clear();
            quantityBox.Clear();
            nameBox.Clear();
        }
    }
}
