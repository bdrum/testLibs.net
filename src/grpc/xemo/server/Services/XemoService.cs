using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Gxemo;


namespace Server
{


    public class XemoService : Xemo.XemoBase
    {
        private int _currentCell = 0;
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
            _currentCell++;
            Console.WriteLine($"Start movement to cell {_currentCell} one from device {request.DevId}");
            var t = new TakeSampleFromCellReply { CellNum = _currentCell };
            return Task.FromResult(t);
        }


        public override Task<PutSampleAboveDetReply> SampleHasTaken(SampleHasTakenRequest request, ServerCallContext context)
        {
            if (!request.IsTaken)
            {
                Console.WriteLine("Device was not taken");
                return null;
            }
            var t = new PutSampleAboveDetReply { H = PutSampleAboveDetReply.Types.Height.H20 };
            return Task.FromResult(t);
        }

        public override async Task<PutSampleToDiskReply> SampleAboveDetector(SampleAboveDetectorRequest request, ServerCallContext context)
        {
            if (!request.IsAbove)
            {
                Console.WriteLine("Device is not above detector");
                return null;
            }

            Console.WriteLine($"Device '{request.DevId}' has wainting acquisition...");
            await Task.Delay(TimeSpan.FromSeconds(5));
            Console.WriteLine($"Device '{request.DevId}' acquisition has done. Go to cell {_currentCell}");

            var t = new PutSampleToDiskReply { CellNum = _currentCell };

            return await Task.FromResult(t);
        }



        public override Task<TakeSampleFromCellReply> SampleInCell(SampleInCellRequest request, ServerCallContext context)
        {
            if (!request.IsInCell)
            {
                Console.WriteLine("Device is not in the cell");
                return null;
            }
            var t = new TakeSampleFromCellReply { CellNum = _currentCell };
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
