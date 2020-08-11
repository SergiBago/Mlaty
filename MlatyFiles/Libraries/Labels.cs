using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;



namespace PGTA_WPF
{
    public class Labels : GMapMarker
    {
        //  public MeasureLine line;
        public string caption;
        public int num;
        public Labels(PointLatLng p, string caption)
            : base(p)
        {
            //his.line = line;
            this.caption= caption;
        }
    }
}