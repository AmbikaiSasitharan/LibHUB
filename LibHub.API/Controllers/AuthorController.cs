using LibHub.API.Repository.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibHub.Models.DTOs;
using LibHub.API.Extensions;

namespace LibHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository authorRepository;
        private readonly IBookDescriptionRepository bookDescriptionRepository;

        public AuthorController(IAuthorRepository authorRepository, IBookDescriptionRepository bookDescriptionRepository)
        {
            this.authorRepository = authorRepository;
            this.bookDescriptionRepository = bookDescriptionRepository;
        }

        [HttpGet("GetAllAuthors")]
        public async Task<ActionResult<IEnumerable<AuthorDetailsDTO>>> GetAllAuthors()
        {
            try
            {
                var authors = await this.authorRepository.GetAllAuthors();

                if (authors == null)
                {
                    return NotFound();
                }

                var authorsdetailsDTO = authors.ConvertToDTO();
                return Ok(authorsdetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAuthorGivenAuthorId/{Id:int}")]
        public async Task<ActionResult<AuthorDetailsDTO>> GetAuthor(int Id)
        {
            try
            {
                var author = await this.authorRepository.GetAuthor(Id);
                if (author == null)
                {
                    return NotFound();
                }

                var authorDetailDTO = author.ConvertToDTO();
                return Ok(authorDetailDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddAuthor")]
        public async Task<ActionResult<AuthorDetailsDTO>> PostAuthor([FromBody] AuthorToAddDTO authorToAdd)
        {
            try
            {
                var author = await this.authorRepository.AddAuthor(authorToAdd);
                if (author == null)
                {
                    return NoContent();
                }

                var newAuthorDTO = author.ConvertToDTO();
                return CreatedAtAction(nameof(GetAuthor), new { Id = newAuthorDTO.Id }, newAuthorDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("RemoveAuthorGivenAuthorId/{Id:int}")]
        public async Task<ActionResult<AuthorDetailsDTO>> RemoveAuthor(int Id)
        {
            try
            {
                var authorToDelete = await this.authorRepository.GetAuthor(Id);

                if (authorToDelete == null)
                {
                    return NotFound();
                }

                var authorRemovedFromBookDescriptions = await this.bookDescriptionRepository.RemoveAuthorFromBookDescriptions(authorToDelete.BookDescriptions, Id);

                if (authorRemovedFromBookDescriptions == null)
                {
                    return NotFound();
                }

                var author = await this.authorRepository.RemoveAuthor(Id);

                var authorDetailsDTO = author.ConvertToDTO();

                return Ok(authorDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
