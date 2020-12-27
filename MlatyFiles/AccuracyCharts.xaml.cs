using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Win32;
using PGTA_WPF;

namespace Mlaty
{
    /// <summary>
    /// Lógica de interacción para AccuracyCharts.xaml
    /// </summary>
    public partial class AccuracyCharts : Page
    {
        public AccuracyCharts()
        {
            InitializeComponent();

            var r = new Random();
            Runway02 = new ChartValues<ObservablePoint>();
            Runway25L = new ChartValues<ObservablePoint>();
            Runway25R = new ChartValues<ObservablePoint>();
            StandT1 = new ChartValues<ObservablePoint>();
            StandT2 = new ChartValues<ObservablePoint>();
            TaxiZones = new ChartValues<ObservablePoint>();
            ApronT1 = new ChartValues<ObservablePoint>();
            ApronT2 = new ChartValues<ObservablePoint>();
            Airborn25L25 = new ChartValues<ObservablePoint>();
            Airborne25R25 = new ChartValues<ObservablePoint>();
            Airborne0225 = new ChartValues<ObservablePoint>();
            Airborn25L5 = new ChartValues<ObservablePoint>();
            Airborne25R5 = new ChartValues<ObservablePoint>();
            Airborne025 = new ChartValues<ObservablePoint>();

            Circle12m = new ChartValues<ObservablePoint>();
            Circle75m = new ChartValues<ObservablePoint>();
            Circle20m = new ChartValues<ObservablePoint>();
            Circle40m = new ChartValues<ObservablePoint>();


            Runway02Series = new ScatterSeries
            {
                Title = "Runway 02",
                Width = 1,

                Values = Runway02
            };
            Runway25LSeries = new ScatterSeries
            {
                Title = "Runway 25L",
                
                Values = Runway25L
            };
            Runway25RSeries = new ScatterSeries
            {
                Title = "Runway 25R",
                Width = 1,

                Values = Runway25R
            };
            StandT1Series = new ScatterSeries
            {
                Title = "Stand T1",
                Values = StandT1
            };
            StandT2Series = new ScatterSeries
            {
                Title = "Stand T2",
                Values = StandT2
            };
            ApronT1Series = new ScatterSeries
            {
                Title = "Apron T1",
                Values = ApronT1
            };
            ApronT2Series = new ScatterSeries
            {
                Title = "Apron T2",
                Values = ApronT2
            };
            TaxiSeries = new ScatterSeries
            {
                Title = "Taxi Zone",
                Values = TaxiZones
            };
            Airborne0225Series = new ScatterSeries
            {
                Title = "Airborne 02 0-2.5NM",
                Values = Airborne0225
            };
            Airborne25R25Series = new ScatterSeries
            {
                Title = "Airborne 25R 0-2.5NM",
                Values = Airborne25R25
            };
            Airborne25L25Series = new ScatterSeries
            {
                Title = "Ariborne 25L 0-2.5NM",
                Values = Airborn25L25
            };
            Airborne025Series = new ScatterSeries
            {
                Title = "Airborne 02 2.5-5NM",
                Values = Airborne025
            };
            Airborne25R5Series = new ScatterSeries
            {
                Title = "Airborne 25R 2.5-5NM",
                MaxPointShapeDiameter=0.5,

                Values = Airborne25R5
            };
            Airborne25L5Series = new ScatterSeries
            {
                Title = "Ariborne 25L 2.5-5NM",
               
                Values = Airborn25L5
            };
            Circle75mSeries = new LineSeries
            {
                Title = "Error < 7.5m",
                StrokeThickness = 2,
                PointGeometry = DefaultGeometries.None,

                Stroke = new SolidColorBrush(Color.FromArgb(255, (byte)255, (byte)0, (byte)0)),
                Fill = new SolidColorBrush(Color.FromArgb(10, (byte)255, (byte)0, (byte)0)),
                Values = Circle75m
            };
            Circle12mSeries = new LineSeries
            {
                Title= "Error < 12m",
                StrokeThickness=2,
                PointGeometry = DefaultGeometries.None,

                Stroke = new SolidColorBrush(Color.FromArgb(255, (byte)180, (byte)0, (byte)0)),
                Fill = new SolidColorBrush(Color.FromArgb(10, (byte)255, (byte)0, (byte)0)),
                Values =Circle12m
            };

            Circle20mSeries = new LineSeries
            {
                Title = "Error < 20m",
                StrokeThickness = 2,
                PointGeometry = DefaultGeometries.None,

                Stroke = new SolidColorBrush(Color.FromArgb(255, (byte)120, (byte)0, (byte)0)),
                Fill = new SolidColorBrush(Color.FromArgb(10, (byte)255, (byte)0, (byte)0)),
                Values = Circle20m
            };
            Circle40mSeries = new LineSeries
            {
                Title = "Error < 40m",
                StrokeThickness = 2,
                PointGeometry=DefaultGeometries.None,
                Stroke = new SolidColorBrush(Color.FromArgb(255, (byte)80, (byte)0, (byte)0)),
                Fill = new SolidColorBrush(Color.FromArgb(10, (byte)255, (byte)0, (byte)0)),
                Values = Circle40m
            };
            LiveChartScatter.DisableAnimations = true;
            LiveChartScatter.Hoverable = false;
            //  var tooltip = (LiveCharts.Defaults.t)LiveChartScatter.DataTooltip;
           // LiveChartScatter.DataTooltip.Visibility = Visibility.Collapsed;// LiveCharts.TooltipSelectionMode.OnlySender;
            LiveChartScatter.DataTooltip =null;
            LiveChartScatter.Zoom = ZoomingOptions.Xy;
            LiveChartScatter.LegendLocation = LegendLocation.Right;
            LiveChartScatter.ChartLegend.Foreground = new SolidColorBrush(Color.FromArgb(255, (byte)249, (byte)249, (byte)249));


            LiveChartScatter.Series = new SeriesCollection
            {

               StandT1Series,
               StandT2Series,
               ApronT1Series,
               ApronT2Series,
               TaxiSeries,

               Airborne0225Series,
               Airborne25L25Series,
               Airborne25R25Series,

               Runway02Series,
               Runway25LSeries,
               Runway25RSeries,

               Airborne025Series,
               Airborne25L5Series,
               Airborne25R5Series,

               Circle75mSeries,
               Circle12mSeries,
               Circle20mSeries,
               Circle40mSeries,
            };


            DataContext = this;
        }


