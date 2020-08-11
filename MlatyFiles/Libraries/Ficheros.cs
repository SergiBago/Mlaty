using System.Collections.Generic;
using System.IO;
using System.Data;
using System;
using System.Windows;
using GMap.NET;
using System.Linq;
using System.Windows.Forms;
using System.Collections;
using PGTA_WPF;
using MessageBox = System.Windows.MessageBox;

namespace PGTAWPF
{
    public class Ficheros
    {


        public int numficheros = 0;
        public List<string> namestxt = new List<string>();
        public List<string> namesastadsb = new List<string>();
        public List<string> namesastdgps = new List<string>();
        readonly LibreriaDecodificacion lib = new LibreriaDecodificacion();
        public List<int> AirportCodesList = new List<int>();
        public Data data = new Data();
        public List<MapMarker> listMarkers = new List<MapMarker>();
        List<int> PICS = new List<int>();
        bool SavePositions = true;
        List<TrajectoriestoCompute> trajdgps = new List<TrajectoriestoCompute>();


        public Ficheros()
        {
        }

        public void ResetData()
        {

            namestxt.Clear();
            namesastdgps.Clear();
            namesastadsb.Clear();
            numficheros = 0;
            data = new Data();
            listMarkers.Clear();
        }

        public int ComputeValuesADSB(bool SaveMarkers, string PIC)
        {
            //try
            //{
                SavePositions = SaveMarkers;
                for (int i = 0; i < namesastadsb.Count; i += 2)
                {
                    if (namesastadsb[i + 1] == "Uncomputed!")
                    {
                        ComputeFileADSB(namesastadsb[i], (i), PIC);
                    }
                }
                return 1;
            //}
            //catch
            //{
            //    return 0;
            //}
        }

        public int ComputeValuesDGPS(bool SaveMarkers)
        {
            SavePositions = SaveMarkers;
            for (int i = 0; i < namestxt.Count; i += 2)
            {
                if (namestxt[i + 1] == "Uncomputed!")
                {
                    ComputeFileTXT(namestxt[i], (i));
                }
            }
            for (int i = 0; i < namesastdgps.Count; i += 2)
            {
                if (namesastdgps[i + 1] == "Uncomputed!")
                {
                    ComputeFileDGPS(namesastdgps[i], (i));
                }
            }
            return 1;


            //try
            //{
            //SavePositions = SaveMarkers;
            //for (int i = 0; i < names.Count; i += 2)
            //{
            //    if (names[i + 1] == "Uncomputed!")
            //    {
            //        ComputeFile(names[i], (i));
            //    }
            //}
            //return 1;
            //}
            //catch
            //{
         //   return 0;
            //}
        }

        private void AddMapMarker(CAT21vs21 newcat)
        {
            if (newcat.zone != -1)
            {
                PointLatLng p = new PointLatLng(newcat.LatitudeWGS_84_map, newcat.LongitudeWGS_84_map);
                MapMarker mark = new MapMarker(newcat.zone, p,1);
                listMarkers.Add(mark);
            }
        }

