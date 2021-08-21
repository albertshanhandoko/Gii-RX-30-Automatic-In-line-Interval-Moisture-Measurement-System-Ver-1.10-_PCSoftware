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
using System.Windows.Shapes;
using System.Windows.Threading;
using Dashboard1.Helper;
using Dashboard1.Constant;
using Dashboard1.Library;
using System.Configuration;
using System.Media;
using System.Diagnostics;
using Spire;
using Spire.Xls;
using System.Data;

namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for Controller_View.xaml
    /// </summary>
    public partial class Controller_View : Window
    {
        Sql_Measure_Batch Current_Sensor_Batch = new Sql_Measure_Batch();

        Sql_Measure_Batch Sensor_Batch = new Sql_Measure_Batch();

        //string IP_Address_Input = "192.168.0."+ ((MainWindow)Application.Current.MainWindow).txtblock_sensor1.Text.Last();
        //string IP_Address_Input = "192.168.0.2"; //this is only for testing
        int? lastbatchid = 0;
        //string IP_Address_Input = "192.168.0.9";
        string test_textbox = ((MainWindow)Application.Current.MainWindow).TempObject_Textbox.Text;
        string IP_Address_Input = ((MainWindow)Application.Current.MainWindow).TempObject_Textbox.Text;
        static string Folder_Path = ConfigurationManager.AppSettings["Folder_Path"] ?? "Not Found"; //C:/Sensor_data/
        string PDF_folder_location = Folder_Path+ "Print_Result_Sensor" + ((MainWindow)Application.Current.MainWindow).TempObject_Textbox.Text.Last() + "/" ; 

        List<Sql_Measure_Result> List_Measure_Average = new List<Sql_Measure_Result> { };
        List<Sql_Measure_Result> List_Measure_Average_new = new List<Sql_Measure_Result> { };
        List<SQL_Measure_Grid> List_Measure_Average_grid = new List<SQL_Measure_Grid> { };
        List<SQL_Measure_Grid> List_Measure_Average_new_grid = new List<SQL_Measure_Grid> { };


        List<Data_PDFHistory> List_PDF_Histories = new List<Data_PDFHistory> { };
        List<SQL_Data_Config> List_Data_Configs = new List<SQL_Data_Config> { };


        static string application_name = ConfigurationManager.AppSettings["application_name"] ?? "Not Found";
        float total_average = 0;
        float final_average = 0;
        int counter_print = 0;

        // Thereshold measure
        int thereshold_counter = 0;
        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                using (_timeoutTimer)
                    MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }


        public Controller_View()
        {
            InitializeComponent();
            data_initialization();
            //MessageBox.Show(test_textbox, application_name);


            //  DispatcherTimer setup
            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();

        }
        private void data_initialization()
        {
            try
            {
                textbox_SensorTitle.Text = "SENSOR " + ((MainWindow)Application.Current.MainWindow).TempObject_Textbox.Text.Last();
                Current_Sensor_Batch = Sensor_input_Helper.MySql_Get_Average(IP_Address_Input);
                lastbatchid = Current_Sensor_Batch.batch_measure_ID_cls;
                txt_date.Text = Current_Sensor_Batch.start_date_cls;
                //txt_date.Text = Sensor_Batch.start_date_cls;
                txt_application.Text = Current_Sensor_Batch.product_cls;
                txt_TotInterval.Text = Current_Sensor_Batch.total_interval_cls.ToString();
                txt_TotPCS.Text = (Current_Sensor_Batch.total_interval_cls * Current_Sensor_Batch.number_per_interval_cls).ToString();
                txt_Temperature.Text = Current_Sensor_Batch.temperature_cls;

                txt_date.IsEnabled = false;
                txt_application.IsEnabled = false;
                txt_TotInterval.IsEnabled = false;
                txt_TotPCS.IsEnabled = false;
                txt_Temperature.IsEnabled = false;

            }
            catch (Exception error)//(Exception e)
            {
                MessageBox.Show(error.ToString(), application_name);
                Console.WriteLine(error.Message);
            }

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            // This will tricker every second

            //Sensor_Batch = Sensor_input_Helper.MySql_Get_Measure(IP_Address_Input);
            try
            {
                Sensor_Batch = Sensor_input_Helper.MySql_Get_Average(IP_Address_Input);
                
                List_PDF_Histories = Sensor_input_Helper.MySql_Get_PrintPDF(IP_Address_Input);
                List_Data_Configs = Sensor_input_Helper.MySql_Get_DataConfig(IP_Address_Input);

                var thereshold_Max_var = List_Data_Configs.Find(item => item.Config_Param == "Thereshold_Max").Config_Value;
                var thereshold_Min_var = List_Data_Configs.Find(item => item.Config_Param == "Thereshold_Min").Config_Value;

                double thereshold_Max = double.Parse(thereshold_Max_var.ToString());
                double thereshold_Min = double.Parse(thereshold_Min_var.ToString());
                double number_of_thereshold_max = Sensor_Batch.List_Measure_Result.Count(item => item.measure_result_cls > thereshold_Max);
                double number_of_thereshold_min = Sensor_Batch.List_Measure_Result.Count(item => item.measure_result_cls < thereshold_Min);

                string Error_Message_Title = string.Empty;
                string Error_Message_Detail = string.Empty;
                string Error_Message_Fixing = string.Empty;

                if (Sensor_Batch.Error_code_cls != "" && Sensor_Batch.Error_code_cls != string.Empty && Sensor_Batch.Error_code_cls != null)
                {
                    Error_Message_Title = "Err " + Sensor_Batch.Error_code_cls;

                    Error_Sensor_PC_Detail enum_ErrorCode_Detail = (Error_Sensor_PC_Detail)Enum.Parse(typeof(Error_Sensor_PC_Detail), "error" + Sensor_Batch.Error_code_cls);
                    Error_Message_Detail = Sensor_input_Helper.GetDescription(enum_ErrorCode_Detail);

                    Error_Sensor_PC_Fixing enum_ErrorCode_Fixing = (Error_Sensor_PC_Fixing)Enum.Parse(typeof(Error_Sensor_PC_Fixing), "error" + Sensor_Batch.Error_code_cls);
                    Error_Message_Fixing = Sensor_input_Helper.GetDescription(enum_ErrorCode_Fixing);
                }

                //MessageBox.Show(this, Error_Message, application_name);
                //
                // 1. check if batch is new
                // 2. if batch is not new , compare count. if new get all data
                // 3. if there is new average data, add ONLY the new one to the old one.
                if (lastbatchid == Sensor_Batch.batch_measure_ID_cls) // 1
                {
                    int current_countaverage = List_Measure_Average.Count;
                    int last_countaverage = Sensor_Batch.List_Average_Result.Count;
                    int selisih = last_countaverage - current_countaverage;
                    for (int i = selisih; i > 0; i--) // 2
                    {
                        List_Measure_Average.Add(Sensor_Batch.List_Average_Result[Sensor_Batch.List_Average_Result.Count - i]); // 3
                    }
                }
                // klo ganti batch ngapain
                else
                {
                    //txt_date.Text = DateTime.Now.ToString();
                    txt_date.Text = Sensor_Batch.start_date_cls;
                    txt_application.Text = Sensor_Batch.product_cls;
                    txt_TotInterval.Text = Sensor_Batch.total_interval_cls.ToString();
                    txt_TotPCS.Text = (Sensor_Batch.total_interval_cls * Sensor_Batch.number_per_interval_cls).ToString();
                    txt_Temperature.Text = Sensor_Batch.temperature_cls;

                    List_Measure_Average = Sensor_Batch.List_Average_Result;
                    lastbatchid = Sensor_Batch.batch_measure_ID_cls;

                }
                Sensor_Batch.start_date_cls = txt_date.Text;
                total_average = 0;
                foreach (Sql_Measure_Result Measure_Average in List_Measure_Average)
                {
                    total_average = total_average + Measure_Average.measure_result_cls;
                }

                final_average = total_average / List_Measure_Average.Count();

                List_Measure_Average_grid.Clear();
                List_Measure_Average_new_grid.Clear();


                // Change SQL Measure Result to grid
                foreach (Sql_Measure_Result result in List_Measure_Average)
                {
                    SQL_Measure_Grid measgrid = new SQL_Measure_Grid(null,null,null,null,null,null);
                    measgrid.Batch_Id_cls = result.Batch_Id_cls.ToString();
                    measgrid.PerBatch_ID_cls = result.PerBatch_ID_cls.ToString();
                    measgrid.measure_result_cls = result.measure_result_cls.ToString();
                    measgrid.created_on_cls = result.created_on_cls;
                    measgrid.IsAverage_cls = result.IsAverage_cls.ToString();
                    measgrid.No_Of_Peaces = result.No_Of_Peaces.ToString();
                    List_Measure_Average_grid.Add(measgrid);

                }

                foreach (Sql_Measure_Result resultnew in List_Measure_Average_new)
                {
                    SQL_Measure_Grid measgrid_new = new SQL_Measure_Grid(null, null, null, null, null, null);
                    measgrid_new.Batch_Id_cls = resultnew.Batch_Id_cls.ToString();
                    measgrid_new.PerBatch_ID_cls = resultnew.PerBatch_ID_cls.ToString();
                    measgrid_new.measure_result_cls = resultnew.measure_result_cls.ToString();
                    measgrid_new.created_on_cls = resultnew.created_on_cls;
                    measgrid_new.IsAverage_cls = resultnew.IsAverage_cls.ToString();
                    measgrid_new.No_Of_Peaces = resultnew.No_Of_Peaces.ToString();
                    List_Measure_Average_new_grid.Add(measgrid_new);

                }
                // add data until 10

                int jumlah_record_grid = 0;
                jumlah_record_grid = List_Measure_Average_grid.Count;
                while (jumlah_record_grid <= 10)
                {
                    List_Measure_Average_grid.Add(new SQL_Measure_Grid(null, null, null, null, null, null));
                    jumlah_record_grid = List_Measure_Average_grid.Count;
                }


                int jumlah_record_grid_new = 0;
                jumlah_record_grid_new = List_Measure_Average_new_grid.Count;
                while (jumlah_record_grid_new <= 10)
                {
                    List_Measure_Average_new_grid.Add(new SQL_Measure_Grid(null, null, null, null, null, null));
                    jumlah_record_grid_new = List_Measure_Average_new_grid.Count;
                }


                Application.Current.Dispatcher.Invoke(new Action(() => {

                    // Average Grid
                    /*
                    DataContext = List_Measure_Average_new;
                    DataContext = List_Measure_Average;
                    Average_Grid.ItemsSource = List_Measure_Average_new; ;
                    Average_Grid.ItemsSource = List_Measure_Average;
                    */
                    DataContext = List_Measure_Average_new_grid;
                    DataContext = List_Measure_Average_grid;
                    Average_Grid.ItemsSource = List_Measure_Average_new_grid;
                    Average_Grid.ItemsSource = List_Measure_Average_grid;


                    // Final average
                    txt_FinalAverage.Text = final_average.ToString("0.00");

                    // PDF
                    HistoryGrid1.ItemsSource = List_PDF_Histories;

                    HistoryGrid1.Columns[0].Header = "No.";
                    HistoryGrid1.Columns[1].Header = "File Name";

                    // Thereshold
                    TheresholdMax_TextBox.Text = thereshold_Max_var.ToString() + "%";
                    TheresholdMin_TextBox.Text = thereshold_Min_var.ToString() + "%";

                    NumOf_TheresholdMax_TextBox.Text = number_of_thereshold_max.ToString();
                    NumOf_TheresholdMin_TextBox.Text = number_of_thereshold_min.ToString();

                    // Error
                    //Error_TextBox.Text = Error_Message;
                    Error_TextBox_Title.Text = Error_Message_Title;
                    Error_TextBox_Detail.Text = Error_Message_Detail;
                    Error_TextBox_Fixing.Text = Error_Message_Fixing;
                    
                }));

                //bool check_need_to_print = false;
                //check_need_to_print = Sensor_input_Helper.is_batch_printed(IP_Address_Input, Sensor_Batch.batch_measure_ID_cls);
                bool check_need_to_print = Sensor_input_Helper.is_batch_printed(IP_Address_Input, Sensor_Batch.batch_measure_ID_cls);


                if (check_need_to_print == true)
                {
                    int ExpectedPieces = Sensor_Batch.number_per_interval_cls * Sensor_Batch.total_interval_cls;
                    int ActualPieces = Sensor_Batch.List_Measure_Result.Count();
                    bool isPremature = false;
                    if (ActualPieces < ExpectedPieces)
                    {
                        isPremature = true;
                    //    MessageBox.Show("Measurement was stopped prematurely", application_name);
                    }

                    string company_name = SensorHelper_2.read_config_name();
                    string company_addres = SensorHelper_2.read_config_addr();
                    Sensor_Batch.List_Average_Result = List_Measure_Average;
                    SensorHelper_2.Generate_Controller_PDF_revised_5Aug2021(company_name, company_addres, txt_supplier.Text
                        , txt_PrintedBy.Text, Sensor_Batch, 1, isPremature);


                    if (ActualPieces < ExpectedPieces)
                    {
                        //MessageBox.Show("Measurement was stopped prematurely", application_name);
                        AutoClosingMessageBox.Show("Measurement was stopped prematurely", application_name, 3000);

                    }

                }
                thereshold_counter = Sensor_Batch.List_Measure_Result.Count;
            }

            catch (Exception error)//(Exception e)
            {
                //MessageBox.Show(error.ToString(), application_name);
                Console.WriteLine(error.Message);
            }


        }

        private void Generate_PDF_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(test_textbox, application_name);

            if (String.IsNullOrEmpty(txt_supplier.Text))
            {
                MessageBox.Show("Please enter Supplier Name", application_name);
            }

            //Printed By
            else if (String.IsNullOrEmpty(txt_PrintedBy.Text))
            {
                MessageBox.Show("Please enter Printed By", application_name);
            }
            else if (List_Measure_Average.Count() != Sensor_Batch.total_interval_cls)
            {
                MessageBox.Show("Measurement Not Finish", application_name);
            }
            else
            {
                try
                {
                    Sensor_Batch = Sensor_input_Helper.MySql_Get_Average(IP_Address_Input);
                    string company_name = SensorHelper_2.read_config_name();
                    string company_addres = SensorHelper_2.read_config_addr();
                    bool isPremature = false;
                    int ExpectedPieces = Sensor_Batch.number_per_interval_cls * Sensor_Batch.total_interval_cls;
                    int ActualPieces = Sensor_Batch.List_Measure_Result.Count();
                    if (ActualPieces < ExpectedPieces)
                    {
                        isPremature = true;
                        //MessageBox.Show("Measurement was stopped prematurely", application_name);
                    }
                    //SensorHelper_2.Generate_Controller_PDF_revised(company_name,company_addres,txt_supplier.Text,txt_PrintedBy.Text,Sensor_Batch,1);
                    SensorHelper_2.Generate_Controller_PDF_revised_5Aug2021(company_name, company_addres, txt_supplier.Text, txt_PrintedBy.Text, Sensor_Batch, 1, isPremature);


                    MessageBox.Show("PDF has been successfully generated", application_name);

                }
                catch (Exception error)//(Exception e)
                {
                    MessageBox.Show(error.ToString(), application_name);
                    Console.WriteLine(error.Message);
                }
            }

        }

        private void txt_supplier_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txt_supplier_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void Error_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Folder_Path);

        }

        private void HistoryGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedclick = HistoryGrid1.SelectedItems.ToString();
            Data_PDFHistory PDFHistory = (Data_PDFHistory)HistoryGrid1.SelectedItem;
            //D:\Sensor_Data\Print_Result_Sensor1

            //string urlpdf = "D:/Sensor_data/Print_Result_Sensor1/" + PDFHistory.Histories;
            string urlpdf = PDF_folder_location + PDFHistory.Histories;

            Console.WriteLine(urlpdf);
            Process.Start("sumatrapdf.exe");
            System.Diagnostics.Process.Start(urlpdf);
            //Process.Start("sumatrapdf.exe", urlpdf);
            //Process.Start("sumatrapdf.exe", "C:\\Sensor_data\\Print_Result_Sensor5\\_Sensor 5_20210815_1208hr");


        }

        private void btn_Excel_Download(object sender, RoutedEventArgs e)
        {
            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            //convert the datagrid source to datatable
            //DataTable table = ((DataView)this.dataGrid1.ItemsSource).Table;
            List<Sql_Excel_Measure> result_excel = Sensor_input_Helper.MySql_Get_ExcelLastMeasure(IP_Address_Input);


            IEnumerable<Sql_Excel_Measure> data = result_excel;
            DataTable table = new DataTable();
            table = SensorHelper_2.ToDataTable(result_excel);
            sheet.InsertDataTable(table, true, 1, 1);
            string ExcelFile_Name = "Sensor " + IP_Address_Input.Last() + "_"+ DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + ".xlsx";
            string ExcelURL = PDF_folder_location + ExcelFile_Name;
            //book.SaveToFile("exportDataGridToExcel.xlsx", ExcelVersion.Version2013);
            book.SaveToFile(ExcelURL, ExcelVersion.Version2013);

        }

        private void Average_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
