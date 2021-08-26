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
using Spire.Xls;
using Spire.Xls.Charts;

using Color = System.Drawing.Color;
using Spire.Pdf;
using Size = System.Drawing.Size;
using System.Drawing.Drawing2D;
using System.Data;
using System.Reflection;

namespace Dashboard1.Helper
{
    class SensorHelper_2
    {
        static string Folder_Path = ConfigurationManager.AppSettings["Folder_Path"] ?? "Not Found";
        static string application_name = ConfigurationManager.AppSettings["application_name"] ?? "Not Found";
        static string RadioButtonDefault = ConfigurationManager.AppSettings["RadioBtn_Default"] ?? "Not Found";

        public static string read_config_name()
        {
            string url_name_config = Folder_Path + "/DataConfig/config_name.txt";
            string name_text = "";
            if (File.Exists(url_name_config))
            {
                name_text = File.ReadLines(url_name_config).First();
            }
            return name_text;
        }

        public static string read_config_addr()
        {
            string url_addr_config = Folder_Path + "/DataConfig/config_addr.txt";
            string addr_text = "";
            if (File.Exists(url_addr_config))
            {
                addr_text = File.ReadLines(url_addr_config).First();
            }
            return addr_text;
        }


        public static void writeTextFile(string FileLocation, string textfile)
        {
            //string urlHistory_data = "D:/Sensor_data/History_data_Sensor1/" + month.ToString().Trim() + ".txt";
            string url_config = FileLocation;
            if (File.Exists(url_config))
            {
                File.WriteAllText(url_config, String.Empty);
                //File.Delete(url_config);
                // write to file
                File.WriteAllText(url_config, textfile);
            }
            else
            {
                // Create new file
                System.IO.File.WriteAllText(url_config, textfile);
            }
        }

        public static String GetDestinationPath(string filename, string foldername)
        {
            String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
            return appStartPath;
        }

        public static int test_int(int test)
        {
            if (test > 0)
            {
                return test + 10;
            }
            else
            {
                return -10;
            }
        }

        public static void Generate_Initial_Folder()
        {
            string pathmain = Folder_Path;
            try
            {
                if (Directory.Exists(pathmain))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }
                DirectoryInfo di = Directory.CreateDirectory(pathmain);
                string pathconfig = Folder_Path + "DataConfig";
                if (Directory.Exists(pathconfig))
                {
                    Console.WriteLine("That path exists already.");
                    //return;
                }
                else
                {
                    DirectoryInfo dir_pathresult = Directory.CreateDirectory(pathconfig);
                }


                for (int j = 1; j <= 10; j++)
                {
                    //Print_Result_Sensor1
                    //string pathresult = @"D:\Sensor_Data\Print_Result_Sensor" + j.ToString();
                    string pathresult = Folder_Path + "Print_Result_Sensor" + j.ToString();

                    if (Directory.Exists(pathresult))
                    {
                        Console.WriteLine("That path exists already.");
                        //return;
                    }
                    else
                    {
                        DirectoryInfo dir_pathresult = Directory.CreateDirectory(pathresult);
                    }
                    /*
                    string pathhistory = Folder_Path + "History_data_Sensor" + j.ToString();

                    if (Directory.Exists(pathhistory))
                    {
                        Console.WriteLine("That path exists already.");
                    }
                    else
                    {
                        DirectoryInfo dir_pathhistory = Directory.CreateDirectory(pathhistory);
                    }
                    */

                }
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(pathmain));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
            finally { }
        }

        public static List<Data_PDFHistory> Read_PDF_History(string sensorname)
        {
            string month = DateTime.Now.ToString("yyyyMM");
            //string urlHistory_data = "D:/Sensor_data/History_data_Sensor1/" + month.ToString().Trim() + ".txt";
            string urlHistory_data = Folder_Path + "History_data_" + sensorname + "/" + month.ToString().Trim() + ".txt";
            List<Data_PDFHistory> data_pdfhistories = new List<Data_PDFHistory> { };

            if (File.Exists(urlHistory_data))
            {
                string[] lines = File.ReadAllLines(urlHistory_data);
                int i = 1;
                foreach (string line in lines)
                {
                    if (line.Length > 3)
                    {
                        Data_PDFHistory data_pdfhistory = new Data_PDFHistory(i, line.Substring(36));
                        data_pdfhistories.Add(data_pdfhistory);
                        i++;
                    }
                    else
                    {
                        Console.WriteLine("Data will not be printed");
                    }
                }

            }
            //
            //data_pdfhistories = (from p in data_pdfhistories orderby p.Id descending select p).Take(5);
            //List<Data_PDFHistory> data_pdfhistories_result = new List<Data_PDFHistory> { };
            var data_pdfhistories_var = data_pdfhistories.OrderByDescending(p => p.Id).Take(50);

            data_pdfhistories = (data_pdfhistories_var.OrderBy(p => p.Id)).ToList();

            return data_pdfhistories;
        }

        public static List<Data_History> Read_PDF_History_old(string SensorName)
        {
            string month = DateTime.Now.ToString("yyyyMM");
            string urlHistory_data = "D:/Sensor_data/History_data/" + SensorName + month.ToString().Trim() + ".txt";
            List<Data_History> data_histories = new List<Data_History> { };

            if (File.Exists(urlHistory_data))
            {
                string[] lines = File.ReadAllLines(urlHistory_data);
                int i = 1;
                foreach (string line in lines)
                {
                    Data_History data_history = new Data_History(i, "", "", DateTime.Now, "Sensor2_1Jan2020_T130000.pdf");
                    data_histories.Add(data_history);
                    i++;
                }
            }
            return data_histories;
        }

        public static List<Data_Measure> Test_DataMeasure()
        {
            List<Data_Measure> data_Measures = new List<Data_Measure>
            {
                //new Data_Measure {Id = 1, Measures = 241, Created_date = DateTime.Now},
                new Data_Measure (1, "241", DateTime.Now),
                new Data_Measure (2, "242", DateTime.Now),
                new Data_Measure (3, "243", DateTime.Now),
                new Data_Measure (4, "244", DateTime.Now),
                new Data_Measure (5, "245", DateTime.Now),
                new Data_Measure (6, "246", DateTime.Now),
                new Data_Measure (7, "247", DateTime.Now),
                new Data_Measure (8, "248", DateTime.Now),
                new Data_Measure (9, "249", DateTime.Now),
                //new Data_Measure (0, "245", DateTime.Now)
            };
            return data_Measures;
        }

        public static List<data_measure_2> Test_DataMeasure_2()
        {
            List<data_measure_2> data_Measures = new List<data_measure_2>
            {
                //new Data_Measure {Id = 1, Measures = 241, Created_date = DateTime.Now},
                new data_measure_2 (1, "241", (DateTime.Now).ToString()),
                new data_measure_2 (2, "242", (DateTime.Now).ToString()),
                new data_measure_2 (3, "243", (DateTime.Now).ToString()),
                new data_measure_2 (4, "244", (DateTime.Now).ToString()),
                new data_measure_2 (5, "245", (DateTime.Now).ToString()),
                new data_measure_2 (6, "246", (DateTime.Now).ToString()),
                new data_measure_2 (7, "247", (DateTime.Now).ToString()),
                new data_measure_2 (8, "248", (DateTime.Now).ToString()),
                new data_measure_2 (9, "249", (DateTime.Now).ToString()),
                //new Data_Measure (0, "245", DateTime.Now)
            };
            return data_Measures;
        }

