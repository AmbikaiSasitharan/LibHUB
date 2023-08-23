using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient httpClient;

        public BookService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<BookDetailsDTO>> GetBooksForBookDescription(int bookDescriptionId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Book/GetBooksGiveBookDescriptionId/{bookDescriptionId}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<BookDetailsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<BookDetailsDTO>>();
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

        public async Task<BookDetailsDTO> GetBook(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Book/GetBookGivenBookId/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(BookDetailsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<BookDetailsDTO>();
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

        public async Task<BookDetailsDTO> AddBook(BookToAddDTO bookToAddDTO)
        {
            var response = await httpClient.PostAsJsonAsync<BookToAddDTO>("api/Book/AddBook", bookToAddDTO);
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(BookDetailsDTO);
                }
                return await response.Content.ReadFromJsonAsync<BookDetailsDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async Task<BookDetailsDTO> RemoveBook(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/Book/BorrowRemoveBookGivenBookId/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BookDetailsDTO>();
                }
                return default(BookDetailsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<BookDetailsDTO>> GetAllBooks()
        {
            try
            {
                var response = await httpClient.GetAsync($"api/Book/GetAllBooks");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<BookDetailsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<BookDetailsDTO>>();
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
