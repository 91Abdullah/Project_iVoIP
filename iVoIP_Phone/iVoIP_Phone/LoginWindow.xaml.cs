using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace iVoIP_Phone
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : MetroWindow
    {
        CDatabaseClass dataClass;

        public LoginWindow()
        {
            InitializeComponent();
            dataClass = new CDatabaseClass();
            textBox1.Focus();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (dataClass.AuthenticateUser(textBox1.Text, passwordBox1.Password))
            {
                MainWindow window = new MainWindow(dataClass);
                window.Show();
                Close();
            }
            else MessageBox.Show("Please Login again with correct credentials", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
