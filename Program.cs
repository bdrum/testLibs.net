using System;
using System.Linq;
using System.Threading;
// using CanberraDataAccessLib;
using CanberraDeviceAccessLib;
//using CanberraInteractiveDataFitLib;

namespace WorkFlowCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            // the most probably this library can't work on linux, because 
            // devices can be connected to windows only
            // this means .net core doesn't have a sense, but anyway
            // good that i can use it even for windows only!
            // DataAccess dev = null;
            DeviceAccess det = null;
            try
            {
                // dev = new DataAccess();
                det = new DeviceAccess();
                det.Connect("D1");
                Console.WriteLine(det.Param[ParamCodes.CAM_T_SGEOMTRY].ToString());
                det.Param[ParamCodes.CAM_T_SGEOMTRY] = "120";
                det.Save("", true);
                // dev.Open(@"D:\Spectra\2020\03\dji-1\1108002.cnf");

                Console.WriteLine(det.Param[ParamCodes.CAM_T_SGEOMTRY].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // dev.Close();
                det.Disconnect();

            }

            Console.ReadLine();
        }

    }
}
