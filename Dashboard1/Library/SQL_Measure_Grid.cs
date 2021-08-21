using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard1.Library
{
    class SQL_Measure_Grid
    {

        public string Batch_Id_cls { get; set; }
        public string PerBatch_ID_cls { get; set; }
        public string measure_result_cls { get; set; }
        public string created_on_cls { get; set; }
        public string IsAverage_cls { get; set; }
        public string No_Of_Peaces { get; set; }

        public void set(string set_Batch_Id_cls, string set_PerBatch_ID_cls
            , string set_measure_result_cls, string set_No_Of_Peaces, string set_created_on_cls, string set_IsAverage_cls)
        {
            Batch_Id_cls = set_Batch_Id_cls;
            PerBatch_ID_cls = set_PerBatch_ID_cls;
            measure_result_cls = set_measure_result_cls;
            No_Of_Peaces = set_No_Of_Peaces;
            created_on_cls = set_created_on_cls;
            IsAverage_cls = set_IsAverage_cls;

        }
        public SQL_Measure_Grid(string Batch_Id_cls, string PerBatch_ID_cls
            , string measure_result_cls, string No_Of_Peaces, string created_on_cls, string IsAverage_cls)
        {
            set(Batch_Id_cls, PerBatch_ID_cls, measure_result_cls, No_Of_Peaces, created_on_cls, IsAverage_cls);
        }


    }
}
