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
    /// Interaction logic for UTable.xaml
    /// </summary>
    public partial class UTable : UserControl
    {
        public string DialCalls { get { return textBlock6.Text; } set { textBlock6.Text = value; } }
        public string RcvCalls { get { return textBlock9.Text; } set { textBlock9.Text = value; } }
        public string AnsCalls { get { return textBlock10.Text; } set { textBlock10.Text = value; } }

        public string PSL { get { return textBlock13.Text; } set { textBlock13.Text = value; } }
        public string GSL { get { return textBlock14.Text; } set { textBlock14.Text = value; } }

        public string ASD { get { return textBlock17.Text; } set { textBlock17.Text = value; } }
        public string AHT { get { return textBlock18.Text; } set { textBlock18.Text = value; } }
        public string ATT { get { return textBlock20.Text; } set { textBlock20.Text = value; } }

        public string TLD { get { return textBlock22.Text; } set { textBlock22.Text = value; } }
        public string Talk { get { return textBlock24.Text; } set { textBlock24.Text = value; } }
        public string Hold { get { return textBlock28.Text; } set { textBlock28.Text = value; } }
        public string Nrdy { get { return textBlock29.Text; } set { textBlock29.Text = value; } }
        public string Rdy { get { return textBlock30.Text; } set { textBlock30.Text = value; } }

        public UTable()
        {
            InitializeComponent();
        }
    }
}
