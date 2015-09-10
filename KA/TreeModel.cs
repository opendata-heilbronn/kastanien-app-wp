// ********************************************************
// <copyright file="TreeModel.cs">
//    This file is under MIT license.
//    https://opensource.org/licenses/MIT
// </copyright>
// <author>Jonathan Günz</author>
// ********************************************************

namespace KA
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GeoJSON.Net.Feature;
    using Windows.Devices.Geolocation;

    public class TreeModel
    {
        private Feature feature;

        public TreeModel(Feature feature)
        {
            this.feature = feature;

            this.Ort = feature.Properties["ort"].ToString();
            this.deuText = feature.Properties["deuText"].ToString();
            this.wisText = feature.Properties["wisText"].ToString();
            this.treeId = feature.Properties["treeId"].ToString();
            this.baumNr = Convert.ToInt32(feature.Properties["baumNr"].ToString());
            this.stadtteil = feature.Properties["stadtteil"].ToString();
            this.strasse = feature.Properties["strasse"].ToString();
            this.treeType = feature.Properties["treeType"].ToString();
            this.stUmfang = Convert.ToDouble(feature.Properties["stUmfang"].ToString());
            this.kroneDm = Convert.ToDouble(feature.Properties["kroneDm"].ToString());
            this.typ = feature.Properties["typ"].ToString();

            var point = (GeoJSON.Net.Geometry.Point)feature.Geometry;
            var coords = (GeoJSON.Net.Geometry.GeographicPosition)point.Coordinates;
            this.Location = new Geopoint(new BasicGeoposition()
            {
                Latitude = coords.Latitude,
                Longitude = coords.Longitude
            });
        }

        public string Ort { get; set; }

        public string deuText { get; set; }

        public string wisText { get; set; }

        public string treeId { get; set; }

        public int baumNr { get; set; }

        public string stadtteil { get; set; }

        public string strasse { get; set; }

        public string treeType { get; set; }

        public double stUmfang { get; set; }

        public double kroneDm { get; set; }

        public string LabelDesc { get
            {
                if (this.kroneDm > 4)
                    return "Großer Baum - viele Kastanien";
                return "Kleiner Baum - wenig Kastanien";
            }
        }

        public string typ { get; set; }

        public Geopoint Location { get; set; }
    }
}
