using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Timers;
using Sipek.Common;
using Sipek.Common.CallControl;
using Sipek.Sip;

namespace iVoIP_Phone
{
    public class SipekResources : AbstractFactory
    {
        IMediaProxyInterface _mediaproxy = new CMediaPlayerProxy();
        pjsipStackProxy _stackproxy = pjsipStackProxy.Instance;
        PhoneConfig _config;

        public pjsipStackProxy StackProxy { get { return _stackproxy; } set { _stackproxy = value; } }
        public PhoneConfig Configurator { get { return _config; } }
        public IMediaProxyInterface MediaProxy { get { return _mediaproxy; } set { } }

        private IRegistrar _registrar = pjsipRegistrar.Instance;
        public IRegistrar Registrar { get { return _registrar; } }

        //TODO Messaging!!

        private CCallManager _callManager = CCallManager.Instance;
        public CCallManager CallManager { get { return CCallManager.Instance; } }

        public SipekResources(PhoneConfig Instance)
        {
            _config = Instance;
            _callManager.StackProxy = _stackproxy;
            _callManager.Config = _config;
            _callManager.Factory = this;
            _callManager.MediaProxy = _mediaproxy;
            _stackproxy.Config = _config;
            _registrar.Config = _config;
        }

        #region AbstractFactory Members

        public IStateMachine createStateMachine()
        {
            return new CStateMachine();
        }

        public ITimer createTimer()
        {
            return new GUITimer();
        }

        #endregion
    }

    public class GUITimer : ITimer
    {
        Timer _guiTimer;

        public GUITimer()
        {
            //_form = mf;
            _guiTimer = new Timer();
            if (this.Interval > 0) _guiTimer.Interval = this.Interval;
            _guiTimer.Interval = 100;
            _guiTimer.Enabled = true;
            _guiTimer.Elapsed += new ElapsedEventHandler(_guiTimer_Tick);
        }

        void _guiTimer_Tick(object sender, EventArgs e)
        {
            _guiTimer.Stop();
            //_elapsed(sender, e);
             //Synchronize thread with GUI because SIP stack works with GUI thread only
            //if ((_form.IsDisposed) || (_form.Disposing) || (!_form.IsInitialized))
            //{
            //    return;
            //}
            //_form.Invoke(_elapsed, new object[] { sender, e });
        }

        public bool Start()
        {
            _guiTimer.Start();
            return true;
        }

        public bool Stop()
        {
            _guiTimer.Stop();
            return true;
        }

        private int _interval;
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; _guiTimer.Interval = value; }
        }

        private TimerExpiredCallback _elapsed;
        public TimerExpiredCallback Elapsed
        {
            set
            {
                _elapsed = value;
            }
        }
    }

    class CMediaPlayerProxy : IMediaProxyInterface
    {
        SoundPlayer player = new SoundPlayer();

        #region Methods

        public int playTone(ETones toneId)
        {
            string fname;

            switch (toneId)
            {
                case ETones.EToneDial:
                    fname = "Sounds/dial.wav";
                    break;
                case ETones.EToneCongestion:
                    fname = "Sounds/congestion.wav";
                    break;
                case ETones.EToneRingback:
                    fname = "Sounds/ringback.wav";
                    break;
                case ETones.EToneRing:
                    fname = "Sounds/ring.wav";
                    break;
                default:
                    fname = "";
                    break;
            }

            player.SoundLocation = fname;
            player.Load();
            player.PlayLooping();

            return 1;
        }

        public int stopTone()
        {
            player.Stop();
            player.Dispose();
            return 1;
        }

        #endregion
    }

}
