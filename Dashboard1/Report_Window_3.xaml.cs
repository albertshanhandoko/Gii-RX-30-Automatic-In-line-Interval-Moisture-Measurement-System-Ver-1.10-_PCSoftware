using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Dashboard1.Library;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.html;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using System.ComponentModel; // CancelEventArgs
using System.Configuration;
using Dashboard1.Helper;
using System.Text.RegularExpressions;

namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for Report_Window_3.xaml
    /// </summary>
    public partial class Report_Window_3 : Window
    {
        //static string comport = ((MainWindow)Application.Current.MainWindow).ComboBox_Port3.Text;
        //static int BaudRate = int.Parse(((MainWindow)Application.Current.MainWindow).ComboBox_BaudRate3.Text);

        static string comport = "COM2";
        static int BaudRate = 1200;


        SerialPort mySerialPort = new SerialPort(comport);
        List<Data_Measure> data_finals = new List<Data_Measure> { };
        int counter = 0;
        const string sensorName = "Sensor3";
        static string Folder_Path = ConfigurationManager.AppSettings["Folder_Path"] ?? "Not Found";
        static string application_name = ConfigurationManager.AppSettings["application_name"] ?? "Not Found";
        public Report_Window_3()
        {
            InitializeComponent();
            txt_time.Text = DateTime.Now.ToString("h:mm:ss tt");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            OpenCon_Port(BaudRate);
            //MessageBox.Show(BaudRate.ToString(), "Whatwhat");

            List<Data_PDFHistory> Sensor1_Histories = new List<Data_PDFHistory> { };
            Sensor1_Histories = SensorHelper_2.Read_PDF_History(sensorName);
            HistoryGrid1.ItemsSource = Sensor1_Histories;

            if (((MainWindow)Application.Current.MainWindow).RadioBtn_English.IsChecked == true)
            {
                Title_Label.Text = "Supplier Name:";
                Title_Date.Text = "Date:";
                Title_Time.Text = "Time:";
                Title_Average.Text = "Average:";
                Title_NoMeasure.Text = "No. of Measurement:";
                Title_PrintedBy.Text = "Printed By:";
            }
            else
            {
                Title_Label.Text = "Nama Supplier:";
                Title_Date.Text = "Tanggal:";
                Title_Time.Text = "Waktu:";
                Title_Average.Text = "Rata-Rata:";
                Title_NoMeasure.Text = "Jumlah Pengukuran:";
                Title_PrintedBy.Text = "Print Oleh:";

            }


        }

        private void OpenCon_Port(int BaudRate)
        {
            //SerialPort SerialPort = new SerialPort(PortName);
            mySerialPort.BaudRate = BaudRate;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.ReadBufferSize = 2000000;
            mySerialPort.Encoding = ASCIIEncoding.ASCII;
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(ProcessSensorData);
            mySerialPort.Open();
            Application.Current.Dispatcher.Invoke(new Action(() => {
                MessageBox.Show("Port is opened. Start Collecting Data", application_name);
            }));

        }

        private void Open_PDF_Folder_click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", @"C:\Sensor_Data\Print_Result_Sensor3");
        }
        private void Mouse_OpenPDF(object sender, MouseEventArgs e)
        {
            string PDFurl = Folder_Path + "Print_Result_" + sensorName;
            Process.Start("explorer.exe", @PDFurl);
        }
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedclick = HistoryGrid1.SelectedItems.ToString();
            Data_PDFHistory PDFHistory = (Data_PDFHistory)HistoryGrid1.SelectedItem;
            //D:\Sensor_Data\Print_Result_Sensor1

            //string urlpdf = "D:/Sensor_data/Print_Result_Sensor1/" + PDFHistory.Histories;
            string urlpdf = Folder_Path + "Print_Result_" + sensorName + "/" + PDFHistory.Histories;

            Console.WriteLine(urlpdf);
            Process.Start("sumatrapdf.exe", urlpdf);

        }

        public void DataFinalization()
        {
            // Empty intial data
            txt_value1.Text = String.Empty;
            txt_value2.Text = String.Empty;
            txt_value3.Text = String.Empty;
            txt_value4.Text = String.Empty;
            txt_value5.Text = String.Empty;
            txt_value6.Text = String.Empty;
            txt_value7.Text = String.Empty;
            txt_value8.Text = String.Empty;
            txt_value9.Text = String.Empty;

            txt_date1.Text = String.Empty;
            txt_date2.Text = String.Empty;
            txt_date3.Text = String.Empty;
            txt_date4.Text = String.Empty;
            txt_date5.Text = String.Empty;
            txt_date6.Text = String.Empty;
            txt_date7.Text = String.Empty;
            txt_date8.Text = String.Empty;
            txt_date9.Text = String.Empty;

            int jumlah_data = data_finals.Count;
            string average = data_finals[jumlah_data - 1].Measures;
            data_finals.RemoveAt(jumlah_data - 1);

            txt_average.Text = average;
            txt_valueavg.Text = average;
            txt_measure.Text = (jumlah_data - 1).ToString();

            //@data.weeks[@i].name
            //@txt_value[@i].Text
            //this.approved_by = IsNullOrEmpty(planRec.approved_by) ? "" : planRec.approved_by.toString();
            if (data_finals.Count >= 1)
            {
                txt_value1.Text = string.IsNullOrEmpty(data_finals[0].Measures) ? "" : data_finals[0].Measures;
                string date1 = data_finals[0].Created_date.ToString();
                txt_date1.Text = string.IsNullOrEmpty(date1) ? "" : date1;
            }
            if (data_finals.Count >= 2)
            {
                txt_value2.Text = string.IsNullOrEmpty(data_finals[1].Measures) ? "" : data_finals[1].Measures;
                string date2 = data_finals[1].Created_date.ToString();
                txt_date2.Text = string.IsNullOrEmpty(date2) ? "" : date2;
            }

            if (data_finals.Count >= 3)
            {
                txt_value3.Text = string.IsNullOrEmpty(data_finals[2].Measures) ? "" : data_finals[2].Measures;
                string date3 = data_finals[2].Created_date.ToString();
                txt_date3.Text = string.IsNullOrEmpty(date3) ? "" : date3;
            }
            if (data_finals.Count >= 4)
            {
                txt_value4.Text = string.IsNullOrEmpty(data_finals[3].Measures) ? "" : data_finals[3].Measures;
                string date4 = data_finals[3].Created_date.ToString();
                txt_date4.Text = string.IsNullOrEmpty(date4) ? "" : date4;
            }
            if (data_finals.Count >= 5)
            {
                txt_value5.Text = string.IsNullOrEmpty(data_finals[4].Measures) ? "" : data_finals[4].Measures;
                string date5 = data_finals[4].Created_date.ToString();
                txt_date5.Text = string.IsNullOrEmpty(date5) ? "" : date5;
            }
            if (data_finals.Count >= 6)
            {
                txt_value6.Text = string.IsNullOrEmpty(data_finals[5].Measures) ? "" : data_finals[5].Measures;
                string date6 = data_finals[5].Created_date.ToString();
                txt_date6.Text = string.IsNullOrEmpty(date6) ? "" : date6;
            }
            if (data_finals.Count >= 7)
            {
                txt_value7.Text = string.IsNullOrEmpty(data_finals[6].Measures) ? "" : data_finals[6].Measures;
                string date7 = data_finals[6].Created_date.ToString();
                txt_date7.Text = string.IsNullOrEmpty(date7) ? "" : date7;
            }
            if (data_finals.Count >= 8)
            {
                txt_value8.Text = string.IsNullOrEmpty(data_finals[7].Measures) ? "" : data_finals[7].Measures;
                string date8 = data_finals[7].Created_date.ToString();
                txt_date8.Text = string.IsNullOrEmpty(date8) ? "" : date8;
            }
            if (data_finals.Count >= 9)
            {
                txt_value9.Text = string.IsNullOrEmpty(data_finals[8].Measures) ? "" : data_finals[8].Measures;
                string date9 = data_finals[8].Created_date.ToString();
                txt_date9.Text = string.IsNullOrEmpty(date9) ? "" : date9;
            }

        }
        public void ProcessSensorData(object sender, SerialDataReceivedEventArgs args)
        {
            bool isString = true;

            try
            {
                Thread.Sleep(3000);// this solves the problem

                byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
                int readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                string readStr = string.Empty;
                if (isString)
                {

                    readStr = Encoding.Default.GetString(readBuffer, 0, readLen);
                    //readStr = Encoding.Default.GetString(readBuffer,,)
                    Console.WriteLine("IsString write: " + readStr);
                    //MessageBox.Show("string before: " + readStr, application_name);

                    //MessageBox.Show("string after: " + readStr, application_name);


                    if (readStr == "" || readStr == null)
                    {
                        Console.WriteLine("Data is empty");

                    }
                    else if (readStr.Trim().Contains("E") || readStr.Trim().ToLower().Contains("A"))
                    {
                        // Split
                        string[] lines = readStr.Split(
                            new[] { "\r\n", },
                            StringSplitOptions.None
                        );
                        string[] charactersToReplace = new string[] { @"\t", @"\n", @"\r", " ", "<CR>", "<LF>" };

                        //Insert into Data_final
                        foreach (string line in lines)
                        {
                            string currentline = line.Trim();
                            foreach (string s in charactersToReplace)
                            {
                                currentline = currentline.Replace(s, "");
                            }
                            currentline = Regex.Replace(currentline, "[^0-9.]", "");
                            readStr = currentline;

                            if (line.Length == 6)
                            {
                                string readStr1 = String.Concat(readStr.Substring(0, 3));

                                readStr1 = String.Concat(readStr1.Substring(0, readStr1.Length - 1)
                                    , ".", readStr1.Substring(readStr1.Length - 1, 1));
                                Console.WriteLine("Data Final is: " + readStr1);
                                Data_Measure data_final = new Data_Measure(counter + 1, readStr1, DateTime.Now);
                                data_finals.Add(data_final);

                                string readStr2 = String.Concat(readStr.Substring(3, 3));
                                //readStr = currentline;
                                readStr2 = String.Concat(readStr2.Substring(0, readStr2.Length - 1)
                                    , ".", readStr2.Substring(readStr2.Length - 1, 1));
                                Console.WriteLine("Data Final is: " + readStr2);
                                Data_Measure data_final2 = new Data_Measure(counter + 1, readStr2, DateTime.Now);
                                data_finals.Add(data_final2);
                            }

                            if (!line.ToLower().Contains("e") && line != "" && line != null && line.Length != 6)
                            {
                                readStr = String.Concat(readStr.Substring(0, readStr.Length - 1)
                                    , ".", readStr.Substring(readStr.Length - 1, 1));
                                Console.WriteLine("Data Final is: " + readStr);
                                Data_Measure data_final = new Data_Measure(counter + 1, readStr, DateTime.Now);
                                data_finals.Add(data_final);
                            }
                        }

                        // line 1 = last data. 
                        // line 2 = average data.
                        // line 3 = E

                        // Close the port
                        Console.WriteLine("Data E is : " + readStr);
                        mySerialPort.Close();
                        Application.Current.Dispatcher.Invoke(new Action(() => {

                            DataFinalization();
                            counter = 0;
                            MessageBox.Show("All data collected. Port connection closed", application_name);

                        }));

                    }
                    else
                    {
                        // Clean data
                        readStr = readStr.Trim();
                        string[] charactersToReplace = new string[] { @"\t", @"\n", @"\r", " ", "<CR>", "<LF>" };
                        foreach (string s in charactersToReplace)
                        {
                            readStr = readStr.Replace(s, "");
                        }
                        readStr = Regex.Replace(readStr, "[^0-9.]", "");
                        readStr = String.Concat(readStr.Substring(0, readStr.Length - 1)
                            , ".", readStr.Substring(readStr.Length - 1, 1));
                        //MyString.Substring(MyString.Length-6);
                        Console.WriteLine("Data receive not null is: " + readStr);
                        Data_Measure data_final = new Data_Measure(counter + 1, readStr, DateTime.Now);
                        data_finals.Add(data_final);

                        //
                        if (counter == 0)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value1.Text = data_finals[0].Measures;
                                txt_date1.Text = data_finals[0].Created_date.ToString();
                            }));
                        }

                        else if (counter == 1)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value2.Text = data_finals[1].Measures;
                                txt_date2.Text = data_finals[1].Created_date.ToString();
                            }));
                        }

                        else if (counter == 2)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value3.Text = data_finals[2].Measures;
                                txt_date3.Text = data_finals[2].Created_date.ToString();
                            }));
                        }

                        else if (counter == 3)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value4.Text = data_finals[3].Measures;
                                txt_date4.Text = data_finals[3].Created_date.ToString();
                            }));
                        }

                        else if (counter == 4)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value5.Text = data_finals[4].Measures;
                                txt_date5.Text = data_finals[4].Created_date.ToString();
                            }));
                        }

                        else if (counter == 5)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value6.Text = data_finals[5].Measures;
                                txt_date6.Text = data_finals[5].Created_date.ToString();
                            }));
                        }

                        else if (counter == 6)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value7.Text = data_finals[6].Measures;
                                txt_date7.Text = data_finals[6].Created_date.ToString();
                            }));
                        }

                        else if (counter == 7)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value8.Text = data_finals[7].Measures;
                                txt_date8.Text = data_finals[7].Created_date.ToString();
                            }));
                        }

                        else if (counter == 8)
                        {
                            Application.Current.Dispatcher.Invoke(new Action(() => {
                                txt_value9.Text = data_finals[8].Measures;
                                txt_date9.Text = data_finals[8].Created_date.ToString();
                            }));
                        }
                        else
                        {
                            Console.WriteLine("Data Overload");

                        }
                        counter = counter + 1;
                    }
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i < readLen; i++)
                    {
                        stringBuilder.Append(readBuffer[i].ToString("X2") + " ");
                    }
                    readStr = stringBuilder.ToString();
                    Console.WriteLine("Else write:" + readStr);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Console.WriteLine(ex);
            }
        }

        private void Generate_PDF(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txt_Label.Text))
            {
                MessageBox.Show("Please fill the Label", application_name);
                //check_data = 0;
            }
            else if (String.IsNullOrEmpty(txt_printedby.Text))
            {
                MessageBox.Show("Please fill the Printed by", application_name);
                //check_data = 0;
            }
            else
            {
                int PageSize = ((MainWindow)Application.Current.MainWindow).ComboBox_Pagesize.SelectedIndex;
                int language = 0;
                if (((MainWindow)Application.Current.MainWindow).RadioBtn_English.IsChecked == false)
                {
                    language = 1;
                }
                //Match match = Regex.Match(txt_Label.Text, @"(?i)^[a-z]+");

                // A4
                if (PageSize == 0 || PageSize == 2)
                {
                    SensorHelper_2.Generate_Simple_PDF_A4(txt_Label.Text, sensorName
                        , txt_date.Text, txt_time.Text, txt_average.Text,
                    txt_measure.Text, txt_printedby.Text, data_finals, language);

                }
                //A5
                else if (PageSize == 1)
                {
                    SensorHelper_2.Generate_Simple_PDF_A5(txt_Label.Text, sensorName
                       , txt_date.Text, txt_time.Text, txt_average.Text,
                   txt_measure.Text, txt_printedby.Text, data_finals, language);

                }
                else
                {
                    SensorHelper_2.Generate_Simple_PDF_A5_Small(txt_Label.Text, sensorName
                       , txt_date.Text, txt_time.Text, txt_average.Text,
                   txt_measure.Text, txt_printedby.Text, data_finals, language);

                }


                Thread.Sleep(100);
                Application.Current.Dispatcher.Invoke(new Action(() => {
                    List<Data_PDFHistory> Sensor_Histories = new List<Data_PDFHistory> { };
                    Sensor_Histories = SensorHelper_2.Read_PDF_History(sensorName);
                    HistoryGrid1.ItemsSource = Sensor_Histories;
                }));

                MessageBox.Show("PDF Successfully Created, please check the PDF Folder", application_name);
            }
        }

    }
}
