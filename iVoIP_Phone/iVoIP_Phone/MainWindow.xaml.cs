using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using MahApps.Metro.Controls;
using System.Globalization;
using Sipek.Common.CallControl;
using Sipek.Common;
using Sipek.Sip;
using WaveLib.AudioMixer;
using AsterNET.Manager;
using AsterNET.Manager.Action;

namespace iVoIP_Phone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        Mixers mMixer;
        //Flags
        bool superClickFlag = false;
        //
        System.Timers.Timer timer1;
        //
        private SipekResources  _resources;
        private SipekResources  SipekResources { get { return _resources; } }
        public bool             IsProxyInitialized { get { return SipekResources.StackProxy.IsInitialized; } }
        int                     _sessionId = 0;

        IStateMachine           callObject;
        IStateMachine           inCallObject;
        
        //Call Flags
        bool onCall             = false;
        bool isWorkCodeNeeded   = false;
        bool isCallAnswered     = false;
        bool muteFlag           = false;
        bool isIncomingCall     = false;
        bool isOutgoingCall     = false;
        bool holdFlag           = false;
        bool isKeypadInVisible  = false;
        bool callWasOutgoing    = false;
        bool callWasIncoming    = false;
        bool cdrLogger          = false;
        bool workcodeDone       = false;
        //

        UITimer             timer;
        UKeypad             keypad;
        UCallButtons        btns;
        UIncomingCall       inCall;
        UTable              table;
        UWorkCodeWindow     wcodes;
        USupervisorPanel    supervisePanel;
        UTransfer           xferPanel;
        UCallHistory        callHistory;

        StateMachine        sm;

        //Data Classes
        CDatabaseClass      dataClass;

        //CDR Class
        CCallDetailRecord   record;

        //Timers
        Stopwatch totloginTime;
        Stopwatch totnreadyTime;
        Stopwatch totreadyTime;
        Stopwatch totacwTime;
        Stopwatch totidleTime;
        Stopwatch tottalktime;
        Stopwatch totholdtime;
        Stopwatch totANSdelay;

        //GUI Timers
        Stopwatch talktimer;
        Stopwatch holdtimer;
        Stopwatch ansDelay;

        //Table Parameters
        TimeSpan avgAnsSpeed;
        TimeSpan avgtalkTime;
        TimeSpan avghandlingTime;
        //
        int PSL             = 0;
        int GSL             = 0;
        int totDialCalls    = 0;
        int totAnsCalls     = 0;
        int totRcvCalls     = 0;
        int totMissedCalls  = 0;

        int totCallsAnsAfterThreshold   = 0;
        int totCallsAbndnAfterThreshold = 0;
        //

        int doneFlag = 0;

        public string   NReadyReason { get; set; }
        public BigState BigCurrentState { get; set; }
        public State    CurrentState { get; set; }
        public Controls PreviousState { get; set; }

        private ManagerConnection manager = null;

        #region Close Button Removal

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;

            if (hwndSource != null)
            {
                hwndSource.AddHook(HwndSourceHook);
            }
        }

        private bool allowClosing = false;

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint MF_BYCOMMAND = 0x00000000;
        private const uint MF_GRAYED = 0x00000001;

        private const uint SC_CLOSE = 0xF060;

        private const int WM_SHOWWINDOW = 0x00000018;
        private const int WM_CLOSE = 0x10;

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_SHOWWINDOW:
                    {
                        IntPtr hMenu = GetSystemMenu(hwnd, false);
                        if (hMenu != IntPtr.Zero)
                        {
                            EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED);
                        }
                    }
                    break;
                case WM_CLOSE:
                    if (!allowClosing)
                    {
                        handled = true;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        #endregion

        public enum BigState
        {
            Ready,
            NotReady
        }

        public enum Controls
        {
            Table,
            Incall,
            Keypad,
            buttons,
            WorkCode,
            callHistory
        }

        public MainWindow(CDatabaseClass dataClassInstance)
        {
            InitializeComponent();
            dataClass   = dataClassInstance;
            _resources  = new SipekResources(dataClass.PhoneConfig);
        }

        void sm_StateChanged(State newState)
        {
            updateGUIDisplay(newState);
            CurrentState = newState;
            timerRestart();
        }

        private void updateGUIDisplay(State state)
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    textBlock2.Text = state.ToString();
                    textBlock4.Text = state.ToString();
                    RcvCalls.Text   = totRcvCalls.ToString();
                    DialCalls.Text  = totDialCalls.ToString();
                }));
        }

        private void stackPanel1_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private void stackPanel1_MouseEnter(object sender, MouseEventArgs e)
        {
            this.toolBar1.Visibility = System.Windows.Visibility.Visible;
        }

        private void stackPanel1_GotMouseCapture(object sender, MouseEventArgs e)
        {
            this.toolBar1.Visibility = System.Windows.Visibility.Visible;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //
            int status = SipekResources.CallManager.Initialize();

            if (status != 0)
            {
                MessageBox.Show("Init SIP stack problem! \r\nPlease, check configuration and start again! \r",
                    "Initialize Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SipekResources.Registrar.registerAccounts();
            SipekResources.CallManager.CallStateRefresh += new DCallStateRefresh(CallManager_CallStateRefresh);
            SipekResources.Registrar.AccountStateChanged += new DAccountStateChanged(Registrar_AccountStateChanged);
            SipekResources.CallManager.IncomingCallNotification += new DIncomingCallNotification(CallManager_IncomingCallNotification);

            #region AsterNet Manager Login

            string server = dataClass.AccountConfig.AsteriskIP;
            int port = 5038;
            string username = "ivoip-admin";
            string password = "Root12";
            string queue = "100";
            string iface = dataClass.Extension;

            manager = new ManagerConnection(server, port, username, password);

            try
            {
                manager.Login();
                manager.SendAction(new QueueAddAction(queue, "SIP/" + iface, dataClass.AccountConfig.DisplayName));
                manager.SendAction(new QueuePauseAction("SIP/" + iface, true));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to server. Contact VoIP administrator. " + ex.Message, "Server Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            #endregion

            //
            this.Date.Text = String.Format("{0} {1}, {2}", DateTime.Now.ToString("MMM"), DateTime.Now.Day, DateTime.Now.Year);
            this.textBlock1.Text = dataClass.LoginName+" - "+dataClass.Extension;
            this.BigCurrentState = BigState.NotReady;
            this.textBlock2.Text = BigCurrentState.ToString();
            this.textBlock3.Text = "Connected";
            //Timer
            this.timer = new UITimer(this);
            this.timer.Start();
            //
            keypad      = new UKeypad();
            btns        = new UCallButtons();
            inCall      = new UIncomingCall();
            table       = new UTable();
            wcodes      = new UWorkCodeWindow();
            xferPanel   = new UTransfer(dataClass);
            callHistory = new UCallHistory();

            CreateControls();

            //Testing UserControls
            //keypad = new UKeypad();
            //keypad.RenderSize = this.ChildGrid.RenderSize;
            //ChildGrid.Children.Add(keypad);
            //
            //btns = new UCallButtons();
            //btns.RenderSize = this.CallButt;onsGrid.RenderSize;
            //CallButtonsGrid.Children.Add(btns);
            //
            //inCall = new UIncomingCall();
            //inCall.RenderSize = this.ChildGrid.RenderSize;
            //ChildGrid.Children.Add(inCall);
            //
            wcodes.NodeClicked              += new EventHandler(wcodes_NodeClicked);
            keypad.CallButtonClicked        += new EventHandler(keypad_CallButtonClicked);
            keypad.DisconnectCall           += new EventHandler(keypad_DisconnectCall);
            btns.CallButtonClicked          += new EventHandler(keypad_CallButtonClicked);
            btns.DisconnectCall             += new EventHandler(keypad_DisconnectCall);
            btns.HoldButtonClicked          += new EventHandler(btns_HoldButtonClicked);
            btns.toIVRinHangupbtnClicked    += new EventHandler(btns_toIVRinHangupbtnClicked);
            btns.Transfer                   += new EventHandler(btns_Transfer);
            btns.MuteBtnClicked             += new EventHandler(btns_MuteBtnClicked);
            xferPanel.XferNow               += new EventHandler(xferPanel_XferNow);
            //
            btns.WorkcodeBtnClicked         += new EventHandler(btns_WorkcodeBtnClicked);
            callHistory.DialCall += new EventHandler(callHistory_DialCall);
            //
            //Timer Handlers
            totloginTime   = new Stopwatch();
            totloginTime.Start();
            //N/Ready Timer
            totnreadyTime  = new Stopwatch();
            totreadyTime   = new Stopwatch();
            totacwTime     = new Stopwatch();
            totidleTime    = new Stopwatch();
            tottalktime    = new Stopwatch();
            totholdtime    = new Stopwatch();
            totANSdelay    = new Stopwatch();
            //GUI Timers
            holdtimer      = new Stopwatch();
            talktimer      = new Stopwatch();
            ansDelay       = new Stopwatch();
            //
            sm = new StateMachine();
            sm.StateChanged += new StateMachine.StateChangeEvent(sm_StateChanged);
            //CDR
            record = new CCallDetailRecord();
            //
            e.Handled = true;
            //InCall Timers
            timer1 = new System.Timers.Timer();
            timer1.Start();
            timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed);
            //NReady time start
            totnreadyTime.Start();
            //
            //DTMF Event handlers
            keypad.send0 += new EventHandler(keypad_send0);
            keypad.send1 += new EventHandler(keypad_send1);
            keypad.send2 += new EventHandler(keypad_send2);
            keypad.send3 += new EventHandler(keypad_send3);
            keypad.send4 += new EventHandler(keypad_send4);
            keypad.send5 += new EventHandler(keypad_send5);
            keypad.send6 += new EventHandler(keypad_send6);
            keypad.send7 += new EventHandler(keypad_send7);
            keypad.send8 += new EventHandler(keypad_send8);
            keypad.send9 += new EventHandler(keypad_send9);
            keypad.sendstar += new EventHandler(keypad_sendstar);
            keypad.sendhash += new EventHandler(keypad_sendhash);
        }

        void callHistory_DialCall(object sender, EventArgs e)
        {
            disableAllControls();
            enableKeypadControl();
            enableButtonControls();
            keypad.DialedNumber = callHistory.numToDial;
            keypad.YoDialIt(callHistory.numToDial);
        }

        void keypad_send0(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "0", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "0", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send1(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "1", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "1", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send2(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "2", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "2", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send3(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "3", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "3", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send4(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "4", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "4", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send5(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "5", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "5", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send6(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "6", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "6", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send7(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "7", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "7", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send8(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "8", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "8", EDtmfMode.DM_Inband);
            }
        }

        void keypad_send9(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "9", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "9", EDtmfMode.DM_Inband);
            }
        }

        void keypad_sendstar(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "*", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "*", EDtmfMode.DM_Inband);
            }
        }

        void keypad_sendhash(object sender, EventArgs e)
        {
            if (onCall)
            {
                if (isIncomingCall) SipekResources.CallManager.onUserDialDigit(inCallObject.Session,
                    "#", EDtmfMode.DM_Inband);
                if (isOutgoingCall) SipekResources.CallManager.onUserDialDigit(callObject.Session,
                    "#", EDtmfMode.DM_Inband);
            }
        }

        void btns_MuteBtnClicked(object sender, EventArgs e)
        {
            mMixer = new Mixers();
            MixerLine micLine;
            micLine = mMixer.Recording.UserLines.
                GetMixerFirstLineByComponentType(MIXERLINE_COMPONENTTYPE.SRC_MICROPHONE);

            if (!muteFlag)
            {
                micLine.Mute = true;
                muteFlag = true;
            }
            else if (muteFlag)
            {
                micLine.Mute = false;
                muteFlag = false;
            }
        }

        void btns_Transfer(object sender, EventArgs e)
        {
            xferPanel.Load();
            enableTransferControl();
        }

        void xferPanel_XferNow(object sender, EventArgs e)
        {
            TransferCall(_sessionId, xferPanel.exten);
            disableTransferControl();
        }

        void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    if (isOutgoingCall)
                    {
                        if (talktimer.IsRunning && !isIncomingCall && keypad.IsVisible)
                            keypad.TalkTimer.Text = String.Format("{0:00}:{1:00}:{2:00}",
                                talktimer.Elapsed.Hours, talktimer.Elapsed.Minutes, talktimer.Elapsed.Seconds);
                        if (holdtimer.IsRunning && !isIncomingCall && keypad.IsVisible)
                            keypad.HoldTimer.Text = String.Format("{0:00}:{1:00}:{2:00}",
                                holdtimer.Elapsed.Hours, holdtimer.Elapsed.Minutes, holdtimer.Elapsed.Seconds);
                        if ((talktimer.IsRunning || holdtimer.IsRunning) && !isIncomingCall && keypad.IsVisible)
                            keypad.HandleTime.Text = String.Format("{0:00}:{1:00}:{2:00}",
                                talktimer.Elapsed.Hours + holdtimer.Elapsed.Hours,
                                talktimer.Elapsed.Minutes + holdtimer.Elapsed.Minutes,
                                talktimer.Elapsed.Seconds + holdtimer.Elapsed.Seconds);
                    }
                    else if (isIncomingCall)
                    {
                        if (talktimer.IsRunning && isIncomingCall && inCall.IsVisible)
                            inCall.TalkTime.Text = String.Format("{0:00}:{1:00}:{2:00}",
                                talktimer.Elapsed.Hours, talktimer.Elapsed.Minutes, talktimer.Elapsed.Seconds);
                        if (holdtimer.IsRunning && isIncomingCall && inCall.IsVisible)
                            inCall.HoldTime.Text = String.Format("{0:00}:{1:00}:{2:00}",
                                holdtimer.Elapsed.Hours, holdtimer.Elapsed.Minutes, holdtimer.Elapsed.Seconds);
                        if ((talktimer.IsRunning || holdtimer.IsRunning) && isIncomingCall && inCall.IsVisible)
                            inCall.HandleTime.Text = String.Format("{0:00}:{1:00}:{2:00}",
                                talktimer.Elapsed.Hours + holdtimer.Elapsed.Hours,
                                talktimer.Elapsed.Minutes + holdtimer.Elapsed.Minutes,
                                talktimer.Elapsed.Seconds + holdtimer.Elapsed.Seconds);
                        if (ansDelay.IsRunning && isIncomingCall && inCall.IsVisible)
                            inCall.AnsDelay.Text = String.Format("{0:00}:{1:00}:{2:00}",
                                ansDelay.Elapsed.Hours, ansDelay.Elapsed.Minutes, ansDelay.Elapsed.Seconds);
                    }
                }));
        }

        void btns_toIVRinHangupbtnClicked(object sender, EventArgs e)
        {
            btns.ReChangeImage();
            incomingDisconnect();
        }

        void btns_HoldButtonClicked(object sender, EventArgs e)
        {
            if (!holdFlag)
            {
                HoldCall(_sessionId);
                holdFlag = true;
                tottalktime.Stop();
                talktimer.Stop();
                holdtimer.Start();
                totholdtime.Start();
            }
            else if (holdFlag)
            {
                HoldCall(_sessionId);
                holdFlag = false;
                totholdtime.Stop();
                tottalktime.Start();
                holdtimer.Stop();
                talktimer.Start();
            }
        }

        void btns_WorkcodeBtnClicked(object sender, EventArgs e)
        {
            enableWorkcodeControl();
        }

        void keypad_DisconnectCall(object sender, EventArgs e)
        {
            if (isOutgoingCall) disconnectCall();
            if (isIncomingCall) incomingDisconnect();
        }

        void incomingDisconnect()
        {
            if (!isCallAnswered)
            {
                record.answerTime = record.startTime;
                callHistory.AddCall("Missed", inCallObject.CallingNumber, DateTime.Now);
                totMissedCalls++;
            }
            SyncImages("DisconnectCall");
            keypad.disconnectCall();
            if (isIncomingCall) CallHangup(_sessionId);
            record.Duration = (int)inCallObject.Duration.TotalSeconds;
            record.endTime = DateTime.Now;
            if (inCallObject.Duration.TotalSeconds == 0 && ansDelay.Elapsed.TotalSeconds >= 20)
                totCallsAbndnAfterThreshold++;
            toACW();
            if (isWorkCodeNeeded)
            {
                if (Dispatcher.CheckAccess())
                    enableButtonControls();
                else Dispatcher.Invoke(new Action(() =>
                    {
                        enableButtonControls();
                    }));
                btns.WorkcodeView();
            }
            else if (!isWorkCodeNeeded)
            {
                disableAllControls();
            }
            onCall = false;
            isCallAnswered = false;
            btns.call = false;
            //if (isWorkCodeNeeded) getTheWorkcode();
            //else
            //{
            //    disableAllControls();
            //    toIdle();
            //}
            //updateGUIDisplay(CurrentState);
            //Cdr Handlers
            //
            //if (workcodeDone)
            //{
            //    dumpTheCDR();
            //}
        }

        private void disconnectCall()
        {
            SyncImages("DisconnectCall");
            keypad.disconnectCall();
            record.destination = callObject.CallingNumber;
            if (!callObject.Incoming) CallHangup(_sessionId);
            if (isWorkCodeNeeded) getTheWorkcode();
            toACW();
            onCall = false;
            isCallAnswered = false;
            record.source = dataClass.Extension;
            record.endTime = DateTime.Now;
            record.type = CCallDetailRecord.EType.OUTGOING;
            record.Duration = (int)callObject.Duration.TotalSeconds;
            if (!callObject.IsNull)
            {
                if (callObject.StateId == EStateId.RELEASED &&
                    callObject.Duration == TimeSpan.Zero)
                    record.Disposition = CCallDetailRecord.EDisposition.REJECTED;
                else if (callObject.Duration == TimeSpan.Zero)
                {
                    record.Disposition = CCallDetailRecord.EDisposition.NOANSWER;
                    record.answerTime = record.startTime;
                }
                else if (callObject.Duration != TimeSpan.Zero)
                {
                    record.Disposition = CCallDetailRecord.EDisposition.ANSWERED;
                    //record.answerTime = record.startTime;
                }
            }
        }

        void getTheWorkcode()
        {
            btns.WorkcodeView();
        }

        void keypad_CallButtonClicked(object sender, EventArgs e)
        {
            if (!onCall && !String.IsNullOrEmpty(keypad.DialedNumber))
            {
                if (CurrentState == State.ACW) return;
                SyncImages("Call");
                keypad.Call();
                MakeCall(keypad.DialedNumber);
                totDialCalls++;
                onCall = true;
                isOutgoingCall = true;
                toOutgoingCall();
                //Timer Reset
                talktimer.Reset();
                holdtimer.Reset();
                record.startTime = DateTime.Now;
                Addcall("Outgoing", keypad.DialedNumber, DateTime.Now);
                //
            }
            else if (isIncomingCall)
            {
                AcceptCall(_sessionId);
                SyncImages("Call");
                btns.ReChangeImage();
                isIncomingCall = true;
                onCall = true;
                totANSdelay.Stop();
                ansDelay.Stop();
                inCall.TimerStop();
                holdtimer.Reset();
                if (ansDelay.Elapsed.TotalSeconds > 20) totCallsAnsAfterThreshold++;
            }
        }

        void SyncImages(string str)
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    if (str == "DisconnectCall")
                    {
                        var uri = new Uri(@"dialer.png", UriKind.RelativeOrAbsolute);
                        var brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(uri);
                        brush.Stretch = Stretch.None;
                        btns.Btn.Background = brush;
                        btns.Btn.ToolTip = "Dial";

                        var urisource = new Uri(@"/iVoIP_Phone;component/Images/phone-18-32.png", UriKind.Relative);
                        keypad.DialButton.NormalImage = new BitmapImage(urisource);
                    }
                    else if (str == "Call")
                    {
                        var uri = new Uri(@"hangup.png", UriKind.RelativeOrAbsolute);
                        var brush = new ImageBrush();
                        brush.ImageSource = new BitmapImage(uri);
                        brush.Stretch = Stretch.None;
                        btns.Btn.Background = brush;
                        btns.Btn.ToolTip = "Hangup";

                        var urisource = new Uri(@"/iVoIP_Phone;component/Images/phone-red.png", UriKind.Relative);
                        keypad.DialButton.NormalImage = new BitmapImage(urisource);
                    }
                }));
        }

        void CallManager_IncomingCallNotification(int sessionId, string number, string info)
        {
            if (!onCall && BigCurrentState == BigState.Ready && CurrentState == State.Idle)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    disableAllControls();
                    enableInCallControl();
                    inCall.InCallShow(number);
                    enableButtonControls();
                    btns.OnShow();
                    btns.btnsIncomingCallView();
                    toIncomingCall();
                    ansDelay.Reset();
                    ansDelay.Start();
                    totANSdelay.Start();
                    //System.Windows.Forms.MessageBox.Show("Test - Notify");
                    Addcall("Incoming", number, DateTime.Now);
                    this.Activate();
                    this.WindowState = System.Windows.WindowState.Normal;
                }));
            }
            else CallHangup(sessionId);
        }

        private void Addcall(string type, string number, DateTime dt)
        {
            callHistory.AddCall(type, number, dt);
        }

        void Registrar_AccountStateChanged(int accountId, int accState)
        {
            if (Dispatcher.CheckAccess())
                Dispatcher.Invoke(new DAccountStateChanged(OnRegistrationUpdate), new object[] { accountId, accState });
            else
                OnRegistrationUpdate(accountId, accState);
        }

        private void OnRegistrationUpdate(int accountId, int accState)
        {
            if (!Dispatcher.CheckAccess())
                Dispatcher.Invoke(new Action(() =>
                    {
                        if (accState != 200) textBlock3.Text = "Not Registered!";
                    }));
            else if (accState != 200) textBlock3.Text = "Not Registered!";
        }

        void CallManager_CallStateRefresh(int sessionId)
        {
            if (Dispatcher.CheckAccess())
                Dispatcher.Invoke(new DCallStateRefresh(OnStateUpdate), new object[] { sessionId });
            else
                OnStateUpdate(sessionId);
        }

        private void OnStateUpdate(int sessionId)
        {
            _sessionId = sessionId;
            switch (SipekResources.CallManager.getCall(sessionId).StateId)
            {
                case EStateId.ACTIVE:
                    isWorkCodeNeeded    = true;
                    isCallAnswered      = true;
                    btns.InCallView();
                    //
                    tottalktime.Start();
                    talktimer.Start();
                    //
                    totAnsCalls++;
                    if (isIncomingCall) totRcvCalls++;
                    //Cdr handlers
                    if (doneFlag != 1)
                    {
                        record.answerTime = DateTime.Now;
                    }
                    //
                    break;
                case EStateId.ALERTING:
                    break;
                case EStateId.CONNECTING:
                    break;
                case EStateId.HOLDING:
                    totAnsCalls--;
                    if (isIncomingCall) totRcvCalls--;
                    doneFlag = 1;
                    break;
                case EStateId.IDLE:
                    break;
                case EStateId.INCOMING:
                    isIncomingCall = true;
                    //totRcvCalls++;
                    inCallObject = SipekResources.CallManager.getCall(sessionId);
                    //Cdr Handlers
                    record.source = inCallObject.CallingNumber;
                    record.destination = dataClass.Extension;
                    record.type = CCallDetailRecord.EType.INCOMING;
                    record.startTime = DateTime.Now;
                    //
                    //System.Windows.Forms.MessageBox.Show("Test - State");
                    break;
                case EStateId.NULL:
                    doneFlag = 0;
                    break;
                case EStateId.RELEASED:
                    //
                    if (tottalktime.IsRunning) tottalktime.Stop();
                    if (talktimer.IsRunning) talktimer.Stop();
                    //
                    if (isOutgoingCall) callWasOutgoing = true;
                    if (isIncomingCall) callWasIncoming = true;
                    isOutgoingCall = false;
                    isIncomingCall = false;
                    Thread.Sleep(1000);
                    if (callWasOutgoing) disconnectCall();
                    if (callWasIncoming) incomingDisconnect();
                    SipekResources.MediaProxy.stopTone();
                    break;
                case EStateId.TERMINATED:
                    //
                    if (tottalktime.IsRunning) tottalktime.Stop();
                    if (talktimer.IsRunning) talktimer.Stop();
                    //
                    if (isOutgoingCall) callWasOutgoing = true;
                    if (isIncomingCall) callWasIncoming = true;
                    isOutgoingCall = false;
                    isIncomingCall = false;
                    break;
                default:
                    break;
            }
        }

        void CallHangup(int sessionId)
        {
            //SIpek Call Hangup
            SipekResources.CallManager.onUserRelease(sessionId);
        }

        void MakeCall(string number)
        {
            //Sipek Create Outbound Call
            callObject = SipekResources.CallManager.createOutboundCall(number);
        }

        void HoldCall(int sessionId)
        {
            //SIpek Hold
            SipekResources.CallManager.onUserHoldRetrieve(sessionId);
        }

        void TransferCall(int sessionId, string number)
        {
            //Sipek Transfer
            SipekResources.CallManager.onUserTransfer(sessionId, number);
        }

        void ConfCall(int sessionId)
        {
            //Sipek Conference
            SipekResources.CallManager.onUserConference(sessionId);
        }

        void AcceptCall(int sessionId)
        {
            //Sipek Answer Call
            SipekResources.CallManager.onUserAnswer(sessionId);
        }

        void wcodes_NodeClicked(object sender, EventArgs e)
        {
            disableOneControl(Controls.WorkCode);
            record.Workcode = wcodes.SelectedCode;
            isWorkCodeNeeded = false;
            workcodeDone = true;
            if (onCall) btns.InCallWorkCodeView();
            else if (callWasIncoming && !onCall)
            {
                disableAllControls();
                callWasIncoming = false;
            }
            else if (callWasOutgoing && !onCall)
            {
                enableButtonControls();
                enableKeypadControl();
                btns.OnShow();
                callWasOutgoing = false;
            }
            //if (callWasOutgoing && onCall)
            //{
            //    enableKeypadControl();
            //    enableButtonControls();
            //    btns.InCallWorkCodeView();
            //    callWasOutgoing = false;
            //}
            //else if (isOutgoingCall && onCall)
            //{
            //    enableKeypadControl();
            //    enableButtonControls();
            //    btns.InCallWorkCodeView();
            //}
            //if (callWasIncoming)
            //{
            //    disableAllControls();
            //    callWasIncoming = false;
            //}
            //else if (isIncomingCall)
            //{
            //    enableInCallControl();
            //    enableButtonControls();
            //    btns.InCallWorkCodeView();
            //}
            //if (!onCall)
            //{
            //    toIdle();
            //}
        }

        private void dumpTheCDR()
        {
            //PENDING
            if (record.Workcode == null)
            {
                record.Workcode = "NULL";
            }
            dataClass.LogCall(record);
            initializeRecord();
        }

        void initializeRecord()
        {
            record = new CCallDetailRecord();
        }

        void CreateControls()
        {
            btns.RenderSize = this.CallButtonsGrid.RenderSize;
            CallButtonsGrid.Children.Add(btns);
            keypad.RenderSize = this.ChildGrid.RenderSize;
            ChildGrid.Children.Add(keypad);
            inCall.RenderSize = this.ChildGrid.RenderSize;
            ChildGrid.Children.Add(inCall);
            table.RenderSize = this.ChildGrid.RenderSize;
            ChildGrid.Children.Add(table);
            wcodes.RenderSize = this.ChildGrid.RenderSize;
            ChildGrid.Children.Add(wcodes);
            xferPanel.RenderSize = this.ChildGrid.RenderSize;
            ChildGrid.Children.Add(xferPanel);
            callHistory.RenderSize = this.ChildGrid.RenderSize;
            ChildGrid.Children.Add(callHistory);
            disableAllControls();
        }

        private void disableAllControls()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new Action(() =>
                    {
                        if (btns.IsVisible)
                        {
                            btns.Visibility     = System.Windows.Visibility.Hidden;
                            PreviousState       = Controls.buttons;
                        }
                        if (inCall.IsVisible)
                        {
                            inCall.Visibility   = System.Windows.Visibility.Hidden;
                            PreviousState       = Controls.Incall;
                        }
                        if (keypad.IsVisible)
                        {
                            keypad.Visibility   = System.Windows.Visibility.Hidden;
                            PreviousState       = Controls.Keypad;
                        }
                        if (table.IsVisible)
                        {
                            table.Visibility    = System.Windows.Visibility.Hidden;
                            PreviousState       = Controls.Table;
                        }
                        if (wcodes.IsVisible)
                        {
                            wcodes.Visibility   = System.Windows.Visibility.Hidden;
                            PreviousState       = Controls.WorkCode;
                        }
                        if (xferPanel.IsVisible)
                        {
                            xferPanel.Visibility = System.Windows.Visibility.Hidden;
                        }
                        if (callHistory.IsVisible)
                        {
                            callHistory.Visibility = System.Windows.Visibility.Hidden;
                        }
                    }));
            }
            else
            {
                if (btns.IsVisible)
                {
                    btns.Visibility     = System.Windows.Visibility.Hidden;
                    PreviousState       = Controls.buttons;
                }
                if (inCall.IsVisible)
                {
                    inCall.Visibility   = System.Windows.Visibility.Hidden;
                    PreviousState       = Controls.Incall;
                }
                if (keypad.IsVisible)
                {
                    keypad.Visibility   = System.Windows.Visibility.Hidden;
                    PreviousState       = Controls.Keypad;
                }
                if (table.IsVisible)
                {
                    table.Visibility    = System.Windows.Visibility.Hidden;
                    PreviousState       = Controls.Table;
                }
                if (wcodes.IsVisible)
                {
                    wcodes.Visibility   = System.Windows.Visibility.Hidden;
                    PreviousState       = Controls.WorkCode;
                }
                if (xferPanel.IsVisible)
                {
                    xferPanel.Visibility = System.Windows.Visibility.Hidden;
                }
                if (callHistory.IsVisible)
                {
                    callHistory.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        private void disableOneControl(Controls control)
        {
            switch (control)
            {
                case Controls.Table:
                    if (table.IsVisible) table.Visibility   = System.Windows.Visibility.Hidden;
                    break;
                case Controls.Incall:
                    if (inCall.IsVisible) inCall.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case Controls.Keypad:
                    if (keypad.IsVisible) keypad.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case Controls.buttons:
                    if (btns.IsVisible) btns.Visibility     = System.Windows.Visibility.Hidden;
                    break;
                case Controls.WorkCode:
                    if (wcodes.IsVisible) wcodes.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case Controls.callHistory:
                    if (callHistory.IsVisible) callHistory.Visibility = System.Windows.Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }

        private void enableKeypadControl()
        {
            keypad.Visibility   = System.Windows.Visibility.Visible;
        }

        private void enableInCallControl()
        {
            inCall.Visibility   = System.Windows.Visibility.Visible;
        }

        void enableButtonControls()
        {
            btns.Visibility = System.Windows.Visibility.Visible;
        }

        void enableTableControl()
        {
            table.Visibility    = System.Windows.Visibility.Visible;
        }

        void enableWorkcodeControl()
        {
            wcodes.Visibility   = System.Windows.Visibility.Visible;
        }

        void enableTransferControl()
        {
            xferPanel.Visibility = System.Windows.Visibility.Visible;
        }

        void enableCallHistory()
        {
            callHistory.Visibility = System.Windows.Visibility.Visible;
        }

        void disableTransferControl()
        {
            xferPanel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void UpdateTableData()
        {
            table.TLD   = String.Format("{0:00}:{1:00}", totloginTime.Elapsed.Minutes, totloginTime.Elapsed.Seconds);
            table.Nrdy  = String.Format("{0:00}:{1:00}", totnreadyTime.Elapsed.Minutes, totnreadyTime.Elapsed.Seconds);
            table.Rdy   = String.Format("{0:00}:{1:00}", totreadyTime.Elapsed.Minutes, totreadyTime.Elapsed.Seconds);
            table.Talk  = String.Format("{0:00}:{1:00}", tottalktime.Elapsed.Minutes, tottalktime.Elapsed.Seconds);
            table.Hold  = String.Format("{0:00}:{1:00}", totholdtime.Elapsed.Minutes, totholdtime.Elapsed.Seconds);
            table.GSL   = GSL.ToString();

            if (totRcvCalls == 0) table.PSL = PSL.ToString();
            else table.PSL = ((((totRcvCalls + totMissedCalls) - (totCallsAnsAfterThreshold + totCallsAbndnAfterThreshold)) * 100) / (totRcvCalls+totMissedCalls)).ToString();
            
            table.DialCalls = totDialCalls.ToString();
            table.AnsCalls  = totAnsCalls.ToString();
            table.RcvCalls  = (totRcvCalls+totMissedCalls).ToString();
            
            if (totRcvCalls == 0) avgAnsSpeed       = new TimeSpan(0);
            else avgAnsSpeed        = new TimeSpan(totANSdelay.Elapsed.Ticks / (totRcvCalls+totMissedCalls));
            if (totRcvCalls == 0) avghandlingTime   = new TimeSpan(0);
            else avghandlingTime = new TimeSpan(totholdtime.Elapsed.Ticks + tottalktime.Elapsed.Ticks / (totRcvCalls + totMissedCalls));
            if (totRcvCalls == 0) avgtalkTime       = new TimeSpan(0);
            else avgtalkTime = new TimeSpan(tottalktime.Elapsed.Ticks / (totRcvCalls + totMissedCalls));
            
            table.ASD   = String.Format("{0:00}:{1:00}", avgAnsSpeed.Minutes, avgAnsSpeed.Seconds);
            table.AHT   = String.Format("{0:00}:{1:00}", avghandlingTime.Minutes, avghandlingTime.Seconds);
            table.ATT   = String.Format("{0:00}:{1:00}", avgtalkTime.Minutes, avgtalkTime.Seconds);
        }

        bool checkVisibiltyofKeypadBtns()
        {
            if (keypad.IsVisible && btns.IsVisible)
                return true;
            else return false;
        }

        private void NotReady(object sender, RoutedEventArgs e)
        {
            if (BigCurrentState == BigState.NotReady) { statusLabel.Content = String.Format("Already in {0} state", BigCurrentState.ToString()); return; }
            var btn         = sender as Button;
            NReadyReason    = btn.ToolTip.ToString().Split('-')[1].Trim();
            BigCurrentState = BigState.NotReady;
            if (totreadyTime.IsRunning) totreadyTime.Stop();
            totnreadyTime.Start();
            ControlsNReady();

            manager.SendAction(new QueuePauseAction("SIP/" + dataClass.Extension, true));

            dataClass.LogBigStates(iVoIP_Phone.BigState.NotReady, NReadyReason);
            disableAllControls();
            e.Handled = true;
        }

        private void ControlsReady()
        {
            BigCurrentState                         = BigState.Ready;
            this.textBlock4.Text                    = BigCurrentState.ToString();
            this.textBlock2.Text                    = BigCurrentState.ToString();
            this.statusLabel.Content                = BigCurrentState.ToString();
            var urisource                           = new Uri(@"/iVoIP_Phone;component/Images/Check.png", UriKind.Relative);
            this.ribbonDisplayImage1.NormalImage    = new BitmapImage(urisource);
            // Initial State i.e. Idle!!
            // Jump into "IDLE state"
            //changeInState(State.Idle, State.Idle);
            //Timer
            //timerRestart();
            //
            CurrentState = State.Idle;
        }

        void timerRestart()
        {
            this.timer.Stop();
            this.timer.Reset();
            this.timer.Start();
        }

        private void ControlsNReady()
        {
            BigCurrentState                         = BigState.NotReady;
            this.textBlock4.Text                    = BigCurrentState.ToString();
            this.textBlock2.Text                    = BigCurrentState.ToString();
            this.statusLabel.Content                = "Waiting to Get Ready";
            var urisource                           = new Uri(@"/iVoIP_Phone;component/Images/cross.png", UriKind.Relative);
            this.ribbonDisplayImage1.NormalImage    = new BitmapImage(urisource);
            //Timer
            //timerRestart();
            //
        }

        private void ribbonDisplayImage1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != ribbonDisplayImage1) return;
            if (!isWorkCodeNeeded && CurrentState == State.ACW)
            {
                toIdle();
                ControlsReady();
                timerRestart();
                if (checkVisibiltyofKeypadBtns()) btns.PreCallView();
                dumpTheCDR();
                callWasOutgoing     = false;
                callWasIncoming     = false;
                isIncomingCall      = false;
                isOutgoingCall      = false;
                return;
            }
            if (isWorkCodeNeeded || onCall) { statusLabel.Content = String.Format("Please disconnect the Call or select a WORKCODE first!"); return; }
            if (BigCurrentState == BigState.Ready) { statusLabel.Content = String.Format("Already in {0} state", BigCurrentState.ToString()); return; }
            if (totnreadyTime.IsRunning) totnreadyTime.Stop();
            if (!totreadyTime.IsRunning) totreadyTime.Start();

            try
            {
                manager.SendAction(new QueuePauseAction("SIP/" + dataClass.Extension, false));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            ControlsReady();
            dataClass.LogBigStates(iVoIP_Phone.BigState.Ready, null);
            dataClass.ChangeState(State.Idle);
            timerRestart();
            e.Handled = true;
        }

        private void changeInState(State _state, State pState)
        {
            if (sm == null) sm = new StateMachine();
            else sm.ChangeState(_state, pState);
        }

        private void btnDial_Click(object sender, RoutedEventArgs e)
        {
            if (isWorkCodeNeeded) { statusLabel.Content = "Please select a WORKCODE first!"; return; }
            if (onCall && isOutgoingCall || isIncomingCall) return;
            if (!checkVisibiltyofKeypadBtns())
            {
                if (BigCurrentState == BigState.Ready)
                {
                    isKeypadInVisible = false;
                    disableAllControls();
                    enableButtonControls();
                    enableKeypadControl();
                    btns.PreCallView();
                }
            }
            else 
            {
                disableAllControls(); 
                isKeypadInVisible = true; 
            }
        }

        private void statusLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            //Upate data
            //if (onCall) return;
            //disableAllControls();
            //UpdateTableData();
            //enableTableControl();
        }

        private void statusLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            disableOneControl(Controls.Table);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                manager.SendAction(new QueueRemoveAction(dataClass.AccountConfig.AsteriskIP, "SIP/" +
                dataClass.Extension));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            totloginTime.Stop();
            dataClass.LogoutTime = DateTime.Now;
            dataClass.LogLogoutCurrentState();
            dataClass.LogConsolidated(totacwTime.Elapsed, totidleTime.Elapsed, totnreadyTime.Elapsed,
                totreadyTime.Elapsed, totholdtime.Elapsed, tottalktime.Elapsed, totloginTime.Elapsed);
            dataClass.LogTable(totacwTime.Elapsed, totidleTime.Elapsed, totnreadyTime.Elapsed,
                totreadyTime.Elapsed, totholdtime.Elapsed, tottalktime.Elapsed, totloginTime.Elapsed,
                totholdtime.Elapsed + tottalktime.Elapsed, avgAnsSpeed, avgtalkTime, totRcvCalls, totDialCalls,
                totAnsCalls, PSL, GSL);
            SipekResources.CallManager.Shutdown();
            allowClosing = true;
            this.Close();
        }

        private void toACW()
        {
            updateGUIDisplay(State.ACW);
            SyncImageACW();
            timerRestart();
            totacwTime.Start();
            CurrentState = State.ACW;
            dataClass.ChangeState(State.ACW);
        }

        private void SyncImageACW()
        {
            if (Dispatcher.CheckAccess())
            {
                var uri = new Uri(@"/iVoIP_Phone;component/Images/work.png", UriKind.Relative);
                ribbonDisplayImage1.NormalImage = new BitmapImage(uri);
            }
            else
                Dispatcher.Invoke(new Action(() =>
                    {
                        var uri = new Uri(@"/iVoIP_Phone;component/Images/work.png", UriKind.Relative);
                        ribbonDisplayImage1.NormalImage = new BitmapImage(uri);
                    }));
        }

        private void toOutgoingCall()
        {
            updateGUIDisplay(State.OnCall);
            timerRestart();
            if (totacwTime.IsRunning) totacwTime.Stop();
            if (totidleTime.IsRunning) totidleTime.Stop();
            CurrentState = State.OnCall;
            dataClass.ChangeState(State.OnCall);
        }

        private void toIncomingCall()
        {
            updateGUIDisplay(State.OnCall);
            timerRestart();
            if (totacwTime.IsRunning) totacwTime.Stop();
            if (totidleTime.IsRunning) totidleTime.Stop();
            CurrentState = State.OnCall;
            dataClass.ChangeState(State.OnCall);
        }

        private void toIdle()
        {
            updateGUIDisplay(State.Idle);
            timerRestart();
            totidleTime.Start();
            if (totacwTime.IsRunning) totacwTime.Stop();
            CurrentState = State.Idle;
            dataClass.ChangeState(State.Idle);
        }

        private void btnSUpervise_Click(object sender, RoutedEventArgs e)
        {
            if (dataClass.GetSystemRights(dataClass.LoginName) == 'S' && !superClickFlag
                && CurrentState == State.Idle)
            {
                supervisePanel = new USupervisorPanel(SipekResources);
                supervisePanel.OnShow();
                disableAllControls();
                supervisePanel.RenderSize = ChildGrid.RenderSize;
                ChildGrid.Children.Add(supervisePanel);
                superClickFlag = true;
            }
            else if (dataClass.GetSystemRights(dataClass.LoginName) == 'S' && superClickFlag)
            {
                supervisePanel.Visibility   = System.Windows.Visibility.Hidden;
                superClickFlag              = false;
            }
        }

        private void statusLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Upate data
            if (onCall) return;
            UpdateTableData();
            enableTableControl();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            callHistory.Load();
            enableCallHistory();
        }
    }

    #region Clock

    [ValueConversion(typeof(DateTime), typeof(int))]
    public class SecondsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.Second * 6;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(DateTime), typeof(int))]
    public class MinutesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.Minute * 6;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(DateTime), typeof(int))]
    public class HoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return (date.Hour * 30) + (date.Minute / 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(DateTime), typeof(string))]
    public class WeekdayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.DayOfWeek.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

#endregion
}
