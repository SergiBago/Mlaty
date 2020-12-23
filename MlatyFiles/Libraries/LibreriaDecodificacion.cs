using System;
using GMap.NET;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PGTA_WPF;
using System.Security.Policy;
using MultiCAT6.Utils;

namespace PGTAWPF
{
    public class LibreriaDecodificacion
    {
        Zonas zones = new Zonas();
       public List<string> ExcludedMLATS = new List<string>() { "342384", "342387", "342386", "342385", "342383", "3433D5", "341041","34104F", "341097", "341103", "34434B","344357", "344389" , "3443C6", "341042","341050","3410C9","341105","34434C","344358","34438A","3443C3","341043","341051","3410CA", "341107", "34434D", "344359", "34438B", "3443C4", "341044", "341052", "3410CE", "341108", "34434E", "34435A", "34438C", "3443C5", "341045", "341053", "3410D0", "341109", "34434F", "344381", "34438D", "3443C2", "341046", "341055", "3410D2" ,"34110A", "344350", "344382", "34438E", "3443C6", "341047", "341081", "3410D3", "34110B", "344351", "344383", "34438F", "3443C3", "341049", "341082", "3410D4", "34110F", "344352", "344384", "344390", "3443C4", "34104B", "341090", "3410D5", "344347", "344353", "344385", "3443C3", "3443C5", "34104C", "341092", "3410D6", "344348", "344354", "344386", "3443C4", "3443C2", "34104D", "341094", "3410D7", "344349", "344355", "344387", "3443C5", "3443C6", "34104E", "341095", "3410D8", "34434A", "344356", "344388", "3443C2" };

 

        readonly private Dictionary<char, string> HexadecimalAbinario = new Dictionary<char, string>
        {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'A', "1010" },
            { 'B', "1011" },
            { 'C', "1100" },
            { 'D', "1101" },
            { 'E', "1110" },
            { 'F', "1111" }
        };

        readonly private Dictionary<string, char> BinarioaHexadecimal = new Dictionary<string, char>
        {
            {"0000", '0' },
            {"0001", '1'},
            {"0010",'2' },
            {"0011",'3' },
            {"0100",'4' },
            {"0101",'5' },
            {"0110",'6' },
            {"0111",'7' },
            {"1000",'8' },
            {"1001",'9' },
            {"1010",'A' },
            {"1011",'B' },
            {"1100",'C' },
            {"1101",'D' },
            {"1110",'E' },
            {"1111",'F' }
        };

        public string HexaoctetoAbinario (string octeto)
        {
            octeto = this.Zerodelante(octeto);
            StringBuilder Octeto = new StringBuilder();
            foreach (char a in octeto)
            {
                Octeto.Append(HexadecimalAbinario[char.ToUpper(Convert.ToChar(a))]);
            }
             return Octeto.ToString();
        }

        public string BinarytoHexa(string octeto)
        {
            StringBuilder Octeto = new StringBuilder();
            Octeto.Append(BinarioaHexadecimal[octeto.Substring(0,4)]);
            Octeto.Append(BinarioaHexadecimal[octeto.Substring(4, 4)]);
            return Octeto.ToString();
        }

        public string Zerodelante (string octeto)
        {
            if (octeto.Length==1)
            {
                return string.Concat('0', octeto);
            }
            else
            {
                return octeto;
            }
        }
        public int Longitud (string[] mensaje )
        {
            string octetos = string.Concat(mensaje[1], mensaje[2]);
            return Convert.ToInt32(octetos, 2);
        }
        
        public string FSPEC (string [] message)
        {
            string FSPEC1 = "";
            int pos = 3;
            bool continua = true;
            while (continua == true)
            {
                string newocteto = Convert.ToString(Convert.ToInt32(message[pos], 16), 2).PadLeft(8, '0');
                FSPEC1 +=  newocteto.Substring(0,7);
                if (newocteto.Substring(7, 1) == "1") 
                    pos++;
                else 
                    continua = false;
            }
            return FSPEC1;
        }

        public string[] Passarmensajeenteroabinario(string[] mensaje)
        {
            string[] Mensajebinario = new string[mensaje.Length];
            for (int i=0; i<mensaje.Length; i++) {Mensajebinario[i] = this.HexaoctetoAbinario(mensaje[i]);}
            return Mensajebinario;
        }


        public double ComputeA2Complement(string bits)
        {
            if (Convert.ToString(bits[0]) == "0")
            {
                int num = Convert.ToInt32(bits, 2);
                return Convert.ToSingle(num);
            }
            else
            {
                string bitss = bits.Substring(1, bits.Length - 1);
                string newbits = "";
                int i = 0;
                while (i < bitss.Length)
                {
                    if (Convert.ToString(bitss[i]) == "1")
                        newbits +=  "0";
                    if (Convert.ToString(bitss[i]) == "0")
                        newbits +=  "1";
                    i++;
                }
                double num = Convert.ToInt32(newbits, 2);
                return -(num + 1);
            }

        }
        public int ConvertDecimalToOctal(int decimalNumber)
        {
            int octalNumber = 0, i = 1;

            while (decimalNumber != 0)
            {
                octalNumber += (decimalNumber % 8) * i;
                decimalNumber /= 8;
                i *= 10;
            }

            return octalNumber;
        }

