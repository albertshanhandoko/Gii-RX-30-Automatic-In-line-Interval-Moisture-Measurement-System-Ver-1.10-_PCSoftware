using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard1.Library
{
    public static class Moisture_Measure_enum
    {
        /*
        enum Moisture_Measure
        {
            Electronics = 2,
            Short_Paddy = 2,
            Jasmine_Paddy = 3,
            Long_Sticky_Paddy =4,
            Long_Parboiled_Rice = 5,
            Peak_one_kernel paddy = 6,
            Wheat = 7
        }
        */
        public const string Short_Paddy = "Short Paddy";
        public const string Long_Paddy = "Long Paddy";
        public const string Jasmine_Paddy = "Jasmine Paddy";
        public const string Long_Sticky_Paddy = "Long Sticky Paddy";
        public const string Long_Parboiled_Rice = "Long Parboiled Rice";
        public const string Peak_one_kernel = "Peak A/D count value of one kernel paddy";
        public const string Wheat = "Wheat";

        /*
                 *   int a = 0x2;
          int b = 0x5f;
          int value = a + b; //adding hex values

         string maxHex = int.MaxValue.ToString("x"); //maximum range of hex value as int
         */

        public static string GetHex_String(string measure_dropdown)
        {
            string result = "";
            switch (measure_dropdown) 
                {
                case Moisture_Measure_enum.Short_Paddy:
                    result = "22094\r";
                    break;
                case Moisture_Measure_enum.Long_Paddy:
                    result = "32095\r";
                    break;
                case Moisture_Measure_enum.Jasmine_Paddy:
                    result = "42096\r";
                    break;
                case Moisture_Measure_enum.Long_Sticky_Paddy:
                    result = "52097\r";
                    break;
                case Moisture_Measure_enum.Long_Parboiled_Rice:
                    result = "62098\r";
                    break;
                case Moisture_Measure_enum.Peak_one_kernel:
                    result = "72094\r";
                    break;
                case Moisture_Measure_enum.Wheat:
                    result = "8209A\r";
                    break;

            }

        /*
        char[] hexaDeciNum = new char[100];

            string hex = "";

            for (int i = 0;i < result.Length; i++)
            {
                char ch = result[i];
                int tmp = (int)ch;
                string part = decToHexa(tmp);
                hex += part;
            }

            return result;
        }

        static string decToHexa(int n)
        {
            char[] hexaDeciNum = new char[100];
            int i = 0;

            while (n != 0)
            {
                int temp = 0;
                temp = n % 16;
                if (temp < 10)
                {
                    hexaDeciNum[i] =
                        (char)(temp + 48);
                    i++;
                }
                else
                {
                    hexaDeciNum[i] =
                        (char)(temp + 87);
                    i++;
                }
                n = n / 16;
            }

            string ans = "";

            // Printing hexadecimal number 
            // array in reverse order 
            for (int j = i - 1;
                    j >= 0; j--)
            {
                ans += hexaDeciNum[j];
            }
        */
            return result;
        }



    }
}
