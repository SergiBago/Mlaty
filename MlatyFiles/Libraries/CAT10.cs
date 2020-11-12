using GMap.NET;
using MultiCAT6.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PGTAWPF
{
    public class CAT10
    {
        /// <summary>
        /// Hi
        /// </summary>
        readonly LibreriaDecodificacion lib;
        readonly string[] mensaje;
        readonly string FSPEC1;
        public string CAT = "10";
        public int num;
        public int cat10num;
        public int airportCode;
        public bool used = false;
        public bool usedPD = false;
        double FirstTime;
        public int zone=-1; //
        public bool saved = false ;

        public CAT10()
        {
        }


        public CAT10(string[] mensajehexa, double firsttime, LibreriaDecodificacion lib)
        {
            try
            {
                this.lib = lib;
                this.FirstTime = firsttime;
                this.mensaje = mensajehexa;
                FSPEC1 = lib.FSPEC(mensaje);
                int longFSPEC = this.FSPEC1.Length / 7;
                int pos = 3 + longFSPEC;
                char[] FSPEC = FSPEC1.ToCharArray(0, FSPEC1.Length);
                this.mensaje = lib.Passarmensajeenteroabinario(mensaje);
                if (FSPEC[0] == '1') { pos = this.Compute_Data_Source_Identifier(mensaje, pos); } //
                if (FSPEC[1] == '1') { pos = this.Compute_Message_Type(mensaje, pos); } //
                if (FSPEC[2] == '1') { pos = this.Compute_Target_Report_Descriptor(mensaje, pos); } //
                if (TYP == "Mode S MLAT")
                {
                    if (FSPEC[3] == '1') { pos = this.Compute_Time_of_Day(mensaje, pos); } //
                    if (FSPEC[4] == '1') { pos = this.Compute_Position_in_WGS_84_Coordinates(mensaje, pos); } //
                    if (FSPEC[5] == '1') { pos = this.Compute_Measured_Position_in_Polar_Coordinates(mensaje, pos); }
                    if (FSPEC[6] == '1') { pos = this.Compute_Position_in_Cartesian_Coordinates(mensaje, pos); } //
                    if (FSPEC.Count() > 8)
                    {
                        if (FSPEC[7] == '1') { pos = this.Compute_Track_Velocity_in_Polar_Coordinates(mensaje, pos); } //
                        if (FSPEC[8] == '1') { pos = this.Compute_Track_Velocity_in_Cartesian_Coordinates(mensaje, pos); } //
                        if (FSPEC[9] == '1') { pos = this.Compute_Track_Number(mensaje, pos); } //
                        if (FSPEC[10] == '1') { pos = this.Compute_Track_Status(mensaje, pos); } //
                        if (FSPEC[11] == '1') { pos = this.Compute_Mode_3A_Code_in_Octal_Representation(mensaje, pos); } //
                        if (FSPEC[12] == '1') { pos = this.Compute_Target_Address(mensaje, pos); } //
                        if (FSPEC[13] == '1') { pos = this.Compute_Target_Identification(mensaje, pos); } //
                    }
                    if (FSPEC.Count() > 16)
                    {
                        if (FSPEC[14] == '1') { pos = this.Compute_Mode_S_MB_Data(mensaje, pos); } //
                        if (FSPEC[15] == '1') { pos = this.Compute_Vehicle_Fleet_Identificatior(mensaje, pos); } //
                        if (FSPEC[16] == '1') { pos = this.Compute_Flight_Level_in_Binary_Representaion(mensaje, pos); } //
                        if (FSPEC[17] == '1') { pos = this.Compute_Measured_Height(mensaje, pos); } //
                        if (FSPEC[18] == '1') { pos = this.Compute_Target_Size_and_Orientation(mensaje, pos); } //
                        if (FSPEC[19] == '1') { pos = this.Compute_System_Status(mensaje, pos); } //
                        if (FSPEC[20] == '1') { pos = this.Compute_Preprogrammed_Message(mensaje, pos); }// 
                    }
                    if (FSPEC.Count() > 22)
                    {
                        if (FSPEC[21] == '1') { pos = this.Compute_Standard_Deviation_of_Position(mensaje, pos); }
                        if (FSPEC[22] == '1') { pos = this.Compute_Presence(mensaje, pos); }
                        if (FSPEC[23] == '1') { pos = this.Compute_Amplitude_of_Primary_Plot(mensaje, pos); }
                        if (FSPEC[24] == '1') { pos = this.Compute_Calculated_Acceleration(mensaje, pos); }
                    }                    
                }
            }
            catch
            {
            }
        }

        public void Setnum(int num)
        {
            this.num = num;
        }


        //COMPUTE CLASSES
        // Classes to compute the parameters

        // DATA ITEM I010/000, MESSAGE TYPE
        public string MESSAGE_TYPE;
        private int Compute_Message_Type(string[] message, int pos)
        {
            int Message_Type = Convert.ToInt32(message[pos], 2);
            if (Message_Type == 1) { MESSAGE_TYPE = "Target Report"; }
            if (Message_Type == 2) { MESSAGE_TYPE = "Start of Update Cycle"; }
            if (Message_Type == 3) { MESSAGE_TYPE = "Periodic Status Message"; }
            if (Message_Type == 4) { MESSAGE_TYPE = "Event-triggered Status Message"; }
            pos++;
            return pos;
        }

        // DATA ITEM I010/010, DATA SOURCE IDENTIFIER
        public string SAC;
        public string SIC;
        public string Data_flow_local_Airport;
        private int Compute_Data_Source_Identifier(string[] message, int pos)
        {
            //int SAC_data_flow_local_Airport = Convert.ToInt32(message[pos], 2);
            //if (SAC_data_flow_local_Airport == 0) { SAC = "Data flow local to the airport"; }
            //else {
            SAC = Convert.ToString(Convert.ToInt32(message[pos],2)); 
            SIC = Convert.ToString(Convert.ToInt32(message[pos + 1],2));
            this.airportCode = lib.GetAirporteCode(Convert.ToInt32(SIC));

            pos += 2;
            return pos;
        }




        // DATA ITEM I010/020, TARGET REPORT DESCRIPTOR
        public string TYP;
        public string DCR;
        public string CHN;
        public string GBS;
        public string CRT;
        //First extension
        public string SIM;
        public string TST;
        public string RAB;
        public string LOP;
        public string TOT;
        //Second extension
        public string SPI;
        public int GroundBit; //0 air, 1 land

        private int Compute_Target_Report_Descriptor(string[] message, int pos)
        {
            int cont = 1;
            string octeto1 = message[pos];
            string TYP = octeto1.Substring(0, 3);
            if (TYP == "000")
                this.TYP = "SSR MLAT";
            if (TYP == "001")
                this.TYP = "Mode S MLAT";
            if (TYP == "010")
                this.TYP = "ADS-B";
            if (TYP == "011")
                this.TYP = "PSR";
            if (TYP == "100")
                this.TYP = "Magnetic Loop System";
            if (TYP == "101")
                this.TYP = "HF MLAT";
            if (TYP == "110")
                this.TYP = "Not defined";
            if (TYP == "111")
                this.TYP = "Other types";

            string DCR = octeto1.Substring(3, 1);
            if (DCR == "0")
                this.DCR = "No differential correction";
            if (DCR == "1")
                this.DCR = "Differential correction";

            string CHN = octeto1.Substring(4, 1);
            if (CHN == "1")
                this.CHN = "Chain 2";
            if (CHN == "0")
                this.CHN = "Chain 1";

            string GBS = octeto1.Substring(5, 1);
            if (GBS == "0")
                GroundBit=0;
            if (GBS == "1")
                GroundBit = 1;
                //this.GBS = "Transponder Ground bit set";

            string CRT = octeto1.Substring(6, 1);
            if (CRT == "0")
                this.CRT = "No Corrupted reply in multilateration";
            if (CRT == "1")
                this.CRT = "Corrupted replies in multilateration";

            string FX = Convert.ToString(octeto1[7]);
            while (FX == "1")
            {
                string newoctet = message[pos + cont];
                FX = Convert.ToString(newoctet[7]);
                if (cont == 1) 
                {
                    string SIM = newoctet.Substring(0, 1);
                    if (SIM == "0")
                        this.SIM = "Actual target report";
                    if (SIM == "1")
                        this.SIM = "Simulated target report";

                    string TST = newoctet.Substring(1, 1);
                    if (TST == "0")
                        this.TST = "TST: Default";
                    if (TST == "1")
                        this.TST = "Test Target";

                    string RAB = newoctet.Substring(2, 1);
                    if (RAB == "0")
                        this.RAB = "Report from target transponder";
                    if (RAB == "1")
                        this.RAB = "Report from field monitor (fixed transponder)";

                    string LOP = newoctet.Substring(3, 2);
                    if (LOP == "00")
                        this.LOP = "Loop status: Undetermined";
                    if (LOP == "01")
                        this.LOP = "Loop start";
                    if (LOP == "10")
                        this.LOP = "Loop finish";

                    string TOT = newoctet.Substring(5, 2);
                    if (TOT == "00")
                        this.TOT = "Type of vehicle: Undetermined";
                    if (TOT == "01")
                        this.TOT = "Aircraft";
                    if (TOT == "10")
                        this.TOT = "Ground vehicle";
                    if (TOT == "11")
                        this.TOT = "Helicopter";
                }
                else 
                {
                    if (newoctet.Substring(0, 1) == "0")
                        this.SPI = "Absence of SPI (Special Position Identification)";
                    if (newoctet.Substring(0, 1) == "1")
                        this.SPI = "SPI (Special Position Identification)";
                }
                cont++;
            }

            pos= cont+pos;
            return pos;
        }

        // DATA ITEM I010/040, MEASURED POSITION IN POLAR CO-ORDINATES
        public string RHO;
        public string THETA;
        public string Position_In_Polar;
        private int Compute_Measured_Position_in_Polar_Coordinates(string[] message, int pos)
        {
            double Range = Convert.ToInt32(string.Concat(message[pos], message[pos + 1]), 2); //I suppose in meters
            if (Range >= 65536) { RHO = "RHO exceds the max range or is the max range ";} //RHO = " + Convert.ToString(Range) + "m"; MessageBox.Show("RHO exceed the max range or is the max range, RHO = {0}m", Convert.ToString(Range)); }
            else { RHO = "ρ:" + Convert.ToString(Range) + "m"; }//MessageBox.Show("RHO is the max range, RHO = {0}m", Convert.ToString(Range)); }
            THETA = ", θ:" + String.Format("{0:0.00}",Convert.ToDouble(Convert.ToInt32(string.Concat(message[pos + 2], message[pos + 3]), 2)) * (360 / (Math.Pow(2, 16)))) + "°"; //I suppose in degrees
            Position_In_Polar = RHO + THETA;
            pos += 4;
            return pos;
        }

        // DATA ITEM I010/040, POSITION IN WGS-84 CO-ORDINATES
        public string Latitude_in_WGS_84;
        public string Longitude_in_WGS_84;
        public double LatitudeWGS_84_map = -200;
        public double LongitudeWGS_84_map = -200;
        private int Compute_Position_in_WGS_84_Coordinates(string[] message, int pos)
        {
            double Latitude = lib.ComputeA2Complement(string.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3])) * (180 / (Math.Pow(2, 31))); pos += 4;
            double Longitude = lib.ComputeA2Complement(string.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3])) * (180 / (Math.Pow(2, 31))); pos += 4;
            int Latdegres = Convert.ToInt32(Math.Truncate(Latitude));
            int Latmin = Convert.ToInt32(Math.Truncate((Latitude - Latdegres) * 60));
            double Latsec = Math.Round(((Latitude - (Latdegres + (Convert.ToDouble(Latmin) / 60))) * 3600), 5);
            int Londegres = Convert.ToInt32(Math.Truncate(Longitude));
            int Lonmin = Convert.ToInt32(Math.Truncate((Longitude - Londegres) * 60));
            double Lonsec = Math.Round(((Longitude - (Londegres + (Convert.ToDouble(Lonmin) / 60))) * 3600), 5);
            Latitude_in_WGS_84 = Convert.ToString(Latdegres) + "º " + Convert.ToString(Latmin) + "' " + Convert.ToString(Latsec) + "''";
            Longitude_in_WGS_84 = Convert.ToString(Londegres) + "º" + Convert.ToString(Lonmin) + "' " + Convert.ToString(Lonsec) + "''";
            return pos;
        }

        // DATA ITEM I010/040, MEASURED POSITION IN CARTESIAN CO-ORDINATES
        public string X_Component;
        public string Y_Component;
        public double X_Component_map=-99999;
        public double Y_Component_map=-99999;
        public string Position_Cartesian_Coordinates;
        private int Compute_Position_in_Cartesian_Coordinates(string[] message, int pos)
        {
            X_Component_map= lib.ComputeA2Complement(string.Concat(message[pos], message[pos + 1]));
            Y_Component_map= lib.ComputeA2Complement(string.Concat(message[pos+2], message[pos + 3]));
            X_Component = Convert.ToString(X_Component_map);
            Y_Component = Convert.ToString(Y_Component_map);
            Position_Cartesian_Coordinates = "X: " + X_Component + ", Y: " + Y_Component;
            ComputeWGS_84_from_Cartesian();
            pos += 4;
            return pos;
        }

        private void ComputeZone()
        {
            Point p = new Point(X_Component_map, Y_Component_map);
            zone = lib.ComputeZone(p, GroundBit);
        }

        private void ComputeWGS_84_from_Cartesian()
        {
            //PointLatLng AirportPoint = lib.GetAirportARPCoorde(airportCode);
            double X = this.X_Component_map;
            double Y = this.Y_Component_map;
            CoordinatesXYZ ObjectCartesian = new CoordinatesXYZ(X, Y,0);
            PointLatLng AirportPoint = lib.GetCoordenatesSMRMALT(Convert.ToInt32(SIC));
            CoordinatesWGS84 AirportGeodesic = new CoordinatesWGS84(AirportPoint.Lat * (Math.PI / 180), AirportPoint.Lng * (Math.PI / 180));
            GeoUtils geoUtils = new GeoUtils();
            CoordinatesWGS84 MarkerGeodesic = geoUtils.change_system_cartesian2geodesic(ObjectCartesian, AirportGeodesic);
            geoUtils = null;
            this.LatitudeWGS_84_map = MarkerGeodesic.Lat * (180 / Math.PI);
            this.LongitudeWGS_84_map = MarkerGeodesic.Lon*(180/Math.PI);
            int Latdegres = Convert.ToInt32(Math.Truncate(LatitudeWGS_84_map));
            int Latmin = Convert.ToInt32(Math.Truncate((LatitudeWGS_84_map - Latdegres) * 60));
            double Latsec = Math.Round(((LatitudeWGS_84_map - (Latdegres + (Convert.ToDouble(Latmin) / 60))) * 3600), 5);
            int Londegres = Convert.ToInt32(Math.Truncate(LongitudeWGS_84_map));
            int Lonmin = Convert.ToInt32(Math.Truncate((LongitudeWGS_84_map - Londegres) * 60));
            double Lonsec = Math.Round(((LongitudeWGS_84_map - (Londegres + (Convert.ToDouble(Lonmin) / 60))) * 3600), 5);
            Latitude_in_WGS_84 = Convert.ToString(Latdegres) + "º " + Convert.ToString(Latmin) + "' " + Convert.ToString(Latsec) + "''";
            Longitude_in_WGS_84 = Convert.ToString(Londegres) + "º" + Convert.ToString(Lonmin) + "' " + Convert.ToString(Lonsec) + "''";
        }




        // DATA ITEM I010/060, MODE-3/A CODE IN OCTAL REPRESANTATION
        //public string V_Mode_3A;
        //public string G_Mode_3A;
        //public string L_Mode_3A;
        //public string Mode_3A;
        private int Compute_Mode_3A_Code_in_Octal_Representation(string[] message, int pos)
        {
            //char[] OctetoChar = message[pos].ToCharArray(0, 8);
            //if (OctetoChar[0] == '0') { V_Mode_3A = "V: Code validated"; }
            //else { V_Mode_3A = "V: Code not validated"; }
            //if (OctetoChar[1] == '0') { G_Mode_3A = "G: Default"; }
            //else { G_Mode_3A = "G: Garbled code"; }
            //if (OctetoChar[2] == '0') { L_Mode_3A = "L: Mode-3/A code derived from the reply of the transponder"; }
            //else { L_Mode_3A = "L: Mode-3/A code not extracted during the last scan"; }
            //Mode_3A = Convert.ToString(lib.ConvertDecimalToOctal(Convert.ToInt32(string.Concat(message[pos], message[pos + 1]).Substring(4, 12), 2))).PadLeft(4,'0');
            pos += 2;
            return pos;
        }

        // DATA ITEM I010/090, FLIGHT LEVEL IN BINARY REPRESENTATION
        public string V_Flight_Level;
        public string G_Flight_Level;
        public string Flight_Level_Binary;
        public double Flight_Level=-999;
        private int Compute_Flight_Level_in_Binary_Representaion(string[] message, int pos)
        {
            char[] OctetoChar = message[pos].ToCharArray(0, 8);
            if (OctetoChar[0] == '0') { V_Flight_Level = "Code validated"; }
            else { V_Flight_Level = "Code not validated"; }
            if (OctetoChar[1] == '0') { G_Flight_Level = "Default"; }
            else { G_Flight_Level = "Garbled code"; }
            Flight_Level_Binary = Convert.ToString(lib.ComputeA2Complement(string.Concat(message[pos], message[pos + 1]).Substring(2, 14)) * (0.25));
          //  Flight_Level = V_Flight_Level + ", " + G_Flight_Level + ", Flight Level: " + Flight_Level_Binary;
            Flight_Level = Convert.ToDouble(Flight_Level_Binary)* (0.3048*100);
            pos += 2;
            return pos;
        }

        // DATA ITEM I010/091, MEASURED HEIGHT
        public double Measured_Height=-999;
        private int Compute_Measured_Height(string[] message, int pos)
        {
            Measured_Height=lib.ComputeA2Complement(string.Concat(message[pos], message[pos + 1])) * 6.25* 0.3048 ;
            pos += 2;
            return pos;
        }

        // DATA ITEM I010/131, AMPLITUDE OF PRIMARY PLOT
        //public string PAM;
        private int Compute_Amplitude_of_Primary_Plot(string[] message, int pos)
        {
            //double pam = Convert.ToInt32(message[pos], 2); // Lo de Range 0.255 no se si está bien ponerlo
            //if (pam == 0) { PAM = "PAM: 0, the minimum detectable level for the radar"; }
            //else { PAM = "PAM: " + Convert.ToString(Convert.ToInt32(message[pos], 2)); }
            pos++;
            return pos;
        }

        // DATA ITEM I010/140, TIME OF DAY
        public string Time_Of_Day;
        public int Time_of_day_sec;
        public double Time_milisec;
        private int Compute_Time_of_Day(string[] message, int pos)
        {
            int str = Convert.ToInt32(string.Concat(message[pos], message[pos + 1], message[pos + 2]), 2);
            double segundos = (Convert.ToDouble(str) / 128);
            Time_of_day_sec = Convert.ToInt32(Math.Truncate(segundos));
            Time_milisec = segundos;
            if (Time_milisec<FirstTime) { Time_milisec += 86400; }
            TimeSpan tiempo = TimeSpan.FromSeconds(segundos);
            Time_Of_Day = tiempo.ToString(@"hh\:mm\:ss\:fff");
            pos += 3;
            return pos;
        }

        // DATA ITEM I010/161, TRACK NUMBER
        public int Track_Number=-1;
        private int Compute_Track_Number(string[] message, int pos) { Track_Number = Convert.ToInt32(string.Concat(message[pos], message[pos + 1]).Substring(4, 12), 2); pos += 2; return pos; }
      
        
        // DATA ITEM I010/170, TRACK STATUS
        //public string CNF;
        //public string TRE;
        //public string CST;
        //public string MAH;
        //public string TCC;
        //public string STH;
        ////Extension
        //// string FX_Track_status;
        ////First extension
        //public string TOM;
        //public string DOU;
        //public string MRS;
        ////Second extension
        //public string GHO;
        private int Compute_Track_Status(string[] message, int pos)
        {
            char[] OctetoChar = message[pos].ToCharArray(0, 8);
            //if (OctetoChar[0] == '0') { CNF = "Confirmed track"; }
            //else { CNF = "Track in initialisation phase"; }
            //if (OctetoChar[1] == '0') { TRE = "TRE: Default"; }
            //else { TRE = "TRE: Last report for a track"; }
            //int crt = Convert.ToInt32(string.Concat(OctetoChar[2], OctetoChar[3]), 2);
            //if (crt == 0) { CST = "No extrapolation"; }
            //else if (crt == 1) { CST = "Predictable extrapolation due to sensor refresh period"; }
            //else if (crt == 2) { CST = "Predictable extrapolation in masked area"; }
            //else if (crt == 3) { CST = "Extrapolation due to unpredictable absence of detection"; }
            //if (OctetoChar[4] == '0') { MAH = "MAH: Default"; }
            //else { MAH = "MAH: Horizontal manoeuvre"; }
            //if (OctetoChar[5] == '0') { TCC = "Tracking performed in 'Sensor Plane', i.e. neither slant range correction nor projection was applied"; }
            //else { TCC = "Slant range correction and a suitable projection technique are used to track in a 2D.reference plane, tangential to the earth model at the Sensor Site co-ordinates"; }
            //if (OctetoChar[6] == '0') { STH = "Measured position"; }
            //else { STH = "Smoothed position"; }
            pos++;
            if (OctetoChar[7] == '1')
            {
                OctetoChar = message[pos].ToCharArray(0, 8);
                //int tom = Convert.ToInt32(string.Concat(OctetoChar[0], OctetoChar[1]), 2);
                //if (tom == 0) { TOM = "TOM: Unknown type of movement"; }
                //else if (tom == 1) { TOM = "TOM: Taking-off"; }
                //else if (tom == 2) { TOM = "TOM: Landing"; }
                //else if (tom == 3) { TOM = "TOM: Other types of movement"; }
                //int dou = Convert.ToInt32(string.Concat(OctetoChar[2], OctetoChar[3], OctetoChar[4]), 2);
                //if (dou == 0) { DOU = "No doubt"; }
                //else if (dou == 1) { DOU = "Doubtful correlation (undetermined reason)"; }
                //else if (dou == 2) { DOU = "Doubtful correlation in clutter"; }
                //else if (dou == 3) { DOU = "Loss of accuracy"; }
                //else if (dou == 4) { DOU = "Loss of accuracy in clutter"; }
                //else if (dou == 5) { DOU = "Unstable track"; }
                //else if (dou == 6) { DOU = "Previously coasted"; }
                //int mrs = Convert.ToInt32(string.Concat(OctetoChar[5], OctetoChar[6]), 2);
                //if (mrs == 0) { MRS = "Merge or split indication undetermined"; }
                //else if (mrs == 1) { MRS = "Track merged by association to plot"; }
                //else if (mrs == 2) { MRS = "Track merged by non-association to plot"; }
                //else if (mrs == 3) { MRS = "Split track"; }
                pos++;
                if (OctetoChar[7] == '1')
                {
                    //OctetoChar = message[pos].ToCharArray(0, 8);
                    //if (OctetoChar[0] == '0') { GHO = "GHO: Default"; }
                    //else { GHO = "Ghost track"; }
                    pos++;
                }
            }
            return pos;
        }

        // DATA ITEM I010/200, CALCULATED TRACK VELOCITY IN POLAR CO-ORDINATES
        public double Ground_Speed;
        public double Track_Angle;
        public string Track_Velocity_Polar_Coordinates;
        private int Compute_Track_Velocity_in_Polar_Coordinates(string[] message, int pos)
        {
            double ground_speed = Convert.ToDouble(Convert.ToInt32(string.Concat(message[pos], message[pos + 1]), 2)) * Math.Pow(2, -14);
            double Ground_Speed = ground_speed * 1852;
            //if (ground_speed >= 2) { Ground_Speed = "Ground Speed exceed the max value [2 NM/s] or is the max value, "; }//; MessageBox.Show("Ground Speed exceed the max value [2 NM/s] or is the max value, Ground Speed = " + Convert.ToString(ground_speed) + " NM/s"); }
            //else { Ground_Speed = "GS: " + String.Format("{0:0.00}", meters) + " m/s, "; }
            Track_Angle = (Convert.ToInt32(string.Concat(message[pos + 2], message[pos + 3]), 2)) * (360 / (Math.Pow(2, 16)));
            //Track_Velocity_Polar_Coordinates = Ground_Speed + Track_Angle;
            pos += 4;
            return pos;
        }

        // DATA ITEM I010/200, CALCULATED TRACK VELOCITY IN CARTESIAN CO-ORDINATES
        public double Vx;
        public double Vy;
        public string Track_Velocity_in_Cartesian_Coordinates;
        private int Compute_Track_Velocity_in_Cartesian_Coordinates(string[] message, int pos)
        {
            double vx = (lib.ComputeA2Complement(string.Concat(message[pos], message[pos + 1])) * 0.25);
            Vx = vx;
            double vy = (lib.ComputeA2Complement(string.Concat(message[pos + 2], message[pos + 3])) * 0.25);
            Vy =vy;
            //Track_Velocity_in_Cartesian_Coordinates = Vx + Vy;
            pos += 4;
            return pos;
        }

        // DATA ITEM I010/200, CALCULATED ACCELERATION
        //public string Ax;
        //public string Ay;
        //public string Calculated_Acceleration;
        private int Compute_Calculated_Acceleration(string[] message, int pos)
        {
            //double ax = lib.ComputeA2Complement(message[pos]) * 0.25;
            //double ay = lib.ComputeA2Complement(message[pos + 1]) * 0.25;
            //if (Convert.ToInt32(ax) >= 31 || Convert.ToInt32(ax) <= -31) { Ax = "Ax exceed the max value or is the max value (+-31 m/s^2)"; }
            //else { Ax = "Ax: " + Convert.ToString(ax) + "m/s^2"; }
            //if (Convert.ToInt32(ay) >= 31 || Convert.ToInt32(ax) <= -31) { Ay = "Ay exceed the max value or is the max value (+-31 m/s^2)"; }
            //else { Ay = "Ay: " + Convert.ToString(ay) + "m/s^2"; }
            //Calculated_Acceleration = Ax + " " + Ay;
            pos += 2;
            return pos;
        }

        // DATA ITEM I010/200, TARGET ADDRESS
        public string Target_Address;
        private int Compute_Target_Address(string[] message, int pos) { Target_Address = string.Concat(lib.BinarytoHexa(message[pos]), lib.BinarytoHexa(message[pos + 1]), lib.BinarytoHexa(message[pos + 2])); pos += 3; return pos; }


        // DATA ITEM I010/200, TARGET IDENTIFICATION
        public string STI;
        public string Target_Identification;
        public string TAR;
        private int Compute_Target_Identification(string[] message, int pos)
        {
            char[] OctetoChar = message[pos].ToCharArray(0, 8);
            int sti = Convert.ToInt32(string.Concat(OctetoChar[0], OctetoChar[1]), 2);
            if (sti == 0) { STI = "Callsign or registration downlinked from transponder"; }
            else if (sti == 1) { STI = "Callsign not downlinked from transponder"; }
            else if (sti == 2) { STI = "Registration not downlinked from transponder"; }
            StringBuilder Identification = new StringBuilder();
            string octets = string.Concat(message[pos + 1], message[pos + 2], message[pos + 3], message[pos + 4], message[pos + 5], message[pos + 6]);
            for (int i = 0; i < 8; i++) { Identification.Append(lib.Compute_Char(octets.Substring(i * 6, 6))); }
            Target_Identification = Identification.ToString().Trim();
            pos += 7;
            return pos;
        }



        // DATA ITEM I010/200, MODE S MB DATA
        //public string[] MB_Data;
        //public string[] BDS1;
        //public string[] BDS2;
        //public int modeS_rep;
        private int Compute_Mode_S_MB_Data(string[] message, int pos)
        {
            int modeS_rep = Convert.ToInt32(message[pos], 2);
            //if (modeS_rep < 0) { MB_Data = new string[modeS_rep]; BDS1 = new string[modeS_rep]; BDS2 = new string[modeS_rep]; }
            pos++;
            for (int i = 0; i < modeS_rep; i++)
            {
                //MB_Data[i] = String.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3], message[pos + 4], message[pos + 5], message[pos + 6]);
                //BDS1[1] = message[pos + 7].Substring(0, 4);
                //BDS2[1] = message[pos + 7].Substring(4, 4);
                pos +=8;
            }
            return pos;
        }

        // DATA ITEM I010/270, TARGET SIZE & ORIENTATION
        //public string Target_size_and_orientation;
        //public string LENGHT;
        ////Extension
        ////string FX_Target_Size_And_Orientation;
        //////First extension
        //public string ORIENTATION;
        ////Second extension
        //public string WIDTH;

        private int Compute_Target_Size_and_Orientation(string[] message, int pos)
        {
            //LENGHT = "Lenght:  " + Convert.ToString(Convert.ToInt32(message[pos].Substring(0, 7), 2)) + "m";
            //Target_size_and_orientation = LENGHT;
            pos = pos++;
            if (message[pos - 1].Substring(7, 1) == "1")
            {
                //ORIENTATION = "Orientation: " + Convert.ToString(Convert.ToDouble(Convert.ToInt32(message[pos].Substring(0, 7), 2)) * (360 / 128)) + "°";
                //Target_size_and_orientation = Target_size_and_orientation + ", " + ORIENTATION;
                pos = pos++;
                if (message[pos - 1].Substring(7, 1) == "1")
                {
                    //WIDTH = "Widht: " + Convert.ToString(Convert.ToInt32(message[pos].Substring(0, 7), 2)) + "m";
                    //Target_size_and_orientation = Target_size_and_orientation + ", " + WIDTH;
                    pos = pos++;
                }
            }
            return pos;
        }

        // DATA ITEM I010/280, PRESENCE
        public int REP_Presence=0;
        //public string[] DRHO;
        //public string[] DTHETA;
        private int Compute_Presence(string[] message, int pos)
        {
            REP_Presence = Convert.ToInt32(string.Concat(message[pos]), 2);
            pos++;
            for (int i = 0; i < REP_Presence; i++)
            {
                //DRHO[i] = Convert.ToString(Convert.ToInt32(message[pos],2))+"m";
                //DTHETA[i] = Convert.ToString(Convert.ToDouble(Convert.ToInt32(message[pos+1], 2))*0.15)+ "º";
                pos += 2;
            }
            return pos;
        }

        // DATA ITEM I010/300, VEHICLE FLEET IDENTIFICATION
        //public string VFI;
        private int Compute_Vehicle_Fleet_Identificatior(string[] message, int pos)
        {
            int vfi = Convert.ToInt32(message[pos], 2);
            //if (vfi == 0) { VFI = "Unknown"; }
            //else if (vfi == 1) { VFI = "ATC equipment maintenance"; }
            //else if (vfi == 2) { VFI = "Airport maintenance"; }
            //else if (vfi == 3) { VFI = "Fire"; }
            //else if (vfi == 4) { VFI = "Bird scarer"; }
            //else if (vfi == 5) { VFI = "Snow plough"; }
            //else if (vfi == 6) { VFI = "Runway sweeper"; }
            //else if (vfi == 7) { VFI = "Emergency"; }
            //else if (vfi == 8) { VFI = "Police"; }
            //else if (vfi == 9) { VFI = "Bus"; }
            //else if (vfi == 10) { VFI = "Tug (push/tow)"; }
            //else if (vfi == 11) { VFI = "Grass cutter"; }
            //else if (vfi == 12) { VFI = "Fuel"; }
            //else if (vfi == 13) { VFI = "Baggage"; }
            //else if (vfi == 14) { VFI = "Catering"; }
            //else if (vfi == 15) { VFI = "Aircraft maintenance"; }
            //else if (vfi == 16) { VFI = "Flyco (follow me)"; }
            pos = pos++;
            return pos;
        }

        // DATA ITEM I010/310, PRE-PROGRAMMED MESSAGE
        //public string TRB;
        //public string MSG;
        //public string Pre_programmed_message;
        private int Compute_Preprogrammed_Message(string[] message, int pos)
        {
            //char[] OctetoChar = message[pos].ToCharArray(0, 8);
            //if (OctetoChar[0] == '0') { TRB = "Trouble: Default"; }
            //else if (OctetoChar[0] == '1') { TRB = "Trouble: In Trouble"; }
            //int msg = Convert.ToInt32(message[pos].Substring(1, 7), 2);
            //if (msg == 1) { MSG = "Message: Towing aircraft"; }
            //else if (msg == 2) { MSG = "Message: “Follow me” operation"; }
            //else if (msg == 3) { MSG = "Message: Runway check"; }
            //else if (msg == 4) { MSG = "Message: Emergency operation (fire, medical…)"; }
            //else if (msg == 5) { MSG = "Message: Work in progress (maintenance, birds scarer, sweepers…)"; }
            pos++;
            //Pre_programmed_message = TRB + " " + MSG;
            return pos;
        }

        //// DATA ITEM I010/500, STANDARD DERIVATION OF POSITION
        //public string Deviation_X;
        //public string Deviation_Y;
        //public string Covariance_XY;
        private int Compute_Standard_Deviation_of_Position(string[] message, int pos)
        {
            //Deviation_X = "Standard Deviation of X component (σx):" + Convert.ToString(Convert.ToDouble(Convert.ToInt32(message[pos], 2)) * 0.25) + "m";
            //Deviation_Y = "Standard Deviation of Y component (σy): " + Convert.ToString(Convert.ToDouble(Convert.ToInt32(message[pos + 1], 2)) * 0.25) + "m";
            //Covariance_XY = "Covariance (σxy): " + Convert.ToString(lib.ComputeA2Complement(string.Concat(message[pos + 2], message[pos + 3])) * 0.25) + "m^2";
            pos += 4;
            return pos;
        }


        private int Compute_System_Status(string[] message, int pos)
        {
            pos = pos++;
            return pos;
        }

    }
}
