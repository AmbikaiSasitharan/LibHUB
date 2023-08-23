using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class BookDescriptionInventoryService : IBookDescriptionInventoryService
    {
        private readonly HttpClient httpClient;

        public BookDescriptionInventoryService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<BookDescriptionDetailsDTO> GetBookDescription(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/BookDescription/GetBookDescriptionGivenBookDescriptionId/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(BookDescriptionDetailsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<BookDescriptionDetailsDTO>();
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

        public async Task<IEnumerable<BookDescriptionInventoryDTO>> GetBookDescriptions()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/BookDescription/GetAllBookDescriptions");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<BookDescriptionInventoryDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<BookDescriptionInventoryDTO>>();
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

        public async Task<BookDescriptionDetailsDTO> AddBookDescription(BookDescriptionToAddDTO bookDescriptionToAddDTO)
        {
            var response = await httpClient.PostAsJsonAsync<BookDescriptionToAddDTO>("api/BookDescription/AddBookDescription", bookDescriptionToAddDTO);
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(BookDescriptionDetailsDTO);
                }
                return await response.Content.ReadFromJsonAsync<BookDescriptionDetailsDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async Task<BookDescriptionDetailsDTO> RemoveBookDescription(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/BookDescription/RemoveBookDescriptionGivenBookDescriptionId/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BookDescriptionDetailsDTO>();
                }
                return default(BookDescriptionDetailsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BookDescriptionDetailsDTO> RemovingAuthorFromBookDescription(AuthorAndBookDescriptionRelationshipDTO authorAndBookDescriptionRelationshipDTO)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<AuthorAndBookDescriptionRelationshipDTO>("api/BookDescription/RemoveAuthorFromBookDescription", authorAndBookDescriptionRelationshipDTO);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BookDescriptionDetailsDTO>();
                }
                return default(BookDescriptionDetailsDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BookDescriptionDetailsDTO> AddingAuthorToBookDescription(AuthorAndBookDescriptionRelationshipDTO authorAndBookDescriptionRelationshipDTO)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<AuthorAndBookDescriptionRelationshipDTO>("api/BookDescription/AddingAuthorToBookDescription", authorAndBookDescriptionRelationshipDTO);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BookDescriptionDetailsDTO>();
                }
                return default(BookDescriptionDetailsDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BookDescriptionDetailsDTO> DeactivateBookDescription(int Id)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<int>($"api/BookDescription/DeactivateBookDescriptionGivenBookDescriptionId/{Id}", Id);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BookDescriptionDetailsDTO>();
                }
                return default(BookDescriptionDetailsDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<BookDescriptionDetailsDTO> ActivateBookDescription(int Id)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<int>($"api/BookDescription/ActivateBookDescriptionGivenBookDescriptionId/{Id}", Id);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BookDescriptionDetailsDTO>();
                }
                return default(BookDescriptionDetailsDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
