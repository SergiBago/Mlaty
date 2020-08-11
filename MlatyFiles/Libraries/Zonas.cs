using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PGTA_WPF
{
    public class Zonas
    {
        public List<Polygon> Runway25LZones = new List<Polygon>();
        public List<Polygon> Runway02Zones = new List<Polygon>();
        public List<Polygon> Runway25RZones = new List<Polygon>();
        public List<Polygon> StandT1Zones = new List<Polygon>();
        public List<Polygon> StandT2Zones = new List<Polygon>();
        public List<Polygon> ApronT1Zones = new List<Polygon>();
        public List<Polygon> ApronT2Zones = new List<Polygon>();
        public List<Polygon> TaxiZones = new List<Polygon>();
        public List<Polygon> Airborne25RZones25 = new List<Polygon>();
        public List<Polygon> Airborne02Zones25 = new List<Polygon>();
        public List<Polygon> Airborne25LZones25 = new List<Polygon>();
        public List<Polygon> Airborne25RZones5 = new List<Polygon>();
        public List<Polygon> Airborne02Zones5 = new List<Polygon>();
        public List<Polygon> Airborne25LZones5 = new List<Polygon>();
        public List<Polygon> IntersectionRW25LAir02 = new List<Polygon>();


        public Zonas()
        {
            Define_RW25L();
            Define_RW25R();
            Define_RW02();
            Define_Stand_T1();
            Define_Stand_T2();
            Define_Apron_T1();
            Define_Apron_T2();
            Define_TaxyZone();
            // Define_Interesction_RW25L_Air02();
            //  Define_Area_de_Maniobras_25L_07R_1();
            //  Define_Area_de_Maniobras_25L_07R_2();
            //  Define_Area_de_Maniobras_25L_07R_3();
            //  Define_Area_de_Maniobras_25L_07R_4();
            //  Define_Area_de_Maniobras_25L_07R_5();
            //  Define_Area_de_Maniobras_02_20_1();
            //  Define_Area_de_Maniobras_02_20_2();
            //  Define_Area_de_Maniobras_25R_07L();
            //  Define_Stand_T1_1();
            //  Define_Stand_T1_2();
            //  Define_Stand_T1_3();
            //  Define_Apron_T1_1();
            //  Define_Apron_T1_2();
            //  Define_Apron_T1_3();
            //  Define_Apron_T1_4();
            //  Define_Stand_T2_1();
            //  Define_Stand_T2_2();
            //  Define_Stand_T2_3();
            //  Define_Stand_T2_4();
            //  Define_Stand_T2_5();
            //  Define_Apron_T2_1();
            //  Define_Apron_T2_2();
            //  Define_Apron_T2_3();
            Define_25R2_0_25_NM();
            Define_02_0_25_NM();
            Define_25L_0_25_NM();
            //  Define_25R_0_25_NM();
            Define_20_0_25_NM();
            Define_07L_0_25_NM();
            Define_07R_0_25_NM();
            Define_02_25_5_NM();
            Define_25R_25_5_NM();
            Define_25L_25_5_NM();
            Define_20_25_5_NM();
            Define_07L_25_5_NM();
            Define_07R_25_5_NM();
            Define_25R_0_25_NM();
        }

        //private void Define_Interesction_RW25L_Air02()
        //{
        //    List<Point> Area_de_Maniobras_25L_07R_1 = new List<Point>();
        //    Area_de_Maniobras_25L_07R_1.Add(new Point(320, - 1380));
        //    Area_de_Maniobras_25L_07R_1.Add(new Point(524.5, - 1287));
        //    Area_de_Maniobras_25L_07R_1.Add(new Point(536, - 1206));
        //    Area_de_Maniobras_25L_07R_1.Add(new Point(372.5, - 1280));
        //    Polygon pol = new Polygon("Ruynway 25L/07R", Area_de_Maniobras_25L_07R_1);
        //    IntersectionRW25LAir02.Add(pol);
        //}

        private void Define_RW25L()
        {
            List<Point> Area = new List<Point>();
            Area.Add(new Point(2124.56, - 482.79));
            Area.Add(new Point(-408.94, - 1636.82));
            Area.Add(new Point(-382.04, - 1694.62));
            Area.Add(new Point(2157.22, - 545.73));
            Polygon pol = new Polygon("Ruynway 25L/07R", Area);
            Runway25LZones.Add(pol);
        }

        private void Define_RW25R()
        {
            List<Point> Area = new List<Point>();
            Area.Add(new Point(-1003.71, - 419.61));
            Area.Add(new Point(-979.19, - 475.79));
            Area.Add(new Point(2132.32, 933.72));
            Area.Add(new Point(2105.66, 990.27));
            Polygon pol = new Polygon("Ruynway 25R/07L", Area);
            Runway25RZones.Add(pol);
        }

        private void Define_RW02()
        {
            List<Point> Area = new List<Point>();
            Area.Add(new Point(505.25, - 1028.53));
            Area.Add(new Point(559.43, - 1046.05));
            Area.Add(new Point(1420.19, 1423.28));
            Area.Add(new Point(1350.87, 1442.79));
            Polygon pol = new Polygon("Ruynway 02", Area);
            Runway02Zones.Add(pol);
        }


        private void Define_Stand_T1()
        {
            List<Point> Area = new List<Point>();
            Area.Add(new Point(-817.18, - 765.68));
            Area.Add(new Point(-552.81, - 1352.69));
            Area.Add(new Point(-0.07, - 1105.22));
            Area.Add(new Point(11.31, - 1090.08));
            Area.Add(new Point(8, - 1066.81));
            Area.Add(new Point(-236.16, - 525.97)); 
            Area.Add(new Point(-258.3, - 514.83));
            Area.Add(new Point(-279.82, - 520.21));
            Polygon pol = new Polygon("Stand T1 Zones", Area);
            StandT1Zones.Add(pol);
            Area = new List<Point>(); //7
            Area.Add(new Point(-67.01, - 911.17));
            Area.Add(new Point(408.41, - 693.36));
            Area.Add(new Point(453.83, - 561.87));
            Area.Add(new Point(456.83, - 550.73));
            Area.Add(new Point(457.21, - 534.35));
            Area.Add(new Point(412.17, - 438.51));
            Area.Add(new Point(399.65, - 427.37));
            Area.Add(new Point(376.13, - 430.75));
            Area.Add(new Point(-170.23, - 680.00));
            pol = new Polygon("Stand T1 Zones", Area);
            StandT1Zones.Add(pol);
            Area = new List<Point>(); //8
            Area.Add(new Point(51.35, -949.59));
            Area.Add(new Point(111.52, -1083.7));
            Area.Add(new Point(303.0, -995.63));
            Area.Add(new Point(353.99, -853.75));
            Area.Add(new Point(352.99, -836.23));
            Area.Add(new Point(333.47, -825.47));
            Area.Add(new Point(319.34, -827.47));
            pol = new Polygon("Stand T1 Zones", Area);
            StandT1Zones.Add(pol);
            Area = new List<Point>(); //9
            Area.Add(new Point(354.99, - 347.67));
            Area.Add(new Point(371.13, - 335.91));
            Area.Add(new Point(370.51, - 319.77));
            Area.Add(new Point(323.09, - 218.94));
            Area.Add(new Point(309.58, - 212.56));
            Area.Add(new Point(285.68, - 216.56));
            Area.Add(new Point(- 155.71, - 416.61));
            Area.Add(new Point(-167.85, - 430.75));
            Area.Add(new Point(-167.85, - 452.27));
            Area.Add(new Point(-126.19, - 545.35));
            Area.Add(new Point(-115.43, - 555.73));
            Area.Add(new Point(-96.91, - 555.73));
            pol = new Polygon("Stand T1 Zones", Area);
            StandT1Zones.Add(pol);
        }


        private void Define_Stand_T2()
        {
            List<Point> Area = new List<Point>(); //10
            Area.Add(new Point(-731.34, 261.24));
            Area.Add(new Point(-777.14, 354.7));
            Area.Add(new Point(-1693.32, - 61.55));
            Area.Add(new Point(-1650.29, - 148.62));
            Polygon pol = new Polygon("Stand T2 Zones", Area);
            StandT2Zones.Add(pol);
            Area = new List<Point>(); //11
            Area.Add(new Point(-1650.29, - 148.62));
            Area.Add(new Point(-1577.73, - 303.26));
            Area.Add(new Point(-621.75, 131.89));
            Area.Add(new Point(-613.75, 145.89));
            Area.Add(new Point(-639.89, 209.68));
            Area.Add(new Point(-731.34, 261.24));
            pol = new Polygon("Stand T2 Zones", Area);
            StandT2Zones.Add(pol);
            Area = new List<Point>(); //12
            Area.Add(new Point(502.25, 700.76));
            Area.Add(new Point(398.03, 933.72));
            Area.Add(new Point(-683.3, 438.77));
            Area.Add(new Point(-702.44, 418.5));
            Area.Add(new Point(-661.79, 336.56));
            Area.Add(new Point(-575.71, 282.76));
            Area.Add(new Point(-440.35, 272));
            pol = new Polygon("Stand T2 Zones", Area);
            StandT2Zones.Add(pol);
            Area = new List<Point>(); //13
            Area.Add(new Point(502.25, 700.76));
            Area.Add(new Point(545.91, 721));
            Area.Add(new Point(424.55, 981.37));
            Area.Add(new Point(380.51, 961.61));
            pol = new Polygon("Stand T2 Zones", Area);
            StandT2Zones.Add(pol);
            Area = new List<Point>(); //14
            Area.Add(new Point(424.55, 981.37));
            Area.Add(new Point(545.91, 721));
            Area.Add(new Point(575.94, 684.25));
            Area.Add(new Point(591.7, 680.25));
            Area.Add(new Point(877.21, 811));
            Area.Add(new Point(728.2, 1126.39));
            pol = new Polygon("Stand T2 Zones", Area);
            StandT2Zones.Add(pol);
            Area = new List<Point>(); //15
            Area.Add(new Point(728.2, 1126.39));
            Area.Add(new Point(790.52, 987.6));
            Area.Add(new Point(830.79, 1005.51));
            Area.Add(new Point(754.86, 1140.91));
            pol = new Polygon("Stand T2 Zones", Area);
            Area = new List<Point>(); //16
            Area.Add(new Point(754.86, 1140.91));
            Area.Add(new Point(830.79, 1005.51));
            Area.Add(new Point(978.53, 1070.17));
            Area.Add(new Point(1040.23, 1236.36));
            Area.Add(new Point(987.43, 1348.96));
            Area.Add(new Point(742.34, 1234.98));
            Area.Add(new Point(727.2, 1202.32));
            pol = new Polygon("Stand T2 Zones", Area);
        }


        private void Define_Apron_T1()
        {
            List<Point> Area = new List<Point>();//17
            Area.Add(new Point(-1074.65, - 823.47));
            Area.Add(new Point(-947.91, - 901.55));
            Area.Add(new Point(-787.9, - 828.85));
            Area.Add(new Point(-830.7, - 713.64));
            Polygon pol = new Polygon("Apron T1 Zones", Area);
            ApronT1Zones.Add(pol);
            Area = new List<Point>();//18
            Area.Add(new Point(-830.7, - 713.64));
            Area.Add(new Point(-787.9, - 828.85));
            Area.Add(new Point(-559.19, - 1338.31));
            Area.Add(new Point(-535.43, - 1355.07));
            Area.Add(new Point(-379, - 1414.73));
            Area.Add(new Point(79.38, - 1205));
            Area.Add(new Point(-257.3, - 454.27));
            pol = new Polygon("Apron T1 Zones", Area);
            ApronT1Zones.Add(pol);
            Area = new List<Point>();//19
            Area.Add(new Point(-379, - 1414.73));
            Area.Add(new Point(-482.25, - 1372.21));
            Area.Add(new Point(-437.83, - 1440.77));
            pol = new Polygon("Apron T1 Zones", Area);
            ApronT1Zones.Add(pol);
            Area = new List<Point>();//20
            Area.Add(new Point(79.38, - 1205));
            Area.Add(new Point(266, - 1120));
            Area.Add(new Point(310.96, - 1035.29));
            Area.Add(new Point(452.59, - 620.62));
            Area.Add(new Point(472.73, - 526.21));
            Area.Add(new Point(450.45, - 443.51));
            Area.Add(new Point(400.03, - 335.91)); 
            Area.Add(new Point(387.27, - 328.29));
            Area.Add(new Point(362.99, - 334.29)); 
            Area.Add(new Point(-199.12, - 594.14));
            pol = new Polygon("Apron T1 Zones", Area);
            ApronT1Zones.Add(pol);
            Area = new List<Point>();//21
            Area.Add(new Point(-176.23, - 585.9));
            Area.Add(new Point(-108.81, - 554.49));
            Area.Add(new Point(-199.74, - 538.35));
            pol = new Polygon("Apron T1 Zones", Area);
            ApronT1Zones.Add(pol);
            Area = new List<Point>();//22
            Area.Add(new Point(-176.23, - 585.9));
            Area.Add(new Point(-199.74, - 538.35));
            Area.Add(new Point(-233.78, - 513.45));
            Area.Add(new Point(-197.12, - 596.52));
            pol = new Polygon("Apron T1 Zones", Area);
            ApronT1Zones.Add(pol);
        }

        private void Define_Apron_T2()
        {
            List<Point> Area = new List<Point>();//23
            Area.Add(new Point(-702.44, 418.5));
            Area.Add(new Point(-777.14, 354.7));
            Area.Add(new Point(-731.34, 261.24));
            Area.Add(new Point(-621.75, 131.89));
            Area.Add(new Point(-420.08, 223.2));
            Area.Add(new Point(-440.35, 272));
            Polygon pol = new Polygon("Apron T2 Zones", Area);
            ApronT2Zones.Add(pol);
            Area = new List<Point>();//24
            Area.Add(new Point(-420.08, 223.2));
            Area.Add(new Point(591.7, 680.25));
            Area.Add(new Point(545.91, 721));
            Area.Add(new Point(-440.35, 272));
            pol = new Polygon("Apron T2 Zones", Area);
            ApronT2Zones.Add(pol);
            Area = new List<Point>();//25
            Area.Add(new Point(877.21, 811));
            Area.Add(new Point(934.39, 838.12));
            Area.Add(new Point(1003.57, 1032.93));
            Area.Add(new Point(978.53, 1070.17));
            Area.Add(new Point(790.52, 987.6));
            pol = new Polygon("Apron T2 Zones", Area);
            ApronT2Zones.Add(pol);
        }

        private void Define_TaxyZone()
        {
            List<Point> Area = new List<Point>();//26
            Area.Add(new Point(-437.83, - 1440.77));
            Area.Add(new Point(-450.59, - 1481.81));
            Area.Add(new Point(-457.35, - 1530.23));
            Area.Add(new Point(-408.94, - 1636.82));
            Area.Add(new Point(2124.56, - 482.79));
            Area.Add(new Point(2121.56, - 470.79));
            Area.Add(new Point(2041.43, - 366.95));
            Area.Add(new Point(2014.15, - 352.05));
            Area.Add(new Point(1979.69, - 342.29));
            Area.Add(new Point(1947.41, - 348.05));
            Polygon pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//27
            Area.Add(new Point(675.54, - 934.07));
            Area.Add(new Point(450.45, - 443.51));
            Area.Add(new Point( 250, -1120));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//28
            Area.Add(new Point(675.54, - 934.07));
            Area.Add(new Point(950.77, - 808.96));
            Area.Add(new Point(620.74, - 71.68));
            Area.Add(new Point(363.75, - 189.04));
            Area.Add(new Point(400.03, - 335.91));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//29
            Area.Add(new Point(400.03, - 335.91));
            Area.Add(new Point(363.75, - 189.04));
            Area.Add(new Point(311.09, - 211.94));
            Area.Add(new Point(370.13, - 335.53));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//30
            Area.Add(new Point(-233.78, - 513.45));
            Area.Add(new Point(-199.74, - 538.35));
            Area.Add(new Point(-116.81, - 556.11));
            Area.Add(new Point(-162.85, - 426.27));
            Area.Add(new Point(-253.16, - 467.55));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//31
            Area.Add(new Point(-1578.73, - 301.26));
            Area.Add(new Point(-1463.37, - 513.45));
            Area.Add(new Point(-1335.64, - 712.5));
            Area.Add(new Point(-1309.74, - 741.78));
            Area.Add(new Point(-1076.03, - 842.61));
            Area.Add(new Point(2242.54, 658.73));
            Area.Add(new Point(2270.58, 689.62));
            Area.Add(new Point(2315.24, 808.6));
            Area.Add(new Point(2310.86, 861.16));
            Area.Add(new Point(2132.32, 1245.12));
            Area.Add(new Point(2110.42, 1266.26));
            Area.Add(new Point(2006.59, 1315.68));
            Area.Add(new Point(934.39, 838.12));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//32
            Area.Add(new Point(934.39, 838.12));
            Area.Add(new Point(1183.1, 945.47));
            Area.Add(new Point(1338.49, 1397.38));
            Area.Add(new Point(1265.56, 1482.07));
            Area.Add(new Point(1255.42, 1490.83));
            Area.Add(new Point(1235.9, 1490.45));
            Area.Add(new Point(1145.82, 1449));
            Area.Add(new Point(1136.36, 1434.04));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//33
            Area.Add(new Point(2006.59, 1315.68));
            Area.Add(new Point(2120.07, 1700));
            Area.Add(new Point(1818.3, 1552.39));
            Area.Add(new Point(1947.41, 1286.38));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//34
            Area.Add(new Point(1818.3, 1552.39));
            Area.Add(new Point(1349.58, 1676.13));
            Area.Add(new Point(1392.62, 1584.67));
            Area.Add(new Point(1586.96, 1477.07));
            Area.Add(new Point(1764.5, 1471.69));
            Area.Add(new Point(1844, 1502.97));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//35
            Area.Add(new Point(1265.56, 1482.07));
            Area.Add(new Point(1299, 1439.42));
            Area.Add(new Point(1392.62, 1584.67));
            Area.Add(new Point(1349.58, 1676.13));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
            Area = new List<Point>();//36
            Area.Add(new Point(2250.06, 982.13));
            Area.Add(new Point(2283, 995));
            Area.Add(new Point(2247, 1070.83));
            Area.Add(new Point(2217, 1055.83));
            pol = new Polygon("Taxy Zones", Area);
            TaxiZones.Add(pol);
        }


        private void Define_25R_0_25_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(2250.06, 982.13));
            list.Add(new Point(2283, 995));
            list.Add(new Point(2247, 1070.83));
            list.Add(new Point(2217, 1055.83));
            Polygon pol = new Polygon("THR-25R 0-2.5NM", list);
            Airborne25RZones25.Add(pol);
        }

        private void Define_02_0_25_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(505.25, - 1028.53));
            list.Add(new Point(-1696.10, - 5182.74));
            list.Add(new Point(-88.34, - 5702.64));
            list.Add(new Point(559.43, - 1046.05));
            Polygon pol = new Polygon("THR-02 0-2.5NM", list);
            Airborne02Zones25.Add(pol);
        }

        private void Define_25L_0_25_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(2096.84, - 571.2));
            list.Add(new Point(6614.27, 731.19));
            list.Add(new Point(5866.74, 2258.27));
            list.Add(new Point(2067.19, - 510.63));
            Polygon pol = new Polygon("THR-25L 0-2.5NM", list);
            Airborne25LZones25.Add(pol);
        }


        private void Define_25R2_0_25_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(2132.32, 933.72));
            list.Add(new Point(6668.39, 2169.64));
            list.Add(new Point(5945.46, 3703.08));
            list.Add(new Point(2105.66, 990.27));
            Polygon pol = new Polygon("THR-25R 0-2.5NM", list);
            Airborne25RZones25.Add(pol);
        }

        private void Define_20_0_25_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(1399, 1360.37));
            list.Add(new Point(3719.73, 5449.09));
            list.Add(new Point(2114.35, 6020.07));
            list.Add(new Point(1332, 1384.2));
            Polygon pol = new Polygon("THR-20 0-2.5NM", list);
            Airborne02Zones25.Add(pol);
        }

        private void Define_07L_0_25_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(-556.92, - 217.06));
            list.Add(new Point(-5112.77, - 1377.95));
            list.Add(new Point(-4415.60, - 2922.33));
            list.Add(new Point(-531.55, - 273.26));
            Polygon pol = new Polygon("THR-07L 0-2.5NM", list);
            Airborne25RZones25.Add(pol);
        }

        private void Define_07R_0_25_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(-357.31, - 1614.15));
            list.Add(new Point(-4919.09, - 2751.50));
            list.Add(new Point(-4229.50, - 4300.36));
            list.Add(new Point(-331.83, - 1671.38));
            Polygon pol = new Polygon("THR-07R 0-2.5NM", list);
            Airborne25LZones25.Add(pol);
        }

        private void Define_02_25_5_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(-1696.10, - 5182.74));
            list.Add(new Point(-3897.45, - 9336.95));
            list.Add(new Point(-736.11, - 10359.22));
            list.Add(new Point(-88.34, - 5702.64));
            Polygon pol = new Polygon("THR-02 2.5-5NM", list);
            Airborne02Zones5.Add(pol);
        }

        private void Define_25R_25_5_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(6614.27, 731.19));
            list.Add(new Point(11131.70, 2033.59));
            list.Add(new Point(9666.29, 5027.17));
            list.Add(new Point(5866.74, 2258.27));
            Polygon pol = new Polygon("THR-25L 2.5-5NM", list);
            Airborne25LZones5.Add(pol);
        }

        private void Define_25L_25_5_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(6668.39, 2169.64));
            list.Add(new Point(11204.45, 3405.56));
            list.Add(new Point(9785.26, 6415.89));
            list.Add(new Point(5945.46, 3703.08));
            Polygon pol = new Polygon("THR-25R_2.5-5_NM", list);
            Airborne25RZones5.Add(pol);
        }

        private void Define_20_25_5_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(3719.73, 5449.09));
            list.Add(new Point(6040.47, 9537.80));
            list.Add(new Point(2896.71, 10655.95));
            list.Add(new Point(2114.35, 6020.07));
            Polygon pol = new Polygon("THR-20 2.5-5NM", list);
            Airborne02Zones5.Add(pol);
        }

        private void Define_07L_25_5_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(-5112.77, - 1377.95));
            list.Add(new Point(-9668.61, - 2538.84));
            list.Add(new Point(-8299.64, - 5571.41));
            list.Add(new Point(-4415.60, - 2922.33));
            Polygon pol = new Polygon("THR-07L 2.5-5NM", list);
            Airborne25RZones5.Add(pol);
        }

        private void Define_07R_25_5_NM()
        {
            List<Point> list = new List<Point>();
            list.Add(new Point(-4919.09, - 2751.50));
            list.Add(new Point(-9480.87, - 3888.85));
            list.Add(new Point(-8127.18, - 6929.34));
            list.Add(new Point(-4229.50, - 4300.36));
            Polygon pol = new Polygon("THR-07R 2.5-5NM", list);
            Airborne25LZones5.Add(pol);
        }
    }
}
