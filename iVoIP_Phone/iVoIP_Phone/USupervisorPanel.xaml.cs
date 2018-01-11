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
    /// Interaction logic for USupervisorPanel.xaml
    /// </summary>
    public partial class USupervisorPanel : UserControl
    {
        CDatabaseClass dataClass;
        TreeViewItem itemTree = new TreeViewItem();
        SipekResources _sipekInstance;

        public USupervisorPanel(SipekResources instance)
        {
            InitializeComponent();
            dataClass = new CDatabaseClass();
            _sipekInstance = instance;
        }

        public void OnShow()
        {
            itemTree.Header = "Please select a User to Monitor";
            itemTree.ItemsSource = dataClass.GetUsersToMonitor();
            treeView1.Items.Clear();
            treeView1.Items.Add(itemTree);
        }

        private void treeView1_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (button4.Content == "Hangup")
            {
                _sipekInstance.CallManager.onUserRelease
                    (_sipekInstance.CallManager.getCallInState
                    (Sipek.Common.EStateId.ACTIVE).Session);
                button4.Content = "Back";
            }
            else this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (treeView1.SelectedItem == itemTree) return;
            var exten = dataClass.GetExtenFromName(treeView1.SelectedItem.ToString());
            _sipekInstance.CallManager.createOutboundCall("*222" + exten.Trim());
            button4.Content = "Hangup";
            //this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (treeView1.SelectedItem == itemTree) return;
            var exten = dataClass.GetExtenFromName(treeView1.SelectedItem.ToString());
            _sipekInstance.CallManager.createOutboundCall("*223" + exten.Trim());
            button4.Content = "Hangup";
            //this.Visibility = System.Windows.Visibility.Hidden;
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (treeView1.SelectedItem == itemTree) return;
            var exten = dataClass.GetExtenFromName(treeView1.SelectedItem.ToString());
            _sipekInstance.CallManager.createOutboundCall("*224" + exten.Trim());
            button4.Content = "Hangup";
            //this.Visibility = System.Windows.Visibility.Hidden;
        }


    }
}
