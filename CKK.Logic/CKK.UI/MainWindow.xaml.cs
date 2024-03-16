using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CKK.DB.Interfaces;
using CKK.DB.UOW;

namespace CKK.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConnectionFactory _connectionFactory;
        public MainWindow()
        {
            InitializeComponent();
            InventoryManagementForm inven = new InventoryManagementForm(_connectionFactory);
            inven.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(usernameBox.Text == "" && passwordBox.Password == "")
            {
                InventoryManagementForm inven = new InventoryManagementForm(_connectionFactory);
                inven.Show();
                this.Close();
            }
        }
    }
}
