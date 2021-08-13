using GMap.NET;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PGTA_WPF
{
    public class MarkerDGPS
    {
        public PointLatLng p { get; set; }
        public Point Pxy { get; set; }
        public double Time { get; set; }
        public double Height { get; set; }
        public int zone { get; set; }
        public bool used { get; set; } = false;
        public bool saved { get; set; } = false;

        public MarkerDGPS(PointLatLng p,Point Pxy, double t)
        {
            this.p = p;
            this.Pxy = Pxy;
            this.Time = t;
        }

        public MarkerDGPS(PointLatLng p, Point Pxy, double t,double Height)
        {
            this.p = p;
            this.Pxy = Pxy;
            this.Time = t;
            this.Height = Height;
        }

        public MarkerDGPS()
        {
        }
    }
}
