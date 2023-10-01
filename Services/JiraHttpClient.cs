using System.Net.Http.Headers;
using System.Text;

    public class JiraHttpClient
    {
        private readonly HttpClient _httpClient;

        private string user = "login";
        private string password = "password";

    public JiraHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("Сюда передай uri jira");
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authValue);
       
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            return await _httpClient.GetAsync(url);
        }
    }
