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
using Color = System.Windows.Media.Color;
using Microsoft.Win32;
using System.IO;
using Path = System.Windows.Shapes.Path;

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
        List<VehicleTrajectories> TrajectoriesList= new List<VehicleTrajectories>();

        List<CustomMarker> ADSBMarkers = new List<CustomMarker>();
        List<CustomMarker> MLATMarkers = new List<CustomMarker>();
        List<CustomMarker> DGPSMarkers = new List<CustomMarker>();

        bool ADSBLoaded = false;
        bool MLATLoaded = false;
        bool DGPSLoaded = false;

        Brush ADSBcolor= new SolidColorBrush(Color.FromArgb(255, (byte)80, (byte)170, (byte)120));
        Brush MLATcolor = new SolidColorBrush(Color.FromArgb(255, (byte)200, (byte)41, (byte)39));
        Brush DGPScolor = new SolidColorBrush(Color.FromArgb(255, (byte)40, (byte)130, (byte)200));



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

        public void GetTrajectoriesMarkers(List<VehicleTrajectories> list)
        {
            TrajectoriesList = list;
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
                        Brush color = ADSBcolor;
                        //if (marker.zone == 1 || marker.zone == 2 || marker.zone == 3) { color = Brushes.Goldenrod; }
                        //if (marker.zone == 4 || marker.zone == 5) { color = Brushes.Black; }
                        //if (marker.zone == 6 || marker.zone == 7) { color = Brushes.Red; }
                        //if (marker.zone == 8) { color = Brushes.Green; }
                        //if (marker.zone == 9 || marker.zone == 10 || marker.zone == 11) { color = Brushes.Purple; }
                        //if (marker.zone == 12 || marker.zone == 13 || marker.zone == 14) { color = Brushes.Purple; }
                        mark.Shape = new Ellipse
                        {
                            Width = 3,
                            Height = 3,
                            Stroke = color,
                            StrokeThickness = 1,
                            Fill = color
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
                        Brush color = MLATcolor;
                        //if (marker.zone == 1 || marker.zone == 2 || marker.zone == 3) { color = Brushes.Goldenrod; }
                        //if (marker.zone == 4 || marker.zone == 5) { color = Brushes.Black; }
                        //if (marker.zone == 6 || marker.zone == 7) { color = Brushes.Red; }
                        //if (marker.zone == 8) { color = Brushes.Green; }
                        //if (marker.zone == 9 || marker.zone == 10 || marker.zone == 11) { color = Brushes.Purple; }
                        //if (marker.zone == 12 || marker.zone == 13 || marker.zone == 14) { color = Brushes.Purple; }
                        mark.Shape = new Ellipse
                        {
                            Width = 3,
                            Height =3,
                            Stroke = color,
                            StrokeThickness = 1,
                            Fill = color
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
                        Brush color =DGPScolor;
                        //if (marker.zone == 1 || marker.zone == 2 || marker.zone == 3) { color = Brushes.Goldenrod; }
                        //if (marker.zone == 4 || marker.zone == 5) { color = Brushes.Black; }
                        //if (marker.zone == 6 || marker.zone == 7) { color = Brushes.Red; }
                        //if (marker.zone == 8) { color = Brushes.Green; }
                        //if (marker.zone == 9 || marker.zone == 10 || marker.zone == 11) { color = Brushes.Purple; }
                        //if (marker.zone == 12 || marker.zone == 13 || marker.zone == 14) { color = Brushes.Purple; }
                        mark.Shape = new Ellipse
                        {
                            Width = 3,
                            Height = 3,
                            Stroke = color,
                            StrokeThickness = 1,
                            Fill=color
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

        public bool repeteddelete = false;
        public void getaction(bool delete)
        {
            this.repeteddelete = delete;
        }

        private void ExportKMLClick(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Filter = "kml files (*.kml*)|*.kml*";//|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            bool correct = false;
            if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
            {
                string path0 = saveFileDialog1.FileName;
                string path = path0 + ".kml";
                // bool correct=false;
                if (File.Exists(path) == false) { correct = true; }
                while (correct == false)
                {
                    correct = false;
                    MessageboxYesNo deleteform = new MessageboxYesNo();
                    deleteform.getMapPage(this);
                    deleteform.ShowDialog();
                    if (this.repeteddelete == true)
                    {
                        File.Delete(path);
                        correct = true;
                    }
                    else
                    {
                        saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog1.Filter = "kml files (*.kml*)|*.kml*";//|*.txt|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
                        {
                            path0 = saveFileDialog1.FileName;
                            path = path0 + ".kml";
                        }
                        if (File.Exists(path) == false) { correct = true; }

                    }
                }
                if (correct == true)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    ExportKML(path);
                    Mouse.OverrideCursor = null;
                }
            }
        }


        private void ExportKML(string Path)
        {
            StringBuilder KMLbuilder = new StringBuilder();
            KMLbuilder.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            KMLbuilder.AppendLine("<kml xmlns='http://www.opengis.net/kml/2.2'>");
            KMLbuilder.AppendLine("<Document>");
            AddZonesPolygons(KMLbuilder);
            if (Markerslist.Exists(x => x.type == 0))
            {
                AddClassMarkers(KMLbuilder, 0, "MLAT");
            }
            if (Markerslist.Exists(x => x.type == 1))
            {
                AddClassMarkers(KMLbuilder, 1, "ADSB");
            }
            if (Markerslist.Exists(x => x.type == 2))
            {
                AddClassMarkers(KMLbuilder, 2, "DGPS");
            }
            KMLbuilder.Append("</Document>");
            KMLbuilder.AppendLine("</kml>");
            File.WriteAllText(Path, KMLbuilder.ToString());
        }

        private void AddClassMarkers(StringBuilder KMLBuilder, int type, string Name)
        {
            List<MapMarker> markers = Markerslist.Where(x => x.type ==type).ToList();
            KMLBuilder.AppendLine($"<Folder><name>{Name}</name><open>0</open><visibility>1</visibility>");
            string color ="";
            if (type == 0) { color = "ff2729C8"; }
            else if (type == 1) { color = "ff78AA50"; }
            else { color = "ff2882C8"; }
            foreach(MapMarker marker in markers)
            {
                KMLBuilder.Append(GetMarkerText(marker, color));
            }
            KMLBuilder.AppendLine("</Folder>");
        }

        private StringBuilder GetMarkerText(MapMarker marker, string Color)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<Placemark>");
            builder.AppendLine("<Style>");
            builder.AppendLine("<IconStyle>");
            builder.AppendLine($"<color>{Color}</color>");
            builder.AppendLine("<Icon>");
            builder.AppendLine("<href>http://maps.google.com/mapfiles/kml/paddle/wht-blank.png</href>");
            builder.AppendLine("</Icon>");
            builder.AppendLine("</IconStyle>");
            builder.AppendLine("</Style>");
            builder.AppendLine("<Point>");
            builder.AppendLine($"<coordinates>{Convert.ToString(marker.p.Lng).Replace(",", ".")},{Convert.ToString(marker.p.Lat).Replace(",", ".")}</coordinates>");
            builder.AppendLine("</Point>");
            builder.AppendLine("</Placemark>");
            return builder;

        }

        private void AddZonesPolygons(StringBuilder builder)
        {
            builder.AppendLine($"<Folder><name>Zones</name><open>0</open><visibility>1</visibility>");

            AddPolygonZone(builder,"Runway 25L","1BD8E5",zones.Runway25LZones);
            AddPolygonZone(builder, "Runway 02", "1BD8E5", zones.Runway02Zones);
            AddPolygonZone(builder, "Runway 25R", "1BD8E5", zones.Runway25RZones);
            AddPolygonZone(builder, "Stand T1", "000000", zones.StandT1Zones);
            AddPolygonZone(builder, "Stand T2", "000000", zones.StandT2Zones);
            AddPolygonZone(builder, "Apron T1", "0606ff", zones.ApronT1Zones);
            AddPolygonZone(builder, "Apron T2", "0606ff", zones.ApronT2Zones);
            AddPolygonZone(builder, "Taxi", "00C912", zones.TaxiZones);
            AddPolygonZone(builder, "Airborne 02 0-2.5 NM", "C90092", zones.Airborne02Zones25);
            AddPolygonZone(builder, "Airborne 25R 0-2.5 NM", "C90092", zones.Airborne25RZones25);
            AddPolygonZone(builder, "Airborne 25L 0-2.5 NM", "C90092", zones.Airborne25LZones25);
            AddPolygonZone(builder, "Airborne 02 2.5-5 NM", "8900C9", zones.Airborne02Zones5);
            AddPolygonZone(builder, "Airborne 25R 2.5-5 NM", "8900C9", zones.Airborne25RZones5);
            AddPolygonZone(builder, "Airborne 25L 2.5-5 NM", "8900C9", zones.Airborne25LZones5);
            builder.AppendLine("</Folder>");

        }

        private void AddPolygonZone(StringBuilder builder,string Name, string Color, List<Polygon> PolygonsList)
        {
            string line = $"<Style id=#{Name.Replace(" ","")}#>";
            builder.AppendLine(line.Replace('#','"'));
            builder.AppendLine("<LineStyle>");
            builder.AppendLine($"<color>FF{Color}</color>");
            builder.AppendLine("</LineStyle>");
            builder.AppendLine("<PolyStyle>");
            builder.AppendLine($"<color>75{Color}</color>");
            builder.AppendLine("<colorMode>normal</colorMode>");
            builder.AppendLine("<fill>1</fill>");
            builder.AppendLine("<outline>1</outline>");
            builder.AppendLine("</PolyStyle>");
            builder.AppendLine("</Style>");

            builder.AppendLine($"<Folder><name>{Name}</name><open>0</open>");
            foreach (Polygon pol in PolygonsList)
            {
                builder.AppendLine("<Placemark>");
                builder.AppendLine($"<name>{Name}</name>");
                builder.AppendLine($"<styleUrl>#{Name.Replace(" ", "")}</styleUrl>");
                builder.AppendLine("<Polygon>");
                builder.AppendLine("<extrude>1</extrude>");
                builder.AppendLine("<altitudeMode>relativeToGround</altitudeMode>");
                builder.AppendLine("<outerBoundaryIs>");
                AddPolygonToKML(pol.PointsLatLng, builder);
                builder.AppendLine("</outerBoundaryIs>");
                builder.AppendLine("</Polygon>");
                builder.AppendLine("</Placemark>");
            }
            builder.AppendLine("</Folder>");
        }

        private void AddPolygonToKML(List<PointLatLng> PointsList, StringBuilder builder)
        {
            builder.AppendLine("<LinearRing>");
            builder.AppendLine("<coordinates>");
            foreach(PointLatLng p in PointsList)
            {
                builder.AppendLine($"{Convert.ToString(p.Lng).Replace(",", ".")},{Convert.ToString(p.Lat).Replace(",", ".")},90");
            }
            builder.AppendLine($"{Convert.ToString(PointsList[0].Lng).Replace(",", ".")},{Convert.ToString(PointsList[0].Lat).Replace(",", ".")},90");

            builder.AppendLine("</coordinates>");
            builder.AppendLine("</LinearRing>");

        }

        private void ExportKMLClickTraject(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Filter = "kml files (*.kml*)|*.kml*";//|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            bool correct = false;
            if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
            {
                string path0 = saveFileDialog1.FileName;
                string path = path0 + ".kml";
                // bool correct=false;
                if (File.Exists(path) == false) { correct = true; }
                while (correct == false)
                {
                    correct = false;
                    MessageboxYesNo deleteform = new MessageboxYesNo();
                    deleteform.getMapPage(this);
                    deleteform.ShowDialog();
                    if (this.repeteddelete == true)
                    {
                        File.Delete(path);
                        correct = true;
                    }
                    else
                    {
                        saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog1.Filter = "kml files (*.kml*)|*.kml*";//|*.txt|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
                        {
                            path0 = saveFileDialog1.FileName;
                            path = path0 + ".kml";
                        }
                        if (File.Exists(path) == false) { correct = true; }

                    }
                }
                if (correct == true)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    ExportKMLTraject(path);
                    Mouse.OverrideCursor = null;
                }
            }
        }


        private void ExportKMLTraject(string Path)
        {
            StringBuilder KMLbuilder = new StringBuilder();
            KMLbuilder.AppendLine("<?xml version='1.0' encoding='UTF-8'?>");
            KMLbuilder.AppendLine("<kml xmlns='http://www.opengis.net/kml/2.2'>");
            KMLbuilder.AppendLine("<Document>");
            AddZonesPolygons(KMLbuilder);
            KMLbuilder.AppendLine($"<Folder><name>Trajectories</name><open>0</open><visibility>1</visibility>");

            foreach (VehicleTrajectories traject in TrajectoriesList)
            {
                KMLbuilder.AppendLine(GetVehicleTrajectoriesKML(traject));
            }
            KMLbuilder.AppendLine("</Folder>");

            KMLbuilder.Append("</Document>");
            KMLbuilder.AppendLine("</kml>");
            File.WriteAllText(Path, KMLbuilder.ToString());
        }

        public string GetVehicleTrajectoriesKML(VehicleTrajectories vehicleTrajectories)
        {
            StringBuilder KMLBuilder = new StringBuilder();
            KMLBuilder.AppendLine($"<Folder><name>{vehicleTrajectories.TargetAddress}</name><open>0</open><visibility>1</visibility>");
            if(vehicleTrajectories.MLATTrajectories.Count>0)
            {
                KMLBuilder.AppendLine($"<Folder><name>MLAT</name><open>0</open><visibility>1</visibility>");
                foreach(MapTrajectory traject in vehicleTrajectories.MLATTrajectories)
                {
                    KMLBuilder.AppendLine(GetTrajectorieKML(traject));
                }
                KMLBuilder.AppendLine("</Folder>");
            }
            if (vehicleTrajectories.ADSBTrajectories.Count > 0)
            {
                KMLBuilder.AppendLine($"<Folder><name>ADSB</name><open>0</open><visibility>1</visibility>");
                foreach (MapTrajectory traject in vehicleTrajectories.ADSBTrajectories)
                {
                    KMLBuilder.AppendLine(GetTrajectorieKML(traject));
                }
                KMLBuilder.AppendLine("</Folder>");
            }
            if (vehicleTrajectories.DGPSTrajectories.Count > 0)
            {
                KMLBuilder.AppendLine($"<Folder><name>DGPS</name><open>0</open><visibility>1</visibility>");
                foreach (MapTrajectory traject in vehicleTrajectories.DGPSTrajectories)
                {
                    KMLBuilder.AppendLine(GetTrajectorieKML(traject));
                }
                KMLBuilder.AppendLine("</Folder>");
            }
            KMLBuilder.AppendLine("</Folder>");
            return KMLBuilder.ToString();

        }

        public string GetTrajectorieKML(MapTrajectory traject)
        {
            StringBuilder KMLBuilder = new StringBuilder();
            string caption;
            string color;
            if(traject.Type==0)
            {
                color = "ff2729C8";
            }
            else if(traject.Type==1)
            {
                color = "ff78AA50";
            }
            else
            {
                color = "ff2882C8";
            }
            KMLBuilder.AppendLine("<Placemark>");
            KMLBuilder.AppendLine("<Style id='yellowLineGreenPoly'>");
            KMLBuilder.AppendLine("<LineStyle>");
            KMLBuilder.AppendLine("<color>" + color + "</color>");
            KMLBuilder.AppendLine("<width>4</width>");
            KMLBuilder.AppendLine("</LineStyle>");
            KMLBuilder.AppendLine("<PolyStyle>");
            KMLBuilder.AppendLine("<color>" + color + "</color>");
            KMLBuilder.AppendLine("</PolyStyle>");
            KMLBuilder.AppendLine("</Style>");
            KMLBuilder.AppendLine($"<name>{traject.TargetAddress}</name>");
            KMLBuilder.AppendLine("<styleUrl>#yellowLineGreenPoly</styleUrl>");
            KMLBuilder.AppendLine("<LineString>");
            KMLBuilder.AppendLine(KMLcoordenates(traject.ListPoints));
            KMLBuilder.AppendLine("</LineString>");
            KMLBuilder.AppendLine("</Placemark>");
            return KMLBuilder.ToString();
        }

        private string KMLcoordenates(List<PointLatLng> points)
        {
            StringBuilder KMLcoor = new StringBuilder();
            KMLcoor.AppendLine("<coordinates>");
            foreach (PointLatLng p in points)
            {
                string Lat = Convert.ToString(p.Lat).Replace(",", ".");
                string Lon = Convert.ToString(p.Lng).Replace(",", ".");
                KMLcoor.AppendLine(Lon + "," + Lat);
            }
            KMLcoor.AppendLine("</coordinates>");
            return KMLcoor.ToString();
        }
    }
}