using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using GMap.NET.MapProviders;
using GMap.NET;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Drawing;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using GMap.NET.WindowsPresentation;

namespace PGTA_WPF
{
    /// <summary>
    /// Lógica de interacción para MAP.xaml
    /// </summary>
    public partial class MAP : Page
    {
        public MAP()
        {
            InitializeComponent();
            ShowPlygons();
        }
        PointLatLng ARPBarcelona = new PointLatLng(41.2970767, 2.07846278);
        Zonas zones = new Zonas();
        LabelsDrawings DrawLabel = new LabelsDrawings();
        List<MapMarker> Markerslist = new List<MapMarker>();
        List<CustomMarker> ADSBMarkers = new List<CustomMarker>();
        List<CustomMarker> MLATMarkers = new List<CustomMarker>();
        List<CustomMarker> DGPSMarkers = new List<CustomMarker>();

        bool ADSBLoaded = false;
        bool MLATLoaded = false;
        bool DGPSLoaded = false;

        private void MapView_Loaded(object sender, RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            gMapControl1.CanDragMap = true;
            gMapControl1.DragButton = MouseButton.Left;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.MinZoom = 2;
            gMapControl1.MaxZoom = 20;
            gMapControl1.Zoom = 14;
            gMapControl1.Position = ARPBarcelona;
            gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            gMapControl1.ShowCenter = false;
            gMapControl1.IgnoreMarkerOnMouseWheel = true;
        }

        public void GetListMarkers(List<MapMarker> list)
        {
            Markerslist = list;
        }
        private void FormLoad(object sender, RoutedEventArgs e)
        {
            // ShowPlygons();
            //ShowMarkers();
        }

        private void DrawPolygon(Polygon pol, System.Windows.Media.Brush color)
        {

            GMap.NET.WindowsPresentation.GMapPolygon polygon = new GMap.NET.WindowsPresentation.GMapPolygon(pol.PointsLatLng);
            polygon.RegenerateShape(gMapControl1);
            (polygon.Shape as Path).Stroke = color;
            (polygon.Shape as Path).StrokeThickness = 1.5;
            (polygon.Shape as Path).Effect = null;
            string hexcolor = color.ToString();
            char[] hexc = hexcolor.ToCharArray();
            hexc[1] = '5';
            hexc[2] = '0';
            hexcolor = new string(hexc);
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString(hexcolor);
            (polygon.Shape as Path).Fill = brush;
            gMapControl1.Markers.Add(polygon);
            //SetLabel(pol);
        }


        private void ShowMarkers()
        {

            foreach (MapMarker marker in Markerslist)
            {
                GMapMarker mark = new GMapMarker(marker.p);
                Brush color = Brushes.Black;
                if (marker.zone == 1 || marker.zone == 2 || marker.zone == 3) { color = Brushes.Yellow; }
                if (marker.zone == 4 || marker.zone == 5) { color = Brushes.Black; }
                if (marker.zone == 6 || marker.zone == 7) { color = Brushes.Red; }
                if (marker.zone == 8) { color = Brushes.Green; }
                if (marker.zone == 9 || marker.zone == 10 || marker.zone == 11) { color = Brushes.Purple; }
                if (marker.zone == 12 || marker.zone == 13 || marker.zone == 14) { color = Brushes.Purple; }
                mark.Shape = new Ellipse
                {
                    Width = 2,
                    Height = 2,
                    Stroke = color,
                    StrokeThickness = 3
                };
                gMapControl1.Markers.Add(mark);

            }
            //   MessageBox.Show(Convert.ToString(Markerslist.Count()));

            //  MessageBox.Show(Convert.ToString(i)+" " + Convert.ToString(e) + " " + Convert.ToString(s) + " " + Convert.ToString(h) + " " + Convert.ToString(f) + " " + Convert.ToString(m));
        }

        private void SetLabel(Polygon pol)
        {
            //    PointLatLng p1 = pol.PointsLatLng[0];
            //  PointLatLng p2 = pol.PointsLatLng[Convert.ToInt32(pol.PointsLatLng.Count() / 2)];
            double Latmax = 0;
            double Latmin = 1000;
            double Lngmax = 0;
            double Lngmin = 1000;
            foreach (PointLatLng p in pol.PointsLatLng)
            {
                if (p.Lat > Latmax) { Latmax = p.Lat; }
                if (p.Lat < Latmin) { Latmin = p.Lat; }
                if (p.Lng > Lngmax) { Lngmax = p.Lng; }
                if (p.Lng < Lngmax) { Lngmin = p.Lng; }
            }

            PointLatLng point = new PointLatLng(Latmin + ((Latmax - Latmin) / 2), Lngmin + ((Lngmax - Lngmin) / 2));
            Labels lab = new Labels(point, pol.id);
            SetMarkerShape(lab);
        }

