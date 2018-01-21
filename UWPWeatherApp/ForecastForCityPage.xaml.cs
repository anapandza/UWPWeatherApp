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
    public sealed partial class ForecastForCityPage : Page
    {
        public RootObject Weather { get; set; }

        public ForecastForCityPage()
        {
            this.InitializeComponent();

            this.Weather = new RootObject();
            this.DataContext = this.Weather;
        }

        private async void GetForecast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var city = CityName.Text;

                RootObject temp = await WeatherAPI.GetWeatherWithCityName(city);
                //binding za sve varijable
                this.Weather.city = temp.city; //binding klasu city za dohvacanje imena unutar te klase
                this.Weather.list = temp.list; //binding liste za pristup podacima unutar liste

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
            catch
            {
                ContentDialog wrongInputDialog = new ContentDialog
                {
                    Title = "Wrong input",
                    Content = "You entered unavaibale city. Please try again.",
                    IsPrimaryButtonEnabled = true,
                    PrimaryButtonText = "OK"
                };
                ContentDialogResult result = await wrongInputDialog.ShowAsync();
            }

        }
    }
}