        public string Compute_Char(string Char)
        {
            int code = Convert.ToInt32(Char, 2);
            List<string> codelist = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            if (code == 0)
                return "";
            else
                return codelist[code - 1];
        }

        public int GetVersion(string[] message)
        {
            string[] mensaje = message;
            string FSPEC1 = this.FSPEC(mensaje);
            int longFSPEC = FSPEC1.Length / 7;
            int pos = 3 + longFSPEC;
            mensaje = this.Passarmensajeenteroabinario(mensaje);
            int SAC = Convert.ToInt32(mensaje[pos], 2);
            int SIC = Convert.ToInt32(mensaje[pos + 1], 2);
            if (SAC == 0 && SIC != 101) { return 0; }
            else { return 1; }
        }






        public int GetAirporteCode(int SIC)
        {
            int i = 0;
            if (SIC==107 || SIC ==7 || SIC==219)  { i= 0; } //BARCELONA
            else if (SIC==5 || SIC== 105 || SIC==209) { i= 1; } //ASTURIAS
            else if (SIC==2 || SIC== 102 ) { i= 2; } //PALMA
            else if (SIC==6 || SIC==106 || SIC== 227 || SIC== 228) { i= 3; } //SANTIAGO
            else if (SIC==3 || SIC==4 || SIC==104) { i= 4; } //BARAJAS
            else if (SIC == 1 || SIC == 101 ) { i = 5; } //TENERIFE
            else if (SIC == 108) { i = 6; } //Malaga
            else if (SIC==203) { i=7; } //Bilbao
            else if (SIC == 206 ) { i=8; } //ALICANTE
            else if (SIC == 207) { i=9; } //GRANADA
            else if (SIC == 210) { i=10; } //LANZAROTE
            else if (SIC == 211) { i=11; } //TURRILLAS
            else if (SIC == 212) { i=12; } //Menorca
            else if (SIC == 213 || SIC==229) { i=13; } //IBIZA
            else if (SIC == 214 ) { i=14; } //VALDESPINA
            else if (SIC == 215 || SIC==221) { i=15; } //PARACUELLOS
            else if (SIC == 216) { i=16; } //RANDA
            else if (SIC == 218) { i=17; } //GERONA
            else if (SIC == 220 || SIC==222) { i=18; } //ESPIÑEIRAS
            else if (SIC == 223) { i=19; } //VEJER
            else if (SIC == 224) { i=20; } //YESTE
            else if (SIC == 225 || SIC==226) { i=21; } //VIGO
            else if (SIC == 230) { i=22; } //VALENCIA
            else if (SIC == 231) { i=23; } //SEVILLA
            return i;
        }


        public PointLatLng GetCoordenatesSMRMALT(int SIC)
        {
            PointLatLng point = new PointLatLng(0, 0);
            if (SIC == 1) { point = SMRTenerife; }
            else if (SIC == 2) { point = SMRPalma; }
            else if (SIC == 3) { point = SMRBarajas_S; }
            else if (SIC == 4) { point = SMRBarajas_N; }
            else if (SIC == 5) { point = SMRAsturias; }
            else if (SIC == 6) { point = SMRSantiago; }
            else if (SIC == 7) { point = SMRBarcelona; }
            else if (SIC == 101) { point = ARPTenerife; }
            else if (SIC == 102) { point = ARPPalma; }
            else if (SIC == 104) { point = ARPBarajas; }
            else if (SIC == 105) { point = ARPAsturias; }
            else if (SIC==106) { point = ARPSantiago; }
            else if (SIC == 107) { point = ARPBarcelona; }
            else if (SIC==108) { point = ARPMalaga; }
            return point;
        }


        public static bool IsPointInZone(List<Polygon> PolList, Point p)
        {
            bool isinside= false;
            foreach(Polygon pol in PolList)
            {
                if (IsPointInPolygon(pol.Points, p) == true) { isinside = true; }
            }
            return isinside;
        }

