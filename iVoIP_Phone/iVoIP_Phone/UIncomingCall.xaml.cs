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
using System.Timers;

namespace iVoIP_Phone
{
    /// <summary>
    /// Interaction logic for UIncomingCall.xaml
    /// </summary>
    public partial class UIncomingCall : UserControl
    {
        Timer timer;
        private string number = "";
        public string Number { get { return number; } set { number = value; } }

        public TextBlock TalkTime   { get { return textBlock7;  } set { textBlock7 = value;     } }
        public TextBlock HoldTime   { get { return textBlock8;  } set { textBlock8 = value;     } }
        public TextBlock AnsDelay   { get { return textBlock4;  } set { textBlock4 = value;     } }
        public TextBlock HandleTime { get { return textBlock10; } set { textBlock10 = value;    } }

        public UIncomingCall()
        {
            InitializeComponent();
            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    if (image1.Visibility == System.Windows.Visibility.Visible)
                        image1.Visibility = System.Windows.Visibility.Hidden;
                    else image1.Visibility = System.Windows.Visibility.Visible;
                }));
        }

        private void ChildGrid_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        public void TimerStop()
        {
            timer.Stop();
            image1.Visibility = System.Windows.Visibility.Visible;
        }

        public void InCallShow(string number)
        {
            Number = number;
            textBlock2.Text = number;
        }
    }
}
