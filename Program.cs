using CanberraDataAccessLib;
using CanberraDeviceAccessLib;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Regata.Core.Hardware;
using Regata.Core.DataBase;
using Regata.Core.DataBase.Models;
using Microsoft.PowerShell;
using System.Management.Automation;

namespace WorkFlowCore
{
    public class Program
    {

        static void Main(string[] args)
        {

            DeviceAccessClass d = null;
            DataAccessClass f = null;
            try
            {
                d = new DeviceAccessClass();
                d.Connect("D2");
                // f = new DataAccessClass();
                // f.Open(@"C:\GENIE2K\empty_spectra.cnf");
                // d.Param[ParamCodes.CAM_L_PRCLRB4ACQ] = true;
                // d.Param[ParamCodes.CAM_T_STITLE] = "";
                // d.Param[ParamCodes.CAM_T_SCOLLNAME] = "";
                // d.Param[ParamCodes.CAM_T_SIDENT] = "";
                // d.Param[ParamCodes.CAM_F_SQUANT] = 0;
                // d.Param[ParamCodes.CAM_F_SQUANTERR] = null;
                // d.Param[ParamCodes.CAM_T_SUNITS] = null;
                // d.Param[ParamCodes.CAM_F_SSYSERR] = null;
                // d.Param[ParamCodes.CAM_F_SSYSTERR] = null;
                // d.Param[ParamCodes.CAM_T_STYPE] = null;
                // d.Param[ParamCodes.CAM_T_BUILDUPTYPE] = string.Empty;
                // d.Param[ParamCodes.CAM_X_SDEPOSIT] = Encoding.UTF8.GetBytes(string.Empty).ToString();
                // d.Param[ParamCodes.CAM_X_STIME] = Encoding.UTF8.GetBytes(string.Empty);
                // d.Param[ParamCodes.CAM_T_SGEOMTRY] = string.Empty;
                f.CopyBlock(d, CanberraDataAccessLib.ClassCodes.CAM_CLS_SAMP);
                d.Save("", true);
            }
            finally
            {
                d?.Disconnect();
                f?.Close();
            }
        }

    }
}
