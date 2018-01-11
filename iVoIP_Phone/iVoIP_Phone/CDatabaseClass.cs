using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iVoIP_Phone
{
    public class CDatabaseClass
    {
        DataClassesiVoIPDataContext dataContext;

        //
        AccountConfig accountConfig;
        PhoneConfig phoneConfig;

        public AccountConfig AccountConfig { get { return this.accountConfig; } }
        public PhoneConfig PhoneConfig { get { return this.phoneConfig; } }
        //

        public string LoginName { get; set; }
        public string Extension { get; set; }

        public DateTime LoginTime { get; set; }
        public DateTime LogoutTime { get; set; }

        CurrStateInfo currState;

        public CDatabaseClass()
        {
            dataContext     = new DataClassesiVoIPDataContext();
            phoneConfig     = new PhoneConfig();
            accountConfig   = new AccountConfig();
        }

        public List<string> GetWorkCodes()
        {
            try
            {
                var query = from u in dataContext.Workcodes select u.WORKCODE1;
                return query.ToList();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4002: "+ex.Message);
                return null;
            }
        }

        public List<string> GetUsersToTransfer()
        {
            try
            {
                DataClassesiVoIPDataContext tempDataContext = new DataClassesiVoIPDataContext();
                var query = from u in tempDataContext.CurrStateInfos
                            where u.IsLogin == true && u.IsReady == true && u.OnCall == false && u.ACW == false
                            select u.Name;
                return query.ToList();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4005: " + ex.Message);
                return null;
            }
        }

        public List<string> GetUsersToMonitor()
        {
            try
            {
                DataClassesiVoIPDataContext tempDataContext = new DataClassesiVoIPDataContext();
                var query = from u in tempDataContext.CurrStateInfos
                            where u.IsLogin == true && u.IsReady == true
                            select u.Name;
                return query.ToList();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4013: " + ex.Message);
                return null;
            }
        }

        public string GetExtenFromName(string name)
        {
            try
            {
                var query = from u in dataContext.Logins where u.Name == name select u.Extension;
                return query.Single();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4006: " + ex.Message);
                return null;
            }
        }

        public List<CurrStateInfo> GetCurrentStateInfo()
        {
            try
            {
                DataClassesiVoIPDataContext tempContext = new DataClassesiVoIPDataContext();
                var query = from u in tempContext.CurrStateInfos select u;
                return query.ToList();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4003: "+ex.Message);
                return null;
            }
        }

        public char GetSystemRights(string username)
        {
            try
            {
                var query = from u in dataContext.Logins where u.Name == username.Trim() select u.SystemRights;
                return query.Single();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4004: "+ex.Message);
                return ' ';
            }
        }

        public Setting GetSettings(string extension)
        {
            try
            {
                var query = from u in dataContext.Settings
                            where u.extension == extension
                            select u;
                var result = query.Single();
                return result;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4007: "+ex.Message);
                return null;
            }
        }

        public bool AuthenticateUser(string username, string password)
        {
            try
            {
                var query = from u in dataContext.Logins 
                            where u.Extension == username && u.Secret == password 
                            select u;
               
                if (Enumerable.Count(query) > 0)
                {
                    LoginName           = query.Single().Name;
                    Extension           = query.Single().Extension;
                    LoginTime           = DateTime.Now;
                    currState           = dataContext.CurrStateInfos.Single(u => u.Extension == Extension.Trim());
                    currState.LoginTime = LoginTime.ToString("yyyyMMddHHmmss");
                    currState.IsLogin   = true;
                    

                    //Adding Account Info
                    this.accountConfig.AccountName  = LoginName;
                    this.accountConfig.DisplayName  = LoginName;
                    this.accountConfig.DomainName   = "*";
                    this.accountConfig.HostName     = GetProxyIP() + ":5060";
                    this.accountConfig.Id           = LoginName;
                    this.accountConfig.Password     = password.Trim();
                    this.accountConfig.ProxyAddress = "";
                    this.accountConfig.UserName     = Extension;
                    this.accountConfig.AsteriskIP   = GetProxyIP();
                    this.accountConfig.Queue        = "100";

                    //Adding Phone Config
                    this.phoneConfig.AAFlag         = GetSettings(Extension).AAFlag;
                    this.phoneConfig.CFBFlag        = GetSettings(Extension).CFBFlag;
                    this.phoneConfig.CFNRFlag       = GetSettings(Extension).CFNRFlag;
                    this.phoneConfig.CFUFlag        = GetSettings(Extension).CFUFlag;
                    this.phoneConfig.DNDFlag        = GetSettings(Extension).DNDFlag;
                    this.phoneConfig.SIPPort        = GetSettings(Extension).Port;
                    this.phoneConfig.CFBNumber      = GetSettings(Extension).CFBNumber;
                    this.phoneConfig.CFNRNumber     = GetSettings(Extension).CFNRNumber;
                    this.phoneConfig.CFUNumber      = GetSettings(Extension).CFUNumber;
                    this.phoneConfig.Accounts[0]    = accountConfig;
                }
                return Enumerable.Count(query) > 0;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4001: "+ex.Message);
                return false;
            }
        }

        private string GetProxyIP()
        {
            var q =  dataContext.AsteriskTables.Single();
            return q.ProxyIP.Trim();
        }

        public void LogCall(CCallDetailRecord record)
        {
            try
            {
                CallDetailRecord dbrecord   = new CallDetailRecord();
                dbrecord.answer             = record.answerTime.ToString("yyyyMMddHHmmss");
                dbrecord.destination        = record.destination;
                dbrecord.disposition        = record.Disposition.ToString();
                dbrecord.duration           = record.Duration;
                dbrecord.endtime            = record.endTime.ToString("yyyyMMddHHmmss");
                dbrecord.source             = record.source;
                dbrecord.start              = record.startTime.ToString("yyyyMMddHHmmss");
                dbrecord.type               = record.type.ToString();
                dbrecord.workcode           = record.Workcode;
                dataContext.CallDetailRecords.InsertOnSubmit(dbrecord);
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4008: " + ex.Message);
            }
        }

        public void LogConsolidated(TimeSpan acw, TimeSpan idle, TimeSpan nready, TimeSpan ready,
            TimeSpan hold, TimeSpan talk, TimeSpan login)
        {
            try
            {
                Consolidated report     = new Consolidated();
                report.Name             = LoginName;
                report.Extension        = Extension;
                report.LoginTime = LoginTime.ToString("yyyyMMddHHmmss");
                report.LogoutTime = LogoutTime.ToString("yyyyMMddHHmmss");
                //
                report.TotalACWTime     = String.Format("{0:00}:{1:00}:{2:00}", acw.Hours, acw.Minutes, acw.Seconds);
                report.TotalHoldTime    = String.Format("{0:00}:{1:00}:{2:00}", hold.Hours, hold.Minutes, hold.Seconds);
                report.TotalIdleTime    = String.Format("{0:00}:{1:00}:{2:00}", idle.Hours, idle.Minutes, idle.Seconds);
                report.TotalLoginTime   = String.Format("{0:00}:{1:00}:{2:00}", login.Hours, login.Minutes, login.Seconds);
                report.TotalNtRdyTime   = String.Format("{0:00}:{1:00}:{2:00}", nready.Hours, nready.Minutes, nready.Seconds);
                report.TotalReadyTime   = String.Format("{0:00}:{1:00}:{2:00}", ready.Hours, ready.Minutes, ready.Seconds);
                report.TotalTalkTime    = String.Format("{0:00}:{1:00}:{2:00}", talk.Hours, talk.Minutes, talk.Seconds);
                //
                dataContext.Consolidateds.InsertOnSubmit(report);
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4008: " + ex.Message);
            }
        }

        public void LogTable(TimeSpan acw, TimeSpan idle, TimeSpan nready, TimeSpan ready,
            TimeSpan hold, TimeSpan talk, TimeSpan login, TimeSpan handle, TimeSpan avgans,
            TimeSpan avgtalk, int totrcv, int totdial, int totans, int psl, int gsl)
        {
            try
            {
                Table table         = new Table();
                table.avgAnsSpeed   = avgans;
                table.avgHandTime   = handle;
                table.avgTalkTime   = avgtalk;
                table.Exten         = Extension;
                table.Name          = LoginName;
                table.GSL           = gsl;
                table.LoginTime = LoginTime.ToString("yyyyMMddHHmmss");
                table.PSL           = psl;
                table.totACWTime    = acw;
                table.totAnsCalls   = totans;
                table.totDialCalls  = totdial;
                table.totHoldTime   = hold;
                table.totLoginDur   = login;
                table.totNrdyTime   = nready;
                table.totRcvCalls   = totrcv;
                table.totTalkTime   = talk;
                dataContext.Tables.InsertOnSubmit(table);
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4009: " + ex.Message);
            }
        }

        public void ChangeState(State state)
        {
            /*try
            {
                switch (state)
                {
                    case State.OnCall:
                        currState.Idle      = false;
                        currState.ACW       = false;
                        currState.OnCall    = true;
                        break;
                    case State.ACW:
                        currState.Idle      = false;
                        currState.OnCall    = false;
                        currState.ACW       = true;
                        break;
                    case State.Idle:
                        currState.ACW       = false;
                        currState.OnCall    = false;
                        currState.Idle      = true;
                        break;
                    default:
                        break;
                }
                dataContext.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4010: " + ex.Message);
            }*/
        }

        public void LogLogoutCurrentState()
        {
            try
            {
                currState.LogoutTime = LogoutTime.ToString("yyyyMMddHHmmss");
                currState.IsLogin       = false;
                currState.IsReady       = false;
                currState.OnCall        = false;
                currState.Idle          = false;
                currState.ACW           = false;
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4011: " + ex.Message);
            }
        }

        public void LogBigStates(BigState state, string reason)
        {
            try
            {
                StateChangeEvent Event = new StateChangeEvent();
                switch (state)
                {
                    case BigState.Ready:
                        currState.IsReady   = true;
                        Event.Datetime      = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Event.Exten         = Extension;
                        Event.Name          = LoginName;
                        Event.State         = state.ToString();
                        Event.Reason        = "NIL";
                        break;
                    case BigState.NotReady:
                        currState.IsReady   = false;
                        currState.ACW       = false;
                        currState.Idle      = false;
                        currState.OnCall    = false;
                        Event.Datetime      = DateTime.Now.ToString("yyyyMMddHHmmss");
                        Event.Exten         = Extension;
                        Event.Name          = LoginName;
                        Event.State         = state.ToString();
                        Event.Reason        = reason;
                        break;
                    default:
                        break;
                }
                dataContext.StateChangeEvents.InsertOnSubmit(Event);
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.ToString(), "Error# 4012: " + ex.Message);
            }
        }
    }
}