        public static bool IsPointInPolygon(List<Point> polygon, Point testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y || polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
                {
                    if (polygon[i].X + (testPoint.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) * (polygon[j].X - polygon[i].X) < testPoint.X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        //public int ComputeZone(PointLatLng p, int type) //Type1 =ground, type0= air, 2 unspecified
        //{
        //    Point Pxy = ComputeCartesianFromWGS84(p);
        //    int zone = ComputeZone(Pxy, 2);

        //}

        public int ComputeZone(Point p, int type) //Type1 =ground, type0= air, 2=unspecified 
        {
            int zone=-1;
            if (type != 2)
            {
                if (type == 0)
                {
                    if (IsPointInZone(zones.Airborne25RZones25, p) == true)
                    {
                        return 9;
                    }
                    if (IsPointInZone(zones.Airborne02Zones25, p) == true)
                    {
                        return 10;
                    }
                    if (IsPointInZone(zones.Airborne25LZones25, p) == true)
                    {
                        return 11;
                    }
                    if (IsPointInZone(zones.Airborne25RZones5, p) == true)
                    {
                        return 12;
                    }
                    if (IsPointInZone(zones.Airborne02Zones5, p) == true)
                    {
                        return 13;
                    }
                    if (IsPointInZone(zones.Airborne25LZones5, p) == true)
                    {
                        return 14;
                    }
                }
                if (type == 1 || type == 0)
                {

                    if (IsPointInZone(zones.Runway25LZones, p) == true)
                    {
                        return 1;
                    }
                    if (IsPointInZone(zones.Runway02Zones, p) == true)
                    {
                        return 2;
                    }
                    if (IsPointInZone(zones.Runway25RZones, p) == true)
                    {
                        return 3;
                    }
                }
                if (type == 1)
                {
                    if (IsPointInZone(zones.StandT1Zones, p) == true)
                    {
                        return 4;
                    }
                    if (IsPointInZone(zones.StandT2Zones, p) == true)
                    {
                        return 5;
                    }
                    if (IsPointInZone(zones.ApronT1Zones, p) == true)
                    {
                        return 6;
                    }
                    if (IsPointInZone(zones.ApronT2Zones, p) == true)
                    {
                        return 7;
                    }
                    if (IsPointInZone(zones.TaxiZones, p) == true)
                    {
                        return 8;
                    }
                }
            }
            else
            {
                if (IsPointInZone(zones.Runway25LZones, p) == true)
                {
                    return 1;
                }
                if (IsPointInZone(zones.Runway02Zones, p) == true)
                {
                    return 2;
                }
                if (IsPointInZone(zones.Runway25RZones, p) == true)
                {
                    return 3;
                }

                if (IsPointInZone(zones.StandT1Zones, p) == true)
                {
                    return 4;
                }
                if (IsPointInZone(zones.StandT2Zones, p) == true)
                {
                    return 5;
                }
                if (IsPointInZone(zones.ApronT1Zones, p) == true)
                {
                    return 6;
                }
                if (IsPointInZone(zones.ApronT2Zones, p) == true)
                {
                    return 7;
                }
                if (IsPointInZone(zones.TaxiZones, p) == true)
                {
                    return 8;
                }
                if (IsPointInZone(zones.Airborne25RZones25, p) == true)
                {
                    return 9;
                }
                if (IsPointInZone(zones.Airborne02Zones25, p) == true)
                {
                    return 10;
                }
                if (IsPointInZone(zones.Airborne25LZones25, p) == true)
                {
                    return 11;
                }
                if (IsPointInZone(zones.Airborne25RZones5, p) == true)
                {
                    return 12;
                }
                if (IsPointInZone(zones.Airborne02Zones5, p) == true)
                {
                    return 13;
                }
                if (IsPointInZone(zones.Airborne25LZones5, p) == true)
                {
                    return 14;
                }             
            }
            return zone;
        }

        public int RecomputeZone(CAT10 Before, CAT10 MLAT, CAT10 After)
        {
            double dir = 0;

            if (IsPointInZone(zones.Runway25LZones, new Point(MLAT.X_Component_map, MLAT.Y_Component_map)) == true)
            {
                if (After == null)
                {
                    double X = (MLAT.X_Component_map - Before.X_Component_map);
                    double Y = (MLAT.Y_Component_map - Before.Y_Component_map);
                    dir = Math.Atan2(Y, X);
                }
                else
                {
                    double X = (After.X_Component_map - MLAT.X_Component_map);
                    double Y = (After.Y_Component_map - MLAT.Y_Component_map);
                    dir = Math.Atan2(Y, X);
                }
                dir = dir * (180 / Math.PI);
                if (dir < 0) { dir += 360; }
                if ((dir <= 42 || dir > 356) || (dir >= 176 && dir <= 222))
                {
                    return 1;
                }
                else
                {
                    return 10;
                }
            }
            else
            {
                return 10;
            }
        }

        public int RecomputeZone(CAT21vs21 Before, CAT21vs21 ADSB, CAT21vs21 After)
        {
            double dir=0;

            if (IsPointInZone(zones.Runway25LZones, new Point(ADSB.X_Component_map,ADSB.Y_Component_map)) == true)
            {
                if (After == null)
                {
                    double X = (ADSB.X_Component_map - Before.X_Component_map);
                    double Y = (ADSB.Y_Component_map - Before.Y_Component_map);
                    dir = Math.Atan2(Y, X);
                }
                else
                {
                    double X = (After.X_Component_map - ADSB.X_Component_map);
                    double Y = (After.Y_Component_map - ADSB.Y_Component_map);
                    dir = Math.Atan2(Y, X);
                }
                dir = dir * (180 / Math.PI);
                if (dir < 0) { dir += 360; }
                if ((dir <= 42 ||  dir >=356) || (dir >= 176 && dir <= 222))
                {
                    return 1;
                }
                else
                {
                    return 10;
                }
            }
            else
            {
                return 10;
            }
        }

        public int RecomputeZone(MarkerDGPS Before, MarkerDGPS DGPS, MarkerDGPS After)
        {
            double dir = 0;

            if (IsPointInZone(zones.Runway25LZones, DGPS.Pxy) == true)
            {
                if (After == null)
                {
                    double X = (DGPS.Pxy.X - Before.Pxy.X);
                    double Y = (DGPS.Pxy.Y - Before.Pxy.Y);
                    dir = Math.Atan2(Y, X);
                }
                else
                {
                    double X = (After.Pxy.X - DGPS.Pxy.X);
                    double Y = (After.Pxy.Y - DGPS.Pxy.Y);
                    dir = Math.Atan2(Y, X);
                }
                dir = dir * (180 / Math.PI);
                if (dir < 0) { dir += 360; }
                if ((dir <= 42 || dir >= 356) || (dir >= 176 && dir <= 222))
                {
                    return 1;
                }
                else
                {
                    return 10;
                }
            }
            else
            {
                return 10;
            }
        }

        public Point ComputeCartesianFromWGS84(PointLatLng Platlng)
        {

            CoordinatesWGS84 ObjectCoordinates = new CoordinatesWGS84((Math.PI / 180) * Platlng.Lat, Platlng.Lng* (Math.PI / 180));
            CoordinatesWGS84 RadarCoordinates = new CoordinatesWGS84(41.2970767 * (Math.PI / 180), 2.07846278 * (Math.PI / 180));
            GeoUtils geoUtils = new GeoUtils();
            CoordinatesXYZ MarkerCartesian = geoUtils.change_geodesic2system_cartesian(ObjectCoordinates, RadarCoordinates);
            geoUtils = null;
            double X = MarkerCartesian.X;
            double Y = MarkerCartesian.Y;
            Point p = new Point(X, Y);
            return p;
        }

        public Point ComputeCartesianFromWGS84(PointLatLng Platlng,double Height,double ARPHeight)
        {

            CoordinatesWGS84 ObjectCoordinates = new CoordinatesWGS84((Math.PI / 180) * Platlng.Lat, Platlng.Lng * (Math.PI / 180),Height);
            CoordinatesWGS84 RadarCoordinates = new CoordinatesWGS84(41.2970767 * (Math.PI / 180), 2.07846278 * (Math.PI / 180),ARPHeight);
            GeoUtils geoUtils = new GeoUtils();
            CoordinatesXYZ MarkerCartesian = geoUtils.change_geodesic2system_cartesian(ObjectCoordinates, RadarCoordinates);
            geoUtils = null;
            double X = MarkerCartesian.X;
            double Y = MarkerCartesian.Y;
            Point p = new Point(X, Y);
            return p;
        }

        PointLatLng SMRAsturias = new PointLatLng(43.56464083, -6.030623056);
        PointLatLng SMRBarcelona = new PointLatLng(41.29561833, 2.095114167);
        PointLatLng SMRPalma = new PointLatLng(39.54864778, 2.732764444);
        PointLatLng SMRSantiago = new PointLatLng(42.89805333, -8.413033056);
        PointLatLng SMRBarajas_N = new PointLatLng(40.49184306, -3.569051667);
        PointLatLng SMRBarajas_S = new PointLatLng(40.46814972, -3.568730278);
        PointLatLng SMRTenerife = new PointLatLng(28.47749583, -16.33252028);
        PointLatLng ARPPalma = new PointLatLng(39.5486842, 2.73276111);
        PointLatLng ARPAsturias = new PointLatLng(43.56356722, -6.034621111);
        PointLatLng ARPBarajas = new PointLatLng(40.47224833, -3.560945278);
        PointLatLng ARPBarcelona = new PointLatLng(41.2970767, 2.07846278); 
        PointLatLng ARPMalaga = new PointLatLng(36.67497111, - 4.499206944);
        PointLatLng ARPTenerife = new PointLatLng(28.48265333, -16.34153722);
        PointLatLng ARPSantiago = new PointLatLng(42.896335, -8.41514361);
    }
}
