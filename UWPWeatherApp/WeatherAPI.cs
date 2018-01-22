using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace UWPWeatherApp
{
    // gets JSON from web servis https://openweathermap.org with API 
    public class WeatherAPI
    {
        //gets weather with coordinates
        //because of async method return object is type task which says that once everything is done it will return root object
        public async static Task<RootObject> GetWeatherWithCoordinates(double lat, double lon)
        {
            //new istance of class for sending HTTP requests and receaving HTTP replys from resource identifyed by URI
            var http = new HttpClient();

            // URI to which request is sent
            var uri = String.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&APPID=733d2c527b09b615dc30f6059bf4ac64&units=metric", lat, lon);

            //getAsync sends asyncronus request to URI and gets reply for that request 
            //await ensures that result is waites for 
            var response = await http.GetAsync(uri);

            //content of response is saved as string
            //in this case ids JSON which has to be deserialized so that objects could be used in app 
            var result = await response.Content.ReadAsStringAsync();

            //new istance of class which serilizes or deserilizes JSON into type of used object 
            //in this case its RootObject which is connected to all other objects
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));

            //ReadObject reads stream in JSON and returns deserilized object
            var data = (RootObject)serializer.ReadObject(memoryStream);

            //returning data in type of rootobject
            return data;
        }

        //gets weather by city name
        public async static Task<RootObject> GetWeatherWithCityName(string city)
        {
            var http = new HttpClient();
            var uri = String.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&APPID=733d2c527b09b615dc30f6059bf4ac64&units=metric", city);
            var response = await http.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(memoryStream);

            return data;
        }
    }

    // objects and classes that JSON contains

    // datacontract describes data which are excanged between client and service which dont share same types of data
    [DataContract] //with datamember its needed for serilization and deserilization 
    public class Main : INotifyPropertyChanged
    {
        [DataMember]
        private double _temp;
        [DataMember]
        public double temp
        {
            get { return this._temp; }
            set
            {
                this._temp = value;
                this.NotifyPropertyChanged("temp");
            }
        }

        [DataMember]
        public double temp_min { get; set; }

        [DataMember]
        public double temp_max { get; set; }

        [DataMember]
        public double pressure { get; set; }

        [DataMember]
        public double sea_level { get; set; }

        [DataMember]
        public double grnd_level { get; set; }

        [DataMember]
        public int humidity { get; set; }

        [DataMember]
        public double temp_kf { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [DataContract]
    public class Weather : INotifyPropertyChanged
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string main { get; set; }

        [DataMember]
        public string _description;
        [DataMember]
        public string description
        {
            get { return this._description; }
            set
            {
                this._description = value;
                this.NotifyPropertyChanged("description");
            }
        }

        [DataMember]
        private string _icon;
        [DataMember]
        public string icon
        {
            get { return this._icon; }
            set
            {
                this._icon = value;
                this.NotifyPropertyChanged("icon");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [DataContract]
    public class Clouds
    {
        [DataMember]
        public int all { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember]
        public double speed { get; set; }

        [DataMember]
        public double deg { get; set; }
    }

    [DataContract]
    public class Sys
    {
        [DataMember]
        public string pod { get; set; }
    }

    [DataContract]
    public class List : INotifyPropertyChanged
    {
        [DataMember]
        public int dt { get; set; }

        [DataMember]
        private Main _main;
        [DataMember]
        public Main main
        {
            get { return this._main; }
            set
            {
                this._main = value;
                main.PropertyChanged += AddressChanged;
                this.NotifyPropertyChanged("main");
            }
        }

        [DataMember]
        private List<Weather> _weather;
        [DataMember]
        public List<Weather> weather
        {
            get { return this._weather; }
            set
            {
                this._weather = value;
                weather[0].PropertyChanged += AddressChanged;
                this.NotifyPropertyChanged("weather");
            }
        }

        [DataMember]
        public Clouds clouds { get; set; }

        [DataMember]
        public Wind wind { get; set; }

        [DataMember]
        public Sys sys { get; set; }

        [DataMember]
        private string _dt_txt;
        [DataMember]
        public string dt_txt
        {
            get { return this._dt_txt; }
            set
            {
                this._dt_txt = value;
                this.NotifyPropertyChanged("dt_txt");
            }
        }

        //this is for weather and main podclasses
        private void AddressChanged(object sender, PropertyChangedEventArgs args)
        {
            NotifyPropertyChanged("main");
            NotifyPropertyChanged("weather");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [DataContract]
    public class Coord
    {
        [DataMember]
        public double lat { get; set; }

        [DataMember]
        public double lon { get; set; }
    }

    [DataContract]
    public class City : INotifyPropertyChanged
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        private string _name;
        [DataMember]
        public string name
        {
            get { return this._name; }
            set
            {
                this._name = value;
                this.NotifyPropertyChanged("name");
            }
        }

        [DataMember]
        public Coord coord { get; set; }

        [DataMember]
        public string country { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [DataContract]
    public class RootObject : INotifyPropertyChanged
    {
        [DataMember]
        public string cod { get; set; }

        [DataMember]
        public double message { get; set; }

        [DataMember]
        public int cnt { get; set; }

        [DataMember]
        private List<List> _list;
        [DataMember]
        public List<List> list
        {
            get { return this._list; }
            set
            {
                _list = value;
                //getting data which will be printed inside of the list
                for (int i = 0; i < 17; i += 4)
                {
                    _list[i].PropertyChanged += AddressChanged;
                }
                NotifyPropertyChanged("list");
            }
        }

        [DataMember]
        private City _city;
        [DataMember]
        public City city
        {
            get { return this._city; }
            set
            {
                _city = value;
                _city.PropertyChanged += AddressChanged;
                NotifyPropertyChanged("city");
            }
        }
        //this is for city and list podclasses
        private void AddressChanged(object sender, PropertyChangedEventArgs args)
        {
            NotifyPropertyChanged("city");
            NotifyPropertyChanged("list");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
