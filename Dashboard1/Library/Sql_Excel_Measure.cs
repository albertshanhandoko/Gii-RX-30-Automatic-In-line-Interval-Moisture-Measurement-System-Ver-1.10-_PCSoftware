using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard1.Library
{
    class Sql_Excel_Measure
    {

        public int Batch_ID { get; set; }
        public string IP_Address { get; set; }
        public string Product { get; set; }
        public int Total_Interval { get; set; }
        public string Time_Interval { get; set; }
        public int Number_Per_Interval { get; set; }
        public string Temperature { get; set; }
        public string Error_Code { get; set; }
        public int Current_Interval { get; set; }
        public int Current_KernelCounter { get; set; }
        public float Measurement_Result { get; set; }
        public string Result_Type { get; set; }

        /*
        public void set(int set_Batch_ID, int set_IP_Address, string set_Product, int set_Total_Interval
            , float set_measure_result_cls, int set_No_Of_Peaces, string set_created_on_cls, int set_IsAverage_cls)
        {
            Batch_Id_cls = set_Batch_Id_cls;
            PerBatch_ID_cls = set_PerBatch_ID_cls;
            measure_result_cls = set_measure_result_cls;
            No_Of_Peaces = set_No_Of_Peaces;
            created_on_cls = set_created_on_cls;
            IsAverage_cls = set_IsAverage_cls;

        }
        public Sql_Measure_Result(int Batch_Id_cls, int PerBatch_ID_cls
            , float measure_result_cls, int No_Of_Peaces, string created_on_cls, int IsAverage_cls)
        {
            set(Batch_Id_cls, PerBatch_ID_cls, measure_result_cls, No_Of_Peaces, created_on_cls, IsAverage_cls);
        }
        */
    }
}
