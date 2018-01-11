using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iVoIP_Phone
{
    public class CCallDetailRecord
    {
        public enum EDisposition
        {
            ANSWERED,
            BUSY,
            REJECTED,
            NOANSWER
        }

        public enum EType
        {
            INCOMING,
            OUTGOING,
        }

        public string source { get; set; }
        public string destination { get; set; }
        public EType type { get; set; }
        public DateTime startTime { get; set; }
        public DateTime answerTime { get; set; }
        public DateTime endTime { get; set; }
        public EDisposition Disposition { get; set; }
        public string Workcode { get; set; }
        public int Duration { get; set; }
    }
}
