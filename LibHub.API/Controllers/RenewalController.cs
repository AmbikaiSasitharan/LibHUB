using LibHub.API.Repository.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibHub.Models.DTOs;
using LibHub.API.Extensions;
using LibHub.API.Entities;
using System.Linq;

namespace LibHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenewalController : ControllerBase
    {
        private readonly IRenewalRepository renewalRepository;
        private readonly IBorrowRepository borrowRepository;

        public RenewalController(IRenewalRepository renewalRepository, IBorrowRepository borrowRepository)
        {
            this.renewalRepository = renewalRepository;
            this.borrowRepository = borrowRepository;
        }

        [HttpGet("GetRenewalGivenRenewalId/{Id:int}")]
        public async Task<ActionResult<RenewalDetailsDTO>> GetRenewal(int Id)
        {
            try
            {
                var renewal = await this.renewalRepository.GetRenewal(Id);
                if (renewal == null)
                {
                    return BadRequest();
                }
                else
                {
                    var renewalDetailDTO = renewal.ConvertToDTO();
                    return Ok(renewalDetailDTO);

                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("GetLogHistoryGivenUserId/{Id:int}")]
        public async Task<ActionResult<IEnumerable<LogEntryDTO>>> GetLogHistory(int Id)
        {
            try
            {
                var logEntryList = new List<LogEntryDTO>();
                var borrowListOfReturnedBooksOfUser = await this.borrowRepository.GetAllReturnedBorrows(Id);

                if (borrowListOfReturnedBooksOfUser == null)
                {
                    return BadRequest();
                }
                else
                { 
                    foreach (Borrow borrow in  borrowListOfReturnedBooksOfUser)
                    {
                        var borrowLogDTO = borrow.ConvertToLogEntryDTO();
                        logEntryList.Add(borrowLogDTO);

                        for (var i = 0; i < borrow.Renewals.Count; i++)
                        {
                            var renewalLogDTO = ((borrow.Renewals)[i]).ConvertToLogEntryDTO(borrow);
                            logEntryList.Add(renewalLogDTO);
                        }
                    }

                    var sortedList = logEntryList.OrderBy(p => p.EntryDate).ToList();

                    return sortedList;

                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost("AddRenewalGivenBorrowId/{borrowId}")]
        public async Task<ActionResult<RenewalDetailsDTO>> PostRenewal(int borrowId)
        {
            var borrowRenewalAddTo = await this.borrowRepository.GetBorrow(borrowId);
            try
            {
                var newRenewal = await this.renewalRepository.AddRenewal(borrowRenewalAddTo);

                if (newRenewal == null)
                {
                    return NotFound();
                }

                var borrowWithRenewalAdded = await this.borrowRepository.AddRenewalToBorrow(borrowId, newRenewal.Id);
                var borrowWithUpdatedNumRenewals = await this.borrowRepository.AddOneToNumRenewals(borrowId);
                var borrowWithUpdatedDueDate = await this.borrowRepository.Add14DaysToDueDate(borrowId);

                if ((borrowWithRenewalAdded == null) || (borrowWithUpdatedNumRenewals == null) || (borrowWithUpdatedDueDate == null))
                {
                    return NotFound();
                }

                var newRenewalDTO = newRenewal.ConvertToDTO();

                return CreatedAtAction(nameof(GetRenewal), new { Id = newRenewalDTO.Id }, newRenewalDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("RemoveRenewalGivenId/{Id:int}")]
        public async Task<ActionResult<RenewalDetailsDTO>> RemoveRenewal(int Id)
        {
            try
            {
                var renewalToDelete = await this.renewalRepository.GetRenewal(Id);

                if (renewalToDelete == null)
                {
                    return NotFound();
                }

                var BorrowRenewalIsRemovedFrom = await this.borrowRepository.RemoveRenewalFromBorrow(renewalToDelete.BorrowId, renewalToDelete.Id);
                var borrowWithUpdatedNumRenewals = await this.borrowRepository.SubtractOneFromNumRenewals(renewalToDelete.Id);

                if (BorrowRenewalIsRemovedFrom == null)
                {
                    return NotFound();
                }

                var renewal = await this.renewalRepository.RemoveRenewal(Id);

                var renewalDetailDTO = renewalToDelete.ConvertToDTO();

                return Ok(renewalDetailDTO);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
