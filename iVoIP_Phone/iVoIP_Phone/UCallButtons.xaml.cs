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
    /// Interaction logic for UCallButtons.xaml
    /// </summary>
    public partial class UCallButtons : UserControl
    {
        public Button Btn { get { return button1; } set { button1 = value; } }

        public event EventHandler CallButtonClicked;
        public event EventHandler DisconnectCall;
        public event EventHandler WorkcodeBtnClicked;
        public event EventHandler HoldButtonClicked;
        public event EventHandler toIVRinHangupbtnClicked;
        public event EventHandler Transfer;
        public event EventHandler MuteBtnClicked;

        bool muteClicked = false;

        public enum View
        {
            Workcode,
            InCall,
            PreCall,
            InCallWorkCode
        }

        public View CurrentView { get; set; }

        public bool call = false;

        public UCallButtons()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!call)
            {
                call = true;
                if (CallButtonClicked != null) CallButtonClicked(this, e);
            }
            else if (call)
            {
                call = false;
                if (DisconnectCall != null) DisconnectCall(this, e);
            }
        }

        public void WorkcodeView()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                button1.IsEnabled = false;
                button2.IsEnabled = true;
                button3.IsEnabled = false;
                button4.IsEnabled = false;
                button5.IsEnabled = false;
                button6.IsEnabled = false;
                CurrentView = View.Workcode;
            }));
        }

        public void OnShow()
        {
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = false;
            button4.IsEnabled = false;
            button5.IsEnabled = false;
            button6.IsEnabled = false;
        }

        public void PreCallView()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                button1.IsEnabled = true;
                button2.IsEnabled = false;
                button3.IsEnabled = false;
                button4.IsEnabled = false;
                button5.IsEnabled = false;
                button6.IsEnabled = false;
                CurrentView = View.PreCall;
            }));
        }

        public void InCallView()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                button1.IsEnabled = true;
                button2.IsEnabled = true;
                button3.IsEnabled = true;
                button4.IsEnabled = true;
                button5.IsEnabled = true;
                button6.IsEnabled = true;
                CurrentView = View.InCall;
            }));
        }

        public void InCallWorkCodeView()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                button1.IsEnabled = true;
                button2.IsEnabled = false;
                button3.IsEnabled = true;
                button4.IsEnabled = true;
                button5.IsEnabled = true;
                button6.IsEnabled = true;
                CurrentView = View.InCallWorkCode;
            }));
        }

        public void btnsIncomingCallView()
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    button1.IsEnabled = true;
                    button2.IsEnabled = false;
                    button3.IsEnabled = false;
                    button4.IsEnabled = false;
                    button5.IsEnabled = false;
                    button6.IsEnabled = true;

                    ChangeImage();
                }));
        }

        private void ChangeImage()
        {
            var uri = new Uri(@"hangup.png", UriKind.RelativeOrAbsolute);
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(uri);
            brush.Stretch = Stretch.None;
            button6.Background = brush;
            button6.ToolTip = "Hangup";
        }

        public void ReChangeImage()
        {
            var uri = new Uri(@"response.png", UriKind.RelativeOrAbsolute);
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(uri);
            brush.Stretch = Stretch.None;
            button6.Background = brush;
            button6.ToolTip = "To IVR";
        }

        void ChangeMuteImage()
        {
            var uri = new Uri(@"mute.png", UriKind.RelativeOrAbsolute);
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(uri);
            brush.Stretch = Stretch.None;
            button4.Background = brush;
            button4.ToolTip = "UnMute";
        }

        void ReChangeMuteImage()
        {
            var uri = new Uri(@"volume24.png", UriKind.RelativeOrAbsolute);
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(uri);
            brush.Stretch = Stretch.None;
            button4.Background = brush;
            button4.ToolTip = "Mute";
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (WorkcodeBtnClicked != null) WorkcodeBtnClicked(this, e);
        }

        private void button3_Checked(object sender, RoutedEventArgs e)
        {
            if (HoldButtonClicked != null) HoldButtonClicked(this, e);
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (button6.ToolTip.ToString() == "Hangup")
            {
                if (toIVRinHangupbtnClicked != null) toIVRinHangupbtnClicked(this, e);
            }
            else if (button6.ToolTip.ToString() == "To IVR") return;
        }

        private void button3_Unchecked(object sender, RoutedEventArgs e)
        {
            if (HoldButtonClicked != null) HoldButtonClicked(this, e);
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (Transfer != null) Transfer(this, e);
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (!muteClicked)
            {
                if (MuteBtnClicked != null) MuteBtnClicked(this, e);
                ChangeMuteImage();
                muteClicked = true;
            }
            else if (muteClicked)
            {
                if (MuteBtnClicked != null) MuteBtnClicked(this, e);
                ReChangeMuteImage();
                muteClicked = false;
            }
        }
    }
}
