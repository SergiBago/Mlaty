using GMap.NET;
using GMap.NET.MapProviders;
using PGTA_WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.Entity.Core.Metadata.Edm;
using System.Windows;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace PGTAWPF
{
    public class TrajectoriestoCompute
    {
        public List<CAT10> ListMLAT = new List<CAT10>();
        public List<CAT21vs21> ListADSB = new List<CAT21vs21>();
        public List<MarkerDGPS> ListDGPS = new List<MarkerDGPS>();
        public string TargetIdentification;
        public string TargetAdress;
        public int TrackNumber=-1;

        public bool aircraftMLAT = false;
        public bool aircraftADSB = false;

        // Data data;

        public string PD;
        public PointLatLng ARPCoords = new PointLatLng(41.2970767, 2.07846278);


        public TrajectoriestoCompute(CAT10 message)
        {
            ListMLAT.Add(message);
            if (message.Target_Identification != null)
            {
                this.TargetIdentification = message.Target_Identification;
            }
            if (message.Target_Address != null)
            {
                this.TargetAdress = message.Target_Address;
            }
            if (message.Track_Number != -1)
            {
                this.TrackNumber = message.Track_Number;
            }

        }

        public TrajectoriestoCompute(CAT21vs21 message)
        {
            ListADSB.Add(message);
            if (message.Target_Identification != null)
            {
                this.TargetIdentification = message.Target_Identification;
            }
            if (message.Target_address != null)
            {
                this.TargetAdress = message.Target_address;
            }
            if (message.Track_Number != -1)
            {
                this.TrackNumber = message.Track_Number;
            }
        }

        public TrajectoriestoCompute()
        {
        }

        public void setAtributes(string TargetID, string TargetAdd)
        {
            this.TargetIdentification = TargetID;
            this.TargetAdress = TargetAdd;
        }


        public void ADDCAT21(CAT21vs21 message)
        {
            ListADSB.Add(message);
            if(TrackNumber==-1)
            {
                TrackNumber = message.Track_Number;
            }
            if(TargetAdress==null)
            {
                TargetAdress = message.Target_address;
            }
            if(TargetIdentification==null)
            {
                TargetIdentification = message.Target_Identification;
            }
        }

        public void ADDCAT10(CAT10 message)
        {
            ListMLAT.Add(message);
            if (TrackNumber == -1)
            {
                TrackNumber = message.Track_Number;
            }
            if (TargetAdress == null)
            {
                TargetAdress = message.Target_Address;
            }
            if (TargetIdentification == null)
            {
                TargetIdentification = message.Target_Identification;
            }
        }

        public void ADDDGPS(MarkerDGPS marker)
        {
            ListDGPS.Add(marker);
        }


        private void ComputeZonePD(double startTime, double endTime, int zone, Data data)
        {
            if (zone != -1)
            {
                double refreshrate = 0;
                if (zone == 1 || zone == 2 || zone == 3 || zone == 6 || zone == 7 || zone == 8) { refreshrate = 2; }
                if (zone == 4 || zone == 5) { refreshrate = 5; }
                if (zone == 9 || zone == 10 || zone == 11 || zone == 12 || zone == 13 || zone == 14) { refreshrate = 2; }

                int expected = 0;
                // int missing = 0;
                int refrat = Convert.ToInt32(refreshrate);
                List<CAT10> ListPDMLAT = new List<CAT10>();
                foreach (CAT10 MLAT in ListMLAT)
                {
                    if (MLAT.Time_milisec >= startTime -refreshrate && MLAT.Time_milisec <= endTime + refreshrate)
                    {
                        ListPDMLAT.Add(MLAT);
                    }
                }
                List<double> times = new List<double>();
                if (ListPDMLAT.Count > 0)
                {
                    double Start = ListPDMLAT[0].Time_milisec;
                    for (int i = 1; i < ListPDMLAT.Count; i++)
                    {

                        if (ListPDMLAT[i].Time_milisec > ListPDMLAT[i - 1].Time_milisec + 30)
                        {
                            times.Add(Start);
                            times.Add(ListPDMLAT[i - 1].Time_milisec);
                            Start = ListPDMLAT[i].Time_milisec;
                        }
                    }
                }
                if (times.Count == 0)
                {
                    for (int i = Convert.ToInt32(startTime + (refreshrate )); i < Convert.ToInt32(endTime - (refreshrate )); i+=refrat)
                    {
                        double mintime = (i-(refreshrate/2));
                        double maxtime = (i + (refreshrate/2));
                        bool found = false;
                        foreach (CAT10 MLAT in ListMLAT)
                        {
                            if (MLAT.Time_milisec >= mintime && MLAT.Time_milisec <= maxtime)
                            {
                                if (ComputeMessageAccuracy(MLAT))
                                {
                                    found = true;
                                    MLAT.used = true;
                                    break;
                                }

                            }
                        }
                        if (found == false)
                        {
                            data.ListZones[zone-1].MissedMLATSPD++; 
                        }
                    }
                    expected = Convert.ToInt32((endTime - startTime) / refreshrate);
                }
                else
                {

                    for (int s = 0; s < times.Count; s += 2)
                    {
                        startTime = times[s];
                        endTime = times[s + 1];
                        expected += Convert.ToInt32((endTime - startTime) / refreshrate);
                        for (int i = Convert.ToInt32(startTime + (refreshrate )); i < Convert.ToInt32(endTime - (refreshrate )); i += refrat)
                        {
                            double mintime = i - (refreshrate / 2);
                            double maxtime = i + (refreshrate / 2);
                            bool found = false;
                            foreach (CAT10 MLAT in ListMLAT)
                            {
                                if (MLAT.Time_milisec >= mintime && MLAT.Time_milisec <= maxtime)
                                {
                                    if (ComputeMessageAccuracy(MLAT))
                                    {
                                        found = true;
                                        MLAT.used = true;
                                        break;
                                    }
                                }
                            }
                            if (found == false) { data.ListZones[zone-1].MissedMLATSPD++; }
                        }
                    }
                }
                data.ListZones[zone-1].ExpectedMessagesPD += expected;

            }
        }


        private bool ComputeMessageAccuracy(CAT10 MLAT)
        {
            bool Less50m = false;
            PointWithHeight p=null;
            if (ListADSB.Count>0)
            {
                CAT21vs21 Before = new CAT21vs21();
                CAT21vs21 After = new CAT21vs21();
                bool Correct = false;
                bool BeforeFound = false;
                bool AfterFound = false;
                for (int i = 0; true; i++)
                {
                    CAT21vs21 ADSB = ListADSB[i];
                    if (ADSB.Time_milisec <= MLAT.Time_milisec && ADSB.Time_milisec > MLAT.Time_milisec - 5)
                    {
                        Before = ADSB;
                        BeforeFound = true;
                    }
                    if (ADSB.Time_milisec >= MLAT.Time_milisec && ADSB.Time_milisec < MLAT.Time_milisec + 5 && AfterFound == false)
                    {
                        After = ADSB;
                        AfterFound = true;
                    }
                    if (BeforeFound == true && AfterFound == true)
                    {
                        Correct = true;
                        break;
                    }
                    if (i == ListADSB.Count - 1)
                    {
                        break;
                    }
                }
                if (Correct == true)
                {
                    p = ComputePositionXY(Before, After, MLAT);
                }
            }
            else if(ListDGPS.Count>0)
            {
                MarkerDGPS Before = new MarkerDGPS();
                MarkerDGPS After = new MarkerDGPS();
                bool Correct = false;
                bool BeforeFound = false;
                bool AfterFound = false;
                for (int i = 0; true; i++)
                {
                    MarkerDGPS DGPS = ListDGPS[i];
                    if (DGPS.Time <= MLAT.Time_milisec && DGPS.Time > MLAT.Time_milisec - 5)
                    {
                        Before = DGPS;
                        BeforeFound = true;
                    }
                    if (DGPS.Time >= MLAT.Time_milisec && DGPS.Time < MLAT.Time_milisec + 5 && AfterFound == false)
                    {
                        After = DGPS;
                        AfterFound = true;
                    }
                    if (BeforeFound == true && AfterFound == true)
                    {
                        Correct = true;
                        break;
                    }
                    if (i == ListDGPS.Count - 1)
                    {
                        break;
                    }
                }
                if (Correct == true)
                {
                    p = ComputePositionXY(Before, After, MLAT);
                }
               
            }
            if (p != null)
            {
                Point MLATp = new Point(MLAT.X_Component_map, MLAT.Y_Component_map);
                double dist = ComputeDistanceXY(MLATp, p);
                if (dist < 50)
                {
                    Less50m = true;
                }
            }
            return Less50m;
        }

        private void ComputeZoneUP(double startTime, double endTime, int zone, Data data)
        {
            if (zone != -1)
            {
                double refreshrate = 1;
                int expected = 0;
                int refrat = Convert.ToInt32(refreshrate);
                List<CAT10> ListUPMLAT = new List<CAT10>();
                data.ListZones[zone].expected_PDok++;
                foreach (CAT10 MLAT in ListMLAT)
                {
                    if (MLAT.Time_milisec >= startTime - refreshrate && MLAT.Time_milisec <= endTime + refreshrate)
                    {
                        ListUPMLAT.Add(MLAT);
                    }
                }
                List<double> times = new List<double>();
                if (ListUPMLAT.Count > 0)
                {
                    double Start = ListUPMLAT[0].Time_milisec;
                    for (int i = 1; i < ListUPMLAT.Count; i++)
                    {

                        if (ListUPMLAT[i].Time_milisec > ListUPMLAT[i - 1].Time_milisec + 30)
                        {
                            times.Add(Start);
                            times.Add(ListUPMLAT[i - 1].Time_milisec);
                            Start = ListUPMLAT[i].Time_milisec;
                        }
                    }
                }
                if (times.Count == 0)
                {
                    double time = (startTime + (refreshrate / 2));
                    int first = 0;
                    double TimeDif = 10000000;
                    double mintime = (startTime - (refreshrate / 2));
                    double maxtime = (startTime + (refreshrate / 2));
                    for (int i = 0; i < ListMLAT.Count(); i++)
                    {
                        CAT10 MLAT = ListMLAT[i];
                        if (MLAT.Time_milisec >= mintime && MLAT.Time_milisec <= maxtime)
                        {
                            first = i;
                            MLAT.used = true;
                            break;
                        }
                        else
                        {
                            if (Math.Abs(MLAT.Time_milisec-mintime)<TimeDif)
                            {
                                TimeDif = Math.Abs(MLAT.Time_milisec - mintime);
                                first = 0;
                            }
                        }
                    }
                    for (int i = first; i < ListMLAT.Count(); i++)
                    {
                        CAT10 MLAT = ListMLAT[i];

                        if (MLAT.Time_milisec >= time-refreshrate && MLAT.Time_milisec <= time + refreshrate)
                        {
                            MLAT.used = true;
                            time = MLAT.Time_milisec;

                        }
                        else
                        {
                            if (MLAT.Time_milisec > time + refreshrate)
                            {
                                int miss = Convert.ToInt32(MLAT.Time_milisec - time);
                                data.ListZones[zone - 1].MissedMLATSUP+=miss;
                                time = MLAT.Time_milisec;
                            }

                        }
                        if (time >= endTime)
                        {
                            break;
                        }
                    }
                    data.ListZones[zone - 1].ExpectedMessagesUP += Convert.ToInt32((endTime - startTime) / refreshrate);
                }
                else
                {

                    for (int s = 0; s < times.Count; s += 2)
                    {
                        startTime = times[s];
                        endTime = times[s + 1];
                        expected += Convert.ToInt32((endTime - startTime) / refreshrate);
                        double time = (startTime + (refreshrate / 2));
                        double TimeDif = 10000000;
                        int first = 0;
                        double mintime = (startTime - (refreshrate / 2));
                        double maxtime = (startTime + (refreshrate / 2));
                        for (int i = 0; i < ListMLAT.Count(); i++)
                        {
                            CAT10 MLAT = ListMLAT[i];
                            if (MLAT.Time_milisec >= mintime && MLAT.Time_milisec <= maxtime)
                            {
                                first = i;
                                MLAT.used = true;
                                break;
                            }
                            else
                            {
                                if (Math.Abs(MLAT.Time_milisec - mintime) < TimeDif)
                                {
                                    TimeDif = Math.Abs(MLAT.Time_milisec - mintime);
                                    first = 0;
                                }
                            }
                        }
                        for (int i = first; i < ListMLAT.Count(); i++)
                        {
                            CAT10 MLAT = ListMLAT[i];

                            if (MLAT.Time_milisec >= time && MLAT.Time_milisec <= time + refreshrate)
                            {
                                MLAT.used = true;
                                time = MLAT.Time_milisec;

                            }
                            else
                            {
                                if (MLAT.Time_milisec > time + refreshrate)
                                {
                                    int miss = Convert.ToInt32(MLAT.Time_milisec - time);
                                    data.ListZones[zone - 1].MissedMLATSUP += miss;
                                    time = MLAT.Time_milisec;
                                }
                            }
                            if (time >= endTime)
                            {
                                break;
                            }
                        }
                    }
                    data.ListZones[zone - 1].ExpectedMessagesUP += expected;
                }
            }
        }

        public void SaveMarkers(List<MapMarker> listMarkers)
        {
            foreach (CAT10 MLAT in ListMLAT)
            {
                if (MLAT.used==true && MLAT.saved==false && MLAT.zone != -1)
                {
                    PointLatLng p = new PointLatLng(MLAT.LatitudeWGS_84_map, MLAT.LongitudeWGS_84_map);
                    MapMarker mark = new MapMarker(MLAT.zone, p, 0);
                    listMarkers.Add(mark);
                }
            }
            foreach (CAT21vs21 ADSB in ListADSB)
            {
                if (ADSB.used==true && ADSB.saved==false && ADSB.zone != -1)
                {
                    PointLatLng p = new PointLatLng(ADSB.LatitudeWGS_84_map, ADSB.LongitudeWGS_84_map);
                    MapMarker mark = new MapMarker(ADSB.zone, p, 1);
                    listMarkers.Add(mark);
                }
            }
            foreach (MarkerDGPS DGPS in ListDGPS)
            {      
                if (DGPS.used == true && DGPS.saved == false && DGPS.zone != -1)
                {
                    PointLatLng p = new PointLatLng(DGPS.p.Lat, DGPS.p.Lng);
                    MapMarker mark = new MapMarker(DGPS.zone, p, 2);
                    listMarkers.Add(mark);
                }
            }

        }


        public void SetZones(LibreriaDecodificacion lib)
        {
            for (int i = 0; i < ListMLAT.Count; i++)
            {
                CAT10 MLAT = ListMLAT[i];
                //if (MLAT.TOT == "Aircraft")
                //{

                    Point p = new Point(MLAT.X_Component_map, MLAT.Y_Component_map);
                    MLAT.zone = lib.ComputeZone(p, MLAT.GroundBit);
                    if (MLAT.zone == 10)
                    {
                        if (i < ListMLAT.Count - 2)
                        {
                            MLAT.zone = lib.RecomputeZone(null, MLAT, ListMLAT[i + 1]);
                        }
                        else if (i > 0)
                        {
                            MLAT.zone = lib.RecomputeZone(ListMLAT[i - 1], MLAT, null);
                        }
                    }

                //}
            }
            for (int i = 0; i < ListADSB.Count; i++)
            {
                CAT21vs21 ADSB = ListADSB[i];
                //if (ADSB.ECAT == "Light aircraft" || ADSB.ECAT == "Small aircraft" || ADSB.ECAT == "Medium aircraft" || ADSB.ECAT == "Heavy aircraft")
                //{
                    Point p = new Point(ADSB.X_Component_map, ADSB.Y_Component_map);
                    ADSB.zone = lib.ComputeZone(p, ADSB.GroundBit);
                    if (ADSB.zone == 10)
                    {
                        if (i < ListADSB.Count - 2)
                        {
                            ADSB.zone = lib.RecomputeZone(null, ADSB, ListADSB[i + 1]);
                        }
                        else if (i > 0)
                        {
                            ADSB.zone = lib.RecomputeZone(ListADSB[i - 1], ADSB, null);
                        }
                    }
                    //if (save == true && ADSB.zone!=-1)
                    //{
                    //    AddMapMarker(ADSB, listMarkers);
                    //}
                //}
            }
            for (int i = 0; i < ListDGPS.Count; i++)
            {
                MarkerDGPS DGPS = ListDGPS[i];
                //if (ADSB.ECAT == "Light aircraft" || ADSB.ECAT == "Small aircraft" || ADSB.ECAT == "Medium aircraft" || ADSB.ECAT == "Heavy aircraft")
                //{

                DGPS.zone = lib.ComputeZone(DGPS.Pxy, 2);
                if (DGPS.zone == 10)
                {
                    if (i < ListADSB.Count - 2)
                    {
                        DGPS.zone = lib.RecomputeZone(null, DGPS, ListDGPS[i + 1]);
                    }
                    else if (i > 0)
                    {
                        DGPS.zone = lib.RecomputeZone(ListDGPS[i - 1], DGPS, null);
                    }
                }
                //if (save == true && ADSB.zone!=-1)
                //{
                //    AddMapMarker(ADSB, listMarkers);
                //}
            }
        }
    


        //public void ComputePDADSBInterpoled2(Data data)
        //{
        //    if (ListMLAT.Count > 9 && ListADSB.Count > 9)
        //    {
        //        ListADSB = ListADSB.OrderBy(x => x.Time_milisec).ToList();
        //        foreach (CAT10 MLAT in ListMLAT)
        //        {
        //            if (MLAT.TOT == "Aircraft")
        //            {
        //                aircraftMLAT = true;
        //                break;
        //            }
        //        }
        //        foreach (CAT21vs21 ADSB in ListADSB)
        //        {
        //            if (ADSB.ECAT == "Light aircraft" || ADSB.ECAT == "Small aircraft" || ADSB.ECAT == "Medium aircraft" || ADSB.ECAT == "Heavy aircraft")
        //            {
        //                aircraftADSB = true;
        //                break;
        //            }
        //        }
        //        int zone = 0;
        //        bool first = true;
        //        if (aircraftMLAT == true && aircraftADSB == true)
        //        {
        //            double startTime = 0;
        //            double EndTime = 0;
        //            for (int i = 0; i < ListADSB.Count; i++)
        //            {
        //                CAT21vs21 ADSB = ListADSB[i];
        //                if (ADSB.zone != zone && first == true)
        //                {
        //                    startTime = ADSB.Time_milisec;
        //                    first = false;
        //                    zone = ADSB.zone;
        //                }
        //                if (ADSB.zone != zone && first == false)
        //                {
        //                    EndTime = ADSB.Time_milisec;
        //                    ComputeZonePD(startTime, EndTime, zone, data);
        //                    ComputeZoneUP(startTime, EndTime, zone, data);
        //                    startTime = ADSB.Time_milisec;
        //                    zone = ADSB.zone;
        //                }
        //                if (i == ListADSB.Count - 1)
        //                {
        //                    EndTime = ADSB.Time_milisec;
        //                    ComputeZonePD(startTime, EndTime, zone, data);
        //                    ComputeZoneUP(startTime, EndTime, zone, data);

        //                }
        //            }
        //        }
        //    }
        //}

        public void ComputePDUD(Data data)
        {
            if (ListMLAT.Count() > 1)
            {
                if (ListDGPS.Count > 0) { aircraftMLAT = true; }

                if (!aircraftMLAT)
                {
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        if (MLAT.TOT == "Aircraft")
                        {
                            aircraftMLAT = true;
                            break;
                        }
                    }
                }
                if (!aircraftMLAT)
                {
                    foreach (CAT21vs21 ADSB in ListADSB)
                    {
                        if (ADSB.ECAT == "Light aircraft" || ADSB.ECAT == "Small aircraft" || ADSB.ECAT == "Medium aircraft" || ADSB.ECAT == "Heavy aircraft")
                        {
                            aircraftMLAT = true;
                            break;
                        }
                    }
                }
                if (aircraftMLAT)
                {
                    ListDGPS = ListDGPS.OrderBy(x => x.Time).ToList();
                    int zone = 0;
                    bool first = true;
                    double startTime = 0;
                    double EndTime = 0;
                    for (int i = 0; i < ListMLAT.Count; i++)
                    {
                        CAT10 MLAT = ListMLAT[i];
                        if (MLAT.zone != zone && first == true)
                        {
                            startTime = MLAT.Time_milisec;
                            first = false;
                            zone = MLAT.zone;
                        }
                        if (MLAT.zone != zone && first == false)
                        {
                            EndTime = MLAT.Time_milisec;
                            if (ListADSB.Count > 0 || ListDGPS.Count > 0)
                            {
                                ComputeZonePD(startTime, EndTime, zone, data);
                            }
                            ComputeZoneUP(startTime, EndTime, zone, data);

                            startTime = MLAT.Time_milisec;
                            zone = MLAT.zone;
                        }
                        if (i == ListMLAT.Count - 1)
                        {
                            EndTime = MLAT.Time_milisec;
                            if (ListADSB.Count > 0 || ListDGPS.Count > 0)
                            {
                                ComputeZonePD(startTime, EndTime, zone, data);
                            }
                            ComputeZoneUP(startTime, EndTime, zone, data);

                        }
                    }
                }
            }
        }

        class PossibleAdress
        {
            public string Name;
            public int Times = 1;

            public PossibleAdress(string name)
            {
                this.Name = name;
            }
        }


        public void ComputePI(Data data)
        {
            if (ListMLAT.Count() > 1)
            {
                if (ListDGPS.Count > 0) { aircraftMLAT = true; }

                if (!aircraftMLAT)
                {
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        if (MLAT.TOT == "Aircraft")
                        {
                            aircraftMLAT = true;
                            break;
                        }
                    }
                }
                if (!aircraftMLAT)
                {
                    foreach (CAT21vs21 ADSB in ListADSB)
                    {
                        if (ADSB.ECAT == "Light aircraft" || ADSB.ECAT == "Small aircraft" || ADSB.ECAT == "Medium aircraft" || ADSB.ECAT == "Heavy aircraft")
                        {
                            aircraftMLAT = true;
                            break;
                        }
                    }
                }
                if (aircraftMLAT)
                {
                    List<PossibleAdress> adresses = new List<PossibleAdress>();
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        PossibleAdress addres = adresses.Find(x => x.Name == MLAT.Target_Address);
                        if (addres!=null)
                        {
                            addres.Times++;
                        }
                        else
                        {
                            addres = new PossibleAdress(MLAT.Target_Address);
                            adresses.Add(addres);
                        }
                    }
                    PossibleAdress address = new PossibleAdress("");
                    foreach (PossibleAdress add in adresses)
                    {
                        if (add.Times>address.Times)
                        {
                            address = add;    
                        }
                    }
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        int zone = MLAT.zone;
                        if (zone != -1)
                        {
                            if (MLAT.Target_Address != address.Name)
                            {
                                data.ListZones[zone - 1].IncorrectPI++;
                            }
                            else
                            {
                                data.ListZones[zone - 1].CorrectPI++;
                            }
                        }
                    }
                }
                
            }
        }

        public void ComputePFI(Data data)
        {
            if (ListMLAT.Count() > 1)
            {
                if (ListDGPS.Count > 0) { aircraftMLAT = true; }

                if (!aircraftMLAT)
                {
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        if (MLAT.TOT == "Aircraft")
                        {
                            aircraftMLAT = true;
                            break;
                        }
                    }
                }
                if (!aircraftMLAT)
                {
                    foreach (CAT21vs21 ADSB in ListADSB)
                    {
                        if (ADSB.ECAT == "Light aircraft" || ADSB.ECAT == "Small aircraft" || ADSB.ECAT == "Medium aircraft" || ADSB.ECAT == "Heavy aircraft")
                        {
                            aircraftMLAT = true;
                            break;
                        }
                    }
                }
                if (aircraftMLAT)
                {
                    List<CAT10> ListMLATOrdered = ListMLAT.OrderBy(x => x.Time_milisec).ToList();
                    double time = ListMLATOrdered[0].Time_milisec;
                    double endTime = ListMLATOrdered[ListMLATOrdered.Count() - 1].Time_milisec;
                    int numberwindows = Convert.ToInt32((endTime - time) / 5);
                    int index = 0;
                    string TargetIdentification = this.TargetIdentification;
                    int same = 0;
                    int dif = 0;
                    //foreach(CAT10 MLAT in ListMLATOrdered)
                    //{
                    //    int zone = MLAT.zone;
                    //    if (zone != -1)
                    //    {
                    //        data.ListZones[zone - 1].CorrectIdentification++;
                    //    }
                    //}
                    for (int i = 1; i < numberwindows; i++)
                    {
                        int zone = ListMLATOrdered[index].zone;
                        bool Continue = true;
                        while (zone == -1 && Continue==true)
                        {
                            if (index < ListMLATOrdered.Count())
                            {
                                zone = ListMLATOrdered[index].zone;
                            }
                            index++;

                            if (index >= ListMLATOrdered.Count() - 1)
                            {
                                Continue= false;
                                break;
                                
                            }
                        }
                        if(Continue==false)
                        {
                            break;
                        }

                        double StartTime = ListMLATOrdered[index].Time_milisec;
                        List<CAT10> ThisWindowList = new List<CAT10>();
                        double endWindowTime = StartTime + 5;
                        if (index < ListMLATOrdered.Count() - 1)
                        {
                            while (ListMLATOrdered[index].Time_milisec < endWindowTime)
                            {
                                ThisWindowList.Add(ListMLATOrdered[index]);
                                index++;
                                if (index == ListMLATOrdered.Count() - 1)
                                {
                                    break;
                                }
                            }
                            bool Error = false;
                            if (ThisWindowList.Count() > 0)
                            {
                                Error = true;

                                foreach (CAT10 MLAT in ThisWindowList)
                                {


                                    if ((MLAT.Target_Address != null || MLAT.Target_Address!="") && MLAT.Target_Address==TargetAdress)
                                
                                    {
                                        Error = false;
                                        break;
                                    }
                                    // if (((MLAT.Target_Address != null || MLAT.Target_Address!="") && MLAT.Target_Address!=TargetAdress) && ((MLAT.Target_Identification != null || MLAT.Target_Identification != "") && MLAT.Target_Identification != TargetIdentification))
                                    //{
                                    //    double t = MLAT.Time_milisec;
                                    //    double mintime = t - 5;
                                    //    double maxtime = t + 5;
                                    //    Error = true;

                                    //    List<CAT10> newList = new List<CAT10>();
                                    //    foreach (CAT10 mlat in ListMLAT)
                                    //    {
                                    //        if (mlat.Time_milisec > mintime && mlat.Time_milisec < maxtime)
                                    //        {
                                    //            newList.Add(mlat);
                                    //        }
                                    //    }
                                    //    foreach (CAT10 mlat in newList)
                                    //    {
                                    //        if ((MLAT.Target_Address != null || MLAT.Target_Address != "") && MLAT.Target_Address == TargetAdress)
                                    //        {
                                    //            Error = false;
                                    //            break;
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                            if (Error == true)
                            {
                                data.ListZones[zone - 1].FalseIdentification++;
                            }
                            else
                            {
                                data.ListZones[zone - 1].CorrectIdentification++;
                            }

                        }
                    }

                }

            }
        }

        public void ComputePrecissionADSBinterpoled(Data data, int PIC)
        {
            if (ListMLAT.Count() > 1)
            {
                if (ListDGPS.Count > 0) { aircraftMLAT = true; }

                if (!aircraftMLAT)
                {
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        if (MLAT.TOT == "Aircraft")
                        {
                            aircraftMLAT = true;
                            break;
                        }
                    }
                }
                if (!aircraftMLAT)
                {
                    foreach (CAT21vs21 ADSB in ListADSB)
                    {
                        if (ADSB.ECAT == "Light aircraft" || ADSB.ECAT == "Small aircraft" || ADSB.ECAT == "Medium aircraft" || ADSB.ECAT == "Heavy aircraft")
                        {
                            aircraftMLAT = true;
                            break;
                        }
                    }
                }
                if (aircraftMLAT)
                {
                    if (ListMLAT.Count > 9 && ListADSB.Count > 9)
                    {
                        foreach (CAT10 MLAT in ListMLAT)
                        {
                            if (MLAT.zone != -1)
                            {
                                //double time = MLAT.Time_milisec;
                                CAT21vs21 Before = new CAT21vs21();
                                CAT21vs21 After = new CAT21vs21();
                                bool Correct = false;
                                bool cont = true;
                                bool BeforeFound = false;
                                bool AfterFound = false;
                                for (int i = 0; cont == true; i++)
                                {
                                    CAT21vs21 ADSB = ListADSB[i];
                                    if (ADSB.Time_milisec <= MLAT.Time_milisec && ADSB.Time_milisec > MLAT.Time_milisec - 5)
                                    {
                                        Before = ADSB;
                                        BeforeFound = true;
                                    }
                                    if (ADSB.Time_milisec >= MLAT.Time_milisec && ADSB.Time_milisec < MLAT.Time_milisec + 5 && AfterFound == false)
                                    {
                                        After = ADSB;
                                        AfterFound = true;
                                    }
                                    if (BeforeFound == true && AfterFound == true)
                                    {
                                        Correct = true;
                                        cont = false;
                                    }
                                    if (i == ListADSB.Count - 1)
                                    {
                                        cont = false;
                                    }
                                }
                                if (Correct == true && Before.PIC >= PIC && After.PIC >= PIC)
                                {

                                    ComputeMLATPrecission(MLAT, Before, After, data);
                                }
                            }
                        }
                        foreach (CAT21vs21 ADSB in ListADSB)
                        {
                            if (ADSB.used == true && ADSB.zone != -1)
                            {
                                data.ListZones[ADSB.zone - 1].ADSBMessagesUsed++;
                            }
                        }
                    }
                }
            }
        }
        

        
        public void ComputePrecissionMLATinterpoledDGPS(Data data)
        {

            if (ListMLAT.Count > 9 && ListDGPS.Count > 9)
            {
                foreach (CAT10 MLAT in ListMLAT)
                { 
                    if ((ComputeDistance(MLAT, ARPCoords) < 20000) && MLAT.zone!=-1)
                    {
                   
                        MarkerDGPS Before = new MarkerDGPS();
                        MarkerDGPS After = new MarkerDGPS();
                        bool Correct = false;
                        bool cont = true;
                        bool BeforeFound = false;
                        bool AfterFound = false;
                        for (int i = 0; cont == true; i++)
                        {
                           MarkerDGPS DGPS = ListDGPS[i];
                            if (DGPS.Time <= MLAT.Time_milisec && DGPS.Time > MLAT.Time_milisec - 5)
                            {
                                Before = DGPS;
                                BeforeFound = true;
                            }
                            if (DGPS.Time >= MLAT.Time_milisec && DGPS.Time < MLAT.Time_milisec + 5 && AfterFound == false)
                            {
                                After = DGPS;
                                AfterFound = true;
                            }
                            if (BeforeFound == true && AfterFound == true)
                            {
                                Correct = true;
                                cont = false;
                            }
                            if (i == ListDGPS.Count - 1)
                            {
                                cont = false;
                            }
                        }
                        if (Correct == true)
                        {
                            ComputeMLATPrecission(MLAT, Before, After, data);
                        }
                        else
                        {
                            int a = 0;
                        }
                    }
                }

                foreach (MarkerDGPS DGPS in ListDGPS)
                {

                    if (DGPS.used == true && DGPS.zone != -1)

                    {
                        data.ListZones[DGPS.zone - 1].DGPSMessagesUsed++;
                    }
                }

            }
        }
        

        private void ComputeMLATPrecission(CAT10 MLAT, CAT21vs21 Before, CAT21vs21 After, Data data)
        {
            if (MLAT.zone != -1)
            {
                PointWithHeight p = ComputePositionXY(Before, After, MLAT);
                Point MLATp = new Point(MLAT.X_Component_map, MLAT.Y_Component_map);
                double dist = ComputeDistanceXY(MLATp, p);
              //  PointLatLng p = ComputePosition(Before, After, MLAT);
                double difH = ComputeHeightDif(Before, After, MLAT);
                // double dist = ComputeDistance(MLAT, p);
                double PFDdist = 50;/////DISTANCE IN FUNCTION OF ZONE
                int zone = MLAT.zone;
                if (zone ==9|| zone == 10 || zone == 11)
                {
                    PFDdist = 80;
                }
                else if( zone==12 || zone == 13 || zone == 14)
                {
                    PFDdist = 160;
                }
                if (dist>PFDdist)
                {
                    data.ListZones[zone - 1].FalseDetection++;
                }
                else
                {
                    data.ListZones[zone - 1].CorrectDetection++;
                }
                if (dist < 500)
                {
                    

                    data.ListZones[zone - 1].MLATPrecision.Add(dist);
                    data.ListZones[zone - 1].MLATMessagesUsed++;
                    data.ListZones[zone - 1].NAC += After.NAC;
                    data.ListZones[zone - 1].NAC += Before.NAC;
                    data.ListZones[zone - 1].NACused += 2;
                    Before.used = true;
                    After.used = true;
                    MLAT.used = true;

                    double ErrorLocalX = MLAT.X_Component_map - p.X;
                    double ErrorLocalY = MLAT.Y_Component_map - p.Y;
                    PrecissionPoint pressP = new PrecissionPoint(TargetIdentification, MLAT.X_Component_map, MLAT.Y_Component_map, 53.321, p.X, p.Y, p.Z, ErrorLocalX, ErrorLocalY, dist, MLAT.zone, MLAT.GroundBit, MLAT.Time_milisec);
                    data.PrecissionPoints.Add(pressP);


                    if (difH != -999)
                    {
                        data.ListZones[zone - 1].totalMLATPrecissionheight.Add(difH);
                        data.ListZones[zone - 1].MLATMessagesUsedheight++;
                    }
                }
            }
        }

        private class PointWithHeight
        {
            public double X;
            public double Y;
            public double Z;
            public PointWithHeight(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }
        }

        private void ComputeMLATPrecission(CAT10 MLAT, MarkerDGPS Before, MarkerDGPS After, Data data)
        {
            if (MLAT.zone != -1)
            {
                PointWithHeight p = ComputePositionXY(Before, After, MLAT);
                Point MLATp = new Point(MLAT.X_Component_map, MLAT.Y_Component_map);
                double dist = ComputeDistanceXY(MLATp, p);
                double PFDdist = 50;
                int zone = MLAT.zone;
                if (zone == 9 || zone == 10 || zone == 11)
                {
                    PFDdist = 80;
                }
                else if (zone == 12 || zone == 13 || zone == 14)
                {
                    PFDdist = 160;
                }
                if (dist > PFDdist)
                {
                    data.ListZones[zone - 1].FalseDetection++;
                }
                else
                {
                    data.ListZones[zone - 1].CorrectDetection++;
                }

                if (dist < 500)
                {                 
                    data.ListZones[zone - 1].MLATPrecision.Add(dist);
                    data.ListZones[zone - 1].MLATMessagesUsed++;
                    Before.used = true;
                    After.used = true;
                    MLAT.used = true;
                    double ErrorLocalX = MLAT.X_Component_map - p.X;
                    double ErrorLocalY = MLAT.Y_Component_map - p.Y;
                    PrecissionPoint pressP = new PrecissionPoint(TargetIdentification, MLAT.X_Component_map, MLAT.Y_Component_map, 53.321, p.X, p.Y, p.Z, ErrorLocalX, ErrorLocalY, dist, MLAT.zone, MLAT.GroundBit,MLAT.Time_milisec);
                    data.PrecissionPoints.Add(pressP);

                }
            }
        }


        private double ComputeHeightDif(CAT21vs21 Before, CAT21vs21 After, CAT10 MLAT)
        {
            double difFL=-999;
            double difGH=-999;
            double timeBefore_After = After.Time_milisec - Before.Time_milisec;
            double Vtime = (MLAT.Time_milisec - Before.Time_milisec);
            double moveddist;
            if (Before.Flight_Level!=-999 && After.Flight_Level!=999 && MLAT.Flight_Level!=999)
            {
                double dist = (After.Flight_Level - Before.Flight_Level);
                double vel = dist / timeBefore_After;
                moveddist = vel * Vtime;
                double height = Before.Flight_Level + moveddist;
                difFL = Math.Abs(MLAT.Flight_Level - height);
            }
            if (Before.Geometric_Height!=-999 && After.Geometric_Height!=-999 && MLAT.Measured_Height!=-999)
            {
                double dist = (After.Geometric_Height - Before.Geometric_Height);
                double vel = dist / timeBefore_After;
                moveddist = vel * Vtime;
                double height = Before.Geometric_Height + moveddist;
                difGH = Math.Abs(MLAT.Measured_Height - height);
            }
            if(difGH!=-999 && difFL!=-999)
            {
                double dif=(difFL+difGH)/ 2;
                return dif;
            }
            else if (difGH!=-999)
            {
                return difGH;
            }
            else if (difFL!=-999)
            {
                return difFL;
            }
            else
            {
                return -999;
            }

        }


        private PointWithHeight ComputePositionXY(CAT21vs21 Before, CAT21vs21 After, CAT10 MLAT)
        {
            double X0 = Before.X_Component_map;
            double Y0 = Before.Y_Component_map;
            double X1 = After.X_Component_map;
            double Y1 = After.Y_Component_map;
            double X = X1 - X0;
            double Y = Y1 - Y0;
            double dist = Math.Sqrt(Math.Pow((X1 - X0), 2) + Math.Pow((Y1 - Y0), 2));
            double moveddist;
            double Vtime = (MLAT.Time_milisec - Before.Time_milisec);
            double timeBefore_After = After.Time_milisec - Before.Time_milisec;
            if (Before.Ground_Speed != -1 && After.Ground_Speed != -1)
            {
                double acceleration = (Before.Ground_Speed - After.Ground_Speed) / timeBefore_After;
                moveddist = (Before.Ground_Speed * Vtime) + ((acceleration * (Math.Pow(Vtime, 2))) / (2));
            }
            else
            {
                double vel = dist / timeBefore_After;
                moveddist = vel * Vtime;
            }
            double dir = Math.Atan2(Y, X);
            double Xfin = X0;
            double Yfin = Y0;
            if (moveddist != 0 && double.IsNaN(moveddist) == false)
            {
                Xfin = X0 + moveddist * Math.Cos(dir);
                Yfin = Y0 + moveddist * Math.Sin(dir);
            }
            double Height = 0;
            if (Before.Geometric_Height != -999 && After.Geometric_Height != -999)
            {
                double verticalSpeed = (After.Geometric_Height  - Before.Geometric_Height) / timeBefore_After;
                Height = Before.Geometric_Height + (verticalSpeed * Vtime);
            }
            PointWithHeight p = new PointWithHeight(Xfin, Yfin,Height);
            return p;
        }

        private PointWithHeight ComputePositionXY(MarkerDGPS Before, MarkerDGPS After, CAT10 MLAT)
        {
            double X0 = Before.Pxy.X;
            double Y0 = Before.Pxy.Y;
            double X1 = After.Pxy.X;
            double Y1 = After.Pxy.Y;
            double X = X1 - X0;
            double Y = Y1 - Y0;
            double dist = Math.Sqrt(Math.Pow((X1 - X0), 2) + Math.Pow((Y1 - Y0), 2));
            double moveddist;
            double Vtime = (MLAT.Time_milisec - Before.Time);
            double timeBefore_After = After.Time - Before.Time;
            double vel = dist / timeBefore_After;
            moveddist = vel * Vtime;
            double dir = Math.Atan2(Y, X);
            double Xfin = X0;
            double Yfin = Y0;
            if (moveddist!=0 && double.IsNaN(moveddist)==false)
            {
                Xfin = X0 + moveddist * Math.Cos(dir);
                Yfin = Y0 + moveddist * Math.Sin(dir);
            }
            double Height = 0;
            if( Before.Height!=0 && After.Height!=0)
            {
                double verticalSpeed = (After.Height - Before.Height)/timeBefore_After;
                Height=Before.Height+(verticalSpeed*Vtime);

            }
            PointWithHeight p = new PointWithHeight(Xfin, Yfin,Height);
            return p;
        }

        //private PointLatLng ComputePosition(CAT21vs21 Before, CAT21vs21 After, CAT10 MLAT)
        //{


        //    PointLatLng p = new PointLatLng();
        //    double lat = Before.LatitudeWGS_84_map;
        //    double lon = Before.LongitudeWGS_84_map;
        //    double lat2 = After.LatitudeWGS_84_map;
        //    double lon2 = After.LongitudeWGS_84_map;
        //    double dist = 6371000 * (Math.Acos(Math.Cos((Math.PI / 180) * (90 - lat)) * Math.Cos((Math.PI / 180) * (90 - lat2)) + Math.Sin((Math.PI / 180) * (90 - lat)) * Math.Sin((Math.PI / 180) * (90 - lat2)) * Math.Cos((Math.PI / 180) * (lon - lon2))));
        //    double moveddist;
        //    double Vtime = (MLAT.Time_milisec - Before.Time_milisec);
        //    double timeBefore_After = After.Time_milisec - Before.Time_milisec;
        //    if (Before.Ground_Speed != -1 && After.Ground_Speed != -1)
        //    {
        //        double acceleration = (Before.Ground_Speed - After.Ground_Speed) / timeBefore_After;
        //        moveddist = (Before.Ground_Speed * Vtime) + ((acceleration * (Math.Pow(Vtime, 2))) / (2));
        //    }
        //    else
        //    {
        //        double vel = dist / timeBefore_After;
        //        moveddist = vel * Vtime;
        //    }
        //    double X0 = lon - lon2;
        //    double Y0 = lat - lat2;
        //    double dir = Math.Atan2(Y0, X0);
        //    double X = moveddist * Math.Cos(dir);
        //    double Y = moveddist * Math.Sin(dir);
        //    double R = 6371 * 1000;
        //    double d = Math.Sqrt((X * X) + (Y * Y));
        //    double brng = Math.Atan2(Y, -X) - (Math.PI / 2);          
        //    double Lat1 = Before.LatitudeWGS_84_map * (Math.PI / 180);
        //    double Lon1 = Before.LongitudeWGS_84_map * (Math.PI / 180);
        //    var Lat2 = Math.Asin(Math.Sin(Lat1) * Math.Cos(d / R) + Math.Cos(Lat1) * Math.Sin(d / R) * Math.Cos(brng));
        //    var Lon2 = Lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(Lat1), Math.Cos(d / R) - Math.Sin(Lat1) * Math.Sin(Lat2));
        //    p.Lat = Lat2 * (180 / Math.PI);
        //    p.Lng = Lon2 * (180 / Math.PI);
        //    return p;
        //}


        //private PointLatLng ComputePosition(CAT10 Before, CAT10 After, CAT21vs21 ADSB)
        //{
        //    PointLatLng p = new PointLatLng();
        //    double lat = Before.LatitudeWGS_84_map;
        //    double lon = Before.LongitudeWGS_84_map;
        //    double lat2 = After.LatitudeWGS_84_map;
        //    double lon2 = After.LongitudeWGS_84_map;
        //    double dist = 6371000 * (Math.Acos(Math.Cos((Math.PI / 180) * (90 - lat)) * Math.Cos((Math.PI / 180) * (90 - lat2)) + Math.Sin((Math.PI / 180) * (90 - lat)) * Math.Sin((Math.PI / 180) * (90 - lat2)) * Math.Cos((Math.PI / 180) * (lon - lon2))));
        //    double moveddist;
        //    double Vtime = (ADSB.Time_milisec- Before.Time_milisec);
        //    double timeBefore_After = After.Time_milisec - Before.Time_milisec;
        //    //  bool accelerationComputed = false;
        //    if (Before.Vx != -1 && Before.Vy != -1 && After.Vy != -1 && After.Vx != -1)
        //    {
        //        double Vbefore = Math.Sqrt(Math.Pow(Before.Vx, 2) + Math.Pow(Before.Vy, 2));
        //        double Vafter = Math.Sqrt(Math.Pow(After.Vx, 2) + Math.Pow(After.Vy, 2));
        //        double acceleration = (Vbefore - Vafter) / timeBefore_After;
        //        moveddist = (Vbefore * Vtime) + ((acceleration * (Math.Pow(Vtime, 2))) / (2));
        //    }
        //    else
        //    if (Before.Ground_Speed != -1 && After.Ground_Speed != -1)
        //    {
        //        double acceleration = (Before.Ground_Speed - After.Ground_Speed) / timeBefore_After;
        //        moveddist = (Before.Ground_Speed * Vtime) + ((acceleration * (Math.Pow(Vtime, 2))) / (2));
        //    }
        //    else
        //    {
        //        double vel = dist / timeBefore_After;
        //        moveddist = vel * Vtime;
        //    }
        //    double X0 = lon - lon2;
        //    double Y0 = lat - lat2;
        //    double dir = ((Math.Atan2(Y0, X0) * (180 / Math.PI)) - 90);
        //    double X = moveddist * Math.Sin(dir);
        //    double Y = moveddist * Math.Cos(dir);
        //    double R = 6371 * 1000;
        //    double d = Math.Sqrt((X * X) + (Y * Y));
        //    double brng = Math.Atan2(Y, -X) - (Math.PI / 2);
        //    //double Lat1 = 0;
        //    //double Lon1 = 0;          
        //    double Lat1 = Before.LatitudeWGS_84_map * (Math.PI / 180);
        //    double Lon1 = Before.LongitudeWGS_84_map * (Math.PI / 180);
        //    var Lat2 = Math.Asin(Math.Sin(Lat1) * Math.Cos(d / R) + Math.Cos(Lat1) * Math.Sin(d / R) * Math.Cos(brng));
        //    var Lon2 = Lon1 + Math.Atan2(Math.Sin(brng) * Math.Sin(d / R) * Math.Cos(Lat1), Math.Cos(d / R) - Math.Sin(Lat1) * Math.Sin(Lat2));
        //    p.Lat = Lat2 * (180 / Math.PI);
        //    p.Lng = Lon2 * (180 / Math.PI);
        //    return p;
        //}



        private double ComputeDistanceXY(Point MLAT, PointWithHeight ADSB)
        {
            double X = MLAT.X - ADSB.X;
            double Y = MLAT.Y - ADSB.Y;
            double dist = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            return dist;
        }

        private double ComputeDistance(CAT10 MLAT, PointLatLng p)
        {
            double lat = MLAT.LatitudeWGS_84_map;
            double lon = MLAT.LongitudeWGS_84_map;
            double lat2 = p.Lat;
            double lon2 = p.Lng;
            double dist = 6371000 * (Math.Acos(Math.Cos((Math.PI / 180) * (90 - lat)) * Math.Cos((Math.PI / 180) * (90 - lat2)) + Math.Sin((Math.PI / 180) * (90 - lat)) * Math.Sin((Math.PI / 180) * (90 - lat2)) * Math.Cos((Math.PI / 180) * (lon - lon2))));
            return dist;
        }

        //private double ComputeDistance(CAT21vs21 ADSB, PointLatLng p)
        //{
        //    double lat = ADSB.LatitudeWGS_84_map;
        //    double lon = ADSB.LongitudeWGS_84_map;
        //    double lat2 = p.Lat;
        //    double lon2 = p.Lng;
        //    double dist = 6371000 * (Math.Acos(Math.Cos((Math.PI / 180) * (90 - lat)) * Math.Cos((Math.PI / 180) * (90 - lat2)) + Math.Sin((Math.PI / 180) * (90 - lat)) * Math.Sin((Math.PI / 180) * (90 - lat2)) * Math.Cos((Math.PI / 180) * (lon - lon2))));
        //    return dist;
        //}

    }
}
