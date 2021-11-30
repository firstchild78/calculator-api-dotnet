using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Calculator.api
{
    public static class Calculate
    {
        [FunctionName("Calculate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Calculate")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Operation operation = JsonConvert.DeserializeObject<Operation>(requestBody);

            int result = 0;
    
            switch (operation.Operator)
            {
                case "+":
                    result = operation.LeftNumber + operation.RightNumber;
                    break;
                case "-":
                    result = operation.LeftNumber - operation.RightNumber;
                    break;
                case "*":
                    result = operation.LeftNumber * operation.RightNumber;
                    break;
                case "/":
                    result = operation.LeftNumber/operation.RightNumber;
                    break;
                default:
                    return new NotFoundResult();
            }

            Result response = new Result { Value = result};
            
            return new OkObjectResult(response);
        }
    }

    public class Operation 
    {
        public int LeftNumber {get;set;}
        public int RightNumber {get;set;}
        public string Operator {get;set;}
    }

    public class Result
    {
        public int Value { get; set; }
    }
}