using LibHub.API.Entities;
using LibHub.API.Extensions;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;

namespace LibHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookDescriptionController : ControllerBase
    {
        private readonly IBookDescriptionRepository bookDescriptionRepository;
        private readonly IAuthorRepository authorRepository;
        private readonly IGenreRepository genreRepository;
        private readonly IRatingRespository ratingRespository;
        private readonly IUserRepository userRepository;

        public BookDescriptionController(IBookDescriptionRepository bookDescriptionRepository, IAuthorRepository authorRepository, IGenreRepository genreRepository, IRatingRespository ratingRespository, IUserRepository userRepository)
        {
            this.bookDescriptionRepository = bookDescriptionRepository;
            this.authorRepository = authorRepository;
            this.genreRepository = genreRepository;
            this.ratingRespository = ratingRespository;
            this.userRepository = userRepository;
        }

        [HttpGet("GetAllBookDescriptions")]
        public async Task<ActionResult<IEnumerable<BookDescriptionInventoryDTO>>> GetBookDescriptions()
        {
            try
            {
                var bookdescriptions = await this.bookDescriptionRepository.GetBookDescriptions(); 

                if(bookdescriptions == null)
                {
                    return NotFound();
                }
                else
                {
                    var bookDescriptionDTOs = bookdescriptions.ConvertToDTO();
                    return Ok(bookDescriptionDTOs);
                }
            
            } 
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetBookDescriptionGivenBookDescriptionId/{id:int}")]
        public async Task<ActionResult<BookDescriptionDetailsDTO>> GetBookDescriptionDetails(int id)
        {
            try
            {
                var bookdescription = await this.bookDescriptionRepository.GetBookDescription(id);

                if (bookdescription == null)
                {
                    return BadRequest();
                }
                else
                {
                    var bookDescrptionDTO = bookdescription.ConvertToDTO();
                    return Ok(bookDescrptionDTO);
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPut("RemoveAuthorFromBookDescription")]
        public async Task<ActionResult<BookDescriptionDetailsDTO>> RemovingAuthorFromBookDescription([FromBody] AuthorAndBookDescriptionRelationshipDTO authorAndBookDescriptionRelationshipDTO)
        {
            try
            {
                var authorToUpdate = await this.authorRepository.GetAuthor(authorAndBookDescriptionRelationshipDTO.AuthorId);
                var bookDescriptionToUpdate = await this.bookDescriptionRepository.GetBookDescription(authorAndBookDescriptionRelationshipDTO.BookDescriptionId);

                if ((authorToUpdate == null) || (bookDescriptionToUpdate == null))
                {
                    return NotFound();
                }

                var author = await this.authorRepository.RemoveBookDescriptionFromAuthor(authorAndBookDescriptionRelationshipDTO.AuthorId, authorAndBookDescriptionRelationshipDTO.BookDescriptionId);
                var bookdescription = await this.bookDescriptionRepository.RemoveAuthorFromBookDescription(authorAndBookDescriptionRelationshipDTO.BookDescriptionId, authorAndBookDescriptionRelationshipDTO.AuthorId);
                
                var bookDescriptionDetailDTO = bookdescription.ConvertToDTO();

                return Ok(bookDescriptionDetailDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("AddingAuthorToBookDescription")]
        public async Task<ActionResult<BookDescriptionDetailsDTO>> AddingAuthorToBookDescription([FromBody] AuthorAndBookDescriptionRelationshipDTO authorAndBookDescriptionRelationshipDTO)
        {
            try
            {
                var authorToUpdate = await this.authorRepository.GetAuthor(authorAndBookDescriptionRelationshipDTO.AuthorId);
                var bookDescriptionToUpdate = await this.bookDescriptionRepository.GetBookDescription(authorAndBookDescriptionRelationshipDTO.BookDescriptionId);

                if ((authorToUpdate == null) || (bookDescriptionToUpdate == null))
                {
                    return NotFound();
                }

                var author = await this.authorRepository.AddBookDescriptionToAuthor(authorAndBookDescriptionRelationshipDTO.AuthorId, authorAndBookDescriptionRelationshipDTO.BookDescriptionId);
                var bookdescription = await this.bookDescriptionRepository.AddAuthorToBookDescription(authorAndBookDescriptionRelationshipDTO.BookDescriptionId, authorAndBookDescriptionRelationshipDTO.AuthorId);

                var bookDescriptionDetailDTO = bookdescription.ConvertToDTO();

                return Ok(bookDescriptionDetailDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddBookDescription")]
        public async Task<ActionResult<BookDescriptionDetailsDTO>> PostBookDescription([FromBody] BookDescriptionToAddDTO bookDescriptionToAddDTO)
        {
            try
            {
                var newBookDescription = await this.bookDescriptionRepository.AddBookDescription(bookDescriptionToAddDTO);
                
                if (newBookDescription == null)
                {
                    return NotFound($"A BookDescription with Title {bookDescriptionToAddDTO.Title} and PublishDate {bookDescriptionToAddDTO.PublishDate}");
                }

                for (var i = 0; i < bookDescriptionToAddDTO.AuthorIds.Count; i++)
                {
                    var authorToAdd = await this.authorRepository.GetAuthor(bookDescriptionToAddDTO.AuthorIds[i]);
                    
                    if (authorToAdd == null)
                    {
                        return NotFound($"Author with ID {bookDescriptionToAddDTO.AuthorIds[i]} not found.");
                    }

                    var bookDescriptionToAddAuthorTo = await this.bookDescriptionRepository.AddAuthorToBookDescription(newBookDescription.Id, bookDescriptionToAddDTO.AuthorIds[i]);
                    var authorToAddBookDescriptionTo = await this.authorRepository.AddBookDescriptionToAuthor(bookDescriptionToAddDTO.AuthorIds[i], newBookDescription.Id);

                    if ((bookDescriptionToAddAuthorTo == null) || (authorToAddBookDescriptionTo == null))
                    {
                        return NotFound();
                    }
                }

                for (var i = 0; i < bookDescriptionToAddDTO.GenreIds.Count; i++)
                {
                    var genreToAdd = await this.genreRepository.GetGenre(bookDescriptionToAddDTO.GenreIds[i]);

                    if (genreToAdd == null)
                    {
                        return NotFound($"Genre with ID {bookDescriptionToAddDTO.GenreIds[i]} not found."); ;
                    }

                    var bookDescriptionToAddGenreTo = await this.bookDescriptionRepository.AddGenreToBookDescription(newBookDescription.Id, bookDescriptionToAddDTO.GenreIds[i]);
                    var genreToAddBookDescriptionTo = await this.genreRepository.AddBookDescriptionToGenre(bookDescriptionToAddDTO.GenreIds[i], newBookDescription.Id);

                    if ((bookDescriptionToAddGenreTo == null) || (genreToAddBookDescriptionTo == null))
                    {
                        return NotFound();
                    }
                }

                var newBorrowDetailsDTO = newBookDescription.ConvertToDTO();
                return CreatedAtAction(nameof(GetBookDescriptionDetails), new { Id = newBorrowDetailsDTO.Id }, newBorrowDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }            
        }

        [HttpDelete("RemoveBookDescriptionGivenBookDescriptionId/{Id:int}")]
        public async Task<ActionResult<BookDescriptionDetailsDTO>> RemoveBookDescription(int Id)
        {
            try
            {
                var bookDescriptionToDelete = await this.bookDescriptionRepository.GetBookDescription(Id);
                if (bookDescriptionToDelete == null)
                {
                    return NotFound();
                }

                var bookDescriptionRemovedFromAuthors = await this.authorRepository.RemoveBookDescriptionFromAuthors(bookDescriptionToDelete.Authors, Id);
                var bookDescriptionRemovedFromGenres = await this.genreRepository.RemoveBookDescriptionFromGenres(bookDescriptionToDelete.Genres, Id);
                var bookDescriptionRemovedFromRatings = await this.ratingRespository.RemoveAllRatingsOfBookDescription(Id);

                if ((bookDescriptionRemovedFromAuthors == null) || (bookDescriptionRemovedFromGenres == null) || (bookDescriptionRemovedFromRatings == null))
                {
                    return NotFound();
                }

                var bookDescription = await this.bookDescriptionRepository.RemoveBookDescription(Id);
                var bookDescriptionDetailsDTO = bookDescription.ConvertToDTO();

                return Ok(bookDescriptionDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("DeactivateBookDescriptionGivenBookDescriptionId/{Id:int}")]
        public async Task<ActionResult<BookDescriptionDetailsDTO>> DeactivateBookDescription(int Id)
        {
            try
            {
                var bookDescriptionToUpdate = await this.bookDescriptionRepository.GetBookDescription(Id);
                if (bookDescriptionToUpdate == null)
                {
                    return NotFound();
                }

                var bookDescription = await this.bookDescriptionRepository.DeactivateBookDescription(Id);
                var userDetailsDTO = bookDescription.ConvertToDTO();

                return Ok(userDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpPut("ActivateBookDescriptionGivenBookDescriptionId/{Id:int}")]
        public async Task<ActionResult<BookDescriptionDetailsDTO>> ActivateBookDescription(int Id)
        {
            try
            {
                var bookDescriptionToUpdate = await this.bookDescriptionRepository.GetBookDescription(Id);
                if (bookDescriptionToUpdate == null)
                {
                    return NotFound();
                }

                var bookDescription = await this.bookDescriptionRepository.ActivateBookDescription(Id);
                var userDetailsDTO = bookDescription.ConvertToDTO();

                return Ok(userDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
