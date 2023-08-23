using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly HttpClient httpClient;

        public BorrowService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<BorrowDetailsDTO> AddBorrow(BorrowToAddDTO borrowToAddDTO)
        {
            var response = await httpClient.PostAsJsonAsync<BorrowToAddDTO>("api/Borrow/AddBorrow", borrowToAddDTO);
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(BorrowDetailsDTO);
                }
                return await response.Content.ReadFromJsonAsync<BorrowDetailsDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async Task<BorrowDetailsDTO> RemoveBorrow(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Borrow/RemoveBorrowGivenBorrowId/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BorrowDetailsDTO>();
                }
                return default(BorrowDetailsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BorrowDetailsDTO> ReturnBorrow(int id, BorrowDetailsDTO borrowDetailsDTO)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<BorrowDetailsDTO>($"api/Borrow/ReturnBorrowGivenBorrowId/{id}", borrowDetailsDTO);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BorrowDetailsDTO>();
                }
                return default(BorrowDetailsDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BorrowDetailsDTO> GetBorrow(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Borrow/GetBorrowGivenBorrowId/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(BorrowDetailsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<BorrowDetailsDTO>();
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

        public async Task<IEnumerable<BorrowDetailsDTO>> GetBorrowHistoryOfAUser(int userId1)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Borrow/GetAllBorrowsGivenUserId/{userId1}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<BorrowDetailsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<BorrowDetailsDTO>>();
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

        public async Task<List<BorrowDetailsDTO>> GetCurrentBorrowsOfAUser(int userId2)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Borrow/GetAllCurrentBorrowsGivenUserId/{userId2}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<BorrowDetailsDTO>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<BorrowDetailsDTO>>();
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

        public async Task<IEnumerable<BorrowDetailsDTO>> GetAllBorrowsNotReturned()
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Borrow/GetAllBorrowsNotReturned");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<BorrowDetailsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<BorrowDetailsDTO>>();
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

        public async void UpdateBorrowIsLateNotified(int borrowId)
        {
            var response = await httpClient.PutAsync($"api/Borrow/UpdateBorrowIsLateNotified/{borrowId}", null);

            if (response.IsSuccessStatusCode)
            {

            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async void UpdateBorrowIsFineNotified(int borrowId)
        {
            var response = await httpClient.PutAsync($"api/Borrow/UpdateBorrowIsFineNotified/{borrowId}", null);

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
