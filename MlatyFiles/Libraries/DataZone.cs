using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace PGTA_WPF
{
    public class DataZone
    {
        public int MLATMessagesUsed { get; set; } // 
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
        public int MissedMLATSPD_50m_Restriction = 0;
        public int MissedMLATSUP = 0;

        public string name;
        public int expected_PDok = 0;
        public int PDwrong = 0;
        public int DGPSMessagesUsed;

        public int CorrectDetection = 0;
        public int FalseDetection = 0;


        public int CorrectIdentification = 0;
        public int FalseIdentification = 0;

        public int CorrectPI = 0;
        public int IncorrectPI = 0;

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
            //MissedMLATSPD_50m_Restriction=0;
            expected_PDok = 0;
            PDwrong = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                int e = list[i];
                ExpectedMessagesPD += listzones[e].ExpectedMessagesPD;
                MissedMLATSPD += listzones[e].MissedMLATSPD;
                //MissedMLATSPD_50m_Restriction += listzones[e].MissedMLATSPD_50m_Restriction;
                expected_PDok += listzones[e].expected_PDok;
                PDwrong += listzones[e].PDwrong;
            }
        }

        public void CreateTotalUP(int[] list, List<DataZone> listzones)
        {
            ExpectedMessagesUP = 0;
            MissedMLATSUP = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                int e = list[i];
                ExpectedMessagesUP += listzones[e].ExpectedMessagesUP;
                MissedMLATSUP += listzones[e].MissedMLATSUP;
            }
        }

        public void CreateTotalPFD(int[] list, List<DataZone> listzones)
        {
            CorrectDetection = 0;
            FalseDetection = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                int e = list[i];
                CorrectDetection += listzones[e].CorrectDetection;
                FalseDetection += listzones[e].FalseDetection;
            }
        }


        public void CreateTotalPI(int[] list, List<DataZone> listzones)
        {
            CorrectPI = 0;
            IncorrectPI = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                int e = list[i];
                CorrectPI += listzones[e].CorrectPI;
                IncorrectPI += listzones[e].IncorrectPI;
            }
        }

        public void CreateTotalPFI(int[] list, List<DataZone> listzones)
        {
            CorrectIdentification = 0;
            FalseIdentification = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                int e = list[i];
                CorrectIdentification += listzones[e].CorrectIdentification;
                FalseIdentification += listzones[e].FalseIdentification;
            }
        }



        public string GetUR(double min)
        {
            if (ExpectedMessagesUP == 0)
            {
                return "Uncomputed";
            }
            else if (MissedMLATSUP != 0)
            {
                double UR = 100-((Convert.ToDouble(MissedMLATSUP) / Convert.ToDouble( ExpectedMessagesUP)) * 100);
                if (UR < min)
                {
                    return ("<" + String.Format("{0:0.00}", UR) + "%");
                }
                return (String.Format("{0:0.00}", UR) + "%");
            }
            else
            {
                return "100%";
            }
        }

        public string GetPD(double min) // ACABAR
        {
            if (ExpectedMessagesPD == 0)
            {
                return "Uncomputed";
            }
            else if (MissedMLATSPD != 0)
            {
                double PD = 100-((Convert.ToDouble(MissedMLATSPD) / Convert.ToDouble(ExpectedMessagesPD)) * 100); 
                if (PD/100 < min)
                {
                    return ("<"+String.Format("{0:0.00}", PD) + "%");
                }
                return (String.Format("{0:0.00}", PD) + "%");
            }
            else
            {
                return "100%";
            }
        }

        public string GetPD_50m_Restriction(double min) // ACABAR
        {
            if (ExpectedMessagesPD == 0)
            {
                return "Uncomputed";
            }
            else if (MissedMLATSPD_50m_Restriction != 0)
            {
                double PD = 100 - ((Convert.ToDouble(MissedMLATSPD_50m_Restriction) / Convert.ToDouble( ExpectedMessagesPD)) * 100);
                if (PD / 100 < min)
                {
                    return ("<" + String.Format("{0:0.00}", PD) + "%");
                }
                return (String.Format("{0:0.00}", PD) + "%");
            }
            else
            {
                return "100%";
            }
        }

        public string get95(double max95)
        {
            if (MLATPrecision.Count > 20)
            {
                MLATPrecision.Sort();
                // int len = MLATPrecision.Count();
                double pos = (MLATPrecision.Count() * 0.95);
                int position = Convert.ToInt32(pos);
                double press = MLATPrecision[position];
                if (max95 > 0)
                {
                    if (press > max95)
                    {
                        return ("<"+String.Format("{0:0.00}", MLATPrecision[position]) + "m");
                    }
                    else
                    {
                        return (String.Format("{0:0.00}", MLATPrecision[position]) + "m");

                    }
                }
                else
                {
                    return (String.Format("{0:0.00}", MLATPrecision[position]) + "m");

                }
            }
            else
            {
                return "Uncomputed";
            }
        }
        public string get99(double max99)
        {
            if (MLATPrecision.Count > 100)
            {
                MLATPrecision.Sort();
                // int len = MLATPrecision.Count();
                double pos = (MLATPrecision.Count()  * 0.99);
                int position = Convert.ToInt32(pos);
                double press = 0;
                if (position == MLATPrecision.Count())
                { 
                    press = MLATPrecision[position - 1];
                }
                else
                {
                    press = MLATPrecision[position];
                }
                if(max99>0)
                {
                    if(press>max99)
                    {
                        return ("<"+String.Format("{0:0.00}", MLATPrecision[position]) + "m");

                    }
                    else
                    {
                        return (String.Format("{0:0.00}", MLATPrecision[position]) + "m");

                    }
                }
                else 
                {
                    return (String.Format("{0:0.00}", MLATPrecision[position]) + "m");
                }
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

        public string GetPFD(double min)
        {
            if (FalseDetection+CorrectDetection > 0)
            {
                if(FalseDetection == 0)
                {
                    return ("0 %");
                }
                else
                {
                    double prob = ((Convert.ToDouble(FalseDetection) / Convert.ToDouble(CorrectDetection+FalseDetection))*100);
                    if(prob>min)
                    {
                        return $"<{String.Format("{0:0.00}", prob)} %";
                    }
                    return $"{String.Format("{0:0.00}", prob)} %";
                }
            }
            else
            {
                return "Uncomputed";
            }
        }

        public string GetPFI(double minPFI)
        {
            if (CorrectIdentification + FalseIdentification > 0)
            {
                if (FalseIdentification == 0)
                {
                    return ("0 %");
                }
                else
                {
                    double prob = ((Convert.ToDouble(FalseIdentification) / Convert.ToDouble(CorrectIdentification + FalseIdentification)) * 100);
                    if(prob/100<minPFI)
                    {
                        return $"<{String.Format("{0:0.00}", prob)} %";

                    }
                    return $"{String.Format("{0:0.00}", prob)} %";
                }
            }
            else
            {
                return "Uncomputed";
            }
        }

        public string GetPI(double minPI)
        {
            if (CorrectPI + IncorrectPI > 0)
            {
                if (IncorrectPI == 0)
                {
                    return ("100 %");
                }
                else
                {
                    double prob = ((Convert.ToDouble(CorrectPI) / Convert.ToDouble(CorrectPI + IncorrectPI)) * 100);
                    if(prob/100<minPI)
                    {
                        return $"<{String.Format("{0:0.00}", prob)} %";

                    }
                    return $"{String.Format("{0:0.00}", prob)} %";
                }
            }
            else
            {
                return "Uncomputed";
            }
        }
    }
}
