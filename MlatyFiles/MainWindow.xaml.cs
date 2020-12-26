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
using Drawing = System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.TextFormatting;
using System.Data;
using System.Runtime.InteropServices;
using GMap.NET;
using System.IO;
using System.Reflection;
using PGTA_WPF;
using Microsoft.Win32;

namespace PGTAWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<System.Windows.Controls.Image> ActiveButtons;
        List<System.Windows.Controls.Image> UnActiveButtons;
        List<Border> ListPanels;
        List<TextBlock> ListLabels;
        bool Loadadsbstarted = false;
        bool Loaddgpsstarted = false;
        Ficheros Archivo = new Ficheros();
        DataTable PrecisionTable = new DataTable();
        DataTable UR = new DataTable();
        bool mapstarted = false;
        MAP map = new MAP();
        LoadADSB loadADSB;
        LoadDGPS loadDGPS;
        DataTable PD = new DataTable();
        DataTable Precission = new DataTable();
        DataTable PFITable = new DataTable();
        DataTable PFDTable = new DataTable();


        public MainWindow()
        {
            InitializeComponent();
            ActiveButtons = new List<System.Windows.Controls.Image> { HomeIco2, LoadIco2, ListIco2,URIco2,PrecissionIco2,PFDIco2,PFIIco2 ,MapIco2, ExportIco2,ExportAccuIco2, HelpIco2,DGPSIco2, ADSBIco2,PDIco2,PIIco2};
            UnActiveButtons = new List<System.Windows.Controls.Image> { HomeIco1, LoadIco1, ListIco1, URIco,PrecisionIco,MapIco1, HelpIco1, DGPSIco1, ADSBIco1, ExportIco,ExportAccuIco,PFDIco,PFIIco,PDIco,PIIco};
            ListPanels = new List<Border> { HomePanel, LoadPanel, ListPanel, PrecissionPanel, URPanel, MapPanel, HelpPanel, LoadADSBMLATPanel, LoadMLATDGPSPanel,ExportAccuracyPanel,ExportTablesPanel,PFDPanel,PFIPanel, PDPanel, PIPanel };
            ListLabels = new List<TextBlock> { HomeLabel, LoadFilesLabel, ListLabel, MapLabel, Precision,UpdateRate , HelpLabel, LoadADSBMLATLabel, LoadDGPSMLATLabel,PFD,PFI ,ProbabilityDetection, PI };
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            Archivo.data.CreatePDTable();
            Archivo.data.CreatePrecissionTable();
            PD = Archivo.data.PD;
            Precission = Archivo.data.parameters;
        }

        private void Main_Load(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            HomeLabel.TextAlignment = TextAlignment.Right;
            HomeIco1.Visibility = Visibility.Collapsed;
            HomeIco2.Visibility = Visibility.Visible;
            HomePanel.Background = new SolidColorBrush(RGBColors.color1);
            FormTitle.Text = "Home";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color1);
            HomeLabel.Foreground = new SolidColorBrush(RGBColors.color1);
            FormIco.Source = new BitmapImage(new Uri(@"images/Casa Color.png", UriKind.Relative));
            Intro Presentation = new Intro();
            Presentation.GetMainWindow(this);
            PanelChildForm.Navigate(Presentation);
        }

        private void DisableButtons()
        {
            foreach (System.Windows.Controls.Image im in ActiveButtons) { im.Visibility = Visibility.Hidden; }
            foreach (System.Windows.Controls.Image im in UnActiveButtons) { im.Visibility = Visibility.Visible; }
            foreach (Border bor in ListPanels) { bor.Background = new SolidColorBrush(RGBColors.color6); }
            foreach (TextBlock text in ListLabels) 
            {
                text.TextAlignment = TextAlignment.Left;
                text.Foreground = new SolidColorBrush(RGBColors.color7);
            }
        }

        public void LoadDisableButtons()
        {
            PanelDisableButons.Visibility = Visibility.Visible;
        }

        public void LoadActiveButtons()
        {
            PanelDisableButons.Visibility = Visibility.Hidden;
        }

        private struct RGBColors
        {
            public static Color color1 = Color.FromRgb(194, 184, 178);
            public static Color color2 = Color.FromRgb(176, 190, 169);
            public static Color color3 = Color.FromRgb(250, 248, 212);
            public static Color color4 = Color.FromRgb(250, 248, 212);
            public static Color color5 = Color.FromRgb(194, 184, 178);
            public static Color color6 = Color.FromRgb(70, 70, 70);
            public static Color color7 = Color.FromRgb(255, 255, 255);

        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            HomeLabel.TextAlignment = TextAlignment.Right;
            HomeIco1.Visibility = Visibility.Collapsed;
            HomeIco2.Visibility = Visibility.Visible;
            HomePanel.Background = new SolidColorBrush(RGBColors.color1);
            FormTitle.Text = "Home";
            FormTitle.Foreground= new SolidColorBrush(RGBColors.color1);
            HomeLabel.Foreground = new SolidColorBrush(RGBColors.color1);
            FormIco.Source = new BitmapImage(new Uri(@"images/Casa Color.png", UriKind.Relative));
            Intro Presentation = new Intro();
            Presentation.GetMainWindow(this);
            PanelChildForm.Navigate(Presentation);

        }



        private void DGPSFiles(object sender, MouseButtonEventArgs e)
        {
            ActiveLoadButton();
            ActiveLoadDGPSButton();
            if (Loaddgpsstarted == false)
            {
                loadDGPS = new LoadDGPS();
                loadDGPS.SetArchivo(Archivo);
                loadDGPS.GetForm(this);
                Loaddgpsstarted = true;
            }
            PanelChildForm.Navigate(loadDGPS);
        }

        private void ADSBFiles(object sender, MouseButtonEventArgs e)
        {
            ActiveLoadButton();
            ActiveLoadADSBButton();
            if (Loadadsbstarted == false)
            {
                loadADSB = new LoadADSB();
                loadADSB.SetArchivo(Archivo);
                loadADSB.GetForm(this);
                Loadadsbstarted = true;
            }
            PanelChildForm.Navigate(loadADSB);
        }


        private void ActiveLoadButton()
        {

            DisableButtons();
            LoadFilesLabel.TextAlignment = TextAlignment.Right;
            LoadIco1.Visibility = Visibility.Collapsed;
            LoadIco2.Visibility = Visibility.Visible;
            LoadPanel.Background = new SolidColorBrush(RGBColors.color2);
            FormTitle.Text = "Manage Data";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color2);
            LoadFilesLabel.Foreground = new SolidColorBrush(RGBColors.color2);
            FormIco.Source = new BitmapImage(new Uri(@"images/File Color.png", UriKind.Relative));
        }

        private void ActiveLoadDGPSButton()
        {
            LoadDGPSMLATLabel.TextAlignment = TextAlignment.Right;
            DGPSIco1.Visibility = Visibility.Collapsed;
            DGPSIco2.Visibility = Visibility.Visible;
            LoadDGPSMLATLabel.Foreground = new SolidColorBrush(RGBColors.color2);

        }

        private void ActiveLoadADSBButton()
        {
            LoadADSBMLATLabel.TextAlignment = TextAlignment.Right;
            ADSBIco1.Visibility = Visibility.Collapsed;
            ADSBIco2.Visibility = Visibility.Visible;
            LoadADSBMLATLabel.Foreground = new SolidColorBrush(RGBColors.color2);

        }

        private void activeSeeprecissionbutton()
        {
            DisableButtons();
            ListLabel.TextAlignment = TextAlignment.Right;
            Precision.TextAlignment = TextAlignment.Right;
            PrecisionIco.Visibility = Visibility.Collapsed;
            PrecissionIco2.Visibility = Visibility.Visible;
            ListIco1.Visibility = Visibility.Collapsed;
            ListIco2.Visibility = Visibility.Visible;
            ListPanel.Background = new SolidColorBrush(RGBColors.color3);
            FormTitle.Text = "Results";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color3);
            ListLabel.Foreground = new SolidColorBrush(RGBColors.color3);
            Precision.Foreground = new SolidColorBrush(RGBColors.color3);
            FormIco.Source = new BitmapImage(new Uri(@"images/Lista Color.png", UriKind.Relative));
        }


        private void activeSeeURbutton()
        {
            DisableButtons();
            ListLabel.TextAlignment = TextAlignment.Right;
            UpdateRate.TextAlignment = TextAlignment.Right;
            URIco.Visibility = Visibility.Collapsed;
            URIco2.Visibility = Visibility.Visible;
            ListIco1.Visibility = Visibility.Collapsed;
            ListIco2.Visibility = Visibility.Visible;
            ListPanel.Background = new SolidColorBrush(RGBColors.color3);
            FormTitle.Text = "Results";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color3);
            ListLabel.Foreground = new SolidColorBrush(RGBColors.color3);
            UpdateRate.Foreground = new SolidColorBrush(RGBColors.color3);
            FormIco.Source = new BitmapImage(new Uri(@"images/Lista Color.png", UriKind.Relative));
        }

        private void activeSeePDbutton()
        {
            DisableButtons();
            ListLabel.TextAlignment = TextAlignment.Right;
            ProbabilityDetection.TextAlignment = TextAlignment.Right;
            PDIco.Visibility = Visibility.Collapsed;
            PDIco2.Visibility = Visibility.Visible;
            ListIco1.Visibility = Visibility.Collapsed;
            ListIco2.Visibility = Visibility.Visible;
            ListPanel.Background = new SolidColorBrush(RGBColors.color3);
            FormTitle.Text = "Results";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color3);
            ListLabel.Foreground = new SolidColorBrush(RGBColors.color3);
            ProbabilityDetection.Foreground = new SolidColorBrush(RGBColors.color3);
            FormIco.Source = new BitmapImage(new Uri(@"images/Lista Color.png", UriKind.Relative));
        }

        private void activeSeePIbutton()
        {
            DisableButtons();
            ListLabel.TextAlignment = TextAlignment.Right;
            PI.TextAlignment = TextAlignment.Right;
            PIIco.Visibility = Visibility.Collapsed;
            PIIco2.Visibility = Visibility.Visible;
            ListIco1.Visibility = Visibility.Collapsed;
            ListIco2.Visibility = Visibility.Visible;
            ListPanel.Background = new SolidColorBrush(RGBColors.color3);
            FormTitle.Text = "Results";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color3);
            ListLabel.Foreground = new SolidColorBrush(RGBColors.color3);
            PI.Foreground = new SolidColorBrush(RGBColors.color3);
            FormIco.Source = new BitmapImage(new Uri(@"images/Lista Color.png", UriKind.Relative));
        }

        private void activeSeePFIbutton()
        {
            DisableButtons();
            ListLabel.TextAlignment = TextAlignment.Right;
            PFI.TextAlignment = TextAlignment.Right;
            PFIIco.Visibility = Visibility.Collapsed;
            PFIIco2.Visibility = Visibility.Visible;
            ListIco1.Visibility = Visibility.Collapsed;
            ListIco2.Visibility = Visibility.Visible;
            ListPanel.Background = new SolidColorBrush(RGBColors.color3);
            FormTitle.Text = "Results";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color3);
            ListLabel.Foreground = new SolidColorBrush(RGBColors.color3);
            PFI.Foreground = new SolidColorBrush(RGBColors.color3);
            FormIco.Source = new BitmapImage(new Uri(@"images/Lista Color.png", UriKind.Relative));
        }

        private void activeSeePFDbutton()
        {
            DisableButtons();
            ListLabel.TextAlignment = TextAlignment.Right;
            PFD.TextAlignment = TextAlignment.Right;
            PFDIco.Visibility = Visibility.Collapsed;
            PFDIco2.Visibility = Visibility.Visible;
            ListIco1.Visibility = Visibility.Collapsed;
            ListIco2.Visibility = Visibility.Visible;
            ListPanel.Background = new SolidColorBrush(RGBColors.color3);
            FormTitle.Text = "Results";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color3);
            ListLabel.Foreground = new SolidColorBrush(RGBColors.color3);
            PFD.Foreground = new SolidColorBrush(RGBColors.color3);
            FormIco.Source = new BitmapImage(new Uri(@"images/Lista Color.png", UriKind.Relative));
        }



        private void PrecisIonclick(object sender, MouseButtonEventArgs e)
        {
            activeSeeprecissionbutton();
            ViewPrecission view = new ViewPrecission();
            view.GetData(Precission);
            PanelChildForm.Navigate(view);

        }

        private void UpdateRateClick(object sender, MouseButtonEventArgs e)
        {
            activeSeeURbutton();
            ViewPrecission view = new ViewPrecission();
            view.GetData(PD);
            PanelChildForm.Navigate(view);
        }

        private void PDClick(object sender, MouseButtonEventArgs e)
        {
            activeSeePDbutton();
        }

        private void PFDClick(object sender, MouseButtonEventArgs e)
        {
            activeSeePFDbutton();
            ViewPrecission view = new ViewPrecission();
            view.GetData(PFDTable);
            PanelChildForm.Navigate(view);
        }

        private void PFIClick(object sender, MouseButtonEventArgs e)
        {
            activeSeePFIbutton();
            ViewPrecission view = new ViewPrecission();
            view.GetData(PFITable);
            PanelChildForm.Navigate(view);
        }

        private void PIClick(object sender, MouseButtonEventArgs e)
        {
            activeSeePIbutton();
        }

        private void ExportClick(object sender, MouseButtonEventArgs e)
        {
            ExportResults();
        }

        private void ExportAccuracyClick(object sender, MouseButtonEventArgs e)
        {
            ExportTable();
        }

        private void activeExportbutton()
        {
            DisableButtons();
            ListLabel.HorizontalAlignment = HorizontalAlignment.Right;
            ExportLabel.HorizontalAlignment = HorizontalAlignment.Right;
            ExportIco.Visibility = Visibility.Hidden;
            ExportIco2.Visibility = Visibility.Visible;
            ListIco1.Visibility = Visibility.Hidden;
            ListIco2.Visibility = Visibility.Visible;
            ListPanel.Background = new SolidColorBrush(RGBColors.color3);
            FormTitle.Text = "Export Results";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color3);
            ListLabel.Foreground = new SolidColorBrush(RGBColors.color3);
            ExportLabel.Foreground = new SolidColorBrush(RGBColors.color3);
            FormIco.Source = new BitmapImage(new Uri(@"images/Lista Color.png", UriKind.Relative));
        }

        private void ExportResults()
        {
            SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Filter = "csv files (*.csv*)|*.csv*";//|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            bool correct= false;
            if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
            {
                string path0 = saveFileDialog1.FileName;
                string path = path0 + ".csv";
              // bool correct=false;
              if (File.Exists(path)==false) { correct = true; }
                while (correct==false)
                {
                    correct = false;
                    MessageboxYesNo deleteform = new MessageboxYesNo();
                    deleteform.getMainWindow(this);
                    deleteform.ShowDialog();
                    if (this.repeteddelete == true)
                    {
                        File.Delete(path);
                        correct = true;
                    }
                    else
                    {
                        saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog1.Filter = "csv files (*.csv*)|*.csv*";//|*.txt|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
                        {
                            path0 = saveFileDialog1.FileName;
                            path = path0 + ".csv";
                        }
                        if (File.Exists(path) == false) { correct = true; }

                    }
                }
                if (correct==true)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;


                    StringBuilder sb = new StringBuilder();
                    StringBuilder ColumnsNames = new StringBuilder();
                 //   string Heather = "Precision;;;;;;, , , , , , , , UP & PD";
           //         sb.AppendLine(Heather);
                    foreach (DataColumn col in Precission.Columns)
                    {
                            string Name = col.ColumnName.Replace('\n', ' ');
                            ColumnsNames.Append(Name + ";");                      
                    }
                    ColumnsNames.Append(";");
                    foreach (DataColumn col in PD.Columns)
                    {
                        string Name = col.ColumnName.Replace('\n', ' ');
                        ColumnsNames.Append(Name + ";"); 
                    }
                    ColumnsNames.Append(";");
                    foreach (DataColumn col in PFDTable.Columns)
                    {
                        string Name = col.ColumnName.Replace('\n', ' ');
                        ColumnsNames.Append(Name + ";");
                    }
                    ColumnsNames.Append(";");
                    foreach (DataColumn col in PFITable.Columns)
                    {
                        string Name = col.ColumnName.Replace('\n', ' ');
                        ColumnsNames.Append(Name + ";");
                    }
                    string ColNames = ColumnsNames.ToString();
                    ColNames = ColNames.TrimEnd(';');
                    sb.AppendLine(ColNames);
                    for (int i=0; i<Precission.Rows.Count;i++)
                    {
                      //  string nl = "; ";
                        StringBuilder RowData = new StringBuilder();
                        foreach (DataColumn column in Precission.Columns)
                        {
                            DataRow row = Precission.Rows[i];
                            string data = row[column].ToString();
                            data = data.Replace(",", ".");
                            RowData.Append(data);
                            RowData.Append(";");

                        }
                        RowData.Append(";");
                        foreach (DataColumn column in PD.Columns)
                        {
                            DataRow row = PD.Rows[i];
                            string data = row[column].ToString();
                            data = data.Replace(",", ".");
                            RowData.Append(data);
                            RowData.Append(";");
                        }
                        RowData.Append(";");
                        foreach (DataColumn column in PFDTable.Columns)
                        {
                            DataRow row = PFDTable.Rows[i];
                            string data = row[column].ToString();
                            data = data.Replace(",", ".");
                            RowData.Append(data);
                            RowData.Append(";");

                        }
                        RowData.Append(";");
                        foreach (DataColumn column in PFITable.Columns)
                        {
                            DataRow row = PFITable.Rows[i];
                            string data = row[column].ToString();
                            data = data.Replace(",", ".");
                            RowData.Append(data);
                            RowData.Append(";");
                        }

                        string rowdata = RowData.ToString();
                        rowdata=rowdata.TrimEnd(';');
                        sb.AppendLine(rowdata);

                    }
                    File.WriteAllText(path, sb.ToString());
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private void ExportTable()
        {
            SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Filter = "csv files (*.csv*)|*.csv*";//|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            bool correct = false;
            if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
            {
                string path0 = saveFileDialog1.FileName;
                string path = path0 + ".csv";
                // bool correct=false;
                if (File.Exists(path) == false) { correct = true; }
                while (correct == false)
                {
                    correct = false;
                    MessageboxYesNo deleteform = new MessageboxYesNo();
                    deleteform.getMainWindow(this);
                    deleteform.ShowDialog();
                    if (this.repeteddelete == true)
                    {
                        File.Delete(path);
                        correct = true;
                    }
                    else
                    {
                        saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
                        saveFileDialog1.Filter = "csv files (*.csv*)|*.csv*";//|*.txt|All files (*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == true && saveFileDialog1.SafeFileName != null)
                        {
                            path0 = saveFileDialog1.FileName;
                            path = path0 + ".csv";
                        }
                        if (File.Exists(path) == false) { correct = true; }

                    }
                }
                if (correct == true)
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    StringBuilder sb = new StringBuilder();
                    StringBuilder ColumnsNames = new StringBuilder();
                    string Heather = "TIDENT;TIME;RADAL LOCAL X;RADAR LOCAL Y;ARP HEIGHT;GPS LOCAL X;GPS LOCAL Y;GPS LOCAL Z;ERROR LOCAL X;ERROR LOCAL Y;ERROR LOCAL XY;AREA;GBS";
                    sb.AppendLine(Heather);

                    foreach (PrecissionPoint p in Archivo.data.PrecissionPoints )
                    {
                        // StringBuilder RowData = new StringBuilder();
                        string LocalX = Convert.ToString(p.localX);
                        string LocalY = Convert.ToString(p.localY);
                        string ARPH = Convert.ToString(p.ARPH);
                        string GPSX = Convert.ToString(p.GPSX);
                        string GPSY = Convert.ToString(p.GPSY);
                        string GPSZ = Convert.ToString(p.GPSZ);
                        string ErrorLocalX = Convert.ToString(p.ErrorLocalX);
                        string ErrorLocalY = Convert.ToString(p.ErrorLocalY);
                        string ErrorLocalXY = Convert.ToString(p.ErrorLocalXY);
                        string RowData= (p.Callsign + ";" + p.time+";" + LocalX+";"+LocalY+ ";"+ARPH+ ";"+GPSX+ ";"+GPSY+ ";"+GPSZ+ ";"+ErrorLocalX+ ";"+ErrorLocalY+ ";"+ErrorLocalXY+ ";"+p.Area+ ";"+p.GroundBit);
                        sb.AppendLine(RowData);
                    }
                    File.WriteAllText(path, sb.ToString());
                    Mouse.OverrideCursor = null;
                }
            }
        }

        public bool repeteddelete = false;
        public void getaction(bool delete)
        {
            this.repeteddelete = delete;
        }

        private void MapClick(object sender, MouseButtonEventArgs e)
        {
            ActiveMapButton();
            if (mapstarted == false)
            {
                map = new MAP();
                map.GetListMarkers(Archivo.listMarkers);
                mapstarted = true;
            }
            PanelChildForm.Navigate(map);
        }

        private void ActiveMapButton()
        {
            DisableButtons();
            MapLabel.TextAlignment = TextAlignment.Right;
            MapIco1.Visibility = Visibility.Collapsed;
            MapIco2.Visibility = Visibility.Visible;
            FormTitle.Text = "Map View";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color4);
            MapLabel.Foreground = new SolidColorBrush(RGBColors.color4);
            MapPanel.Background = new SolidColorBrush(RGBColors.color4);
            FormIco.Source = new BitmapImage(new Uri(@"images/MapColor.png", UriKind.Relative));
        }

        private void OpenMapNewWindowClick(object sender, RoutedEventArgs e)
        {
            MapWindow newMap = new MapWindow();
            newMap.GetArchivo(Archivo);
            newMap.Show();
        }

        private void InfoClick(object sender, MouseButtonEventArgs e)
        {
            ActiveInfoButton();
           Info info = new Info();
            PanelChildForm.Navigate(info);
        }

        private void ActiveInfoButton()
        {
            DisableButtons();
            HelpLabel.TextAlignment=TextAlignment.Right;
            HelpIco1.Visibility = Visibility.Collapsed;
            HelpIco2.Visibility = Visibility.Visible;
            FormTitle.Text = "Info";
            FormTitle.Foreground = new SolidColorBrush(RGBColors.color5);
            HelpLabel.Foreground = new SolidColorBrush(RGBColors.color5);
            HelpPanel.Background = new SolidColorBrush(RGBColors.color5);
            FormIco.Source = new BitmapImage(new Uri(@"images/InfoColor.png", UriKind.Relative));
        }

        private void PanelChildForm_ContentRendered(object sender, EventArgs e)
        {
            PanelChildForm.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        public void Getfichero(Ficheros file)
        {
            this.Archivo = file;
            Archivo.data.CreatePDTable();
            Archivo.data.CreatePrecissionTable();
            Archivo.data.CreatePFDTable();
            Archivo.data.CreatePFITable();
            PD = Archivo.data.PD;
            Precission = Archivo.data.parameters;
            PFDTable = Archivo.data.PFD;
            PFITable = Archivo.data.PFI;
            mapstarted = false;
        }


        private void PanelLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

                base.OnMouseLeftButtonDown(e);
                if (this.WindowState == System.Windows.WindowState.Maximized)
                {
                    this.WindowState = System.Windows.WindowState.Normal;
                    Point mousepos= Mouse.GetPosition(this);
                    Application.Current.MainWindow.Left = System.Windows.Forms.Cursor.Position.X-mousepos.X;
                    Application.Current.MainWindow.Top= System.Windows.Forms.Cursor.Position.Y-mousepos.Y;
                }
                this.DragMove();
        }

        private void Close_click(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
            this.Close();
        }

        private void MaximizeClick(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        private void Minimize_click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }



        private void MapRioghtClick(object sender, MouseButtonEventArgs e)
        {
            
            ContextMenu cm = this.FindResource("cmButton") as ContextMenu;
            //b.IsEnabled = true;
            //b.Visibility = Visibility.Visible;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }


    }
}
