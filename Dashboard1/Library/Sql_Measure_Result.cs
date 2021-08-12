using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard1.Library
{
    class Sql_Measure_Result
    {

        public int Batch_Id_cls { get; set; }
        public int PerBatch_ID_cls { get; set; }
        public float measure_result_cls { get; set; }
        public string created_on_cls { get; set; }
        public int IsAverage_cls { get; set; }
        public int No_Of_Peaces { get; set; }

        public void set(int set_Batch_Id_cls, int set_PerBatch_ID_cls
            , float set_measure_result_cls, int set_No_Of_Peaces, string set_created_on_cls, int set_IsAverage_cls )
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



    }
}
