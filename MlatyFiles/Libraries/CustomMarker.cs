using GMap.NET.WindowsPresentation;
using GMap.NET;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGTA_WPF
{
    public class CustomMarker : GMapMarker
    {
        public int type; //0= MLAT 1=ADSB
        public CustomMarker(PointLatLng p, int type)
            : base(p)
        {
            this.type = type;
        }
    }
}
