using LibHub.API.Entities;
using LibHub.API.Extensions;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace LibHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BorrowController : ControllerBase
    {
        private readonly IBorrowRepository borrowRepository;
        private readonly IBookRepository bookRepository;
        private readonly IUserRepository userRepository;
        private readonly IBookDescriptionRepository bookDescriptionRepository;
        public BorrowController(IBorrowRepository borrowRepository, IBookRepository bookRepository, IUserRepository userRepository, IBookDescriptionRepository bookDescriptionRepository)
        {
            this.borrowRepository = borrowRepository;
            this.bookRepository = bookRepository;
            this.userRepository = userRepository;
            this.bookDescriptionRepository = bookDescriptionRepository;
        }

        [HttpGet("GetAllBorrowsGivenUserId/{userId1:int}")]
        public async Task<ActionResult<IEnumerable<BorrowDetailsDTO>>> GetBorrowHistoryOfAUser(int userId1)
        {
            try
            {
                var borrows = await this.borrowRepository.GetBorrowHistoryOfAUser(userId1);

                if (borrows == null)
                {
                    return BadRequest();
                }
                else
                {
                    var borrowDetailsDTOs = borrows.ConvertToDTO();
                    return Ok(borrowDetailsDTOs);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetAllCurrentBorrowsGivenUserId/{userId2:int}")]
        public async Task<ActionResult<IEnumerable<BorrowDetailsDTO>>> GetCurrentBorrowsOfAUser(int userId2)
        {
            try
            {
                var borrows = await this.borrowRepository.GetCurrentBorrowsOfAUser(userId2);

                if (borrows == null)
                {
                    return BadRequest();
                }
                else
                {
                    var borrowDetailsDTOs = borrows.ConvertToDTO();
                    return Ok(borrowDetailsDTOs);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetBorrowGivenBorrowId/{id:int}")]
        public async Task<ActionResult<BorrowDetailsDTO>> GetBorrow(int id)
        {
            try
            {
                var borrow = await this.borrowRepository.GetBorrow(id);

                if (borrow == null)
                {
                    return BadRequest();
                }
                else
                {
                    var borrowDetailsDTO = borrow.ConvertToDTO();
                    return Ok(borrowDetailsDTO);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost("AddBorrow")]
        public async Task<ActionResult<BorrowDetailsDTO>> PostBorrow([FromBody] BorrowToAddDTO borrowToAddDTO)
        {
            var bookToAddToBorrow = await this.bookRepository.GetBook(borrowToAddDTO.BookId);
            var userToAddToBorrow = await this.userRepository.GetUser(borrowToAddDTO.UserId);
            try
            {
                var newBorrow = await this.borrowRepository.AddBorrow(borrowToAddDTO, bookToAddToBorrow, userToAddToBorrow);              
                if (newBorrow == null)
                {
                    return NoContent();
                }

                var bookDescriptionToUpdate = await this.bookDescriptionRepository.SubtractOneFromNumAvailable(bookToAddToBorrow.BookDescriptionId);
                var bookToUpdate = await this.bookRepository.ChangeStatusToUnavailable(borrowToAddDTO.BookId);
                var userToUpdate = await this.userRepository.AddOneToNumBorrowingBooks(borrowToAddDTO.UserId);
                
                if ((bookDescriptionToUpdate == null) || (bookToUpdate == null) || (userToUpdate == null))
                {
                    return NotFound();
                }
                                
                var newBorrowDetailsDTO = newBorrow.ConvertToDTO();
                return CreatedAtAction(nameof(GetBorrow), new { Id = newBorrowDetailsDTO.Id }, newBorrowDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("RemoveBorrowGivenBorrowId/{Id:int}")]
        public async Task<ActionResult<BorrowDetailsDTO>> RemoveBorrow(int Id)
        {
            try
            {
                var borrowToDelete = await this.borrowRepository.GetBorrow(Id);
                if (borrowToDelete == null)
                {
                    return NotFound();
                }
                var bookDescriptionToUpdate = await this.bookDescriptionRepository.AddOneToNumAvailable(borrowToDelete.Book.BookDescriptionId);
                var bookToUpdate = await this.bookRepository.ChangeStatusToAvailable(borrowToDelete.BookId);
                var userToUpdate = await this.userRepository.SubtractOneFromNumBorrowingBooks(borrowToDelete.UserId);
                              
                var borrow = await this.borrowRepository.RemoveBorrow(Id);

                if ((bookDescriptionToUpdate == null) || (bookToUpdate == null) || (userToUpdate == null))
                {
                    return NotFound();
                }

                var borrowDetailsDTO = borrow.ConvertToDTO();

                return Ok(borrowDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("ReturnBorrowGivenBorrowId/{Id:int}")]
        public async Task<ActionResult<BorrowDetailsDTO>> ReturnBorrow(int Id)
        {
            var borrowToReturn = await this.borrowRepository.GetBorrow(Id);
            try
            {
                var bookDescriptionToUpdate = await this.bookDescriptionRepository.AddOneToNumAvailable(borrowToReturn.Book.BookDescriptionId);
                var bookToUpdate = await this.bookRepository.ChangeStatusToAvailable(borrowToReturn.BookId);
                var userToUpdate = await this.userRepository.SubtractOneFromNumBorrowingBooks(borrowToReturn.UserId);

                if ((bookDescriptionToUpdate == null) || (bookToUpdate == null) || (userToUpdate == null) || (borrowToReturn == null))
                {
                    return NotFound();
                }

                var borrow = await this.borrowRepository.ReturnBorrow(Id);
                var borrowDetailsDTO = borrow.ConvertToDTO();

                return Ok(borrowDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("CreateLibraryStatistics")]
        public async Task<ActionResult<LibraryStatsDTO>> PostLibraryStats()
        {
            var usersToAnalyse = await this.userRepository.GetAllUsersForLibraryStats();
            var booksToAnalyse = await this.bookRepository.GetAllBooksForLibraryStats();
            var borrowsToAnalyse = await this.borrowRepository.GetAllBorrowsForLibraryStats();

            if ((usersToAnalyse == null) || (booksToAnalyse == null) || (borrowsToAnalyse == null))
            {
                return NotFound();
            }

            try
            {
                var libraryStatsProperties = booksToAnalyse.ConvertToDTO(usersToAnalyse, borrowsToAnalyse);
                
                return libraryStatsProperties;
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAllBorrowsNotReturned")]
        public async Task<ActionResult<IEnumerable<BorrowDetailsDTO>>> GetAllBorrowsNotReturned()
        {
            try
            {
                var borrows = await this.borrowRepository.GetAllBorrowsForLibraryStats();

                if (borrows == null)
                {
                    return BadRequest();
                }
                else
                {
                    var borrowDetailsDTOs = borrows.ConvertToDTO();
                    return Ok(borrowDetailsDTOs);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPut("UpdateBorrowIsLateNotified/{borrowId:int}")]
        public async Task<IActionResult> UpdateBorrowIsLateNotified(int borrowId)
        {
            try
            {
                var borrowToUpdate = await this.borrowRepository.GetBorrow(borrowId);
                if (borrowToUpdate == null)
                {
                    return NotFound();
                }

                this.borrowRepository.ChangeIsLateNotified(borrowId);

                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("UpdateBorrowIsFineNotified/{borrowId:int}")]
        public async Task<IActionResult> UpdateBorrowIsFineNotified(int borrowId)
        {
            try
            {
                var borrowToUpdate = await this.borrowRepository.GetBorrow(borrowId);
                if (borrowToUpdate == null)
                {
                    return NotFound();
                }

                this.borrowRepository.ChangeIsFineNotified(borrowId);

                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
              

    }
}
