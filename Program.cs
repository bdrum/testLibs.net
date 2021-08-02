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
            DeviceAccess d = null;
            DataAccess effFile = null;
            DataAccess spctr = null;

            try
            {
                effFile = new DataAccess();
                effFile.Open(@"C:\GENIE2K\CALFILES\Efficiency\D1\D1-eff-10.CAL");

                // spctr = new DataAccess();
                // spctr.Open(@"D:\Spectra\2020\09\kji\1006421.cnf", OpenMode.dReadWrite);

                d = new DeviceAccess();
                d.Connect("D1");
                // d.Param[CanberraDeviceAccessLib.ParamCodes.CAM_F_SQUANTERR] = 10;
                // d.Param[CanberraDeviceAccessLib.ParamCodes.CAM_F_SSYSTERR] = 20;
                // d.Param[CanberraDeviceAccessLib.ParamCodes.CAM_F_SSYSTERR] = 30;
                // d.Param[CanberraDeviceAccessLib.ParamCodes.CAM_T_SGEOMTRY] = "2.5";
                // Console.WriteLine(effFile.Calibrations.Mass);
                effFile.CopyBlock(d, CanberraDataAccessLib.ClassCodes.CAM_CLS_GEOM);

            }
            finally
            {
                // d?.Disconnect();
                if (effFile != null && effFile.IsOpen)
                    effFile.Close();

                if (spctr != null && spctr.IsOpen)
                    spctr.Close();
            }

        }

    }
}
