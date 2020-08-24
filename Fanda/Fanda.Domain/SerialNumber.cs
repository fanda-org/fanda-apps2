using Fanda.Core;
using System;

namespace Fanda.Domain
{
    public class SerialNumber
    {
        // YY, YYYY, MM, MMM, DD
        // JJJ
        // N, NNN
        // HH, MI, SS, -- MS
        public Guid YearId { get; set; }

        public SerialNumberModule Module { get; set; }

        public string ModuleString
        {
            get { return Module.ToString(); }
            set { Module = (SerialNumberModule)Enum.Parse(typeof(SerialNumberModule), value, true); }
        }

        public string Prefix { get; set; }
        public string SerialFormat { get; set; }
        public string Suffix { get; set; }
        public string LastValue { get; set; }
        public int LastNumber { get; set; }
        public DateTime LastDate { get; set; }
        public SerialNumberReset Reset { get; set; }

        public string ResetString
        {
            get { return Reset.ToString(); }
            set { Reset = (SerialNumberReset)Enum.Parse(typeof(SerialNumberReset), value, true); }
        }

        public virtual AccountYear AccountYear { get; set; }
    }
}
