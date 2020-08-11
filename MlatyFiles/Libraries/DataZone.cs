using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PGTA_WPF
{
    public class DataZone
    {
        public int MLATMessagesUsed; //
        public int MLATMessagesUsedheight; //
        public int ADSBMessagesUsed; //
        public List<double> MLATPrecision = new List<double>(); //
        public int TotalVehiclesMLATused = 0; //
        public int TotalVehiclesMLATusedheight = 0; //
        public List<double> totalMLATPrecissionheight = new List<double>(); // 
        public double NAC = 0;
        public double NACused = 0;
        public double ExpectedMessagesPD=0;
        public double ExpectedMessagesUP = 0;

        public double obtainedMessages = 0;
        public int MissedMLATSPD = 0;
        public int MissedMLATSUP = 0;

        public string name;
        public int expected_PDok = 0;
        public int PDwrong = 0;
        public int DGPSMessagesUsed;


        public DataZone(string name)
        {
            this.name = name;
        }

        public void CreateTotal(int[] list ,List<DataZone> listzones)
        {
            MLATMessagesUsed = 0;
            MLATMessagesUsedheight = 0;
            ADSBMessagesUsed = 0;
            DGPSMessagesUsed = 0;
            MLATPrecision.Clear();
            totalMLATPrecissionheight.Clear();
            NAC = 0;
            NACused = 0;
            for (int i=0; i<list.Count();i++)
            {
                int e = list[i];
                MLATMessagesUsed += listzones[e].MLATMessagesUsed;
                MLATMessagesUsedheight += listzones[e].MLATMessagesUsedheight;
                ADSBMessagesUsed += listzones[e].ADSBMessagesUsed;
                DGPSMessagesUsed += listzones[e].DGPSMessagesUsed;
                MLATPrecision.AddRange(listzones[e].MLATPrecision);
                totalMLATPrecissionheight.AddRange(listzones[e].totalMLATPrecissionheight);
                NAC += listzones[e].NAC;
                NACused += listzones[e].NACused;
            }
        }

        public void CreateTotalPD(int[] list, List<DataZone> listzones)
        {
            ExpectedMessagesPD = 0;
            MissedMLATSPD = 0;
            ExpectedMessagesUP = 0;
            MissedMLATSUP = 0;
            expected_PDok = 0;
            PDwrong = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                int e = list[i];
                ExpectedMessagesPD += listzones[e].ExpectedMessagesPD;
                MissedMLATSPD += listzones[e].MissedMLATSPD;
                ExpectedMessagesUP += listzones[e].ExpectedMessagesUP;
                MissedMLATSUP += listzones[e].MissedMLATSUP;
                expected_PDok += listzones[e].expected_PDok;
                PDwrong += listzones[e].PDwrong;
            }
        }

        public string GetUR()
        {
            if (ExpectedMessagesUP == 0)
            {
                return "Uncomputed";
            }
            else if (MissedMLATSUP != 0)
            {
                double UR = (100 - ((100/Convert.ToDouble(ExpectedMessagesUP)) * Convert.ToDouble(MissedMLATSUP)));
                return (String.Format("{0:0.00}", UR) + "%");
            }
            else
            {
                return "100%";
            }
        }

        public string GetPD() // ACABAR
        {
            if (ExpectedMessagesPD == 0)
            {
                return "Uncomputed";
            }
            else if (MissedMLATSPD != 0)
            {
                double UR = (100 - ((100 / Convert.ToDouble(ExpectedMessagesPD)) * Convert.ToDouble(MissedMLATSPD)));
                return (String.Format("{0:0.00}", UR) + "%");
            }
            else
            {
                return "100%";
            }
        }

        public string get95()
        {
            if (MLATPrecision.Count > 20)
            {
                MLATPrecision.Sort();
                // int len = MLATPrecision.Count();
                double pos = (MLATPrecision.Count() * 0.95);
                int position = Convert.ToInt32(pos);
                return (String.Format("{0:0.00}", MLATPrecision[position])+"m");
            }
            else
            {
                return "Uncomputed";
            }
        }
        public string get99()
        {
            if (MLATPrecision.Count > 100)
            {
                MLATPrecision.Sort();
                // int len = MLATPrecision.Count();
                double pos = (MLATPrecision.Count()  * 0.99);
                int position = Convert.ToInt32(pos);
                if (position == MLATPrecision.Count()) { return (String.Format("{0:0.00}", MLATPrecision[position-1]) + "m"); }
                else { return (String.Format("{0:0.00}", MLATPrecision[position]) + "m"); }
            }
            else
            {
                return "Uncomputed";
            }
        }

        double media;
        public string GetMedia()
        {
            if (MLATPrecision.Count > 1)
            {
                double tot = 0;
                double count = 0;
                for (int i = 0; i < MLATPrecision.Count(); i++)
                {
                    if (MLATPrecision[i] > -1)
                    {
                        tot += MLATPrecision[i];
                        count++;
                    }
                }
                media = tot / count;
                return (String.Format("{0:0.00}",media) + "m");
            }
        
            else
            {
                return "Uncomputed";
            }

        }

        public string GetDesviacion()
        {
            if (MLATPrecision.Count>1)
            {
                double tot = 0;
                double count = 0;
                for (int i = 0; i < MLATPrecision.Count(); i++)
                {
                    if (MLATPrecision[i] > -1)
                    {
                        tot += MLATPrecision[i];
                        count++;
                    }
                }
                double media = tot / MLATPrecision.Count();
                double sum = 0;
                for (int i=0;i<MLATPrecision.Count();i++)
                {
                    if (MLATPrecision[i] > -1)
                    {
                        sum += Math.Pow((MLATPrecision[i] - media), 2);
                    }
                }
                double desviation = Math.Sqrt(sum / count);
                return (String.Format("{0:0.00}", desviation));
            }
            else
            {
                return "Uncomputed";
            }
        }
    }
}
