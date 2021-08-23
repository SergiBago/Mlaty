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
using Accord.Math;
using System.Threading.Tasks;

namespace PGTAWPF
{
    public class Ficheros
    {


        public int numficheros = 0;
        public List<string> namestxt = new List<string>();
        public List<string> namesastadsb = new List<string>();
        public List<string> namesastdgps = new List<string>();       
        public List<int> AirportCodesList = new List<int>();
        public Data data = new Data();
        public List<MapMarker> listMarkers = new List<MapMarker>();
        List<int> PICS = new List<int>();
        bool SavePositions = true;
        List<TrajectoriesToCompute> trajdgps = new List<TrajectoriesToCompute>();

        public Ficheros()
        {
        }

        /// <summary>
        /// Clear all list, and delet all messages and loaded data. 
        /// </summary>
        public void ResetData()
        {
            namestxt.Clear();
            namesastdgps.Clear();
            namesastadsb.Clear();
            numficheros = 0;
            data = new Data();
            listMarkers.Clear();
        }

        /// <summary>
        /// Compute ADS-B MLATY files
        /// </summary>
        /// <param name="SaveMarkers"></param>
        /// <param name="PIC"></param>
        /// <returns></returns>
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
        //    catch
        //    {
        //        return 0;
        //    }
        }


