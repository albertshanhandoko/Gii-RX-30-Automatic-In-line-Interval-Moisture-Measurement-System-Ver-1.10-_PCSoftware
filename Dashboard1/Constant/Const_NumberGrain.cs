using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections;
using System.Reflection;

namespace Dashboard1.Constant
{
    // Time Interval => pake Enum desc
    // number of interval => pake integer ( 1- 50 )
    // number grain ( 1- 250)
    // number measure  => enum

    public enum myEnum
    {
        [Description("1A")]
        OneA = 1,
        [Description("2A")]
        TwoA = 2,
        [Description("3A")]
        ThreeA = 3,
    };

    public enum Time_Interval
    {
        [Description("0.5 min")]
        min1 = 500,
        [Description("1 min")]
        min2 = 1000,
        [Description("1.5 min")]
        min3 = 1500,
        [Description("2 min")]
        min4 = 2000,
        [Description("2.5 min")]
        min5 = 2500,
        [Description("3 min")]
        min6 = 3000,
        [Description("3.5 min")]
        min7 = 3500,
        [Description("4 min")]
        min8 = 4000,
        [Description("4.5 min")]
        min9 = 4500,
        [Description("5 min")]
        min10 = 5000,
        [Description("5.5 min")]
        min11 = 5500,
        [Description("6 min")]
        min12 = 6000,
        [Description("6.5 min")]
        min13 = 6500,
        [Description("7 min")]
        min14 = 7000,
        [Description("7.5 min")]
        min15 = 7500,
        [Description("8 min")]
        min16 = 8000,
        [Description("8.5 min")]
        min17 = 8500,
        [Description("9 min")]
        min18 = 9000,
        [Description("9.5 min")]
        min19 = 9500,
        [Description("10 min")]
        min20 = 10000,
        [Description("10.5 min")]
        min21 = 10500,
        [Description("11 min")]
        min22 = 11000,
        [Description("11.5 min")]
        min23 = 11500,
        [Description("12 min")]
        min24 = 12000,
        [Description("12.5 min")]
        min25 = 12500,
        [Description("13 min")]
        min26 = 13000,
        [Description("13.5 min")]
        min27 = 13500,
        [Description("14 min")]
        min28 = 14000,
        [Description("14.5 min")]
        min29 = 14500,
        [Description("15 min")]
        min30 = 15000,
        [Description("15.5 min")]
        min31 = 15500,
        [Description("16 min")]
        min32 = 16000,
        [Description("16.5 min")]
        min33 = 16500,
        [Description("17 min")]
        min34 = 17000,
        [Description("17.5 min")]
        min35 = 17500,
        [Description("18 min")]
        min36 = 18000,
        [Description("18.5 min")]
        min37 = 18500,
        [Description("19 min")]
        min38 = 19000,
        [Description("19.5 min")]
        min39 = 19500,
        [Description("20 min")]
        min40 = 20000,
        [Description("20.5 min")]
        min41 = 20500,
        [Description("21 min")]
        min42 = 21000,
        [Description("21.5 min")]
        min43 = 21500,
        [Description("22 min")]
        min44 = 22000,
        [Description("22.5 min")]
        min45 = 22500,
        [Description("23 min")]
        min46 = 23000,
        [Description("23.5 min")]
        min47 = 23500,
        [Description("24 min")]
        min48 = 24000,
        [Description("24.5 min")]
        min49 = 24500,
        [Description("25 min")]
        min50 = 25000,
        [Description("25.5 min")]
        min51 = 25500,
        [Description("26 min")]
        min52 = 26000,
        [Description("26.5 min")]
        min53 = 26500,
        [Description("27 min")]
        min54 = 27000,
        [Description("27.5 min")]
        min55 = 27500,
        [Description("28 min")]
        min56 = 28000,
        [Description("28.5 min")]
        min57 = 28500,
        [Description("29 min")]
        min58 = 29000,
        [Description("29.5 min")]
        min59 = 29500,
        [Description("30 min")]
        min60 = 30000
    };


    public enum TypeOfMeasure
    {
        [Description("22094\r")]
        Short_Paddy,
        [Description("32095\r")]
        Long_Paddy,
        [Description("42096\r")]
        Jasmine_Paddy,
        [Description("52097\r")]
        Long_Sticky_Paddy,
        [Description("62098\r")]
        Long_Parboiled_Rice,
        [Description("72099\r")]
        Peak_AD_count,
        [Description("8209A\r")]
        Wheat
    }

    public enum number_grain
    {

        [Description("10192\r")]
        _10 = 10,
        [Description("10293\r")]
        _20 = 20,
        [Description("10394\r")]
        _30 = 30,
        [Description("10495\r")]
        _40 = 40,
        [Description("10596\r")]
        _50 = 50,

        [Description("10697\r")]
        _60 = 60,
        [Description("10798\r")]
        _70 = 70,
        [Description("10899\r")]
        _80 = 80,
        [Description("1099A\r")]
        _90 = 90,
        [Description("11093\r")]
        _100 = 100,

        [Description("11193\r")]
        _110 = 110,
        [Description("11294\r")]
        _120 = 120,
        [Description("11395\r")]
        _130 = 130,
        [Description("11496\r")]
        _140 = 140,
        [Description("11597\r")]
        _150 = 150,

        [Description("11698\r")]
        _160 = 160,
        [Description("11799\r")]
        _170 = 170,
        [Description("1189A\r")]
        _180 = 180,
        [Description("1199B\r")]
        _190 = 190,
        [Description("12093\r")]
        _200 = 200,

