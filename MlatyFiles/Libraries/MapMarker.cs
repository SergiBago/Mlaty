using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PGTA_WPF
{
    public class MapMarker
    {
        public int zone;
        public PointLatLng p;
        public int type; //0 MLAT, 1 ADSB, 2DGPS
        
        public MapMarker(int zone,PointLatLng p, int type)
        {
            this.zone = zone;
            this.p = p;
            this.type = type;
        }
    }
}
