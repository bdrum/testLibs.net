using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Gxemo;
using Regata.Core.Hardware;

namespace Client
{

    class Program
    {

        public static IReadOnlyDictionary<PutSampleAboveDetReply.Types.Height, Heights> HH = new Dictionary<PutSampleAboveDetReply.Types.Height, Heights>()
        {
            { PutSampleAboveDetReply.Types.Height.H2P5, Heights.h2p5 },
            { PutSampleAboveDetReply.Types.Height.H5,   Heights.h5   },
            { PutSampleAboveDetReply.Types.Height.H10,  Heights.h10  },
            { PutSampleAboveDetReply.Types.Height.H20,  Heights.h20  }
        };
        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            var devId = int.Parse(args[0]);

            using (var sc = new SampleChanger(devId))
            {

                var client = new Xemo.XemoClient(channel);

                var ct = new CancellationTokenSource(TimeSpan.FromMinutes(5));

                await sc.HomeAsync(ct.Token);

                while (!ct.IsCancellationRequested)
                {
                    var ts = await client.DeviceIsReadyAsync(new DeviceIsReadyRequest { DevId = devId, IsReady = true });

                    await sc.TakeSampleFromTheCellAsync((short)ts.CellNum, ct.Token);

                    var gotodeth = await client.SampleHasTakenAsync(new SampleHasTakenRequest { DevId = devId, IsTaken = true });

                    await sc.PutSampleAboveDetectorWithHeightAsync(HH[gotodeth.H], ct.Token);

                    var takeSample = await client.SampleAboveDetectorAsync(new SampleAboveDetectorRequest { DevId = devId, IsAbove = true });

                    await sc.PutSampleToTheDiskAsync((short)ts.CellNum, ct.Token);

                    var tsNext = await client.SampleInCellAsync(new SampleInCellRequest { DevId = devId, IsInCell = true });
                }

            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
