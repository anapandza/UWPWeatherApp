using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPWeatherApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public RootObject Weather { get; set; }

        public HomePage()
        {
            this.InitializeComponent();

            this.Weather = new RootObject();
            this.DataContext = this.Weather;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RootObject temp;
            try
            {

                // get position in class location manager fetches geolocation which contains current coordinates of device
                var currentPosition = await LocationManager.GetPosition();
                //fetches root object in temp variable
                temp = await WeatherAPI.GetWeatherWithCoordinates(currentPosition.Coordinate.Point.Position.Latitude, currentPosition.Coordinate.Point.Position.Longitude);
            }
            catch
            {
                temp = await WeatherAPI.GetWeatherWithCityName("Zagreb");
            }

            
            this.Weather.city = temp.city; 
            this.Weather.list = temp.list; 

            for (int i = 0; i < 17; i += 4)
            {
                this.Weather.list[i].main = temp.list[i].main;
                this.Weather.list[0].weather = temp.list[0].weather;
            }

            CelsiusSign.Text = "°C";
            Time0.Text = "Time";
            Temperature0.Text = "Temperature";
            Description0.Text = "Description";
            Icon0.Text = "Icon";

            string icon = String.Format("ms-appx:///Assets/WeatherIcons/{0}.png", this.Weather.list[0].weather[0].icon);
            CurrentIcon.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));

            string icon1 = String.Format("ms-appx:///Assets/WeatherIcons/{0}.png", this.Weather.list[4].weather[0].icon);
            Icon1.Source = new BitmapImage(new Uri(icon1, UriKind.Absolute));

            string icon2 = String.Format("ms-appx:///Assets/WeatherIcons/{0}.png", this.Weather.list[8].weather[0].icon);
            Icon2.Source = new BitmapImage(new Uri(icon2, UriKind.Absolute));

            string icon3 = String.Format("ms-appx:///Assets/WeatherIcons/{0}.png", this.Weather.list[12].weather[0].icon);
            Icon3.Source = new BitmapImage(new Uri(icon3, UriKind.Absolute));

            string icon4 = String.Format("ms-appx:///Assets/WeatherIcons/{0}.png", this.Weather.list[16].weather[0].icon);
            Icon4.Source = new BitmapImage(new Uri(icon4, UriKind.Absolute));
        }
    }
}
