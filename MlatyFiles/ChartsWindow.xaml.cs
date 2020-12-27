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
using Mlaty;

namespace PGTAWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class ChartsWindow : Window
    {

        Ficheros Archivo = new Ficheros();



        public ChartsWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void WindowLoad(object sender, RoutedEventArgs e)
        {
            AccuracyCharts chartspage = new AccuracyCharts();
            chartspage.GetValues(Archivo.data.PrecissionPoints);
            PanelChildForm.Navigate(chartspage);
        }

        public void GetArchivo(Ficheros archivo)
        {
            this.Archivo = archivo;
        }

        private void PanelChildForm_ContentRendered(object sender, EventArgs e)
        {
            PanelChildForm.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
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
          //  System.Windows.Application.Current.Shutdown();
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


    }
}
