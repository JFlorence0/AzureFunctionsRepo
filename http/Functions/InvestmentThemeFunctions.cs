using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using http.context;
using MySqlConnector;
using System.Collections.Generic;

namespace http.Functions
{
    public class InvestmentThemesFunction
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InvestmentThemesFunction> _logger;

        public InvestmentThemesFunction(ApplicationDbContext context, ILogger<InvestmentThemesFunction> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Fetch all InvestmentTheme rows using raw SQL, selecting all columns dynamically
        /// </summary>
        [Function("GetInvestmentThemes")]
        public async Task<IActionResult> GetInvestmentThemes(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "investment-themes")] HttpRequestData req)
        {
            _logger.LogInformation("Fetching all investment themes using raw SQL...");

            var connectionString = _context.Database.GetConnectionString();
            using var connection = new MySqlConnection(connectionString);
            await connection.OpenAsync();

            var command = new MySqlCommand("SELECT * FROM investmentthemes", connection);
            using var reader = await command.ExecuteReaderAsync();

            var themes = new List<Dictionary<string, object>>();
            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    object value = reader.IsDBNull(i) ? "NULL" : reader.GetValue(i); // Explicitly handle nulls
                    row[columnName] = value;
                }

                themes.Add(row);
            }

            return new OkObjectResult(themes);
        }
    }
}