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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iVoIP_Phone
{
    /// <summary>
    /// Interaction logic for UTransfer.xaml
    /// </summary>
    public partial class UTransfer : UserControl
    {
        CDatabaseClass dataClass;
        TreeViewItem item;
        public string exten = "";

        public event EventHandler XferNow;

        public UTransfer(CDatabaseClass Instance)
        {
            dataClass = Instance;
            item = new TreeViewItem();
            item.MouseDoubleClick += new MouseButtonEventHandler(item_MouseDoubleClick);
            InitializeComponent();
        }

        void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var newItem = sender as TreeViewItem;
            if (treeView1.SelectedItem == item) return;
            else exten = dataClass.GetExtenFromName(treeView1.SelectedItem.ToString());
            if (XferNow != null) XferNow(this, e);
        }

        private void treeView1_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void Load()
        {
            item.Header = "Please select a User to Xfer";
            item.ItemsSource = dataClass.GetUsersToTransfer();
            treeView1.Items.Clear();
            treeView1.Items.Add(item);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
