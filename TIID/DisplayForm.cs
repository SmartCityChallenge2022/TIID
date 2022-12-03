using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Device.Location;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json;

namespace TIID
{
    public partial class DisplayForm : Form
    {
        public double currentLat;
        public double currentLon;
        private GeoCoordinateWatcher watcher;
        private int counter = 0;
        public DisplayForm()
        {
            InitializeComponent();
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            watcher = new GeoCoordinateWatcher();
            watcher.Start();
            
            
            map.MapProvider = GMapProviders.UMPMap;
            
            map.MinZoom = 5;
            map.MaxZoom = 100;
            map.Zoom = 15;
            map.Position = new GMap.NET.PointLatLng(0, 0);


            watcher.StatusChanged += StatusChangedEvent;
            watcher.PositionChanged += PositionChangedEvent;

        }
        private void StatusChangedEvent(object sender, GeoPositionStatusChangedEventArgs e)
        {
            GetCurrentLocation();
        }
        private void PositionChangedEvent(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GetCurrentLocation();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            
            pictureBox1.Image = imageList1.Images[counter];
            
            if (counter < imageList1.Images.Count-1)
                counter++;
            else counter = 0;
            UpdateWeather();
        }

        private void GetCurrentLocation()
        {
            try
            {
                Console.WriteLine(watcher.Status);
                if (watcher.Status == GeoPositionStatus.Ready)
                {
                    if (watcher.Position.Location.IsUnknown)
                    {
                        currentLat = 0;
                        currentLon = 0;
                    }
                    else
                    {
                        currentLat = watcher.Position.Location.Latitude;
                        currentLon = watcher.Position.Location.Longitude;
                        map.Position = new GMap.NET.PointLatLng(currentLat, currentLon);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void UpdateTime()
        {
            txtTime.Text = DateTime.Now.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private async void UpdateWeather()
        {
            var http = new HttpClient();

            var url = "https://api.openweathermap.org/data/2.5/weather?lat=" + currentLat +"&lon=" + currentLon + "&appid=e63e795d72e0ee742c4af3d081bc4c5d";

            http.DefaultRequestHeaders.Add(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 " +
                     "(KHTML, like Gecko) Chrome/51.0.2704.84 Safari/537.36");

            var response = await http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                //var weather = JsonConvert.DeserializeObject<WeatherNet.Model.CurrentWeatherResult>(json);
                //MessageBox.Show(weather.Temp.ToString() + " " +weather.Description);
                //MessageBox.Show(response.StatusCode.ToString());

                
                
            }
        }
    }
}
