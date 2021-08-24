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

    public class VehicleTrajectories
    {
        public string TargetAddress;
        public List<MapTrajectory> ADSBTrajectories = new List<MapTrajectory>();
        public List<MapTrajectory> DGPSTrajectories = new List<MapTrajectory>();
        public List<MapTrajectory> MLATTrajectories = new List<MapTrajectory>();

        public VehicleTrajectories(string Adress)
        {
            TargetAddress = Adress;
        }

    }

    public class MapTrajectory : ICloneable
    {
        public string TargetAddress;
        private List<PointLatLng> listPoints;
        public List<PointLatLng> ListPoints
        {

            get 
            {
                return listPoints;
            }
            set
            {
                listPoints = new List<PointLatLng>();
                foreach (PointLatLng p in value)
                {
                    this.listPoints.Add(new PointLatLng(p.Lat, p.Lng));
                }
            }
        } 
        public int Type; //0 MLAT, 1 ADSB, 2DGPS


        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public MapTrajectory(string TargetAddress, List<PointLatLng> PointsList, int Type)
        {
            this.TargetAddress = TargetAddress;
            ListPoints = PointsList;
            this.Type = Type;
        }

        public MapTrajectory(string TargetAddress,int Type)
        {
            this.TargetAddress = TargetAddress;
            this.Type = Type;
        }

    }
}
