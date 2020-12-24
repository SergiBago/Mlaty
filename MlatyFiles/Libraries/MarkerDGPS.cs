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
        public PointLatLng p;
        public Point Pxy;
        public double Time;
        public double Height;
        public int zone;
        public bool used = false;
        public bool saved = false;

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
