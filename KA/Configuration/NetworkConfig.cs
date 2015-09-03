using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.Configuration
{
    public static class NetworkConfig
    {
        public static readonly string dataUrl = "http://api.grundid.de/tree/near";
        public static readonly string paramLat = "latitude";
        public static readonly string paramLong = "longitude";
        public static readonly string treeType = "treeType";
    }
}
