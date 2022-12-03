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
        int counter = 0;
        public DisplayForm()
        {
            InitializeComponent();
        }

        private void DisplayForm_Load(object sender, EventArgs e)
        {
            watcher = new GeoCoordinateWatcher();
            watcher.Start();
            
            GetCurrentLocation();
            map.MapProvider = GMapProviders.GoogleMap;
            
            map.MinZoom = 5;
            map.MaxZoom = 100;
            map.Zoom = 15;
            map.Position = new GMap.NET.PointLatLng(43.5145096, 16.4229069);

           

        }
        private void GetCurrentLocation()
        {
            try
            {
                if(watcher.Status == GeoPositionStatus.Ready)
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
            MessageBox.Show(currentLat.ToString() + " " + currentLon.ToString());
        }
        
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            pictureBox1.Image = imageList1.Images[counter];
            
            if (counter < imageList1.Images.Count-1)
                counter++;
            else counter = 0;
        }
        private void UpdateTime()
        {
            txtTime.Text = DateTime.Now.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }
    }
}
