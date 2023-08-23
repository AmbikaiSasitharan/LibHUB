using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class RatingService : IRatingService
    {
        private readonly HttpClient httpClient;

        public RatingService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<RatingDetailsDTO> GetRating(int Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Rating/GetRatingGivenRatingId/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(RatingDetailsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<RatingDetailsDTO>();
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

        public async Task<IEnumerable<RatingDetailsDTO>> GetRatingForBookDescription(int descriptionId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Rating/GetAllRatingsGivenBookDescriptionId/{descriptionId}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<RatingDetailsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<RatingDetailsDTO>>();
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

        public async Task<RatingDetailsDTO> AddRating(RatingToAddDTO ratingToAddDTO)
        {
            var response = await httpClient.PostAsJsonAsync<RatingToAddDTO>("api/Rating/AddRating", ratingToAddDTO);
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(RatingDetailsDTO);
                }
                return await response.Content.ReadFromJsonAsync<RatingDetailsDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async Task<RatingDetailsDTO> RemoveRating(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Rating/RemoveRatingGivenRatingId/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<RatingDetailsDTO>();
                }
                return default(RatingDetailsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<RatingDetailsDTO>> GetAllRatingsOofAUser(int userId)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/Rating/GetAllRatingsGivenUserId/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<RatingDetailsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<RatingDetailsDTO>>();
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
