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

    //   Data parameters = new Data();
        DataTable parameters = new DataTable();

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
        //    MessageBox.Show("loaded");
          //  parameters.CreateTable();
            DatagridView.ItemsSource = parameters.DefaultView;
            DatagridView.Items.Refresh();

            //DatagridView.DataContext = parameters.parameters;
            foreach (DataGridColumn col in DatagridView.Columns)
            {
                col.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
                col.CanUserSort = false;
            }
            this.DatagridView.CanUserResizeColumns = false;
            this.DatagridView.CanUserResizeRows = false;
            DatagridView.IsReadOnly = true;
            DatagridView.CanUserReorderColumns = false;
            //DatagridView.Allow
            //    foreach (DataGridViewColumn column in dataGridView.Columns)
            //{
            //    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            ////}
            //DatagridViewPD.ItemsSource = parameters.PD.DefaultView;
            //DatagridViewPD.Items.Refresh();

            ////DatagridView.DataContext = parameters.parameters;
            //foreach (DataGridColumn col in DatagridViewPD.Columns)
            //{
            //    col.Width = new DataGridLength(1, DataGridLengthUnitType.Auto);
            //    col.CanUserSort = false;

            //}
            ////DataRowView item = (DatagridView.Items[1] as DataRowView);
            //DataRow row = item.Row;
            //item
            //row.Background = new SolidColorBrush(Colors.BlanchedAlmond);
            //this.DatagridViewPD.CanUserResizeColumns = false;
            //this.DatagridViewPD.CanUserResizeRows = false;
            //DatagridViewPD.IsReadOnly = true;
            //DatagridViewPD.CanUserReorderColumns = false;


        }

        public void GetData(DataTable Table)
        {
          //  MessageBox.Show("GetData");
            this.parameters = Table;
        }
    }
}
