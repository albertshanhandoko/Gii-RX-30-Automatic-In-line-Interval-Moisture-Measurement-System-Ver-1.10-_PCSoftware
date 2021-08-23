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
using Dashboard1.Constant;
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace Dashboard1.Helper
{
    class Sensor_input_Helper
    {

        public static void Command_Check(SerialPort mySerialPort)
        {
            //mySerialPort.Write("123");
            //public void Write (byte[] buffer, int offset, int count);
            string data = "00191\r";
            byte[] hexstring = Encoding.ASCII.GetBytes(data);

            foreach (byte hexval in hexstring)
            {
                byte[] _hexval = new byte[] { hexval };     // need to convert byte 
                                                            // to byte[] to write
                mySerialPort.Write(_hexval, 0, 1);
                Thread.Sleep(10);
            }
        }

        public static void Command_CheckData(SerialPort mySerialPort)
        {
            //mySerialPort.Write("123");
            //public void Write (byte[] buffer, int offset, int count);
            string data = "9119B\r";
            byte[] hexstring = Encoding.ASCII.GetBytes(data);
            foreach (byte hexval in hexstring)
            {
                byte[] _hexval = new byte[] { hexval };     // need to convert byte 
                                                            // to byte[] to write
                mySerialPort.Write(_hexval, 0, 1);
                Thread.Sleep(10);
            }
        }

        public static void Command_Stop(SerialPort mySerialPort)
        {
            //mySerialPort.Write("123");
            //public void Write (byte[] buffer, int offset, int count);
            string data = "\r";
            byte[] hexstring = Encoding.ASCII.GetBytes(data);
            foreach (byte hexval in hexstring)
            {
                byte[] _hexval = new byte[] { hexval };     // need to convert byte 
                                                            // to byte[] to write
                mySerialPort.Write(_hexval, 0, 1);
                //Thread.Sleep(10);
            }
        }
        public static void Command_MoistureMeasure(SerialPort mySerialPort, string input)
        {
            string data = input;
            byte[] hexstring = Encoding.ASCII.GetBytes(data);
            foreach (byte hexval in hexstring)
            {
                byte[] _hexval = new byte[] { hexval };     // need to convert byte 
                                                            // to byte[] to write
                mySerialPort.Write(_hexval, 0, 1);
                //Thread.Sleep(10);
            }
        }
        public static void Command_NumberofGrain(SerialPort mySerialPort, string input)
        {

            string data = input;
            byte[] hexstring = Encoding.ASCII.GetBytes(data);
            foreach (byte hexval in hexstring)
            {
                byte[] _hexval = new byte[] { hexval };     // need to convert byte 
                                                            // to byte[] to write
                mySerialPort.Write(_hexval, 0, 1);
                //Thread.Sleep(10);
            }
        }

        public static void Command_Write(SerialPort mySerialPort, string input)
        {
            string data = input;
            byte[] hexstring = Encoding.ASCII.GetBytes(data);
            foreach (byte hexval in hexstring)
            {
                byte[] _hexval = new byte[] { hexval };     // need to convert byte 
                                                            // to byte[] to write
                mySerialPort.Write(_hexval, 0, 1);
                //Thread.Sleep(10);
            }

        }

        public static void Command_MoisturAggregate(SerialPort mySerialPort)
        {
            //mySerialPort.Write("123");
            //public void Write (byte[] buffer, int offset, int count);
            string data = "9129C\r";
            byte[] hexstring = Encoding.ASCII.GetBytes(data);
            foreach (byte hexval in hexstring)
            {
                byte[] _hexval = new byte[] { hexval };     // need to convert byte 
                                                            // to byte[] to write
                mySerialPort.Write(_hexval, 0, 1);
                //Thread.Sleep(10);
            }
        }

        public static List<string> Get_Time_Interval()
        {
            var attributes = typeof(Time_Interval).GetMembers()
                .SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                .ToList();

            var result = attributes.Select(x => x.Description);
            List<string> asList = attributes.Select(x => x.Description).ToList();

            return asList;

        }

        public static List<string> Get_Number_Grain_List()
        {
            var attributes = typeof(number_grain).GetMembers()
                .SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                .ToList();

            var result = attributes.Select(x => x.Description);
            List<string> asList = attributes.Select(x => x.Description).ToList();

            return asList;

        }


        public static IEnumerable<string> Get_Time_Interval_enumera()
        {
            var attributes = typeof(Time_Interval).GetMembers()
                .SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>())
                .ToList();

            var result = attributes.Select(x => x.Description);
            List<string> asList = attributes.Select(x => x.Description).ToList();

            return result;

        }

        public static bool MySQL_ConnectDatabase_check(string connection_string_input)
        {
            bool check = false;
            MySqlConnection connection;
            string server = connection_string_input;
            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;

            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                //MessageBox.Show("successful connection");
                connection.Close();
                check = true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message + connectionString);
            }
            return check;
        }

        public static void MySQL_ConnectDatabase_test(string connection_string_input)
        {
            MySqlConnection connection;
            string server = connection_string_input;
            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "admin";
            string port = "3306";
            string sslM = "none";
            string connectionString;

            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                //MessageBox.Show("successful connection");
                connection.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message + connectionString);
            }
        }

        public static int MySql_Insert_Batch(string server, string product_varinput, int total_interval_varinput, string time_interval_varinput, int number_perinterval_varinput)
        {
            //MySqlConnection connection;
            //string server = "localhost";
            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;
            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);

            using (var connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("Insert_Batch", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("Product_var", product_varinput));
                command.Parameters.Add(new MySqlParameter("Total_Interval_var", total_interval_varinput));
                command.Parameters.Add(new MySqlParameter("Time_Interval_var", time_interval_varinput));
                command.Parameters.Add(new MySqlParameter("Number_Per_Interval_var", number_perinterval_varinput));

                // Add parameters
                command.Parameters.Add(new MySqlParameter("?Out_Result_Batch_ID", MySqlDbType.VarChar));
                command.Parameters["?Out_Result_Batch_ID"].Direction = ParameterDirection.Output;
                command.Connection.Open();
                var result = command.ExecuteNonQuery();
                int lastInsertID = Int32.Parse((string)command.Parameters["?Out_Result_Batch_ID"].Value);
                //int lastInsertID = Convert.ToInt32(cmd.Parameters["@fileid"].Value);
                command.Connection.Close();

                return lastInsertID;
            }

        }


        public static void MySql_Insert_PDF(string server, int Batch_ID_varinput, string SupplierName_varinput
            , string PrintedBy_varinput, DateTime Printed_Date_varinput, string File_Location_varinput)
        {
            //MySqlConnection connection;
            //string server = "localhost";
            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;
            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", server, port, user, password, database, sslM);

            using (var connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("Insert_PrintPDF", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("Batch_ID_var", Batch_ID_varinput));
                command.Parameters.Add(new MySqlParameter("IPAddress_var", server));
                command.Parameters.Add(new MySqlParameter("SupplierName_var", SupplierName_varinput));
                command.Parameters.Add(new MySqlParameter("PrintedBy_var", PrintedBy_varinput));
                command.Parameters.Add(new MySqlParameter("Printed_Date_var", Printed_Date_varinput));
                command.Parameters.Add(new MySqlParameter("File_Location_Var", File_Location_varinput));

                // Add parameters
                command.Connection.Open();
                var result = command.ExecuteNonQuery();
                //int lastInsertID = Convert.ToInt32(cmd.Parameters["@fileid"].Value);
                command.Connection.Close();

            }

        }



        public static Sql_Measure_Batch MySql_Get_Average(string IP_Address_varinput)
        {

            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;
            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", IP_Address_varinput, port, user, password, database, sslM);

            Sql_Measure_Batch query_batch = new Sql_Measure_Batch();

            List<Sql_Measure_Result> List_Average_Results = new List<Sql_Measure_Result> { };
            List<Sql_Measure_Result> List_Measure_Results = new List<Sql_Measure_Result> { };


            using (var connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("Get_Average", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("IP_Address_Var", IP_Address_varinput));
                command.Connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        query_batch.batch_measure_ID_cls = (Convert.ToInt32(reader["Batch_Measure_ID"]));
                        query_batch.ipaddress_cls = reader["IPADDRESS"].ToString();
                        query_batch.start_date_cls = DateTime.Now.ToString("dd/MM/yy") + "   " + DateTime.Now.ToString("HH:mm:ss");
                        query_batch.finish_date_cls = (Convert.ToDateTime(reader["Start_Date"]).ToString("dd/MM/yy HH:mm:ss"));
                        query_batch.product_cls = reader["Product"].ToString();
                        query_batch.total_interval_cls = (Convert.ToInt32(reader["Total_Interval"]));
                        query_batch.time_interval_cls = reader["Time_INterval"].ToString();
                        query_batch.number_per_interval_cls = (Convert.ToInt32(reader["Number_Per_Interval"]));
                        query_batch.temperature_cls = reader["temperature"].ToString();
                        query_batch.Error_code_cls = reader["Error_Code"].ToString();


                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            Sql_Measure_Result result_average_temp = new Sql_Measure_Result(1, 1, 1f, 1, DateTime.Now.ToString(), 1);
                            //result_average_temp.PerBatch_ID_cls = (Convert.ToInt32(reader["PerBatch_ID"])) - 100000;
                            result_average_temp.PerBatch_ID_cls = (Convert.ToInt32(reader["PerBatch_ID"])) - 1000;

                            result_average_temp.measure_result_cls = float.Parse(reader["measure_result"].ToString());
                            result_average_temp.No_Of_Peaces = (Convert.ToInt32(reader["jumlah_measure"]));
                            //result_average_temp.No_Of_Peaces = query_batch.List_Measure_Result.Count();
                            result_average_temp.created_on_cls = DateTime.Now.ToString("dd/MM/yy") + "   " + DateTime.Now.ToString("HH:mm:ss");
                            List_Average_Results.Add(result_average_temp);
                        }
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            Sql_Measure_Result result_measure_temp = new Sql_Measure_Result(1, 1, 1f, 1, DateTime.Now.ToString(), 1);
                            result_measure_temp.Batch_Id_cls = (Convert.ToInt32(reader["Batch_Id"]));
                            result_measure_temp.PerBatch_ID_cls = (Convert.ToInt32(reader["PerBatch_ID"]));
                            result_measure_temp.measure_result_cls = float.Parse(reader["measure_result"].ToString());
                            //result_measure_temp.created_on_cls = (Convert.ToDateTime(reader["created_on"]).ToString("yy/MM/dd HH:mm:ss"));
                            result_measure_temp.created_on_cls = DateTime.Now.ToString();

                            result_measure_temp.IsAverage_cls = (Convert.ToInt32(reader["IsAverage"]));
                            List_Measure_Results.Add(result_measure_temp);
                        }
                    }


                }

                // make the result into 10
                
                /*
                int jumlahaverage = 0;
                jumlahaverage = List_Average_Results.Count;
                while (jumlahaverage < 10)
                {
                    Sql_Measure_Result blank_grid_result = new Sql_Measure_Result(null, null, 0, null, null, null);
                    List_Average_Results.Add(blank_grid_result);
                    jumlahaverage = List_Average_Results.Count;
                }
                */

                query_batch.List_Average_Result = List_Average_Results;
                query_batch.List_Measure_Result = List_Measure_Results;

                command.Connection.Close();

                return query_batch;
            }

        }


        public static List<Sql_Excel_Measure> MySql_Get_ExcelLastMeasure(string IP_Address_varinput)
        {

            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;
            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", IP_Address_varinput, port, user, password, database, sslM);

            List<Sql_Excel_Measure> List_Excel_Results = new List<Sql_Excel_Measure> { };
            

            using (var connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("Get_ExcelMeasure", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("IP_Address_Var", IP_Address_varinput));
                command.Connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Sql_Excel_Measure Current_Excel_Result = new Sql_Excel_Measure { };
                        Current_Excel_Result.Batch_ID = Convert.ToInt32(reader["Batch_Measure_ID"]);
                        Current_Excel_Result.IP_Address = reader["IPADDRESS"].ToString();
                        Current_Excel_Result.Product = reader["Product"].ToString();
                        Current_Excel_Result.Total_Interval = (Convert.ToInt32(reader["Total_Interval"]));
                        Current_Excel_Result.Time_Interval = reader["Time_Interval"].ToString();
                        Current_Excel_Result.Number_Per_Interval = (Convert.ToInt32(reader["Number_Per_Interval"]));
                        Current_Excel_Result.Temperature = reader["temperature"].ToString();
                        Current_Excel_Result.Error_Code = reader["Error_Code"].ToString();
                        Current_Excel_Result.Current_Interval = (Convert.ToInt32(reader["Current_Interval"]));
                        Current_Excel_Result.Current_KernelCounter = (Convert.ToInt32(reader["Current_KernelCounter"]));
                        Current_Excel_Result.Measurement_Result = float.Parse(reader["Measurement_Result"].ToString());
                        Current_Excel_Result.Result_Type = reader["Result_Type"].ToString();

                        List_Excel_Results.Add(Current_Excel_Result);

                    }

                }

                command.Connection.Close();

                return List_Excel_Results;
            }

        }


        public static List<Data_PDFHistory> MySql_Get_PrintPDF(string IP_Address_varinput)
        {
            //MySqlConnection connection;
            //string server = "localhost";
            //string server = "192.168.0.4";
            //string server = "192.168.0.6";

            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;
            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", IP_Address_varinput, port, user, password, database, sslM);

            Sql_Measure_Batch query_batch = new Sql_Measure_Batch();

            List<Data_PDFHistory> List_PDF_Results = new List<Data_PDFHistory> { };


            using (var connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("Get_PrintPDF", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("IP_Address_Var", IP_Address_varinput));

                // Add parameters
                /*
                command.Parameters.Add(new MySqlParameter("?Out_Result_Batch_ID", MySqlDbType.VarChar));
                command.Parameters["?Out_Result_Batch_ID"].Direction = ParameterDirection.Output;
                */
                command.Connection.Open();



                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Data_PDFHistory result_PDF_Temp = new Data_PDFHistory(1, "");
                        result_PDF_Temp.Id = (Convert.ToInt32(reader["id"]));
                        result_PDF_Temp.Histories = reader["File_Location"].ToString();
                        List_PDF_Results.Add(result_PDF_Temp);


                    }

                }
                command.Connection.Close();

                var data_pdfhistories_var = List_PDF_Results.OrderByDescending(p => p.Id).Take(20);

                List_PDF_Results = (data_pdfhistories_var.OrderByDescending(p => p.Id)).ToList();

                return List_PDF_Results;
            }

        }

        public static List<SQL_Data_Config> MySql_Get_DataConfig(string IP_Address_varinput)
        {
            //MySqlConnection connection;
            //string server = "localhost";
            //string server = "192.168.0.4";
            //string server = "192.168.0.6";

            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;
            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", IP_Address_varinput, port, user, password, database, sslM);

            //Sql_Measure_Batch query_batch = new Sql_Measure_Batch();

            List<SQL_Data_Config> List_Data_Config = new List<SQL_Data_Config> { };


            using (var connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("Get_Parameter", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("Parameter_var", "0"));
                command.Connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        SQL_Data_Config data_Config = new SQL_Data_Config(1, "1", 1);
                        data_Config.Config_Id = (Convert.ToInt32(reader["id"]));
                        data_Config.Config_Param = reader["Parameter"].ToString();
                        data_Config.Config_Value = double.Parse(reader["value"].ToString());

                        List_Data_Config.Add(data_Config);
                    }

                }
                command.Connection.Close();


                return List_Data_Config;
            }

        }


        public static void Update_DataConfig(string IP_Address_varinput, string parameter, string value)
        {
            //MySqlConnection connection;
            //string server = "localhost";
            //string server = "192.168.0.4";
            //string server = "192.168.0.6";

            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;
            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", IP_Address_varinput, port, user, password, database, sslM);

            //Sql_Measure_Batch query_batch = new Sql_Measure_Batch();

            List<SQL_Data_Config> List_Data_Config = new List<SQL_Data_Config> { };


            using (var connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("Update_Parameter", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("Parameter_var", parameter));
                command.Parameters.Add(new MySqlParameter("Value_var", value));

                command.Connection.Open();
                command.ExecuteReader();

            }

        }

        public static string GetDescription(Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static bool is_batch_printed(string IP_Address_varinput, int batch_id)
        {

            string database = "sensor_database";
            string user = "root";
            //string password = "admin";
            string password = "raspberry";
            string port = "3306";
            string sslM = "none";
            string connectionString;
            connectionString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}", IP_Address_varinput, port, user, password, database, sslM);

            int check_result = 0;
            //List<int> List_PDF_BatchID = new List<int> { };
            using (var connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand("check_pdf", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("Batch_id_input", batch_id));
                command.Connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        check_result = (Convert.ToInt32(reader["check_result"]));

                    }

                }
                command.Connection.Close();
            }

            return Convert.ToBoolean(check_result);
        }
    }
}
