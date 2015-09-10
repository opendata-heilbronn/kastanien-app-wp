// ********************************************************
// <copyright file="MainPage.xaml.cs">
//    This file is under MIT license.
//    https://opensource.org/licenses/MIT
// </copyright>
// <author>Jonathan Günz</author>
// ********************************************************

namespace KA
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices.WindowsRuntime;
    using Windows.Devices.Geolocation;
    using Windows.Foundation;
    using Windows.Foundation.Collections;
    using Windows.Storage.Streams;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Maps;
    using Windows.UI.Xaml.Controls.Primitives;
    using Windows.UI.Xaml.Data;
    using Windows.UI.Xaml.Input;
    using Windows.UI.Xaml.Media;
    using Windows.UI.Xaml.Navigation;
    using Windows.UI.Xaml.Shapes;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Polygon oldPoly = null;
        StackPanel currentGrid = null;


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

                var featureList = await Helper.NetworkHelper.GetData(geoposition.Coordinate.Latitude, geoposition.Coordinate.Longitude);

                foreach (var feature in featureList.Features)
                {
                    var tree = new TreeModel(feature);

                    Polygon polygon = new Polygon();
                    polygon.Points.Add(new Point(0, 0));
                    polygon.Points.Add(new Point(0, 50));
                    polygon.Points.Add(new Point(20, 35));
                    polygon.Points.Add(new Point(20, 0));
                    polygon.Fill = new SolidColorBrush(Colors.Black);

                    Map.Children.Add(polygon);
                    MapControl.SetLocation(polygon, new Geopoint(tree.Location.Position));
                    MapControl.SetNormalizedAnchorPoint(polygon, new Point(0.0, 1.0));
                    polygon.Tag = tree;
                    polygon.Tapped += Polygon_Tapped;
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void Polygon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Polygon polygon = (Polygon)sender;

            TreeModel tree = (TreeModel)polygon.Tag;
            
            Map.Children.Remove(currentGrid);

            TextBlock tb = new TextBlock();

            var content = new StackPanel()
            {
                Margin = new Thickness(0, 0, 0, 25),
                Background = new SolidColorBrush(Colors.Black),
            };

            var text = new TextBlock
            {
                Text = tree.deuText,
                Foreground = new SolidColorBrush(Colors.White),
                FontSize = 20,
                Margin = new Thickness(5, 3, 5, 3),
            };
            var text2 = new TextBlock
            {
                Text = tree.LabelDesc,
                Foreground = new SolidColorBrush(Colors.White),
                Margin = new Thickness(5, 0, 5, 3),
                FontSize = 15,
            };
            content.Children.Add(text);
            content.Children.Add(text2);

            Map.Children.Add(content);
            MapControl.SetLocation(content, tree.Location);
            MapControl.SetNormalizedAnchorPoint(content, new Point(0.0, 1.0));

            currentGrid = content;
        }
    }
}