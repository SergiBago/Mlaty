using GMap.NET;
using MultiCAT6.Utils;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace PGTAWPF
{
    public class CAT21vs21
    {
     
        readonly string FSPEC1;
        readonly string[] mensaje;
        public string CAT ="21 v. 2.1";
        public int num;
        public int cat21v21num;
        public int airportCode;
        public bool used = false;
        public double Time_milisec;
        public bool time = false;
        public int zone=-1;
        public bool saved = false;
        double firsttime;
        public CAT21vs21() { }
        public CAT21vs21(string[] mensajehexa, double firsttime)
        {
            try
            {
                
                this.firsttime = firsttime;
                this.mensaje = mensajehexa;//LibreriaDecodificacion.passarmensajeenteroabinario(mensajehexa);
                FSPEC1 = LibreriaDecodificacion.FSPEC(mensaje);
                int longFSPEC = this.FSPEC1.Length / 7;
                int pos = 3 + longFSPEC;
                char[] FSPEC = FSPEC1.ToCharArray(0, FSPEC1.Length);
                this.mensaje = LibreriaDecodificacion.Passarmensajeenteroabinario(mensaje);
                //  this.airportCode = airportCode;
                if (FSPEC[0] == '1') { pos = this.Compute_Data_Source_Identification(mensaje, pos); }
                if (FSPEC[1] == '1') { pos = this.Compute_Target_Report_Descripter(mensaje, pos); }
                if (FSPEC[2] == '1') { pos = this.Compute_Track_Number(mensaje, pos); }
                if (FSPEC[3] == '1') { pos++; }
                if (FSPEC[4] == '1') { pos = this.Compute_Time_of_Aplicabillity_Position(mensaje, pos); } //1
                if (FSPEC[5] == '1') { pos = this.Compute_PositionWGS_84(mensaje, pos); }
                if (FSPEC[6] == '1') { pos = this.Compute_High_Resolution_PositionWGS_84(mensaje, pos); }
                if (FSPEC.Count() > 8)
                {
                    if (FSPEC[7] == '1') { pos = this.Compute_Time_of_Aplicabillity_Velocity(mensaje, pos); }
                    if (FSPEC[8] == '1') { pos = this.Compute_Air_Speed(mensaje, pos); }
                    if (FSPEC[9] == '1') { pos = this.Compute_True_Air_Speed(mensaje, pos); }
                    if (FSPEC[10] == '1') { pos = this.Compute_Target_Address(mensaje, pos); }
                    if (FSPEC[11] == '1') { pos = this.Compute_Time_of_Message_Reception_Position(mensaje, pos); } //3
                    if (FSPEC[12] == '1') { pos = this.Compute_Time_of_Message_Reception_Position_High_Precision(mensaje, pos); } //2
                    if (FSPEC[13] == '1') { pos = this.Compute_Time_of_Message_Reception_Velocity(mensaje, pos); }
                }
                if (FSPEC.Count() > 16)
                {
                    if (FSPEC[14] == '1') { pos = this.Compute_Time_of_Message_Reception_Velocity_High_Precision(mensaje, pos); }
                    if (FSPEC[15] == '1') { pos = this.Compute_Geometric_Height(mensaje,pos); }
                    if (FSPEC[16] == '1') { pos = this.Compute_Quality_Indicators(mensaje, pos); }
                    if (FSPEC[17] == '1') { pos = this.Compute_MOPS_Version(mensaje, pos); }
                    if (MOPSversion == 2)
                    {
                        if (FSPEC[18] == '1') { pos += 2; }
                        if (FSPEC[19] == '1') { pos += 2; }
                        if (FSPEC[20] == '1') { pos = this.Compute_Flight_level(mensaje,pos); }
                    }
                }
                if (FSPEC.Count() > 22 && MOPSversion==2)
                {
                    if (FSPEC[21] == '1') { pos += 2; }
                    if (FSPEC[22] == '1') { pos++; }
                    if (FSPEC[23] == '1') { pos += 2; }
                    if (FSPEC[24] == '1') { pos += 2; }
                    if (FSPEC[25] == '1') { pos = this.Compute_Airborne_Ground_Vector(mensaje, pos); }
                    if (FSPEC[26] == '1') { pos += 2; }
                    if (FSPEC[27] == '1') { pos = this.Compute_Time_of_Asterix_Report_Transmission(mensaje, pos); } //4
                }
                if (FSPEC.Count() > 29 && MOPSversion == 2)
                {
                    if (FSPEC[28] == '1') { pos = this.Compute_Target_Identification(mensaje, pos); }
                    if (FSPEC[29] == '1') { pos = this.Compute_Emitter_Category(mensaje, pos); }
                    if (FSPEC[30] == '1') { pos = this.Compute_Met_Information(mensaje, pos); }
                    if (FSPEC[31] == '1') { pos += 2; }
                    if (FSPEC[32] == '1') { pos += 2; }
                    if (FSPEC[33] == '1') { pos = this.Compute_Trajectory_Intent(mensaje, pos); }
                    if (FSPEC[34] == '1') { pos++; }

                }
                if (FSPEC.Count() > 36 && MOPSversion == 2)
                {
                    if (FSPEC[35] == '1') { pos++; }
                    if (FSPEC[36] == '1') { pos = this.Compute_Surface_Capabiliteies_and_Characteristics(mensaje, pos); }
                    if (FSPEC[37] == '1') { pos++; }
                    if (FSPEC[38] == '1') { pos = this.Compute_Mode_S_MB_DATA(mensaje, pos); }
                    if (FSPEC[39] == '1') { pos += 7; }
                    if (FSPEC[40] == '1') { pos++; }
                    if (FSPEC[41] == '1') { pos = this.Compute_Data_Age(mensaje, pos); }
                }
                if (MOPSversion==2) 
                {
                    this.GetTime();
                   ComputeCartesianFromWGS84();
                  //  this.ComputeZone();
                }         
                  //  LibreriaDecodificacion = null;

            }
            catch
            {
                mensaje = mensajehexa;
            }
        }



        public void Setnum(int num)
        {
            this.num = num;
        }

        /// COMPUTE CLASSES
        /// classes to compute the parameters


        //AIRCRAFT OPERATIONAL STATUS
        //public string RA;
        //public string TC;
        //public string TS;
        //public string ARV;
        //public string CDTIA;
        //public string Not_TCAS;
        //public string SA;
        private int Compute_Aircraft_Operational_Status(string[] message, int pos)
        {
            //char[] OctetoChar = message[pos].ToCharArray(0, 8);
            //if (OctetoChar[0] == '1') { RA = "TCAS RA active"; }
            //else { RA = "TCAS II or ACAS RA not active"; }
            //if (Convert.ToInt32(string.Concat(OctetoChar[1], OctetoChar[2]), 2) == 1) { TC = "No capability for trajectory Change Reports"; }
            //else if (Convert.ToInt32(string.Concat(OctetoChar[1], OctetoChar[2]), 2) == 2) { TC = "Support fot TC+0 reports only"; }
            //else if (Convert.ToInt32(string.Concat(OctetoChar[1], OctetoChar[2]), 2) == 3) { TC = "Support for multiple TC reports"; }
            //else { TC = "Reserved"; }
            //if (OctetoChar[3] == '0') { TS = "No capability to support Target State Reports"; }
            //else { TS = "Capable of supporting target State Reports"; }
            //if (OctetoChar[4] == '0') { ARV = "No capability to generate ARV-Reports"; }
            //else { ARV = "Capable of generate ARV-Reports"; };
            //if (OctetoChar[5] == '0') { CDTIA = "CDTI not operational"; }
            //else { CDTIA = "CDTI operational"; }
            //if (OctetoChar[6] == '0') { Not_TCAS = "TCAS operational"; }
            //else { Not_TCAS = "TCAS not operational"; }
            //if (OctetoChar[7] == '0') { SA = "Antenna Diversity"; }
            //else { SA = "Single Antenna only"; }
            pos++;
            return pos;
        }


        /// DATA SOURCE IDENTIFICATION
        public string SAC;
        public string SIC;
        private int Compute_Data_Source_Identification(string[] message, int pos)
        {
            SAC = Convert.ToString(Convert.ToInt32(message[pos],2));
            SIC = Convert.ToString(Convert.ToInt32(message[pos+1],2));
            this.airportCode = LibreriaDecodificacion.GetAirporteCode(Convert.ToInt32(SIC));

            pos += 2;
            return pos;
        }


        //SERVICE IDENTIFICATION
        //public string Service_Identification;
        private int Compute_Service_Identification(string[] message, int pos) 
        { 
            //Service_Identification = Convert.ToString(Convert.ToInt32(message[pos],2));
            pos++;
            return pos;
        }


        //SERVICE MANAGEMENT
        //public string RP;
        private int Compute_Service_Managment(string[] message, int pos) 
        { 
            //RP = Convert.ToString(Convert.ToDouble(Convert.ToInt32(message[pos], 2)) * 0.5) + " sec";
            pos++;
            return pos; 
        }


        //EMITTER CATEGORY
       public string ECAT;
        private int Compute_Emitter_Category(string[] message, int pos)
        {
            int ecat = Convert.ToInt32(message[pos], 2);
            if (Target_Identification == "7777XBEG") { ECAT = "No ADS-B Emitter Category Information"; }
            else
            {
                if (ecat == 0) { ECAT = "No ADS-B Emitter Category Information"; }
                if (ecat == 1) { ECAT = "Light aircraft"; }
                if (ecat == 2) { ECAT = "Small aircraft"; }
                if (ecat == 3) { ECAT = "Medium aircraft"; }
                if (ecat == 4) { ECAT = "High Vortex Large"; }
                if (ecat == 5) { ECAT = "Heavy aircraft"; }
                if (ecat == 6) { ECAT = "Highly manoeuvrable(5g acceleration capability) and high speed(> 400 knots cruise)"; }
                if (ecat == 7 || ecat == 8 || ecat == 9) { ECAT = "Reserved"; }
                if (ecat == 10) { ECAT = "Rotocraft"; }
                if (ecat == 11) { ECAT = "Glider / Sailplane"; }
                if (ecat == 12) { ECAT = "Lighter than air"; }
                if (ecat == 13) { ECAT = "Unmanned Aerial Vehicle"; }
                if (ecat == 14) { ECAT = "Space / Transatmospheric Vehicle"; }
                if (ecat == 15) { ECAT = "Ultralight / Handglider / Paraglider"; }
                if (ecat == 16) { ECAT = "Parachutist / Skydiver"; }
                if (ecat == 17 || ecat == 18 || ecat == 19) { ECAT = "Reserved"; }
                if (ecat == 20) { ECAT = "Surface emergency vehicle"; }
                if (ecat == 21) { ECAT = "Surface service vehicle"; }
                if (ecat == 22) { ECAT = "Fixed ground or tethered obstruction"; }
                if (ecat == 23) { ECAT = "Cluster obstacle"; }
                if (ecat == 24) { ECAT = "Line obstacle"; }
            }
            pos++;
            return pos;
        }


        ////TARGET REPORT DESCRIPTOR
        //public string ATP;
        //public string ARC;
        //public string RC;
        //public string RAB;
        ////First Extension
        //public string DCR;
        //public string GBS;
        //public string SIM;
        //public string TST;
        //public string SAA;
        //public string CL;
        ////Second Extension
        //public string IPC;
        //public string NOGO;
        //public string CPR;
        //public string LDPJ;
        //public string RCF;
        //public string FX;
        public int GroundBit; //0 air, 1 land
        private int Compute_Target_Report_Descripter(string[] message, int pos)
        {
            char[] OctetoChar = message[pos].ToCharArray(0, 8);
            //int atp = Convert.ToInt32(string.Concat(OctetoChar[0], OctetoChar[1], OctetoChar[2]), 2);
            //int arc = Convert.ToInt32(string.Concat(OctetoChar[3], OctetoChar[4]), 2);
            //if (atp == 0) { ATP = "24-Bit ICAO address"; }
            //else if (atp == 1) { ATP = "Duplicate address"; }
            //else if (atp == 2) { ATP = "Surface vehicle address"; }
            //else if (atp == 3) { ATP = "Anonymous address"; }
            //else { ATP = "Reserved for future use"; }
            //if (arc == 0) { ARC = "25 ft "; }
            //else if (arc == 1) { ARC = "100 ft"; }
            //else if (arc == 2) { ARC = "Unknown"; }
            //else { ARC = "Invalid"; }
            //if (OctetoChar[5] == '0') { RC = "Default"; }
            //else { RC = "Range Check passed, CPR Validation pending"; }
            //if (OctetoChar[6] == '0') { RAB = "Report from target transponder"; }
            //else { RAB = "Report from field monitor (fixed transponder)"; }
            pos ++;
            if (OctetoChar[7] == '1')
            {
                OctetoChar = message[pos].ToCharArray(0, 8);
                //if (OctetoChar[0] == '0') { DCR = "No differential correction (ADS-B)"; }
                //else { DCR = "Differential correction (ADS-B)"; }
                if (OctetoChar[1] == '0') { GroundBit = 0; }// "Ground Bit not set"; }
                else { GroundBit = 1; }// "Ground Bit set"; }
                //if (OctetoChar[2] == '0') { SIM = "Actual target report"; }
                //else { SIM = "Simulated target report"; }
                //if (OctetoChar[3] == '0') { TST = "Default"; }
                //else { TST = "Test Target"; }
                //if (OctetoChar[4] == '0') { SAA = "Equipment capable to provide Selected Altitude"; }
                //else { SAA = "Equipment not capable to provide Selected Altitude"; }
                //int cl = Convert.ToInt32(string.Concat(OctetoChar[5], OctetoChar[6]), 2);
                //if (cl == 0) { CL = "Report valid"; }
                //else if (cl == 1) { CL = "Report suspect"; }
                //else if (cl == 2) { CL = "No information"; }
                //else { CL = "Reserved for future use"; }
                pos ++;
                if (OctetoChar[7] == '1')
                {
                    //OctetoChar = message[pos].ToCharArray(0, 8);
                    //if (OctetoChar[2] == '0') { IPC = "Default"; }
                    //else { IPC = "Independent Position Check failed"; }
                    //if (OctetoChar[3] == '0') { NOGO = "NOGO-bit not set"; }
                    //else { NOGO = "NOGO-bit set"; }
                    //if (OctetoChar[4] == '0') { CPR = "CPR Validation correct "; }
                    //else { CPR = "CPR Validation failed"; }
                    //if (OctetoChar[5] == '0') { LDPJ = "LDPJ not detected"; }
                    //else { LDPJ = "LDPJ detected"; }
                    //if (OctetoChar[6] == '0') { RCF = "Default"; }
                    //else { RCF = "Range Check failed "; }
                    pos ++;
                }
            }
            return pos;
        }


        //MODE A3 CODE IN OCTAL REPRESENTATION
        //public string ModeA3;
        private int Compute_Mode_A3(string[] message, int pos) 
        {
            //ModeA3 = Convert.ToString(LibreriaDecodificacion.ConvertDecimalToOctal(Convert.ToInt32(string.Concat(message[pos], message[pos + 1]).Substring(4,12),2))).PadLeft(4,'0');
            pos += 2;
            return pos; 
        }

 

        //TIME OF APPLICABILITY FOR POSITION
        public double Time_of_Applicability_Position=-1;
        private int Compute_Time_of_Aplicabillity_Position(string[] message, int pos)
        {
           // MessageBox.Show("Entered");
            int str = Convert.ToInt32(string.Concat(message[pos], message[pos + 1], message[pos + 2]), 2);

            Time_of_Applicability_Position = (Convert.ToDouble(str) / 128);
            if (Time_of_Applicability_Position<firsttime)
            {
                Time_of_Applicability_Position += 86400; 
            }
           // Time_of_day_sec = Convert.ToInt32(Math.Truncate(segundos));
          //  TimeSpan tiempo = TimeSpan.FromSeconds(segundos);
           // Time_of_Applicability_Position = tiempo.ToString(@"hh\:mm\:ss\:fff");
            pos += 3;
            return pos;
        }


        //TIME OF APPLICABILITY FOR VELOICITY
        public string Time_of_Applicability_Velocity;
        private int Compute_Time_of_Aplicabillity_Velocity(string[] message, int pos)
        {
            int str = Convert.ToInt32(string.Concat(message[pos], message[pos + 1], message[pos + 2]), 2);
            double segundos = (Convert.ToDouble(str) / 128);
            TimeSpan tiempo = TimeSpan.FromSeconds(segundos);
            Time_of_Applicability_Velocity = tiempo.ToString(@"hh\:mm\:ss\:fff");
            pos += 3;
            return pos;
        }


        //TIME OF MESSAGE RECEPTION FOR POSITION
        public double Time_of_Message_Reception_Position =-1;
        private int Compute_Time_of_Message_Reception_Position(string[] message, int pos)
        {
            int str = Convert.ToInt32(string.Concat(message[pos], message[pos + 1], message[pos + 2]), 2);
            Time_of_Message_Reception_Position = (Convert.ToDouble(str) / 128);
            //TimeSpan tiempo = TimeSpan.FromSeconds(segundos);
            //Time_of_Message_Reception_Position = tiempo.ToString(@"hh\:mm\:ss\:fff");
            pos +=3;
            return pos;
        }


        //TIME OF MESSAGE RECEPTION FOR POSITION HIGH PRECISION
        public double Time_of_Message_Reception_Position_High_Precision=-1;
        private int Compute_Time_of_Message_Reception_Position_High_Precision(string[] message, int pos)
        {
            string octet = string.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3]);
            string FSI = octet.Substring(0, 2);
            string time = octet.Substring(2, 30);
            int str = Convert.ToInt32(time, 2);
            double sec = (Convert.ToDouble(str)) * Math.Pow(2, -30);
            if (FSI == "10") { sec--; }
            if (FSI == "01") { sec++; }
            Time_of_Message_Reception_Position_High_Precision =sec;
            pos += 4;
            return pos;
        }


        //TIME OF MESSAGE RECEPTION FOR VELOCITY
        public string Time_of_Message_Reception_Velocity;
        private int Compute_Time_of_Message_Reception_Velocity(string[] message, int pos)
        {
            int str = Convert.ToInt32(string.Concat(message[pos], message[pos + 1], message[pos + 2]), 2);
            double segundos = (Convert.ToDouble(str) / 128);
            TimeSpan tiempo = TimeSpan.FromSeconds(segundos);
            Time_of_Message_Reception_Velocity = tiempo.ToString(@"hh\:mm\:ss\:fff");
            pos +=  3;
            return pos;
        }


        //TIME OF MESSAGE RECEPTION FOR VELOCITY HIGH PRECISION
        public string Time_of_Message_Reception_Velocity_High_Precision;
        private int Compute_Time_of_Message_Reception_Velocity_High_Precision(string[] message, int pos)
        {
            string octet = string.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3]);
            string FSI = octet.Substring(0, 2);
            string time = octet.Substring(2, 30);
            int str = Convert.ToInt32(time, 2);
            double sec = (Convert.ToDouble(str)) * Math.Pow(2, -30);
            if (FSI == "10") { sec--; }
            if (FSI == "01") { sec++; }
            Time_of_Message_Reception_Velocity_High_Precision = Convert.ToString(sec) + " sec";
            pos +=4;
            return pos;
        }


        //TIME OF ASTERIX REPORT TRANSMISSION
        public string Time_of_Asterix_Report_Transmission;
        public double Time_of_day_sec;
        public double time_milisec;
        private int Compute_Time_of_Asterix_Report_Transmission(string[] message, int pos)
        {
            
            int str = Convert.ToInt32(string.Concat(message[pos], message[pos + 1], message[pos + 2]), 2);
            double segundos = (Convert.ToDouble(str) / 128);
            time_milisec = segundos;
           // if (time_milisec < FirstTime) { time_milisec += 86400; }
           // Time_of_day_sec = Convert.ToInt32(Math.Truncate(segundos));
            TimeSpan tiempo = TimeSpan.FromSeconds(segundos);
            Time_of_Asterix_Report_Transmission = tiempo.ToString(@"hh\:mm\:ss\:fff")+ " " + Convert.ToString(Time_of_day_sec);
            pos += 3;
            return pos;
        }


        //TARGET ADDRESS
        public string Target_address;
        private int Compute_Target_Address(string[] message, int pos) { Target_address = string.Concat(LibreriaDecodificacion.BinarytoHexa(message[pos]), LibreriaDecodificacion.BinarytoHexa(message[pos + 1]), LibreriaDecodificacion.BinarytoHexa(message[pos + 2])); pos += 3; return pos; }


        //QUALITY INDICATORS
        public string Quality_Indicators;
        public string NUCr_NACv;
        public string NUCp_NIC;
        //First extension
        public string NICbaro;
        public string SIL;
        public int NACp;
        //Second extension
        public string SILS;
        public string SDA;
        public string GVA;
        //Third extension
        public int PIC;
        public string ICB;
        public string NUCp;
        public string NIC;
        public double NAC=-1;

        private int Compute_Quality_Indicators(string[] message, int pos)
        {
            NUCr_NACv = Convert.ToString(Convert.ToInt32(message[pos].Substring(0, 3), 2));
            NUCp_NIC = Convert.ToString(Convert.ToInt32(message[pos].Substring(3, 4), 2));
            pos++;
            if (message[pos-1].Substring(7, 1) == "1")
            {
                
                NICbaro = Convert.ToString(Convert.ToInt32(message[pos].Substring(0, 1), 2));
                SIL = Convert.ToString(Convert.ToInt32(message[pos].Substring(1, 2), 2));
                NACp = Convert.ToInt32(message[pos].Substring(3, 4), 2);
                ComputeNACp(NACp);
                pos++;
                if (message[pos-1].Substring(7, 1) == "1")
                {
                    
                    if (message[pos].Substring(2, 1) == "0") { SILS = "Measured per flight-Hour"; }
                    else { SILS = "Measured per sample"; }
                    SDA = Convert.ToString(Convert.ToInt32(message[pos].Substring(3, 2), 2));
                    GVA = Convert.ToString(Convert.ToInt32(message[pos].Substring(5, 2), 2));
                    pos++;
                    if (message[pos-1].Substring(7, 1) == "1")
                    {
                        
                        PIC = Convert.ToInt32(message[pos].Substring(0, 4), 2);
                        if (PIC == 0) { ICB = "No integrity(or > 20.0 NM)"; NUCp = "0"; NIC = "0"; }
                        if (PIC == 1) { ICB = "< 20.0 NM"; NUCp = "1"; NIC = "1"; }
                        if (PIC == 2) { ICB = "< 10.0 NM"; NUCp = "2"; NIC = "-"; }
                        if (PIC == 3) { ICB = "< 8.0 NM"; NUCp = "-"; NIC = "2"; }
                        if (PIC == 4) { ICB = "< 4.0 NM"; NUCp = "-"; NIC = "3"; }
                        if (PIC == 5) { ICB = "< 2.0 NM"; NUCp = "3"; NIC = "4"; }
                        if (PIC == 6) { ICB = "< 1.0 NM"; NUCp = "4"; NIC = "5"; }
                        if (PIC == 7) { ICB = "< 0.6 NM"; NUCp = "-"; NIC = "6 (+ 1/1)"; }
                        if (PIC == 8) { ICB = "< 0.5 NM"; NUCp = "5"; NIC = "6 (+ 0/0)"; }
                        if (PIC == 9) { ICB = "< 0.3 NM"; NUCp = "-"; NIC = "6 (+ 0/1)"; }
                        if (PIC == 10) { ICB = "< 0.2 NM"; NUCp = "6"; NIC = "7"; }
                        if (PIC == 11) { ICB = "< 0.1 NM"; NUCp = "7"; NIC = "8"; }
                        if (PIC == 12) { ICB = "< 0.04 NM"; NUCp = ""; NIC = "9"; }
                        if (PIC == 13) { ICB = "< 0.013 NM"; NUCp = "8"; NIC = "10"; }
                        if (PIC == 14) { ICB = "< 0.004 NM"; NUCp = "9"; NIC = "11"; }
                        pos++;
                    }
                }
            }
            return pos;
        }


        private void ComputeNACp(int NACp)
        {
            if (NACp<0 ||  NACp>11)
            { NAC = -1; }
            if (NACp==0) {NAC=18520; }
            else if (NACp == 0) { NAC =18520; }
            else if (NACp == 1) { NAC =18520; }
            else if (NACp == 2) { NAC =7408; }
            else if (NACp == 3) { NAC =3704; }
            else if (NACp == 4) { NAC =1852; }
            else if (NACp == 5) { NAC =929; }
            else if (NACp == 6) { NAC =555.6; }
            else if (NACp == 7) { NAC =185.2; }
            else if (NACp == 8) { NAC =92.6; }
            else if (NACp == 9) { NAC =30; }
            else if (NACp == 10) { NAC =10; }
            else if (NACp == 11) { NAC =3; }
        }

        ////TRAJECTORY INTENT
        public int Trajectory_present = 0;
        public bool subfield1;
        public bool subfield2;
        ////Subfield 1
        //public string NAV;
        //public string NVB;
        ////Subfield 2
        //public int REP;
        //public string[] TCA;
        //public string[] NC;
        //public int[] TCP;
        //public string[] Altitude;
        //public string[] Latitude;
        //public string[] Longitude;
        //public string[] Point_Type;
        //public string[] TD;
        //public string[] TRA;
        //public string[] TOA;
        //public string[] TOV;
        //public string[] TTR;
        private int Compute_Trajectory_Intent(string[] message, int pos)
        {
            Trajectory_present = 1;
            if (message[pos].Substring(0, 1) == "1") { subfield1 = true; }
            else { subfield1 = false; }
            if (message[pos].Substring(1, 1) == "1") { subfield2 = true; }
            else { subfield2 = false; }
            if (subfield1 == true)
            {
                pos++;
                //if (message[pos].Substring(0, 1) == "0") { NAV = "Trajectory Intent Data is available for this aircraft"; }
                //else { NAV = "Trajectory Intent Data is not available for this aircraft "; }
                //if (message[pos].Substring(1, 1) == "0") { NVB = "Trajectory Intent Data is valid"; }
                //else { NVB = "Trajectory Intent Data is not valid"; }
            }
            if (subfield2 == true)
            {
                pos++;
                
                int REP = Convert.ToInt32(message[pos], 2);
                //TCA = new string[REP];
                //NC = new string[REP];
                //TCP = new int[REP];
                //Altitude = new string[REP];
                //Latitude = new string[REP];
                //Longitude = new string[REP];
                //Point_Type = new string[REP];
                //TD = new string[REP];
                //TRA = new string[REP];
                //TOA = new string[REP];
                //TOV = new string[REP];
                //TTR = new string[REP];
                pos++;

                for (int i = 0; i < REP; i++)
                {
                    //if (message[pos].Substring(0, 1) == "0") { TCA[i] = "TCP number available"; }
                    //else { TCA[i] = "TCP number not available"; }
                    //if (message[pos].Substring(1, 1) == "0") { NC[i] = "TCP compliance"; }
                    //else { NC[i] = "TCP non-compliance"; }
                    //TCP[i] = Convert.ToInt32(message[pos].Substring(2, 6));
                    pos++;
                    //Altitude[i] = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1])) * 10) + " ft";
                    pos += 2;
                    //Latitude[i] = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1])) * (180 / (Math.Pow(2, 23)))) + " deg";
                    pos +=2;
                    //Longitude[i] = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1])) * (180 / (Math.Pow(2, 23)))) + " deg";
                    pos += 2;
                    //int pt = Convert.ToInt32(message[pos].Substring(0, 4), 2);
                    //if (pt == 0) { Point_Type[i] = "Unknown"; }
                    //else if (pt == 1) { Point_Type[i] = "Fly by waypoint (LT) "; }
                    //else if (pt == 2) { Point_Type[i] = "Fly over waypoint (LT)"; }
                    //else if (pt == 3) { Point_Type[i] = "Hold pattern (LT)"; }
                    //else if (pt == 4) { Point_Type[i] = "Procedure hold (LT)"; }
                    //else if (pt == 5) { Point_Type[i] = "Procedure turn (LT)"; }
                    //else if (pt == 6) { Point_Type[i] = "RF leg (LT)"; }
                    //else if (pt == 7) { Point_Type[i] = "Top of climb (VT)"; }
                    //else if (pt == 8) { Point_Type[i] = "Top of descent (VT)"; }
                    //else if (pt == 9) { Point_Type[i] = "Start of level (VT)"; }
                    //else if (pt == 10) { Point_Type[i] = "Cross-over altitude (VT)"; }
                    //else { Point_Type[i] = "Transition altitude (VT)"; }
                    //string td = message[pos].Substring(4, 2);
                    //if (td == "00") { TD[i] = "N/A"; }
                    //else if (td == "01") { TD[i] = "Turn right"; }
                    //else if (td == "10") { TD[i] = "Turn left"; }
                    //else { TD[i] = "No turn"; }
                    //if (message[pos].Substring(6, 1) == "0") { TRA[i] = "TTR not available"; }
                    //else { TRA[i] = "TTR available"; }
                    //if (message[pos].Substring(7, 1) == "0") { TOA[i] = "TOV available"; }
                    //else { TOA[i] = "TOV not available"; }
                    pos++;
                    //TOV[i] = Convert.ToString(Convert.ToInt32(string.Concat(message[pos], message[pos + 1], message[pos + 2]), 2)) + " sec";
                    pos += 3;
                    //TTR[i] = Convert.ToString(Convert.ToInt32(string.Concat(message[pos], message[pos + 1]), 2) * 0.01) + " Nm";
                    pos += 2;
                }
            }
            return pos;
        }


        //POSITION IN WG-84 CO-ORDENATES
        public string LatitudeWGS_84;
        public string LongitudeWGS_84;
        public double LatitudeWGS_84_map=-200;
        public double LongitudeWGS_84_map=-200;
        private int Compute_PositionWGS_84(string[] message, int pos)
        {
            
            double Latitude  =LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1], message[pos + 2]))*(180/(Math.Pow(2,23))); pos += 3;
            double Longitude = LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1], message[pos + 2])) * (180 / (Math.Pow(2, 23)));
            LatitudeWGS_84_map = Convert.ToDouble(Latitude);
            LongitudeWGS_84_map = Convert.ToDouble(Longitude);
            int Latdegres = Convert.ToInt32(Math.Truncate(Latitude));
            int Latmin = Convert.ToInt32(Math.Truncate((Latitude - Latdegres) * 60));
            double Latsec = Math.Round(((Latitude - (Latdegres + (Convert.ToDouble(Latmin) / 60))) * 3600), 2);
            int Londegres = Convert.ToInt32(Math.Truncate(Longitude));
            int Lonmin = Convert.ToInt32(Math.Truncate((Longitude - Londegres) * 60));
            double Lonsec = Math.Round(((Longitude - (Londegres + (Convert.ToDouble(Lonmin) / 60))) * 3600), 2);
            LatitudeWGS_84 = Convert.ToString(Latdegres) + "º " + Convert.ToString(Latmin) + "' " + Convert.ToString(Latsec) + "''";
            LongitudeWGS_84 = Convert.ToString(Londegres) + "º" + Convert.ToString(Lonmin) + "' " + Convert.ToString(Lonsec) + "''";
            pos += 3;
            //ComputeZone();
            return pos;
        }


        //HIGH RESOLUTION POSITION IN WG-84 CO-ORDENATES
        public string High_Resolution_LatitudeWGS_84;
        public string High_Resolution_LongitudeWGS_84;
        private int Compute_High_Resolution_PositionWGS_84(string[] message, int pos)
        {
            
            double Latitude= LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3])) * (180 / (Math.Pow(2, 30))); pos +=  4;
            double Longitude= LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3])) * (180 / (Math.Pow(2, 30))); pos += 4;
            LatitudeWGS_84_map = Convert.ToDouble(Latitude);
            LongitudeWGS_84_map = Convert.ToDouble(Longitude);
            int Latdegres = Convert.ToInt32(Math.Truncate(Latitude));
            int Latmin = Convert.ToInt32(Math.Truncate((Latitude - Latdegres) * 60));
            double Latsec = Math.Round(((Latitude - (Latdegres + (Convert.ToDouble(Latmin) / 60))) * 3600), 5);
            int Londegres = Convert.ToInt32(Math.Truncate(Longitude));
            int Lonmin = Convert.ToInt32(Math.Truncate((Longitude - Londegres) * 60));
            double Lonsec = Math.Round(((Longitude - (Londegres + (Convert.ToDouble(Lonmin) / 60))) * 3600), 5);
            High_Resolution_LatitudeWGS_84 = Convert.ToString(Latdegres) + "º " + Convert.ToString(Latmin) + "' " + Convert.ToString(Latsec) + "''";
            High_Resolution_LongitudeWGS_84 = Convert.ToString(Londegres) + "º" + Convert.ToString(Lonmin) + "' " + Convert.ToString(Lonsec) + "''";
            
            return pos;
        }

        //private void ComputeZone()
        //{
        //   Point p= ComputeCartesianFromWGS84();
        //    zone = LibreriaDecodificacion.ComputeZone(p, GroundBit);
        //}

        public double X_Component_map = -99999;
        public double Y_Component_map = -99999;

        private void ComputeCartesianFromWGS84()
        {
            if (LatitudeWGS_84_map != -200 && LongitudeWGS_84_map != -200)
            {
                CoordinatesWGS84 ObjectCoordinates = new CoordinatesWGS84((Math.PI / 180) * this.LatitudeWGS_84_map, this.LongitudeWGS_84_map * (Math.PI / 180), Geometric_Height);
                CoordinatesWGS84 RadarCoordinates = new CoordinatesWGS84(41.2970767 * (Math.PI / 180), 2.07846278 * (Math.PI / 180), 53.321);
                GeoUtils geoUtils = new GeoUtils();
                CoordinatesXYZ MarkerCartesian = geoUtils.change_geodesic2system_cartesian(ObjectCoordinates, RadarCoordinates);
                geoUtils = null;
                X_Component_map = MarkerCartesian.X;
                Y_Component_map = MarkerCartesian.Y;
            }
        }

        //MESSAGE AMPLITUDE
        //public string Message_Amplitude;
        private int Compute_Message_Amplitude(string[] message, int pos) 
        {
            //Message_Amplitude = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(message[pos])) + " dBm";
            pos++; 
            return pos; 
        }


        //GEOMETRIC HEIGHT
        public double Geometric_Height=-999;
        private int Compute_Geometric_Height(string[] message, int pos) 
        {
            Geometric_Height = LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1])) * 6.25* 0.3048 ;
            pos += 2;
            return pos;
        }


        //FLIGHT LEVEL
        public double Flight_Level=-999;
        private int Compute_Flight_level(string[] message, int pos) 
        {
            Flight_Level = LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1])) * (0.25) * (0.3048 * 100);
            pos += 2; 
            return pos;
        }


        //SELECTED ALTITUDE
        //public string SAS;
        //public string Source;
        //public string Sel_Altitude;
        //public string Selected_Altitude;
        private int Compute_Selected_Altitude(string[] message, int pos)
        {
            
        //    string sou = message[pos].Substring(1, 2);
        //    if (sou == "00") { Source = "Unknown"; }
        //    else if (sou == "01") { Source = "Aircraft Altitude (Holding Altitude)"; }
        //    else if (sou == "10") { Source = "MCP/FCU Selected Altitude"; }
        //    else { Source = "FMS Selected Altitude"; }
        //    Sel_Altitude = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos+1]).Substring(3, 13)) * 25) + " ft";
        ////    if (message[pos].Substring(0, 1) == "0") { Selected_Altitude = "No source information provided. Altitude: "+Sel_Altitude; }
        //  //  else { Selected_Altitude = "Source: "+ Source+ " SA: "+ Sel_Altitude; }
        //    Selected_Altitude= "SA: "+ Convert.ToString(Sel_Altitude);
            pos += 2;
            return pos;
        }


        //FINAL STATE SELECTED ALTITUDE
        //public string MV;
        //public string AH;
        //public string AM;
        //public string Final_State_Altitude;
        private int Compute_Final_State_Selected_Altitude(string[] message, int pos)
        {
            //if (message[pos].Substring(0, 1) == "0") { MV = "Not active or unknown"; }
            //else { MV = "Active"; }
            //if (message[pos].Substring(1, 1) == "0") { AH = "Not active or unknown"; }
            //else { AH = "Active"; }
            //if (message[pos].Substring(2, 1) == "0") { AM = "Not active or unknown"; }
            //else { AM = "Active"; }
            //Final_State_Altitude = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos+1]).Substring(3, 13)) * 25) + " ft";
            pos += 2;
            return pos ;
        }


        //AIR SPEED
        //public string Air_Speed;
        private int Compute_Air_Speed(string[] message, int pos)
        {
            //if (message[pos].Substring(0, 1) == "0") { Air_Speed = Convert.ToString(Convert.ToInt32(string.Concat(message[pos], message[pos + 1]).Substring(1, 15),2) * Math.Pow(2, -14)) + " NM/s"; }
            //else { Air_Speed = Convert.ToString(Convert.ToInt32(string.Concat(message[pos], message[pos + 1]).Substring(1, 15),2) * 0.001) + " Mach"; }
            pos += 2;
            return pos;
        }


        //TRUE AIRSPEED
        //public string True_Air_Speed;
        private int Compute_True_Air_Speed(string[] message, int pos)
        {
            //if (message[pos].Substring(0, 1) == "0") { True_Air_Speed = Convert.ToString(Convert.ToInt32(string.Concat(message[pos], message[pos + 1]).Substring(1, 15),2)) + " Knots"; }
            //else { True_Air_Speed = "Value exceeds defined rage"; }
            pos += 2;
            return pos;
        }


        //MAGNETIC HEADING
        //public string Magnetic_Heading;
        private int Compute_Magnetic_Heading(string[] message, int pos) 
        {
            //Magnetic_Heading = Convert.ToString(Convert.ToInt32(string.Concat(message[pos], message[pos]), 2) * (360 / (Math.Pow(2, 16)))) + "º";
            pos += 2;  
            return pos;
        }


        //BAROMETRIC VERTICAL RATE
        //public string Barometric_Vertical_Rate;
        private int Compute_Barometric_Vertical_Rate(string[] message, int pos)
        {
            //if (message[pos].Substring(0, 1) == "0") { Barometric_Vertical_Rate = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1]).Substring(1, 15)) * 6.25) + " feet/minute"; }
            //else { Barometric_Vertical_Rate = "Value exceeds defined rage"; }
            pos += 2;
            return pos;
        }


        //GEOMETRIC VERTICAL RATE
        //public string Geometric_Vertical_Rate;
        private int Compute_Geometric_Vertical_Rate(string[] message, int pos)
        {
            //if (message[pos].Substring(0, 1) == "0") { Geometric_Vertical_Rate = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos], message[pos + 1]).Substring(1, 15)) * 6.25) + " feet/minute"; }
            //else { Geometric_Vertical_Rate = "Value exceeds defined rage"; }
            pos += 2;
            return pos;
        }


        //AIRBORNE GROUND VECTOR
        public double Ground_Speed { get; set; } = -1;
        public string Track_Angle;
        public string Ground_vector;
        private int Compute_Airborne_Ground_Vector(string[] message, int pos)
        {
            if (message[pos].Substring(0, 1) == "0")
            {
                Ground_Speed = (Convert.ToInt32(string.Concat(message[pos], message[pos + 1]).Substring(1, 15),2) * Math.Pow(2, -14)*3600* 0.514444444);
               // double meters = 
                Track_Angle = String.Format("{0:0.00}", Convert.ToInt32(string.Concat(message[pos + 2], message[pos + 3]).Substring(0, 16),2) * (360 / (Math.Pow(2, 16))));
                Ground_vector = "GS: " + Ground_Speed + ", T.A: "+ String.Format("{0:0.00}",Track_Angle)+"º";
            }
            else { Ground_vector= "Value exceeds defined rage"; }
            pos +=  4;
            return pos;
        }


        //TRACK NUMBER
        public int Track_Number=-1;
        private int Compute_Track_Number(string[] message, int pos) { Track_Number=Convert.ToInt32(string.Concat(message[pos],message[pos+1]).Substring(4,12),2); pos += 2; return pos;  }


        //TRACK ANGLE RATE
        //public string Track_Angle_Rate;
        private int Compute_Track_Angle_Rate(string[] message, int pos) 
        {
            //Track_Angle_Rate = Convert.ToString(Convert.ToInt32(string.Concat(message[pos], message[pos]).Substring(6, 10), 2)*(1/32))+" º/s";
            pos += 2;
            return pos;
        }


        //TARGET IDENTIFICATION
        public string Target_Identification;
        private int Compute_Target_Identification (string[] message, int pos)
        {
            StringBuilder Identification= new StringBuilder();
            string octets = string.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3], message[pos + 4], message[pos + 5]);
            for (int i=0; i<8;i++) {Identification.Append(LibreriaDecodificacion.Compute_Char(octets.Substring(i*6,6)));}
            string tar = Identification.ToString();
            if (tar.Length > 1) { Target_Identification = tar; }
            return pos + 6;
        }


        //TARGET STATUS
        
        //public string ICF;
        //public string LNAV;
        //public string PS;
        //public string SS;
        private int Compute_Target_Status(string[] message, int pos)
        {
            //if (message[pos].Substring(0, 1) == "0") { ICF = "No intent change active"; }
            //else {ICF= "Intent change flag raised"; }
            //if (message[pos].Substring(1, 1) == "0") { LNAV = "LNAV Mode engaged"; }
            //else { LNAV = "LNAV Mode not engaged"; }
            //int ps = Convert.ToInt32(message[pos].Substring(3,3), 2);
            //if (ps==0) { PS = "No emergency / not reported"; }
            //else if (ps == 1) { PS = "General emergency"; }
            //else if (ps == 2) { PS = "Lifeguard / medical emergency"; }
            //else if (ps == 3) { PS = "Minimum fuel"; }
            //else if (ps == 4) { PS = "No communications"; }
            //else if (ps == 5) { PS = "Unlawful interference"; }
            //else { PS = "'Downed' Aircraft "; }
            //int ss = Convert.ToInt32(message[pos].Substring(6, 2), 2);
            //if (ss == 0) { SS = "No condition reported"; }
            //else if (ss == 1) { SS = "Permanent Alert (Emergency condition)"; }
            //else if (ss == 2) { SS = "Temporary Alert (change in Mode 3/A Code other than emergency)"; }
            //else { SS = "SPI set"; }
            pos++;
            return pos;
        }


        //MOPS VERSION
        public string VNS;
        public string LTT;
        public string MOPS;
        public int MOPSversion;
        private int Compute_MOPS_Version(string[] message, int pos)
        {
            if (message[pos].Substring(1, 1) == "0") { VNS = "The MOPS Version is supported by the GS"; }
            else { VNS = "The MOPS Version is not supported by the GS"; }
            int ltt = Convert.ToInt32(message[pos].Substring(5, 3), 2);
            if (ltt == 0) { LTT = "Other"; }
            else if (ltt == 1) { LTT = "UAT"; }
            if (ltt == 2)
            {
                int vn = Convert.ToInt32(message[pos].Substring(2, 3), 2);
                string VN = "";
                if (vn == 0) { VN = "ED102/DO-260"; }
                if (vn == 1) { VN = "DO-260A"; }
                if (vn == 2) { MOPSversion=2; }
                LTT = "1090 ES, Version Number: " + VN;
                LTT = "Version Number: " + VN;
            }
            else if (ltt == 3) { LTT = "VDL 4"; }
            else { LTT = "Not assigned"; }
            MOPS = VNS + ". Link technology type: " + LTT;
            MOPS = LTT;
            pos++;
            return pos;
        }


        //MET INFORMATION
        //public int MET_present = 0;
        //public string Wind_Speed;
        //public string Wind_Direction;
        //public string Temperature;
        //public string Turbulence;
        private int Compute_Met_Information (string[] message, int pos)
        {
            //MET_present = 1;
            //int posin = pos;
            pos = pos++;
            //if (message[posin].Substring(0, 1) == "1") {Wind_Speed = Convert.ToString(Convert.ToInt32(string.Concat(message[posfin],message[posfin]), 2)) + " Knots"; posfin += 2;}
            //if (message[posin].Substring(1, 1) == "1") { Wind_Direction = Convert.ToString(Convert.ToInt32(string.Concat(message[posfin], message[posfin]), 2)) + " degrees"; posfin += 2; }
            //if (message[posin].Substring(2, 1) == "1") { Temperature = Convert.ToString(Convert.ToInt32(string.Concat(message[posfin], message[posfin]), 2)*0.25) + " ºC"; posfin += 2; }
            //if (message[posin].Substring(3, 1) == "1") { Turbulence = Convert.ToString(Convert.ToInt32(string.Concat(message[posfin], message[posfin]), 2)) + " Turbulence"; posfin+=2; }
            return pos;
        }


        //ROLL ANGLE
        //public string Roll_Angle;
        private int Compute_Roll_Angle(string[] message, int pos)
        {
            //Roll_Angle = Convert.ToString(LibreriaDecodificacion.ComputeA2Complement(string.Concat(message[pos],message[pos]))*0.01) + "º";
            pos++;
            return pos; 
        }


        //MODE S MB DATA

        //public string[] MB_Data;
        //public string[] BDS1;
        //public string[] BDS2;
        //public int modeS_rep;
        private int Compute_Mode_S_MB_DATA(string[] message, int pos)
        {
            int modeS_rep = Convert.ToInt32(message[pos], 2);
            //if (modeS_rep < 0) {MB_Data = new string[modeS_rep];BDS1 = new string[modeS_rep]; BDS2 = new string[modeS_rep]; }
            pos++;
            for (int i=0;i<modeS_rep;i++)
            {
                //MB_Data[i] = String.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3], message[pos + 4], message[pos + 5], message[pos + 6]);
                //BDS1[1] = message[pos + 7].Substring(0, 4);
                //BDS2[1] = message[pos + 7].Substring(4, 4);
                pos +=8;
            }
            return pos;
        }


        ////ACAS RESOLUTION ADVISORY REPORT
        //public string TYP;
        //public string STYP;
        //public string ARA;
        //public string RAC;
        //public string RAT;
        //public string MTE;
        //public string TTI;
        //public string TID;
        private int Compute_ACAS_Resolution_Advisory_Report(string[] message, int pos)
        {
            //string messg = string.Concat(message[pos], message[pos + 1], message[pos + 2], message[pos + 3], message[pos + 4], message[pos + 5], message[pos + 6]);
            //TYP = messg.Substring(0,5);
            //STYP = messg.Substring(5, 3);
            //ARA = messg.Substring(8, 14);
            //RAC = messg.Substring(22, 4);
            //RAT = messg.Substring(26, 1);
            //MTE = messg.Substring(27, 1);
            //TTI = messg.Substring(28, 2);
            //TID = messg.Substring(30, 26);
            pos +=7;
            return pos;
        }


        //SURFACE CAPABILITIES AND CHARACTERISTICS
        
        //public string POA;
        //public string CDTIS;
        //public string B2_low;
        //public string RAS;
        //public string IDENT;
        ////First extension
        ////int LaW;
        //public string LengthandWidth;
        private int Compute_Surface_Capabiliteies_and_Characteristics (string[] message, int pos)
        {
            
            //if (message[pos].Substring(2, 1) == "0") { POA = "Position transmitted is not ADS-B position reference point"; }
            //else { POA = "Position transmitted is the ADS-B position reference point"; }
            //if (message[pos].Substring(3, 1) == "0") { CDTIS = "Cockpit Display of Traffic Information not operational"; }
            //else { CDTIS = "Cockpit Display of Traffic Information operational"; }
            //if (message[pos].Substring(4, 1) == "0") { B2_low= "Class B2 transmit power ≥ 70 Watts"; }
            //else { B2_low= "Class B2 transmit power < 70 Watts"; }
            //if (message[pos].Substring(5, 1) == "0") { RAS = "Aircraft not receiving ATC-services"; }
            //else { RAS = "Aircraft receiving ATC services"; }
            //if (message[pos].Substring(6, 1) == "0") { IDENT = "IDENT switch not active"; }
            //else { IDENT = "IDENT switch active"; }
            if (message[pos].Substring(7, 1) == "1") 
            {
                pos++;
                //int LaW =Convert.ToInt32(message[pos].Substring(4,4),2) ; 
                //if ( LaW == 0) { LengthandWidth  = "Lenght < 15  and Width < 11.5";  }
                //if (LaW == 1) { LengthandWidth = "Lenght < 15  and Width < 23"; }
                //if (LaW == 2) { LengthandWidth = "Lenght < 25  and Width < 28.5"; }
                //if (LaW == 3) { LengthandWidth = "Lenght < 25  and Width < 34"; }
                //if (LaW == 4) { LengthandWidth = "Lenght < 35  and Width < 33"; }
                //if (LaW == 5) { LengthandWidth = "Lenght < 35  and Width < 38"; }
                //if (LaW == 6) { LengthandWidth = "Lenght < 45  and Width < 39.5"; }
                //if (LaW == 7) { LengthandWidth = "Lenght < 45  and Width < 45"; }
                //if (LaW == 8) { LengthandWidth = "Lenght < 55  and Width < 45"; }
                //if (LaW == 9) { LengthandWidth = "Lenght < 55  and Width < 52"; }
                //if (LaW == 10) { LengthandWidth = "Lenght < 65  and Width < 59.5"; }
                //if (LaW == 11) { LengthandWidth = "Lenght < 65  and Width < 67"; }
                //if (LaW == 12) { LengthandWidth = "Lenght < 75  and Width < 72.5"; }
                //if (LaW == 13) { LengthandWidth = "Lenght < 75  and Width < 80"; }
                //if (LaW == 14) { LengthandWidth = "Lenght < 85  and Width < 80"; }
                //if (LaW == 15) { LengthandWidth = "Lenght > 85  and Width > 80"; }
            }
            pos++;
            return pos;
        }


        //DATA AGES
        public int Data_Ages_present=0;
        //Octet 1
        public string AOS;
        public string TRD;
        public string M3A;
        public string QI;
        public string TI;
        public string MAM;
        public string GH;
        //Octet 2
        public string FL;
        public string ISA;
        public string FSA;
        public string AS;
        public string TAS;
        public string MH;
        public string BVR;
        //Octet 3
        public string GVR;
        public string GV;
        public string TAR;
        public string TI_DataAge;
        public string TS_DataAge;
        public string MET;
        public string ROA;
        //Octet 4
        public string ARA_DataAge;
        public string SCC;
        private int Compute_Data_Age(string[] message, int pos)
        {
            Data_Ages_present = 1;
            int posin = pos;
            if (message[pos].Substring(7, 1) == "1")
            {
                pos++;
                if (message[pos].Substring(7, 1) == "1")
                {
                    pos++;
                    if (message[pos].Substring(7, 1) == "1")
                    {
                        pos++;
                    }
                }
            }
            pos++;
            if (message[posin].Substring(0, 1) == "1") { AOS = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            if (message[posin].Substring(1, 1) == "1") { TRD = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            if (message[posin].Substring(2, 1) == "1") { M3A = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            if (message[posin].Substring(3, 1) == "1") { QI = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            if (message[posin].Substring(4, 1) == "1") { TI = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            if (message[posin].Substring(5, 1) == "1") { MAM = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            if (message[posin].Substring(6, 1) == "1") { GH = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            if (message[posin].Substring(7, 1) == "1")
            {
                if (message[posin + 1].Substring(0, 1) == "1") { FL = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 1].Substring(1, 1) == "1") { ISA = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 1].Substring(2, 1) == "1") { FSA = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 1].Substring(3, 1) == "1") { AS = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 1].Substring(4, 1) == "1") { TAS = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 1].Substring(5, 1) == "1") { MH = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 1].Substring(6, 1) == "1") { BVR = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            }
            if (message[posin+1].Substring(7, 1) == "1")
            {
                if (message[posin + 2].Substring(0, 1) == "1") { GVR = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 2].Substring(1, 1) == "1") { GV = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 2].Substring(2, 1) == "1") { TAR = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 2].Substring(3, 1) == "1") { TI_DataAge = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 2].Substring(4, 1) == "1") { TS_DataAge = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 2].Substring(5, 1) == "1") { MET = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 2].Substring(6, 1) == "1") { ROA = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            }
            if (message[posin+2].Substring(7, 1) == "1")
            {
                if (message[posin + 3].Substring(0, 1) == "1") { ARA_DataAge = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
                if (message[posin + 3].Substring(1, 1) == "1") { SCC = Convert.ToString(Convert.ToInt32(message[pos], 2) * 0.1) + " s"; pos++; }
            }
            return pos; 
        }


        //RECEIVER ID
        //public string Receiver_ID;
        private int Compute_Receiver_ID(string[] message, int pos) 
        {
            //Receiver_ID = Convert.ToString(Convert.ToInt32(message[pos],2));
            pos++;
            return pos;
        }

        private void GetTime()
        {
            if (Time_of_Applicability_Position != -1) //Data item I021/071
            { 
                Time_milisec = Time_of_Applicability_Position;
            }
            else if (Time_of_Message_Reception_Position_High_Precision!= -1)  //Data item I021/074
            {
                Time_milisec = Time_of_Message_Reception_Position_High_Precision; //
            }
            else if (Time_of_Message_Reception_Position != -1) //Data item I021/073
            {
                Time_milisec = Time_of_Message_Reception_Position; 
            }
            else
            {
                Time_milisec = time_milisec;
                time = true; 
            }
            if (Time_milisec < firsttime)
            {
                Time_milisec += 86400;
            }
        }
    }   

}
