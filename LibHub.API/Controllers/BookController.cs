using LibHub.API.Extensions;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository bookRepository;
        private readonly IBookDescriptionRepository bookDescriptionRepository;
        public BookController(IBookRepository bookRepository, IBookDescriptionRepository bookDescriptionRepository)
        {
            this.bookRepository = bookRepository;
            this.bookDescriptionRepository = bookDescriptionRepository;
        }

        [HttpGet("GetBooksGiveBookDescriptionId/{bookDescriptionId:int}")]
        public async Task<ActionResult<IEnumerable<BookDetailsDTO>>> GetBooksForBookDescription(int bookDescriptionId)
        {
            try
            {
                var books = await this.bookRepository.GetBooksForBookDescription(bookDescriptionId);

                if (books == null)
                {
                    return BadRequest();
                }
                else
                {
                    var bookDetailsDTO = books.ConvertToDTO();
                    return Ok(bookDetailsDTO);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetBookGivenBookId/{id:int}")]
        public async Task<ActionResult<BookDetailsDTO>> GetBook(int id)
        {
            try
            {
                var book = await this.bookRepository.GetBook(id);

                if (book == null)
                {
                    return BadRequest();
                }
                else
                {
                    var bookDetailsDTO = book.ConvertToDTO();
                    return Ok(bookDetailsDTO);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost("AddBook")]
        public async Task<ActionResult<BookDetailsDTO>> PostBook([FromBody] BookToAddDTO bookToAddDTO)
        {
            var bookDescriptionToAddBookTo = await this.bookDescriptionRepository.GetBookDescription(bookToAddDTO.BookDescriptionId);
            if (bookDescriptionToAddBookTo == null)
            {
                return NoContent();
            }
            
            try
            {
                var newBook = await this.bookRepository.AddBook(bookToAddDTO, bookDescriptionToAddBookTo);
                if (newBook == null)
                {
                    return NoContent();
                }

                var bookDescriptionWithNumAvailableUpdated = await this.bookDescriptionRepository.AddOneToNumAvailable(bookToAddDTO.BookDescriptionId);
                var bookDescriptionWithNumCopiesUpdated = await this.bookDescriptionRepository.AddOneToNumCopies(bookToAddDTO.BookDescriptionId);

                var newBookDTO = newBook.ConvertToDTO();
                return CreatedAtAction(nameof(GetBook), new { Id = newBook.Id }, newBookDTO);

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("RemoveBookGivenBookId/{Id:int}")]
        public async Task<ActionResult<BookDetailsDTO>> RemoveBook(int Id)
        {
            try
            {
                var bookToDelete = await this.bookRepository.GetBook(Id);

                if (bookToDelete == null)
                {
                    return NotFound();
                }

                var bookDescirptionBookIsRemovedFrom = await this.bookDescriptionRepository.RemoveBookFromBookDescription(bookToDelete.BookDescriptionId, bookToDelete.Id);
                var bookDescriptionWithNumAvailableUpdated = await this.bookDescriptionRepository.SubtractOneFromNumAvailable(bookToDelete.BookDescriptionId);
                var bookDescriptionWithNumCopiesUpdated = await this.bookDescriptionRepository.SubtractOneFromNumCopies(bookToDelete.BookDescriptionId);

                if ((bookDescirptionBookIsRemovedFrom == null) || (bookDescriptionWithNumAvailableUpdated == null) || (bookDescriptionWithNumCopiesUpdated == null))
                {
                    return NotFound();
                }

                var borrow = await this.bookRepository.RemoveBook(Id);

                var bookDetailsDTO = bookToDelete.ConvertToDTO();

                return Ok(bookDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAllBooks")]
        public async Task<ActionResult<IEnumerable<BookDetailsDTO>>> GetAllBooks()
        {
            try
            {
                var books = await this.bookRepository.GetAllBooksForLibraryStats();

                if (books == null)
                {
                    return BadRequest();
                }
                else
                {
                    var bookDetailsDTO = books.ConvertToDTO();
                    return Ok(bookDetailsDTO);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }
    }
}