        private void SetMarkerShape(Labels label)
        {
            Bitmap bitmaptxt = LabelsDrawings.InsertText(label);
            int heig = 10; //35
            int wid = 120; //35
            label.Shape = new System.Windows.Controls.Image
            {

                Width = wid,
                Height = heig,
                Source = LabelsDrawings.ToBitmapImage(bitmaptxt)
            };

            label.Offset = new System.Windows.Point(-(wid / 2), -heig / 2);
            bitmaptxt.Dispose();
            bitmaptxt = null;
            gMapControl1.Markers.Add(label);
            //  line.Shape.MouseRightButtonUp += LabelRightButton;
        }

        //private void ShowRunway25LClick(object sender, RoutedEventArgs e)
        //{

        //}

        //private void ShowRunway25RClick(object sender, RoutedEventArgs e)
        //{

        //}

        //private void ShowRunway02Click(object sender, RoutedEventArgs e)
        //{

        //}

        //private void ShowClick(object sender, RoutedEventArgs e)
        //{
        //    ShowPlygons();

        //}

        private void ShowPlygons()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            gMapControl1.Markers.Clear();
            //  ShowMarkers();
            if (ShowRunway25L.IsChecked == true)
            {
                foreach (Polygon pol in zones.Runway25LZones)
                {
                    DrawPolygon(pol, Brushes.Gold);
                }
            }
            if (ShowRunway25R.IsChecked == true)
            {
                foreach (Polygon pol in zones.Runway25RZones)
                {
                    DrawPolygon(pol, Brushes.Gold);
                }
            }
            if (ShowRunway02.IsChecked == true)
            {
                foreach (Polygon pol in zones.Runway02Zones)
                {
                    DrawPolygon(pol, Brushes.Gold);
                }
            }
            if (StandT1.IsChecked == true)
            {
                foreach (Polygon pol in zones.StandT1Zones)
                {
                    DrawPolygon(pol, Brushes.Black);
                }
            }

            if (StandT2.IsChecked == true)
            {
                foreach (Polygon pol in zones.StandT2Zones)
                {
                    DrawPolygon(pol, Brushes.Black);
                }
            }
            if (ApronT1.IsChecked == true)
            {
                foreach (Polygon pol in zones.ApronT1Zones)
                {
                    DrawPolygon(pol, Brushes.Red);
                }
            }
            if (ApronT2.IsChecked == true)
            {
                foreach (Polygon pol in zones.ApronT2Zones)
                {
                    DrawPolygon(pol, Brushes.Red);
                }
            }
            if (Taxi.IsChecked == true)
            {
                foreach (Polygon pol in zones.TaxiZones)
                {
                    DrawPolygon(pol, Brushes.Green);
                }
            }
            if (AirborneRunway25L25.IsChecked == true)
            {
                foreach (Polygon pol in zones.Airborne25LZones25)
                {
                    DrawPolygon(pol, Brushes.Purple);
                }
            }
            if (AirborneRunway25R25.IsChecked == true)
            {
                foreach (Polygon pol in zones.Airborne25RZones25)
                {
                    DrawPolygon(pol, Brushes.Purple);
                }
            }
            if (AirborneRunway0225.IsChecked == true)
            {
                foreach (Polygon pol in zones.Airborne02Zones25)
                {
                    DrawPolygon(pol, Brushes.Purple);
                }
            }
            if (AirborneRunway25R5.IsChecked == true)
            {
                foreach (Polygon pol in zones.Airborne25RZones5)
                {
                    DrawPolygon(pol, Brushes.MediumVioletRed);
                }
            }
            if (AirborneRunway025.IsChecked == true)
            {
                foreach (Polygon pol in zones.Airborne02Zones5)
                {
                    DrawPolygon(pol, Brushes.MediumVioletRed);
                }
            }
            if (AirborneRunway25L5.IsChecked == true)
            {
                foreach (Polygon pol in zones.Airborne25LZones5)
                {
                    DrawPolygon(pol, Brushes.MediumVioletRed);
                }
            }
            Mouse.OverrideCursor = null;
            ShowHistory();
        }


        private void ShowADSBClick(object sender, RoutedEventArgs e)
        {
            if (ShowADSBHistoryVehicles.IsChecked == true)
            {
                ShowADSB();
            }
            else
            {
                gMapControl1.Markers.Clear();
                ShowPlygons();
                ShowHistory();
            }
        }

        private void ShowHistory()
        {
            // gMapControl1.Markers.Clear();
            Mouse.OverrideCursor = Cursors.Wait;
            if (ShowADSBHistoryVehicles.IsChecked == true)
            {
                ShowADSB();
            }
            if (ShowMLATHistoryVehicles.IsChecked == true)
            {
                ShowMLAT();
            }
            if (ShowDGPSHistoryVehicles.IsChecked==true)
            {
                ShowDGPS();
            }
            Mouse.OverrideCursor = null;

        }

