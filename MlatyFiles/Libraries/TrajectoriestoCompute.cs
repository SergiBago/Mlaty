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
    public class TrajectoriesToCompute
    {
        public List<CAT10> ListMLAT = new List<CAT10>();
        public List<CAT21vs21> ListADSB = new List<CAT21vs21>();
        public List<MarkerDGPS> ListDGPS = new List<MarkerDGPS>();
        public string TargetIdentification;
        public string TargetAdress;
        public int TrackNumberADSB=-1;
        public int TrackNumberMLAT = -1;
        public bool aircraftMLAT = false;
        public bool aircraftADSB = false;

        public string PD;
        public PointLatLng ARPCoords = new PointLatLng(41.2970767, 2.07846278);

        public List<string> FixedMLATS = new List<string>() { "342384", "342387", "342386", "342385", "342383", "3433D5" };

        public TrajectoriesToCompute(CAT10 message)
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
                this.TrackNumberMLAT = message.Track_Number;
            }

        }

        public TrajectoriesToCompute(CAT21vs21 message)
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
                this.TrackNumberADSB = message.Track_Number;
            }
        }

        public void Sort()
        {
            ListADSB = ListADSB.OrderBy(x => x.Time_milisec).ToList();
            ListMLAT = ListMLAT.OrderBy(x => x.Time_milisec).ToList();
            ListDGPS = ListDGPS.OrderBy(x => x.Time).ToList();

        }
        public TrajectoriesToCompute()
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
            if(TrackNumberADSB==-1)
            {
                TrackNumberADSB = message.Track_Number;
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
            if (TrackNumberMLAT == -1)
            {
                TrackNumberMLAT = message.Track_Number;
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
                if (zone == 1 || zone == 2 || zone == 3 || zone == 6 || zone == 7 || zone == 8) { refreshrate = 2; } //Set refresh rate for zones 
                if (zone == 4 || zone == 5) { refreshrate = 5; }
                if (zone == 9 || zone == 10 || zone == 11 || zone == 12 || zone == 13 || zone == 14) { refreshrate = 1; }
                int expected = 0;
                List<CAT10> ListPDMLAT = ListMLAT.Where(x => x.Time_milisec >= startTime - refreshrate && x.Time_milisec <= endTime + refreshrate).ToList(); //Create a list with all messages that are in the actual computing period
          
                List<double> times = new List<double>(); //If a vehicle stops transmiting for more than 30 seconds we assume it stoped the transmitter, so we dount count all that stops as missed MLATS, 
                                                         // instead we create multiple lists and compute the PD for all the times the vehicle is active. 
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

                if (times.Count == 0) //If there are no stops we compute all in once
                {
                    for (int i = Convert.ToInt32(startTime); i < Convert.ToInt32(endTime); i ++)
                    {
                        double mintime = (i - (refreshrate / 2));
                        double maxtime = (i + (refreshrate / 2));
                        bool found = false;
                        foreach (CAT10 MLAT in ListMLAT)
                        {
                            if (MLAT.Time_milisec >= mintime && MLAT.Time_milisec <= maxtime)
                            {
                                    found = true;
                                    MLAT.used = true;
                            }
                        }
                        if (found == false)
                        {
                            data.ListZones[zone - 1].MissedMLATSPD++;
                        }
                    }
                    expected = Convert.ToInt32((endTime - startTime));
                }
                else //if there are stops we compute the PD for each time period
                {
                    for (int s = 0; s < times.Count; s += 2)
                    {
                        startTime = times[s];
                        endTime = times[s + 1];
                        for (int i = Convert.ToInt32(startTime); i < Convert.ToInt32(endTime); i++)
                        {
                            double mintime = (i - (refreshrate / 2));
                            double maxtime = (i + (refreshrate / 2));
                            bool found = false;
                            foreach (CAT10 MLAT in ListMLAT)
                            {
                                if (MLAT.Time_milisec >= mintime && MLAT.Time_milisec <= maxtime)
                                {
                                        found = true;
                                        MLAT.used = true;
                                }
                            }
                            if (found == false)
                            {
                                data.ListZones[zone - 1].MissedMLATSPD++;
                            }
                        }
                        expected += Convert.ToInt32((endTime - startTime));

                        }
                }
                data.ListZones[zone-1].ExpectedMessagesPD += expected;
            }
        }


        private List<CAT21vs21> FindPreviousAndNextADSB(CAT10 MLAT, int PIC)
        {
            if (ListADSB.Count > 0)
            {
                CAT21vs21 Before = new CAT21vs21();
                CAT21vs21 After = new CAT21vs21();
                bool Correct = false;
                bool BeforeFound = false;
                bool AfterFound = false;
                for (int i = 0; true; i++)
                {
                    CAT21vs21 ADSB = ListADSB[i];
                    //if (ADSB.Time_milisec <= MLAT.Time_milisec )
                    //{
                    //    Before = ADSB;
                    //    BeforeFound = true;
                    //}
                    //if (ADSB.Time_milisec >= MLAT.Time_milisec && AfterFound == false)
                    //{
                    //    After = ADSB;
                    //    AfterFound = true;
                    //}
                    if (ADSB.Time_milisec <= MLAT.Time_milisec && ADSB.Time_milisec > MLAT.Time_milisec - 5 && ADSB.PIC >= PIC)
                    {
                        Before = ADSB;
                        BeforeFound = true;
                    }
                    if (ADSB.Time_milisec >= MLAT.Time_milisec && ADSB.Time_milisec < MLAT.Time_milisec + 5 && ADSB.PIC >= PIC && AfterFound == false)
                    {
                        After = ADSB;
                        AfterFound = true;
                    }
                    if (BeforeFound == true && AfterFound == true)
                    {
                        return new List<CAT21vs21> { Before, After };
                      //  Correct = true;
                        //break;
                    }
                    if (i == ListADSB.Count - 1)
                    {
                        return new List<CAT21vs21> { };
                      //  break;
                    }
                }
                //if (Correct == true)
                //{
                //    Before.used = true;
                //    After.used = true;
                //    p = ComputePositionXY(Before, After, MLAT);
                //}
            }
            return new List<CAT21vs21> {};
        }

        private List<MarkerDGPS> FindPreviousAndNextDGPS(CAT10 MLAT)
        {
            if (ListDGPS.Count > 0)
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
                        return new List<MarkerDGPS> { Before, After };
                    }
                    if (i == ListDGPS.Count - 1)
                    {
                        return new List<MarkerDGPS> { };

                    }
                }

            }
            return new List<MarkerDGPS> { };
        }

        private PointWithHeight ComputePointInMlatTIme(CAT10 MLAT, CAT21vs21 Before, CAT21vs21 After)
        {
            PointWithHeight p = null;
            Before.used = true;
            After.used = true;
            p = ComputePositionXY(Before, After, MLAT);
              
            return p;
        }


        private PointWithHeight ComputePointInMlatTIme(CAT10 MLAT, MarkerDGPS Before, MarkerDGPS After)
        {
            PointWithHeight p = null;     
            Before.used = true;
            After.used = true;
            p = ComputePositionXY(Before, After, MLAT);
            return p;
        }



        private void ComputeZoneUP(double startTime, double endTime, int zone, Data data)
        {
            if (zone != -1)
            {
                double refreshrate = 1;
                int expected = 0;
                int refrat = Convert.ToInt32(refreshrate);
                List<CAT10> ListUPMLAT = ListMLAT.Where(x=>x.Time_milisec>=startTime-refreshrate && x.Time_milisec<=endTime+refreshrate).ToList();
                data.ListZones[zone].expected_PDok++;
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
                    double time = (startTime);// + (refreshrate / 2));
                    int first = ListUPMLAT.FindIndex(x => x.Time_milisec == startTime); //Find first message
           //         double mintime = (startTime - (refreshrate / 2));
             //      double maxtime = (startTime + (refreshrate / 2));
                    for (int i = first; i < ListUPMLAT.Count(); i++) //Iterate for all Messages in 
                    {
                        CAT10 MLAT = ListUPMLAT[i];

                        if (MLAT.Time_milisec >= time-0.3 && MLAT.Time_milisec <= time + 0.3)
                        {
                            MLAT.used = true;
                            time = time + refreshrate;
                        }
                        else
                        {
                            if (MLAT.Time_milisec > time + refreshrate)
                            {
                                int miss = Convert.ToInt32(MLAT.Time_milisec - time);
                                data.ListZones[zone - 1].MissedMLATSUP+=miss;
                                time = time + refreshrate;
                                i = i - 1;
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
                        double time = (startTime);// + (refreshrate / 2));
                        int first = ListUPMLAT.FindIndex(x => x.Time_milisec == startTime); //Find first message
                                                                                            //         double mintime = (startTime - (refreshrate / 2));
                                                                                            //      double maxtime = (startTime + (refreshrate / 2));
                        for (int i = first; i < ListUPMLAT.Count(); i++) //Iterate for all Messages in 
                        {
                            CAT10 MLAT = ListUPMLAT[i];

                            if (MLAT.Time_milisec >= time - 0.3 && MLAT.Time_milisec <= time + 0.3)
                            {
                                MLAT.used = true;
                                time = time + refreshrate;
                            }
                            else
                            {
                                if (MLAT.Time_milisec > time + refreshrate)
                                {
                                    int miss = Convert.ToInt32(MLAT.Time_milisec - time);
                                    data.ListZones[zone - 1].MissedMLATSUP += miss;
                                    time = time + refreshrate;
                                    i = i - 1;
                                }

                            }
                            if (time >= endTime)
                            {
                                break;
                            }
                        }
                        //  data.ListZones[zone - 1].ExpectedMessagesUP += Convert.ToInt32((endTime - startTime) / refreshrate);
                        expected += Convert.ToInt32((endTime - startTime) / refreshrate);
                        //    double time = (startTime + (refreshrate / 2));
                        //    int first = ListUPMLAT.FindIndex(x=>x.Time_milisec==startTime);
                        //    double mintime = (startTime - (refreshrate / 2));
                        //    double maxtime = (startTime + (refreshrate / 2));
                        //    for (int i = first; i < ListUPMLAT.Count(); i++)
                        //    {
                        //        CAT10 MLAT = ListUPMLAT[i];

                        //        if (MLAT.Time_milisec >= time - 0.3 && MLAT.Time_milisec <= time + 0.3)
                        //        {
                        //            MLAT.used = true;
                        //            time = time + refreshrate;
                        //        }
                        //        else
                        //        {
                        //            if (MLAT.Time_milisec > time + refreshrate)
                        //            {
                        //                int miss = Convert.ToInt32(MLAT.Time_milisec - time);
                        //                data.ListZones[zone - 1].MissedMLATSUP += miss;
                        //                time = time + refreshrate;
                        //                i = i - 1;
                        //            }
                        //        }
                        //        if (time >= endTime)
                        //        {
                        //            break;
                        //        }
                        //    }
                        //}
                        data.ListZones[zone - 1].ExpectedMessagesUP += expected;
                    }

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


        public void SetZones()
        {
            for (int i = 0; i < ListMLAT.Count; i++)
            {
                CAT10 MLAT = ListMLAT[i];
                if (MLAT.Target_Address == "342384" || MLAT.Target_Address == "342387" || MLAT.Target_Address == "3433D5")
                {
                    MLAT.zone = 5;
                }
                else if (MLAT.Target_Address == "342385" || MLAT.Target_Address == "342386" || MLAT.Target_Address == "342383")
                {
                    MLAT.zone = 4;
                }
                else
                {

                    Point p = new Point(MLAT.X_Component_map, MLAT.Y_Component_map);
                    MLAT.zone = LibreriaDecodificacion.ComputeZone(p, MLAT.GroundBit);

                }

            }
                for (int i = 0; i < ListADSB.Count; i++)
                {
                    CAT21vs21 ADSB = ListADSB[i];
                 

                        Point p = new Point(ADSB.X_Component_map, ADSB.Y_Component_map);
                ADSB.zone = LibreriaDecodificacion.ComputeZone(p, ADSB.GroundBit);

                }

            for (int i = 0; i < ListDGPS.Count; i++)
            {
                MarkerDGPS DGPS = ListDGPS[i];


                Point p = new Point(DGPS.Pxy.X, DGPS.Pxy.Y);
                DGPS.zone = LibreriaDecodificacion.ComputeZone(p,2);

            }

            for (int i = 0; i < ListMLAT.Count; i++)
            {
                CAT10 MLAT = ListMLAT[i];
                if (MLAT.Target_Address == "342384" || MLAT.Target_Address == "342387" || MLAT.Target_Address == "3433D5")
                {
                    MLAT.zone = 5;
                }
                else if (MLAT.Target_Address == "342385" || MLAT.Target_Address == "342386" || MLAT.Target_Address == "342383")
                {
                    MLAT.zone = 4;
                }
                else
                {

                    Point p = new Point(MLAT.X_Component_map, MLAT.Y_Component_map);
                    MLAT.zone = LibreriaDecodificacion.ComputeZone(p, MLAT.GroundBit);

                }
            }

        }
    

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
                    ListDGPS = ListDGPS.OrderBy(x => x.Time).ToList(); //Order missages by time parameter
                    int zone = 0;
                    double startTime = 0;
                    double EndTime = 0;
                    startTime = ListMLAT[0].Time_milisec;
                    zone = ListMLAT[0].zone;
                
                    foreach(CAT10 MLAT in ListMLAT) //Iterate for all messages. 
                    {
                        if ((MLAT.zone != zone)|| (MLAT == ListMLAT[ListMLAT.Count - 1])) //if we change zone or is the last message, we close the zone and compute PD and UP
                        {
                            EndTime = MLAT.Time_milisec;
                            if (ListADSB.Count > 0 || ListDGPS.Count > 0)
                            {
                                ComputeZonePD(startTime, EndTime, zone, data);

                                ComputeZoneUP(startTime, EndTime, zone, data);
                            }
                            startTime = MLAT.Time_milisec;
                            zone = MLAT.zone;
                        }
                    
                    }
                }
            }
        }

        //class PossibleAdress
        //{
        //    public string Name;
        //    public int Times = 1;

        //    public PossibleAdress(string name)
        //    {
        //        this.Name = name;
        //    }
        //}


        public void ComputePI(Data data)
        {
            if (ListMLAT.Count() > 1)
            {
                if (ListDGPS.Count > 0) { aircraftMLAT = true; }
                if (FixedMLATS.Contains(TargetAdress)) { aircraftMLAT = true; }

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
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        int zone = MLAT.zone;
                        if (zone != -1)
                        {
                            if (MLAT.Target_Address != this.TargetAdress)
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
                if (FixedMLATS.Contains(TargetAdress)) { aircraftMLAT = true; }

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


        public int ComputePrecissionADSBinterpoled(Data data, int PIC)
        {
            int messagesDiscarted = 0;
            if (ListMLAT.Count() > 0)
            {
                if (FixedMLATS.Contains(TargetAdress))
                {
                    
                    List<CAT10> MlatsInStandsForAccuracy = new List<CAT10>();
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        if (MLAT.zone == 4 || MLAT.zone == 5)
                        {
                            MlatsInStandsForAccuracy.Add(MLAT);
                        }
                        else
                        {
                            ComputeMLATPrecissionFixedTransp(MLAT, data);
                        }
                    }
                    if (MlatsInStandsForAccuracy.Count() > 1)
                    {
                        ComputeMLATprecisionInStandsADSBFixed(MlatsInStandsForAccuracy, data);
                    }              
                }
                else
                {

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
                            List<CAT10> MlatsInStandsForAccuracy = new List<CAT10>();

                            foreach (CAT10 MLAT in ListMLAT)
                            {

                                if (MLAT.zone != -1)
                                {
                                    if (MLAT.zone == 4 || MLAT.zone == 5)
                                    {
                                        MlatsInStandsForAccuracy.Add(MLAT);
                                    }
                                    else
                                    {
                                        CAT21vs21 Before = new CAT21vs21();
                                        CAT21vs21 After = new CAT21vs21();
                                        bool Correct = false;
                                        bool cont = true;
                                        bool BeforeFound = false;
                                        bool AfterFound = false;
                                        //double PreviousPointTimeDifference;
                                        //double NextPointTimeDifference;

                                        for (int i = 0; cont == true; i++)
                                        {
                                            CAT21vs21 ADSB = ListADSB[i];
                                            if (ADSB.Time_milisec <= MLAT.Time_milisec && ADSB.Time_milisec > MLAT.Time_milisec - 5 && ADSB.PIC >= PIC)
                                            {
                                                Before = ADSB;
                                                BeforeFound = true;
                                            }
                                            if (ADSB.Time_milisec >= MLAT.Time_milisec && ADSB.Time_milisec < MLAT.Time_milisec + 5 && ADSB.PIC >= PIC && AfterFound == false)
                                            {
                                                After = ADSB;
                                                AfterFound = true;
                                            }
                                            //if (ADSB.Time_milisec <= MLAT.Time_milisec && ADSB.PIC >= PIC)
                                            //{
                                            //    Before = ADSB;
                                            //    BeforeFound = true;
                                            //}
                                            //if (ADSB.Time_milisec >= MLAT.Time_milisec &&  ADSB.PIC >= PIC && AfterFound == false)
                                            //{
                                            //    After = ADSB;
                                            //    AfterFound = true;
                                            //}
                                            //if (ADSB.Time_milisec <= MLAT.Time_milisec )//&& ADSB.PIC >= PIC)
                                            //{
                                            //    Before = ADSB;
                                            //    BeforeFound = true;
                                            //}
                                            //if (ADSB.Time_milisec >= MLAT.Time_milisec && AfterFound == false)
                                            //{
                                            //    After = ADSB;
                                            //    AfterFound = true;
                                            //}
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
                                        if (Correct == true)
                                        {
                                            ComputeMLATPrecission(MLAT, Before, After, data);
                                        }
                                        else
                                        {
                                            messagesDiscarted++;
                                        }
                                    }
                                }
                            }
                            if (MlatsInStandsForAccuracy.Count() > 1)
                            {
                                ComputeMLATprecisionInStandsADSB(MlatsInStandsForAccuracy, data, PIC);
                            }
                            foreach (CAT21vs21 ADSB in ListADSB)
                            {
                                if (ADSB.used == true && ADSB.zone!=-1)
                                {
                                    data.ListZones[ADSB.zone - 1].ADSBMessagesUsed++;
                                }
                            }
                        }
                    }
                }
            }
            return messagesDiscarted;
        }

        private class ListOfMLATSin5Seconds
        {
            public double StartTime;
            public double EndTime;
            public int zone;
            public List<CAT10> MLATSList = new List<CAT10>();
            public List<double> Distances = new List<double>();


            public ListOfMLATSin5Seconds(double StartTime,int zone,CAT10 MLAT)
            {
                this.StartTime = StartTime;
                this.EndTime = (StartTime+5);
                this.zone = zone;
                MLATSList.Add(MLAT);
            }
        }


        private void ComputeMLATprecisionInStandsADSBFixed(List<CAT10> MlatsInStandsForAccuracy, Data data)
        {
            MlatsInStandsForAccuracy.OrderBy(CAT10 => CAT10.Time_milisec); //Order messages lists by message time
            double startTime = MlatsInStandsForAccuracy[0].Time_milisec; //Set start time in first message time( first time of all)
            List<ListOfMLATSin5Seconds> ListOrderedMlats = new List<ListOfMLATSin5Seconds>(); //Create a list of Messages in 5 seconds
            foreach (CAT10 Mlat in MlatsInStandsForAccuracy) //Set all messages in List of 5 seconds
            {
                ListOfMLATSin5Seconds list = ListOrderedMlats.Find(x => (x.StartTime <= Mlat.Time_milisec && x.EndTime > Mlat.Time_milisec && x.zone == Mlat.zone));
                if (list != null)
                {
                    list.MLATSList.Add(Mlat);
                }
                else
                {
                    list = new ListOfMLATSin5Seconds(Mlat.Time_milisec, Mlat.zone, Mlat);
                    ListOrderedMlats.Add(list);
                }
            }
            foreach (ListOfMLATSin5Seconds List in ListOrderedMlats) //Foreach period of 5 seconds compute it's data
            {
                foreach (CAT10 Mlat in List.MLATSList) //foreach message in the list compute it's precission
                {
                    Point p = LibreriaDecodificacion.GetFixedMLATPos(TargetAdress); 
                    if (p != null) 
                    {
                        Point MLATp = new Point(Mlat.X_Component_map, Mlat.Y_Component_map);
                        double dist = ComputeDistanceXY(MLATp, p);
                        List.Distances.Add(dist); //Add precission to the ListOFMLATSin5Seconds to compute later the precission in that time
                        
                        double ErrorLocalX = Mlat.X_Component_map - p.X;
                        double ErrorLocalY = Mlat.Y_Component_map - p.Y;
                            data.ListZones[List.zone - 1].MLATMessagesUsed ++;

                        PrecissionPoint pressP = new PrecissionPoint(TargetIdentification, TargetAdress, TrackNumberMLAT, Mlat.X_Component_map, Mlat.Y_Component_map, 53.321, p.X, p.Y, 0, ErrorLocalX, ErrorLocalY, dist, Mlat.zone, Mlat.GroundBit, Mlat.Time_milisec,-1,-1);
                        data.PrecissionPoints.Add(pressP); //Create and save the point data
                    }
                }
            }
            foreach (ListOfMLATSin5Seconds List in ListOrderedMlats)
            {
                ComputeTotalListOfMLATSin5Seconds(List, data); //Compute interval list precision in another function
            }
        }

        private void ComputeMLATprecisionInStandsADSB(List<CAT10> MlatsInStandsForAccuracy, Data data,int PIC)
        {
            MlatsInStandsForAccuracy.OrderBy(CAT10 => CAT10.Time_milisec);
            double startTime = MlatsInStandsForAccuracy[0].Time_milisec;
            List<ListOfMLATSin5Seconds> ListOrderedMlats = new List<ListOfMLATSin5Seconds>();
            foreach (CAT10 Mlat in MlatsInStandsForAccuracy)
            {
                ListOfMLATSin5Seconds list = ListOrderedMlats.Find(x => (x.StartTime <= Mlat.Time_milisec && x.EndTime > Mlat.Time_milisec && x.zone == Mlat.zone));
                if(list!=null)
                {
                    list.MLATSList.Add(Mlat);
                }
                else
                {
                    list = new ListOfMLATSin5Seconds(Mlat.Time_milisec, Mlat.zone, Mlat);
                    ListOrderedMlats.Add(list);
                }
            }
            foreach (ListOfMLATSin5Seconds List in ListOrderedMlats)
            {
                foreach(CAT10 Mlat in List.MLATSList)
                {
                    List<CAT21vs21> ADBSList = FindPreviousAndNextADSB(Mlat, PIC);
                    if (ADBSList.Count() == 2)
                    {
                        PointWithHeight p = ComputePointInMlatTIme(Mlat, ADBSList[0], ADBSList[1]);
                        if (p != null)
                        {
                            Point MLATp = new Point(Mlat.X_Component_map, Mlat.Y_Component_map);
                            double dist = ComputeDistanceXY(MLATp, p);
                            List.Distances.Add(dist);
                            double ErrorLocalX = Mlat.X_Component_map - p.X;
                            double ErrorLocalY = Mlat.Y_Component_map - p.Y;
                            double PreviousPointTimeDifference = Mlat.Time_milisec - ADBSList[0].Time_milisec;
                            double NextPointTimeDifference = ADBSList[1].Time_milisec -Mlat.Time_milisec ;
                            data.ListZones[List.zone - 1].MLATMessagesUsed++;

                            PrecissionPoint pressP = new PrecissionPoint(TargetIdentification, TargetAdress, TrackNumberMLAT, Mlat.X_Component_map, Mlat.Y_Component_map, 53.321, p.X, p.Y, p.Z, ErrorLocalX, ErrorLocalY, dist, Mlat.zone, Mlat.GroundBit, Mlat.Time_milisec, PreviousPointTimeDifference, NextPointTimeDifference);
                            data.PrecissionPoints.Add(pressP);
                        }
                    }
                }
            }
            foreach (ListOfMLATSin5Seconds List in ListOrderedMlats)
            {
                ComputeTotalListOfMLATSin5Seconds(List, data);
            }

        }

        private void ComputeMLATprecisionInStandsDGPS(List<CAT10> MlatsInStandsForAccuracy, Data data)
        {
            MlatsInStandsForAccuracy.OrderBy(CAT10 => CAT10.Time_milisec);
            double startTime = MlatsInStandsForAccuracy[0].Time_milisec;
            List<ListOfMLATSin5Seconds> ListOrderedMlats = new List<ListOfMLATSin5Seconds>();
            foreach (CAT10 Mlat in MlatsInStandsForAccuracy)
            {
                ListOfMLATSin5Seconds list = ListOrderedMlats.Find(x => (x.StartTime <= Mlat.Time_milisec && x.EndTime > Mlat.Time_milisec && x.zone == Mlat.zone));
                if (list != null)
                {
                    list.MLATSList.Add(Mlat);
                }
                else
                {
                    list = new ListOfMLATSin5Seconds(Mlat.Time_milisec, Mlat.zone, Mlat);
                    ListOrderedMlats.Add(list);
                }
            }
            foreach (ListOfMLATSin5Seconds List in ListOrderedMlats)
            {
                foreach (CAT10 Mlat in List.MLATSList)
                {
                    List<MarkerDGPS> DGPSList = FindPreviousAndNextDGPS(Mlat);
                    if (DGPSList.Count() == 2)
                    {
                        PointWithHeight p = ComputePointInMlatTIme(Mlat, DGPSList[0], DGPSList[1]);

                        if (p != null)
                        {
                            data.ListZones[List.zone - 1].MLATMessagesUsed ++;
                            Point MLATp = new Point(Mlat.X_Component_map, Mlat.Y_Component_map);
                            double dist = ComputeDistanceXY(MLATp, p);
                            List.Distances.Add(dist);
                            double ErrorLocalX = Mlat.X_Component_map - p.X;
                            double ErrorLocalY = Mlat.Y_Component_map - p.Y;
                            double PreviousPointTimeDifference = Mlat.Time_milisec - DGPSList[0].Time;
                            double NextPointTimeDifference = DGPSList[1].Time - Mlat.Time_milisec;
                            PrecissionPoint pressP = new PrecissionPoint(TargetIdentification, TargetAdress, TrackNumberMLAT, Mlat.X_Component_map, Mlat.Y_Component_map, 53.321, p.X, p.Y, p.Z, ErrorLocalX, ErrorLocalY, dist, Mlat.zone, Mlat.GroundBit, Mlat.Time_milisec, PreviousPointTimeDifference, NextPointTimeDifference);
                            data.PrecissionPoints.Add(pressP);
                        }
                    }
                }
            }
            foreach (ListOfMLATSin5Seconds List in ListOrderedMlats)
            {
                ComputeTotalListOfMLATSin5Seconds(List, data);
            }

        }

        private void ComputeTotalListOfMLATSin5Seconds(ListOfMLATSin5Seconds List, Data data)
        {
            double dist = 0;
            foreach (double distance in List.Distances)
            {
                dist += distance;
            }
            double precision = dist / List.Distances.Count();
            double PFDdist = 50; /////DISTANCE IN FUNCTION OF ZONE
            if (precision > PFDdist)
            {
                data.ListZones[List.zone - 1].FalseDetection++;
            }
            else
            {
                data.ListZones[List.zone - 1].CorrectDetection++;
            }
            data.ListZones[List.zone - 1].MLATPrecision.Add(precision);
                     
        }



        public void ComputePrecissionMLATinterpoledDGPS(Data data)
        {
            if (ListMLAT.Count() > 0)
            {
                if (FixedMLATS.Contains(TargetAdress))
                {

                    List<CAT10> MlatsInStandsForAccuracy = new List<CAT10>();
                    foreach (CAT10 MLAT in ListMLAT)
                    {
                        if (MLAT.zone == 4 || MLAT.zone == 5)
                        {
                            MlatsInStandsForAccuracy.Add(MLAT);
                        }
                        else
                        {
                            ComputeMLATPrecissionFixedTransp(MLAT, data);
                        }
                    }
                    if (MlatsInStandsForAccuracy.Count() > 1)
                    {
                        ComputeMLATprecisionInStandsADSBFixed(MlatsInStandsForAccuracy, data);
                    }
                }
                else
                {

                    //if (!aircraftMLAT)
                    //{
                    //    foreach (CAT10 MLAT in ListMLAT)
                    //    {
                    //        if (MLAT.TOT == "Aircraft")
                    //        {
                    //            aircraftMLAT = true;
                    //            break;
                    //        }
                    //    }
                    //}
                    //if (!aircraftMLAT)
                    //{
                    //    foreach (CAT21vs21 ADSB in ListADSB)
                    //    {
                    //        if (ADSB.ECAT == "Light aircraft" || ADSB.ECAT == "Small aircraft" || ADSB.ECAT == "Medium aircraft" || ADSB.ECAT == "Heavy aircraft")
                    //        {
                    //            aircraftMLAT = true;
                    //            break;
                    //        }
                    //    }
                    //}
                    //if (aircraftMLAT)
                    //{
                        if (ListMLAT.Count > 0 && ListDGPS.Count > 0)
                        {
                            List<CAT10> MlatsInStandsForAccuracy = new List<CAT10>();

                            foreach (CAT10 MLAT in ListMLAT)
                            {

                                if (MLAT.zone != -1)
                                {
                                    if (MLAT.zone == 4 || MLAT.zone == 5)
                                    {
                                        MlatsInStandsForAccuracy.Add(MLAT);
                                    }
                                    else
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
                                    }
                                }
                            }
                            if (MlatsInStandsForAccuracy.Count() > 1)
                            {
                                ComputeMLATprecisionInStandsDGPS(MlatsInStandsForAccuracy, data);
                            }
                            foreach (MarkerDGPS DGPS in ListDGPS)
                            {

                                if (DGPS.used == true && DGPS.zone!=-1)

                                {
                                    data.ListZones[DGPS.zone - 1].DGPSMessagesUsed++;
                                }
                            }
                         
                        }
                    //}
                }
            }        

            
        }

        private void ComputeMLATPrecissionFixedTransp(CAT10 MLAT, Data data)
        {
            if (MLAT.zone != -1)
            {
                Point p = LibreriaDecodificacion.GetFixedMLATPos(TargetAdress); //Get fixed Transponder position from Libreria Decodificacion
                Point MLATp = new Point(MLAT.X_Component_map, MLAT.Y_Component_map);
                double dist = ComputeDistanceXY(MLATp, p); //Compute distance from fixed position and MLAT transmitted position

                ////START of PFD computation
                double PFDdist = 50;/////DISTANCE IN FUNCTION OF ZONE
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
                ////End of PFD computation

                data.ListZones[zone - 1].MLATPrecision.Add(dist); //Add computed precision to the list of precissions of the point zone
                data.ListZones[zone - 1].MLATMessagesUsed++; //Add number of messages used
                MLAT.used = true;
                double ErrorLocalX = MLAT.X_Component_map - p.X; //Compute dX and dY to save to the csv
                double ErrorLocalY = MLAT.Y_Component_map - p.Y;
                PrecissionPoint pressP = new PrecissionPoint(TargetIdentification, TargetAdress, TrackNumberMLAT, MLAT.X_Component_map, MLAT.Y_Component_map, 53.321, p.X, p.Y, 0, ErrorLocalX, ErrorLocalY, dist, MLAT.zone, MLAT.GroundBit, MLAT.Time_milisec,-1,-1);
                data.PrecissionPoints.Add(pressP);
            }

        }



        private void ComputeMLATPrecission(CAT10 MLAT, CAT21vs21 Before, CAT21vs21 After, Data data)
        {
            if (MLAT.zone != -1)
            {
                PointWithHeight p = ComputePositionXY(Before, After, MLAT);
                Point MLATp = new Point(MLAT.X_Component_map, MLAT.Y_Component_map);
                double dist = ComputeDistanceXY(MLATp, p);
                double difH = ComputeHeightDif(Before, After, MLAT);
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
                double PreviousPointTimeDifference= MLAT.Time_milisec-Before.Time_milisec;
                double NextPointTimeDifference = After.Time_milisec- MLAT.Time_milisec;
                PrecissionPoint pressP = new PrecissionPoint(TargetIdentification,TargetAdress ,TrackNumberMLAT, MLAT.X_Component_map, MLAT.Y_Component_map, 53.321, p.X, p.Y, p.Z, ErrorLocalX, ErrorLocalY, dist, MLAT.zone, MLAT.GroundBit, MLAT.Time_milisec, PreviousPointTimeDifference, NextPointTimeDifference);
                data.PrecissionPoints.Add(pressP);
                if (difH != -999)
                {
                    data.ListZones[zone - 1].totalMLATPrecissionheight.Add(difH);
                    data.ListZones[zone - 1].MLATMessagesUsedheight++;
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
                    double PreviousPointTimeDifference = MLAT.Time_milisec - Before.Time;
                    double NextPointTimeDifference = After.Time- MLAT.Time_milisec;
                    PrecissionPoint pressP = new PrecissionPoint(TargetIdentification, TargetAdress, TrackNumberMLAT, MLAT.X_Component_map, MLAT.Y_Component_map, 53.321, p.X, p.Y, p.Z, ErrorLocalX, ErrorLocalY, dist, MLAT.zone, MLAT.GroundBit,MLAT.Time_milisec, PreviousPointTimeDifference, NextPointTimeDifference);
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
        private double ComputeDistanceXY(Point MLAT, Point FixedTrans)
        {
            double X = MLAT.X - FixedTrans.X;
            double Y = MLAT.Y - FixedTrans.Y;
            double dist = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            return dist;
        }

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

    }
}
