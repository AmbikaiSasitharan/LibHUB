using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient httpClient;

        public EmailService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async void SendEmail(EmailDTO request)
        {
            var response = await httpClient.PostAsJsonAsync<EmailDTO>("api/Email", request);

            if (response.IsSuccessStatusCode)
            {
                
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }
    }
}
