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
using Microsoft.Win32;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Media.Animation;
using PGTA_WPF;
using System.Windows.Threading;

namespace PGTAWPF
{
    /// <summary>
    /// Lógica de interacción para LoadFiles.xaml
    /// </summary>
    public partial class LoadADSB : Page
    {
        Ficheros Archivo;
        MainWindow Form;
        int result;
        //int duplicated = 0;
        //int seconds = 0;
        //int duplicated = 0;
        //List<bool> SelectedCats;
        //bool correctcats = false;
        //bool Loading = false;
        DispatcherTimer timer = new DispatcherTimer();
        List<string> LoadFilesList = new List<string>();
        public LoadADSB()
        {
            InitializeComponent();      
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += timer_Tick;
            //
            Image rotateImage = new Image()
            {
                Stretch = Stretch.Uniform,
                Source = new BitmapImage(new Uri(@"Loading.png", UriKind.Relative)),
                RenderTransform = new RotateTransform()

            };



            Storyboard storyboard = new Storyboard();
            storyboard.Duration = new Duration(TimeSpan.FromSeconds(10.0));
            DoubleAnimation rotateAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 360,
                Duration = storyboard.Duration
            };


            Storyboard.SetTarget(rotateAnimation, rotateImage);
            Storyboard.SetTargetProperty(rotateAnimation, new PropertyPath("(UIElement.RenderTransform).(RotateTransform.Angle)"));

            storyboard.Children.Add(rotateAnimation);
        }

        private void Rotateimage()
        {
            ((Storyboard)Resources["Storyboard"]).Begin();
        }

        private void StopRotation()
        {
            ((Storyboard)Resources["Storyboard"]).Stop();
        }



        private void Load_click(object sender, MouseButtonEventArgs e)
        {
            load(sender, e);
        }

        private void LoadFirsClick(object sender, MouseButtonEventArgs e)
        {
            load(sender, e);
        }

