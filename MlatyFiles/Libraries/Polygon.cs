using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PGTA_WPF
{
    public class Polygon
    {
        public string id;
        public List<Point> Points = new List<Point>();
        public List<PointLatLng> PointsLatLng = new List<PointLatLng>();
        PointLatLng ARPBarcelona = new PointLatLng(41.2970767, 2.07846278);

        public Polygon(string name, List<Point> points)
        {
            this.id = name;
            this.Points = points;
            getListLatLng();
        }

        private void getListLatLng()
        {
            foreach(Point p in Points)
            {
                PointLatLng p2 = ComputeWGS_84_from_Cartesian(p);
                PointsLatLng.Add(p2);
            }
        }

        private PointLatLng ComputeWGS_84_from_Cartesian(Point p)
        {
            //PointLatLng AirportPoint = LibreriaDecodificacion.GetAirportARPCoorde(airportCode);
            PointLatLng Platlong = new PointLatLng();
            double X = p.X;
            double Y = p.Y;
            double R = 6371 * 1000;
            double d = Math.Sqrt((X * X) + (Y * Y));
            double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
            double Lat1 = 0;
            double Lon1 = 0;
            Lat1 = ARPBarcelona.Lat * (Math.PI / 180);
            Lon1 = ARPBarcelona.Lng * (Math.PI / 180);
            var Lat2 = Math.Asin(Math.Sin(Lat1) * Math.Cos(d / R) + Math.Cos(Lat1) * Math.Sin(d / R) * Math.Cos(brng));
            var Lon2 = Lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(Lat1), Math.Cos(d / R) - Math.Sin(Lat1) * Math.Sin(Lat2));
            Platlong.Lat = Lat2 * (180 / Math.PI);
            Platlong.Lng = Lon2 * (180 / Math.PI);
            return Platlong;
        }

    }
}