        public static List<Sql_Measure_Result> Test_ListSQLMeasureResult()
        {
            List<Sql_Measure_Result> data_Measures = new List<Sql_Measure_Result>
            {
                //new Data_Measure {Id = 1, Measures = 241, Created_date = DateTime.Now},
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),

                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),

                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),

                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),

                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),
                new Sql_Measure_Result (1,1,11,12, DateTime.Now.ToString("yy/MM/dd") + "   " + DateTime.Now.ToString("HH:mm:ss"), 1),


                //new Data_Measure (0, "245", DateTime.Now)
            };
            return data_Measures;
        }




        public static void writeText(PdfContentByte cb, string Text, int X, int Y, BaseFont font, int Size)
        {
            cb.SetFontAndSize(font, Size);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Text, X, Y, 0);
            //return 1;
        }

        public static void writeTextCenter(PdfContentByte cb, string Text, int X, int Y, BaseFont font, int Size)
        {
            cb.SetFontAndSize(font, Size);
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Text, X, Y, 0);
            //return 1;
        }

        public static void Pick_PDF()
        {

        }



        public static void Generate_Simple_PDF_English(string label, string sensorname, string printdate, string printtime, string average
        , string numberofmeasure, string printedby, List<Data_Measure> datas)
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            //string UrlPDF = Folder_Path + "Print_Result_Sensor1/" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";
            //string UrlPDF = Folder_Path + "Print_Result_Sensor1/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";
            string UrlPDF = Folder_Path + "Print_Result_" + sensorname + "/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";

            FileStream fs = new FileStream(UrlPDF, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document(PageSize.A4, 10, 10, 30, 30);// left,right,top, bottom
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //This equates to 595 x 841 in points and 794 x 1122 in pixels.

            BaseFont Calibri = BaseFont.CreateFont("c:\\windows\\fonts\\Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont Calibri_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED);
            BaseFont Arial = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.TTF", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont Arial_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\arialbd.ttf", "Identity-H", BaseFont.EMBEDDED);

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                float WidthOri = png.ScaledWidth;
                float HeightOri = png.ScaledHeight;
                float WidthFinal = 90;
                float HeightFinal = 90;

                if (WidthOri >= HeightOri)
                {
                    WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }
                png.ScaleAbsolute(WidthFinal, HeightFinal);
                png.SetAbsolutePosition(40, 740);// x,y
                cb.AddImage(png);
            }

            #region page 1

            string conf_companyname = read_config_name();
            string conf_companyaddr = read_config_addr();

            // Draw A line for doc title
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 710);
            cb.LineTo(560, 710);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);
            //cb.SaveState();
            cb.BeginText();
            int left_margin = 300;
            //int top_margin = 800;

            int top_margin = 790;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 20);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 30, Calibri, 14);

            cb.EndText();

            // Document Header ---------------------------
            string Header_Title1 = "Incoming Grain Analysis Report";
            string Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

            left_margin = 40;
            top_margin = 650;

            int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();


            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin, Calibri_Bold, 16);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 30, Calibri_Bold, 16);

            // Label
            writeText(cb, "Supplier Name: ", left_margin, top_margin - 70, Calibri, 12);
            writeText(cb, label, left_margin + 90, top_margin - 70, Calibri, 12);

            // Date
            writeText(cb, "Printed Date: ", left_margin, top_margin - 85, Calibri, 12);
            writeText(cb, printdate, left_margin + 90, top_margin - 85, Calibri, 12);

            // Time
            writeText(cb, "Printed Time: ", left_margin, top_margin - 100, Calibri, 12);
            writeText(cb, printtime, left_margin + 90, top_margin - 100, Calibri, 12);

            // Average
            writeText(cb, "Average: ", left_margin + 315, top_margin - 70, Calibri, 12);
            writeText(cb, average, left_margin + 430, top_margin - 70, Calibri, 12);

            // Number of Measurement
            writeText(cb, "No. of Measurements: ", left_margin + 315, top_margin - 85, Calibri, 12);
            writeText(cb, numberofmeasure, left_margin + 430, top_margin - 85, Calibri, 12);

            // Printed By
            writeText(cb, "Printed By: ", left_margin + 315, top_margin - 100, Calibri, 12);
            writeText(cb, printedby, left_margin + 430, top_margin - 100, Calibri, 12);

            cb.EndText();


            // Document Body ---------------------------

            string moisture_content = "Moisture Content (%)";

            // Column Name

            top_margin = 505;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 18);
            writeText(cb, "ID", left_margin, top_margin - 30, Arial_Bold, 14);
            writeText(cb, "Description", left_margin + 70, top_margin - 30, Arial_Bold, 14);
            writeText(cb, "Created Time", left_margin + 200, top_margin - 30, Arial_Bold, 14);
            cb.EndText();

            // Draw A line for header data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 470);
            cb.LineTo(560, 470);
            cb.Stroke();


            // Measure_Data
            cb.BeginText();

            left_margin = 40;
            top_margin = 450;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 12);
                writeText(cb, data.Measures, left_margin + 70, top_margin, Calibri, 12);
                writeText(cb, data.Created_date.ToString(), left_margin + 200, top_margin, Calibri, 12);
                top_margin -= 20;
            }
            // 465 - 20*10 = 265

            top_margin = 260;
            left_margin = 40;
            string empty_kernels = "Empty Kernels (%)";
            string Grain_Yield = "Grain Yield (%)";
            string Varieties = "Varieties";

            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 14);
            writeText(cb, Grain_Yield, left_margin + 240, top_margin, Calibri_Bold, 14);
            writeText(cb, Varieties, left_margin, top_margin - 45, Calibri_Bold, 14);

            cb.EndText();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5, top_margin - 25, 120, 18);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5 + 240, top_margin - 25, 120, 18);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5, top_margin - 45 - 25, 120, 18);
            cb.Stroke();
            // Document Footer ---------------------------
            top_margin = 150;

            string qcpersonel = "QC Personnel";
            string approvedby = "Approved By: ";

            string label_name = "Name";
            string label_signature = "Signature";

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 12);
            writeText(cb, approvedby, left_margin, top_margin - 30, Calibri, 12);
            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 12);
            writeText(cb, label_signature, left_margin + 380, top_margin - 100, Calibri, 12);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 75);
            cb.LineTo(100, 75);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(420, 75);
            cb.LineTo(480, 75);
            cb.Stroke();
            //cb.RestoreState();
            #endregion Page 1

            // save to text file History ------------------------------------
            string month = DateTime.Now.ToString("yyyyMM");
            string urlHistory_data = Folder_Path + "History_data_" + sensorname + "/"
                + month.ToString().Trim() + ".txt";

            if (File.Exists(urlHistory_data))
            {
                // write to file
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(urlHistory_data, true))
                {
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(UrlPDF);
                }
            }
            else
            {
                // Create new file
                System.IO.File.WriteAllText(urlHistory_data, UrlPDF);
            }

            document.Close();
            writer.Close();
            fs.Close();

        }

        public static void Generate_Simple_PDF_Bahasa(string label, string sensorname, string printdate, string printtime, string average
        , string numberofmeasure, string printedby, List<Data_Measure> datas)
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            //string UrlPDF = Folder_Path + "Print_Result_Sensor1/" + trimmedlabel + "_" +sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";
            string UrlPDF = Folder_Path + "Print_Result_" + sensorname + "/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";
            FileStream fs = new FileStream(UrlPDF, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document(PageSize.A4, 10, 10, 30, 30);// left,right,top, bottom
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //This equates to 595 x 841 in points and 794 x 1122 in pixels.

            BaseFont Calibri = BaseFont.CreateFont("c:\\windows\\fonts\\Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont Calibri_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED);
            BaseFont Arial = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.TTF", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont Arial_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\arialbd.ttf", "Identity-H", BaseFont.EMBEDDED);

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            //string source_dir = "D:/Job/Bebeb/SensorReader/WpfApp1/Resources/Logo_Chua.jpg";
            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                float WidthOri = png.ScaledWidth;
                float HeightOri = png.ScaledHeight;
                float WidthFinal = 90;
                float HeightFinal = 90;

                if (WidthOri >= HeightOri)
                {
                    WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }
                png.ScaleAbsolute(WidthFinal, HeightFinal);
                png.SetAbsolutePosition(40, 740);// x,y
                cb.AddImage(png);
            }

            // Page Indo
            //document.NewPage();
            #region Page2




            //cb.SetLineDash()
            // Draw A line for doc title
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 710);
            cb.LineTo(560, 710);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);
            //cb.SaveState();
            cb.BeginText();

            int left_margin = 300;
            //int top_margin = 800;
            int top_margin = 790;

            string conf_companyname = read_config_name();
            string conf_companyaddr = read_config_addr();

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 20);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 30, Calibri, 14);

            /*
            writeText(cb, sales, left_margin, top_margin - 45, Calibri, 12);

            writeText(cb, GSTReg_No, left_margin, top_margin - 63, Calibri, 12);
            writeText(cb, UEN_No, left_margin + 100, top_margin - 63, Calibri, 12);

            writeText(cb, telepon, left_margin, top_margin - 81, Calibri, 12);
            writeText(cb, Fax_no, left_margin + 100, top_margin - 81, Calibri, 12);
            */

            cb.EndText();
            //cb.RestoreState();
            // Document Header ---------------------------
            string Header_Title1 = "Dari Analisa Biji Padi Kering Panen yang Masuk";
            string Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

            left_margin = 40;
            top_margin = 650;
            int left_margin_title = 300;

            cb.SetColorFill(BaseColor.BLACK);
            //cb.SetLineDash(5);
            //cb.SaveState();
            cb.BeginText();

            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin, Calibri_Bold, 16);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 30, Calibri_Bold, 16);

            // Label
            writeText(cb, "Nama Supplier: ", left_margin, top_margin - 70, Calibri, 12);
            writeText(cb, label, left_margin + 90, top_margin - 70, Calibri, 12);

            // Date
            writeText(cb, "Tanggal Print: ", left_margin, top_margin - 85, Calibri, 12);
            writeText(cb, printdate, left_margin + 90, top_margin - 85, Calibri, 12);

            // Time
            writeText(cb, "Waktu Print: ", left_margin, top_margin - 100, Calibri, 12);
            writeText(cb, printtime, left_margin + 90, top_margin - 100, Calibri, 12);

            // Average
            writeText(cb, "Rata-Rata: ", left_margin + 325, top_margin - 70, Calibri, 12);
            writeText(cb, average, left_margin + 440, top_margin - 70, Calibri, 12);

            // Number of Measurement
            writeText(cb, "Jumlah Pengukuran: ", left_margin + 325, top_margin - 85, Calibri, 12);
            writeText(cb, numberofmeasure, left_margin + 440, top_margin - 85, Calibri, 12);

            // Printed By
            writeText(cb, "Print Oleh: ", left_margin + 325, top_margin - 100, Calibri, 12);
            writeText(cb, printedby, left_margin + 440, top_margin - 100, Calibri, 12);

            cb.EndText();
            // Document Body ---------------------------

            string moisture_content = "Kadar Air (%)";

            // Column Name

            top_margin = 505;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 18);
            writeText(cb, "ID", left_margin, top_margin - 30, Arial_Bold, 14);
            writeText(cb, "Description", left_margin + 70, top_margin - 30, Arial_Bold, 14);
            writeText(cb, "Created Time", left_margin + 200, top_margin - 30, Arial_Bold, 14);
            cb.EndText();

            // Draw A line for header data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 470);
            cb.LineTo(560, 470);
            cb.Stroke();

            // Measure_Data
            cb.BeginText();

            left_margin = 40;
            top_margin = 450;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 12);
                writeText(cb, data.Measures, left_margin + 70, top_margin, Calibri, 12);
                writeText(cb, data.Created_date.ToString(), left_margin + 200, top_margin, Calibri, 12);
                top_margin -= 20;
            }
            // 465 - 20*10 = 265

            top_margin = 260;
            left_margin = 40;
            string empty_kernels = "Butir Hampa (%)";
            string Grain_Yield = "Hasil Biji (%)";
            string Varieties = "Varieties";

            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 14);
            writeText(cb, Grain_Yield, left_margin + 240, top_margin, Calibri_Bold, 14);
            writeText(cb, Varieties, left_margin, top_margin - 45, Calibri_Bold, 14);

            cb.EndText();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5, top_margin - 25, 120, 18);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5 + 240, top_margin - 25, 120, 18);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5, top_margin - 45 - 25, 120, 18);
            cb.Stroke();

            // Document Footer ---------------------------
            top_margin = 150;

            string qcpersonel = "QC Pemeriksa";
            string approvedby = "Disetujui Oleh: ";

            string label_name = "Nama";
            string label_signature = "Tanda Tangan";


            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 12);
            writeText(cb, approvedby, left_margin, top_margin - 30, Calibri, 12);


            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 12);
            writeText(cb, label_signature, left_margin + 380, top_margin - 100, Calibri, 12);

            cb.EndText();

            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(left_margin, top_margin - 100 - 5);
            cb.LineTo(left_margin + 80, top_margin - 100 - 5);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(left_margin + 380, top_margin - 100 - 5);
            cb.LineTo(left_margin + 380 + 80, top_margin - 100 - 5);
            cb.Stroke();
            #endregion Page2 end


            // save to text file History ------------------------------------
            string month = DateTime.Now.ToString("yyyyMM");
            //string urlHistory_data = "D:/Sensor_data/History_data_Sensor1/" + month.ToString().Trim() + ".txt";
            string urlHistory_data = Folder_Path + "History_data_" + sensorname + "/"
                + month.ToString().Trim() + ".txt";

            if (File.Exists(urlHistory_data))
            {
                // write to file
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(urlHistory_data, true))
                {
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(UrlPDF);
                }
            }
            else
            {
                // Create new file
                System.IO.File.WriteAllText(urlHistory_data, UrlPDF);
            }

            document.Close();
            writer.Close();
            fs.Close();

        }


        public static void Generate_Simple_PDF_English_A5(string label, string sensorname, string printdate, string printtime, string average
       , string numberofmeasure, string printedby, List<Data_Measure> datas)
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            string UrlPDF = Folder_Path + "Print_Result_" + sensorname + "/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";

            FileStream fs = new FileStream(UrlPDF, FileMode.Create, FileAccess.Write, FileShare.None);

            Document document = new Document(PageSize.A4, 10, 10, 30, 30);// left,right,top, botto,
            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            //A4 - which is 8.27 inches x 11.69 in, 210 x 297mm
            //This equates to 595 x 841 in points and 794 x 1122 in pixels. => A4

            //A5 - which is 5-7/8 x 8-1/4, 148 x 210 
            //A5 Wiki equates to 420 x 594 in points and 559 x 793 in pixels. => A5 

            //A5 chua which is 210 * 130
            //A5 chua equates to  595 x 368  in points  

            BaseFont Calibri = BaseFont.CreateFont("c:\\windows\\fonts\\Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont Calibri_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED);
            BaseFont Arial = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.TTF", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont Arial_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\arialbd.ttf", "Identity-H", BaseFont.EMBEDDED);

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                float WidthOri = png.ScaledWidth;
                float HeightOri = png.ScaledHeight;
                float WidthFinal = 50;
                float HeightFinal = 50;

                if (WidthOri >= HeightOri)
                {
                    //WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    //HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }
                png.ScaleAbsolute(WidthFinal, HeightFinal);
                png.SetAbsolutePosition(40, 305 + 10);// x,y
                cb.AddImage(png);

                png.SetAbsolutePosition(40, 305 + 445);// x,y
                //png.SetAbsolutePosition(40, 740);// x,y
                cb.AddImage(png);
            }

            // Label English

            // label Title
            string supplier_name_label = "Supplier Name: ";
            string Printed_Date_label = "Supplier Name: ";
            string Printed_Time_label = "Supplier Name: ";
            string Average_label = "Supplier Name: ";
            string Measurement_label = "Supplier Name: ";
            string Printed_By_label = "Supplier Name: ";

            string conf_companyname = read_config_name();
            string conf_companyaddr = read_config_addr();

            string Header_Title1 = "Incoming Grain Analysis Report";
            string Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

            string moisture_content = "Moisture Content (%)";
            string empty_kernels = "Empty Kernels (%)";
            string Grain_Yield = "Grain Yield (%)";
            string Varieties = "Varieties";

            string qcpersonel = "QC Personnel";
            string approvedby = "Approved By: ";

            string label_name = "Name";
            string label_signature = "Signature";

            #region A4 Part bawah
            // Draw A line for doc title

            cb.SetLineWidth(0f);
            cb.MoveTo(40, 300 + 10);
            cb.LineTo(560, 300 + 10);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);

            //cb.SaveState();
            cb.BeginText();
            int left_margin = 296;
            //int top_margin = 800;
            int left_margin_title = 300;
            int top_margin = 330 + 10;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 14);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 20, Calibri, 11);

            cb.EndText();

            // Document Header ---------------------------

            left_margin = 40;
            top_margin = 345 - 50 + 10;

            // Rectangle for Supplier Name
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Supplier Name Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Date
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle forPrinted Date Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Printed Time
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Time Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Average
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Average Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Measurement
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Measurement Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            //int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();

            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin - 18, Calibri_Bold, 14);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 38, Calibri_Bold, 14);

            // Label

            writeText(cb, supplier_name_label, left_margin, top_margin - 70, Calibri, 11);
            writeText(cb, label, left_margin + 120, top_margin - 70, Calibri, 11);

            // Date
            writeText(cb, Printed_Date_label, left_margin, top_margin - 85, Calibri, 11);
            writeText(cb, printdate, left_margin + 120, top_margin - 85, Calibri, 11);

            // Time
            writeText(cb, Printed_Time_label, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, printtime, left_margin + 120, top_margin - 100, Calibri, 11);

            // Average
            writeText(cb, Average_label, left_margin, top_margin - 115, Calibri, 11);
            writeText(cb, average, left_margin + 120, top_margin - 115, Calibri, 11);

            // Number of Measurement
            writeText(cb, Measurement_label, left_margin, top_margin - 130, Calibri, 11);
            writeText(cb, numberofmeasure, left_margin + 120, top_margin - 130, Calibri, 11);

            // Printed By
            writeText(cb, Printed_By_label, left_margin, top_margin - 145, Calibri, 11);
            writeText(cb, printedby, left_margin + 120, top_margin - 145, Calibri, 11);

            cb.EndText();


            // Document Body ---------------------------



            // Column Name

            left_margin = 300;
            top_margin = 345 - 70 - 50 + 10;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 10);
            writeText(cb, "ID", left_margin, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Description", left_margin + 40, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Created Time", left_margin + 120, top_margin - 15, Arial_Bold, 10);
            cb.EndText();

            // Draw A line for header data_measure

            cb.SetLineWidth(0f);
            cb.MoveTo(295, top_margin - 18);
            cb.LineTo(550, top_margin - 18);
            cb.Stroke();


            // Measure_Data
            cb.BeginText();

            left_margin = 300;
            top_margin = 240 - 50 + 10;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 10);
                writeText(cb, data.Measures, left_margin + 40, top_margin, Calibri, 10);
                writeText(cb, data.Created_date.ToString(), left_margin + 120, top_margin, Calibri, 10);
                top_margin -= 12;
            }
            // 465 - 20*10 = 265

            top_margin = 120 - 50 + 10;
            left_margin = 300;

            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, Grain_Yield, left_margin + 140, top_margin, Calibri_Bold, 11);
            writeText(cb, Varieties, left_margin, top_margin - 35, Calibri_Bold, 11);

            cb.EndText();
            //cb.MoveTo(40, 215);
            // buat rectangle
            //var rect_kernel = new iTextSharp.text.Rectangle(30, 190, 120, 200);
            //rect_kernel.BorderWidth = 2;
            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 20, 100, 15);//xstart ,ystart ,width,height
            //cb.Rectangle(,,,,)
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295 + 140, top_margin - 20, 100, 15);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 55, 100, 15);
            cb.Stroke();

            // Document Footer ---------------------------


            top_margin = 110 + 10;
            left_margin = 40;

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, approvedby, left_margin, top_margin - 10, Calibri, 11);
            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, label_signature, left_margin + 140, top_margin - 100, Calibri, 11);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 30);
            cb.LineTo(120, 30);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(180, 30);
            cb.LineTo(260, 30);
            cb.Stroke();
            //cb.RestoreState();


            #endregion A4 Part 1


            #region A4 Part atas


            // Draw A line for doc title

            cb.SetLineWidth(0f);
            cb.MoveTo(40, 300 + 445);
            cb.LineTo(560, 300 + 445);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);

            //cb.SaveState();
            cb.BeginText();
            left_margin = 296;
            //int top_margin = 800;

            top_margin = 330 + 445;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 14);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 20, Calibri, 11);


            cb.EndText();

            // Document Header ---------------------------
            //Header_Title1 = "Incoming Grain Analysis Report";
            //Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

            left_margin = 40;
            top_margin = 345 + 445 - 50;

            // Rectangle for Supplier Name
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Supplier Name Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Date
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle forPrinted Date Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Printed Time
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Time Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Average
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Average Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Measurement
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Measurement Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();



            //int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();


            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin - 18, Calibri_Bold, 14);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 38, Calibri_Bold, 14);

            // Label

            writeText(cb, "Supplier Name: ", left_margin, top_margin - 70, Calibri, 11);
            writeText(cb, label, left_margin + 120, top_margin - 70, Calibri, 11);

            // Date
            writeText(cb, "Printed Date: ", left_margin, top_margin - 85, Calibri, 11);
            writeText(cb, printdate, left_margin + 120, top_margin - 85, Calibri, 11);

            // Time
            writeText(cb, "Printed Time: ", left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, printtime, left_margin + 120, top_margin - 100, Calibri, 11);

            // Average
            writeText(cb, "Average: ", left_margin, top_margin - 115, Calibri, 11);
            writeText(cb, average, left_margin + 120, top_margin - 115, Calibri, 11);

            // Number of Measurement
            writeText(cb, "No. of Measurements: ", left_margin, top_margin - 130, Calibri, 11);
            writeText(cb, numberofmeasure, left_margin + 120, top_margin - 130, Calibri, 11);

            // Printed By
            writeText(cb, "Printed By: ", left_margin, top_margin - 145, Calibri, 11);
            writeText(cb, printedby, left_margin + 120, top_margin - 145, Calibri, 11);

            cb.EndText();


            // Document Body ---------------------------

            //string moisture_content = "Moisture Content (%)";

            // Column Name

            left_margin = 300;
            top_margin = 345 - 70 + 445 - 50;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 10);
            writeText(cb, "ID", left_margin, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Description", left_margin + 40, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Created Time", left_margin + 120, top_margin - 15, Arial_Bold, 10);
            cb.EndText();

            // Draw A line for header data_measure

            cb.SetLineWidth(0f);
            cb.MoveTo(295, top_margin - 18);
            cb.LineTo(550, top_margin - 18);
            cb.Stroke();



            // Measure_Data
            cb.BeginText();

            left_margin = 300;
            top_margin = 240 + 445 - 50;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 10);
                writeText(cb, data.Measures, left_margin + 40, top_margin, Calibri, 10);
                writeText(cb, data.Created_date.ToString(), left_margin + 120, top_margin, Calibri, 10);
                top_margin -= 12;
            }
            // 465 - 20*10 = 265

            top_margin = 120 + 445 - 50;
            left_margin = 300;
            //string empty_kernels = "Empty Kernels (%)";
            //string Grain_Yield = "Grain Yield (%)";
            //string Varieties = "Varieties";
            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, Grain_Yield, left_margin + 140, top_margin, Calibri_Bold, 11);
            writeText(cb, Varieties, left_margin, top_margin - 35, Calibri_Bold, 11);

            cb.EndText();
            //cb.MoveTo(40, 215);
            // buat rectangle
            //var rect_kernel = new iTextSharp.text.Rectangle(30, 190, 120, 200);
            //rect_kernel.BorderWidth = 2;
            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 20, 100, 15);//xstart ,ystart ,width,height
            //cb.Rectangle(,,,,)
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295 + 140, top_margin - 20, 100, 15);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 55, 100, 15);
            cb.Stroke();

            // Document Footer ---------------------------


            top_margin = 110 + 445;
            left_margin = 40;
            //string qcpersonel = "QC Personnel";
            //string approvedby = "Approved By: ";

            //string label_name = "Name";
            //string label_signature = "Signature";

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, approvedby, left_margin, top_margin - 10, Calibri, 11);
            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, label_signature, left_margin + 140, top_margin - 100, Calibri, 11);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 30 + 445 - 10);
            cb.LineTo(120, 30 + 445 - 10);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(180, 30 + 445 - 10);
            cb.LineTo(260, 30 + 445 - 10);
            cb.Stroke();
            //cb.RestoreState();


            #endregion A4 Part 2


            // save to text file History ------------------------------------
            string month = DateTime.Now.ToString("yyyyMM");
            string urlHistory_data = Folder_Path + "History_data_" + sensorname + "/"
                + month.ToString().Trim() + ".txt";

            if (File.Exists(urlHistory_data))
            {
                // write to file
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(urlHistory_data, true))
                {
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(UrlPDF);
                }
            }
            else
            {
                // Create new file
                System.IO.File.WriteAllText(urlHistory_data, UrlPDF);
            }

            document.Close();
            writer.Close();
            fs.Close();

        }

        public static void Generate_Simple_PDF_Bahasa_A5(string label, string sensorname, string printdate, string printtime, string average
       , string numberofmeasure, string printedby, List<Data_Measure> datas)
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            //string UrlPDF = Folder_Path + "Print_Result_Sensor1/" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";
            //string UrlPDF = Folder_Path + "Print_Result_Sensor1/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";
            string UrlPDF = Folder_Path + "Print_Result_" + sensorname + "/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";

            FileStream fs = new FileStream(UrlPDF, FileMode.Create, FileAccess.Write, FileShare.None);


            //iTextSharp.text.Rectangle PageSize_var = new iTextSharp.text.Rectangle(595, 368);

            Document document = new Document(PageSize.A4, 10, 10, 30, 30);// left,right,top, bottom
            //Document document = new Document(PageSize_var, 10, 10, 30, 30);// left,right,top, bottom

            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            //A4 - which is 8.27 inches x 11.69 in, 210 x 297mm
            //This equates to 595 x 841 in points and 794 x 1122 in pixels. => A4

            //A5 - which is 5-7/8 x 8-1/4, 148 x 210 
            //A5 Wiki equates to 420 x 594 in points and 559 x 793 in pixels. => A5 

            //A5 chua which is 210 * 130
            //A5 chua equates to  595 x 368  in points  

            //A5 to A4. 368  + 50 + 368

            BaseFont Calibri = BaseFont.CreateFont("c:\\windows\\fonts\\Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont Calibri_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED);
            BaseFont Arial = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.TTF", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont Arial_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\arialbd.ttf", "Identity-H", BaseFont.EMBEDDED);

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                float WidthOri = png.ScaledWidth;
                float HeightOri = png.ScaledHeight;
                float WidthFinal = 50;
                float HeightFinal = 50;

                if (WidthOri >= HeightOri)
                {
                    //WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    //HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }
                png.ScaleAbsolute(WidthFinal, HeightFinal);
                png.SetAbsolutePosition(40, 305);// x,y
                cb.AddImage(png);

                png.SetAbsolutePosition(40, 305 + 435);// x,y
                //png.SetAbsolutePosition(40, 740);// x,y
                cb.AddImage(png);
            }


            #region A4 Part 1

            string conf_companyname = read_config_name();
            string conf_companyaddr = read_config_addr();

            // Draw A line for doc title

            cb.SetLineWidth(0f);
            cb.MoveTo(40, 300);
            cb.LineTo(560, 300);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);

            //cb.SaveState();
            cb.BeginText();
            int left_margin = 296;
            //int top_margin = 800;

            int top_margin = 330;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 14);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 20, Calibri, 11);


            cb.EndText();

            // Document Header ---------------------------
            //string Header_Title1 = "Incoming Grain Analysis Report";
            //string Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

            left_margin = 40;
            top_margin = 345;

            // Rectangle for Supplier Name
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Supplier Name Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Date
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle forPrinted Date Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Printed Time
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Time Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Average
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Average Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Measurement
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Measurement Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();



            //int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();


            //writeTextCenter(cb, Header_Title1, left_margin_title, top_margin, Calibri_Bold, 16);
            //writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 30, Calibri_Bold, 16);

            // Label

            writeText(cb, "Nama Supplier: ", left_margin, top_margin - 70, Calibri, 11);
            writeText(cb, label, left_margin + 120, top_margin - 70, Calibri, 11);

            // Date
            writeText(cb, "Tanggal Print: ", left_margin, top_margin - 85, Calibri, 11);
            writeText(cb, printdate, left_margin + 120, top_margin - 85, Calibri, 11);

            // Time
            writeText(cb, "Waktu Print: ", left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, printtime, left_margin + 120, top_margin - 100, Calibri, 11);

            // Average
            writeText(cb, "Rata-Rata: ", left_margin, top_margin - 115, Calibri, 11);
            writeText(cb, average, left_margin + 120, top_margin - 115, Calibri, 11);

            // Number of Measurement
            writeText(cb, "Jumlah Pengukuran: ", left_margin, top_margin - 130, Calibri, 11);
            writeText(cb, numberofmeasure, left_margin + 120, top_margin - 130, Calibri, 11);

            // Printed By
            writeText(cb, "Print Oleh: ", left_margin, top_margin - 145, Calibri, 11);
            writeText(cb, printedby, left_margin + 120, top_margin - 145, Calibri, 11);

            cb.EndText();


            // Document Body ---------------------------

            string moisture_content = "Kadar Air (%)";

            // Column Name

            left_margin = 300;
            top_margin = 345 - 70;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 10);
            writeText(cb, "ID", left_margin, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Description", left_margin + 40, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Created Time", left_margin + 120, top_margin - 15, Arial_Bold, 10);
            cb.EndText();

            // Draw A line for header data_measure

            cb.SetLineWidth(0f);
            cb.MoveTo(295, 255);
            cb.LineTo(550, 255);
            cb.Stroke();


            // Measure_Data
            cb.BeginText();

            left_margin = 300;
            top_margin = 240;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 10);
                writeText(cb, data.Measures, left_margin + 40, top_margin, Calibri, 10);
                writeText(cb, data.Created_date.ToString(), left_margin + 120, top_margin, Calibri, 10);
                top_margin -= 12;
            }
            // 465 - 20*10 = 265

            top_margin = 120;
            left_margin = 300;
            string empty_kernels = "Butir Hampa (%)";
            string Grain_Yield = "Hasil Biji (%)";
            string Varieties = "Varieties";
            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, Grain_Yield, left_margin + 140, top_margin, Calibri_Bold, 11);
            writeText(cb, Varieties, left_margin, top_margin - 35, Calibri_Bold, 11);

            cb.EndText();
            //cb.MoveTo(40, 215);
            // buat rectangle
            //var rect_kernel = new iTextSharp.text.Rectangle(30, 190, 120, 200);
            //rect_kernel.BorderWidth = 2;
            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 20, 100, 15);//xstart ,ystart ,width,height
            //cb.Rectangle(,,,,)
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295 + 140, top_margin - 20, 100, 15);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 55, 100, 15);
            cb.Stroke();

            // Document Footer ---------------------------


            top_margin = 110;
            left_margin = 40;
            string qcpersonel = "QC Pemeriksa";
            string approvedby = "Disetujui Oleh: ";

            string label_name = "Nama";
            string label_signature = "Tanda Tangan";

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, approvedby, left_margin, top_margin - 10, Calibri, 11);
            writeText(cb, label_name, left_margin, top_margin - 90, Calibri, 11);
            writeText(cb, label_signature, left_margin + 140, top_margin - 90, Calibri, 11);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 30);
            cb.LineTo(120, 30);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(180, 30);
            cb.LineTo(260, 30);
            cb.Stroke();
            //cb.RestoreState();


            #endregion A4 Part 1


            #region A4 Part 2


            // Draw A line for doc title

            cb.SetLineWidth(0f);
            cb.MoveTo(40, 300 + 435);
            cb.LineTo(560, 300 + 435);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);

            //cb.SaveState();
            cb.BeginText();
            left_margin = 296;
            //int top_margin = 800;

            top_margin = 330 + 435;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 14);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 20, Calibri, 11);


            cb.EndText();

            // Document Header ---------------------------
            //string Header_Title1 = "Incoming Grain Analysis Report";
            //string Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

            left_margin = 40;
            top_margin = 345 + 435;

            // Rectangle for Supplier Name
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Supplier Name Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Date
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle forPrinted Date Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Printed Time
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Time Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Average
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Average Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Measurement
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Measurement Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();



            //int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();


            //writeTextCenter(cb, Header_Title1, left_margin_title, top_margin, Calibri_Bold, 16);
            //writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 30, Calibri_Bold, 16);

            // Label

            writeText(cb, "Nama Supplier: ", left_margin, top_margin - 70, Calibri, 11);
            writeText(cb, label, left_margin + 120, top_margin - 70, Calibri, 11);

            // Date
            writeText(cb, "Tanggal Print: ", left_margin, top_margin - 85, Calibri, 11);
            writeText(cb, printdate, left_margin + 120, top_margin - 85, Calibri, 11);

            // Time
            writeText(cb, "Waktu Print: ", left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, printtime, left_margin + 120, top_margin - 100, Calibri, 11);

            // Average
            writeText(cb, "Rata-Rata: ", left_margin, top_margin - 115, Calibri, 11);
            writeText(cb, average, left_margin + 120, top_margin - 115, Calibri, 11);

            // Number of Measurement
            writeText(cb, "Jumlah Pengukuran: ", left_margin, top_margin - 130, Calibri, 11);
            writeText(cb, numberofmeasure, left_margin + 120, top_margin - 130, Calibri, 11);

            // Printed By
            writeText(cb, "Print Oleh: ", left_margin, top_margin - 145, Calibri, 11);
            writeText(cb, printedby, left_margin + 120, top_margin - 145, Calibri, 11);

            cb.EndText();


            // Document Body ---------------------------

            //string moisture_content = "Moisture Content (%)";

            // Column Name

            left_margin = 300;
            top_margin = 345 - 70 + 435;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 10);
            writeText(cb, "ID", left_margin, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Description", left_margin + 40, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Created Time", left_margin + 120, top_margin - 15, Arial_Bold, 10);
            cb.EndText();

            // Draw A line for header data_measure

            cb.SetLineWidth(0f);
            cb.MoveTo(295, 255);
            cb.LineTo(550, 255);
            cb.Stroke();


            // Measure_Data
            cb.BeginText();

            left_margin = 300;
            top_margin = 240 + 435;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 10);
                writeText(cb, data.Measures, left_margin + 40, top_margin, Calibri, 10);
                writeText(cb, data.Created_date.ToString(), left_margin + 120, top_margin, Calibri, 10);
                top_margin -= 12;
            }
            // 465 - 20*10 = 265

            top_margin = 120 + 435;
            left_margin = 300;
            //string empty_kernels = "Empty Kernels (%)";
            //string Grain_Yield = "Grain Yield (%)";
            //string Varieties = "Varieties";
            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, Grain_Yield, left_margin + 140, top_margin, Calibri_Bold, 11);
            writeText(cb, Varieties, left_margin, top_margin - 35, Calibri_Bold, 11);

            cb.EndText();
            //cb.MoveTo(40, 215);
            // buat rectangle
            //var rect_kernel = new iTextSharp.text.Rectangle(30, 190, 120, 200);
            //rect_kernel.BorderWidth = 2;
            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 20, 100, 15);//xstart ,ystart ,width,height
            //cb.Rectangle(,,,,)
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295 + 140, top_margin - 20, 100, 15);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 55, 100, 15);
            cb.Stroke();

            // Document Footer ---------------------------


            top_margin = 110 + 435;
            left_margin = 40;
            //string qcpersonel = "QC Personnel";
            //string approvedby = "Approved By: ";

            //string label_name = "Name";
            //string label_signature = "Signature";

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, approvedby, left_margin, top_margin - 10, Calibri, 11);
            writeText(cb, label_name, left_margin, top_margin - 90, Calibri, 11);
            writeText(cb, label_signature, left_margin + 140, top_margin - 90, Calibri, 11);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 30 + 435);
            cb.LineTo(120, 30 + 435);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(180, 30 + 435);
            cb.LineTo(260, 30 + 435);
            cb.Stroke();
            //cb.RestoreState();


            #endregion A4 Part 2


            // save to text file History ------------------------------------
            string month = DateTime.Now.ToString("yyyyMM");
            string urlHistory_data = Folder_Path + "History_data_" + sensorname + "/"
                + month.ToString().Trim() + ".txt";

            if (File.Exists(urlHistory_data))
            {
                // write to file
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(urlHistory_data, true))
                {
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(UrlPDF);
                }
            }
            else
            {
                // Create new file
                System.IO.File.WriteAllText(urlHistory_data, UrlPDF);
            }

            document.Close();
            writer.Close();
            fs.Close();

        }


        public static void Generate_Simple_PDF_A5(string label, string sensorname, string printdate, string printtime, string average
       , string numberofmeasure, string printedby, List<Data_Measure> datas, int language)
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            string UrlPDF = Folder_Path + "Print_Result_" + sensorname + "/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";

            FileStream fs = new FileStream(UrlPDF, FileMode.Create, FileAccess.Write, FileShare.None);

            var Pagesize_Chua = new iTextSharp.text.Rectangle(595, 778);
            Document document = new Document(Pagesize_Chua, 10, 10, 30, 30);// left,right,top, bottom

            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            //A4 - which is 8.27 inches x 11.69 in, 210 x 297mm
            //This equates to 595 x 841 in points and 794 x 1122 in pixels. => A4

            //A5 - which is 5-7/8 x 8-1/4, 148 x 210 
            //A5 Wiki equates to 420 x 594 in points and 559 x 793 in pixels. => A5 

            //A4 chua which is 210 * 275
            //A4 chua equates to  595 x 778  in points 

            // calculation 2 A5 to 1 A4 => 36 + 42 + 368. 0 - 368. 42. 410 - 778 

            //A5 chua which is 210 * 130
            //A5 chua equates to  595 x 368  in points  

            BaseFont Calibri = BaseFont.CreateFont("c:\\windows\\fonts\\Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont Calibri_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED);
            BaseFont Arial = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.TTF", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont Arial_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\arialbd.ttf", "Identity-H", BaseFont.EMBEDDED);

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                float WidthOri = png.ScaledWidth;
                float HeightOri = png.ScaledHeight;
                float WidthFinal = 40;
                float HeightFinal = 40;

                if (WidthOri >= HeightOri)
                {
                    //WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    //HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }
                png.ScaleAbsolute(WidthFinal, HeightFinal);
                png.SetAbsolutePosition(40, 305 + 10);// x,y
                cb.AddImage(png);

                png.SetAbsolutePosition(40, 305 + 415);// x,y
                //png.SetAbsolutePosition(40, 740);// x,y
                cb.AddImage(png);
            }

            string conf_companyname = read_config_name();
            string conf_companyaddr = read_config_addr();
            string supplier_name_label;
            string Printed_Date_label;
            string Printed_Time_label;
            string Average_label;
            string Measurement_label;
            string Printed_By_label;

            string Header_Title1;
            string Header_Title2;

            string moisture_content;
            string empty_kernels;
            string Grain_Yield;
            string Varieties;

            string qcpersonel;
            string approvedby;

            string label_name;
            string label_signature;
            // Label English

            // label Title
            if (language == 0)// english
            {
                supplier_name_label = "Supplier Name: ";
                Printed_Date_label = "Printed Date: ";
                Printed_Time_label = "Printed Time: ";
                Average_label = "Average: ";
                Measurement_label = "No of measurement: ";
                Printed_By_label = "Supplier Name: ";

                Header_Title1 = "Incoming Grain Analysis Report";
                Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

                moisture_content = "Moisture Content (%)";
                empty_kernels = "Empty Kernels (%)";
                Grain_Yield = "Grain Yield (%)";
                Varieties = "Varieties";

                qcpersonel = "QC Personnel";
                approvedby = "Approved By: ";

                label_name = "Name";
                label_signature = "Signature";
            }
            else
            {
                supplier_name_label = "Nama Supplier: ";
                Printed_Date_label = "Tanggal Print: ";
                Printed_Time_label = "Waktu Print: ";
                Average_label = "Rata-Rata: ";
                Measurement_label = "Jumlah Pengukuran: ";
                Printed_By_label = "Print Oleh: ";

                Header_Title1 = "Dari Analisa Biji Padi Kering Panen yang Masuk";
                Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

                moisture_content = "Kadar Air (%)";
                empty_kernels = "Butir Hampa (%)";
                Grain_Yield = "Hasil Biji (%)";
                Varieties = "Varieties";

                qcpersonel = "QC Pemeriksa";
                approvedby = "Disetujui Oleh: ";

                label_name = "Nama";
                label_signature = "Tanda Tangan";
            }


            #region A4 Part bawah
            // Draw A line for doc title

            cb.SetLineWidth(0f);
            cb.MoveTo(40, 300 + 10);
            cb.LineTo(560, 300 + 10);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);

            //cb.SaveState();
            cb.BeginText();
            int left_margin = 296;
            //int top_margin = 800;
            int left_margin_title = 300;
            int top_margin = 330 + 10;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 14);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 20, Calibri, 11);

            cb.EndText();

            // Document Header ---------------------------

            left_margin = 40;
            top_margin = 345 - 50 + 10;

            // Rectangle for Supplier Name
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Supplier Name Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Date
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle forPrinted Date Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Printed Time
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Time Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Average
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Average Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Measurement
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Measurement Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            //int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();

            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin - 18, Calibri_Bold, 14);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 38, Calibri_Bold, 14);

            // Label

            writeText(cb, supplier_name_label, left_margin, top_margin - 70, Calibri, 11);
            writeText(cb, label, left_margin + 120, top_margin - 70, Calibri, 11);

            // Date
            writeText(cb, Printed_Date_label, left_margin, top_margin - 85, Calibri, 11);
            writeText(cb, printdate, left_margin + 120, top_margin - 85, Calibri, 11);

            // Time
            writeText(cb, Printed_Time_label, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, printtime, left_margin + 120, top_margin - 100, Calibri, 11);

            // Average
            writeText(cb, Average_label, left_margin, top_margin - 115, Calibri, 11);
            writeText(cb, average, left_margin + 120, top_margin - 115, Calibri, 11);

            // Number of Measurement
            writeText(cb, Measurement_label, left_margin, top_margin - 130, Calibri, 11);
            writeText(cb, numberofmeasure, left_margin + 120, top_margin - 130, Calibri, 11);

            // Printed By
            writeText(cb, Printed_By_label, left_margin, top_margin - 145, Calibri, 11);
            writeText(cb, printedby, left_margin + 120, top_margin - 145, Calibri, 11);

            cb.EndText();


            // Document Body ---------------------------



            // Column Name

            left_margin = 300;
            top_margin = 345 - 70 - 50 + 10;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 10);
            writeText(cb, "ID", left_margin, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Description", left_margin + 40, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Created Time", left_margin + 120, top_margin - 15, Arial_Bold, 10);
            cb.EndText();

            // Draw A line for header data_measure

            cb.SetLineWidth(0f);
            cb.MoveTo(295, top_margin - 18);
            cb.LineTo(550, top_margin - 18);
            cb.Stroke();


            // Measure_Data
            cb.BeginText();

            left_margin = 300;
            top_margin = 240 - 50 + 10;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 10);
                writeText(cb, data.Measures, left_margin + 40, top_margin, Calibri, 10);
                writeText(cb, data.Created_date.ToString(), left_margin + 120, top_margin, Calibri, 10);
                top_margin -= 12;
            }
            // 465 - 20*10 = 265

            top_margin = 120 - 50 + 10;
            left_margin = 300;

            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, Grain_Yield, left_margin + 140, top_margin, Calibri_Bold, 11);
            writeText(cb, Varieties, left_margin, top_margin - 35, Calibri_Bold, 11);

            cb.EndText();
            //cb.MoveTo(40, 215);
            // buat rectangle
            //var rect_kernel = new iTextSharp.text.Rectangle(30, 190, 120, 200);
            //rect_kernel.BorderWidth = 2;
            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 20, 100, 15);//xstart ,ystart ,width,height
            //cb.Rectangle(,,,,)
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295 + 140, top_margin - 20, 100, 15);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 55, 100, 15);
            cb.Stroke();

            // Document Footer ---------------------------


            top_margin = 110 + 10;
            left_margin = 40;

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, approvedby, left_margin, top_margin - 10, Calibri, 11);
            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, label_signature, left_margin + 140, top_margin - 100, Calibri, 11);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 30);
            cb.LineTo(120, 30);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(180, 30);
            cb.LineTo(260, 30);
            cb.Stroke();
            //cb.RestoreState();


            #endregion A4 Part 1


            #region A4 Part atas


            // Draw A line for doc title

            cb.SetLineWidth(0f);
            cb.MoveTo(40, 300 + 415);
            cb.LineTo(560, 300 + 415);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);

            //cb.SaveState();
            cb.BeginText();
            left_margin = 296;
            //int top_margin = 800;

            top_margin = 330 + 415;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 14);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 20, Calibri, 11);


            cb.EndText();

            // Document Header ---------------------------
            //Header_Title1 = "Incoming Grain Analysis Report";
            //Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

            left_margin = 40;
            top_margin = 345 + 415 - 50;

            // Rectangle for Supplier Name
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Supplier Name Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Date
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle forPrinted Date Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Printed Time
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Time Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Average
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Average Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Measurement
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Measurement Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();



            //int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();


            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin - 18, Calibri_Bold, 14);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 38, Calibri_Bold, 14);

            // Label

            writeText(cb, supplier_name_label, left_margin, top_margin - 70, Calibri, 11);
            writeText(cb, label, left_margin + 120, top_margin - 70, Calibri, 11);

            // Date
            writeText(cb, Printed_Date_label, left_margin, top_margin - 85, Calibri, 11);
            writeText(cb, printdate, left_margin + 120, top_margin - 85, Calibri, 11);

            // Time
            writeText(cb, Printed_Time_label, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, printtime, left_margin + 120, top_margin - 100, Calibri, 11);

            // Average
            writeText(cb, Average_label, left_margin, top_margin - 115, Calibri, 11);
            writeText(cb, average, left_margin + 120, top_margin - 115, Calibri, 11);

            // Number of Measurement
            writeText(cb, Measurement_label, left_margin, top_margin - 130, Calibri, 11);
            writeText(cb, numberofmeasure, left_margin + 120, top_margin - 130, Calibri, 11);

            // Printed By
            writeText(cb, Printed_By_label, left_margin, top_margin - 145, Calibri, 11);
            writeText(cb, printedby, left_margin + 120, top_margin - 145, Calibri, 11);

            cb.EndText();


            // Document Body ---------------------------

            //string moisture_content = "Moisture Content (%)";

            // Column Name

            left_margin = 300;
            top_margin = 345 - 70 + 415 - 50;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 10);
            writeText(cb, "ID", left_margin, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Description", left_margin + 40, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Created Time", left_margin + 120, top_margin - 15, Arial_Bold, 10);
            cb.EndText();

            // Draw A line for header data_measure

            cb.SetLineWidth(0f);
            cb.MoveTo(295, top_margin - 18);
            cb.LineTo(550, top_margin - 18);
            cb.Stroke();



            // Measure_Data
            cb.BeginText();

            left_margin = 300;
            top_margin = 240 + 415 - 50;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 10);
                writeText(cb, data.Measures, left_margin + 40, top_margin, Calibri, 10);
                writeText(cb, data.Created_date.ToString(), left_margin + 120, top_margin, Calibri, 10);
                top_margin -= 12;
            }
            // 465 - 20*10 = 265

            top_margin = 120 + 415 - 50;
            left_margin = 300;
            //string empty_kernels = "Empty Kernels (%)";
            //string Grain_Yield = "Grain Yield (%)";
            //string Varieties = "Varieties";
            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, Grain_Yield, left_margin + 140, top_margin, Calibri_Bold, 11);
            writeText(cb, Varieties, left_margin, top_margin - 35, Calibri_Bold, 11);

            cb.EndText();
            //cb.MoveTo(40, 215);
            // buat rectangle
            //var rect_kernel = new iTextSharp.text.Rectangle(30, 190, 120, 200);
            //rect_kernel.BorderWidth = 2;
            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 20, 100, 15);//xstart ,ystart ,width,height
            //cb.Rectangle(,,,,)
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295 + 140, top_margin - 20, 100, 15);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 55, 100, 15);
            cb.Stroke();

            // Document Footer ---------------------------


            top_margin = 110 + 415;
            left_margin = 40;
            //string qcpersonel = "QC Personnel";
            //string approvedby = "Approved By: ";

            //string label_name = "Name";
            //string label_signature = "Signature";

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, approvedby, left_margin, top_margin - 10, Calibri, 11);
            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, label_signature, left_margin + 140, top_margin - 100, Calibri, 11);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 30 + 415 - 10);
            cb.LineTo(120, 30 + 415 - 10);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(180, 30 + 415 - 10);
            cb.LineTo(260, 30 + 415 - 10);
            cb.Stroke();
            //cb.RestoreState();


            #endregion A4 Part 2


            // save to text file History ------------------------------------
            string month = DateTime.Now.ToString("yyyyMM");
            string urlHistory_data = Folder_Path + "History_data_" + sensorname + "/"
                + month.ToString().Trim() + ".txt";

            if (File.Exists(urlHistory_data))
            {
                // write to file
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(urlHistory_data, true))
                {
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(UrlPDF);
                }
            }
            else
            {
                // Create new file
                System.IO.File.WriteAllText(urlHistory_data, UrlPDF);
            }

            document.Close();
            writer.Close();
            fs.Close();

        }

        public static void Generate_Simple_PDF_A4(string label, string sensorname, string printdate, string printtime, string average
        , string numberofmeasure, string printedby, List<Data_Measure> datas, int language)
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            //string UrlPDF = Folder_Path + "Print_Result_Sensor1/" + trimmedlabel + "_" +sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";
            string UrlPDF = Folder_Path + "Print_Result_" + sensorname + "/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";
            FileStream fs = new FileStream(UrlPDF, FileMode.Create, FileAccess.Write, FileShare.None);

            //A4 - which is 8.27 inches x 11.69 in, 210 x 297mm
            //This equates to 595 x 841 in points and 794 x 1122 in pixels. => A4

            //A5 - which is 5-7/8 x 8-1/4, 148 x 210 
            //A5 Wiki equates to 420 x 594 in points and 559 x 793 in pixels. => A5 

            //A4 chua which is 210 * 275
            //A4 chua equates to  595 x 778  in points 

            // calculation 2 A5 to 1 A4 => 368 + 42 + 368

            //A5 chua which is 210 * 130
            //A5 chua equates to  595 x 368  in points  

            var Pagesize_Chua = new iTextSharp.text.Rectangle(595, 778);
            Document document = new Document(Pagesize_Chua, 10, 10, 30, 30);// left,right,top, bottom

            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //This equates to 595 x 841 in points and 794 x 1122 in pixels.

            BaseFont Calibri = BaseFont.CreateFont("c:\\windows\\fonts\\Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont Calibri_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED);
            BaseFont Arial = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.TTF", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont Arial_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\arialbd.ttf", "Identity-H", BaseFont.EMBEDDED);

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            //string source_dir = "D:/Job/Bebeb/SensorReader/WpfApp1/Resources/Logo_Chua.jpg";
            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                float WidthOri = png.ScaledWidth;
                float HeightOri = png.ScaledHeight;
                float WidthFinal = 80;
                float HeightFinal = 80;

                if (WidthOri >= HeightOri)
                {
                    //WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    //HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }
                png.ScaleAbsolute(WidthFinal, HeightFinal);
                png.SetAbsolutePosition(40, 740 - 63);// x,y
                cb.AddImage(png);
            }

            string conf_companyname = read_config_name();
            string conf_companyaddr = read_config_addr();
            string supplier_name_label;
            string Printed_Date_label;
            string Printed_Time_label;
            string Average_label;
            string Measurement_label;
            string Printed_By_label;

            string Header_Title1;
            string Header_Title2;

            string moisture_content;
            string empty_kernels;
            string Grain_Yield;
            string Varieties;

            string qcpersonel;
            string approvedby;

            string label_name;
            string label_signature;
            // Label English

            // label Title
            if (language == 0)// english
            {
                supplier_name_label = "Supplier Name: ";
                Printed_Date_label = "Printed Date: ";
                Printed_Time_label = "Printed Time: ";
                Average_label = "Average: ";
                Measurement_label = "No of measurement: ";
                Printed_By_label = "Supplier Name: ";

                Header_Title1 = "Incoming Grain Analysis Report";
                Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

                moisture_content = "Moisture Content (%)";
                empty_kernels = "Empty Kernels (%)";
                Grain_Yield = "Grain Yield (%)";
                Varieties = "Varieties";

                qcpersonel = "QC Personnel";
                approvedby = "Approved By: ";

                label_name = "Name";
                label_signature = "Signature";
            }
            else
            {
                supplier_name_label = "Nama Supplier: ";
                Printed_Date_label = "Tanggal Print: ";
                Printed_Time_label = "Waktu Print: ";
                Average_label = "Rata-Rata: ";
                Measurement_label = "Jumlah Pengukuran: ";
                Printed_By_label = "Print Oleh: ";

                Header_Title1 = "Dari Analisa Biji Padi Kering Panen yang Masuk";
                Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

                moisture_content = "Kadar Air (%)";
                empty_kernels = "Butir Hampa (%)";
                Grain_Yield = "Hasil Biji (%)";
                Varieties = "Varieties";

                qcpersonel = "QC Pemeriksa";
                approvedby = "Disetujui Oleh: ";

                label_name = "Nama";
                label_signature = "Tanda Tangan";
            }
            // Page Indo
            //document.NewPage();
            #region Print Page

            //cb.SetLineDash()
            // Draw A line for doc title
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 710 - 50);
            cb.LineTo(560, 710 - 50);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);
            //cb.SaveState();
            cb.BeginText();

            int left_margin = 300;
            //int top_margin = 800;
            int top_margin = 790 - 63;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 20);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 30, Calibri, 14);
            cb.EndText();
            //cb.RestoreState();
            // Document Header ---------------------------

            left_margin = 40;
            top_margin = 650;
            int left_margin_title = 300;

            cb.SetColorFill(BaseColor.BLACK);
            //cb.SetLineDash(5);
            //cb.SaveState();
            cb.BeginText();

            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin - 10, Calibri_Bold, 16);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 35, Calibri_Bold, 16);

            // Label
            writeText(cb, supplier_name_label, left_margin, top_margin - 70, Calibri, 12);
            writeText(cb, label, left_margin + 90, top_margin - 70, Calibri, 12);

            // Date
            writeText(cb, Printed_Date_label, left_margin, top_margin - 85, Calibri, 12);
            writeText(cb, printdate, left_margin + 90, top_margin - 85, Calibri, 12);

            // Time
            writeText(cb, Printed_Time_label, left_margin, top_margin - 100, Calibri, 12);
            writeText(cb, printtime, left_margin + 90, top_margin - 100, Calibri, 12);

            // Average
            writeText(cb, Average_label, left_margin + 325, top_margin - 70, Calibri, 12);
            writeText(cb, average, left_margin + 440, top_margin - 70, Calibri, 12);

            // Number of Measurement
            writeText(cb, Measurement_label, left_margin + 325, top_margin - 85, Calibri, 12);
            writeText(cb, numberofmeasure, left_margin + 440, top_margin - 85, Calibri, 12);

            // Printed By
            writeText(cb, Printed_By_label, left_margin + 325, top_margin - 100, Calibri, 12);
            writeText(cb, printedby, left_margin + 440, top_margin - 100, Calibri, 12);

            cb.EndText();
            // Document Body ---------------------------
            top_margin = 505;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 18);
            writeText(cb, "ID", left_margin, top_margin - 30, Arial_Bold, 14);
            writeText(cb, "Description", left_margin + 70, top_margin - 30, Arial_Bold, 14);
            writeText(cb, "Created Time", left_margin + 200, top_margin - 30, Arial_Bold, 14);
            cb.EndText();

            // Draw A line for header data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 470);
            cb.LineTo(560, 470);
            cb.Stroke();

            // Measure_Data
            cb.BeginText();

            left_margin = 40;
            top_margin = 450;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 12);
                writeText(cb, data.Measures, left_margin + 70, top_margin, Calibri, 12);
                writeText(cb, data.Created_date.ToString(), left_margin + 200, top_margin, Calibri, 12);
                top_margin -= 20;
            }
            // 465 - 20*10 = 265

            top_margin = 260;
            left_margin = 40;

            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 14);
            writeText(cb, Grain_Yield, left_margin + 240, top_margin, Calibri_Bold, 14);
            writeText(cb, Varieties, left_margin, top_margin - 45, Calibri_Bold, 14);

            cb.EndText();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5, top_margin - 25, 120, 18);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5 + 240, top_margin - 25, 120, 18);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 5, top_margin - 45 - 25, 120, 18);
            cb.Stroke();

            // Document Footer ---------------------------
            top_margin = 150;

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 12);
            writeText(cb, approvedby, left_margin, top_margin - 30, Calibri, 12);


            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 12);
            writeText(cb, label_signature, left_margin + 380, top_margin - 100, Calibri, 12);

            cb.EndText();

            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(left_margin, top_margin - 100 - 5);
            cb.LineTo(left_margin + 80, top_margin - 100 - 5);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(left_margin + 380, top_margin - 100 - 5);
            cb.LineTo(left_margin + 380 + 80, top_margin - 100 - 5);
            cb.Stroke();
            #endregion Page2 end


            // save to text file History ------------------------------------
            string month = DateTime.Now.ToString("yyyyMM");
            //string urlHistory_data = "D:/Sensor_data/History_data_Sensor1/" + month.ToString().Trim() + ".txt";
            string urlHistory_data = Folder_Path + "History_data_" + sensorname + "/"
                + month.ToString().Trim() + ".txt";

            if (File.Exists(urlHistory_data))
            {
                // write to file
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(urlHistory_data, true))
                {
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(UrlPDF);
                }
            }
            else
            {
                // Create new file
                System.IO.File.WriteAllText(urlHistory_data, UrlPDF);
            }

            document.Close();
            writer.Close();
            fs.Close();

        }

        public static void Generate_Simple_PDF_A5_Small(string label, string sensorname, string printdate, string printtime, string average
       , string numberofmeasure, string printedby, List<Data_Measure> datas, int language)
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            string UrlPDF = Folder_Path + "Print_Result_" + sensorname + "/" + trimmedlabel + "_" + sensorname + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";

            FileStream fs = new FileStream(UrlPDF, FileMode.Create, FileAccess.Write, FileShare.None);

            var Pagesize_Chua = new iTextSharp.text.Rectangle(595, 368);
            Document document = new Document(Pagesize_Chua, 10, 10, 30, 30);// left,right,top, botto,
            PdfWriter writer = PdfWriter.GetInstance(document, fs);

            //A4 - which is 8.27 inches x 11.69 in, 210 xiTe 297mm
            //This equates to 595 x 841 in points and 794 x 1122 in pixels. => A4

            //A5 - which is 5-7/8 x 8-1/4, 148 x 210 
            //A5 Wiki equates to 420 x 594 in points and 559 x 793 in pixels. => A5 

            //A4 chua which is 210 * 275
            //A4 chua equates to  595 x 368  in points 

            //A5 chua which is 210 * 130
            //A5 chua equates to  595 x 368  in points  

            BaseFont Calibri = BaseFont.CreateFont("c:\\windows\\fonts\\Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont Calibri_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\Calibrib.ttf", "Identity-H", BaseFont.EMBEDDED);
            BaseFont Arial = BaseFont.CreateFont("c:\\windows\\fonts\\Arial.TTF", BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont Arial_Bold = BaseFont.CreateFont(@"C:\Windows\Fonts\arialbd.ttf", "Identity-H", BaseFont.EMBEDDED);

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                float WidthOri = png.ScaledWidth;
                float HeightOri = png.ScaledHeight;
                float WidthFinal = 40;
                float HeightFinal = 40;

                if (WidthOri >= HeightOri)
                {
                    //WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    //HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }
                png.ScaleAbsolute(WidthFinal, HeightFinal);
                png.SetAbsolutePosition(40, 305 + 10);// x,y
                cb.AddImage(png);

                //png.SetAbsolutePosition(40, 305 + 445);// x,y
                //png.SetAbsolutePosition(40, 740);// x,y
                //cb.AddImage(png);
            }
            string conf_companyname = read_config_name();
            string conf_companyaddr = read_config_addr();
            string supplier_name_label;
            string Printed_Date_label;
            string Printed_Time_label;
            string Average_label;
            string Measurement_label;
            string Printed_By_label;

            string Header_Title1;
            string Header_Title2;

            string moisture_content;
            string empty_kernels;
            string Grain_Yield;
            string Varieties;

            string qcpersonel;
            string approvedby;

            string label_name;
            string label_signature;
            // Label English

            // label Title
            if (language == 0)// english
            {
                supplier_name_label = "Supplier Name: ";
                Printed_Date_label = "Printed Date: ";
                Printed_Time_label = "Printed Time: ";
                Average_label = "Average: ";
                Measurement_label = "No of measurement: ";
                Printed_By_label = "Supplier Name: ";

                Header_Title1 = "Incoming Grain Analysis Report";
                Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

                moisture_content = "Moisture Content (%)";
                empty_kernels = "Empty Kernels (%)";
                Grain_Yield = "Grain Yield (%)";
                Varieties = "Varieties";

                qcpersonel = "QC Personnel";
                approvedby = "Approved By: ";

                label_name = "Name";
                label_signature = "Signature";
            }
            else
            {
                supplier_name_label = "Nama Supplier: ";
                Printed_Date_label = "Tanggal Print: ";
                Printed_Time_label = "Waktu Print: ";
                Average_label = "Rata-Rata: ";
                Measurement_label = "Jumlah Pengukuran: ";
                Printed_By_label = "Print Oleh: ";

                Header_Title1 = "Dari Analisa Biji Padi Kering Panen yang Masuk";
                Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

                moisture_content = "Kadar Air (%)";
                empty_kernels = "Butir Hampa (%)";
                Grain_Yield = "Hasil Biji (%)";
                Varieties = "Varieties";

                qcpersonel = "QC Pemeriksa";
                approvedby = "Disetujui Oleh: ";

                label_name = "Nama";
                label_signature = "Tanda Tangan";
            }

            // Draw A line for doc title

            cb.SetLineWidth(0f);
            cb.MoveTo(40, 300 + 10);
            cb.LineTo(560, 300 + 10);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);

            //cb.SaveState();
            cb.BeginText();
            int left_margin = 296;
            //int top_margin = 800;
            int left_margin_title = 300;
            int top_margin = 330 + 10;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 14);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 20, Calibri, 11);

            cb.EndText();

            // Document Header ---------------------------

            left_margin = 40;
            top_margin = 345 - 50 + 10;

            // Rectangle for Supplier Name
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Supplier Name Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Date
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle forPrinted Date Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Printed Time
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Time Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Average
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Average Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Measurement
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Measurement Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            //int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();

            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin - 18, Calibri_Bold, 14);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 38, Calibri_Bold, 14);

            // Label

            writeText(cb, supplier_name_label, left_margin, top_margin - 70, Calibri, 11);
            writeText(cb, label, left_margin + 120, top_margin - 70, Calibri, 11);

            // Date
            writeText(cb, Printed_Date_label, left_margin, top_margin - 85, Calibri, 11);
            writeText(cb, printdate, left_margin + 120, top_margin - 85, Calibri, 11);

            // Time
            writeText(cb, Printed_Time_label, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, printtime, left_margin + 120, top_margin - 100, Calibri, 11);

            // Average
            writeText(cb, Average_label, left_margin, top_margin - 115, Calibri, 11);
            writeText(cb, average, left_margin + 120, top_margin - 115, Calibri, 11);

            // Number of Measurement
            writeText(cb, Measurement_label, left_margin, top_margin - 130, Calibri, 11);
            writeText(cb, numberofmeasure, left_margin + 120, top_margin - 130, Calibri, 11);

            // Printed By
            writeText(cb, Printed_By_label, left_margin, top_margin - 145, Calibri, 11);
            writeText(cb, printedby, left_margin + 120, top_margin - 145, Calibri, 11);

            cb.EndText();


            // Document Body ---------------------------



            // Column Name

            left_margin = 300;
            top_margin = 345 - 70 - 50 + 10;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 10);
            writeText(cb, "ID", left_margin, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Description", left_margin + 40, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Created Time", left_margin + 120, top_margin - 15, Arial_Bold, 10);
            cb.EndText();

            // Draw A line for header data_measure

            cb.SetLineWidth(0f);
            cb.MoveTo(295, top_margin - 18);
            cb.LineTo(550, top_margin - 18);
            cb.Stroke();


            // Measure_Data
            cb.BeginText();

            left_margin = 300;
            top_margin = 240 - 50 + 10;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 10);
                writeText(cb, data.Measures, left_margin + 40, top_margin, Calibri, 10);
                writeText(cb, data.Created_date.ToString(), left_margin + 120, top_margin, Calibri, 10);
                top_margin -= 12;
            }
            // 465 - 20*10 = 265

            top_margin = 120 - 50 + 10;
            left_margin = 300;

            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, Grain_Yield, left_margin + 140, top_margin, Calibri_Bold, 11);
            writeText(cb, Varieties, left_margin, top_margin - 35, Calibri_Bold, 11);

            cb.EndText();
            //cb.MoveTo(40, 215);
            // buat rectangle
            //var rect_kernel = new iTextSharp.text.Rectangle(30, 190, 120, 200);
            //rect_kernel.BorderWidth = 2;
            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 20, 100, 15);//xstart ,ystart ,width,height
            //cb.Rectangle(,,,,)
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295 + 140, top_margin - 20, 100, 15);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 55, 100, 15);
            cb.Stroke();

            // Document Footer ---------------------------


            top_margin = 110 + 10;
            left_margin = 40;

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, approvedby, left_margin, top_margin - 10, Calibri, 11);
            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, label_signature, left_margin + 140, top_margin - 100, Calibri, 11);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 30);
            cb.LineTo(120, 30);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(180, 30);
            cb.LineTo(260, 30);
            cb.Stroke();
            //cb.RestoreState();


            // Draw A line for doc title

            cb.SetLineWidth(0f);
            cb.MoveTo(40, 300 + 445);
            cb.LineTo(560, 300 + 445);
            cb.Stroke();
            cb.SetColorFill(BaseColor.BLACK);

            //cb.SaveState();
            cb.BeginText();
            left_margin = 296;
            //int top_margin = 800;

            top_margin = 330 + 445;

            writeTextCenter(cb, conf_companyname, left_margin, top_margin, Arial_Bold, 14);
            writeTextCenter(cb, conf_companyaddr, left_margin, top_margin - 20, Calibri, 11);


            cb.EndText();

            // Document Header ---------------------------
            //Header_Title1 = "Incoming Grain Analysis Report";
            //Header_Title2 = "Sensor " + sensorname.Substring(sensorname.Length - 1);

            left_margin = 40;
            top_margin = 345 + 445 - 50;

            // Rectangle for Supplier Name
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Supplier Name Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 70 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Date
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle forPrinted Date Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 85 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Printed Time
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Printed Time Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 100 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Average
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Average Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 115 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();


            // Rectangle for Measurement
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for Measurement Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 130 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();

            // Rectangle for PrintedBy Value
            cb.SetLineWidth(1);
            cb.Rectangle(left_margin - 4 + 115, top_margin - 145 - 4, 115, 15);//xstart ,ystart ,width,height
            cb.Stroke();



            //int left_margin_title = 300;
            cb.SetColorFill(BaseColor.BLACK);
            cb.BeginText();


            writeTextCenter(cb, Header_Title1, left_margin_title, top_margin - 18, Calibri_Bold, 14);
            writeTextCenter(cb, Header_Title2, left_margin_title, top_margin - 38, Calibri_Bold, 14);

            // Label

            writeText(cb, supplier_name_label, left_margin, top_margin - 70, Calibri, 11);
            writeText(cb, label, left_margin + 120, top_margin - 70, Calibri, 11);

            // Date
            writeText(cb, Printed_Date_label, left_margin, top_margin - 85, Calibri, 11);
            writeText(cb, printdate, left_margin + 120, top_margin - 85, Calibri, 11);

            // Time
            writeText(cb, Printed_Time_label, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, printtime, left_margin + 120, top_margin - 100, Calibri, 11);

            // Average
            writeText(cb, Average_label, left_margin, top_margin - 115, Calibri, 11);
            writeText(cb, average, left_margin + 120, top_margin - 115, Calibri, 11);

            // Number of Measurement
            writeText(cb, Measurement_label, left_margin, top_margin - 130, Calibri, 11);
            writeText(cb, numberofmeasure, left_margin + 120, top_margin - 130, Calibri, 11);

            // Printed By
            writeText(cb, Printed_By_label, left_margin, top_margin - 145, Calibri, 11);
            writeText(cb, printedby, left_margin + 120, top_margin - 145, Calibri, 11);

            cb.EndText();


            // Document Body ---------------------------

            //string moisture_content = "Moisture Content (%)";

            // Column Name

            left_margin = 300;
            top_margin = 345 - 70 + 445 - 50;

            cb.BeginText();
            writeText(cb, moisture_content, left_margin, top_margin, Arial_Bold, 10);
            writeText(cb, "ID", left_margin, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Description", left_margin + 40, top_margin - 15, Arial_Bold, 10);
            writeText(cb, "Created Time", left_margin + 120, top_margin - 15, Arial_Bold, 10);
            cb.EndText();

            // Draw A line for header data_measure

            cb.SetLineWidth(0f);
            cb.MoveTo(295, top_margin - 18);
            cb.LineTo(550, top_margin - 18);
            cb.Stroke();



            // Measure_Data
            cb.BeginText();

            left_margin = 300;
            top_margin = 240 + 445 - 50;

            foreach (Data_Measure data in datas)
            {
                writeText(cb, data.Id.ToString(), left_margin, top_margin, Calibri, 10);
                writeText(cb, data.Measures, left_margin + 40, top_margin, Calibri, 10);
                writeText(cb, data.Created_date.ToString(), left_margin + 120, top_margin, Calibri, 10);
                top_margin -= 12;
            }
            // 465 - 20*10 = 265

            top_margin = 120 + 445 - 50;
            left_margin = 300;
            //string empty_kernels = "Empty Kernels (%)";
            //string Grain_Yield = "Grain Yield (%)";
            //string Varieties = "Varieties";
            writeText(cb, empty_kernels, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, Grain_Yield, left_margin + 140, top_margin, Calibri_Bold, 11);
            writeText(cb, Varieties, left_margin, top_margin - 35, Calibri_Bold, 11);

            cb.EndText();
            //cb.MoveTo(40, 215);
            // buat rectangle
            //var rect_kernel = new iTextSharp.text.Rectangle(30, 190, 120, 200);
            //rect_kernel.BorderWidth = 2;
            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 20, 100, 15);//xstart ,ystart ,width,height
            //cb.Rectangle(,,,,)
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295 + 140, top_margin - 20, 100, 15);
            cb.Stroke();

            cb.SetLineWidth(1);
            cb.Rectangle(295, top_margin - 55, 100, 15);
            cb.Stroke();

            // Document Footer ---------------------------


            top_margin = 110 + 445;
            left_margin = 40;
            //string qcpersonel = "QC Personnel";
            //string approvedby = "Approved By: ";

            //string label_name = "Name";
            //string label_signature = "Signature";

            cb.BeginText();

            writeText(cb, qcpersonel, left_margin, top_margin, Calibri_Bold, 11);
            writeText(cb, approvedby, left_margin, top_margin - 10, Calibri, 11);
            writeText(cb, label_name, left_margin, top_margin - 100, Calibri, 11);
            writeText(cb, label_signature, left_margin + 140, top_margin - 100, Calibri, 11);

            cb.EndText();

            // Draw A line for Average data_measure
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 282);
            //cb.LineTo(560, 282);
            cb.Stroke();


            // Draw A line for name
            cb.SetLineWidth(0f);
            cb.MoveTo(40, 30 + 445 - 10);
            cb.LineTo(120, 30 + 445 - 10);
            cb.Stroke();

            // Draw A line for Signature
            cb.SetLineWidth(0f);
            cb.MoveTo(180, 30 + 445 - 10);
            cb.LineTo(260, 30 + 445 - 10);
            cb.Stroke();
            //cb.RestoreState();





            // save to text file History ------------------------------------
            string month = DateTime.Now.ToString("yyyyMM");
            string urlHistory_data = Folder_Path + "History_data_" + sensorname + "/"
                + month.ToString().Trim() + ".txt";

            if (File.Exists(urlHistory_data))
            {
                // write to file
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(urlHistory_data, true))
                {
                    file.WriteLine(Environment.NewLine);
                    file.WriteLine(UrlPDF);
                }
            }
            else
            {
                // Create new file
                System.IO.File.WriteAllText(urlHistory_data, UrlPDF);
            }

            document.Close();
            writer.Close();
            fs.Close();

        }

        public static void OpenCon_Port(SerialPort mySerialPort, int BaudRate)
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
            //mySerialPort.DataReceived += new SerialDataReceivedEventHandler(ProcessSensorData);
            mySerialPort.Open();
            Application.Current.Dispatcher.Invoke(new Action(() => {
                MessageBox.Show("Port is opened. Start Collecting Data", application_name);
            }));
        }

        public static Sql_Measure_Batch Test_SQL_Measure_Result()
        {
            float measure_data = 12;
            float measure_data_inc = 0.2f;

            Sql_Measure_Batch Batch_Test = new Sql_Measure_Batch();
            Batch_Test.batch_measure_ID_cls = 999;
            Batch_Test.start_date_cls = DateTime.Now.ToString();
            Batch_Test.finish_date_cls = DateTime.Now.ToString();
            Batch_Test.ipaddress_cls = "1";

            Batch_Test.number_per_interval_cls = 10;
            Batch_Test.product_cls = "Corn";
            Batch_Test.time_interval_cls = "1 min";
            Batch_Test.temperature_cls = "29.8";

            List<Sql_Measure_Result> List_Average_Test = new List<Sql_Measure_Result>
            {
                
                //new Data_Measure {Id = 1, Measures = 241, Created_date = DateTime.Now},
                new Sql_Measure_Result(999 ,1,13,10,DateTime.Now.ToString(),1 ),

            };

            List<Sql_Measure_Result> List_Measure_Test = new List<Sql_Measure_Result>
            {
                
                //new Data_Measure {Id = 1, Measures = 241, Created_date = DateTime.Now},
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc*2,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc*3,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc*4,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc*5,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc*6,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc*7,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc*8,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc*9,10,DateTime.Now.ToString(),1 ),
                new Sql_Measure_Result(999 ,1,measure_data + measure_data_inc,10,DateTime.Now.ToString(),1 ),

            };

            Batch_Test.List_Average_Result = List_Average_Test;
            Batch_Test.List_Measure_Result = List_Measure_Test;
            return Batch_Test;
        }


        private static System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        public static void Generate_Controller_PDF(string label, string sensorname, string printdate, string printtime, string average
        , string numberofmeasure, string printedby, Sql_Measure_Batch batch_data
            , int language
            //, List<string> List_Histo_value, List<string> List_Freq_Value
            )
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            string UrlPDF = Folder_Path + "Print_Result" + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss").Trim() + ".pdf";

            Workbook workbook = new Workbook();
            //workbook.LoadFromFile(@"E:\Work\Documents\ExcelFiles\Staff Contact Info.xlsx");
            //string sourceexcel = "D:\\Job\\Bebeb\\Publish_Input\\Requirement\\latest\\excel_template_input_V04.xlsx";
            string sourceexcel_2003 = "D:\\Job\\Bebeb\\Publish_Input\\Requirement\\latest\\excel_template_input_V05_2003.xls";
            //workbook.LoadFromFile(sourceexcel,true);
            workbook.LoadFromFile(sourceexcel_2003, ExcelVersion.Version97to2003);

            //workbook.CreateEmptySheets(2);

            //test


            Worksheet sheet = workbook.Worksheets[0];
            sheet.PageSetup.TopMargin = 0.0;
            sheet.PageSetup.BottomMargin = 0.0;
            sheet.PageSetup.LeftMargin = 0.7;
            sheet.PageSetup.RightMargin = 0.7;
            sheet.PageSetup.HeaderMarginInch = 0.0;
            sheet.PageSetup.FooterMarginInch = 0.0;

            // Company Name
            //string conf_companyname = "abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde";
            string conf_companyname = "PT PLN INDONESIA";

            sheet.Range["E2"].Value = conf_companyname;


            // Company Address
            //string conf_companyaddr = "abcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcdeabcde";
            string conf_companyaddr = "Jl Cendrawasih 3 no1A, Cikarang";
            sheet.Range["E3"].Value = conf_companyaddr;
            //sheet.PageSetup.CenterHeader = "test center";
            //sheet.PageSetup.CenterHeader = "&C&BAmalgamated Widgets, Inc.&B";
            // Logo
            //sheet.Range["A2"].Value = "Logo";
            string source_dir = Folder_Path + "dataconfig/Logo.jpg";
            iTextSharp.text.Image png;
            System.Drawing.Image newImage;

            newImage = System.Drawing.Image.FromFile(source_dir);
            newImage = resizeImage(newImage, new Size(100, 100));
            sheet.Pictures.Add(2, 1, newImage);

            //sheet.PageSetup.LeftHeaderImage = newImage;

            //List<Sql_Measure_Result> List_Measure_Average = batch_data.List_Average_Result;
            // Admin data
            #region admin Data

            sheet.Range["C7"].Value = label;
            sheet.Range["K7"].Value = printedby;

            sheet.Range["C8"].Value = DateTime.Now.ToString();
            sheet.Range["C9"].Value = batch_data.product_cls;
            sheet.Range["C10"].Value = batch_data.ipaddress_cls;


            float total_average = 0;
            foreach (Sql_Measure_Result Measure_Average in batch_data.List_Average_Result)
            {
                total_average = total_average + Measure_Average.measure_result_cls;
            }

            sheet.Range["C11"].Value = (total_average / batch_data.List_Average_Result.Count()).ToString();

            sheet.Range["K8"].Value = batch_data.total_interval_cls.ToString();
            sheet.Range["K9"].Value = (batch_data.total_interval_cls * batch_data.number_per_interval_cls).ToString();
            sheet.Range["K10"].Value = batch_data.temperature_cls;
            //sheet.Range["L11"].Value = "Total No Of intervals";

            #endregion

            // AVERAGE DATA Input //
            #region Average Data Start

            int ex_row;
            int ex_col;
            string column_MeasureAvg;
            string column_Pcs;
            string column_date;


            #region 1-17
            //1-17
            for (ex_row = 25; ex_row <= 41; ex_row++)
            {
                try
                {
                    column_MeasureAvg = "B" + ex_row.ToString();
                    column_Pcs = "C" + ex_row.ToString();
                    column_date = "D" + ex_row.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row - 25].measure_result_cls.ToString();
                    //sheet.Range[column_Pcs].Value = test_sql_data[ex_row - 25].PerBatch_ID_cls.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.number_per_interval_cls.ToString();
                    sheet.Range[column_date].Value = batch_data.List_Average_Result[ex_row - 25].created_on_cls.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ex_row = 999;
                }

            }
            #endregion

            #region 18-34
            //18-34
            try
            {
                for (ex_row = 25; ex_row <= 41; ex_row++)
                {
                    column_MeasureAvg = "G" + ex_row.ToString();
                    column_Pcs = "H" + ex_row.ToString();
                    column_date = "I" + ex_row.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row - 8].measure_result_cls.ToString();
                    //sheet.Range[column_Pcs].Value = test_sql_data[ex_row - 8].PerBatch_ID_cls.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.number_per_interval_cls.ToString();
                    sheet.Range[column_date].Value = batch_data.List_Average_Result[ex_row - 8].created_on_cls.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex_row = 999;
            }
            #endregion

            #region 35-50
            //35-50
            try
            {
                for (ex_row = 25; ex_row <= 40; ex_row++)
                {
                    column_MeasureAvg = "L" + ex_row.ToString();
                    column_Pcs = "M" + ex_row.ToString();
                    column_date = "N" + ex_row.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row + 9].measure_result_cls.ToString();
                    //sheet.Range[column_Pcs].Value = test_sql_data[ex_row + 9].PerBatch_ID_cls.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.number_per_interval_cls.ToString();
                    sheet.Range[column_date].Value = batch_data.List_Average_Result[ex_row + 9].created_on_cls.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex_row = 999;
            }
            #endregion


            #endregion


            // Create Histogram Chart
            #region Create Chart

            // Prepare Data
            List<float> Histo_Original_measures = new List<float> { };
            List<float> Histo_Rounded_measures = new List<float> { };
            List<float> Group_Histo_Rounded_measures = new List<float> { };
            List<int> Histo_Rounded_freqs = new List<int> { };
            double single_measure;
            double final_measure;

            foreach (Sql_Measure_Result data in batch_data.List_Measure_Result)
            {
                Histo_Original_measures.Add(data.measure_result_cls);
            }

            foreach (float original_measure in Histo_Original_measures)
            {
                single_measure = original_measure;
                final_measure = (Math.Round(single_measure * 2, MidpointRounding.AwayFromZero) / 2);
                Histo_Rounded_measures.Add(Convert.ToSingle(final_measure));
            }

            var l1 = Histo_Rounded_measures;

            var g = l1.GroupBy(i => i);
            foreach (var grp in g)
            {
                Group_Histo_Rounded_measures.Add(grp.Key);
                Histo_Rounded_freqs.Add(grp.Count());
            }



            // Assign to Excel
            Worksheet sheet2 = workbook.Worksheets[1];
            SensorHelper_2.Insert_Chart_Data(sheet2, Group_Histo_Rounded_measures, Histo_Rounded_freqs);

            Chart chart_left = sheet.Charts[0];
            chart_left.LeftColumn = 1;
            chart_left.TopRow = 14;
            chart_left.Width = 260;
            chart_left.Height = 370;


            Chart chart_right = sheet.Charts[1];
            chart_right.LeftColumn = 9;
            chart_right.TopRow = 14;
            chart_right.Width = 250;
            chart_right.Height = 370;
            //chart_right.AutoSize = true;

            #endregion

            Worksheet sheet_pdf = workbook.Worksheets[0];
            //string targetpdf = UrlPDF;
            sheet_pdf.SaveToPdf(UrlPDF);
            Sensor_input_Helper.MySql_Insert_PDF(batch_data.ipaddress_cls, batch_data.batch_measure_ID_cls,
                label, printedby, DateTime.Now, UrlPDF);

        }


        public static void Generate_Controller_PDF_revised(string conf_companyname, string conf_companyaddr
            , string label, string printedby, Sql_Measure_Batch batch_data
            , int language
            //, List<string> List_Histo_value, List<string> List_Freq_Value
            )
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            string PDF_Filename = label + "_" + "Sensor 1" + "_" + DateTime.Now.ToString("yyyyMMdd_hhmm").Trim() + "hr" + ".pdf";
            string UrlPDF = Folder_Path + "Print_Result_" + "Sensor1".Trim() + "/" + PDF_Filename;

            Workbook workbook = new Workbook();

            //string sourceexcel_2003 = "C:\\Job\\Bebeb\\Publish_Input\\Requirement\\latest\\excel_template_revised.xls";
            string sourceexcel_2003 = "C:\\Sensor_data\\DataConfig\\excel_template_revised.xls";

            //file:///D:\Job\Bebeb\Publish_Input\Requirement\Latest\excel_template_revised.xls
            //file:///C:\Sensor_data\DataConfig\excel_template_revised.xls

            workbook.OpenPassword = "GX_MX_001";
            workbook.LoadFromFile(sourceexcel_2003, ExcelVersion.Version97to2003);

            //workbook.CreateEmptySheets(2);

            //test

            Worksheet sheet = workbook.Worksheets[0];
            sheet.PageSetup.TopMargin = 0.5;
            sheet.PageSetup.BottomMargin = 0.0;
            sheet.PageSetup.LeftMargin = 0.4;
            sheet.PageSetup.RightMargin = 0.0;
            sheet.PageSetup.HeaderMarginInch = 0.3;
            sheet.PageSetup.FooterMarginInch = 0.5;
            sheet.PageSetup.PaperSize = PaperSizeType.PaperA4;
            sheet.RowColumnHeadersVisible = false;

            // Company Name
            sheet.PageSetup.CenterHeader = conf_companyname;
            sheet.Range["C1"].Value = conf_companyaddr;

            //Logo
            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                System.Drawing.Image image2 = System.Drawing.Image.FromFile(source_dir);
                //Bitmap bitmap = new Bitmap(image2, new Size(100, 100));


                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                double WidthOri = image2.Width;
                double HeightOri = image2.Height;
                double WidthFinal = 80;
                double HeightFinal = 80;


                //float WidthOri = png.ScaledWidth;
                //float HeightOri = png.ScaledHeight;
                //float WidthFinal = 80;
                //float HeightFinal = 80;

                if (WidthOri >= HeightOri)
                {
                    //WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    //HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }

                Bitmap bitmap = new Bitmap(image2, new Size(int.Parse(Math.Floor(WidthFinal).ToString())
                    , int.Parse(Math.Floor(HeightFinal).ToString())));

                sheet.PageSetup.LeftHeaderImage = bitmap;
                sheet.PageSetup.LeftHeader = "&G";

                //png.ScaleAbsolute(WidthFinal, HeightFinal);
                //png.SetAbsolutePosition(40, 740 - 63);// x,y

                //cb.AddImage(png);

            }

            #region admin Data

            sheet.Range["C6"].Text = label;
            sheet.Range["C7"].Text = batch_data.start_date_cls;

            //sheet.Range["C7"].DateTimeValue = DateTime.Now;
            //sheet.Range["C7"].Text = DateTime.Now.ToString("yy/MM/dd HH:mm:ss");

            sheet.Range["C8"].Text = printedby;
            if (batch_data.product_cls == "Brown Rice")
            {
                sheet.Range["L6"].Text = "Corn High";
            }
            else if (batch_data.product_cls == "Wheat")
            {
                sheet.Range["L6"].Text = "Corn Medium";
            }
            else if (batch_data.product_cls == "Corn")
            {
                sheet.Range["L6"].Text = "Corn Low";
            }
            else
            {
                sheet.Range["L6"].Text = batch_data.product_cls;
            }
            sheet.Range["L7"].Text = batch_data.temperature_cls + "°C";
            //sheet.Range["L8"].Value = batch_data.ipaddress_cls;

            sheet.Range["L8"].Text = "1";

            //sheet.Range["L8"].Value = batch_data.ipaddress_cls;
            sheet.Range["S6"].Text = (batch_data.total_interval_cls * batch_data.number_per_interval_cls).ToString();
            sheet.Range["S7"].Text = batch_data.total_interval_cls.ToString();

            float total_average = 0;
            foreach (Sql_Measure_Result Measure_Average in batch_data.List_Average_Result)
            {
                total_average = total_average + Measure_Average.measure_result_cls;
            }

            sheet.Range["S8"].Text = (total_average / batch_data.List_Average_Result.Count()).ToString() + "%";

            // 

            //sheet.Range["L11"].Value = "Total No Of intervals";

            #endregion

            // AVERAGE DATA Input //
            #region Average Data Start

            int ex_row;
            int ex_col;
            string column_MeasureAvg;
            string column_Pcs;
            string column_date;


            #region 1-17
            //1-17
            for (ex_row = 34; ex_row <= 50; ex_row++)
            {
                try
                {
                    column_MeasureAvg = "B" + ex_row.ToString();
                    column_Pcs = "C" + ex_row.ToString();
                    column_date = "D" + ex_row.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row - 34].measure_result_cls.ToString();
                    //sheet.Range[column_Pcs].Value = test_sql_data[ex_row - 25].PerBatch_ID_cls.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.number_per_interval_cls.ToString();
                    //sheet.Range[column_date].Value = batch_data.List_Average_Result[ex_row - 34].created_on_cls.ToString();
                    sheet.Range[column_date].Text = batch_data.List_Average_Result[ex_row - 34].created_on_cls;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ex_row = 999;
                }

            }
            #endregion

            #region 18-34
            //18-34
            try
            {
                for (ex_row = 34; ex_row <= 50; ex_row++)
                {
                    column_MeasureAvg = "G" + ex_row.ToString();
                    column_Pcs = "H" + ex_row.ToString();
                    column_date = "I" + ex_row.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row - 17].measure_result_cls.ToString();
                    //sheet.Range[column_Pcs].Value = test_sql_data[ex_row - 8].PerBatch_ID_cls.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.number_per_interval_cls.ToString();
                    sheet.Range[column_date].Value = batch_data.List_Average_Result[ex_row - 17].created_on_cls.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex_row = 999;
            }
            #endregion

            #region 35-50
            //35-50
            try
            {
                for (ex_row = 34; ex_row <= 49; ex_row++)
                {
                    column_MeasureAvg = "M" + ex_row.ToString();
                    column_Pcs = "P" + ex_row.ToString();
                    column_date = "Q" + ex_row.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row].measure_result_cls.ToString();
                    //sheet.Range[column_Pcs].Value = test_sql_data[ex_row + 9].PerBatch_ID_cls.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.number_per_interval_cls.ToString();
                    sheet.Range[column_date].Value = batch_data.List_Average_Result[ex_row].created_on_cls.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex_row = 999;
            }
            #endregion


            #endregion


            // Create Histogram Chart
            #region Create Chart

            // Prepare Data
            List<float> Histo_Original_measures = new List<float> { };
            List<float> Histo_Rounded_measures = new List<float> { };
            List<float> Group_Histo_Rounded_measures = new List<float> { };
            List<int> Histo_Rounded_freqs = new List<int> { };
            double single_measure;
            double final_measure;

            foreach (Sql_Measure_Result data in batch_data.List_Measure_Result)
            {
                Histo_Original_measures.Add(data.measure_result_cls);
            }

            foreach (float original_measure in Histo_Original_measures)
            {
                single_measure = original_measure;
                final_measure = (Math.Round(single_measure * 2, MidpointRounding.AwayFromZero) / 2);
                Histo_Rounded_measures.Add(Convert.ToSingle(final_measure));
            }

            var l1 = Histo_Rounded_measures;

            var g = l1.GroupBy(i => i);
            foreach (var grp in g)
            {
                Group_Histo_Rounded_measures.Add(grp.Key);
                Histo_Rounded_freqs.Add(grp.Count());
            }



            // Assign to Excel
            Worksheet sheet2 = workbook.Worksheets[1];
            SensorHelper_2.Insert_Chart_Data(sheet2, Group_Histo_Rounded_measures, Histo_Rounded_freqs);

            /*
            Chart chart_left = sheet.Charts[0];
            chart_left.LeftColumn = 1;
            chart_left.TopRow = 14;
            chart_left.Width = 260;
            chart_left.Height = 370;


            Chart chart_right = sheet.Charts[1];
            chart_right.LeftColumn = 9;
            chart_right.TopRow = 14;
            chart_right.Width = 250;
            chart_right.Height = 370;
            //chart_right.AutoSize = true;
            */
            #endregion

            #region thereshold
            List<SQL_Data_Config> List_Data_Configs = new List<SQL_Data_Config> { };
            List_Data_Configs = Sensor_input_Helper.MySql_Get_DataConfig(batch_data.ipaddress_cls);

            var thereshold_Max_var = List_Data_Configs.Find(item => item.Config_Param == "Thereshold_Max").Config_Value;
            var thereshold_Min_var = List_Data_Configs.Find(item => item.Config_Param == "Thereshold_Min").Config_Value;

            double thereshold_Max = double.Parse(thereshold_Max_var.ToString());
            double thereshold_Min = double.Parse(thereshold_Min_var.ToString());

            //var theresholdmax = 
            //ListFilesToProcess.Count(item => item.IsChecked);
            double number_of_thereshold_max = batch_data.List_Measure_Result.Count(item => item.measure_result_cls > thereshold_Max);
            double number_of_thereshold_min = batch_data.List_Measure_Result.Count(item => item.measure_result_cls < thereshold_Min);

            sheet.Range["G52"].Text = thereshold_Max_var.ToString() + "%";
            sheet.Range["G53"].Text = thereshold_Min_var.ToString() + "%";

            sheet.Range["H52"].Text = number_of_thereshold_max.ToString();
            sheet.Range["H53"].Text = number_of_thereshold_min.ToString();


            #endregion

            Worksheet sheet_pdf = workbook.Worksheets[0];
            //string targetpdf = UrlPDF;
            sheet_pdf.SaveToPdf(UrlPDF);
            sheet_pdf.SaveToImage("C:\\Sensor_data\\testimage123.png");
            Sensor_input_Helper.MySql_Insert_PDF(batch_data.ipaddress_cls, batch_data.batch_measure_ID_cls,
                label, printedby, DateTime.Now, UrlPDF);

        }

        public static void Generate_Controller_PDF_revised_5Aug2021(string conf_companyname, string conf_companyaddr
            , string label, string printedby, Sql_Measure_Batch batch_data
            , int language, bool IsPrematureStop
            //, List<string> List_Histo_value, List<string> List_Freq_Value
            )
        {
            string trimmedlabel = String.Concat(label.Where(c => !Char.IsWhiteSpace(c)));
            string PDF_Filename = label + "_" + "Sensor "+ batch_data.ipaddress_cls.Last() + "_" + DateTime.Now.ToString("yyyyMMdd_hhmm").Trim() + "hr" + ".pdf";
            string UrlPDF = Folder_Path + "Print_Result_" + "Sensor".Trim() + batch_data.ipaddress_cls.Last() + "/" + PDF_Filename;

            Workbook workbook = new Workbook();

            //string sourceexcel_2003 = "C:\\Job\\Bebeb\\Publish_Input\\Requirement\\latest\\excel_template_revised.xls";
            //string sourceexcel_2003 = "C:\\Sensor_data\\DataConfig\\excel_template_revised_3.xls";
            string sourceexcel_2003 = "C:\\Sensor_data\\DataConfig\\excel_template.xls";

            //file:///D:\Job\Bebeb\Publish_Input\Requirement\Latest\excel_template_revised.xls
            //file:///C:\Sensor_data\DataConfig\excel_template_revised.xls

            workbook.OpenPassword = "GX_MX_001";
            workbook.LoadFromFile(sourceexcel_2003, ExcelVersion.Version97to2003);

            //workbook.CreateEmptySheets(2);
            //test

            Worksheet sheet = workbook.Worksheets[0];
            sheet.PageSetup.TopMargin = 0.5;
            sheet.PageSetup.BottomMargin = 0.0;
            sheet.PageSetup.LeftMargin = 0.4;
            sheet.PageSetup.RightMargin = 0.0;
            sheet.PageSetup.HeaderMarginInch = 0.3;
            sheet.PageSetup.FooterMarginInch = 0.5;
            sheet.PageSetup.PaperSize = PaperSizeType.PaperA4;
            sheet.RowColumnHeadersVisible = false;

            // Company Name
            sheet.PageSetup.CenterHeader = conf_companyname;
            sheet.Range["A1"].Value = conf_companyaddr;

            //Logo
            string source_dir = Folder_Path + "dataconfig/Logo.png";
            if (File.Exists(source_dir))
            {
                System.Drawing.Image image2 = System.Drawing.Image.FromFile(source_dir);
                iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(source_dir);
                double WidthOri = image2.Width;
                double HeightOri = image2.Height;
                double WidthFinal = 80;
                double HeightFinal = 80;

                if (WidthOri >= HeightOri)
                {
                    //WidthFinal = 90;
                    HeightFinal = ((WidthFinal / WidthOri) * HeightOri);
                }
                else
                {
                    //HeightFinal = 90;
                    WidthFinal = ((HeightFinal / HeightOri) * WidthOri);
                }

                Bitmap bitmap = new Bitmap(image2, new Size(int.Parse(Math.Floor(WidthFinal).ToString())
                    , int.Parse(Math.Floor(HeightFinal).ToString())));

                sheet.PageSetup.LeftHeaderImage = bitmap;
                sheet.PageSetup.LeftHeader = "&G";

                //png.ScaleAbsolute(WidthFinal, HeightFinal);
                //png.SetAbsolutePosition(40, 740 - 63);// x,y

                //cb.AddImage(png);

            }

            #region admin Data

            sheet.Range["D6"].Text = label;
            sheet.Range["D7"].Text = batch_data.start_date_cls;
            sheet.Range["D8"].Text = printedby;
            sheet.Range["N6"].Text = batch_data.product_cls;
            sheet.Range["N7"].Text = batch_data.temperature_cls + "°C";
            sheet.Range["N8"].Text = batch_data.ipaddress_cls.Substring(batch_data.ipaddress_cls.Length - 1);
            //Substring(input.Length - 3)

            //sheet.Range["L8"].Value = batch_data.ipaddress_cls;
            sheet.Range["W6"].Text = (batch_data.total_interval_cls * batch_data.number_per_interval_cls).ToString();
            sheet.Range["W7"].Text = batch_data.total_interval_cls.ToString();

            float total_average = 0;
            foreach (Sql_Measure_Result Measure_Average in batch_data.List_Average_Result)
            {
                total_average = total_average + Measure_Average.measure_result_cls;
            }
            sheet.Range["W8"].Text = (total_average / batch_data.List_Average_Result.Count()).ToString("0.00") + "%";
            // 

            //sheet.Range["L11"].Value = "Total No Of intervals";

            #endregion

            // AVERAGE DATA Input //
            #region Average Data Start

            int ex_row;
            int ex_col;
            string column_MeasureAvg;
            string column_Pcs;
            string column_date;


            #region 1-17
            //1-17
            for (ex_row = 34; ex_row <= 50; ex_row++)
            {
                try
                {
                    column_Pcs = "B" + ex_row.ToString();
                    column_MeasureAvg = "C" + ex_row.ToString();
                    column_date = "E" + ex_row.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.List_Average_Result[ex_row - 34].No_Of_Peaces.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row - 34].measure_result_cls.ToString();
                    sheet.Range[column_date].Text = batch_data.List_Average_Result[ex_row - 34].created_on_cls;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ex_row = 999;
                }

            }
            #endregion

            #region 18-34
            //18-34
            try
            {
                for (ex_row = 34; ex_row <= 50; ex_row++)
                {
                    column_Pcs = "I" + ex_row.ToString();
                    column_MeasureAvg = "K" + ex_row.ToString();
                    column_date = "L" + ex_row.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.List_Average_Result[ex_row - 17].No_Of_Peaces.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row - 17].measure_result_cls.ToString();
                    sheet.Range[column_date].Text = batch_data.List_Average_Result[ex_row - 17].created_on_cls.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex_row = 999;
            }
            #endregion

            #region 35-50
            //35-50
            try
            {
                for (ex_row = 34; ex_row <= 49; ex_row++)
                {
                    column_Pcs = "S" + ex_row.ToString();
                    column_MeasureAvg = "T" + ex_row.ToString();
                    column_date = "V" + ex_row.ToString();
                    sheet.Range[column_Pcs].Value = batch_data.List_Average_Result[ex_row].No_Of_Peaces.ToString();
                    sheet.Range[column_MeasureAvg].Value = batch_data.List_Average_Result[ex_row].measure_result_cls.ToString();
                    sheet.Range[column_date].Text = batch_data.List_Average_Result[ex_row].created_on_cls.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ex_row = 999;
            }
            #endregion


            #endregion


            // Create Histogram Chart
            #region Create Chart

            // Prepare Data
            List<float> Histo_Original_measures = new List<float> { };
            List<float> Histo_Rounded_measures = new List<float> { };
            List<float> Group_Histo_Rounded_measures = new List<float> { };
            List<int> Histo_Rounded_freqs = new List<int> { };
            double single_measure;
            double final_measure;

            foreach (Sql_Measure_Result data in batch_data.List_Measure_Result)
            {
                Histo_Original_measures.Add(data.measure_result_cls);
            }

            foreach (float original_measure in Histo_Original_measures)
            {
                single_measure = original_measure;
                final_measure = (Math.Round(single_measure * 2, MidpointRounding.AwayFromZero) / 2);
                Histo_Rounded_measures.Add(Convert.ToSingle(final_measure));
            }

            var l1 = Histo_Rounded_measures;

            var g = l1.GroupBy(i => i);
            foreach (var grp in g)
            {
                Group_Histo_Rounded_measures.Add(grp.Key);
                Histo_Rounded_freqs.Add(grp.Count());
            }



            // Assign to Excel
            Worksheet sheet2 = workbook.Worksheets[1];
            SensorHelper_2.Insert_Chart_Data(sheet2, Group_Histo_Rounded_measures, Histo_Rounded_freqs);

            /*
            Chart chart_left = sheet.Charts[0];
            chart_left.LeftColumn = 1;
            chart_left.TopRow = 14;
            chart_left.Width = 260;
            chart_left.Height = 370;


            Chart chart_right = sheet.Charts[1];
            chart_right.LeftColumn = 9;
            chart_right.TopRow = 14;
            chart_right.Width = 250;
            chart_right.Height = 370;
            //chart_right.AutoSize = true;
            */
            #endregion

            #region thereshold
            List<SQL_Data_Config> List_Data_Configs = new List<SQL_Data_Config> { };
            List_Data_Configs = Sensor_input_Helper.MySql_Get_DataConfig(batch_data.ipaddress_cls);

            var thereshold_Max_var = List_Data_Configs.Find(item => item.Config_Param == "Thereshold_Max").Config_Value;
            var thereshold_Min_var = List_Data_Configs.Find(item => item.Config_Param == "Thereshold_Min").Config_Value;

            double thereshold_Max = double.Parse(thereshold_Max_var.ToString());
            double thereshold_Min = double.Parse(thereshold_Min_var.ToString());

            //var theresholdmax = 
            //ListFilesToProcess.Count(item => item.IsChecked);
            double number_of_thereshold_max = batch_data.List_Measure_Result.Count(item => item.measure_result_cls > thereshold_Max);
            double number_of_thereshold_min = batch_data.List_Measure_Result.Count(item => item.measure_result_cls < thereshold_Min);

            sheet.Range["G52"].Text = thereshold_Max_var.ToString() + "%";
            sheet.Range["G53"].Text = thereshold_Min_var.ToString() + "%";

            sheet.Range["J52"].Text = number_of_thereshold_max.ToString();
            sheet.Range["J53"].Text = number_of_thereshold_min.ToString();


            #endregion

            #region prematurestop
            if (IsPrematureStop == true)
            {
                CellRange range_stop_prem = sheet.Range["P52:Y52"];
                range_stop_prem.BorderAround(LineStyleType.Medium, Color.Black);
                range_stop_prem.Text = "Measurement was stopped prematurely";

            }

            #endregion

            Worksheet sheet_pdf = workbook.Worksheets[0];
            //string targetpdf = UrlPDF;
            sheet_pdf.SaveToPdf(UrlPDF);
            //sheet_pdf.SaveToImage("C:\\Sensor_data\\testimage123.png");
            //Sensor_input_Helper.MySql_Insert_PDF(batch_data.ipaddress_cls, batch_data.batch_measure_ID_cls, label, printedby, DateTime.Now, UrlPDF);
            Sensor_input_Helper.MySql_Insert_PDF(batch_data.ipaddress_cls, batch_data.batch_measure_ID_cls, label, printedby, DateTime.Now, PDF_Filename);
        }

        
        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        public static void Insert_Chart_Data(Worksheet sheet, List<float> List_Histo_Value, List<int> List_Freq_Value)
        {

            int length_value = List_Histo_Value.Count;

            int i = 0;
            string cellvalue;
            string cellfreq;

            /*
             * 1. Cari Lokasi index dari masing2 value di histo_rounded
             * 2. Cari index histo rounded
             * 3. Masukkan Frequency histo rounded
             */

            foreach (float Histo_value in List_Histo_Value)
            {
                try
                {

                    for (i = 2; i < 62; i++)
                    {
                        if (i != 32 && i != 62)
                        {
                            cellvalue = "A" + i.ToString();
                            string full = sheet.Range[cellvalue].Value.ToString(); // 0.115
                            double full_convert = double.Parse(full) * 100;

                            string compare = full_convert.ToString();
                            if (Histo_value.ToString() == compare)
                            {
                                int index = List_Histo_Value.IndexOf(Histo_value);
                                cellfreq = "B" + i.ToString();
                                sheet.Range[cellfreq].Value = List_Freq_Value[index].ToString();
                            }
                        }
                    }

                    int FreqMax = List_Freq_Value.Max() + 5;
                    sheet.Range["B32"].Value = FreqMax.ToString();
                    sheet.Range["B62"].Value = FreqMax.ToString();

                }
                catch (Exception error)//(Exception e)
                {
                    MessageBox.Show("Nilai iterasi adalah: " + i, application_name);
                    MessageBox.Show(error.ToString(), application_name);
                    Console.WriteLine(error.Message);
                }

            }

        }

        public static void ExcelDocViewer(string fileName)
        {
            try
            {
                System.Diagnostics.Process.Start(fileName);
            }
            catch { }
        }



        public string ProcessSensorData(SerialPort mySerialPort, object sender, SerialDataReceivedEventArgs args)
        {
            try
            {
                Thread.Sleep(3000);// this solves the problem
                byte[] readBuffer = new byte[mySerialPort.ReadBufferSize];
                int readLen = mySerialPort.Read(readBuffer, 0, readBuffer.Length);
                string readStr = string.Empty;

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
                return readStr;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                Console.WriteLine(ex);
                return "";
            }

        }




    }
}
