using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;

namespace MyWebApi.Controllers
{
    public class CalculationRequest
    {
        public string Input { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        [HttpPost]
        public IActionResult Calculate([FromBody] CalculationRequest request)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(request.Input))
                {
                    return BadRequest("Input cannot be null or empty.");
                }

                // Define a regular expression to parse the input
                var match = Regex.Match(request.Input, @"(\d+)([+\-*/])(\d+)");
                if (!match.Success)
                {
                    return BadRequest("Invalid input format. Use format like '123+456'.");
                }

                // Extract the operands and operator
                var operand1 = int.Parse(match.Groups[1].Value);
                var operation = match.Groups[2].Value;
                var operand2 = int.Parse(match.Groups[3].Value);

                // Perform the calculation
                double result = operation switch
                {
                    "+" => operand1 + operand2,
                    "-" => operand1 - operand2,
                    "*" => operand1 * operand2,
                    "/" => operand2 != 0 ? (double)operand1 / operand2 : throw new DivideByZeroException(),
                    _ => throw new InvalidOperationException("Invalid operation.")
                };

                // Return the result
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}


