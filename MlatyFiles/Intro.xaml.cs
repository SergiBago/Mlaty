﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PGTAWPF
{
    /// <summary>
    /// Lógica de interacción para Intro.xaml
    /// </summary>
    public partial class Intro : Page
    {
        public Intro()
        {
            InitializeComponent();
        }

        MainWindow Form = new MainWindow();

        public void GetMainWindow( MainWindow form)
        {
            this.Form = form;
        }
        private void GetStarted_Click(object sender, MouseButtonEventArgs e)
        {
            //Form.OpenLoad();
        }

        private void Resize(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();
            if (this.ActualWidth<1050)
            {
                PageGrid.MaxWidth = 850;
            }
            else
            {
                PageGrid.MaxWidth = this.ActualWidth - 200;
            }
            if(PageScrollViewer.ComputedVerticalScrollBarVisibility==Visibility.Visible && PageScrollViewer.ComputedHorizontalScrollBarVisibility==Visibility.Visible)
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