        public LineSeries Circle12mSeries { get; set; }
        public LineSeries Circle75mSeries { get; set; }
        public LineSeries Circle20mSeries { get; set; }
        public LineSeries Circle40mSeries { get; set; }


        public ScatterSeries Runway02Series { get; set; }
        public ScatterSeries Runway25RSeries { get; set; }
        public ScatterSeries Runway25LSeries { get; set; }
        public ScatterSeries StandT1Series { get; set; }
        public ScatterSeries StandT2Series { get; set; }

        public ScatterSeries TaxiSeries { get; set; }
        public ScatterSeries ApronT1Series { get; set; }
        public ScatterSeries ApronT2Series { get; set; }
        public ScatterSeries Airborne0225Series { get; set; }
        public ScatterSeries Airborne25R25Series { get; set; }
        public ScatterSeries Airborne25L25Series { get; set; }
        public ScatterSeries Airborne025Series { get; set; }
        public ScatterSeries Airborne25R5Series { get; set; }
        public ScatterSeries Airborne25L5Series { get; set; }


        public ChartValues<ObservablePoint> Circle12m {get;set;}
        public ChartValues<ObservablePoint> Circle75m { get; set; }

        public ChartValues<ObservablePoint> Circle20m { get; set; }

        public ChartValues<ObservablePoint> Circle40m { get; set; }


