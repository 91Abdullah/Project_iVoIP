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
    /// Interaction logic for UCallHistory.xaml
    /// </summary>
    public partial class UCallHistory : UserControl
    {
        TreeViewItem item1;
        TreeViewItem item2;
        TreeViewItem item3;

        ContextMenu menu;
        Button btn;

        List<string> InCalls;
        List<string> OutCalls;
        List<string> MissedCalls;

        public event EventHandler DialCall;

        string _numtoDial;
        public string numToDial { get { return _numtoDial; } }

        public UCallHistory()
        {
            InitializeComponent();

            item1 = new TreeViewItem();
            item2 = new TreeViewItem();
            item3 = new TreeViewItem();

            item1.Header = "Incoming Calls";
            item2.Header = "Outgoing Calls";
            item3.Header = "Missed Calls";

            item1.MouseDoubleClick += new MouseButtonEventHandler(item1_MouseDoubleClick);
            item2.MouseDoubleClick += new MouseButtonEventHandler(item1_MouseDoubleClick);
            item3.MouseDoubleClick += new MouseButtonEventHandler(item1_MouseDoubleClick);

            InCalls = new List<string>();
            OutCalls = new List<string>();
            MissedCalls = new List<string>();
        }

        void item1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (sender != typeof(ItemCollection)) return;
            var item = sender as TreeViewItem;
            _numtoDial = item.Items[0].ToString().Split('.')[1].Split('-')[0].Trim();
            if (DialCall != null) DialCall(sender, e);
        }

        public void AddCall(string type, string number, DateTime dt)
        {
            if (type == "Incoming")
            {
                InCalls.Add(number + " - " + dt);
                item1.Items.Add(item1.Items.Count+1 + ". " + number + " - " + dt);
            }
            else if (type == "Outgoing")
            {
                OutCalls.Add(number + " - " + dt);
                item2.Items.Add(item2.Items.Count + 1 + ". " + number + " - " + dt);
            }
            else if (type == "Missed")
            {
                MissedCalls.Add(number + " - " + dt);
                Dispatcher.Invoke(new Action(() =>
                    {
                        item3.Items.Add(item3.Items.Count + 1 + ". " + number + " - " + dt);
                    }));
            }
        }

        public void Load()
        {
            treeView1.Items.Clear();

            item1.Header = String.Format("({0}) ", InCalls.Count) + "Incoming Calls";
            item2.Header = String.Format("({0}) ", OutCalls.Count) + "Outgoing Calls";
            item3.Header = String.Format("({0}) ", MissedCalls.Count) + "Missed Calls";
            
            treeView1.Items.Add(item1);
            treeView1.Items.Add(item2);
            treeView1.Items.Add(item3);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