        private void load(object sender, MouseButtonEventArgs e)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            //correctcats = false;
            //string filePath;
            //duplicated = 0;
            OpenFileDialog openFileDialog = new OpenFileDialog();
          //  openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "ast files (*.ast)|*.ast*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;
            //bool repeted = false;
            //bool CorrectParameters = true;
            if (openFileDialog.ShowDialog() == true && openFileDialog.SafeFileName != null)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    if (Archivo.namesastadsb.Contains(file) == false)
                    {
                        Archivo.namesastadsb.Add(file);
                        Archivo.namesastadsb.Add("Uncomputed!");
                    }                    
                }

            }
            this.Load_load(sender, e);

        }

        //public void GetSelectedCats(List<bool> Selectedcats)
        //{
        //    this.SelectedCats = Selectedcats;
        //    correctcats = true;
        //}

        private void Delete_click(object sender, MouseButtonEventArgs e)
        {
            Archivo.ResetData();
            LoadFilesList.Clear();
            Form.Getfichero(Archivo);
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
            this.Load_load(sender, e);

        }

        private void Load_load(object sender, RoutedEventArgs e)
        {

            //if (Loading == true) { IsNoFiles(2); }
            //else
            //{
            if (Archivo.namesastadsb.Count==0) { IsNoFiles(0); }
            else { IsNoFiles(1); }
            //}
            LoadFirst.Margin = new Thickness(0, 0, 0, 0);
            LoadFirst.Height = 180;
            LoadFirst.Cursor = Cursors.Hand;
        }
        private void IsNoFiles(int a)
        {
            RotateImage.Visibility = Visibility.Hidden;
            StopRotation();
            if (a == 0)
            {
                LoadFirst.Visibility = Visibility.Visible;
                TitleLab.Text = "Therere are no loaded files, plesase load one to start!";
                ScrollBar.Visibility = Visibility.Visible;
                TitleLab.FontStyle = FontStyles.Normal;
                LoadSecond.Visibility = Visibility.Hidden;
                LoadFirst.Visibility = Visibility.Visible;
                DeleteIcon.Visibility = Visibility.Hidden;
                FilesList.Visibility = Visibility.Hidden;
                ComputeLabel.Visibility = Visibility.Hidden;
                LoadLabel.Visibility = Visibility.Hidden;
                DeleteLabel.Visibility = Visibility.Hidden;
                LoadFirstLabel.Visibility = Visibility.Visible;
                Calculate.Visibility = Visibility.Hidden;
                Scrollbarvisibility.Visibility = Visibility.Visible;
                LabelRow.Height = new GridLength(0);
                SaveMarkers.Visibility = Visibility.Hidden;
                StackPanelPIC.Visibility = Visibility.Hidden;

            }
            else if (a == 1)
            {
                SaveMarkers.Visibility = Visibility.Visible;

                LabelRow.Height = new GridLength(1, GridUnitType.Auto);
                ScrollBar.Visibility = Visibility.Visible;
                string filenames = "";
                for (int i = 0; i < Archivo.namesastadsb.Count; i += 2)
                { 
                //foreach (string name in Archivo.names)
                //{
                    string[] parts = Archivo.namesastadsb[i].Split('\\');
                    filenames = filenames + "-" + parts[parts.Length - 1] +"  "+ Archivo.namesastadsb[i+1] +"\n";
                }
                //foreach (string name in LoadFilesList)
                //{
                //    string[] parts = name.Split('\\');
                //    filenames = filenames + "-" + parts[parts.Length - 1] + + "\n";
                //}
                if (Archivo.namesastadsb.Count>18) { Scrollbarvisibility.Visibility = Visibility.Hidden; } /// AQUI
                TitleLab.Text = "Actually, there are these files loaded: ";
                TitleLab.FontStyle = FontStyles.Normal;
                FilesList.Text = filenames;
                FilesList.Visibility = Visibility.Visible;
                LoadSecond.Visibility = Visibility.Visible;
                LoadFirst.Visibility = Visibility.Hidden;
                DeleteIcon.Visibility = Visibility.Visible;
                ComputeLabel.Visibility = Visibility.Visible;
                LoadLabel.Visibility = Visibility.Visible;
                DeleteLabel.Visibility = Visibility.Visible;
                LoadFirstLabel.Visibility = Visibility.Hidden;
                Calculate.Visibility = Visibility.Visible;
                StackPanelPIC.Visibility = Visibility.Visible;

            }
        }


        public void GetDuplicated(int i)
        {
            //this.duplicated = i;
        }

        public void GetParameters(int time)
        {
            //this.seconds = time;
            //this.AirportCode = code;
        }


        public void GetForm(MainWindow Form)
        {
            this.Form = Form;
        }

        public void SetArchivo(Ficheros Archivo)
        {
            this.Archivo = Archivo;
        }

        private void Startloading()
        {
            TitleLab.Text = "Loading";
            timer.Start();
            TitleLab.FontStyle = FontStyles.Italic;
            LoadFirst.Margin = new Thickness(0, 0, 0, 0); ;
            LoadFirst.Cursor = Cursors.Arrow;
            LoadSecond.Visibility = Visibility.Hidden;
            DeleteIcon.Visibility = Visibility.Hidden;
            LoadFirst.Visibility = Visibility.Hidden;
            //FilesList.Visibility = Visibility.Hidden;
            RotateImage.Visibility = Visibility.Visible;
            ComputeLabel.Visibility = Visibility.Hidden;
            LoadLabel.Visibility = Visibility.Hidden;
            DeleteLabel.Visibility = Visibility.Hidden;
            LoadFirstLabel.Visibility = Visibility.Hidden;
            Calculate.Visibility = Visibility.Hidden;
            SaveMarkers.Visibility = Visibility.Hidden;
            StackPanelPIC.Visibility = Visibility.Hidden;


            Rotateimage();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                string filenames = "";

                //ProcessLabel.Text = Archivo.process;
                for (int i = 0; i < Archivo.namesastadsb.Count; i += 2)
                {
                    //foreach (string name in Archivo.names)
                    //{
                    string[] parts = Archivo.namesastadsb[i].Split('\\');
                    filenames = filenames + "-" + parts[parts.Length - 1] + "  " + Archivo.namesastadsb[i + 1] + "\n";
                }
                FilesList.Text = filenames;

            });
        }

        //private void Stoploading()
        //{
        //    StopRotation();
        //    timer.Stop();
        //}

        private void ComputeClick(object sender, RoutedEventArgs e)
        {
            Startloading();
            Form.LoadDisableButtons();
            bool save = false;
            string PIC = Convert.ToString(PICSelect.SelectionBoxItem.ToString());

            if (SaveMarkers.IsChecked == true) { save = true; }
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                //   filePath = openFileDialog.FileName;
                
                this.result = Archivo.ComputeValuesADSB(save,PIC);
                if (result == 1)
                {
                    this.Dispatcher.Invoke(() =>
                    {

                        Form.Getfichero(Archivo);
                        this.Load_load(sender, e);
                        Form.LoadActiveButtons();
                    });
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        this.Load_load(sender, e);
                        Form.LoadActiveButtons();
                    });
                }

            }).Start();
            //Archivo.ComputeValues(LoadFilesList);
        }

        private void Resize(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();
            if (this.ActualWidth < 1050)
            {
                PageGrid.MaxWidth = 850;
            }
            else
            {
                PageGrid.MaxWidth = this.ActualWidth - 200;
            }
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
