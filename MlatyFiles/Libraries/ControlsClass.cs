using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Mlaty
{


    public class MyButton : Button
    {
        public bool down = false;

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            StackPanel panel = this.Content as StackPanel;
            foreach (object child in panel.Children)
            {
                Image image =child as Image;
                
                if (image != null)
                {
                    image.Source = new BitmapImage(new Uri(@"Images/SaveBlackIcon.png", UriKind.Relative));
                    break;
                }
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            StackPanel panel = this.Content as StackPanel;
            foreach (object child in panel.Children)
            {
                Image image = child as Image;
                if (image != null)
                {
                    image.Source = new BitmapImage(new Uri(@"Images/SaveIcon.png", UriKind.Relative));
                    break;
                }

            }
        }
    }
    
}
