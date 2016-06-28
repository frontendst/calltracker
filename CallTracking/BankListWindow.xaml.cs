using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using CallTracking.DB_Classes;

namespace CallTracking
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class BankListWindow : Window
    {
        public BankListWindow()
        {
            InitializeComponent();
        }

        private void ComboDb_OnDropDownOpened(object sender, EventArgs e)
        {
            using (var db = new CallRecordContext())
            {
                db.Banks.Load();
                ComboBox.ItemsSource = db.Banks.Local.ToDictionary(b => b.BankName).Keys;
            }
        }

        private void CancelAddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ConfirmAddButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
