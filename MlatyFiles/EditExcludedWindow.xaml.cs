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
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Threading;

namespace PGTAWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class EditExcludedWindow : Window
    {
        MainWindow window;
        DispatcherTimer Timer = new DispatcherTimer();

        public EditExcludedWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            Timer.Interval = TimeSpan.FromSeconds(0.5);
            Timer.Tick += timer_Tick;

        }

        public void GetMainWindow(MainWindow window)
        {
            this.window = window;
        }

        private void Main_Load(object sender, RoutedEventArgs e)
        {
            this.Owner = window;
            LoadList();
        }

        private void LoadList()
        {
            NamesPanel.Children.Clear();
            List<string> List = new List<string>();
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ExcludedMLATs.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            string sql = "Select * From Excluded";
            SqlCommand command = new SqlCommand(sql, cnn);
            SqlDataReader datareader = command.ExecuteReader();
            while (datareader.Read())
            {
                List.Add((string)datareader.GetValue(0));
            }
            cnn.Close();
            foreach(string name in List)
            {
                AddExcludedLabel(name);
            }
        }

        private void AddExcludedLabel(string Name)
        {
            StackPanel Panel = new StackPanel 
            {
                Orientation=Orientation.Horizontal,
                Margin= new Thickness(10,5,10,0),
            };
            TextBlock Label = new TextBlock
            {
                Text = Name,
                Style=(Style)TryFindResource("font_style_Content"),
                Width=200,
            };
            Image im = new Image
            {
                Height=30,
                Source= new BitmapImage(new Uri("images/DeleteExcluded.png",UriKind.Relative)),
                Cursor=Cursors.Hand,
            };
            im.MouseLeftButtonUp += DeleteFromList;
            Panel.Children.Add(Label);
            Panel.Children.Add(im);
            NamesPanel.Children.Add(Panel);
        }

       

        private void DeleteFromList(object sender, MouseButtonEventArgs e)
        {
            Image im = sender as Image;
            StackPanel panel = im.Parent as StackPanel;
            foreach(object child in panel.Children)
            {
                TextBlock text = child as TextBlock;
                if(text!=null)
                {
                    DeleteItemFromList(text.Text);
                    break;
                }
            }
            LoadList();
        }

        private void DeleteItemFromList(string Name)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ExcludedMLATs.mdf;Integrated Security=True;Connect Timeout=30";
            SqlConnection cnn = new SqlConnection(connectionString);
            cnn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = $"Delete Excluded where Name='{Name}'";
            //SqlCommand command = new SqlCommand(sql, cnn);
            adapter.InsertCommand = new SqlCommand(sql, cnn);
            adapter.InsertCommand.ExecuteNonQuery();
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
           // System.Windows.Application.Current.Shutdown();
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

        private void Resize(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();
           
            if (PageScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Visible && PageScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Visible)
            {
                ScrollBorder.Visibility = Visibility.Visible;
            }
            else
            {
                ScrollBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void AddExcluded(object sender, MouseButtonEventArgs e)
        {
            if (Text != null)
            {
                Text = Text.Trim(' ');
                if (Text != "")
                {
                    string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ExcludedMLATs.mdf;Integrated Security=True;Connect Timeout=30";
                    SqlConnection cnn = new SqlConnection(connectionString);
                    cnn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    string sql = $"Insert into Excluded values('{Text}')";
                    adapter.InsertCommand = new SqlCommand(sql, cnn);
                    adapter.InsertCommand.ExecuteNonQuery();
                    cnn.Close();
                    AddExcludedLabel(Text);
                    Text = "";
                }
            }
        }

        private void AddExcluded(string text)
        {
            if (text != null)
            {
                text = text.Trim(' ');
                if (text != "")
                {
                    string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ExcludedMLATs.mdf;Integrated Security=True;Connect Timeout=30";
                    SqlConnection cnn = new SqlConnection(connectionString);
                    cnn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter();

                    string sql = $"Insert into Excluded values('{text}')";
                    adapter.InsertCommand = new SqlCommand(sql, cnn);
                    adapter.InsertCommand.ExecuteNonQuery();
                    cnn.Close();
                    AddExcludedLabel(text);
                    Text = "";
                }
            }
        }

        private void TextBoxGotFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            AddExcludedTextBox.Text = "";
        }

        string Text = "";
        private void TextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            Text = AddExcludedTextBox.Text;
            Timer.Start();
            AddExcludedTextBox.Text = "Add...";
        }

        void timer_Tick(object sender, EventArgs e)
        {
            Text = "";
            Timer.Stop();
        }

        private void TextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                AddExcluded(AddExcludedTextBox.Text);
                AddExcludedTextBox.Text = "";
            }
        }
    }
}