        public ChartValues<ObservablePoint> Runway25L { get; set; }
        public ChartValues<ObservablePoint> Runway02 { get; set; }
        public ChartValues<ObservablePoint> Runway25R { get; set; }
        public ChartValues<ObservablePoint> StandT1 { get; set; }
        public ChartValues<ObservablePoint> StandT2 { get; set; }
        public ChartValues<ObservablePoint> TaxiZones { get; set; }
        public ChartValues<ObservablePoint> ApronT1 { get; set; }
        public ChartValues<ObservablePoint> ApronT2 { get; set; }
        public ChartValues<ObservablePoint> Airborn25L25 { get; set; }
        public ChartValues<ObservablePoint> Airborne0225 { get; set; }
        public ChartValues<ObservablePoint> Airborne25R25 { get; set; }
        public ChartValues<ObservablePoint> Airborn25L5 { get; set; }
        public ChartValues<ObservablePoint> Airborne025 { get; set; }
        public ChartValues<ObservablePoint> Airborne25R5 { get; set; }

        private void ChartsPageLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void SetAxis(double minX, double maxX, double minY, double maxY)
        {
            maxX += 10;
            maxY += 10;
            minX -= 10;
            minY -= 10;

            double Y = maxY - minY;
            double X = (maxX - minX);
            if(X>Y*1.25)
            {

                X = (maxX - minX);
                double dif = X / Y;
                if (dif > 1.25)
                {
                    dif -= 0.25;
                    maxY = maxY * dif;
                    minY = minY * dif;
                }
                else
                {
                    dif = 1.25 - dif;
                    dif = 1 + dif;
                    maxX = maxX * dif;
                    minX = minX * dif;
                }

            }
            else
            {
                double dif = Y / X;
                maxX = maxX * dif;
                minX = minX * dif; 
                maxX = maxX * 1.25;
                minX = minX * 1.25;
            }

            LiveChartScatter.AxisY.Clear();
            LiveChartScatter.AxisX.Clear();
            LiveChartScatter.AxisY.Add(
            new Axis
            {
                MinValue =minY,
                Foreground = Brushes.White,
                FontSize = 16,
                Title = "Y difference (m)",
                MaxValue = maxY
            });
            LiveChartScatter.AxisX.Add(
            new Axis
            {
                MinValue = minX,
                Foreground = Brushes.White,
                FontSize = 16,
                Title = "X difference (m)",
                MaxValue = maxX
            });
        }


        public void GetValues(List<PrecissionPoint> ListPoints)
        {
            double minY=-40;
            double minX=-40;
            double maxY=40;
            double maxX=40;
            foreach(PrecissionPoint p in ListPoints)
            {
                if(p.ErrorLocalX>maxX)
                {
                    maxX = p.ErrorLocalX;
                }
                if (p.ErrorLocalX < minX)
                {
                    minX = p.ErrorLocalX;
                }
                if (p.ErrorLocalY > maxY)
                {
                    maxY = p.ErrorLocalY;
                }
                if (p.ErrorLocalY < minY)
                {
                    minY = p.ErrorLocalY;
                }
                if (p.Area== "Runway25L")
                {
                    Runway25L.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "Runway25R")
                {
                    Runway25R.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "Runway02")
                {
                    Runway02.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "StandT1")
                {
                    StandT1.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "StandT2")
                {
                    StandT2.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "TaxiZones")
                {
                    TaxiZones.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "ApronT2")
                {
                    ApronT2.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "ApronT1")
                {
                    ApronT1.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "Airborne25RZones25")
                {
                    Airborne25R25.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "Airborne02Zones25")
                {
                    Airborne025.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "Airborne25LZones25")
                {
                    Airborn25L25.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "Airborne25RZones5")
                {
                    Airborne25R25.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "Airborne02Zones5")
                {
                    Airborne025.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }
                if (p.Area == "Airborne25LZones5")
                {
                    Airborn25L25.Add(new ObservablePoint(p.ErrorLocalX, p.ErrorLocalY));
                }


               
            }

                SetAxis(minX, maxX, minY, maxY);
            
            CreateCircles();

           

            //Airborne25L25Series.Visibility = Visibility.Hidden;
            //Airborne0225Series.Visibility = Visibility.Hidden;
            //Airborne25R25Series.Visibility = Visibility.Hidden;
            //Airborne25L5Series.Visibility = Visibility.Hidden;
            //Airborne025Series.Visibility = Visibility.Hidden;
            //Airborne25R5Series.Visibility = Visibility.Hidden;


        }

