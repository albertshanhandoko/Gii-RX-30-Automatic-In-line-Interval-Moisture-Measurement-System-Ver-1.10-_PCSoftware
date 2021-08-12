using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
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
using LiveCharts;
using LiveCharts.Wpf;
using Spire.Xls;
using Spire.Xls.Charts;

namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for LiveChart_Test.xaml
    /// </summary>
    public partial class LiveChart_Test : Window
    {

        public LiveChart_Test()
        {
            InitializeComponent();


            //CartesianChart BarChart = new CartesianChart();
            //\BarChart.AxisY

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                }
            };

            //adding series will update and animate the chart automatically
            SeriesCollection.Add(new ColumnSeries
            {
                Title = "2016",
                Values = new ChartValues<double> { 11, 56, 42 }
            });

            //also adding values updates and animates the chart automatically
            SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            Formatter = value => value.ToString("N");

            DataContext = this;

            
            //Bitmap bmp = new Bitmap(CartesianChart_1.Width.ToString(), CartesianChart_1.Height.ToString());
            //Bitmap bmp = new Bitmap(800, 450);

            //CartesianChart_1.  DrawToBitmap(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height));
            //bmp.Save("C:\\graph.png", ImageFormat.Png);

            Bitmap printscreen = new Bitmap(800,450);
            Graphics graphics = Graphics.FromImage(printscreen as System.Drawing.Image);

            graphics.CopyFromScreen(10, 10, 10, 10, printscreen.Size);

            //printscreen.Save("D:\\printscreen.jpg", ImageFormat.Jpeg);

            //Rectangle rect = new Rectangle(0, 0, 100, 100);
            Bitmap bmp = new Bitmap(800, 450, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            //bmp.Save("D:\\printscreen.jpg", ImageFormat.Jpeg);

            var image = ScreenCapture.CaptureActiveWindow();
            image.Save("D:\\snippetsource.jpg", ImageFormat.Jpeg);


            var DataValue = new LiveCharts.Wpf.PieChart
            {
                DisableAnimations = true,
                Width = 600,
                Height = 387,
                Series = SeriesCollection
            };

            var viewbox = new Viewbox();
            viewbox.Child = DataValue;
            viewbox.Measure(DataValue.RenderSize);
            viewbox.Arrange(new Rect(new System.Windows.Point(0, 0), DataValue.RenderSize));
            DataValue.Update(true, true);
            viewbox.UpdateLayout();
            //Save as image method with chart and name of image
            
            //SaveToPng(DataValue, "image.png");



        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }


        public class ScreenCapture
        {
            [DllImport("user32.dll")]
            private static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
            public static extern IntPtr GetDesktopWindow();

            [StructLayout(LayoutKind.Sequential)]
            private struct Rect
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }

            [DllImport("user32.dll")]
            private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

            public static System.Drawing.Image CaptureDesktop()
            {
                return CaptureWindow(GetDesktopWindow());
            }

            public static Bitmap CaptureActiveWindow()
            {
                return CaptureWindow(GetForegroundWindow());
            }

            public static Bitmap CaptureWindow(IntPtr handle)
            {
                var rect = new Rect();
                GetWindowRect(handle, ref rect);
                var bounds = new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                var result = new Bitmap(bounds.Width, bounds.Height);

                using (var graphics = Graphics.FromImage(result))
                {
                    graphics.CopyFromScreen(new System.Drawing.Point(bounds.Left, bounds.Top), System.Drawing.Point.Empty, bounds.Size);
                }

                return result;
            }
        }

    }
}
