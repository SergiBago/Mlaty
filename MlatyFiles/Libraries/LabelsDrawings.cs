using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PGTA_WPF
{
    public class LabelsDrawings
    {
        static public Bitmap InsertText(Labels marker)
        {
            //Bitmap bmp;
            Bitmap returnBitmap = new Bitmap(120, 10);
            Graphics g = Graphics.FromImage(returnBitmap);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            ////     //// The interpolation mode determines how intermediate values between two endpoints are calculated.
            g.InterpolationMode = InterpolationMode.High;

            ////// Use this property to specify either higher quality, slower rendering, or lower quality, faster rendering of the contents of this Graphics object.
            g.PixelOffsetMode = PixelOffsetMode.Default;

            ////// This one is important
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            Font font = new Font("Arial", 11, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel);


            var stringSize = g.MeasureString(marker.caption, font);
            var localPoint = new PointF((returnBitmap.Width - stringSize.Width) / 2, (returnBitmap.Height - stringSize.Height) / 2); //

            System.Drawing.Brush color = new SolidBrush(System.Drawing.Color.FromArgb(255, (byte)0, (byte)0, (byte)0));



            g.DrawString(marker.caption, font, color, localPoint); /// locarPoint cambiado por rectf

            g.Flush();



            g.Dispose();

            return (returnBitmap);
        }

        static public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            MemoryStream memory;
            BitmapImage bitmapImage;
            using (memory = new MemoryStream())
            {
                //var bitmap1 = bitmap;
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                bitmapImage.StreamSource = memory;

                bitmapImage.EndInit();
                memory.Close();
                memory.Dispose();
                memory = null;
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
    }
}
