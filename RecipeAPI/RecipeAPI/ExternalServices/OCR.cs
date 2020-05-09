using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

namespace PantryTracker.ExternalServices
{
    public interface IOCRService
    {
        Task<IEnumerable<string>> ImageToText(string imageData);
    }

    public class OCR : IOCRService
    {
        private const int MaxRetries = 10;
        private const int DelayInMs = 1000;

        private readonly string _ocrKey = Environment.GetEnvironmentVariable("MSCognitiveServicesKey", EnvironmentVariableTarget.Process);
        private readonly string _ocrEndpoint = Environment.GetEnvironmentVariable("MSCognitiveServicesEndpoint", EnvironmentVariableTarget.Process);
        private ComputerVisionClient _client;

        public OCR()
        {
            _client = GetClient();
        }

        // TODO: Find better way than waiting in a loop.
        public async Task<IEnumerable<string>> ImageToText(string imageData)
        {
            ReadOperationResult textResult = null;
            var operationId = await SubmitRequest(imageData);
            int i = 0;
            while ((textResult == null ||
                    textResult.Status == TextOperationStatusCodes.Running ||
                    textResult.Status == TextOperationStatusCodes.NotStarted) && i++ < MaxRetries)
            {
                await Task.Delay(DelayInMs);
                textResult = await _client.GetReadOperationResultAsync(operationId);
            }

            var result = textResult?.RecognitionResults?.FirstOrDefault();
            var lines = result?.Lines ?? new List<Line>();
            lines = ReadVerticalColumns(lines.ToList(), result?.Width ?? 0D);
            return (lines.Select(l => l.Text));
        }

        private async Task<string> SubmitRequest(string imageData)
        {
            BatchReadFileInStreamHeaders result = null;
            string operationId = string.Empty;

            using (var mstrm = new System.IO.MemoryStream(Convert.FromBase64String(imageData)))
            {
                result = await _client.BatchReadFileInStreamAsync(mstrm);
                var location = result.OperationLocation;
                operationId = location.Substring(location.Length - 36);
            }

            return operationId;
        }

        private ComputerVisionClient GetClient()
        {
            var creds = new ApiKeyServiceClientCredentials(_ocrKey);
            return new ComputerVisionClient(creds, new DelegatingHandler[] { })
            {
                Endpoint = _ocrEndpoint
            };
        }

        private List<Line> ReadVerticalColumns(List<Line> lines, double width)
        {
            var tolerance = Math.Max(width * .1, 1);
            Dictionary<double, List<Line>> groups = new Dictionary<double, List<Line>>();

            while(lines.Any())
            {
                var x = lines.First().BoundingBox[0];
                var xCols = lines.Where(l => (x - tolerance) <= l.BoundingBox[0] && l.BoundingBox[0] <= (x + tolerance));
                groups.Add(x, new List<Line>(xCols));
                lines.RemoveAll(p => xCols.Contains(p));
            }

            var sortedGroups = groups.OrderBy(g => g.Key);
            lines = sortedGroups.SelectMany(g => g.Value)
                                .ToList();

            return lines;
        }
    }
}
