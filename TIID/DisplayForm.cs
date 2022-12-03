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

namespace TIID
{
    public partial class DisplayForm : Form
    {
        public double currentLat;
        public double currentLon;
        private GeoCoordinateWatcher watcher;
        public DisplayForm()
        {
            InitializeComponent();
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            watcher = new GeoCoordinateWatcher();
            watcher.Start();
            //watcher.PositionChanged += GetCurrentLocation;
            GetCurrentLocation();
            //map.MapProvider = GMapProviders.GoogleMap;
            
            map.MinZoom = 5;
            map.MaxZoom = 100;
            map.Zoom = 10;
            //map.Position = new GMap.NET.PointLatLng(currentLon, currentLat);
           

        }
        private void GetCurrentLocation()
        {
            /* try
             {
                 if(e.Status == GeoPositionStatus.Ready)
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
                     }
                 }
             }
             catch (Exception)
             {

                 throw;
             }
             MessageBox.Show(currentLat.ToString() + " " + currentLon.ToString());*/
            MessageBox.Show(watcher.Position.Location.Longitude + " " + watcher.Position.Location.Latitude);
        }
    }
}