        [Description("12194\r")]
        _210 = 210,
        [Description("12295\r")]
        _220 = 220,
        [Description("12396\r")]
        _230 = 230,
        [Description("12497\r")]
        _240 = 240,
        [Description("12598\r")]
        _250 = 250


    }

    public enum myEnum1
    {
        [Description("1A")]
        OneA = 1,
        [Description("2A")]
        TwoA = 2,
        [Description("3A")]
        ThreeA = 3,
    };



    public enum Error_Sensor_Controller_olddd
    {
        [Description("Connecting cable is not properly connected. Please Turn of power PT 2700 and check sensor connection")]
        error001,
        [Description("Number of grains that were tested is less than two. Please reset with stop key")]
        error010,
        [Description("No grain feed for more than 5 minutes. Please reset with stop key")]
        error011,
        [Description("Temperature less than -20 celc. Please reset with stop key")]
        error020,
        [Description("Temperature more than 70 celc. Please reset with stop key")]
        error021,
        [Description("Abnormal data. Please reset with stop key")]
        error030,
        [Description("Abnormal memory. Please turn off and on PT 2700")]
        error040,
        [Description("Abnormal PU-359 circuits 1. Reset with Stop key")]
        error101,
        [Description("Abnormal PU-359 circuits 2. Reset with Stop key")]
        error102,
        [Description("Abnormal PU-359 circuits 3. Reset with Stop key")]
        error103,
        [Description("Motor Lock. Reset with Stop key")]
        error104,
    };
    public enum Error_Sensor_Controller_old
    {
        [Description("Connecting cable is not properly connected. Please Turn of power PT 2700 and check sensor connection")]
        Error001,
        [Description("Number of grains that were tested is less than two. Please reset with stop key")]
        Error010,
        [Description("No grain feed for more than 5 minutes. Please reset with stop key")]
        Error011,
        [Description("Temperature less than -20 celc. Please reset with stop key")]
        Error020,
        [Description("Temperature more than 70 celc. Please reset with stop key")]
        Error021,
        [Description("Abnormal data. Please reset with stop key")]
        Error030,
        [Description("Abnormal memory. Please turn off and on PT 2700")]
        Error040,
        [Description("Abnormal PU-359 circuits 1. Reset with Stop key")]
        Error101,
        [Description("Abnormal PU-359 circuits 2. Reset with Stop key")]
        Error102,
        [Description("Abnormal PU-359 circuits 3. Reset with Stop key")]
        Error103,
        [Description("Motor Lock. Reset with Stop key")]
        Error104,
    };


    public enum Error_Sensor_PC_old
    {
        [Description("Connecting cable is not properly connected or is broken")]
        error001,
        [Description("Error code: 010 \n At the start of automatic measurement for moisture, the number of grains that were tested in each of three measurements was less than two \n Reset with Stop Key. Clean the grain intake on PU -359")]
        error010,
        [Description("In automatic/manual measurement, there was no grain feed for more than five minutes")]
        error011,
        [Description("The temperature of the temperature sensor shows less than -20 celc.")]
        error020,
        [Description("The temperature of the temperature sensor shows more than 70 celc.")]
        error021,
        [Description("Abnormal data.")]
        error030,
        [Description("Abnormal memory.")]
        error040,
        [Description("Abnormal PU-359 circuits 1.")]
        error101,
        [Description("Abnormal PU-359 circuits 2.")]
        error102,
        [Description("Abnormal PU-359 circuits 3.")]
        error103,
        [Description("Motor Lock.")]
        error104,
    };


    public enum Error_Sensor_PC_Detail
    {
        [Description("Error 020_1")]
        error1000, // 1000 ga mungkin penilaian 
        [Description("Error 022")]
        error1200, // 1200 ga mungkin nilai
        [Description("Error 024")]
        error1400, // 1400 ga mungkin nilai
        [Description("Error 021_1")]
        error1600, // 1600

        // found during measurement
        [Description("Error 020_2")]
        error020, // STX 020 ETX CheckSUM CR
        [Description("Error 021_2")]
        error021, // STX 070 ETX CheckSUM CR

        // 5 minutes
        [Description("In automatic/manual measurement, there was no grain feed for more than five minutes.")]
        error030, // STX 020 ETX CheckSUM CR
        // finish found during measurement

        // Check Command
        [Description("Error 101")]
        error00091,
        [Description("Error 102")]
        error00092,
        [Description("Error 103")]
        error00093,
        [Description("Error 104")]
        error00094,
    };

    public enum Error_Sensor_PC_Fixing
    {
        [Description("Error 020_1")]
        error1000, // 1000 ga mungkin penilaian 
        [Description("Error 022")]
        error1200, // 1200 ga mungkin nilai
        [Description("Error 024")]
        error1400, // 1400 ga mungkin nilai
        [Description("Error 021_1")]
        error1600, // 1600

        [Description("Error 020_2")]
        error020, // STX 020 ETX CheckSUM CR
        [Description("Error 021_2")]
        error021, // STX 070 ETX CheckSUM CR

        // 5 minutes
        [Description("Reset with the [STOP] key. Clean the grain intake on PU-359")]
        error030, // STX 020 ETX CheckSUM CR


        // Check Command
        [Description("Error 101")]
        error00091,
        [Description("Error 102")]
        error00092,
        [Description("Error 103")]
        error00093,
        [Description("Error 104")]
        error00094,
    };




}
