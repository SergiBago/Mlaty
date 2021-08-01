using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core;

namespace Mlaty
{
    /// <summary>
    /// Lógica de interacción para AccuracyCharts.xaml
    /// </summary>
    /// 
    


    public partial class AccuracyChartsSciCharts : Page
    {

        List<PrecissionPoint> ListPoints = new List<PrecissionPoint>();
        XyDataSeries<double, double> Runway02 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Runway25L = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Runway25R = new XyDataSeries<double, double>();
        XyDataSeries<double, double> StandT1 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> StandT2 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> TaxiZones = new XyDataSeries<double, double>();
        XyDataSeries<double, double> ApronT1 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> ApronT2 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Airborne25L25 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Airborne25R25 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Airborne0225 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Airborne25L5 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Airborne25R5 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Airborne025 = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Circle12m = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Circle75m = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Circle20m = new XyDataSeries<double, double>();
        XyDataSeries<double, double> Circle40m = new XyDataSeries<double, double>();

        XyScatterRenderableSeries Runway02Series;
        XyScatterRenderableSeries Runway25LSeries;
        XyScatterRenderableSeries Runway25RSeries;
        XyScatterRenderableSeries StandT1Series;
        XyScatterRenderableSeries StandT2Series;
        XyScatterRenderableSeries TaxiSeries;
        XyScatterRenderableSeries ApronT1Series;
        XyScatterRenderableSeries ApronT2Series;
        XyScatterRenderableSeries Airborne25L25Series;
        XyScatterRenderableSeries Airborne25R25Series;
        XyScatterRenderableSeries Airborne0225Series;
        XyScatterRenderableSeries Airborne25L5Series;
        XyScatterRenderableSeries Airborne25R5Series;
        XyScatterRenderableSeries Airborne025Series;
        FastLineRenderableSeries Circle12Series;
        FastLineRenderableSeries Circle75Series;
        FastLineRenderableSeries Circle20Series;
        FastLineRenderableSeries Circle40Series;

        Color Runway02Color = Color.FromArgb(255, 255, 255, 0);
        Color Runway25LColor = Color.FromArgb(255, 200, 200, 0);
        Color Runway25RColor = Color.FromArgb(255, 150,150, 0);
        Color StandT1Color = Color.FromArgb(255, 200, 200, 200);
        Color StandT2Color = Color.FromArgb(255, 150, 150, 150);
        Color TaxiZonesColor = Color.FromArgb(255, 255, 137, 0);
        Color ApronT1Color = Color.FromArgb(255, 255, 91, 102);
        Color ApronT2Color = Color.FromArgb(255, 210, 0, 14);
        Color Airborne25L25Color = Color.FromArgb(255, 255, 110, 255);
        Color Airborne25R25Color = Color.FromArgb(255, 229, 0, 229);
        Color Airborne0225Color = Color.FromArgb(255, 137, 0, 137);
        Color Airborne25L5Color = Color.FromArgb(255, 255, 47, 165);
        Color Airborne25R5Color = Color.FromArgb(255, 203, 0, 115);
        Color Airborne025Color = Color.FromArgb(255, 149, 29, 97);
        Color Circle12Color = Color.FromArgb(255, 255, 0, 0);
        Color Circle75Color = Color.FromArgb(255, 255, 0, 0);
        Color Circle20Color = Color.FromArgb(255, 255, 0, 0);
        Color Circle40Color = Color.FromArgb(255, 255, 0, 0);


        public AccuracyChartsSciCharts()
        {
            InitializeComponent();
            YAxisDragModifier YAxisDrag = new YAxisDragModifier();
            XAxisDragModifier XAxisDrag = new XAxisDragModifier();
            MouseWheelZoomModifier WheelZoom = new MouseWheelZoomModifier();
            RubberBandXyZoomModifier RubberMod = new RubberBandXyZoomModifier()
            {
                ExecuteOn = ExecuteOn.MouseRightButton,
                ReceiveHandledEvents = false,
            };
            ZoomPanModifier Pan = new ZoomPanModifier()
            {
                ExecuteOn = ExecuteOn.MouseLeftButton,
                //          ReceiveHandledEvents = false,
            };
            ZoomExtentsModifier ZoomExtents = new ZoomExtentsModifier()
            {
            };
            LegendModifier legendModifier = new LegendModifier()
            {
                ShowLegend = true,
                Orientation = Orientation.Vertical,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                LegendPlacement = LegendPlacement.Inside,
                Margin = new Thickness(0, 10, 10, 0),
                Background = new SolidColorBrush(Color.FromArgb(255, 60, 60, 60)),
                FontFamily = new FontFamily("Microsoft Sans Serif"),

            };
      
            sciChart.ChartModifier = new ModifierGroup(Pan,RubberMod,YAxisDrag, legendModifier, XAxisDrag, WheelZoom, ZoomExtents);

        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            SetGraphSeries();
            SetDataSeriesData();
            Mouse.OverrideCursor = null;
        }

