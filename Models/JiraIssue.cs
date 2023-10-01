namespace JiraMetrics.Models
{

    public class JiraIssueFields
    {
        public string Summary { get; set; }
        public string? Description { get; set; }
        public JiraUser Assignee { get; set; }
        public double? TimeSpent { get; set; }
        public double? TimeOriginalEstimate { get; set; }

    }

    public class JiraIssue
    {
        public string Id { get; set; }
        public string Self { get; set; }
        public string Key { get; set; }
        public JiraIssueFields Fields { get; set; }

    }


    public class IssueDto
    {
        public string Id { get; set; }
        public string Self { get; set; }
        public string Key { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public double? TimeSpent { get; set; }
        public double? TimeOriginalEstimate { get; set; }
    }


    public class ResponseJiraIssue
    {
        //public string Expand { get; set; }
        //public double StartAt { get; set; }
        //public double MaxResults { get; set; }
        //public double Total { get; set; }
        public List<JiraIssue> Issues { get; set; }
    }

    public class ResponseUserIssue
    {
        public JiraUser Assignee { get; set; }
        public List<IssueDto> Issues { get; set; }
    }
}


