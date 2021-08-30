// using CanberraDataAccessLib;
// using CanberraDeviceAccessLib;
using System;
using System.Runtime.InteropServices;

using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Win32;

using System.Threading.Tasks;

// using Regata.Core.Hardware;
// using Regata.Core.DataBase;
// using Regata.Core.DataBase.Models;
// using Microsoft.PowerShell;
// using System.Management.Automation;


namespace WorkFlowCore
{
    public class Program
    {
        private ErrorHandlerDelegate ErrorHandlerDel { get; set; }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr ErrorHandlerDelegate(IntPtr ptr);

        [DllImport("XemoDll.dll", EntryPoint = "_ML_ErrorCallBack@4", ExactSpelling = false, CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ML_ErrorCallBackDelegate(ErrorHandlerDelegate edel);

        private IntPtr ErrorHandler(IntPtr errNo)
        {
            Console.WriteLine("HandledNew");
            Console.WriteLine($"{(int)errNo} Err num has got");
            return errNo;
        }




        static void Main(string[] args)
        {
            short ax = 1; // 0,1,2

            try
            {
                // var p = new Program();
                var xemoSN = "107376";
                Console.WriteLine(string.Join(' ', xemoSN, ComPortDevId.GetComPortById(xemoSN)));
                // XemoDLL.ML_ErrorCallBack(((int)Handler(new IntPtr(XemoDLL.MB_Get(XemoKonst.m_ErrNo)))));

                // p.ErrorHandlerDel = p.ErrorHandler; // you must save a "copy" of the delegate so that if the C functions calls this method at any time, this copy is still "alive" and hasn't been GC 
                // ML_ErrorCallBackDelegate(p.ErrorHandlerDel);

                XemoDLL.ML_DeIniCom();
                XemoDLL.ML_IniUsb(0, "107376");
                // XemoDLL.ML_IniUsb(0, "114005");
                // XemoDLL.ML_ComSelect(4);
                // XemoDLL.MB_SysControl(XemoKonst.m_Break);
                // XemoDLL.MB_SysControl(XemoKonst.m_Halt);
                // XemoDLL.MB_SysControl(XemoKonst.m_Reset);
                // XemoDLL.MB_ResErr();
                // XemoDLL.MB_Waitinp(12, 1, 1, 1);

                // XemoDLL.ML_SetErrState(XemoKonst.NO_ERR);
                // XemoDLL.MB_SetFifo(XemoKonst.m_FfClear);

                Console.WriteLine($"XemoType: {XemoDLL.MB_Get(1004)}");
                Console.WriteLine($"XemoExtent: {XemoDLL.MB_Get(1007)}");

                Console.WriteLine();

                Console.WriteLine(XemoDLL.ML_GetRcvState());
                Console.WriteLine(XemoDLL.MB_GetState());
                Console.WriteLine(XemoDLL.ML_GetErrState());
                Console.WriteLine(XemoDLL.ML_GetErrCode());

                // InitAxisParam(ax);

                var speed = 5000;
                var keepRunning = true;
                while (keepRunning)
                {
                    var com = Console.ReadLine();

                    switch (com)
                    {
                        case "r":
                            XemoDLL.MB_Jog(1, speed);
                            break;

                        case "l":
                            XemoDLL.MB_Jog(1, -speed);
                            break;

                        case "u":
                            XemoDLL.MB_Jog(0, speed);
                            break;

                        case "d":
                            XemoDLL.MB_Jog(0, -speed);
                            break;

                        case "cw":
                            XemoDLL.MB_Jog(2, speed);
                            break;

                        case "ccw":
                            XemoDLL.MB_Jog(2, -speed);
                            break;

                        case "p":
                            Console.WriteLine($"apos: {XemoDLL.MB_AGet(ax, XemoKonst.m_APos)}");
                            Console.WriteLine($"rpos: {XemoDLL.MB_AGet(ax, XemoKonst.m_RPos)}");
                            Console.WriteLine($"sl lim: {XemoDLL.MB_AGet(ax, XemoKonst.m_SlLimit)}");
                            Console.WriteLine($"sr lim: {XemoDLL.MB_AGet(ax, XemoKonst.m_SrLimit)}");
                            Console.WriteLine($"Vel: {XemoDLL.MB_AGet(ax, XemoKonst.m_Speed)}");
                            Console.WriteLine($"MaxVel: {XemoDLL.MB_AGet(ax, XemoKonst.m_MaxVel)}");
                            //     Console.WriteLine($"IpAccel: {XemoDLL.MB_AGet(ax, XemoKonst.m_IpAccel)}");
                            Console.WriteLine($"Accel: {XemoDLL.MB_AGet(ax, XemoKonst.m_Accel)}");

                            break;
                        case "s":
                            XemoDLL.MB_Stop(0);
                            XemoDLL.MB_Stop(1);
                            XemoDLL.MB_Stop(2);
                            break;
                        case "h":
                            XemoDLL.MB_ASet(ax, XemoKonst.m_SrLimit, 0);
                            XemoDLL.MB_ASet(ax, XemoKonst.m_SlLimit, 0);
                            XemoDLL.MB_ASet(ax, XemoKonst.m_LDecel, 0);
                            XemoDLL.MB_Home(ax);
                            XemoDLL.MB_Still(ax);
                            Console.WriteLine("Still");
                            //while (XemoDLL.MB_IoGet(ax, 0, 2, XemoConst.4000))
                            Console.ReadLine();
                            XemoDLL.MB_ASet(1, XemoKonst.m_APos, -75000);
                            XemoDLL.MB_ASet(1, XemoKonst.m_LDecel, 500000);
                            XemoDLL.MB_ASet(1, XemoKonst.m_SrLimit, 76000);
                            XemoDLL.MB_ASet(1, XemoKonst.m_SlLimit, 0);

                            //     XemoDLL.MB_ASet(0, XemoKonst.m_APos, -37300);
                            //     XemoDLL.MB_ASet(0, XemoKonst.m_LDecel, 500000);
                            //     XemoDLL.MB_ASet(0, XemoKonst.m_SrLimit, 38300);
                            //     XemoDLL.MB_ASet(0, XemoKonst.m_SrLimit, 1500);

                            //     XemoDLL.MB_ASet(2, XemoKonst.m_APos, 0);
                            //     XemoDLL.MB_ASet(2, XemoKonst.m_LDecel, 500000);
                            //     XemoDLL.MB_ASet(2, XemoKonst.m_SrLimit, 38000);
                            //     XemoDLL.MB_ASet(2, XemoKonst.m_SlLimit, -1000);

                            break;
                        case "e":
                            Console.WriteLine(XemoDLL.ML_GetErrState());
                            Console.WriteLine(XemoDLL.ML_GetErrCode());
                            Console.WriteLine(XemoDLL.MB_Get(XemoKonst.m_ErrNo));
                            Console.WriteLine(XemoDLL.MB_Get(XemoKonst.m_ErrAxis));
                            Console.WriteLine(XemoDLL.MB_Get(XemoKonst.m_ErrParam));
                            Console.WriteLine(XemoDLL.MB_Get(XemoKonst.m_SubError));

                            break;
                        case "sw":
                            Console.WriteLine("0_");
                            Console.WriteLine(XemoDLL.MB_IoGet(ax, 1, 0, 4000));
                            Console.WriteLine("1_");
                            Console.WriteLine(XemoDLL.MB_IoGet(ax, 0, 1, 4000));
                            Console.WriteLine("2_");
                            Console.WriteLine(XemoDLL.MB_IoGet(ax, 0, 2, 4000));
                            break;
                        case "slres":
                            XemoDLL.MB_ASet(ax, XemoKonst.m_SlLimit, 0);
                            XemoDLL.MB_ASet(ax, XemoKonst.m_SrLimit, 0);

                            break;

                        case "eres":
                            XemoDLL.MB_SysControl(XemoKonst.m_Reset);
                            XemoDLL.MB_ResErr();

                            break;

                        case "get_params":
                            for (var i = 0; i <= 4003; ++i)
                                Console.WriteLine($"{i};{XemoDLL.MB_Get(i)}");
                            break;


                        case "deini":
                            XemoDLL.ML_DeIniCom();
                            XemoDLL.ML_IniUsb(0, "107376");
                            InitAxisParam(0);
                            InitAxisParam(1);

                            break;

                        case "q":
                            XemoDLL.MB_Stop(0);
                            XemoDLL.MB_Stop(1);
                            keepRunning = false;
                            break;

                        default:
                            XemoDLL.MB_Stop(0);
                            XemoDLL.MB_Stop(1);
                            XemoDLL.MB_Stop(2);
                            break;
                    };

                };


            }
            finally
            {
                Console.WriteLine(XemoDLL.ML_GetErrState());
                // if (XemoDLL.ML_GetErrState() != 0)
                // {
                //     XemoDLL.MB_ResErr();
                //     XemoDLL.MB_SysControl(XemoKonst.m_Reset);

                //     XemoDLL.MB_Stop(ax);
                // }
                XemoDLL.MB_Stop(0);
                XemoDLL.MB_Stop(1);
                XemoDLL.MB_SysControl(XemoKonst.m_Halt);

                Console.WriteLine(XemoDLL.ML_GetErrCode());

                XemoDLL.ML_DeIniCom();
            }
        }

        public static IntPtr Handler(IntPtr errNo)
        {
            // I don't know what ptr is
            Console.WriteLine("Handled");
            Console.WriteLine($"{(int)errNo} Err num has got");
            //     Console.WriteLine(XemoDLL.ML_GetErrState());
            //     Console.WriteLine(XemoDLL.ML_GetErrCode());
            //     Console.WriteLine(XemoDLL.MB_Get(XemoKonst.m_ErrNo));
            //     Console.WriteLine(XemoDLL.MB_Get(XemoKonst.m_ErrAxis));
            //     Console.WriteLine(XemoDLL.MB_Get(XemoKonst.m_ErrParam));
            //     Console.WriteLine(XemoDLL.MB_Get(XemoKonst.m_SubError));

            return errNo; // Return something sensible
        }
        public static void InitAxisParam(int num)
        {
            var XemoType = (long)XemoDLL.MB_Get(1004);
            var XemoExtent = (long)XemoDLL.MB_Get(1007);


            XemoDLL.MB_ASet((short)num, XemoKonst.m_Current, 50);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_StopCurr, 50);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_Micro, 2);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_Uscale, 800);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_Iscale, 10000);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_Speed, 2000);

            XemoDLL.MB_ASet((short)num, XemoKonst.m_Accel, 10000);


            //     XemoDLL.MB_ASet((short)num, XemoKonst.m_IpGroup, 1);
            //     XemoDLL.MB_ASet((short)num, XemoKonst.m_IpSpeed, 1000);
            //     XemoDLL.MB_ASet((short)num, XemoKonst.m_IpAccel, 10000);

            XemoDLL.MB_ASet((short)num, XemoKonst.m_Vmin, 1000);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_LDecel, 10000);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_H1Speed, 2000);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_H2Speed, -100);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_H3Speed, 10000);

            XemoDLL.MB_ASet((short)num, XemoKonst.m_SlLimit, -10000);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_SrLimit, 10000);

            XemoDLL.MB_ASet((short)num, 2056, -2000);
            XemoDLL.MB_ASet((short)num, XemoKonst.m_FErrWin, 200);

            XemoDLL.MB_IoSet((short)num, 0, 3, 4000, (short)POLARITY_SWITCHES[num]);

        }


        private static long[] REF_VELOCITY_H1 = new long[]
                {
                50L, // - down + up
                50L,  // - left + right
                50L,
                100L,
                100L
                };
        private static long[] MAX_VELOCITY = new long[]
        {
                100L,
                100L,
                120L,
                400L,
                400L
        };

        private static long[] TRAVEL_AXIS = new long[]
        {
                400L,
                800L,
                0L,
                100L,
                100L
        };

        private static long[] MICROSTEP_DEFINER = new long[]
        {
                1L,
                1L,
                1L,
                1L,
                1L
        };

        private static long[] INC_PER_REVOLUTION = new long[]
        {
                10000L,
                10000L,
                10000L,
                10000L,
                10000L
        };

        private static float[] MM_PER_REVOLUTION = new float[]
        {
                8f,
                8f,
                14.4f,
                130f,
                130f
        };

        private static long[] INC_MONITORING_ENCODER = new long[]
        {
                2000L,
                -2000L,
                2000L,
                0L,
                130L
        };

        private static long[] MOTOR_CURRENT = new long[]
        {
                60L,
                50L,
                50L,
                100L,
                100L
        };

        private static long[] MOTOR_STOP_CURRENT = new long[]
        {
                70L,
                70L,
                70L,
                70L,
                70L
        };

        private static float[] ZERO_REF_OFFSET = new float[]
        {
                0f,
                0f,
                0f,
                0f,
                0f
        };

        private static float[] NULL_OFFSET = new float[]
        {
                -373f,
                -774f,
                0f,
                0f,
                0f
        };

        private static long[] REF_ORDER = new long[]
        {
                1L,
                2L,
                3L,
                4L,
                5L
        };

        private static long[] REF_PORT = new long[]
        {
                -1L,
                -1L,
                -1L,
                -1L,
                -1L
        };

        private static long[] REF_SWITCH = new long[]
        {
                -1L,
                -1L,
                -1L,
                -1L,
                -1L
        };

        private static long[] POLARITY_SWITCHES = new long[]
        {
                3L,
                3L,
                0L,
                3L,
                3L
        };


        private static long[] REF_VELOCITY_H2 = new long[]
        {
                -1L,
                -1L,
                -1L,
                -30L,
                -30L
        };

        private static long[] REF_VELOCITY_H3 = new long[]
        {
                100L,
                100L,
                100L,
                200L,
                200L
        };

        private static float[] ACCELERATION_FACTOR = new float[]
        {
                10f,
                10f,
                1f,
                10f,
                10f
        };

        private static float[] DECELERATION_FACTOR = new float[]
        {
                10f,
                10f,
                1f,
                10f,
                10f
        };

        private static long[] START_STOP_FREQUENCY = new long[]
        {
                0L,
                0L,
                0L,
                2L,
                2L
        };

        private static float[] EMERGCY_DECEL_FACTOR = new float[]
        {
                10f,
                10f,
                10f,
                10f,
                10f
        };

        private static float[] BLASH = new float[]
        {
                0f,
                0f,
                0f,
                0f,
                0f
        };

        private static long[] NODE_ID = new long[]
        {
                1L,
                2L,
                3L,
                4L,
                5L
        };

        private static long[] POSITION_ERROR = new long[]
        {
                200L,
                200L,
                200L,
                10000L,
                10000L
        };

        private static long[] POSITION_WINDOW = new long[]
        {
                100L,
                100L,
                100L,
                100L,
                100L
        };

        private static long[] POSITION_TIME = new long[]
        {
                0L,
                0L,
                0L,
                0L,
                0L
        };

        private static long[] HOME_METHOD = new long[]
        {
                19L,
                19L,
                19L,
                19L,
                19L
        };

        private static long[] ENCODER_NACHBILDUNG = new long[]
        {
                -1L,
                -1L,
                -1L,
                -1L,
                -1L
        };

        private static long[] ENCODER_RESOLUTION = new long[]
        {
                2000L,
                2000L,
                2000L,
                0L,
                0L
        };

        private static long[] MAX_ENCODER_FEHLER = new long[]
        {
                200L,
                200L,
                200L,
                0L,
                0L
        };

        private static int[] BRAKE = new int[]
        {
                -1,
                -1,
                -1,
                -1,
                -1
        };

        private static int[] INIT_PNOZ_OUT = new int[]
        {
                -1,
                -1,
                -1,
                -1,
                -1
        };

        private static int[] P_FAKTOR_GESCHW = new int[]
        {
                0,
                0,
                0,
                0,
                -1
        };

        private static int[] P_FAKTOR_POSMODUS = new int[]
        {
                0,
                0,
                0,
                0,
                0
        };

        private static int[] KOMMUTIERUNGS_METHODE = new int[]
        {
                3,
                3,
                3,
                3,
                3
        };

        private static long[] GANTRY_ACHSE = new long[]
        {
                0L,
                0L,
                0L,
                0L,
                0L
        };

        private static long[] XTYPE = new long[]
        {
                0L,
                0L,
                0L,
                0L,
                0L
        };

        private static long[] JERKMS = new long[]
        {
                0L,
                0L,
                0L,
                0L,
                0L
        };

    }
}
