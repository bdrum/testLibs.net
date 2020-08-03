using System;
using CanberraDataAccessLib;
using CanberraDataAccessLib;
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
      IDataAccess dev = null;
      //IIDataFit iDataFit = null;
      try
      {
        dev = new DataAccess();

        //iDataFit = new IDFClass();


        //iDataFit.PlotEx(dev.Calibrations.Energy.Points, ModelType.idfPolynomial, "pol", 1, DataPtSymbol.idfPlus, dev.Calibrations.Energy.Curve);


        dev.Open(@"D:\Spectra\2020\01\dji-1\7107916.cnf");
        //Console.WriteLine(dev.Param[ParamCodes.]);


        Console.WriteLine("Number\tChannel\tEnergy\tError");

        for (int i = 0; i < 11; ++i)
        {
          float ch = ((Single[,])dev.Calibrations.Energy.Points)[0, i];
          float energy = ((Single[,])dev.Calibrations.Energy.Points)[1, i];
          float err = ((Single[,])dev.Calibrations.Energy.Points)[2, i];

          Console.WriteLine($"{i}\t{ch}\t{energy}\t{err}");
        }


        Console.WriteLine("Number\tEnergy\tEff\tError\tDensity");

        for (int i = 0; i < 26; ++i)
        {
          float energy = ((Single[,])dev.Calibrations.Efficiency.EfficiencyPoints[1])[0, i];
          float eff = ((Single[,])dev.Calibrations.Efficiency.EfficiencyPoints[1])[1, i];
          float err = ((Single[,])dev.Calibrations.Efficiency.EfficiencyPoints[1])[2, i];
          float dnst = ((Single[,])dev.Calibrations.Efficiency.EfficiencyPoints[1])[3, i];

          Console.WriteLine($"{i}\t{energy}\t{eff}\t{err}\t{dnst}");

        }

        for (int i = 0; i < 7; ++i)
        {
          float energy = ((Single[,])dev.Calibrations.MatrixCorrection.Points)[0, i];
          //float eff = ((Single[,])dev.Calibrations.Efficiency.EmpiricalCurve[1])[1, i];
          //float err = ((Single[,])dev.Calibrations.Efficiency.EmpiricalCurve[1])[2, i];
          //float dnst = ((Single[,])dev.Calibrations.Efficiency.EmpiricalCurve[1])[3, i];

          Console.WriteLine($"{i}\t{energy}");

        }

        //Console.WriteLine(dev.Param[ParamCodes.CAM_F_DENERGY]);
        //Console.WriteLine(dev.Param[ParamCodes.CAM_F_DEFF]);
        //Console.WriteLine(dev.Param[ParamCodes.CAM_F_DEFFERR]);
        //Console.WriteLine(dev.Param[ParamCodes.CAM_F_DEFFEFAC]);

        //Console.WriteLine(dev.Param[ParamCodes.CAM_T_EFFCALFILE]);
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
        dev?.Close();
      }
      finally
      {
        dev?.Close();
      }

      Console.ReadLine();
    }

  }
}
