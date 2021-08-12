using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard1.Library
{
    class SQL_Data_Config
    {
        public int Config_Id { get; set; }
        public string Config_Param { get; set; }
        public double Config_Value { get; set; }

        public void set(int set_Config_Id, string set_Config_Param, double set_Config_Value)
        {
            Config_Id = set_Config_Id;
            Config_Param = set_Config_Param;
            Config_Value = set_Config_Value;

        }
        public SQL_Data_Config(int Config_Id, string Config_Param, double Config_Value)
        {
            set(Config_Id, Config_Param, Config_Value);
        }

    }
}
