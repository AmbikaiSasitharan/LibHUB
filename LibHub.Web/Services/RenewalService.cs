using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class RenewalService : IRenewalService
    {
        private readonly HttpClient httpClient;

        public RenewalService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<RenewalDetailsDTO> GetRenewal(int Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Renewal/GetRenewalGivenRenewalId/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(RenewalDetailsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<RenewalDetailsDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {
                //Log Exception
                throw;
            }
        }

        public async Task<RenewalDetailsDTO> AddRenewal(int borrowId)
        {
            var response = await httpClient.PostAsJsonAsync<int>($"api/Renewal/AddRenewalGivenBorrowId/{borrowId}", borrowId);
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(RenewalDetailsDTO);
                }
                return await response.Content.ReadFromJsonAsync<RenewalDetailsDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async Task<RenewalDetailsDTO> RemoveRenewal(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Renewal/RemoveRenewalGivenId/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RenewalDetailsDTO>();
                }
                return default(RenewalDetailsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<LogEntryDTO>> GetLogHistory(int userId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Renewal/GetLogHistoryGivenUserId/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<LogEntryDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<LogEntryDTO>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception)
            {
                //Log Exception
                throw;
            }
        }
    }
}
