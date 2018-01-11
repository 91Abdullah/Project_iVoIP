using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sipek.Common;

namespace iVoIP_Phone
{
    public class AccountConfig : IAccount
    {
        #region IAccount Members

        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string DomainName { get; set; }
        public string HostName { get; set; }
        public string Id { get; set; }
        public int Index { get; set; }
        public string Password { get; set; }
        public string ProxyAddress { get; set; }
        public int RegState { get; set; }
        public ETransportMode TransportMode { get { return ETransportMode.TM_UDP; } set { } }
        public string UserName { get; set; }
        public string AsteriskIP { get; set; }

        #endregion
    }

}
