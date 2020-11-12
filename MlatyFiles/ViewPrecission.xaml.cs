using System;
using System.Collections.Generic;
using System.Data;
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

namespace PGTA_WPF
{
    /// <summary>
    /// Lógica de interacción para ViewPrecission.xaml
    /// </summary>
    public partial class ViewPrecission : Page
    {
        public ViewPrecission()
        {
            InitializeComponent();
        }

        DataTable parameters = new DataTable();

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            DatagridView.ItemsSource = parameters.DefaultView;
            DatagridView.Items.Refresh();
            foreach (DataGridColumn col in DatagridView.Columns)
            {
                col.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                col.CanUserSort = false;
            }
            this.DatagridView.CanUserResizeColumns = false;
            this.DatagridView.CanUserResizeRows = false;
            DatagridView.IsReadOnly = true;
            DatagridView.CanUserReorderColumns = false;
        }

        public void GetData(DataTable Table)
        {
            this.parameters = Table;
        }
    }
}
