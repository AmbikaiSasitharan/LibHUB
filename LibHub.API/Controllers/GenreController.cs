using LibHub.API.Extensions;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository genreRepository;
        private readonly IBookDescriptionRepository bookDescriptionRepository;

        public GenreController(IGenreRepository genreRepository, IBookDescriptionRepository bookDescriptionRepository)
        {
            this.genreRepository = genreRepository;
            this.bookDescriptionRepository = bookDescriptionRepository;
        }

        [HttpGet("GetAllGenres")]
        public async Task<ActionResult<IEnumerable<GenreDetailsDTO>>> GetAllGenres()
        {
            try
            {
                var genres = await this.genreRepository.GetAllGenres();

                if (genres == null)
                {
                    return NotFound();
                }

                var genresDetailsDTO = genres.ConvertToDTO();
                return Ok(genresDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetGenreGivenGenreId/{Id:int}")]
        public async Task<ActionResult<GenreDetailsDTO>> GetGenre(int Id)
        {
            try
            {
                var genre = await this.genreRepository.GetGenre(Id);
                if (genre == null)
                {
                    return NotFound();
                }

                var genreDetailDTO = genre.ConvertToDTO();
                return Ok(genreDetailDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddGenre")]
        public async Task<ActionResult<GenreDetailsDTO>> PostGenre([FromBody] GenreToAddDTO genreToAdd)
        {
            try
            {
                var genre = await this.genreRepository.AddGenre(genreToAdd);
                if (genre == null)
                {
                    return NoContent();
                }

                var newGenreDTO = genre.ConvertToDTO();
                return CreatedAtAction(nameof(GetGenre), new { Id = newGenreDTO.Id }, newGenreDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("RemoveGenreGivenGenreId/{Id:int}")]
        public async Task<ActionResult<GenreDetailsDTO>> RemoveGenre(int Id)
        {
            try
            {
                var genreToDelete = await this.genreRepository.GetGenre(Id);

                if (genreToDelete == null)
                {
                    return NotFound();
                }

                var genreRemovedFromBookDescriptions = await this.bookDescriptionRepository.RemoveGenreFromBookDescriptions(genreToDelete.BookDescriptions, Id);

                if (genreRemovedFromBookDescriptions == null)
                {
                    return NotFound();
                }

                var genre = await this.genreRepository.RemoveGenre(Id);

                var genreDetailsDTO = genre.ConvertToDTO();

                return Ok(genreDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