        private void CreateCircles()
        {
            for(int i=0;i< 360; i++)
            {
                double angle = i;/// 10;
                angle=(Math.PI*angle)/ 180;
                Circle12m.Add(new ObservablePoint(Math.Cos(angle)*12,Math.Sin(angle)*12));
                Circle75m.Add(new ObservablePoint(Math.Cos(angle)*7.5,Math.Sin(angle)*7.5));
                Circle20m.Add(new ObservablePoint(Math.Cos(angle) * 20, Math.Sin(angle) * 20));
                Circle40m.Add(new ObservablePoint(Math.Cos(angle) * 40, Math.Sin(angle) * 40));

            }
        }

        bool first = true;


        private void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Filter = "png files (*.png*)|*.png*";//|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            bool correct = false;
            if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
            {
                string path0 = saveFileDialog1.FileName;
                string path = path0;
                if (path0.Length > 3 && path0.Substring(path0.Length - 5, 4) != ".png")
                {
                    path = path + ".png";
                }
                // bool correct=false;
                if (File.Exists(path) == false) { correct = true; }
                while (correct == false)
                {
                    correct = false;
                    MessageboxYesNo deleteform = new MessageboxYesNo();
                    deleteform.getChartsPage(this);
                    deleteform.ShowDialog();
                    if (this.repeteddelete == true)
                    {
                        File.Delete(path);
                        correct = true;
                    }
                    else
                    {
                        saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog1.Filter = "png files (*.png*)|*.png*";//|*.txt|All files (*.*)|*.*";

                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
                        {
                            path0 = saveFileDialog1.FileName;
                            path = path0;
                            if (path0.Length > 3 && path0.Substring(path0.Length - 5, 4) != ".png")
                            {
                                path = path + ".png";
                            }
                        }
                        if (File.Exists(path) == false) { correct = true; }

                    }
                }
                if (correct == true)
                {
                    SaveToPng(LiveChartScatter, path);
                }
            }
        }

        public bool repeteddelete = false;
        public void getaction(bool delete)
        {
            this.repeteddelete = delete;
        }
        public void SaveToPng(FrameworkElement visual, string fileName)
        {
            var encoder = new PngBitmapEncoder();
            EncodeVisual(visual, fileName, encoder);
        }

