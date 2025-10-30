using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobCardBackend.Models;
using Newtonsoft.Json;

namespace JobCardBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        public JobController(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobCardDto jobDto)
        {
            if (jobDto == null)
                return BadRequest("Job details missing.");

            var user = await _db.Users.FindAsync(jobDto.UserId);
            if (user == null)
                return BadRequest($"User with ID {jobDto.UserId} not found.");
            try
            {
                var job = new JobCard
                {
                    ClientName = jobDto.ClientName,
                    JobName = jobDto.JobName,
                    OrderNo = jobDto.OrderNo,
                    PaperDetails = jobDto.PaperDetails,
                    JobSize = jobDto.JobSize,
                    Processes = JsonConvert.SerializeObject(jobDto.Processes),
                    JobDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(3),
                    UserId = jobDto.UserId
                };

                _db.JobCards.Add(job);
                await _db.SaveChangesAsync();

                return Ok(new
                {
                    message = "Job card created successfully!",
                    job
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving job: {ex.Message}");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateJob(int id, [FromBody] List<string> updatedProcesses)
        {
            var job = await _db.JobCards.FindAsync(id);
            if (job == null) return NotFound("Job not found.");
            Dictionary<string, string> processDict;
            try
            {
                processDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(job.Processes)
                              ?? new Dictionary<string, string>();
            }
            catch
            {
                var arr = System.Text.Json.JsonSerializer.Deserialize<List<string>>(job.Processes)
                          ?? new List<string>();
                processDict = arr.ToDictionary(p => p, p => "Pending");
            }

            foreach (var process in updatedProcesses)
            {
                var key = process.Trim();
                if (processDict.ContainsKey(key))
                    processDict[key] = "Completed";
            }

            job.Processes = System.Text.Json.JsonSerializer.Serialize(processDict);
            await _db.SaveChangesAsync();
            return Ok(job);
        }


        [HttpGet("reports")]
        public async Task<IActionResult> GetReports()
        {
            int userId = Convert.ToInt32(Request.Query["userId"]);
            var jobs = await _db.JobCards
                .Where(j => j.UserId == userId)
                .ToListAsync();

            var result = jobs.Select(j =>
            {
                Dictionary<string, string> processDict;
                try
                {
                    processDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(j.Processes)
                                  ?? new Dictionary<string, string>();
                }
                catch
                {
                    var arr = System.Text.Json.JsonSerializer.Deserialize<List<string>>(j.Processes)
                              ?? new List<string>();
                    processDict = arr.ToDictionary(p => p, p => "Pending");
                }

                return new
                {
                    j.Id,
                    j.ClientName,
                    j.JobName,
                    j.JobDate,
                    j.DeliveryDate,
                    Processes = processDict
                };
            });

            return Ok(result);
        }

        public class ChatRequest
        {
            public string UserMessage { get; set; }
        }

        [HttpPost("chat")]
        public async Task<IActionResult> Chat([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.UserMessage))
                return BadRequest("User message missing.");

            var userMessage = request.UserMessage;
             var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
                return StatusCode(500, "API key missing in environment variables.");


            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var systemPrompt = @"
You are the AI brain of 'Indas Printing Chatbot'.
Your job is to:
1. Understand user's intent even if they type in Hindi, English, or Hinglish (mixed).
2. Auto-correct spelling mistakes and normalize the command.
3. Always return a clean, simple English action keyword that the bot can handle.

Possible actions:
- If user means 'create job card' (e.g., 'job card banao', 'new job', 'create job', 'bnao job') → respond exactly: 'create job card'
- If user means 'update status' (e.g., 'update karo', 'job update', 'statas updet') → respond exactly: 'update status'
- If user means 'view report' (e.g., 'report dikhao', 'vew repot', 'show report') → respond exactly: 'view report'
- Otherwise, reply with a short, friendly English sentence.

Your reply should be plain text only.
";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
            new { role = "system", content = systemPrompt },
            new { role = "user", content = userMessage }
        },
                temperature = 0.3
            };

            var content = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonResponse = System.Text.Json.JsonDocument.Parse(responseString);
            var aiMessage = jsonResponse.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            var cleanMessage = aiMessage?.Trim().ToLower();

            return Ok(cleanMessage);
        }


    }
}