        public void GetData(List<PrecissionPoint> ListPoints)
        {
            this.ListPoints = ListPoints;
        }


        private void SetGraphSeries()
        {
              Runway02Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,


            };

            Runway02Series.DataSeries = Runway02;
            Runway02.SeriesName = "Runway 02";
            Runway02.AcceptsUnsortedData = true;
            Runway02Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke= Runway02Color, Fill=Runway02Color};
            sciChart.RenderableSeries.Add(Runway02Series);
              Runway25LSeries = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,


            };
            Runway25LSeries.DataSeries = Runway25L;
            Runway25L.SeriesName = "Runway 25L";
            Runway25L.AcceptsUnsortedData = true;
            Runway25LSeries.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke=Runway25LColor, Fill = Runway25LColor };

            sciChart.RenderableSeries.Add(Runway25LSeries);
              Runway25RSeries = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,


            };
            Runway25RSeries.DataSeries = Runway25R;
            Runway25R.SeriesName = "Runway 25R";
            Runway25R.AcceptsUnsortedData = true;
            Runway25RSeries.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke=Runway25RColor, Fill= Runway25RColor };

            sciChart.RenderableSeries.Add(Runway25RSeries);
              StandT1Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,

            };
            StandT1Series.DataSeries = StandT1;
            StandT1.SeriesName = "Stand T1";
            StandT1.AcceptsUnsortedData = true;
            StandT1Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke=StandT1Color, Fill= StandT1Color };

            sciChart.RenderableSeries.Add(StandT1Series);
              StandT2Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,

            };
            StandT2Series.DataSeries = StandT2;
            StandT2.SeriesName = "Stand T2";
            StandT2.AcceptsUnsortedData = true;
            StandT2Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5, Stroke=StandT2Color , Fill= StandT2Color };

            sciChart.RenderableSeries.Add(StandT2Series);
              ApronT1Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,

            };
            ApronT1Series.DataSeries = ApronT1;
            ApronT1.SeriesName = "Apron T1";
            ApronT1.AcceptsUnsortedData = true;
            ApronT1Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke=ApronT1Color, Fill= ApronT1Color };

            sciChart.RenderableSeries.Add(ApronT1Series);
              ApronT2Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,

            };
            ApronT2Series.DataSeries = ApronT2;
            ApronT2.SeriesName = "Apron T2";
            ApronT2.AcceptsUnsortedData = true;
            ApronT2Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke=ApronT2Color, Fill= ApronT2Color };

            sciChart.RenderableSeries.Add(ApronT2Series);
              TaxiSeries = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,

            };
            TaxiSeries.DataSeries = TaxiZones;
            TaxiZones.SeriesName = "Taxi Zones";
            TaxiZones.AcceptsUnsortedData = true;
            TaxiSeries.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5, Stroke=TaxiZonesColor, Fill= TaxiZonesColor };

            sciChart.RenderableSeries.Add(TaxiSeries);
              Airborne0225Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,

            };
            Airborne0225Series.DataSeries = Airborne0225;
            Airborne0225.AcceptsUnsortedData = true;
            Airborne0225.SeriesName = "Airborne 02 0-2.5NM";
            Airborne0225Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke=Airborne0225Color, Fill= Airborne0225Color };

            sciChart.RenderableSeries.Add(Airborne0225Series);
              Airborne25R25Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,

            };
            Airborne25R25Series.DataSeries = Airborne25R25;
            Airborne25R25Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5, Stroke=Airborne25R25Color , Fill= Airborne25R25Color };

            Airborne25R25.SeriesName = "Airborne 25R 0-2.5NM";
            Airborne25R25.AcceptsUnsortedData = true;
            sciChart.RenderableSeries.Add(Airborne25R25Series);
              Airborne25L25Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,
            };
            Airborne25L25Series.DataSeries = Airborne25L25;
            Airborne25L25Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke= Airborne25L25Color, Fill= Airborne25L25Color };

            Airborne25L25.SeriesName= "Airborne 25L 0 - 2.5NM";
            Airborne25L25.AcceptsUnsortedData = true;
            sciChart.RenderableSeries.Add(Airborne25L25Series);
              Airborne025Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,
                
            };
            Airborne025Series.DataSeries = Airborne025;
            Airborne025Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5, Stroke=Airborne025Color, Fill= Airborne025Color };

            Airborne025.SeriesName = "Airborne 02 2.5-5NM";
            Airborne025.AcceptsUnsortedData = true;
            sciChart.RenderableSeries.Add(Airborne0225Series);
              Airborne25R5Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,
            };
            Airborne25R5Series.DataSeries = Airborne25R5;
            Airborne25R25Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5, Stroke=Airborne25R5Color, Fill= Airborne25R5Color };

            Airborne25R25.SeriesName = "Airborne 25R 2.5-5NM";
            Airborne25R25.AcceptsUnsortedData = true;
            sciChart.RenderableSeries.Add(Airborne25R25Series);

              Airborne25L5Series = new XyScatterRenderableSeries
            {
                Visibility = Visibility.Visible,
            };
            Airborne25L25Series.DataSeries = Airborne25L5;
            Airborne25L25Series.PointMarker = new EllipsePointMarker() { Width = 5, Height = 5 , Stroke=Airborne25L5Color, Fill= Airborne25L5Color };

            Airborne25L25.AcceptsUnsortedData = true;
            Airborne25L5.SeriesName = "Airborne 25L 2.5-5NM";
            sciChart.RenderableSeries.Add(Airborne25L5Series);

              Circle12Series = new FastLineRenderableSeries()
            {
                StrokeThickness = 1,
                Stroke = Circle12Color
            };
            Circle12Series.DataSeries = Circle12m;
            Circle12m.AcceptsUnsortedData = true;
            Circle12m.SeriesName = "Circle 12m";
            sciChart.RenderableSeries.Add(Circle12Series);


              Circle75Series = new FastLineRenderableSeries()
            {
                StrokeThickness = 1,
                Stroke = Circle75Color
            };
            Circle75Series.DataSeries = Circle75m;
            Circle75m.AcceptsUnsortedData = true;
            Circle75m.SeriesName = "Circle 7.5m";
            sciChart.RenderableSeries.Add(Circle75Series);


              Circle20Series = new FastLineRenderableSeries()
            {
                StrokeThickness = 1,
                Stroke= Circle20Color
            };
            Circle20Series.DataSeries = Circle20m;
            Circle20m.AcceptsUnsortedData = true;
            Circle20m.SeriesName = "Circle 20m";
            sciChart.RenderableSeries.Add(Circle20Series);

              Circle40Series = new FastLineRenderableSeries()
            {
                StrokeThickness = 1,
                Stroke = Circle40Color
            };
            Circle40Series.DataSeries = Circle40m;
            Circle40m.AcceptsUnsortedData = true;
            Circle40m.SeriesName = "Circle 40m";
            sciChart.RenderableSeries.Add(Circle40Series);
            DataContext = this;
        }


        public void SetDataSeriesData()
        {

            Parallel.ForEach(ListPoints, p =>
             {

                 if (p.Area == "Runway25L")
                 {
                     Runway25L.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "Runway25R")
                 {
                     Runway25R.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "Runway02")
                 {
                     Runway02.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "StandT1")
                 {
                     StandT1.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "StandT2")
                 {
                     StandT2.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "TaxiZones")
                 {
                     TaxiZones.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "ApronT2")
                 {
                     ApronT2.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "ApronT1")
                 {
                     ApronT1.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "Airborne25RZones25")
                 {
                     Airborne25R25.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "Airborne02Zones25")
                 {
                     Airborne025.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "Airborne25LZones25")
                 {
                     Airborne25L25.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "Airborne25RZones5")
                 {
                     Airborne25R25.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "Airborne02Zones5")
                 {
                     Airborne025.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }
                 if (p.Area == "Airborne25LZones5")
                 {
                     Airborne25L25.Append(p.ErrorLocalX, p.ErrorLocalY);
                 }



             });

            //    SetAxis(minX, maxX, minY, maxY);
            
            CreateCircles();


        }

        private void CreateCircles()
        {
            for(int i=0;i< 360; i++)
            {
                double angle = i;/// 10;
                angle=(Math.PI*angle)/ 180;
                Circle12m.Append(Math.Cos(angle) * 12, Math.Sin(angle) * 12);
                Circle75m.Append(Math.Cos(angle) * 7.5, Math.Sin(angle) * 7.5);
                Circle20m.Append(Math.Cos(angle) * 20, Math.Sin(angle) * 20);
                Circle40m.Append(Math.Cos(angle) * 40, Math.Sin(angle) * 40);

            }
        }

        bool first = true;



        //private bool GetAndCheckPath(string filter, out string path)
        //{
        //    var ret = false;
        //    var isGoodPath = false;
        //    var saveFileDialog = CreateFileDialog(filter);
        //    path = null;

        //    while (!isGoodPath)
        //    {
        //        if (saveFileDialog.ShowDialog() == true)
        //        {
        //            if (IsFileGoodForWriting(saveFileDialog.FileName))
        //            {
        //                path = saveFileDialog.FileName;
        //                isGoodPath = true;
        //                ret = true;
        //            }
        //            else
        //            {
        //                MessageBox.Show(
        //                    "File is inaccesible for writing or you can not create file in this location, please choose another one.");
        //            }
        //        }
        //        else
        //        {
        //            isGoodPath = true;
        //        }
        //    }

        //    return ret;
        //}


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
                    sciChart.ExportToFile(path, ExportType.Png, false);
                  //  SaveToPng(LiveChartScatter, path);
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

      
        private void Resize(object sender, SizeChangedEventArgs e)
        {
            //UpdateLayout();            
            //if (PageScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible && PageScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
            //{
            //    ScrollBorder.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    ScrollBorder.IsEnabled = false;
            //}
        }


    }
    
}
