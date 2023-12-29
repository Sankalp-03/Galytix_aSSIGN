using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/gwp")]
    public class CountryGwpController : ControllerBase
    {
        private const string FilePath = "./Data/gwpByCountry.csv";

        [HttpPost("avg")]
        public async Task<IActionResult> GetAverageGWP([FromBody] JObject requestData)
        {
            try
            {
                if (requestData == null || !requestData.TryGetValue("country", out var countryToken) || !requestData.TryGetValue("lineOfBusiness", out var lobToken))
                {
                    return BadRequest("Invalid JSON format. Required fields: 'country' and 'lob'.");
                }

                string country = countryToken.ToString();
                var lines = await System.IO.File.ReadAllLinesAsync(FilePath);
                var data = lines.Skip(1)
                                .Where(line => line.StartsWith(country, StringComparison.OrdinalIgnoreCase))
                                .Select(line => line.Split(','))
                                .ToDictionary(
                                    parts => parts[3].ToLower(), // assuming lineOfBusiness is at index 3 in the CSV
                                    parts =>
                                    {
                                        var values = parts.Skip(4).Where(s => !string.IsNullOrEmpty(s)).Select(decimal.Parse).ToArray();
                                        return values.Length > 0 ? values.Sum() / values.Length : 0m; // Calculate average here
                                    }
                                );

                var result = new JObject();
                foreach (var lob in lobToken)
                {
                    string lobKey = lob.ToString().ToLower();
                    if (data.ContainsKey(lobKey))
                    {
                        result[lobKey] = data[lobKey];
                    }
                    else
                    {
                        result[lobKey] = 0; // Or any default value you prefer
                    }
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
