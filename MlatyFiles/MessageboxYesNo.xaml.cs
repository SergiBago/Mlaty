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

        MainWindow Form = new MainWindow();
        public MessageboxYesNo()
        {
            InitializeComponent();
        }

        public void getMainWindow(MainWindow form)
        {
            this.Form = form;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Form.getaction(false);
            this.Close();
        }

        private void CalculateClick(object sender, RoutedEventArgs e)
        {
            Form.getaction(true);
            this.Close();
        }

        private void CancelClick(object sender, MouseButtonEventArgs e)
        {
            Form.getaction(false);
            this.Close();
        }

        private void MouseLeftButton_Down(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
    }
}
