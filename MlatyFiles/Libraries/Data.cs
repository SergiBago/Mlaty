using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PGTA_WPF
{
    public class Data
    {
       
        //public int totaltraj;

        public DataTable parameters = new DataTable();
        public DataTable PD = new DataTable();
        public DataTable UP = new DataTable();
        public DataTable PFD = new DataTable();
        public DataTable PFI = new DataTable();
        public DataTable PI = new DataTable();

        public List<DataZone> ListZones = new List<DataZone>();
        public List<PrecissionPoint> PrecissionPoints = new List<PrecissionPoint>();
        public Data()
        {
            DataZone zone = new DataZone("Runway25L");
            ListZones.Add(zone);
            DataZone zone1 = new DataZone("Runway02");
            ListZones.Add(zone1);
            DataZone zone2 = new DataZone("Runway25R");
            ListZones.Add(zone2);
            DataZone zone3 = new DataZone("StandT1");
            ListZones.Add(zone3);
            DataZone zone4 = new DataZone("StandT2");
            ListZones.Add(zone4);
            DataZone zone5 = new DataZone("ApronT1");
            ListZones.Add(zone5);
            DataZone zone6 = new DataZone("ApronT2");
            ListZones.Add(zone6);
            DataZone zone7 = new DataZone("TaxiZones");
            ListZones.Add(zone7);
            DataZone zone8 = new DataZone("Airborne25RZones25");
            ListZones.Add(zone8);
            DataZone zone9 = new DataZone("Airborne02Zones25");
            ListZones.Add(zone9);
            DataZone zone10 = new DataZone("Airborne25LZones25");
            ListZones.Add(zone10);
            DataZone zone11 = new DataZone("Airborne25RZones5");
            ListZones.Add(zone11);
            DataZone zone12 = new DataZone("Airborne02Zones5");
            ListZones.Add(zone12);
            DataZone zone13 = new DataZone("Airborne25LZones5");
            ListZones.Add(zone13);
            DataZone zone14 = new DataZone("Total Runways");
            ListZones.Add(zone14);
            DataZone zone15 = new DataZone("Total Stands");
            ListZones.Add(zone15);
            DataZone zone16 = new DataZone("Total Aprons");
            ListZones.Add(zone16);
            DataZone zone17 = new DataZone("Total Airborne 0-2.5NM");
            ListZones.Add(zone17);
            DataZone zone18 = new DataZone("Total Airborne 2.5-5NM");
            ListZones.Add(zone18);
            parameters.Columns.Add("Zone");
            parameters.Columns.Add("P95");
            parameters.Columns.Add("Max P95");
            parameters.Columns.Add("P99");
            parameters.Columns.Add("Max P99");
            parameters.Columns.Add("Used MLAT");
            parameters.Columns.Add("Used ADSB-DGPS");
            parameters.Columns.Add("Mean");
            parameters.Columns.Add("STD");

            PD.Columns.Add("Zone");
            PD.Columns.Add("Expected\nUpdates PD");
            PD.Columns.Add("Missing\nUpdates PD");
            PD.Columns.Add("PD %");
            PD.Columns.Add("Missing Updates PD \n (50m restriction)");
            PD.Columns.Add("PD % \n (50m restriction)");
            PD.Columns.Add("Minimum\nPD %");

            UP.Columns.Add("Zone");
            UP.Columns.Add("Expected\nUpdates UR");
            UP.Columns.Add("Missing\nUpdates UR");
            UP.Columns.Add("UR %");
            UP.Columns.Add("Minimum\nUR %");

            PFD.Columns.Add("Zone");
            PFD.Columns.Add("Correct Reports");
            PFD.Columns.Add("False Reports");
            PFD.Columns.Add("PFD %");
            PFD.Columns.Add("Maximum PFD %");

            PI.Columns.Add("Zone");
            PI.Columns.Add("Correct Reports");
            PI.Columns.Add("False Reports");
            PI.Columns.Add("PI %");
            PI.Columns.Add("Minimum PI %");

            PFI.Columns.Add("Zone");
            PFI.Columns.Add("Correct Identification");
            PFI.Columns.Add("False Identification");
            PFI.Columns.Add("PFI %");
            PFI.Columns.Add("Minimum PFI %");

        }

        public void CreateTable()
        {
            CreatePrecissionTable();
            CreatePDTable();
            CreateUPTable();
            CreatePFDTable();
            CreatePFITable();
            CreatePITable();
        }

        public void CreatePrecissionTable()
        {
            parameters.Clear();
            AddHeather("--- RUNWAYS ---");
            for (int i = 0; i < 3; i++)
            {
                AddRow(ListZones[i],7.5, 12);
            }
            int[] list1 = new int[] { 0, 1, 2 };
            AddTotal(ListZones[14], list1,7.5,12);
            AddHeather("--- STANDS ---");
            for (int i = 3; i < 5; i++)
            {
                AddRow(ListZones[i],20,-2);
            }
            list1 = new int[] { 3, 4 };
            AddTotal(ListZones[15], list1,20,-2);
            AddHeather("--- APRONS ---");
            for (int i = 5; i < 7; i++)
            {
                AddRow(ListZones[i],7.5,12);
            }
            list1 = new int[] { 5, 6 };
            AddTotal(ListZones[16], list1,7.5,12);
            AddHeather("--- TAXI ---");
            AddRow(ListZones[7],7.5,12);
            AddHeather("-- AIRBORNE 0-2.5NM --");
            for (int i = 8; i < 11; i++)
            {
                AddRow(ListZones[i],20,-1);
            }
            list1 = new int[] { 8, 9, 10 };
            AddTotal(ListZones[17], list1,20,-1);
            AddHeather("-- AIRBORNE 2.5-5NM --");
            for (int i = 11; i < 14; i++)
            {
                AddRow(ListZones[i],40,-1);
            }
            list1 = new int[] { 11, 12, 13 };
            AddTotal(ListZones[18], list1,40,-1);
        }

        public void CreatePDTable()
        {
            PD.Clear();
            AddHeatherPD("--- RUNWAYS ---");
            for (int i = 0; i < 3; i++)
            {
                AddRowPD(ListZones[i], 99.9);
            }
            int[] list1 = new int[] { 0, 1, 2 };
            AddTotalPD(ListZones[14], list1,  99.9);
            AddHeatherPD("--- STANDS ---");
            for (int i = 3; i < 5; i++)
            {
                AddRowPD(ListZones[i], 99.9);
            }
            list1 = new int[] { 3, 4 };
            AddTotalPD(ListZones[15], list1,  99.9);
            AddHeatherPD("--- APRONS ---");
            for (int i = 5; i < 7; i++)
            {
                AddRowPD(ListZones[i],  99.9);
            }
            list1 = new int[] { 5, 6 };
            AddTotalPD(ListZones[16], list1, 99.9);
            AddHeatherPD("--- TAXI ---");
            AddRowPD(ListZones[7], 99.9);
            AddHeatherPD("-- AIRBORNE 0-2.5NM --");
            for (int i = 8; i < 11; i++)
            {
                AddRowPD(ListZones[i], 99.9);
            }
            list1 = new int[] { 8, 9, 10 };
            AddTotalPD(ListZones[17], list1, 99.9);
            AddHeatherPD("-- AIRBORNE 2.5-5NM --");
            for (int i = 11; i < 14; i++)
            {
                AddRowPD(ListZones[i], 99.9);
            }
            list1 = new int[] { 11, 12, 13 };
            AddTotalPD(ListZones[18], list1, 99.9);
        }

        public void CreateUPTable()
        {
            UP.Clear();
            AddHeatherUP("--- RUNWAYS ---");
            for (int i = 0; i < 3; i++)
            {
                AddRowUP(ListZones[i], 95);
            }
            int[] list1 = new int[] { 0, 1, 2 };
            AddTotalUP(ListZones[14], list1, 95);
            AddHeatherUP("--- STANDS ---");
            for (int i = 3; i < 5; i++)
            {
                AddRowUP(ListZones[i], 50);
            }
            list1 = new int[] { 3, 4 };
            AddTotalUP(ListZones[15], list1, 50);
            AddHeatherUP("--- APRONS ---");
            for (int i = 5; i < 7; i++)
            {
                AddRowUP(ListZones[i], 70);
            }
            list1 = new int[] { 5, 6 };
            AddTotalUP(ListZones[16], list1, 70);
            AddHeatherUP("--- TAXI ---");
            AddRowUP(ListZones[7], 95);
            AddHeatherUP("-- AIRBORNE 0-2.5NM --");
            for (int i = 8; i < 11; i++)
            {
                AddRowUP(ListZones[i], 95);
            }
            list1 = new int[] { 8, 9, 10 };
            AddTotalUP(ListZones[17], list1, 95);
            AddHeatherUP("-- AIRBORNE 2.5-5NM --");
            for (int i = 11; i < 14; i++)
            {
                AddRowUP(ListZones[i], 95);
            }
            list1 = new int[] { 11, 12, 13 };
            AddTotalUP(ListZones[18], list1, 95);
        }

        public void CreatePFDTable()
        {
            PFD.Clear();
            AddHeatherPFD("--- RUNWAYS ---");
            for (int i = 0; i < 3; i++)
            {
                AddRowPFD(ListZones[i],0.01);
            }
            int[] list1 = new int[] { 0, 1, 2 };
            AddTotalPFD(ListZones[14], list1,0.01);
            AddHeatherPFD("--- STANDS ---");
            for (int i = 3; i < 5; i++)
            {
                AddRowPFD(ListZones[i],0.01);
            }
            list1 = new int[] { 3, 4 };
            AddTotalPFD(ListZones[15], list1, 0.01);
            AddHeatherPFD("--- APRONS ---");
            for (int i = 5; i < 7; i++)
            {
                AddRowPFD(ListZones[i],0.01);
            }
            list1 = new int[] { 5, 6 };
            AddTotalPFD(ListZones[16], list1, 0.01);
            AddHeatherPFD("--- TAXI ---");
            AddRowPFD(ListZones[7], 0.01);
            //AddHeatherPFD("-- AIRBORNE 0-2.5NM --");
            //for (int i = 8; i < 11; i++)
            //{
            //    AddRowPFD(ListZones[i], 0.01);
            //}
            //list1 = new int[] { 8, 9, 10 };
            //AddTotalPFD(ListZones[17], list1, 0.01);
            //AddHeatherPFD("-- AIRBORNE 2.5-5NM --");
            //for (int i = 11; i < 14; i++)
            //{
            //    AddRowPFD(ListZones[i], 0.01);
            //}
            //list1 = new int[] { 11, 12, 13 };
            //AddTotalPFD(ListZones[18], list1, 0.01);
        }

        public void CreatePITable()
        {
            PI.Clear();
            AddHeatherPI("--- RUNWAYS ---");
            for (int i = 0; i < 3; i++)
            {
                AddRowPI(ListZones[i], 99.9);
            }
            int[] list1 = new int[] { 0, 1, 2 };
            AddTotalPI(ListZones[14], list1, 99.9);
            AddHeatherPI("--- STANDS ---");
            for (int i = 3; i < 5; i++)
            {
                AddRowPI(ListZones[i], 99.9);
            }
            list1 = new int[] { 3, 4 };
            AddTotalPI(ListZones[15], list1, 99.9);
            AddHeatherPI("--- APRONS ---");
            for (int i = 5; i < 7; i++)
            {
                AddRowPI(ListZones[i], 99.9);
            }
            list1 = new int[] { 5, 6 };
            AddTotalPI(ListZones[16], list1, 99.9);
            AddHeatherPI("--- TAXI ---");
            AddRowPI(ListZones[7], 99.9);
            AddHeatherPI("-- AIRBORNE 0-2.5NM --");
            for (int i = 8; i < 11; i++)
            {
                AddRowPI(ListZones[i], 99.9);
            }
            list1 = new int[] { 8, 9, 10 };
            AddTotalPI(ListZones[17], list1, 99.9);
            AddHeatherPI("-- AIRBORNE 2.5-5NM --");
            for (int i = 11; i < 14; i++)
            {
                AddRowPI(ListZones[i], 99.9);
            }
            list1 = new int[] { 11, 12, 13 };
            AddTotalPI(ListZones[18], list1, 99.9);
        }



        public void CreatePFITable()
        {
            PFI.Clear();
            AddHeatherPFI("--- RUNWAYS ---");
            for (int i = 0; i < 3; i++)
            {
                AddRowPFI(ListZones[i], 0.0001);
            }
            int[] list1 = new int[] { 0, 1, 2 };
            AddTotalPFI(ListZones[14], list1, 0.0001);
            AddHeatherPFI("--- STANDS ---");
            for (int i = 3; i < 5; i++)
            {
                AddRowPFI(ListZones[i], 0.0001);
            }
            list1 = new int[] { 3, 4 };
            AddTotalPFI(ListZones[15], list1, 0.0001);
            AddHeatherPFI("--- APRONS ---");
            for (int i = 5; i < 7; i++)
            {
                AddRowPFI(ListZones[i], 0.0001);
            }
            list1 = new int[] { 5, 6 };
            AddTotalPFI(ListZones[16], list1, 0.0001);
            AddHeatherPFI("--- TAXI ---");
            AddRowPFI(ListZones[7], 0.0001);
            AddHeatherPFI("-- AIRBORNE 0-2.5NM --");
            for (int i = 8; i < 11; i++)
            {
                AddRowPFI(ListZones[i], 0.0001);
            }
            list1 = new int[] { 8, 9, 10 };
            AddTotalPFI(ListZones[17], list1, 0.0001);
            AddHeatherPFI("-- AIRBORNE 2.5-5NM --");
            for (int i = 11; i < 14; i++)
            {
                AddRowPFI(ListZones[i], 0.0001);
            }
            list1 = new int[] { 11, 12, 13 };
            AddTotalPFI(ListZones[18], list1, 0.0001);
        }


        private void AddHeather(String name)
        {
            var row = parameters.NewRow();
            row["Zone"] =name;
            row["P95"] ="--------";
            row["Max P95"] = "--------";
            row["P99"] = "--------";
            row["Max P99"] ="--------";
            row["Used MLAT"] = "--------";
            row["Used ADSB-DGPS"] = "--------";
            row["Mean"] = "--------";
            row["STD"] = "--------";
            parameters.Rows.Add(row);
        }

        private void AddHeatherPD(String name)
        {
            var row =PD.NewRow();
            row["Zone"] = name;
            row["Expected\nUpdates PD"] = "--------";
            row["Missing\nUpdates PD"] = "--------";
            row["PD %"] = "--------";
            row["Missing Updates PD \n (50m restriction)"] = "--------";
            row["PD % \n (50m restriction)"] = "--------";
            row["Minimum\nPD %"] = "--------";
            PD.Rows.Add(row);
        }

        private void AddHeatherUP(String name)
        {
            var row = UP.NewRow();
            row["Zone"] = name;
            row["Expected\nUpdates UR"] = "--------";
            row["Missing\nUpdates UR"] = "--------";
            row["UR %"] = "--------";
            row["Minimum\nUR %"] = "--------";
            UP.Rows.Add(row);
        }

        private void AddHeatherPFD(String name)
        {
            var row = PFD.NewRow();
            row["Zone"] = name;
            row["Correct Reports"] = "--------";
            row["False Reports"] = "--------";
            row["PFD %"] = "--------";
            row["Maximum PFD %"] = "--------";
            PFD.Rows.Add(row);
        }

        private void AddHeatherPI(String name)
        {
            var row = PI.NewRow();
            row["Zone"] = name;
            row["Correct Reports"] = "--------";
            row["False Reports"] = "--------";
            row["PI %"] = "--------";
            row["Minimum PI %"] = "--------";
            PI.Rows.Add(row);
        }


        private void AddHeatherPFI(String name)
        {
            var row = PFI.NewRow();
            row["Zone"] = name;
            row["Correct Identification"] = "--------";
            row["False Identification"] = "--------";
            row["PFI %"] = "--------";
            row["Minimum PFI %"] = "--------";
            PFI.Rows.Add(row);
        }


        private void AddRow(DataZone zone, double max95, double max99)
        {
            var row = parameters.NewRow();
            row["Zone"] = zone.name;
            row["P95"] = zone.get95(max95);
            row["Max P95"] = Convert.ToString(max95);
            row["P99"] = zone.get99(max99);
            if(max99<0)
            {
                row["Max P99"] = "--------";

            }
            else
            {
                row["Max P99"] = Convert.ToString(max99);

            }
            row["Used MLAT"] = Convert.ToString(zone.MLATMessagesUsed);
            row["Used ADSB-DGPS"] = Convert.ToString(zone.ADSBMessagesUsed+ zone.DGPSMessagesUsed);
            row["Mean"] = zone.GetMedia();
            row["STD"] = zone.GetDesviacion();
            parameters.Rows.Add(row);
        }



        private void AddRowPD(DataZone zone,  double minPD)
        {
            var row = PD.NewRow();
            row["Zone"] = zone.name;
            row["Expected\nUpdates PD"] = Convert.ToString(zone.ExpectedMessagesPD);
            row["Missing\nUpdates PD"] = Convert.ToString(zone.MissedMLATSPD); ;
            row["PD %"] = zone.GetPD(minPD);
            row["Missing Updates PD \n (50m restriction)"] = Convert.ToString(zone.MissedMLATSPD_50m_Restriction);
            row["PD % \n (50m restriction)"] = zone.GetPD_50m_Restriction(minPD);
            row["Minimum\nPD %"] = Convert.ToString(minPD) + "%";
            PD.Rows.Add(row);
        }

        private void AddRowUP(DataZone zone, int minUR)
        {
            var row = UP.NewRow();
            row["Zone"] = zone.name;
            row["Expected\nUpdates UR"] = Convert.ToString(zone.ExpectedMessagesUP);
            row["Missing\nUpdates UR"] = Convert.ToString(zone.MissedMLATSUP); ;
            row["UR %"] = zone.GetUR(minUR);
            row["Minimum\nUR %"] = Convert.ToString(minUR) + "%";        
            UP.Rows.Add(row);
        }


        private void AddRowPFD(DataZone zone, double maxPFD)
        {
            var row = PFD.NewRow();
            row["Zone"] = zone.name;
            row["Correct Reports"] = Convert.ToString(zone.CorrectDetection);
            row["False Reports"] = Convert.ToString(zone.FalseDetection);
            row["PFD %"] = zone.GetPFD(maxPFD);
            if (maxPFD < 0)
            {
                row["Maximum PFD %"] = $"Not Required";

            }
            else
            {
                row["Maximum PFD %"] = $"{maxPFD} %";
            }
            PFD.Rows.Add(row);
        }

        private void AddRowPI(DataZone zone, double minPI)
        {
            var row = PI.NewRow();
            row["Zone"] = zone.name;
            row["Correct Reports"] = Convert.ToString(zone.CorrectPI);
            row["False Reports"] = Convert.ToString(zone.IncorrectPI);
            row["PI %"] =  zone.GetPI(minPI);
            row["Minimum PI %"] = $"{minPI} %";
            PI.Rows.Add(row);
        }

        private void AddRowPFI(DataZone zone,  double minPFI)
        {
            var row = PFI.NewRow();
            row["Zone"] = zone.name;
            row["Correct Identification"] =Convert.ToString(zone.CorrectIdentification);
            row["False Identification"] = Convert.ToString(zone.FalseIdentification);
            row["PFI %"] = zone.GetPFI(minPFI);
            row["Minimum PFI %"] = $"{minPFI} %"; ;
            PFI.Rows.Add(row);        
        }

        private void AddTotal(DataZone zone, int[] list, double max95,double max99)
        {
            zone.CreateTotal(list,ListZones);
            AddRow(zone,max95,max99);
        }

        private void AddTotalPD(DataZone zone, int[] list, double minPD)
        {
            zone.CreateTotalPD(list, ListZones);
            AddRowPD(zone, minPD);
        }

        private void AddTotalUP(DataZone zone, int[] list, int minUR)
        {
            zone.CreateTotalUP(list, ListZones);
            AddRowUP(zone, minUR);
        }

        private void AddTotalPFD(DataZone zone, int[] list, double minPFD)
        {
            zone.CreateTotalPFD(list, ListZones);
            AddRowPFD(zone, minPFD);
        }

        private void AddTotalPI(DataZone zone, int[] list, double minPI)
        {
            zone.CreateTotalPI(list, ListZones);
            AddRowPI(zone, minPI);
        }

        private void AddTotalPFI(DataZone zone, int[] list, double minPFI)
        {
            zone.CreateTotalPFI(list, ListZones);
            AddRowPFI(zone, minPFI);
        }



    }
}