        private void ShowADSB()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            if (ADSBLoaded == false)
            {
                foreach (MapMarker marker in Markerslist)
                {
                    if (marker.type == 1)
                    {
                        CustomMarker mark = new CustomMarker(marker.p,1);
                        Brush color = Brushes.Black;
                        if (marker.zone == 1 || marker.zone == 2 || marker.zone == 3) { color = Brushes.Goldenrod; }
                        if (marker.zone == 4 || marker.zone == 5) { color = Brushes.Black; }
                        if (marker.zone == 6 || marker.zone == 7) { color = Brushes.Red; }
                        if (marker.zone == 8) { color = Brushes.Green; }
                        if (marker.zone == 9 || marker.zone == 10 || marker.zone == 11) { color = Brushes.Purple; }
                        if (marker.zone == 12 || marker.zone == 13 || marker.zone == 14) { color = Brushes.Purple; }
                        mark.Shape = new Ellipse
                        {
                            Width = 2,
                            Height = 2,
                            Stroke = color,
                            StrokeThickness = 3
                        };
                        gMapControl1.Markers.Add(mark);
                        ADSBMarkers.Add(mark);
                    }

                }
                ADSBLoaded = true;
            }

            else
            {
                foreach (CustomMarker marker in ADSBMarkers)
                {
                    gMapControl1.Markers.Add(marker);
                }
            }
            Mouse.OverrideCursor = null;
        }

        private void ShowMLATClick(object sender, RoutedEventArgs e)
        {
            if (ShowMLATHistoryVehicles.IsChecked==true)
            {
                ShowMLAT();
            }
            else
            {
                gMapControl1.Markers.Clear();
                ShowPlygons();
                ShowHistory();

            }
        }

        private void ShowMLAT()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            if (MLATLoaded == false)
            {
                foreach (MapMarker marker in Markerslist)
                {
                    if (marker.type == 0)
                    {
                        CustomMarker mark = new CustomMarker(marker.p,0);
                        Brush color = Brushes.Black;
                        if (marker.zone == 1 || marker.zone == 2 || marker.zone == 3) { color = Brushes.Goldenrod; }
                        if (marker.zone == 4 || marker.zone == 5) { color = Brushes.Black; }
                        if (marker.zone == 6 || marker.zone == 7) { color = Brushes.Red; }
                        if (marker.zone == 8) { color = Brushes.Green; }
                        if (marker.zone == 9 || marker.zone == 10 || marker.zone == 11) { color = Brushes.Purple; }
                        if (marker.zone == 12 || marker.zone == 13 || marker.zone == 14) { color = Brushes.Purple; }
                        mark.Shape = new Ellipse
                        {
                            Width = 2,
                            Height = 2,
                            Stroke = color,
                            StrokeThickness = 3
                        };
                        gMapControl1.Markers.Add(mark);
                        MLATMarkers.Add(mark);
                    }

                }
                MLATLoaded = true;
            }

            else
            {
                foreach (CustomMarker marker in MLATMarkers)
                {
                    gMapControl1.Markers.Add(marker);
                }
            }
            Mouse.OverrideCursor = null;
        }

        private void ShowDGPSClick(object sender, RoutedEventArgs e)
        {
            if (ShowDGPSHistoryVehicles.IsChecked == true)
            {
                ShowDGPS();
            }
            else
            {
                gMapControl1.Markers.Clear();
                ShowPlygons();
                ShowHistory();

            }
        }

        private void ShowDGPS()
        {
            Mouse.OverrideCursor = Cursors.Wait;

            if (DGPSLoaded == false)
            {
                foreach (MapMarker marker in Markerslist)
                {
                    if (marker.type == 2)
                    {
                        CustomMarker mark = new CustomMarker(marker.p, 2);
                        Brush color = Brushes.Black;
                        if (marker.zone == 1 || marker.zone == 2 || marker.zone == 3) { color = Brushes.Goldenrod; }
                        if (marker.zone == 4 || marker.zone == 5) { color = Brushes.Black; }
                        if (marker.zone == 6 || marker.zone == 7) { color = Brushes.Red; }
                        if (marker.zone == 8) { color = Brushes.Green; }
                        if (marker.zone == 9 || marker.zone == 10 || marker.zone == 11) { color = Brushes.Purple; }
                        if (marker.zone == 12 || marker.zone == 13 || marker.zone == 14) { color = Brushes.Purple; }
                        mark.Shape = new Ellipse
                        {
                            Width = 2,
                            Height = 2,
                            Stroke = color,
                            StrokeThickness = 3
                        };
                        gMapControl1.Markers.Add(mark);
                        DGPSMarkers.Add(mark);
                    }

                }
                DGPSLoaded = true;
            }

            else
            {
                foreach (CustomMarker marker in DGPSMarkers)
                {
                    gMapControl1.Markers.Add(marker);
                }
            }
            Mouse.OverrideCursor = null;
        }

        private void ShowClick(object sender, RoutedEventArgs e)
        {
            ShowPlygons();
        }


    }
}