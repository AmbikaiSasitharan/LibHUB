using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class GenreService: IGenreService
    {
        private readonly HttpClient httpClient;

        public GenreService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<GenreDetailsDTO>> GetAllGenres()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Genre/GetAllGenres");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<GenreDetailsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<GenreDetailsDTO>>();
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

        public async Task<GenreDetailsDTO> GetGenre(int Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Genre/GetGenreGivenGenreId/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(GenreDetailsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<GenreDetailsDTO>();
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

        public async Task<GenreDetailsDTO> AddGenre(GenreToAddDTO genreToAdd)
        {
            var response = await httpClient.PostAsJsonAsync<GenreToAddDTO>("api/Genre/AddGenre", genreToAdd);
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(GenreDetailsDTO);
                }
                return await response.Content.ReadFromJsonAsync<GenreDetailsDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async Task<GenreDetailsDTO> RemoveGenre(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Genre/RemoveGenreGivenGenreId/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<GenreDetailsDTO>();
                }
                return default(GenreDetailsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
