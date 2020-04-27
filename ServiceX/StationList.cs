using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceX
{
    [DataContract]
    public class StationList
    {
        [DataMember]
        public List<VelibStation> stationList;
    }
}
