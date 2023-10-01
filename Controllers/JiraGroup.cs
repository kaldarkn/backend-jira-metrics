using Microsoft.AspNetCore.Mvc;
using JiraMetrics.Models;

namespace JiraMetrics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraGroupController : ControllerBase
    {
        private readonly JiraHttpClient _jiraHttpClient;

        public JiraGroupController(JiraHttpClient jiraHttpClient)
        {
            _jiraHttpClient = jiraHttpClient;
        }

        [HttpGet]
        public async Task<ActionResult<JiraGroupList>> Get()
        {

            var response = await _jiraHttpClient.Get("groups/picker?maxResults=1000");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Запрос не выполнен с кодом состояния {response.StatusCode}: {content}");
            }

            var result = await response.Content.ReadAsAsync<JiraGroupList>();
            return Ok(result.Groups);
        }
    }
}