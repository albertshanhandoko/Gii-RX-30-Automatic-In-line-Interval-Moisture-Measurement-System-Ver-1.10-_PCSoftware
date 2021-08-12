using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard1.Library
{
    class Sql_Measure_Batch
    {

        public int batch_measure_ID_cls { get; set; }
        public string ipaddress_cls { get; set; }
        public string start_date_cls { get; set; }
        public string finish_date_cls { get; set; }
        public string product_cls { get; set; }
        public int total_interval_cls { get; set; }
        public string time_interval_cls { get; set; }

        public int number_per_interval_cls { get; set; }

        public string temperature_cls { get; set; }
        public string Error_code_cls { get; set; }

        public List<Sql_Measure_Result> List_Measure_Result { get; set; }

        public List<Sql_Measure_Result> List_Average_Result { get; set; }


    }
}
