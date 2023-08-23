using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class AuthorsService : IAuthorService
    {
        private readonly HttpClient httpClient;

        public AuthorsService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<AuthorDetailsDTO> AddAuthor(AuthorToAddDTO authorToAdd)
        {
            var response = await httpClient.PostAsJsonAsync<AuthorToAddDTO>("api/Author/AddAuthor", authorToAdd);
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(AuthorDetailsDTO);
                }
                return await response.Content.ReadFromJsonAsync<AuthorDetailsDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async Task<IEnumerable<AuthorDetailsDTO>> GetAllAuthors()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/Author/GetAllAuthors");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<AuthorDetailsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<AuthorDetailsDTO>>();
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

        public async Task<AuthorDetailsDTO> GetAuthor(int Id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Author/GetAuthorGivenAuthorId/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(AuthorDetailsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<AuthorDetailsDTO>();
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

        public async Task<AuthorDetailsDTO> RemoveAuthor(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Author/RemoveAuthorGivenAuthorId/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<AuthorDetailsDTO>();
                }
                return default(AuthorDetailsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
