using Microsoft.AspNetCore.Mvc;
using JiraMetrics.Models;
using System;


namespace JiraMetrics.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JiraIssueController : ControllerBase
    {
        private readonly JiraHttpClient _jiraHttpClient;

        public JiraIssueController(JiraHttpClient jiraHttpClient)
        {
            _jiraHttpClient = jiraHttpClient;
        }
        //Данный метод контроллера выдает issues пользователей за заданный период в зависимости от различных комбинаций projectId, groupName, assignee
        [HttpGet]
        public async Task<ActionResult<ResponseUserIssue>> GetUserIssues(string? projectId, string? groupName, string? assignee,  DateTime startDate, DateTime endDate)
        {
            string requestString = $"search?jql=assignee is not null AND status=Closed AND (labels is EMPTY OR labels not in (RND)) ";

            if (projectId != null && assignee != null && groupName != null)
            {
                requestString += $"AND project={projectId} AND assignee={assignee} AND assignee in membersOf('{groupName}') ";
            }
            else if (projectId != null && assignee == null && groupName == null)
            {
                requestString += $"AND project={projectId} ";
            }
            else if (projectId == null && assignee != null && groupName == null)
            {
                requestString += $"AND assignee={assignee} ";
            }
            else if (projectId == null && assignee == null && groupName != null)
            {
                requestString += $"AND assignee in membersOf('{groupName}') ";
            }
            else if (projectId != null && assignee != null && groupName == null)
            {
                requestString += $"AND project={projectId} AND assignee={assignee} ";
            }
            else if (projectId != null && assignee == null && groupName != null)
            {
                requestString += $"AND project={projectId} AND assignee in membersOf('{groupName}') ";
            }
            else if (projectId == null && assignee != null && groupName != null)
            {
                requestString += $"AND assignee={assignee} AND assignee in membersOf('{groupName}') ";
            }

            //Добавляем в запрос период
            requestString += $"AND updated >='{startDate.ToString("yyyy-MM-dd")}' AND updated <='{endDate.ToString("yyyy-MM-dd")}'";

            //Какие поля нужны
            requestString += "&fields=summary,timespent,timeoriginalestimate,description,assignee&maxResults=1000";

            var response = await _jiraHttpClient.Get(requestString);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Запрос не выполнен с кодом состояния {response.StatusCode}: {content}");
            }

            var result = await response.Content.ReadAsAsync<ResponseJiraIssue>();

            //return Ok(result);

            var users = new Dictionary<string, ResponseUserIssue>();

            foreach (var issue in result.Issues)
            {
                var user = issue.Fields.Assignee;
                if (!users.ContainsKey(user.Name))
                {
                    users[user.Name] = new ResponseUserIssue { Assignee = user, Issues = new List<IssueDto>() };
                }
                users[user.Name].Issues.Add(new IssueDto
                {
                    Id = issue.Id,
                    Self = issue.Self,
                    Key = issue.Key,
                    Summary = issue.Fields.Summary,
                    Description = issue.Fields.Description,
                    TimeSpent = issue.Fields.TimeSpent ?? 0,
                    TimeOriginalEstimate = issue.Fields.TimeOriginalEstimate ?? 0
                });
            }

            return Ok(users);
        }
    }
}

