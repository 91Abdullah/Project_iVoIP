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
    /// Interaction logic for UWorkCodeWindow.xaml
    /// </summary>
    public partial class UWorkCodeWindow : UserControl
    {
        CDatabaseClass dataClass = new CDatabaseClass();
        TreeViewItem item = new TreeViewItem();

        public bool isCodeSelected = false;
        public string SelectedCode { get; set; }

        public event EventHandler NodeClicked;

        public UWorkCodeWindow()
        {
            InitializeComponent();
            item.MouseDoubleClick += new MouseButtonEventHandler(item_MouseDoubleClick);
        }

        void item_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var workcode = sender as TreeViewItem;
            if (treeView1.SelectedItem == item) return;
            else SelectedCode = treeView1.SelectedItem.ToString();
            isCodeSelected = true;
            if (NodeClicked != null) NodeClicked(this, e);
        }

        private void treeView1_Loaded(object sender, RoutedEventArgs e)
        {
            item.Header = "Please Select a Workcode";
            item.ItemsSource = dataClass.GetWorkCodes();
            treeView1.Items.Add(item);
        }
    }
}
