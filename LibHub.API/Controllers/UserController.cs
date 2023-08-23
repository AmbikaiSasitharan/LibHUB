using LibHub.API.Entities;
using LibHub.API.Extensions;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Runtime.InteropServices;

namespace LibHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IBorrowRepository borrowRepository;
        private readonly IBookRepository bookRepository;
        private readonly IBookDescriptionRepository bookDescriptionRepository;
        private readonly IRatingRespository ratingRespository;

        public UserController(IBorrowRepository borrowRepository, IBookRepository bookRepository, IUserRepository userRepository, IBookDescriptionRepository bookDescriptionRepository, IRatingRespository ratingRespository)
        {
            this.userRepository = userRepository;
            this.borrowRepository = borrowRepository;
            this.bookRepository = bookRepository;            
            this.bookDescriptionRepository = bookDescriptionRepository;
            this.ratingRespository = ratingRespository;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserInventoryDTO>>> GetUsers()
        {
            try
            {
                var users = await this.userRepository.GetUsers();

                if (users == null)
                {
                    return NotFound();
                }
                else
                {
                    var usersDTOS = users.ConvertToDTO();
                    return Ok(usersDTOS);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetUsersWithFeesFined")]
        public async Task<ActionResult<IEnumerable<UserWithLateBorrowsDTO>>> GetUsersWithLateBorrows()
        {
            try
            {
                var usersWithLateBorrowsDetails = new List<UserWithLateBorrowsDTO>();

                var users = await this.userRepository.GetUsersWithLateBorrows();

                if (users == null)
                {
                    return NotFound();
                }
                else
                {
                    foreach(User user in users)
                    {
                        var borrows = await this.borrowRepository.GetLateBorrowOfAUser(user.Id);
                        var borrowsDetailsDTOlist = borrows.ConvertToDTO();
                        var userWithBorrowsDetails = user.ConvertToDTO(borrowsDetailsDTOlist);

                        if(userWithBorrowsDetails.borrows.Any(b => (b.AreFeesFined == true) && (b.IsFineNotified == false)))
                        {
                            usersWithLateBorrowsDetails.Add(userWithBorrowsDetails);
                        }

                    }

                    return Ok(usersWithLateBorrowsDetails);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUsersWithLateBorrowButNoFeesFined")]
        public async Task<ActionResult<IEnumerable<UserWithLateBorrowsDTO>>> GetUsersWithLateBorrowButNoFeesFined()
        {
            try
            {
                var usersWithLateBorrowsDetails = new List<UserWithLateBorrowsDTO>();

                var users = await this.userRepository.GetUsersWithLateBorrows();

                if (users == null)
                {
                    return NotFound();
                }
                else
                {
                    foreach (User user in users)
                    {
                        var borrows = await this.borrowRepository.GetLateBorrowOfAUser(user.Id);
                        var borrowsDetailsDTOlist = borrows.ConvertToDTO();
                        var userWithBorrowsDetails = user.ConvertToDTO(borrowsDetailsDTOlist);

                        if (userWithBorrowsDetails.borrows.All(b => (b.AreFeesFined == false) && (b.IsLateNotified == false)))
                        {
                            usersWithLateBorrowsDetails.Add(userWithBorrowsDetails);
                        }
                    }

                    return Ok(usersWithLateBorrowsDetails);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserWithLateDetailsGivenId/{Id:int}")]
        public async Task<ActionResult<IEnumerable<UserWithLateBorrowsDTO>>> GetUserWithLateBorrowDetails(int Id)
        {
            try
            {
                var user = await this.userRepository.GetUser(Id);

                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                        var borrows = await this.borrowRepository.GetLateBorrowOfAUser(user.Id);
                        var borrowsDetailsDTOlist = borrows.ConvertToDTO();
                        var userWithBorrowsDetails = user.ConvertToDTO(borrowsDetailsDTOlist);


                    return Ok(userWithBorrowsDetails);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserGivenUserId/{id:int}")]
        public async Task<ActionResult<UserDetailsWIthLateBorrowDetailsDTO>> GetUser(int id)
        {
            try
            {
                var user = await this.userRepository.GetUser(id);

                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    var userDTO = user.ConvertToDTOForUsersWithLatesDetails();
                    return Ok(userDTO);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<UserDetailsDTO>> PostUser([FromBody] UserToAddDTO userToAddDTO)
        {
            try
            {
                var newUser = await this.userRepository.AddUser(userToAddDTO);
                if (newUser == null)
                {
                    return NoContent();
                }

                var newUserDTO = newUser.ConvertToDTO();
                return CreatedAtAction(nameof(GetUser), new { Id = newUserDTO.Id }, newUserDTO);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("RemoveUserGivenUserId/{Id:int}")]
        public async Task<ActionResult<UserDetailsDTO>> RemoveUser(int Id)
        {
            try
            {
                var userToDelete = await this.userRepository.GetUser(Id);
                var borrowsToDelete = await this.borrowRepository.GetBorrowHistoryOfAUser(Id);

                if ((userToDelete == null) || (borrowsToDelete == null))
                {
                    return NotFound();
                }

                var borrows = await this.borrowRepository.RemovwAllBorrowsOfAUser(Id);
                var ratings = await this.ratingRespository.RemovwAllRatingsOfAUser(Id);
                var user = await this.userRepository.RemoveUser(Id);

                var userDetailDTO = user.ConvertToDTO();

                return Ok(userDetailDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("UpdateUserInformation/{Id:int}")]
        public async Task<ActionResult<UserDetailsDTO>> UpdateUserInformation(int Id, [FromBody] InfoToUpdateUserDTO infoToUpdateUserDTO)
        {
            try
            {
                var userToUpdate = await this.userRepository.GetUser(Id);
                if (userToUpdate == null)
                {
                    return NotFound();
                }

                var user = await this.userRepository.UpdateUserInformation(Id, infoToUpdateUserDTO);
                var userDetailsDTO = user.ConvertToDTO();

                return Ok(userDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpPut("DeactivateUserGivenUserId/{Id:int}")]
        public async Task<ActionResult<UserDetailsDTO>> DeactivateUser(int Id)
        {
            try
            {
                var userToUpdate = await this.userRepository.GetUser(Id);
                if (userToUpdate == null)
                {
                    return NotFound();
                }

                var user = await this.userRepository.DeactivateUser(Id);
                var userDetailsDTO = user.ConvertToDTO();

                return Ok(userDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("ActivateUserGivenUserId/{Id:int}")]
        public async Task<ActionResult<UserDetailsDTO>> ActivateUser(int Id)
        {
            try
            {
                var userToUpdate = await this.userRepository.GetUser(Id);
                if (userToUpdate == null)
                {
                    return NotFound();
                }

                var user = await this.userRepository.ActivateUser(Id);
                var userDetailsDTO = user.ConvertToDTO();

                return Ok(userDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}   