        private void AddMapMarker(CAT10 newcat)
        {
            if (newcat.zone != -1)
            {
                PointLatLng p = new PointLatLng(newcat.LatitudeWGS_84_map, newcat.LongitudeWGS_84_map);
                MapMarker mark = new MapMarker(newcat.zone, p,0);
                listMarkers.Add(mark);
            }
        }

        
        public void ComputeFileADSB(string path, int s, string PICvalue)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            try
            {
                bool computePIC = false;
                if (PICvalue == "Auto") { computePIC = true; }

                PICS.Clear();
                bool first = true;
                double firsttime = 0;
                List<TrajectoriestoCompute> traj = new List<TrajectoriestoCompute>();
                namesastadsb[s + 1] = "Decodifying file...";
                byte[] fileBytes = File.ReadAllBytes(path);
                List<byte[]> listabyte = new List<byte[]>();
                int i = 0;
                int contador = fileBytes[2] + (fileBytes[1] * 256);
                while (i < fileBytes.Length)
                {
                    byte[] array = new byte[contador];
                    for (int j = 0; j < array.Length; j++)
                    {
                        array[j] = fileBytes[i];
                        i++;
                    }
                    listabyte.Add(array);
                    if (i + 2 < fileBytes.Length)
                    {
                        contador = fileBytes[i + 2] + (fileBytes[i + 1] * 256);
                    }
                }
                List<string[]> listahex = new List<string[]>();
                for (int x = 0; x < listabyte.Count; x++)
                {
                    byte[] buffer = listabyte[x];
                    string[] arrayhex = new string[buffer.Length];
                    for (int y = 0; y < buffer.Length; y++)
                    {
                        arrayhex[y] = buffer[y].ToString("X");
                    }
                    listahex.Add(arrayhex);
                }

                for (int q = 0; q < listahex.Count; q++)
                {
                    namesastadsb[s + 1] = "Computing message " + Convert.ToString(q) + " of " + Convert.ToString(listahex.Count) + " messages...";
                    string[] arraystring = listahex[q];
                    int CAT = int.Parse(arraystring[0], System.Globalization.NumberStyles.HexNumber);
                    if (CAT == 10)
                    {
                        CAT10 newcat10 = new CAT10(arraystring, firsttime, lib);
                        if (newcat10.TYP == "Mode S MLAT")
                        {

                            bool trajfound = false;
                            if (newcat10.Target_Identification != null)
                            {
                                if (traj.Exists(x => x.TargetIdentification == newcat10.Target_Identification))
                                {
                                    traj.Find(x => x.TargetIdentification == newcat10.Target_Identification).ADDCAT10(newcat10);
                                    trajfound = true;
                                }
                            }
                            else if (newcat10.Target_Address != null && trajfound == false)
                            {
                                if (traj.Exists(x => x.TargetAdress == newcat10.Target_Address))
                                {
                                    traj.Find(x => x.TargetAdress == newcat10.Target_Address).ADDCAT10(newcat10);
                                    trajfound = true;
                                }
                            }
                            if (newcat10.Track_Number != -1 && trajfound == false)
                            {
                                if (traj.Exists(x => x.TrackNumber == newcat10.Track_Number))
                                {
                                    traj.Find(x => x.TrackNumber == newcat10.Track_Number).ADDCAT10(newcat10);
                                    trajfound = true;
                                }
                            }
                            if (trajfound == false)
                            {
                                TrajectoriestoCompute newtraj = new TrajectoriestoCompute(newcat10);
                                traj.Add(newtraj);
                            }
                            if (first == true)
                            {
                                first = false;
                                firsttime = newcat10.Time_milisec;
                            }
                        }
                    }
                    else if (CAT == 21)
                    {
                        if (lib.GetVersion(arraystring) != 0)
                        {
                            CAT21vs21 newcat21 = new CAT21vs21(arraystring, firsttime, lib);
                            if (computePIC == true) { PICS.Add(newcat21.PIC); }

                            if (newcat21.MOPSversion == 2)
                            {
                                bool trajfound = false;
                                if (newcat21.Target_Identification != null)
                                {
                                    if (traj.Exists(x => x.TargetIdentification == newcat21.Target_Identification))
                                    {
                                        traj.Find(x => x.TargetIdentification == newcat21.Target_Identification).ADDCAT21(newcat21);
                                        trajfound = true;
                                    }
                                }
                                if (newcat21.Target_address != null && trajfound == false)
                                {
                                    if (traj.Exists(x => x.TargetAdress == newcat21.Target_address))
                                    {
                                        traj.Find(x => x.TargetAdress == newcat21.Target_address).ADDCAT21(newcat21);
                                        trajfound = true;
                                    }
                                }
                                if (newcat21.Track_Number != -1 && trajfound == false)
                                {
                                    if (traj.Exists(x => x.TrackNumber == newcat21.Track_Number))
                                    {
                                        traj.Find(x => x.TrackNumber == newcat21.Track_Number).ADDCAT21(newcat21);
                                        trajfound = true;
                                    }
                                }
                                if (trajfound == false)
                                {
                                    TrajectoriestoCompute newtraj = new TrajectoriestoCompute(newcat21);
                                    traj.Add(newtraj);
                                }
                                if (first == true)
                                {
                                    first = false;
                                    firsttime = newcat21.Time_milisec;
                                }
                            }
                        }
                    }
                }

                namesastadsb[s + 1] = "Computing Parameters...";
                int PIC = 0;
                if (computePIC == true)
                {

                    PICS.Sort();
                    if (PICS.Count > 0)
                    {
                        try { PIC = PICS[Convert.ToInt32(Math.Truncate(PICS.Count() * 0.25)) - 1]; }
                        catch { PIC = PICS[PICS.Count() - 1]; }
                    }
                }
                else { PIC = Convert.ToInt32(PICvalue); }
                foreach (TrajectoriestoCompute traject in traj)
                {
                    //if (traject.TargetIdentification== "ECKJQ" || traject.TargetAdress== "34304F")
                    //{
                    //    int a = 1;
                    //}
                    bool Excluded = lib.ExcludedMLATS.Contains(traject.TargetIdentification);
                    if (Excluded == false)
                    {
                        traject.SetZones(lib);
                        traject.ComputePrecissionADSBinterpoled(this.data, PIC);
                        traject.ComputePDADSBInterpoled(this.data);
                        if (SavePositions == true)
                        {
                            traject.SaveMarkers(listMarkers);
                        }
                    }
                }
                listahex = null;
                fileBytes = null;
                numficheros++;
                namesastadsb[s + 1] = "Computed!";
                traj = null;
                traj = new List<TrajectoriestoCompute>();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                // return 1;
            }
            catch
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                namesastadsb[s + 1] = "Can't Compute this file!!!";
            }

        }

        public void ComputeFileDGPS(string path, int s)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            try
            {
                PICS.Clear();
                bool first = true;
                double firsttime = 0;
                //    List<TrajectoriestoCompute> traj = new List<TrajectoriestoCompute>();
                namesastdgps[s + 1] = "Decodifying file...";
                byte[] fileBytes = File.ReadAllBytes(path);
                List<byte[]> listabyte = new List<byte[]>();
                int i = 0;
                int contador = fileBytes[2] + (fileBytes[1] * 256);
                while (i < fileBytes.Length)
                {
                    byte[] array = new byte[contador];
                    for (int j = 0; j < array.Length; j++)
                    {
                        array[j] = fileBytes[i];
                        i++;
                    }
                    listabyte.Add(array);
                    if (i + 2 < fileBytes.Length)
                    {
                        contador = fileBytes[i + 2] + (fileBytes[i + 1] * 256);
                    }
                }
                List<string[]> listahex = new List<string[]>();
                for (int x = 0; x < listabyte.Count; x++)
                {
                    byte[] buffer = listabyte[x];
                    string[] arrayhex = new string[buffer.Length];
                    for (int y = 0; y < buffer.Length; y++)
                    {
                        arrayhex[y] = buffer[y].ToString("X");
                    }
                    listahex.Add(arrayhex);
                }
                for (int q = 0; q < listahex.Count; q++)
                {
                    namesastdgps[s + 1] = "Computing message " + Convert.ToString(q) + " of " + Convert.ToString(listahex.Count) + " messages...";
                    string[] arraystring = listahex[q];
                    int CAT = int.Parse(arraystring[0], System.Globalization.NumberStyles.HexNumber);
                    if (CAT == 10)
                    {
                        CAT10 newcat10 = new CAT10(arraystring, firsttime, lib);
                        if (newcat10.TYP == "Mode S MLAT")
                        {

                            bool trajfound = false;
                            if (newcat10.Target_Identification != null)
                            {
                                if (trajdgps.Exists(x => x.TargetIdentification == newcat10.Target_Identification))
                                {
                                    trajdgps.Find(x => x.TargetIdentification == newcat10.Target_Identification).ADDCAT10(newcat10);
                                    trajfound = true;
                                }
                            }
                            else if (newcat10.Target_Address != null && trajfound == false)
                            {
                                if (trajdgps.Exists(x => x.TargetAdress == newcat10.Target_Address))
                                {
                                    trajdgps.Find(x => x.TargetAdress == newcat10.Target_Address).ADDCAT10(newcat10);
                                    trajfound = true;
                                }
                            }
                            if (first == true)
                            {
                                first = false;
                                firsttime = newcat10.Time_milisec;
                            }
                        }
                    }
                }
                namesastdgps[s + 1] = "Computing Parameters...";
                foreach (TrajectoriestoCompute traject in trajdgps)
                {
                    //MessageBox.Show(Convert.ToString(traject.ListDGPS.Count) + "  " + Convert.ToString(traject.ListADSB.Count));
                    //bool Excluded = lib.ExcludedMLATS.Contains(traject.TargetIdentification);
                    //if (Excluded == false)
                    //{
                        traject.SetZones(lib);
                        traject.ComputePrecissionADSBinterpoledDGPS(this.data);
                        traject.ComputePDADSBInterpoled(this.data);
                        if (SavePositions == true)
                        {
                            traject.SaveMarkers(listMarkers);
                        }
                    //}
                }
                listahex = null;
                //fileBytes = null;
                numficheros++;
                namesastdgps[s + 1] = "Computed!";
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
               // return 1;
            }
            catch
            {
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                namesastdgps[s + 1] = "Can't Compute this file!!!";
            }
        }

        

        public void ComputeFileTXT(string path, int s)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            try
            {
                namestxt[s + 1] = "Decodifying file...";
                string[] lines = File.ReadAllLines(path);
                string Heather = lines[0];
                string[] attributes = Heather.Split(',');
                TrajectoriestoCompute newtraj = new TrajectoriestoCompute();

                if (attributes.Length > 1 && lines.Count()>1)
                {
                    double firsttime = 0;
                    bool first = true;
                    int type = 0;
                    attributes[0] = attributes[0].Replace(" ", "");
                    attributes[0] = attributes[0].Replace("\t", "");

                    attributes[1] = attributes[1].Replace(" ", "");
                    attributes[1] = attributes[1].Replace("\t", "");

                    newtraj.setAtributes(attributes[0], attributes[1]);

                    double time = 0;
                    try
                    {
                        string[] parameters = lines[1].Split(new Char[] { ' ', '\t' });
                        List<double> parameters2 = new List<double>();
                        for (int a = 1; a < parameters.Length; a++)
                        {
                            parameters[a] = parameters[a].Trim(' ');
                            if (parameters[a] != "")
                            {
                                parameters[a] = parameters[a].Replace('.', ',');
                                parameters2.Add(Convert.ToDouble(parameters[a]));
                            }
                        }
                       
                        try
                        {
                            double lat = parameters2[0];
                            double lon = parameters2[1];
                            time = (parameters2[6]) * 3600 + (parameters2[7]) * 60 + (parameters2[8]);
                            if (lat >= -90 && lat <= 90 && lon >= -180 && lon<= 180 && time >= 0 && time <= 86400)
                            {
                                type = 1;
                            }
                        }
                        catch { time = -1; }
                    }
                    catch {; }
                    if (type == 1)
                    {

                        for (int i = 1; i < lines.Length; i++)
                        {
                            try
                            {
                                string[] parameters = lines[i].Split(new Char[] { ' ', '\t' });
                                List<double> parameters2 = new List<double>();
                                for (int a = 1; a < parameters.Length; a++)
                                {
                                    parameters[a] = parameters[a].Trim(' ');
                                    if (parameters[a] != "")
                                    {
                                        parameters[a] = parameters[a].Replace('.', ',');

                                        parameters2.Add(Convert.ToDouble(parameters[a]));
                                    }
                                }
                                double lat = parameters2[0];
                                double lon = parameters2[1];
                                PointLatLng p = new PointLatLng(lat, lon);
                                Point Pxy = lib.ComputeCartesianFromWGS84(p);
                                time = (parameters2[6]) * 3600 + (parameters2[7]) * 60 + (parameters2[8]);
                                if (first==false)
                                {
                                    if (time<firsttime)
                                    {
                                        time += 86400;
                                    }
                                }
                                else
                                {
                                    firsttime = time;
                                    first = false;
                                }
                                MarkerDGPS DGPS = new MarkerDGPS(p, Pxy, time);
                                newtraj.ADDDGPS(DGPS);
                            }
                            catch {; }
                            namestxt[s + 1] = "Computing message " + Convert.ToString(i) + " of " + Convert.ToString(lines.Length) + " messages...";
                        }
                    }

                    else
                    {
                        for (int i = 1; i < lines.Length; i++)
                        {
                            try
                            {
                                string[] parameters = lines[i].Split(new Char[] { ' ', '\t' });
                                List<double> values = new List<double>();
                                foreach (string par in parameters)
                                {
                                    try
                                    {
                                        string par2 = par.Replace('.', ',');
                                        values.Add(Convert.ToDouble(par2));
                                    }
                                    catch {; }
                                }

                                PointLatLng p = new PointLatLng(values[0], values[1]);
                                Point Pxy = lib.ComputeCartesianFromWGS84(p);
                                time = (values[2] * 3600 + values[3] * 60 + values[4]);
                                if (first == false)
                                {
                                    if (time < firsttime)
                                    {
                                        time += 86400;
                                    }
                                }
                                else
                                {
                                    firsttime = time;
                                    first = false;
                                }
                                MarkerDGPS DGPS = new MarkerDGPS(p, Pxy, time);
                                newtraj.ADDDGPS(DGPS);
                                //   MessageBox.Show(Convert.ToString(p.Lat) + "  " + Convert.ToString(p.Lng) + "  " + Convert.ToString(time));
                            }
                            catch {; }
                            namestxt[s + 1] = "Computing message " + Convert.ToString(i) + " of " + Convert.ToString(lines.Length) + " messages...";
                        }
                     //   MessageBox.Show(Convert.ToString(newtraj.ListDGPS.Count));
                    }
                    //if (values [0]>=-90 && values [0]<=90 && values[1] >=-180 && values [1]<=180 && time>=0 && time<=86400 )
                    //{
                    //    for (int i = 1; i < lines.Length; i++)
                    //    {
                    //        try
                    //        {
                    //            parameters = lines[i].Split(new Char[] { ' ', '\t' });
                    //            parameters2 = new List<string>();
                    //            for (int a=0; a<parameters.Length; a++)
                    //            {
                    //                parameters[a] = parameters[a].Trim(' ');
                    //                if (parameters[a] != "") { parameters2.Add(parameters[a]); }
                    //            }
                    //            values = new List<double>();
                    //            list = new int[] { 1, 2, 7, 8, 9 };
                    //        for (int e = 0; e < list.Length; e++)
                    //            {
                    //                try
                    //                {
                    //                    string par = parameters2[list[e]];
                    //                    string par2 = par.Replace('.', ',');
                    //                    values.Add(Convert.ToDouble(par2));
                    //                }
                    //                catch {; }
                    //            }

                    //            PointLatLng p = new PointLatLng(values[0], values[1]);
                    //            Point Pxy = lib.ComputeCartesianFromWGS84(p);
                    //            if ((time>3600 && time<70000) || ( time >87000))
                    //            {
                    //                int a = 0;
                    //            }    
                    //            time = (values[2] * 3600 + values[3] * 60 + values[4]);
                    //            MarkerDGPS DGPS = new MarkerDGPS(p, Pxy, time);
                    //            newtraj.ADDDGPS(DGPS);
                    //            if (time<1)
                    //            {
                    //                int a = 0;
                    //            }
                    //            //   MessageBox.Show(Convert.ToString(p.Lat) + "  " + Convert.ToString(p.Lng) + "  " + Convert.ToString(time));
                    //        }
                    //        catch {; }
                    //        namestxt[s + 1] = "Computing message " + Convert.ToString(i) + " of " + Convert.ToString(lines.Length) + " messages...";
                    //    }
                       
                    //}
                    //else
                    //{
                    //    for (int i = 1; i < lines.Length; i++)
                    //    {
                    //        try
                    //        {
                    //            parameters = lines[i].Split(new Char[] { ' ', '\t' });
                    //            values = new List<double>();
                    //            foreach (string par in parameters)
                    //            {
                    //                try
                    //                {
                    //                    string par2 = par.Replace('.', ',');
                    //                    values.Add(Convert.ToDouble(par2));
                    //                }
                    //                catch {; }
                    //            }

                    //            PointLatLng p = new PointLatLng(values[0], values[1]);
                    //            Point Pxy = lib.ComputeCartesianFromWGS84(p);
                    //            time = (values[2] * 3600 + values[3] * 60 + values[4]);
                    //            MarkerDGPS DGPS = new MarkerDGPS(p, Pxy, time);
                    //            newtraj.ADDDGPS(DGPS);
                    //            //   MessageBox.Show(Convert.ToString(p.Lat) + "  " + Convert.ToString(p.Lng) + "  " + Convert.ToString(time));
                    //        }
                    //        catch {; }
                    //        namestxt[s + 1] = "Computing message " + Convert.ToString(i) + " of " + Convert.ToString(lines.Length) + " messages...";
                    //    }
                    //}
                    trajdgps.Add(newtraj);
                   // MessageBox.Show(Convert.ToString(newtraj.ListDGPS.Count));
                    namestxt[s + 1] = "Computed!";
                 //   MessageBox.Show(Convert.ToString(newtraj.ListDGPS.Count()) + "  " + Convert.ToString(newtraj.ListDGPS[newtraj.ListDGPS.Count - 1].Time));

                }
                else
                {
                    namestxt[s + 1] = "Can't Compute this file!!!";
                }

            }
            catch
            {
                namestxt[s + 1] = "Can't Compute this file!!!";
            }

        }
    }
}