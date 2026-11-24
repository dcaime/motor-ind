namespace EvaluaIndividuos
{
    public partial class api_oauth_access_v1_0Client
    {
        public void SetBasicAuthentication(string base64Credentials)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64Credentials);
        }
    }
}