        /// <summary>
        /// Compute DGPS-MLAT files
        /// </summary>
        /// <param name="SaveMarkers"></param>
        /// <returns></returns>
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

        }


        /// <summary>
        /// Decodify and compute each ADSB MLAT file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="s"></param>
        /// <param name="PICvalue"></param>
        public void ComputeFileADSB(string path, int s, string PICvalue)
        {

            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            //try
            //{
            bool computePIC = false;
            if (PICvalue == "Auto") { computePIC = true; }

                PICS.Clear();
                bool first = true;
                double firsttime = 0;
                List<TrajectoriesToCompute> traj = new List<TrajectoriesToCompute>();
                namesastadsb[s + 1] = "Decodifying file...";



                byte[] fileBytes = File.ReadAllBytes(path); //Read all bytes of the file. This returs a list of bytes which have a value from 0 to 254
                List<byte[]> listabyte = new List<byte[]>();
                int i = 0;
                int contador = fileBytes[2] + (fileBytes[1] * 256); //Compute message lenght. First Octet gives cat, second and third give lenght of message. 
                while (i < fileBytes.Length) //Create a list which every item will be an array of bytes (every item is a message)
                {
                    byte[] array = new byte[contador]; //Lenght of array will be lenght of message octets
                    for (int j = 0; j < array.Length; j++) 
                    {
                        array[j] = fileBytes[i]; //Clone readed bytes to message array
                        i++;
                    }
                    listabyte.Add(array); //Add message array to list of messages
                    if (i + 2 < fileBytes.Length) // Compute lenght of next messsage
                    {
                        contador = fileBytes[i + 2] + (fileBytes[i + 1] * 256); //Again, we take i+1 and i+2 because i+0 will give message cat and not lenght
                    }
                }
                List<string[]> listahex = new List<string[]>(); //Convert list of bytes into list of strings (we will work in Hex format)
                for (int x = 0; x < listabyte.Count; x++) //transform every byte into hex character
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
                        CAT10 newcat10 = new CAT10(arraystring, firsttime);

                        if (newcat10.TYP == "Mode S MLAT")
                        {
                            bool trajfound = false;
                            if (newcat10.Target_Address != null && newcat10.Target_Identification != null && newcat10.Track_Number != -1)
                            {
                                if (traj.Exists(x => (x.TargetAdress == newcat10.Target_Address && x.TargetIdentification == newcat10.Target_Identification && newcat10.Track_Number == x.TrackNumberMLAT)))
                                {
                                    TrajectoriesToCompute traject = traj.Find(x => (x.TargetAdress == newcat10.Target_Address && x.TargetIdentification == newcat10.Target_Identification && newcat10.Track_Number == x.TrackNumberMLAT));
                                    traject.ADDCAT10(newcat10);
                                    trajfound = true;
                                }

                            }
                            if (newcat10.Target_Address != null && newcat10.Track_Number != -1 && trajfound == false)
                            {
                                if (traj.Exists(x => (x.TargetAdress == newcat10.Target_Address && newcat10.Track_Number == x.TrackNumberMLAT)))
                                {
                                    TrajectoriesToCompute traject = traj.Find(x => (x.TargetAdress == newcat10.Target_Address && newcat10.Track_Number == x.TrackNumberMLAT));
                                    traject.ADDCAT10(newcat10);
                                    trajfound = true;
                                }
                            }
                            if (newcat10.Target_Address != null && !trajfound)
                            {

                                if (traj.Exists(x => x.TargetAdress == newcat10.Target_Address))
                                {
                                    TrajectoriesToCompute traject = traj.Find(x => x.TargetAdress == newcat10.Target_Address);
                                    traject.ADDCAT10(newcat10);
                                    trajfound = true;

                                }
                            }
                            if (newcat10.Track_Number != -1 && trajfound == false)
                            {
                                if (traj.Exists(x => x.TrackNumberMLAT == newcat10.Track_Number))
                                {

                                    TrajectoriesToCompute traject = traj.Find(x => x.TrackNumberMLAT == newcat10.Track_Number);
                                    traject.ADDCAT10(newcat10);
                                    trajfound = true;
                                }
                            }
                            if (!trajfound)
                            {

                                TrajectoriesToCompute newtraj = new TrajectoriesToCompute(newcat10);
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
                        CAT21vs21 newcat21 = new CAT21vs21(arraystring, firsttime);

                        if (newcat21.X_Component_map != -99999 && newcat21.Y_Component_map != -99999)
                        {
                            if (computePIC == true) { PICS.Add(newcat21.PIC); }

                            if (newcat21.MOPSversion == 2 || LibreriaDecodificacion.FixedMLATS.Contains(newcat21.Target_address))
                            {
                                bool trajfound = false;
                                if (newcat21.Target_address != null && newcat21.Target_Identification != null && newcat21.Track_Number != -1)
                                {
                                    if (traj.Exists(x => (x.TargetAdress == newcat21.Target_address && x.TargetIdentification == newcat21.Target_Identification && newcat21.Track_Number == x.TrackNumberADSB)))
                                    {
                                        TrajectoriesToCompute traject = traj.Find(x => (x.TargetAdress == newcat21.Target_address && x.TargetIdentification == newcat21.Target_Identification && newcat21.Track_Number == x.TrackNumberADSB));
                                        traject.ADDCAT21(newcat21);
                                        trajfound = true;
                                    }
                                }
                                if (newcat21.Target_address != null && newcat21.Track_Number != -1 && !trajfound)
                                {
                                    if (traj.Exists(x => (x.TargetAdress == newcat21.Target_address && newcat21.Track_Number == x.TrackNumberADSB)))
                                    {
                                        TrajectoriesToCompute traject = traj.Find(x => (x.TargetAdress == newcat21.Target_address && newcat21.Track_Number == x.TrackNumberADSB));
                                        traject.ADDCAT21(newcat21);
                                        trajfound = true;
                                    }
                                }
                                if (newcat21.Track_Number != -1 && !trajfound)
                                {
                                    if (traj.Exists(x => x.TrackNumberADSB == newcat21.Track_Number))
                                    {
                                        TrajectoriesToCompute traject = traj.Find(x => x.TrackNumberADSB == newcat21.Track_Number);
                                        traject.ADDCAT21(newcat21);
                                        trajfound = true;
                                    }
                                }
                                if (newcat21.Target_address != null && !trajfound)
                                {
                                    if (traj.Exists(x => x.TargetAdress == newcat21.Target_address))
                                    {
                                        TrajectoriesToCompute traject = traj.Find(x => x.TargetAdress == newcat21.Target_address);
                                        traject.ADDCAT21(newcat21);
                                        trajfound = true;
                                    }
                                }
                                if (!trajfound)
                                {
                                    TrajectoriesToCompute newtraj = new TrajectoriesToCompute(newcat21);
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
                foreach (TrajectoriesToCompute traject in traj)
                {
                    if (!LibreriaDecodificacion.GetExcluded().Contains(traject.TargetAdress))
                    {
                        traject.SetZones();
                        traject.ComputePrecissionADSBinterpoled(this.data, PIC);
                        traject.ComputePDUD(this.data);
                        traject.ComputePFI(this.data);
                        traject.ComputePI(this.data);
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
                traj = new List<TrajectoriesToCompute>();
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            //}
            //catch
            //{
            //    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            //    namesastadsb[s + 1] = "Can't Compute this file!!!";
            //}

        }

        public void ComputeFileDGPS(string path, int s)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            try
            {
                PICS.Clear();
                bool first = true;
                double firsttime = 0;
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
                        CAT10 newcat10 = new CAT10(arraystring, firsttime);
                        if (newcat10.TYP == "Mode S MLAT")
                        {

                            bool trajfound = false;
                            if(newcat10.Track_Number!=-1)
                            {
                                TrajectoriesToCompute traject = trajdgps.Find(x => x.TargetIdentification == newcat10.Target_Identification);
                                if (traject != null)
                                {
                                    if (traject.ListMLAT.Count() > 0 && traject.ListMLAT[traject.ListMLAT.Count() - 1].Time_of_day_sec >= newcat10.Time_of_day_sec - 25 && traject.ListMLAT[traject.ListMLAT.Count() - 1].Time_of_day_sec <= newcat10.Time_of_day_sec + 25)
                                    {
                                        traject.ADDCAT10(newcat10);
                                        trajfound = true;
                                    }
                                    else
                                    {
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
                                    }
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
                foreach (TrajectoriesToCompute traject in trajdgps)
                {
 
                        traject.SetZones();
                        traject.ComputePrecissionMLATinterpoledDGPS(this.data);
                        traject.ComputePDUD(this.data);
                        traject.ComputePFI(this.data);
                        traject.ComputePI(this.data);

                        if (SavePositions == true)
                        {
                            traject.SaveMarkers(listMarkers);
                        }
                    
                }
                listahex = null;
                numficheros++;
                namesastdgps[s + 1] = "Computed!";
                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
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
                TrajectoriesToCompute newtraj = new TrajectoriesToCompute();

                if (attributes.Length > 1 && lines.Count() > 1)
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
                        if (lines[1].Substring(0, 1) == "#")
                        {
                            type = 2;
                        }
                        else
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
                                if (lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180 && time >= 0 && time <= 86400)
                                {
                                    type = 1;
                                }
                            }
                            catch { time = -1; }
                        }
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
                                Point Pxy = LibreriaDecodificacion.ComputeCartesianFromWGS84(p);
                                time = (parameters2[6]) * 3600 + (parameters2[7]) * 60 + (parameters2[8]);
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
                            }
                            catch {; }
                            namestxt[s + 1] = "Computing message " + Convert.ToString(i) + " of " + Convert.ToString(lines.Length) + " messages...";
                        }
                    }

                    else if (type == 0)
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
                                Point Pxy = LibreriaDecodificacion.ComputeCartesianFromWGS84(p);
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
                            }
                            catch {; }
                            namestxt[s + 1] = "Computing message " + Convert.ToString(i) + " of " + Convert.ToString(lines.Length) + " messages...";
                        }
                    }
                    else
                    {
                        namestxt[s + 1] = "Decodifying file...";
                        for (int i = 1; i < lines.Length; i++)
                        {
                                if (lines[i] != "" && lines[i].Substring(0, 1) != "#")
                                {
                                    char[] Splitter = new Char[] { ' ', '\t' };
                                    string[] parameters = lines[i].Split(Splitter, StringSplitOptions.RemoveEmptyEntries);
                                    if (parameters[0] != ":")
                                    {

                                        List<string> parameters2 = new List<string>();
                                        for (int f = 0; f < parameters.Length; f++)
                                        {
                                            if (parameters[f] != "O")
                                            {
                                                parameters2.Add(parameters[f]);
                                            }
                                        }
                                        parameters = parameters2.ToArray();
                                        parameters2.Clear();

                                        string[] TimeParam = parameters[1].Split(':');
                                        double hours = Convert.ToDouble(TimeParam[0]);
                                        double min = Convert.ToDouble(TimeParam[1]);
                                        double seconds = Convert.ToDouble(TimeParam[2].Replace('.', ','));
                                        double height = (Convert.ToDouble(parameters[2].Replace('.', ',')) * 0.3048);


                                        double lat = ComputeLat(parameters[4]);
                                        double lon = ComputeLon(parameters[5]);
                                        PointLatLng p = new PointLatLng(lat, lon);
                                        Point Pxy = LibreriaDecodificacion.ComputeCartesianFromWGS84(p, height);
                                        time = (hours) * 3600 + (min) * 60 + (seconds);
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
                                        MarkerDGPS DGPS = new MarkerDGPS(p, Pxy, time, height);
                                        newtraj.ADDDGPS(DGPS);
                                    }
                                    namestxt[s + 1] = "Computing message " + Convert.ToString(i) + " of " + Convert.ToString(lines.Length) + " messages...";

                                }
                            }
                        
    
                    }
                    trajdgps.Add(newtraj);
                    namestxt[s + 1] = "Computed!";

                }
                else
                {
                    namestxt[s + 1] = "No flight identification in this file";

                }
            }

            catch
            {
                namestxt[s + 1] = "Can't Compute this file!!!";
            }

        }

        private double ComputeLat(string Lat)
        {
            string[] latParam = Lat.Split('?');
            double latDeg = Convert.ToDouble(latParam[0].Substring(0, latParam[0].Length - 1));
            double latMin = Convert.ToDouble(latParam[1].Substring(0, latParam[1].Length - 1));
            double latSec = Convert.ToDouble(latParam[2].Substring(0, latParam[2].Length - 1).Replace('.',','));
            double Latitude = latDeg + (latMin / 60) + (latSec / 3600);
            if(latParam[3]=="S")
            {
                Latitude = -Latitude;
            }
            return Latitude;
        }

        private double ComputeLon(string Lon)
        {
            string[] lonParam = Lon.Split('?');
            double lonDeg = Convert.ToDouble(lonParam[0].Substring(0, lonParam[0].Length - 1));
            double lonMin = Convert.ToDouble(lonParam[1].Substring(0, lonParam[1].Length - 1));
            double lonSec = Convert.ToDouble(lonParam[2].Substring(0, lonParam[2].Length - 1).Replace('.', ','));
            double Longitude = lonDeg + (lonMin / 60) + (lonSec / 3600);
            if (lonParam[3] == "W")
            {
                Longitude = -Longitude;
            }
            return Longitude;
        }

    }
}