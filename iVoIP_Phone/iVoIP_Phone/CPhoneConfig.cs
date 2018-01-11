using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sipek.Common;

namespace iVoIP_Phone
{
    public class PhoneConfig : IConfiguratorInterface
    {
        List<IAccount> _acclist = new List<IAccount>();

        #region IConfiguratorInterface Members

        public PhoneConfig()
        {
            _acclist.Add(new AccountConfig());
        }

        public List<IAccount> Accounts
        {
            get { return _acclist; }
        }

        public bool AAFlag { get; set; }
        public bool CFBFlag { get; set; }
        public string CFBNumber { get; set; }
        public bool CFNRFlag { get; set; }
        public string CFNRNumber { get; set; }
        public bool CFUFlag { get; set; }
        public string CFUNumber { get; set; }
        public List<string> CodecList { get; set; }
        public bool DNDFlag { get; set; }
        public int DefaultAccountIndex { get { return 0; } }
        public bool IsNull { get { return false; } }
        public bool PublishEnabled { get { throw new NotImplementedException(); } set { throw new NotImplementedException(); } }
        public int SIPPort { get; set; }
        public void Save()
        { }

        #endregion
    }
}
