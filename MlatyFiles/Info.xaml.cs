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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PGTA_WPF
{
    /// <summary>
    /// Lógica de interacción para Info.xaml
    /// </summary>
    public partial class Info : Page
    {
        public Info()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, RoutedEventArgs e)
        {
            Label.Text = "The program allows you to load as many files as you want. In order for the program to calculate the parameters, the files must contain MLAT and ADS-B messages with squitter v.2 or DGPS and MLAT data from the same aircraft.\n \nTo upload a file, in the manage data tab click on load file and a window will open to select the files you want. You can choose as many files as you like. \n \nOnce you have chosen the files you will see that they appear in a list, and next to the name of each file its status is shon. The status will be, Uncomputed if the parameters of that file have not been calculated yet, Computed if they have already been calculated, or an indication of the process if they are being calculated at this time. If the parameters of a file cannot be calculated, a label indicating 'Can't compute this file' will appear next to the wrong file. \n \nOnce the files are selected, click on 'Compute' to calculate their parameters. Before clicking, you can choose whether you want to save the file markers or not, so that you can see them on the map. This will allow you to see the messages used for the calculation of parameters, located on the map, and painted in different colors depending on the area where they are. Even so, keep in mind that saving those messages takes a little more resources, so if you do not intend to use them, it is better to uncheck that option. \n \nAlthough, you can choose this option for some files and not for others, computing the files for which we want to save the markers with this option selected, and then computing the files for which we do not want to save the markers in another batch, with the option unchecked. In this way, only the positions of the files that were calculated with this option checked will be saved. \n \nIf after loading some files you want to load more, repeat the process, and the program will load and compute the new selected files. \n \nOnce you have the files uploaded and computed, you can see the results. These are divided between accuracy and update rate(PD). \n \nIn the Accuracy tab you can see the results of the 95 percentile, the 99 percentile, the number of MLAT messages used, the number of ADSB messages used, the mean and the standard deviation(STD), of each zone and of the global zones. \n \nKeep in mind, that to be able to compute the percentile 95  there must be at least 20 messages, and for the percentile 99, 100 messages are necessary, so if there are fewer, these parameters cannot be calculated. Even so, the mean and STD will be calculated, and in the global P95 and P99 messages from all the areas will be taken into account, including those with less than 20 and 100 messages respectively. \n \nIn the Update Rate you can see the information of that parameter in each zone. The expected messages are the messages that should have been received if the update rate had been as established in the specifications. Missing messages are the messages that were not received, and the Update Rate is the percentage of the missing messages with respect to the expected ones. The update rates used to calculate the expected and the missing messages are those established by document ED117. \n \nTwo corrections were also applied.The first one is to exclude the airport vehicles that use squitter instead of transponder, and the second one took into account the possibility of an airplane being stopped for a while and then turning on again, considering that if for more than a minute it was not received updates the problem is not a MLAT detection failure, but the aircraft stopped transmitting. \n \nFinally, in the map tab, we can see the areas painted on the airport, and we can select and deselect the areas we want to be shown on the map. We can also ask them to show us the MLAT and/or ADS-B positions, which will be painted in a color similar to the area where they bellong so that we know which area they belong to. \n \nThis last option can only be used if when loading the files we choose the option to save the bookmarks.";
            Label.Width = Column.ActualWidth-100;
            
            ScrollBar.Width = this.Width;
            ScrollBar.Height = this.Height;
        }

        private void Resize(object sender, SizeChangedEventArgs e)
        {

            Label.Width = Column.ActualWidth - 100;
        }
    }
}
