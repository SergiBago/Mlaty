using Mlaty;
using PGTAWPF;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PGTA_WPF
{
    /// <summary>
    /// Lógica de interacción para MessageboxYesNo.xaml
    /// </summary>
    public partial class MessageboxYesNo : Window
    {

        MainWindow Form;
        AccuracyChartsSciCharts chartsPage;
        MAP map;
        public MessageboxYesNo()
        {
            InitializeComponent();
        }

        public void getMainWindow(MainWindow form)
        {
            this.Form = form;
        }

        public void getChartsPage(AccuracyChartsSciCharts form)
        {
            this.chartsPage= form;
        }

        public void getMapPage(MAP map)
        {

        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            if (Form != null)
            {
                Form.getaction(false);
            }
            else if(chartsPage!=null)
            {
                chartsPage.getaction(false);
            }
            else
            {
                map.getaction(false);
            }
            this.Close();
        }

        private void CalculateClick(object sender, RoutedEventArgs e)
        {
            if (Form != null)
            {
                Form.getaction(true);
            }
            else if (chartsPage != null)
            {
                chartsPage.getaction(true);
            }
            else
            {
                map.getaction(true);
            }
            this.Close();
        }

        private void CancelClick(object sender, MouseButtonEventArgs e)
        {
            if (Form != null)
            {
                Form.getaction(false);
            }
            else if (chartsPage != null)
            {
                chartsPage.getaction(false);
            }
            else
            {
                map.getaction(false);
            }
            this.Close();
        }

        private void MouseLeftButton_Down(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
