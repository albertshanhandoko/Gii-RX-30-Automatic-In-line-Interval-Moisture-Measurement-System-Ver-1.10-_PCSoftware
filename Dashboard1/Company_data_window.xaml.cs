using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.IO;
using System.Threading;
using Dashboard1.Helper;
using Microsoft.Win32;
using System.Diagnostics;


namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for Company_data_window.xaml
    /// </summary>
    /// 
   
    public partial class Company_data_window : Window
    {
        static string Folder_Path = ConfigurationManager.AppSettings["Folder_Path"] ?? "Not Found";
        static string application_name = ConfigurationManager.AppSettings["application_name"] ?? "Not Found";

        public Company_data_window()
        {
            InitializeComponent();
            txt_companyName.Text = SensorHelper_2.read_config_name();
            txt_companyAddress.Text = SensorHelper_2.read_config_addr();
        }

        private void Btn_AddLogo_Click(object sender, RoutedEventArgs e)
        {

            //string addr_text = "File not found";
            string filepath;
            OpenFileDialog open = new OpenFileDialog();
            open.Multiselect = false;
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            bool? result = open.ShowDialog();
            if (result == true)
            {
                filepath = open.FileName; // Stores Original Path in Textbox
                txt_companyLogo.Text = filepath;
                //ImageSource imgsource = new BitmapImage(new Uri(filepath));
                //File.Copy(filepath, url_logo_config, true);
                //MessageBox.Show("Logo successfully saved", application_name);
            }
        }


        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txt_companyName.Text) || String.IsNullOrEmpty(txt_companyAddress.Text))
            {
                MessageBox.Show("Please enter company name and address", application_name);
            }
            else
            {
                string url_name_config = Folder_Path + "/DataConfig/config_name.txt";
                //string url_name_config = "D:/Sensor_data/DataConfig/config_name.txt";
                string comp_name = txt_companyName.Text;
                SensorHelper_2.writeTextFile(url_name_config, comp_name);
                //MessageBox.Show("Company name successfully saved", application_name);

                string url_addr_config = Folder_Path + "/DataConfig/config_addr.txt";
                //string url_addr_config = "D:/Sensor_data/DataConfig/config_addr.txt";
                string comp_addr = txt_companyAddress.Text;
                SensorHelper_2.writeTextFile(url_addr_config, comp_addr);
                //MessageBox.Show("Company address successfully saved", application_name);

                string url_logo_config = Folder_Path + "/DataConfig/Logo.png";
                //string url_logo_config_temp = Folder_Path + "/DataConfig/Logo_temp.png";
                

                string filepath = txt_companyLogo.Text; // Stores Original Path in Textbox

                Uri resourceUri = new Uri(filepath, UriKind.RelativeOrAbsolute);
                //((MainWindow)Application.Current.MainWindow).imgDynamic.Source = new BitmapImage(resourceUri);

                if (!String.IsNullOrEmpty(txt_companyLogo.Text))
                {

                    //File.Delete(url_logo_config);
                    ImageSource imgsource = new BitmapImage(new Uri(filepath));
                    File.Copy(filepath, url_logo_config, true);
                    //File.Copy(url_logo_config, url_logo_config_temp, true);

                }
                MessageBox.Show("Company Data successfully saved", application_name);

            }
        }
    }
}
