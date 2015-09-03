using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace KA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private async void MapControl_Loaded(object sender, RoutedEventArgs e)
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                     maximumAge: TimeSpan.FromMinutes(5),
                     timeout: TimeSpan.FromSeconds(10)
                    );

                var geopoint = new Geopoint(new BasicGeoposition()
                {
                    Latitude = geoposition.Coordinate.Latitude,
                    Longitude = geoposition.Coordinate.Longitude
                });

                Map.Center = geopoint;
                Map.ZoomLevel = 15;

                var treeList = await Helper.NetworkHelper.GetData(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                foreach(var feature in treeList.Features)
                {
                    MapIcon MapIcon1 = new MapIcon();


                    var point = (GeoJSON.Net.Geometry.Point)feature.Geometry;
                    var coords = (GeoJSON.Net.Geometry.GeographicPosition)point.Coordinates;

                    MapIcon1.Location = new Geopoint(new BasicGeoposition()
                    {
                        Latitude = coords.Latitude,
                        Longitude = coords.Longitude
                    });
                    //MapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    MapIcon1.Title = "Space Needle";
                    Map.MapElements.Add(MapIcon1);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}