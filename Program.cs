using CanberraDataAccessLib;
using CanberraDeviceAccessLib;
// using CanberraInteractiveDataFitLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Regata.Core.Hardware;

namespace WorkFlowCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            // DeviceAccess d = null;
            // DataAccess effFile = null;
            // DataAccess engFile = null;
            // DataAccess spctr = null;


            using (var d = new Detector("D1"))
            {
                Console.WriteLine(d.AcquisitionStartDateTime);
            }

            try
            {
                // effFile = new DataAccess();
                // effFile.Open(@"C:\GENIE2K\CALFILES\Efficiency\D1\D1-eff-10.CAL");

                // engFile = new CanberraDataAccessLib.DataAccess();
                // engFile.Open(@"C:\GENIE2K\CALFILES\Efficiency\D1\D1-energy.CAL");

                // spctr = new DataAccess();
                // spctr.Open(@"D:\Spectra\2020\09\kji\1006421.cnf", OpenMode.dReadWrite);

                // d = new DeviceAccess();
                // d.Connect("D1");
                // Console.WriteLine(DateTime.Parse(d.Param[CanberraDeviceAccessLib.ParamCodes.CAM_X_ASTIME].ToString()));
                // d.Param[CanberraDeviceAccessLib.ParamCodes.CAM_F_SSYSTERR] = 20;
                // d.Param[CanberraDeviceAccessLib.ParamCodes.CAM_F_SSYSTERR] = 30;
                // d.Param[CanberraDeviceAccessLib.ParamCodes.CAM_T_SGEOMTRY] = "2.5";
                // Console.WriteLine(effFile.Calibrations.Mass);
                // effFile.CopyBlock(spctr, CanberraDataAccessLib.ClassCodes.CAM_CLS_GEOM);
                // engFile.CopyBlock(spctr, CanberraDataAccessLib.ClassCodes.CAM_CLS_SHAPECALRES);
                // engFile.CopyBlock(spctr, CanberraDataAccessLib.ClassCodes.CAM_CLS_CALRESULTS);

                // // engFile.CopyBlock(d, CanberraDataAccessLib.ClassCodes.CAM_CLS_SHAPECALRES);
                // // engFile.CopyBlock(d, CanberraDataAccessLib.ClassCodes.CAM_CLS_CALRESULTS);
                // // effFile.CopyBlock(d, CanberraDataAccessLib.ClassCodes.CAM_CLS_GEOM);
                // spctr.Save("");
                // d.Save("");

            }
            finally
            {
                // d?.Disconnect();
                // if (effFile != null && effFile.IsOpen)
                //     effFile.Close();

                // if (spctr != null && spctr.IsOpen)
                //     spctr.Close();

            }

        }

    }
}
