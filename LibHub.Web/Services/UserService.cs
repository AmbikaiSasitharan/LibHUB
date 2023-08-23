using LibHub.Models.DTOs;
using LibHub.Web.Services.Contracts;
using System.Net.Http.Json;

namespace LibHub.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UserDetailsDTO> ActivateUser(int Id)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<int>($"api/User/ActivateUserGivenUserId/{Id}", Id);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDetailsDTO>();
                }
                return default(UserDetailsDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserDetailsDTO> AddUser(UserToAddDTO userToAddDTO)
        {
            var response = await httpClient.PostAsJsonAsync<UserToAddDTO>("api/User/AddUser", userToAddDTO);
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    return default(UserDetailsDTO);
                }
                return await response.Content.ReadFromJsonAsync<UserDetailsDTO>();
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                throw new Exception($"Http status: {response.StatusCode} Message -{message}");
            }
        }

        public async Task<UserDetailsDTO> DeactivateUser(int Id)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<int>($"api/User/DeactivateUserGivenUserId/{Id}", Id);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDetailsDTO>();
                }
                return default(UserDetailsDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<UserDetailsWIthLateBorrowDetailsDTO> GetUser(int id)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/User/GetUserGivenUserId/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserDetailsWIthLateBorrowDetailsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<UserDetailsWIthLateBorrowDetailsDTO>();
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

        public async Task<IEnumerable<UserInventoryDTO>> GetUsers()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/User/GetAllUsers");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<UserInventoryDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<UserInventoryDTO>>();
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

        public async Task<IEnumerable<UserWithLateBorrowsDTO>> GetUsersWithFeesFined()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/User/GetUsersWithFeesFined");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<UserWithLateBorrowsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<UserWithLateBorrowsDTO>>();
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

        public async Task<IEnumerable<UserWithLateBorrowsDTO>> GetUsersWithLateBorrowButNoFeesFined()
        {
            try
            {
                var response = await this.httpClient.GetAsync("api/User/GetUsersWithLateBorrowButNoFeesFined");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<UserWithLateBorrowsDTO>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<UserWithLateBorrowsDTO>>();
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

        public async Task<UserWithLateBorrowsDTO> GetUserWithLateBorrowDetails(int id)
        {
            try
            {
                var response = await this.httpClient.GetAsync($"api/User/GetUserWithLateDetailsGivenId/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(UserWithLateBorrowsDTO);
                    }
                    return await response.Content.ReadFromJsonAsync<UserWithLateBorrowsDTO>();
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

        public async Task<UserDetailsDTO> RemoveUser(int Id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/User/RemoveUserGivenUserId/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDetailsDTO>();
                }
                return default(UserDetailsDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDetailsDTO> UpdateUserInformation(int Id, InfoToUpdateUserDTO infoToUpdateUserDTO)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<InfoToUpdateUserDTO>($"api/User/UpdateUserInformation/{Id}", infoToUpdateUserDTO);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserDetailsDTO>();
                }
                return default(UserDetailsDTO);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
