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
    public class RatingController : ControllerBase
    {
        private readonly IRatingRespository ratingRespository;
        private readonly IUserRepository userRepository;
        private readonly IBookDescriptionRepository bookDescriptionRepository;
        public RatingController(IRatingRespository ratingRespository, IUserRepository userRepository, IBookDescriptionRepository bookDescriptionRepository)
        {
            this.ratingRespository = ratingRespository;
            this.userRepository = userRepository;
            this.bookDescriptionRepository = bookDescriptionRepository;
        }

        [HttpGet("GetRatingGivenRatingId/{Id:int}")]
        public async Task<ActionResult<RatingDetailsDTO>> GetRating(int Id)
        {
            try
            {
                var rating = await this.ratingRespository.GetRating(Id);
                if (rating == null)
                {
                    return BadRequest();
                }
                else
                {
                    var ratingDetailDTO = rating.ConvertToDTO();
                    return Ok(ratingDetailDTO);
                    
                }
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetAllRatingsGivenBookDescriptionId/{descriptionId:int}")]
        public async Task<ActionResult<IEnumerable<RatingDetailsDTO>>> GetRatingForBookDescription(int descriptionId)
        {
            try
            {
                var ratings = await this.ratingRespository.GetRatingsForBookDescription(descriptionId);
                if (ratings == null)
                {
                    return BadRequest();
                }
                
                var ratingDetailsDTOs = ratings.ConvertToDTO();
                return(Ok(ratingDetailsDTOs));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddRating")]
        public async Task<ActionResult<RatingDetailsDTO>> PostRating([FromBody] RatingToAddDTO ratingToAddDTO)
        {
            var UserAddingRating = await this.userRepository.GetUser(ratingToAddDTO.UserId);
            var BookDescriptionGettingRated = await this.bookDescriptionRepository.GetBookDescription(ratingToAddDTO.BookDescriptionId);
            
            if ((BookDescriptionGettingRated == null) || (UserAddingRating == null))
            {
                return NotFound();
            }

            try
            {
                var newRating = await this.ratingRespository.AddRating(ratingToAddDTO, UserAddingRating, BookDescriptionGettingRated);
                if (newRating == null)
                {
                    return NotFound();
                }

                //var UpdatedBookDescription = await this.bookDescriptionRepository.AddRatingToBookDescription(ratingToAddDTO.BookDescriptionId, newRating.rating);

                var newRatingDTO = newRating.ConvertToDTO();
                return CreatedAtAction(nameof(GetRating), new { Id = newRatingDTO.Id}, newRatingDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("RemoveRatingGivenRatingId/{Id:int}")]
        public async Task<ActionResult<RatingDetailsDTO>> RemoveRating(int Id)
        {
            try
            {
                var ratingToDelete = await this.ratingRespository.GetRating(Id);

                if (ratingToDelete == null)
                {
                    return NotFound();
                }

                /*var UpdatedBookDescription = await this.bookDescriptionRepository.RemoveRatingFromBookDescription(ratingToDelete.BookDescriptionId, ratingToDelete.rating);
                
                if (UpdatedBookDescription == null)
                {
                    return NotFound();
                }*/

                var rating = await this.ratingRespository.RemoveRating(Id);

                var ratingDetailsDTO = rating.ConvertToDTO();

                return Ok(ratingDetailsDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAllRatingsGivenUserId/{userId:int}")]
        public async Task<ActionResult<IEnumerable<RatingDetailsDTO>>> GetAllRatingsOfAUser(int userId)
        {
            try
            {
                var ratings = await this.ratingRespository.GetAllRatingsOfAUser(userId);
                if (ratings == null)
                {
                    return BadRequest();
                }

                var ratingDetailsDTOs = ratings.ConvertToDTO();
                return (Ok(ratingDetailsDTOs));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
