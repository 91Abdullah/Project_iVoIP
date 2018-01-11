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
using MahApps.Metro.Controls;
using DNBSoft.WPF.RibbonControl;

namespace iVoIP_Phone
{
    /// <summary>
    /// Interaction logic for UKeypad.xaml
    /// </summary>
    public partial class UKeypad : UserControl
    {
        public string DialedNumber { get { return this.comboBox1.Text; } set { this.comboBox1.Text = value; } }
        public RibbonHalfButton DialButton { get { return this.ribbonHalfButton13; } set { this.ribbonHalfButton13 = value; } }

        bool call = false;

        public event EventHandler CallButtonClicked;
        public event EventHandler DisconnectCall;

        
        //DTMF Handlers
        public event EventHandler send1;
        public event EventHandler send2;
        public event EventHandler send3;
        public event EventHandler send4;
        public event EventHandler send5;
        public event EventHandler send6;
        public event EventHandler send7;
        public event EventHandler send8;
        public event EventHandler send9;
        public event EventHandler send0;
        public event EventHandler sendstar;
        public event EventHandler sendhash;

        public TextBlock HandleTime { get { return textBlock5; } set { textBlock5 = value; } }
        public TextBlock TalkTimer { get { return textBlock1; } set { textBlock1 = value; } }
        public TextBlock HoldTimer { get { return textBlock2; } set { textBlock2 = value; } }

        public UKeypad()
        {
            InitializeComponent();
        }

        void btnClick(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as RibbonHalfButton;
            comboBox1.Text += btn.Text;
            if (call)
            {
                if (btn.Text == "1") if (send1 != null) send1(this, e);
                if (btn.Text == "2") if (send2 != null) send2(this, e);
                if (btn.Text == "3") if (send3 != null) send3(this, e);
                if (btn.Text == "4") if (send4 != null) send4(this, e);
                if (btn.Text == "5") if (send5 != null) send5(this, e);
                if (btn.Text == "6") if (send6 != null) send6(this, e);
                if (btn.Text == "7") if (send7 != null) send7(this, e);
                if (btn.Text == "8") if (send8 != null) send8(this, e);
                if (btn.Text == "9") if (send9 != null) send9(this, e);
                if (btn.Text == "0") if (send0 != null) send0(this, e);
                if (btn.Text == "*") if (sendstar != null) sendstar(this, e);
                if (btn.Text == "#") if (sendhash != null) sendhash(this, e);
            }
            e.Handled = true;
        }

        public void YoDialIt(string num)
        {
            comboBox1.Text = num;
            ribbonHalfButton13_Clicked(null, null);
            Call();
        }


        private void ribbonHalfButton13_Clicked(object sender, MouseButtonEventArgs e)
        {
            if (!call)
            {
                if (CallButtonClicked != null) CallButtonClicked(this, e);
            }
            else
            {
                if (DisconnectCall != null) DisconnectCall(this, e);
            }
        }

        public void Call()
        {
            this.comboBox1.Items.Add(comboBox1.Text);
            call = true;
        }

        public void disconnectCall()
        {
            if (Dispatcher.CheckAccess())
            {
                this.comboBox1.Text = "";
            }
            else Dispatcher.Invoke(new Action(() =>
            {
                this.comboBox1.Text = "";
            }));
            call = false;
        }
    }
}
