using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// 



    public partial class ViewPrecission : Page
    {
        public ViewPrecission()
        {
            InitializeComponent();
            //   DatagridView.DataContext = parameters.DefaultView;

        }

        public DataTable parameters = new DataTable();

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            DatagridView.ItemsSource = parameters.DefaultView;
            DatagridView.Items.Refresh();
            DatagridView.UpdateLayout();
            //   foreach (var item in DatagridView.Items)
            for (int i = 0; i < DatagridView.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)DatagridView.ItemContainerGenerator.ContainerFromIndex(i);




                if (row != null)
                {
                    for (int j = 0; j < DatagridView.Columns.Count(); j++)
                    {
                        DataGridCell cell = GetCell(DatagridView, row, j); //1 = second cell
                        TextBlock text = cell.Content as TextBlock;
                        if (text.Text.Length>0 && text.Text.Substring(0,1)=="<")
                        {
                            text.Text = text.Text.Substring(1, text.Text.Length - 1);
                            cell.Foreground = Brushes.Red;
                        }
                    }
                }
            }

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

        public static DataGridCell GetCell(DataGrid dataGrid, DataGridRow rowContainer, int column)
        {
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                if (presenter == null)
                {
                    rowContainer.ApplyTemplate();
                    presenter = FindVisualChild<DataGridCellsPresenter>(rowContainer);
                }
                if (presenter != null)
                {
                    DataGridCell cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    if (cell == null)
                    {
                        dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[column]);
                        cell = presenter.ItemContainerGenerator.ContainerFromIndex(column) as DataGridCell;
                    }
                    return cell;
                }
            }
            return null;
        }


        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj)
           where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            foreach (childItem child in FindVisualChildren<childItem>(obj))
            {
                return child;
            }

            return null;
        }

    }

}
