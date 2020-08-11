using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGTA_WPF
{
    public class PrecissionPoint
    {
        public string Callsign;
        public double localX;
        public double localY;
        public double ARPH;
        public double GPSX;
        public double GPSY;
        public double GPSZ;
        public double ErrorLocalX;
        public double ErrorLocalY;
        public double ErrorLocalXY;
        public string Area;
        public int GroundBit;
        public string time;

        public PrecissionPoint()
        {

        }

        public PrecissionPoint(string Callsign, double LocalX, double LocalY, double ARPH,  double GPSX,double GPSY, double GPSZ, double ErrorLocalX, double ErrorLocalY, double ErrorLocalXY, int area, int GB, double time)
        {
            this.Callsign = Callsign;
            this.localX = LocalX;
            this.localY = LocalY;
            this.ARPH = ARPH;
            this.GPSX = GPSX;
            this.GPSY = GPSY;
            this.GPSZ = GPSZ;
            this.ErrorLocalX = ErrorLocalX;
            this.ErrorLocalY = ErrorLocalY;
            this.ErrorLocalXY = ErrorLocalXY;
            this.GroundBit = GB;
            this.time = ComputeTime(time);
            this.Area = ComputeArea(area);
        }


        private string ComputeTime(double t)
        {
            TimeSpan tiempo = TimeSpan.FromSeconds(t);
            string time = tiempo.ToString(@"hh\:mm\:ss\:fff");
            return time;
        }

        private string ComputeArea(int i)
        {
            string Area="";
            if (i==1)
            {
                Area = "Runway25L";
            }
            if (i == 2)
            {
                Area = "Runway02";
            }
            if (i == 3)
            {
                Area = "Runway25R";
            }
            if (i == 4)
            {
                Area = "StandT1";
            }
            if (i == 5)
            {
                Area = "StandT2";
            }
            if (i == 6)
            {
                Area = "ApronT1";
            }
            if (i == 7)
            {
                Area = "ApronT2";
            }
            if (i == 8)
            {
                Area = "TaxiZones";
            }
            if (i == 9)
            {
                Area = "Airborne25RZones25";
            }
            if (i == 10)
            {
                Area = "Airborne02Zones25";
            }
            if (i == 11)
            {
                Area = "Airborne25LZones25";
            }
            if (i == 12)
            {
                Area = "Airborne25RZones5";
            }
            if (i == 13)
            {
                Area = "Airborne02Zones5";
            }
            if (i == 14)
            {
                Area = "Airborne25LZones5";
            }
            return Area;
        }
    }
}
