using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Gxemo;


namespace Server
{
    public class XemoService : Xemo.XemoBase
    {
        private readonly ILogger<XemoService> _logger;
        public XemoService(ILogger<XemoService> logger)
        {
            _logger = logger;
        }


        public override Task<TakeSampleFromCellReply> DeviceIsReady(DeviceIsReadyRequest request, ServerCallContext context)
        {
            if (!request.IsReady)
            {
                Console.WriteLine("Device is not ready");
                return null;
            }
            Console.WriteLine($"Start movement to position one from device {request.DevId}");
            var t = new TakeSampleFromCellReply { CellNum = 1 };
            return Task.FromResult(t);
        }


        public override Task<PutSampleAboveDetReply> SampleHasTaken(SampleHasTakenRequest request, ServerCallContext context)
        {
            if (!request.IsTaken)
            {
                Console.WriteLine("Device was not taken");
                return null;
            }
            var t = new PutSampleAboveDetReply { H = PutSampleAboveDetReply.Types.Height.H2P5 };
            return Task.FromResult(t);
        }

        public override Task<PutSampleToDiskReply> SampleAboveDetector(SampleAboveDetectorRequest request, ServerCallContext context)
        {
            if (!request.IsAbove)
            {
                Console.WriteLine("Device is not above detector");
                return null;
            }
            var t = new PutSampleToDiskReply { CellNum = 1 };
            return Task.FromResult(t);
        }

        public override Task<TakeSampleFromCellReply> PutSampleToDisk(SampleInCellRequest request, ServerCallContext context)
        {
            if (!request.IsInCell)
            {
                Console.WriteLine("Device is not in the cell");
                return null;
            }
            var t = new TakeSampleFromCellReply { CellNum = 2 };
            return Task.FromResult(t);
        }

        public override Task<ErrorOccurredReply> DeviceError(ErrorOccurredRequest request, ServerCallContext context)
        {
            Console.WriteLine($"Error code '{request.ErrCode}' has getting from device '{request.DevId}'");

            var t = new ErrorOccurredReply();
            return Task.FromResult(t);
        }


    }
}
