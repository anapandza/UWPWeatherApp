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
    // klasa u kojoj se dohvaca JSON sa web servisa https://openweathermap.org pomocu APIja 
    public class WeatherAPI
    {
        //dohvaca se vrijeme pomocu koordinata lat i lon
        //zbog toga sto je metoda asinkrona return objekt mora biti tipa task koji govori da kad je sve dovrseno ono sto se vraca bit ce rootobject
        public async static Task<RootObject> GetWeatherWithCoordinates(double lat, double lon)
        {
            //nova istanca klase za slanje HTTP zahtjeva i primanje HTTP odgovora od resursa identificiranog URI-jem
            var http = new HttpClient();

            // uri kojem saljemo zahtjev
            var uri = String.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&APPID=733d2c527b09b615dc30f6059bf4ac64&units=metric", lat, lon);

            //getAsync salje asinkroni zahtjev uriju i vraca odgovor na taj zahtjev 
            //await osigurava da se ceka rezultat 
            var response = await http.GetAsync(uri);

            //sadrzaj responsea se asinkrono sprema kao string format
            //u ovom slucaju to je JSON format kojeg treba deserijalizirati kako bi se objekti mogli koristiti u aplikaciji 
            var result = await response.Content.ReadAsStringAsync();

            //nova istanca klase koja sluzi za serilizaciju ili deserilizaciju JSON formata u tip objekta koji se koristi 
            //u ovom slucaju RootObject koji je povezan sa svim ostalim objektima
            var serializer = new DataContractJsonSerializer(typeof(RootObject));

            // sve podatke koji su vraceni u responsu tj. spremljeni kao string u result se spremaju se u memory stream
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));

            //ReadObject cita stream u JSON formatu i vraca deserijalizirani objekt koji nam treba
            var data = (RootObject)serializer.ReadObject(memoryStream);

            //vracamo podatke tipa rootobject
            return data;
        }

        //dohvaca se vrijeme pomocu naziva grada
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

    // objekti i klase koje sadrzi JSON

    // datacontract opisuje podatke koji se izmjenjuju izmedu klijenta (aplikacije) i servisa koji ne dijele iste tipove podataka
    [DataContract] //zajedno s datamember je potrebno za serilizaciju i deserilizaciju tj. u i iz JSON formata 
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

    /* nije ispravno
    public class Rain
    {
        public double __invalid_name__3h { get; set; }
    }

    public class Snow
    {
        public double __invalid_name__3h { get; set; }
    }
    */

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

        /* klase nisu ispravne
         public Rain rain { get; set; }
         public Snow snow { get; set; }
         */

        //ovo je za weather i main podklase
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
                //dohvacamo podatke unutar liste samo one koji ce se ispisati
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
        //ovo je za pristup podklasama city i list
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
