using GeoJSON.Net.Feature;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace KA.Helper
{
    public static class NetworkHelper
    {
        public static async Task<FeatureCollection> GetData(double lat, double lon)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.TryParseAdd("application/json");

            var url = string.Empty;

            url += Configuration.NetworkConfig.dataUrl;
            url += "?" + Configuration.NetworkConfig.paramLat + "=" + lat.ToString();
            url += "&" + Configuration.NetworkConfig.paramLong + "=" + lon.ToString();
            url += "&" + Configuration.NetworkConfig.treeType + "=CASTANEA";

            string ResponseString = await client.GetStringAsync(new Uri(url));

            var feature = JsonConvert.DeserializeObject<FeatureCollection>(ResponseString);

            return feature;
        }
    }
}
