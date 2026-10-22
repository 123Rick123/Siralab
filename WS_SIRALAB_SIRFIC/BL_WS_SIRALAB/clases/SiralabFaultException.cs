using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB.clases
{
    [DataContract(IsReference = true)]
    public class SiralabFaultException
    {
        private string _Message { get; set; }
        [DataMember]
        public string Message { get { return this._Message; } set { this._Message = value; } }
        public SiralabFaultException(string message)
        {
            this._Message = message;
        }
    }
}