        private static void EncodeVisual(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        {
            var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 86, 86, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            encoder.Frames.Add(frame);
            using (var stream = File.Create(fileName)) encoder.Save(stream);
        }

        private void ShowClick(object sender, RoutedEventArgs e)
        {
            CheckBox box = sender as CheckBox;
            string name = box.Name;
            if(name=="RunwaysBox")
            {
                if(RunwaysBox.IsChecked==true)
                {
                    Runway02Series.Visibility = Visibility.Visible;
                    Runway25RSeries.Visibility = Visibility.Visible;
                    Runway25LSeries.Visibility = Visibility.Visible;

                    ShowRunway25LBox.IsChecked = true;
                    ShowRunway25RBox.IsChecked=true;
                    ShowRunway02Box.IsChecked=true;
                }
                else
                {
                    Runway02Series.Visibility = Visibility.Collapsed;
                    Runway25RSeries.Visibility = Visibility.Collapsed;
                    Runway25LSeries.Visibility = Visibility.Collapsed;

                    ShowRunway25LBox.IsChecked = false;
                    ShowRunway25RBox.IsChecked = false;
                    ShowRunway02Box.IsChecked = false;
                }
            }
            if(name== "ShowRunway25LBox")
            {
                if(box.IsChecked==true)
                {
                    Runway25LSeries.Visibility = Visibility.Visible;
                    if(ShowRunway25RBox.IsChecked==true && ShowRunway02Box.IsChecked==true)
                    {
                        RunwaysBox.IsChecked = true;
                    }
                }
                else
                {
                    if(RunwaysBox.IsChecked==true)
                    {
                        RunwaysBox.IsChecked = false;
                    }
                    Runway25LSeries.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "ShowRunway25RBox")
            {
                if (box.IsChecked == true)
                {
                    Runway25RSeries.Visibility = Visibility.Visible;
                    if (ShowRunway25LBox.IsChecked == true && ShowRunway02Box.IsChecked == true)
                    {
                        RunwaysBox.IsChecked = true;
                    }
                }
                else
                {
                    if (RunwaysBox.IsChecked == true)
                    {
                        RunwaysBox.IsChecked = false;
                    }
                    Runway25RSeries.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "ShowRunway02Box")
            {
                if (box.IsChecked == true)
                {
                    Runway02Series.Visibility = Visibility.Visible;
                    if (ShowRunway25RBox.IsChecked == true && ShowRunway25LBox.IsChecked == true)
                    {
                        RunwaysBox.IsChecked = true;
                    }
                }
                else
                {
                    if (RunwaysBox.IsChecked == true)
                    {
                        RunwaysBox.IsChecked = false;
                    }
                    Runway02Series.Visibility = Visibility.Collapsed;
                }
            }
            if(name=="StandsBox")
            {
                if (box.IsChecked == true)
                {
                   StandT1Series.Visibility = Visibility.Visible;
                   StandT2Series.Visibility = Visibility.Visible;

                    StandT1Box.IsChecked = true;
                    StandT2Box.IsChecked = true;
                }
                else
                {
                    StandT1Series.Visibility = Visibility.Collapsed;
                    StandT2Series.Visibility = Visibility.Collapsed;

                    StandT1Box.IsChecked = false;
                    StandT2Box.IsChecked = false;
                }
            }
            if(name=="StandT1Box")
            {
                if(box.IsChecked==true)
                {
                    StandT1Series.Visibility = Visibility.Visible;
                    if(StandT2Box.IsChecked==true)
                    {
                        StandsBox.IsChecked = true;
                    }
                }
                else
                {
                    if(StandsBox.IsChecked==true)
                    {
                        StandsBox.IsChecked = false;
                    }
                    StandT1Series.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "StandT2Box")
            {
                if (box.IsChecked == true)
                {
                    StandT2Series.Visibility = Visibility.Visible;
                    if (StandT1Box.IsChecked == true)
                    {
                        StandsBox.IsChecked = true;
                    }
                }
                else
                {
                    if (StandsBox.IsChecked == true)
                    {
                        StandsBox.IsChecked = false;
                    }
                    StandT2Series.Visibility = Visibility.Collapsed;
                }
            }
            if (name=="ApronsBox")
            {
                if (box.IsChecked == true)
                {
                    ApronT1Series.Visibility = Visibility.Visible;
                    ApronT2Series.Visibility = Visibility.Visible;
                    
                    ApronT1Box.IsChecked = true;
                    ApronT2Box.IsChecked = true;
                }
                else
                {
                    ApronT1Series.Visibility = Visibility.Collapsed;
                    ApronT2Series.Visibility = Visibility.Collapsed;
                    
                    ApronT1Box.IsChecked = false;
                    ApronT2Box.IsChecked = false;
                }
            }
            if (name == "ApronT1Box")
            {
                if (box.IsChecked == true)
                {
                    ApronT1Series.Visibility = Visibility.Visible;
                    if (ApronT2Box.IsChecked == true)
                    {
                        ApronsBox.IsChecked = true;
                    }
                }
                else
                {
                    if (ApronsBox.IsChecked == true)
                    {
                        ApronsBox.IsChecked = false;
                    }
                    ApronT1Series.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "ApronT2Box")
            {
                if (box.IsChecked == true)
                {
                    ApronT2Series.Visibility = Visibility.Visible;
                    if (ApronT2Box.IsChecked == true)
                    {
                        ApronsBox.IsChecked = true;
                    }
                }
                else
                {
                    if (ApronsBox.IsChecked == true)
                    {
                        ApronsBox.IsChecked = false;
                    }
                    ApronT2Series.Visibility = Visibility.Collapsed;
                }
            }
            if(name=="TaxiBox")
            {
                if(box.IsChecked==true)
                {
                    TaxiSeries.Visibility = Visibility.Visible;
                }
                else
                {
                    TaxiSeries.Visibility = Visibility.Collapsed;
                }
            }

            if (name == "Airbornes25Box")
            {
                if (box.IsChecked == true)
                {
                    Airborne0225Series.Visibility = Visibility.Visible;
                    Airborne25R25Series.Visibility = Visibility.Visible;
                    Airborne25L25Series.Visibility = Visibility.Visible;

                    AirborneRunway25L25Box.IsChecked = true;
                    AirborneRunway25R25Box.IsChecked = true;
                    AirborneRunway0225Box.IsChecked = true;
                }
                else
                {
                    Airborne0225Series.Visibility = Visibility.Collapsed;
                    Airborne25R25Series.Visibility = Visibility.Collapsed;
                    Airborne25L25Series.Visibility = Visibility.Collapsed;

                    AirborneRunway25L25Box.IsChecked = false;
                    AirborneRunway25R25Box.IsChecked = false;
                    AirborneRunway0225Box.IsChecked = false;
                }
            }
            if (name == "AirborneRunway25L25Box")
            {
                if (box.IsChecked == true)
                {
                    Airborne25L25Series.Visibility = Visibility.Visible;
                    if (AirborneRunway25R25Box.IsChecked == true && AirborneRunway0225Box.IsChecked == true)
                    {
                        Airbornes25Box.IsChecked = true;
                    }
                }
                else
                {
                    if (Airbornes25Box.IsChecked == true)
                    {
                        Airbornes25Box.IsChecked = false;
                    }
                    Airborne25L25Series.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "AirborneRunway25R25Box")
            {
                if (box.IsChecked == true)
                {
                    Airborne25R25Series.Visibility = Visibility.Visible;
                    if (AirborneRunway25L25Box.IsChecked == true && AirborneRunway0225Box.IsChecked == true)
                    {
                        Airbornes25Box.IsChecked = true;
                    }
                }
                else
                {
                    if (Airbornes25Box.IsChecked == true)
                    {
                        Airbornes25Box.IsChecked = false;
                    }
                    Airborne25R25Series.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "AirborneRunway0225Box")
            {
                if (box.IsChecked == true)
                {
                    Airborne0225Series.Visibility = Visibility.Visible;
                    if (AirborneRunway25R25Box.IsChecked == true && AirborneRunway25L25Box.IsChecked == true)
                    {
                        Airbornes25Box.IsChecked = true;
                    }
                }
                else
                {
                    if (Airbornes25Box.IsChecked == true)
                    {
                        Airbornes25Box.IsChecked = false;
                    }
                    Airborne0225Series.Visibility = Visibility.Collapsed;
                }
            }


            if (name == "Airbornes5Box")
            {
                if (box.IsChecked == true)
                {
                    Airborne025Series.Visibility = Visibility.Visible;
                    Airborne25R5Series.Visibility = Visibility.Visible;
                    Airborne25L5Series.Visibility = Visibility.Visible;

                    AirborneRunway25L5Box.IsChecked = true;
                    AirborneRunway25R5Box.IsChecked = true;
                    AirborneRunway025Box.IsChecked = true;
                }
                else
                {
                    Airborne025Series.Visibility = Visibility.Collapsed;
                    Airborne25R5Series.Visibility = Visibility.Collapsed;
                    Airborne25L5Series.Visibility = Visibility.Collapsed;

                    AirborneRunway25L5Box.IsChecked = false;
                    AirborneRunway25R5Box.IsChecked = false;
                    AirborneRunway025Box.IsChecked = false;
                }
            }
            if (name == "AirborneRunway25L5Box")
            {
                if (box.IsChecked == true)
                {
                    Airborne25L5Series.Visibility = Visibility.Visible;
                    if (AirborneRunway25R5Box.IsChecked == true && AirborneRunway025Box.IsChecked == true)
                    {
                        Airbornes5Box.IsChecked = true;
                    }
                }
                else
                {
                    if (Airbornes5Box.IsChecked == true)
                    {
                        Airbornes5Box.IsChecked = false;
                    }
                    Airborne25L5Series.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "AirborneRunway25R5Box")
            {
                if (box.IsChecked == true)
                {
                    Airborne25R5Series.Visibility = Visibility.Visible;
                    if (AirborneRunway25L5Box.IsChecked == true && AirborneRunway025Box.IsChecked == true)
                    {
                        Airbornes5Box.IsChecked = true;
                    }
                }
                else
                {
                    if (Airbornes5Box.IsChecked == true)
                    {
                        Airbornes5Box.IsChecked = false;
                    }
                    Airborne25R5Series.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "AirborneRunway025Box")
            {
                if (box.IsChecked == true)
                {
                    Airborne025Series.Visibility = Visibility.Visible;
                    if (AirborneRunway25R5Box.IsChecked == true && AirborneRunway25L5Box.IsChecked == true)
                    {
                        Airbornes5Box.IsChecked = true;
                    }
                }
                else
                {
                    if (Airbornes5Box.IsChecked == true)
                    {
                        Airbornes5Box.IsChecked = false;
                    }
                    Airborne025Series.Visibility = Visibility.Collapsed;
                }
            }

            if (name == "CirclesBox")
            {
                if (box.IsChecked == true)
                {
                    Circle75mSeries.Visibility = Visibility.Visible;
                    Circle12mSeries.Visibility = Visibility.Visible;
                    Circle20mSeries.Visibility = Visibility.Visible;
                    Circle40mSeries.Visibility = Visibility.Visible;

                    Circle75Box.IsChecked = true;
                    Circle12Box.IsChecked = true;
                    Circle20Box.IsChecked = true;
                    Circle40Box.IsChecked = true;

                }
                else
                {
                    Circle75mSeries.Visibility = Visibility.Collapsed;
                    Circle12mSeries.Visibility = Visibility.Collapsed;
                    Circle20mSeries.Visibility = Visibility.Collapsed;
                    Circle40mSeries.Visibility = Visibility.Collapsed;

                    Circle75Box.IsChecked = false;
                    Circle12Box.IsChecked = false;
                    Circle20Box.IsChecked = false;
                    Circle40Box.IsChecked = false;
                }
            }
            if (name == "Circle75Box")
            {
                if (box.IsChecked == true)
                {
                    Circle75mSeries.Visibility = Visibility.Visible;
                    if (Circle12Box.IsChecked == true && Circle20Box.IsChecked == true && Circle40Box.IsChecked == true)
                    {
                        CirclesBox.IsChecked = true;
                    }
                }
                else
                {
                    if (CirclesBox.IsChecked == true)
                    {
                        CirclesBox.IsChecked = false;
                    }
                    Circle75mSeries.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "Circle12Box")
            {
                if (box.IsChecked == true)
                {
                    Circle12mSeries.Visibility = Visibility.Visible;
                    if (Circle75Box.IsChecked == true && Circle20Box.IsChecked == true && Circle40Box.IsChecked == true)
                    {
                        CirclesBox.IsChecked = true;
                    }
                }
                else
                {
                    if (CirclesBox.IsChecked == true)
                    {
                        CirclesBox.IsChecked = false;
                    }
                    Circle12mSeries.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "Circle20Box")
            {
                if (box.IsChecked == true)
                {
                    Circle20mSeries.Visibility = Visibility.Visible;
                    if (Circle12Box.IsChecked == true && Circle75Box.IsChecked == true && Circle40Box.IsChecked == true)
                    {
                        CirclesBox.IsChecked = true;
                    }
                }
                else
                {
                    if (CirclesBox.IsChecked == true)
                    {
                        CirclesBox.IsChecked = false;
                    }
                    Circle20mSeries.Visibility = Visibility.Collapsed;
                }
            }
            if (name == "Circle40Box")
            {
                if (box.IsChecked == true)
                {
                    Circle40mSeries.Visibility = Visibility.Visible;
                    if (Circle12Box.IsChecked == true && Circle20Box.IsChecked == true && Circle75Box.IsChecked == true)
                    {
                        CirclesBox.IsChecked = true;
                    }
                }
                else
                {
                    if (CirclesBox.IsChecked == true)
                    {
                        CirclesBox.IsChecked = false;
                    }
                    Circle40mSeries.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Resize(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();            
            if (PageScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible && PageScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
            {
                ScrollBorder.Visibility = Visibility.Visible;
            }
            else
            {
                ScrollBorder.Visibility = Visibility.Collapsed;
            }
        }
    }
    
}
