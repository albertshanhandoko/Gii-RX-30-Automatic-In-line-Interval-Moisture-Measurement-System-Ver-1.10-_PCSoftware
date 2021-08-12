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
using System.Timers;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;
using System.ComponentModel; // CancelEventArgs
using System.Configuration;
using Dashboard1.Helper;
using Dashboard1.Constant;

using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Reflection;
using LiveCharts.Configurations;
using LiveCharts;
using LiveCharts.Wpf;

namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for Data_input_window.xaml
    /// </summary>
    /// 
    public static class MyExt
    {
        public static void PerformClick(this Button btn)
        {
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }
    }
    
    /*
    public string Test1245(this Enum value)
    {
        return ((DescriptionAttribute)Attribute.GetCustomAttribute(
            value.GetType().GetFields(BindingFlags.Public | BindingFlags.Static)
                .Single(x => x.GetValue(null).Equals(value)),
            typeof(DescriptionAttribute)))?.Description ?? value.ToString();
    }
    public void test()
    {
        string testc =
        ((DescriptionAttribute)Attribute.GetCustomAttribute(
    typeof(myEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
        .Single(x => (myEnum)x.GetValue(null) == enumValue),
    typeof(DescriptionAttribute))).Description;
    }
    */


    public partial class Data_input_window : Window
    {
        int pageIndex = 1;
        private int numberOfRecPerPage;
        private enum PagingMode { First = 1, Next = 2, Previous = 3, Last = 4, PageCountChange = 5 };


        //static string comport = ((MainWindow)Application.Current.MainWindow).ComboBox_input_data.Text;
        static string comport = "port1";
        private static System.Timers.Timer aTimer;
        //static int BaudRate = int.Parse(((MainWindow)Application.Current.MainWindow).ComboBox_Input_Baud.Text);
        static int BaudRate = 1200;
        SerialPort mySerialPort = new SerialPort(comport);
        SerialPort mySerialPort2 = new SerialPort(comport);

        int counter_data = 0;

        /*
        string test[] = ((DescriptionAttribute)Attribute.GetCustomAttribute
            (typeof(myEnum).GetFields(BindingFlags.Public | BindingFlags.Static).All(),
        typeof(DescriptionAttribute))
            ).Description;
        */

        int timer_baiz_5;
        static string Folder_Path = ConfigurationManager.AppSettings["Folder_Path"] ?? "Not Found";
        static string application_name = ConfigurationManager.AppSettings["application_name"] ?? "Not Found";

        int counter_interval;
        int JumlahInterval;
        int TimeInterval;
        int NumberGrain;
        int NumberMeasure;
        int NumberGrain_Frekuensi = -1;
        int current_interval = 0;
        int delay = 0;
        int start_next_init = 0;
        string ResultGrain = "";
        string ResultMeasure = "";
        bool StatusListen = false;
        bool StatusAggre = true;
        int countersleep = 0;
        
        bool start_next_cond;
        bool aggregate_cond;
        List<data_measure_2> data_finals_update = new List<data_measure_2> { };
        List<data_measure_2> data_finals_update_2 = new List<data_measure_2> { };
        List<data_measure_2> temp_data_finals_2 = new List<data_measure_2> { };

        List<data_measure_2> data_Grid_1_list_a = new List<data_measure_2> { };
        List<data_measure_2> data_Grid_1_list_b = new List<data_measure_2> { };

        List<data_measure_2> data_Grid_2_list_a = new List<data_measure_2> { };
        List<data_measure_2> data_Grid_2_list_b = new List<data_measure_2> { };

        List<data_measure_2> data_Grid_3_list_a = new List<data_measure_2> { };
        List<data_measure_2> data_Grid_3_list_b = new List<data_measure_2> { };

        List<data_measure_2> data_Grid_4_list_a = new List<data_measure_2> { };
        List<data_measure_2> data_Grid_4_list_b = new List<data_measure_2> { };

        List<data_measure_2> data_Grid_5_list_a = new List<data_measure_2> { };
        List<data_measure_2> data_Grid_5_list_b = new List<data_measure_2> { };

        List<data_measure_2> data_Average = new List<data_measure_2> { };


        List<Data_Measure> data_finals_ori = new List<Data_Measure> { };
        List<Data_Measure> temp_data_finals = new List<Data_Measure> { };

        //public YourCollection MyObjects { get; } = new YourCollection();
        //observe_datameasure data_finals_test { get; } = new observe_datameasure();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        bool stat_continue = true;
        public static IEnumerable<string> GetDescriptions<myEnum>()
        {
            var attributes = typeof(myEnum).GetMembers()
                .SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                .ToList();

            return attributes.Select(x => x.Description);

        }

        

        public Data_input_window()
        {
            InitializeComponent();
            OpenCon_Port_local(mySerialPort, BaudRate);
            this.DataContext = this;
            //data_finals_update = SensorHelper_2.Test_DataMeasure_2();

            //Data_Receive_Grid.ItemsSource = data_finals_ori;
            data_initiation_input();
        }

        private void data_initiation_input()
        {
            /*
            ComboBox_TimeInterval.SelectedValuePath = "Key";
            ComboBox_TimeInterval.DisplayMemberPath = "Value";
            ComboBox_TimeInterval.Items.Add(new KeyValuePair<int, string>(0, "30 sec"));
            ComboBox_TimeInterval.Items.Add(new KeyValuePair<int, string>(1, "60 sec "));
            ComboBox_TimeInterval.Items.Add(new KeyValuePair<int, string>(2, "90 sec"));
            */
            for (int i=1;i<=30;i++)
            {
                ComboBox_TimeInterval.Items.Add(i.ToString());
            }

            List<string> List_TimeInter = Sensor_input_Helper.Get_Time_Interval();
            //IEnumerable<string> List_enumrea = Sensor_input_Helper.Get_Time_Interval_enumera();

            foreach(string TimeInter in List_TimeInter)
            {
                ComboBox_TimeInterval.Items.Add(TimeInter);
            }

            foreach (int i in Enum.GetValues(typeof(number_grain)))
            {
                ComboBox_NumberGrain.Items.Add(i.ToString());
            }    

            List<string> List_TypeMeasure = Enum.GetNames(typeof(TypeOfMeasure)).ToList();
            foreach (string Measure in List_TypeMeasure)
            {
                ComboBox_NumberMeasure.Items.Add(Measure);
            }

            /*
            //
            ComboBox_NumberInterval.SelectedValuePath = "Key";
            ComboBox_NumberInterval.DisplayMemberPath = "Value";
            ComboBox_NumberInterval.Items.Add(new KeyValuePair<int, string>(0, "1"));
            ComboBox_NumberInterval.Items.Add(new KeyValuePair<int, string>(1, "2"));
            ComboBox_NumberInterval.Items.Add(new KeyValuePair<int, string>(2, "3"));

            //
            ComboBox_NumberGrain.SelectedValuePath = "Key";
            ComboBox_NumberGrain.DisplayMemberPath = "Value";
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(0, "10"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(1, "20"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(2, "30"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(3, "40"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(4, "50"));
            /*
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(5, "6"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(6, "7"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(7, "8"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(8, "9"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(9, "10"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(10, "11"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(11, "12"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(12, "13"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(13, "14"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(14, "15"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(15, "16"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(16, "17"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(17, "18"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(18, "19"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(19, "20"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(20, "21"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(21, "22"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(22, "23"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(23, "24"));
            ComboBox_NumberGrain.Items.Add(new KeyValuePair<int, string>(24, "25"));
            

            ComboBox_NumberMeasure.SelectedValuePath = "Key";
            ComboBox_NumberMeasure.DisplayMemberPath = "Value";
            ComboBox_NumberMeasure.Items.Add(new KeyValuePair<int, string>(0, "Short Paddy"));
            ComboBox_NumberMeasure.Items.Add(new KeyValuePair<int, string>(1, "Long Paddy"));
            ComboBox_NumberMeasure.Items.Add(new KeyValuePair<int, string>(2, "Jasmine Paddy"));
            ComboBox_NumberMeasure.Items.Add(new KeyValuePair<int, string>(3, "Long Sticky Paddy"));
            ComboBox_NumberMeasure.Items.Add(new KeyValuePair<int, string>(4, "Long Parboiled Rice"));
            ComboBox_NumberMeasure.Items.Add(new KeyValuePair<int, string>(5, "Peak A/D count value"));
            ComboBox_NumberMeasure.Items.Add(new KeyValuePair<int, string>(6, "Wheat"));
            */
        }


        public void RunSensor()
        {


            int NumberGrain = ComboBox_NumberGrain.SelectedIndex;
            int NumberMeasure = ComboBox_NumberMeasure.SelectedIndex;


            int TimeInterval = ComboBox_TimeInterval.SelectedIndex;
            int delay;
            switch (TimeInterval)
            {
                default:
                    delay = 30000;
                    break;
                case 0:
                    delay = 30000;
                    break;
                case 1:
                    delay = 60000;
                    break;
                case 2:
                    delay = 90000;
                    break;
            }

            string ResultGrain;
            switch (NumberGrain)
            {
                case 0:
                    ResultGrain = "10192\r";
                    NumberGrain_Frekuensi = 10;
                    break;
                case 1:
                    ResultGrain = "10293\r";
                    NumberGrain_Frekuensi = 20;
                    break;
                case 2:
                    ResultGrain = "10394\r";
                    NumberGrain_Frekuensi = 30;
                    break;
                case 3:
                    ResultGrain = "10495\r";
                    NumberGrain_Frekuensi = 40;
                    break;
                case 4:
                    ResultGrain = "10596\r";
                    NumberGrain_Frekuensi = 50;
                    break;
                default:
                    ResultGrain = "10697\r";
                    NumberGrain_Frekuensi = 60;
                    break;

            }

            string ResultMeasure = "";
            switch (NumberMeasure)
            {
                case -1:
                    ResultMeasure = "22094\r";
                    break;
                case 0:
                    ResultMeasure = "22094\r";
                    break;
                case 1:
                    ResultMeasure = "32095\r";
                    break;
                case 2:
                    ResultMeasure = "42096\r";
                    break;
                case 3:
                    ResultMeasure = "52097\r";
                    break;
                case 4:
                    ResultMeasure = "62098\r";
                    break;
                case 5:
                    ResultMeasure = "72094\r";
                    break;
                case 6:
                    ResultMeasure = "8209A\r";
                    break;
                default:
                    ResultMeasure = "22094\r";
                    break;
            }

            if (counter_interval > 0)
            {


                counter_interval = counter_interval - 1;

                if (counter_interval == 0)
                {
                    MessageBox.Show("All Measurement finish", application_name);
                    /*
                    if (temp_data_finals_2.Count > 0)
                    {
                        //data_finals_ori.AddRange(temp_data_finals);

                        //data_finals_update.AddRange(temp_data_finals_2);
                        data_finals_update_2.AddRange(temp_data_finals_2);

                        temp_data_finals_2.Clear();

                    }
                    */
                    //OpenCon_Port_local(mySerialPort, BaudRate);

                    //mySerialPort.Close();
                }
                else
                {
                    Thread.Sleep(delay);
                    OpenCon_Port_local(mySerialPort, BaudRate);
                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();
                    Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();

                    Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
                    //StatusListen = true;
                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();

                    MessageBox.Show("Start Next Sequence", application_name);
                    /*
                    Task.Delay(delay).ContinueWith(_ =>
                    {
                        OpenCon_Port_local(mySerialPort, BaudRate);
                        Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
                        Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
                        //StatusListen = true;

                        MessageBox.Show("Start Next Sequence", application_name);
                    }
                    );
                    */
                    //Thread.Sleep(delay);


                }

            }



        }


        private void btn_Check_click(object sender, RoutedEventArgs e)
        {
            Sensor_input_Helper.Command_Check(mySerialPort);
            MessageBox.Show("test pencet");
        }

        private void btn_CheckData_click(object sender, RoutedEventArgs e)
        {
            Sensor_input_Helper.Command_CheckData(mySerialPort);
        }

        private void btn_Stop_click(object sender, RoutedEventArgs e)
        {
            Sensor_input_Helper.Command_Stop(mySerialPort);
            //Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
            //Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
        }
        private void btn_NumberGrain_click(object sender, RoutedEventArgs e)
        {
            int NumberofGrain = ComboBox_NumberGrain.SelectedIndex;
            //int test = ((MainWindow)Application.).ComboBox_Port1.Text;
            //int NumberMeasure = ComboBox_NumberMeasure.SelectedIndex;

            string result = "";


            //Sensor_input_Helper.Command_NumberofGrain(mySerialPort, result);
            // Sensor_input_Helper.Command_MoistureMeasure(mySerialPort, result);

        }
        private void btn_MoistureAgg_click(object sender, RoutedEventArgs e)
        {
            if (!mySerialPort.IsOpen)
            {
                OpenCon_Port_local(mySerialPort, BaudRate);
            }
            Sensor_input_Helper.Command_MoisturAggregate(mySerialPort);
            //MessageBox.Show("Hitung Aggreagte command");

        }

        

        private void btn_MoistureMeasure_click(object sender, RoutedEventArgs e)
        {
            JumlahInterval = ComboBox_NumberInterval.SelectedIndex + 1;
            counter_interval = JumlahInterval;

            TimeInterval = ComboBox_TimeInterval.SelectedIndex;
            NumberGrain = ComboBox_NumberGrain.SelectedIndex;
            NumberMeasure = ComboBox_NumberMeasure.SelectedIndex;

            //TimeInterval = ComboBox_TimeInterval.SelectedIndex;
            //int delay;
            switch (TimeInterval)
            {
                default:
                    delay = 30000;
                    break;
                case 0:
                    delay = 30000;
                    break;
                case 1:
                    delay = 60000;
                    break;
                case 2:
                    delay = 90000;
                    break;
            }


            switch (NumberGrain)
            {
                case 0:
                    ResultGrain = "10192\r";
                    NumberGrain_Frekuensi = 10;
                    break;
                case 1:
                    ResultGrain = "10293\r";
                    NumberGrain_Frekuensi = 20;
                    break;
                case 2:
                    ResultGrain = "10394\r";
                    NumberGrain_Frekuensi = 30;
                    break;
                case 3:
                    ResultGrain = "10495\r";
                    NumberGrain_Frekuensi = 40;
                    break;
                case 4:
                    ResultGrain = "10596\r";
                    NumberGrain_Frekuensi = 50;
                    break;
                default:
                    ResultGrain = "10697\r";
                    NumberGrain_Frekuensi = 60;
                    break;

            }

            switch (NumberMeasure)
            {
                case -1:
                    ResultMeasure = "22094\r";
                    break;
                case 0:
                    ResultMeasure = "22094\r";
                    break;
                case 1:
                    ResultMeasure = "32095\r";
                    break;
                case 2:
                    ResultMeasure = "42096\r";
                    break;
                case 3:
                    ResultMeasure = "52097\r";
                    break;
                case 4:
                    ResultMeasure = "62098\r";
                    break;
                case 5:
                    ResultMeasure = "72094\r";
                    break;
                case 6:
                    ResultMeasure = "8209A\r";
                    break;
                default:
                    ResultMeasure = "22094\r";
                    break;
            }
            //Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
            //Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);

            if (JumlahInterval < 0 || TimeInterval < 0 || NumberGrain < 0 || NumberMeasure < 0)
            {
                MessageBox.Show("Please fill All mandatory value", application_name);
            }

            else
            {
                ComboBox_NumberInterval.IsEnabled = false;
                ComboBox_TimeInterval.IsEnabled = false;
                ComboBox_NumberGrain.IsEnabled = false;
                ComboBox_NumberMeasure.IsEnabled = false;

                

                MessageBox.Show("Sensor Start Collecting Data", application_name);
                Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
                Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
                //StatusListen = true;
                //await Method_int_async();
                
                Thread readThread = new Thread(Read);
                readThread.Start();
                
                /*
                while (stat_continue)
                {
                    if(StatusListen==false)
                    {
                        Thread.Sleep(5000);
                        Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
                        Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
                    }
                }
                */
                //mySerialPort.DataReceived += new SerialDataReceivedEventHandler(ProcessSensorData_local);
                //StatusListen = true;
                //dispatcherTimer.Tick += dispatcherTimer_Tick;
                //dispatcherTimer.Interval = new TimeSpan(0, 0, 7);
                //dispatcherTimer.Start();

                
                //await SendDataWithAsync();
            }
            //RunSensor();

        }

        public void Read()
        {
            while (stat_continue)
            {

                try
                {

                    if (data_Average.Count() == counter_interval && current_interval > 0 && counter_interval > 0)
                    {
                        stat_continue = false;
                        mySerialPort.DiscardInBuffer();
                        mySerialPort.DiscardOutBuffer();
                        mySerialPort.Close();

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            string Replace_Average = ",";
                            string temp_average;
                            float average = 0;
                            foreach (data_measure_2 data in data_Average)
                            {
                                temp_average = data.Measures;
                                temp_average = temp_average.Replace(Replace_Average, "");
                                average = average + float.Parse(temp_average);
                            }
                            float final_average = average / 10 / data_Average.Count();

                            string final_average_str = final_average.ToString();

                            //final_average_str = String.Concat(final_average_str.Substring(0, final_average_str.Length - 1)
                            //            , ".", final_average_str.Substring(final_average_str.Length - 1, 1));

                            Average_Final.Text = final_average_str;
                            MessageBox.Show("All Measurement Finish", application_name);

                        }));

                    }

                    else
                    {
                        Thread.Sleep(1000);// this solves the problem
                        byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
                        int readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                        string readStr = string.Empty;

                        readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                        readStr = readStr.Trim();
                        Console.WriteLine("ReadStr adalah: " + readStr);

                        char[] delimiter_r = { '\r' };
                        string[] Measures_With_U = readStr.Split(delimiter_r);
                        List<string> Measure_Results = new List<string>();
                        List<string> AllText = new List<string>();

                        foreach (var Measure in Measures_With_U)
                        {
                            string Result_Parsing = GetWords(Measure).FirstOrDefault();
                            string[] charactersToReplace = new string[] { @"\t", @"\n", @"\r", " ", "<CR>", "<LF>" };
                            if (Result_Parsing != "" && Result_Parsing != null)
                            {
                                foreach (string s in charactersToReplace)
                                {
                                    Result_Parsing = Result_Parsing.Replace(s, "");
                                }
                            }

                            // Decide what to do with data
                            if (Result_Parsing != "" && Result_Parsing != null && !Result_Parsing.Trim().ToLower().Contains("r"))
                            {
                                aggregate_cond = true;
                                start_next_cond = true;
                                countersleep = 0;
                                start_next_init = 0;
                                Console.WriteLine("Nilai Measure adalah: " + Result_Parsing);
                                if (Result_Parsing.Contains("-") || (Result_Parsing.Length) > 4)
                                {
                                    AllText = GetWords(Measure);
                                    Result_Parsing = AllText[1].Substring(5, 3);
                                    Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                        , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));
                                    data_Average.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));
                                }
                                else
                                {
                                    Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                        , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));

                                    data_measure_2 data_final_update = new data_measure_2(counter_data + 1, Result_Parsing, (DateTime.Now).ToString());

                                    data_finals_update.Add(data_final_update);
                                    data_finals_update_2.Add(data_final_update);

                                    //var result = from db.MyTable.Where(d => (double)d.Price >= minValue && (double)d.Price <= maxValue)

                                    var data_Grid_1 = data_finals_update.Where(p => p.Id > 0 && p.Id <= 1 * NumberGrain_Frekuensi);
                                    data_Grid_1_list_a = data_Grid_1.ToList();
                                    data_Grid_1_list_b = data_Grid_1.ToList();

                                    var data_Grid_2 = data_finals_update.Where(p => p.Id > 1 * NumberGrain_Frekuensi && p.Id <= 2 * NumberGrain_Frekuensi);
                                    data_Grid_2_list_a = data_Grid_2.ToList();
                                    data_Grid_2_list_b = data_Grid_2.ToList();

                                    var data_Grid_3 = data_finals_update.Where(p => p.Id > 2 * NumberGrain_Frekuensi && p.Id <= 3 * NumberGrain_Frekuensi);
                                    data_Grid_3_list_a = data_Grid_3.ToList();
                                    data_Grid_3_list_b = data_Grid_3.ToList();

                                    var data_Grid_4 = data_finals_update.Where(p => p.Id > 3 * NumberGrain_Frekuensi && p.Id <= 4 * NumberGrain_Frekuensi);
                                    data_Grid_4_list_a = data_Grid_4.ToList();
                                    data_Grid_4_list_b = data_Grid_4.ToList();

                                    var data_Grid_5 = data_finals_update.Where(p => p.Id > 4 * NumberGrain_Frekuensi && p.Id <= 5 * NumberGrain_Frekuensi);
                                    data_Grid_5_list_a = data_Grid_5.ToList();
                                    data_Grid_5_list_b = data_Grid_5.ToList();

                                    //data_pdfhistories = (data_pdfhistories_var.OrderBy(p => p.Id)).ToList();

                                    //data_finals_ori.Add(data_final_ori);
                                    //Data_Receive_Grid.ItemsSource = data_finals_ori;
                                    counter_data = counter_data + 1;
                                }
                                Application.Current.Dispatcher.Invoke(new Action(() =>
                                {

                                    Data_Receive_Grid_1.ItemsSource = data_Grid_1_list_a;
                                    Data_Receive_Grid_1.ItemsSource = data_Grid_1_list_b;

                                    Data_Receive_Grid_2.ItemsSource = data_Grid_2_list_a;
                                    Data_Receive_Grid_2.ItemsSource = data_Grid_2_list_b;

                                    Data_Receive_Grid_3.ItemsSource = data_Grid_3_list_a;
                                    Data_Receive_Grid_3.ItemsSource = data_Grid_3_list_b;

                                    //var x = data_Average[0] != null ? 12 : (int?)null;
                                    //if (val % 2 == 1) { output = “Number is odd”; } else { output = “Number is even”; }
                                    if (data_Average.Count() == 1) { Average_1.Text = data_Average[0].Measures; } else {; }
                                    if (data_Average.Count() == 2) { Average_2.Text = data_Average[1].Measures; } else {; }
                                    if (data_Average.Count() == 3) { Average_3.Text = data_Average[2].Measures; } else {; }
                                    if (data_Average.Count() == 4) { Average_4.Text = data_Average[3].Measures; } else {; }

                                }));

                            }

                            else if (data_finals_update.Count % NumberGrain_Frekuensi == 0 && data_finals_update.Count > 0 
                                && aggregate_cond == true && start_next_cond == true)
                            {
                                Console.WriteLine("Start SendData with Data received");
                                
                                #region Get Aggregate value
                                
                                start_next_init = 0;
                                //OpenCon_Port_local(mySerialPort, BaudRate);
                                while (aggregate_cond)
                                {
                                    
                                    if (start_next_init % 8 == 0)
                                    {
                                        Console.WriteLine("Send button aggregate data");

                                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            ButtonAutomationPeer peer = new ButtonAutomationPeer(btn_MoistureAgg);
                                            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                            invokeProv.Invoke();
                                            //Sensor_input_Helper.Command_MoisturAggregate(mySerialPort);

                                        }));
                                    }
                                    Thread.Sleep(1000);// this solves the problem
                                    Console.WriteLine("Next Iter");
                                    Console.WriteLine("counter init is: ");
                                    readBuffer = new byte[mySerialPort.ReadBufferSize];
                                    readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                                    readStr = string.Empty;
                                    readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                                    readStr = readStr.Trim();

                                    Console.WriteLine("ReadStr adalah: " + readStr);
                                    charactersToReplace = new string[] { @"\t", @"\n", @"\r", " ", "<CR>", "<LF>","R"};
                                    foreach (string s in charactersToReplace)
                                    {
                                        Result_Parsing = readStr.Replace(s, "");
                                    }

                                    //Result_Parsing = GetWords(Result_Parsing).FirstOrDefault();
                                    if (Result_Parsing != null)
                                    {
                                        if (Result_Parsing.Contains("-") || (Result_Parsing.Length) > 4)
                                        {
                                            AllText = GetWords(Result_Parsing);
                                            Result_Parsing = AllText[1].Substring(5, 3);
                                            Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                                , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));
                                            data_Average.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));
                                            aggregate_cond = false;

                                            Application.Current.Dispatcher.Invoke(new Action(() =>
                                            {
                                                if (data_Average.Count() == 1) { Average_1.Text = data_Average[0].Measures; } else {; }
                                                if (data_Average.Count() == 2) { Average_2.Text = data_Average[1].Measures; } else {; }
                                                if (data_Average.Count() == 3) { Average_3.Text = data_Average[2].Measures; } else {; }
                                                if (data_Average.Count() == 4) { Average_4.Text = data_Average[3].Measures; } else {; }

                                            }));
                                            Console.WriteLine("Finish Aggregate");
                                        }
                                    }

                                    start_next_init++;

                                }

                                #endregion Finish get aggregate value
                                Console.WriteLine("Finish aggregate region");

                                #region check if measure finish

                                Console.WriteLine("data_average count adalah: ", data_Average.Count().ToString());
                                Console.WriteLine("data_average count adalah: ", current_interval.ToString());

                                if (data_Average.Count() == counter_interval && current_interval > 0 && counter_interval > 0)
                                {
                                    Console.WriteLine("Start ");
                                    stat_continue = false;
                                    mySerialPort.DiscardInBuffer();
                                    mySerialPort.DiscardOutBuffer();
                                    mySerialPort.Close();

                                    Application.Current.Dispatcher.Invoke(new Action(() =>
                                    {
                                        string Replace_Average = ",";
                                        string temp_average;
                                        float average = 0;
                                        foreach (data_measure_2 data in data_Average)
                                        {
                                            temp_average = data.Measures;
                                            temp_average = temp_average.Replace(Replace_Average, "");
                                            average = average + float.Parse(temp_average);
                                        }
                                        float final_average = average / 10 / data_Average.Count();

                                        string final_average_str = final_average.ToString();

                                        //final_average_str = String.Concat(final_average_str.Substring(0, final_average_str.Length - 1)
                                        //            , ".", final_average_str.Substring(final_average_str.Length - 1, 1));

                                        Average_Final.Text = final_average_str;
                                        MessageBox.Show("All Measurement Finish", application_name);

                                    }));
                                    countersleep = 1;
                                    start_next_cond = false;
                                    aggregate_cond = false;
                                    Console.WriteLine("break finish");
                                    break;

                                }

                                #endregion


                                #region Delay start

                                if (countersleep == 0)
                                {
                                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        ButtonAutomationPeer peer = new ButtonAutomationPeer(btn_Stop);
                                        IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                        invokeProv.Invoke();
                                        //Sensor_input_Helper.Command_MoisturAggregate(mySerialPort);

                                    }));
                                    Console.WriteLine("start delay", "start delay");
                                    //mySerialPort.Close();

                                    Thread.Sleep(delay);
                                    countersleep = 1;
                                    Console.WriteLine("Finish delay", "Finish delay");
                                }
                                #endregion

                                Console.WriteLine("Finish Delay region");

                                #region Start Next sequence

                                start_next_init = 0;
                                //mySerialPort.Close();
                                //OpenCon_Port_local(mySerialPort, BaudRate);
                                while (start_next_cond)
                                {
                                    //Thread.Sleep(1500);// this solves the problem
                                    Console.WriteLine("Next Iter");
                                    Console.WriteLine("counter init is: ");

                                    if (start_next_init % 4 == 0 && start_next_cond == true)
                                    {
                                        Console.WriteLine("Send button start data");

                                        Application.Current.Dispatcher.Invoke(new Action(() =>
                                        {
                                            ButtonAutomationPeer peer = new ButtonAutomationPeer(btn_Start_Next);
                                            IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                            invokeProv.Invoke();

                                        }));
                                    }
                                    //counter_interval++;
                                    current_interval++;
                                    start_next_cond = false;
                                    Console.WriteLine("Finish SendData with Data received");
                                    /*
                                    readBuffer = new byte[mySerialPort.ReadBufferSize];
                                    readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                                    readStr = string.Empty;
                                    readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                                    readStr = readStr.Trim();
                                    Console.WriteLine("ReadStr adalah: " + readStr);

                                    if (readStr == "" || readStr == null)
                                    {
                                        start_next_cond = false;
                                        Console.WriteLine("Finish SendData with Data received");
                                    }
                                    */
                                    /*
                                    Measures_With_U = readStr.Split(delimiter_r);
                                    Measure_Results = new List<string>();
                                    AllText = new List<string>();
                                    foreach (var Measure_temp in Measures_With_U)
                                    {
                                        Result_Parsing = GetWords(Measure_temp).FirstOrDefault();
                                        Result_Parsing = readStr;

                                        if (Result_Parsing != "" && Result_Parsing != null)
                                        {
                                            foreach (string s in charactersToReplace)
                                            {
                                                Result_Parsing = Result_Parsing.Replace(s, "");
                                            }
                                        }
                                        // Decide what to do with data
                                        if (Result_Parsing == "" || Result_Parsing == null)
                                        {
                                            start_next_cond = false;
                                            Console.WriteLine("Finish SendData with Data received");
                                        }
                                    }
                                    */
                                    start_next_init++;
                                    
                                }

                                #endregion
                                Console.WriteLine("Finish start next sequence region");

                            }
                            else
                            {
                                Console.WriteLine("Nilai Else adalah: " + Result_Parsing);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Console.WriteLine(ex);
                    //return "";
                }

            }
        }

        internal async Task SendDataWithAsync()
        {
            Console.WriteLine("Start SendData with ASync");
            OpenCon_Port_local(mySerialPort2, BaudRate);
            //Application.Current.Dispatcher.BeginInvoke(new Action(() => { /* Your code here */ }));

            //DispatcherPriority.inv
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, (ThreadStart)delegate
            {
                Sensor_input_Helper.Command_Check(mySerialPort2);
            });

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                
            }));
            await Task.Delay(1);
            Console.WriteLine("Finish SendData with ASync");
            /*
            while (StatusListen == true)
            {

                if (data_finals_update.Count % NumberGrain_Frekuensi == 0 && data_finals_update.Count > 0 && StatusListen == true)
                {
                    Console.WriteLine("data_finals_update.count: ", data_finals_update.Count().ToString()) ;
                    Console.WriteLine("NumberGrain_Frekuensi: ", NumberGrain_Frekuensi.ToString()) ;

                    mySerialPort.DataReceived -= new SerialDataReceivedEventHandler(ProcessSensorData_local);
                    Console.WriteLine("Start SendData with ASync");
                    await Task.Delay(5);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Sensor_input_Helper.Command_Check(mySerialPort);
                    }));
                    Console.WriteLine("Finish SendData with ASync");
                }
            }
            */
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("dspatch timer outside");
            //Sensor_input_Helper.Command_Check(mySerialPort);
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (StatusListen == false)
                {
                    Console.WriteLine("dspatch timer inside");
                    ButtonAutomationPeer peer = new ButtonAutomationPeer(btn_Start_Next);
                    IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProv.Invoke();
                }

                //Sensor_input_Helper.Command_Check(mySerialPort);
            }));
                        
            // code goes here
        }

        public void OpenCon_Port_local(SerialPort mySerialPort, int BaudRate)
        {
            //SerialPort SerialPort = new SerialPort(PortName);
            mySerialPort.BaudRate = BaudRate;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;
            mySerialPort.DtrEnable = true;
            mySerialPort.ReadBufferSize = 2000000;
            //mySerialPort.Encoding = ASCIIEncoding.ASCII;
            mySerialPort.Encoding = ASCIIEncoding.UTF8;

            //mySerialPort.DataReceived += new SerialDataReceivedEventHandler(ProcessSensorData_local);

            mySerialPort.Open();

            //Sensor_input_Helper.Command_Check(mySerialPort);
            Console.WriteLine("Open port LOcal Port");
            //Application.Run();

        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (StatusListen == false)
            {
                //Sensor_input_Helper.Command_Check(mySerialPort);
            }
            // other code needed
        }

        private List<string> GetWords(string text)
        {
            Regex reg = new Regex("[a-zA-Z0-9]");
            string Word = "";
            char[] ca = text.ToCharArray();
            List<string> characters = new List<string>();
            for (int i = 0; i < ca.Length; i++)
            {
                char c = ca[i];
                if (c > 65535)
                {
                    continue;
                }
                if (char.IsHighSurrogate(c))
                {
                    i++;
                    characters.Add(new string(new[] { c, ca[i] }));
                }
                else
                {
                    if (reg.Match(c.ToString()).Success || c.ToString() == "/")
                    {
                        Word = Word + c.ToString();
                        //characters.Add(new string(new[] { c }));
                    }
                    else if (c.ToString() == " ")
                    {
                        if (Word.Length > 0)
                            characters.Add(Word);
                        Word = "";
                    }
                    else
                    {
                        if (Word.Length > 0)
                            characters.Add(Word);
                        Word = "";
                    }

                }

            }
            return characters;
        }


        

        public void ProcessSensorData_local_temp(object sender, SerialDataReceivedEventArgs args)
        {
            Console.WriteLine("do nothing");
            byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
            int readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
            string readStr = string.Empty;

            readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
            //readStr = Encoding.UTF8.GetString(readBuffer,0,readLen);
            readStr = readStr.Trim();
            Console.WriteLine("ReadStr adalah: " + readStr);

            //mySerialPort.DataReceived -= new SerialDataReceivedEventHandler(ProcessSensorData_local_temp);
            //Thread.Sleep(10000);
            if (StatusListen == true)
            {
                Sensor_input_Helper.Command_Check(mySerialPort);

                StatusListen = false;
            }

        }
        public void ProcessSensorData_local(object sender, SerialDataReceivedEventArgs args)
        {

            try
            {


                if (data_Average.Count() == counter_interval && current_interval > 0 && counter_interval > 0)
                {

                    mySerialPort.DiscardInBuffer();
                    mySerialPort.DiscardOutBuffer();
                    mySerialPort.Close();

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        string Replace_Average = ",";
                        string temp_average;
                        float average = 0;
                        foreach (data_measure_2 data in data_Average)
                        {
                            temp_average = data.Measures;
                            temp_average = temp_average.Replace(Replace_Average, "");
                            average = average + float.Parse(temp_average);
                        }
                        float final_average = average / 10 / data_Average.Count();

                        string final_average_str = final_average.ToString();

                        //final_average_str = String.Concat(final_average_str.Substring(0, final_average_str.Length - 1)
                        //            , ".", final_average_str.Substring(final_average_str.Length - 1, 1));

                        Average_Final.Text = final_average_str;
                        MessageBox.Show("All Measurement Finish", application_name);

                    }));

                }
                
                else
                {
                    Thread.Sleep(1000);// this solves the problem
                    byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
                    int readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                    string readStr = string.Empty;
                    
                    readStr = Encoding.UTF8.GetString(readBuffer, 0, readLen);
                    readStr = readStr.Trim();
                    Console.WriteLine("ReadStr adalah: " + readStr);

                    char[] delimiter_r = { '\r' };
                    string[] Measures_With_U = readStr.Split(delimiter_r);

                    List<string> Measure_Results = new List<string>();

                    List<string> AllText = new List<string>();

                    foreach (var Measure in Measures_With_U)
                    {
                        string Result_Parsing = GetWords(Measure).FirstOrDefault();
                        string[] charactersToReplace = new string[] { @"\t", @"\n", @"\r", " ", "<CR>", "<LF>" };

                        // Parsing data
                        if (Result_Parsing != "" && Result_Parsing != null)
                        {
                            foreach (string s in charactersToReplace)
                            {
                                Result_Parsing = Result_Parsing.Replace(s, "");
                            }
                        }

                        // Decide what to do with data
                        if (Result_Parsing != "" && Result_Parsing != null && !Result_Parsing.Trim().ToLower().Contains("r"))
                        {
                            StatusListen = true;
                            StatusAggre = true;
                            countersleep = 0;
                            Console.WriteLine("Nilai Measure adalah: " + Result_Parsing);



                            if (Result_Parsing.Contains("-") || (Result_Parsing.Length) > 4)
                            {

                                AllText = GetWords(Measure);
                                //Result_Parsing = AllText[1];

                                Result_Parsing = AllText[1].Substring(5, 3);
                                Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                    , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));

                                //data_measure_2 data_final_update =

                                data_Average.Add(new data_measure_2(100, Result_Parsing, (DateTime.Now).ToString()));

                                //data_Average.Add(data_final_update);

                            }
                            else
                            {
                                Result_Parsing = String.Concat(Result_Parsing.Substring(0, Result_Parsing.Length - 1)
                                    , ".", Result_Parsing.Substring(Result_Parsing.Length - 1, 1));

                                data_measure_2 data_final_update = new data_measure_2(counter_data + 1, Result_Parsing, (DateTime.Now).ToString());

                                data_finals_update.Add(data_final_update);
                                data_finals_update_2.Add(data_final_update);

                                //var result = from db.MyTable.Where(d => (double)d.Price >= minValue && (double)d.Price <= maxValue)

                                var data_Grid_1 = data_finals_update.Where(p => p.Id > 0 && p.Id <= 1 * NumberGrain_Frekuensi);
                                data_Grid_1_list_a = data_Grid_1.ToList();
                                data_Grid_1_list_b = data_Grid_1.ToList();

                                var data_Grid_2 = data_finals_update.Where(p => p.Id > 1 * NumberGrain_Frekuensi && p.Id <= 2 * NumberGrain_Frekuensi);
                                data_Grid_2_list_a = data_Grid_2.ToList();
                                data_Grid_2_list_b = data_Grid_2.ToList();

                                var data_Grid_3 = data_finals_update.Where(p => p.Id > 2 * NumberGrain_Frekuensi && p.Id <= 3 * NumberGrain_Frekuensi);
                                data_Grid_3_list_a = data_Grid_3.ToList();
                                data_Grid_3_list_b = data_Grid_3.ToList();

                                var data_Grid_4 = data_finals_update.Where(p => p.Id > 3 * NumberGrain_Frekuensi && p.Id <= 4 * NumberGrain_Frekuensi);
                                data_Grid_4_list_a = data_Grid_4.ToList();
                                data_Grid_4_list_b = data_Grid_4.ToList();

                                var data_Grid_5 = data_finals_update.Where(p => p.Id > 4 * NumberGrain_Frekuensi && p.Id <= 5 * NumberGrain_Frekuensi);
                                data_Grid_5_list_a = data_Grid_5.ToList();
                                data_Grid_5_list_b = data_Grid_5.ToList();

                                //data_pdfhistories = (data_pdfhistories_var.OrderBy(p => p.Id)).ToList();

                                //data_finals_ori.Add(data_final_ori);
                                //Data_Receive_Grid.ItemsSource = data_finals_ori;
                                counter_data = counter_data + 1;
                            }
                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {

                                Data_Receive_Grid_1.ItemsSource = data_Grid_1_list_a;
                                Data_Receive_Grid_1.ItemsSource = data_Grid_1_list_b;

                                Data_Receive_Grid_2.ItemsSource = data_Grid_2_list_a;
                                Data_Receive_Grid_2.ItemsSource = data_Grid_2_list_b;

                                Data_Receive_Grid_3.ItemsSource = data_Grid_3_list_a;
                                Data_Receive_Grid_3.ItemsSource = data_Grid_3_list_b;

                                //var x = data_Average[0] != null ? 12 : (int?)null;
                                //if (val % 2 == 1) { output = “Number is odd”; } else { output = “Number is even”; }
                                if (data_Average.Count() == 1) { Average_1.Text = data_Average[0].Measures; } else {; }
                                if (data_Average.Count() == 2) { Average_2.Text = data_Average[1].Measures; } else {; }
                                if (data_Average.Count() == 3) { Average_3.Text = data_Average[2].Measures; } else {; }
                                if (data_Average.Count() == 4) { Average_4.Text = data_Average[3].Measures; } else {; }

                            }));

                        }

                        else if (data_finals_update.Count % NumberGrain_Frekuensi == 0 && data_finals_update.Count > 0 && StatusListen == true)
                        {
                            Console.WriteLine("Start SendData with Data received");
                            
                            if (countersleep == 0)
                            {
                                Thread.Sleep(5000);
                                countersleep = 1;
                                StatusListen = false;
                                mySerialPort.DataReceived -= new SerialDataReceivedEventHandler(ProcessSensorData_local);
                            }
                            /*
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, (ThreadStart)delegate
                            {
                                ButtonAutomationPeer peer = new ButtonAutomationPeer(btn_Start_Next);
                                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                                invokeProv.Invoke();
                            });
                            */
                            Console.WriteLine("Finish SendData with Data received");
                        }

                        /*
                        else if (data_finals_update.Count % NumberGrain_Frekuensi == 0 && data_finals_update.Count > 0 && StatusListen == true)
                        {
                        }
                        */

                        else
                        {
                            Console.WriteLine("Nilai Else adalah: " + Result_Parsing);
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Console.WriteLine(ex);
                //return "";
            }

        }

        private class YourCollection : ObservableCollection<MyObject>
        {
            // some wrapper functions for example:
            public void Add(string title)
            {
                this.Add(new MyObject { Title = title });
            }
        }
        private class YourCollection_data : ObservableCollection<MyObject>
        {
            // some wrapper functions for example:
            public void Add(string title)
            {
                this.Add(new MyObject { Title = title });
            }
        }

        private void btn_GridPrint_click(object sender, RoutedEventArgs e)
        {
            //Data_Receive_Grid.ItemsSource = data_finals_ori;

        }

        private void Button3_Baiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("start baiz waiting", application_name);
            TheEnclosingMethod();
            MessageBox.Show("Finsih baiz 3 waiting", application_name);
        }

        public async void TheEnclosingMethod()
        {
            //tbkLabel.Text = "thirty seconds delay";

            await Task.Delay(5000);
            Sensor_input_Helper.Command_CheckData(mySerialPort);

            await Task.Delay(5000);
            Sensor_input_Helper.Command_CheckData(mySerialPort);
            await Task.Delay(5000);
            Sensor_input_Helper.Command_CheckData(mySerialPort);
            await Task.Delay(5000);
            Sensor_input_Helper.Command_CheckData(mySerialPort);
            await Task.Delay(5000);
            await Task.Delay(5000);
            Sensor_input_Helper.Command_Check(mySerialPort);
            Sensor_input_Helper.Command_CheckData(mySerialPort);
            //var page = new Page2();
            //page.Show();
        }

        private void Button5_Baiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("start baiz waiting", application_name);

            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();


            if (timer_baiz_5 >= 10)
            {
                dispatcherTimer.Stop();
                Sensor_input_Helper.Command_Check(mySerialPort);
                Sensor_input_Helper.Command_CheckData(mySerialPort);
                MessageBox.Show("Finsih baiz 5 waiting", application_name);
            }

        }

       
        private void Button_Click_Start_Next(object sender, RoutedEventArgs e)
        {
            //Thread.Sleep(5000);
            Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
            Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
            //MessageBox.Show("Start Next Seguence","Start next");

        }
        

        internal async Task HeavyMethod()
        {

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Sensor_input_Helper.Command_Check(mySerialPort);
            }));
            await Task.Delay(1);

            //Sensor_input_Helper.Command_Check(mySerialPort);

        }

        private void Button_Click_Aggregate(object sender, RoutedEventArgs e)
        {
            Sensor_input_Helper.Command_MoisturAggregate(mySerialPort);
            //MessageBox.Show("Start Next Seguence", "Start next");


        }


        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            
            Thread.Sleep(1000);
            
        }

        private void Button_Click_30(object sender, RoutedEventArgs e)
        {

            Sensor_input_Helper.Command_Write(mySerialPort, ResultGrain);
        }

        private void Button_Click_10x3(object sender, RoutedEventArgs e)
        {

            Sensor_input_Helper.Command_Write(mySerialPort, ResultMeasure);
        }

    }
}
