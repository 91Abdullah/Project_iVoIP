using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Diagnostics;
using System.Windows.Threading;

namespace iVoIP_Phone
{
    public class UITimer
    {
        Timer timer;
        Stopwatch watch;
        MainWindow instance;

        public UITimer(MainWindow window)
        {
            timer = new Timer();
            watch = new Stopwatch();
            instance = window;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed_Main);
        }

        void timer_Elapsed_Main(object sender, ElapsedEventArgs e)
        {
            instance.Dispatcher.Invoke(new Action(() =>
                {
                    instance.textBlock5.Text = String.Format("{0:00}:{1:00}:{2:00}",
                        watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds);
                }));
        }

        public void Reset()
        {
            watch.Reset();
        }

        public void Start()
        {
            timer.Start();
            watch.Start();
        }

        public void Stop()
        {
            timer.Stop();
            watch.Stop();
        }

        void timer_tick(object sender, EventArgs e)
        {
            
        }
    }
}
