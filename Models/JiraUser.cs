

namespace JiraMetrics.Models
{
  
    public class JiraUser
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
        public Dictionary<string, string> AvatarUrls { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }  
    }


}

