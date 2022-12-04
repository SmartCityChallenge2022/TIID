﻿using GMap.NET.MapProviders;
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
using static TIID.WeatherApiClass;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace TIID
{
    public partial class DisplayForm : Form
    {
        public double currentLat;
        public double currentLon;
        private GeoCoordinateWatcher watcher;
        private int counter = 0;
        private List<Stanica> listaStanica;
        private List<PointLatLng> points;
        public DisplayForm()
        {
            InitializeComponent();
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            FIllStanicaList();
            watcher = new GeoCoordinateWatcher();
            watcher.Start();


            map.MapProvider = GMapProviders.GoogleMap;

            map.MinZoom = 5;
            map.MaxZoom = 100;
            map.Zoom = 16;
            map.Position = new GMap.NET.PointLatLng(0, 0);


            watcher.StatusChanged += StatusChangedEvent;
            watcher.PositionChanged += PositionChangedEvent;

            
        }

        private void FIllStanicaList()
        {
            listaStanica = new List<Stanica>();
            listaStanica.Add(new Stanica("Spinut", -5));
            listaStanica.Add(new Stanica("Bakotićeva", -3));
            listaStanica.Add(new Stanica("Sedam Kaštela", -2));
            listaStanica.Add(new Stanica("Hrv. Mornarice", 0));
            listaStanica.Add(new Stanica("Dom. rata", 2));
            listaStanica.Add(new Stanica("Livanjska", 4));
            listaStanica.Add(new Stanica("Zagrebačka", 5));
            listaStanica.Add(new Stanica("Trajektna luka", 7));
            listaStanica.Add(new Stanica("Pojišan", 9));
            listaStanica.Add(new Stanica("Poljička", 11));
            listaStanica.Add(new Stanica("Bruna Bušića", 14));
            listaStanica.Add(new Stanica("Matice hrvatske", 16));
            listaStanica.Add(new Stanica("Velebitska", 19));
            listaStanica.Add(new Stanica("Vukovarska", 23));
            listaStanica.Add(new Stanica("Pujanke", 25));
            listaStanica.Add(new Stanica("Zagorski put", 27));
            listaStanica.Add(new Stanica("114. Brigade", 30));
            dgvStanice.DataSource = null;
            dgvStanice.DataSource = listaStanica;
            int i = 0;

            foreach (DataGridViewRow row in dgvStanice.Rows)
            {
                if (i == 3)
                {
                    row.DefaultCellStyle.BackColor = Color.Aquamarine;
                    break;
                }
                else i++;
            }
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

            if (counter < imageList1.Images.Count - 1)
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

            var url = "https://api.openweathermap.org/data/2.5/weather?lat=" + currentLat + "&lon=" + currentLon + "&appid=e63e795d72e0ee742c4af3d081bc4c5d";

            http.DefaultRequestHeaders.Add(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 " +
                     "(KHTML, like Gecko) Chrome/51.0.2704.84 Safari/537.36");

            var response = await http.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Root weather = JsonConvert.DeserializeObject<Root>(json);
                txtVrijeme.Text = ((int)(weather.main.temp - 273.15)).ToString() + "°C - " + weather.weather.First().description + 
                    " - Brzina vjetra: " + Math.Round(weather.wind.speed*3.6 , 2) +" Km/h";



            }

        }
        private void FindRoute()
        {

            /*points = new List<PointLatLng>();
            points.Add(new PointLatLng(43.5139, 16.4558));
            points.Add(new PointLatLng(43.520677, 16.466721));

           
            //var route = GoogleMapProvider.Instance.GetRoute(points[0], points[1], true, false, 14);
            MapRoute route = GMap.NET.MapProviders.GoogleMapProvider.Instance.GetRoute(
                points[0], points[1], false, false, 15);
            GMapRoute r = new GMapRoute(route.Points, "Autobusna linija 11");
            GMapOverlay routeOverlay = new GMapOverlay("routes");

            routeOverlay.Routes.Add(r);
            map.Overlays.Add(routeOverlay);
            r.Stroke.Width = 2;
            r.Stroke.Color = Color.Green;
            */
            //GMapOverlay markersOverlay = new GMapOverlay("markers");
            FillListOfMarkers();
            GMarkerGoogle marker;
            GMarkerGoogle marker2;
            int i = 0;
            foreach(PointLatLng p in points)
            {
                GMapOverlay markersOverlay = new GMapOverlay("markers");
                marker = new GMarkerGoogle(p, GMarkerGoogleType.blue_small);
                markersOverlay.Markers.Add(marker);
                map.Overlays.Add(markersOverlay);
                MessageBox.Show(i.ToString());
            }          
            /*marker = new GMarkerGoogle(new PointLatLng(43.5139, 16.4558), GMarkerGoogleType.blue_small);
            markersOverlay.Markers.Add(marker);
            marker2 = new GMarkerGoogle(new PointLatLng(43.520677, 16.466721), GMarkerGoogleType.blue_small);
            markersOverlay.Markers.Add(marker2);*/

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FindRoute();
        }
        private void FillListOfMarkers()
        {
            points = new List<PointLatLng>();
            points.Add(new PointLatLng(43.5139, 16.4558));
            points.Add(new PointLatLng(43.520677, 16.466721));
            /*points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            points.Add(new PointLatLng(43.520677, 16.466721));
            */
        }
    }
}
