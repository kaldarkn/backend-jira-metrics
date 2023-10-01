using Microsoft.AspNetCore.Mvc;
using JiraMetrics.Models;


namespace JiraMetrics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraUserController : ControllerBase
    {
        private readonly JiraHttpClient _jiraHttpClient;

        public JiraUserController(JiraHttpClient jiraHttpClient)
        {
            _jiraHttpClient = jiraHttpClient;
        }


        [HttpGet]
        public async Task<ActionResult<List<JiraUser>>> GetUsers()
        {
            List<JiraUser> allUsers = new List<JiraUser>();

            //Jira выдаёт только 100 пользователей на запрос, поэтому делаем два запроса
            for (int i = 0; i < 2; i++)
            {
                string requestString = $"user/search?username=''&maxResults=100&startAt={100 * i}";

                var response = await _jiraHttpClient.Get(requestString);

                if (!response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Запрос не выполнен с кодом состояния {response.StatusCode}: {content}");
                }

                var result = await response.Content.ReadAsAsync<List<JiraUser>>();

                allUsers.AddRange(result);
            }

            return Ok(allUsers);
        }


        [HttpGet("username")]
        public async Task<ActionResult<JiraUser>> GetUserByName(string username)
        {
            string requestString = $"user?username={username}";

            var response = await _jiraHttpClient.Get(requestString);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Запрос не выполнен с кодом состояния {response.StatusCode}: {content}");
            }

            var result = await response.Content.ReadAsAsync<JiraUser>();
            return Ok(result);
        }
    }
}