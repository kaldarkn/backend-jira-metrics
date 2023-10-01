using Microsoft.AspNetCore.Mvc;
using JiraMetrics.Models;

namespace JiraMetrics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraProjectController : ControllerBase
    {
        private readonly JiraHttpClient _jiraHttpClient;

        public JiraProjectController(JiraHttpClient jiraHttpClient)
        {
            _jiraHttpClient = jiraHttpClient;
        }

        [HttpGet]
        public async Task<ActionResult<List<JiraProject>>> Get()
        {

            var response = await _jiraHttpClient.Get("project");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Запрос не выполнен с кодом состояния {response.StatusCode}: {content}");
            }

            var result = await response.Content.ReadAsAsync<List<JiraProject>>();
            return Ok(result);
        }
    }
}