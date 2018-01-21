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


namespace UWPWeatherApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //otvori HomePage
            MainFrame.Navigate(typeof(HomePage));
            PageTitle.Text = "Home";  //postavi naslov na Home 
            Home.IsSelected = true;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            //ako je pane open zatvorit ce ga, a ako je zatvoren otvorit ce ga
            ForecastSplitView.IsPaneOpen = !ForecastSplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //kada se odabere home stranica
            if (Home.IsSelected)
            {
                MainFrame.Navigate(typeof(HomePage));
                PageTitle.Text = "Home";
            }

            if (ForecastForCity.IsSelected)
            {
                MainFrame.Navigate(typeof(ForecastForCityPage));
                PageTitle.Text = "Forecast For City";
            }

            if (ForecastForLocation.IsSelected)
            {
                MainFrame.Navigate(typeof(ForecastForLocationPage));
                PageTitle.Text = "Forecast For Location";
            }
        }
    }
}
