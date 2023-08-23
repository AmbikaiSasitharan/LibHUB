using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using System.Net.Mail;
using LibHub.API.Repository.Contracts;
using LibHub.Models.DTOs;

namespace LibHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _emailRepository;
        public EmailController(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDTO request)
        {

            _emailRepository.SendEmail(request);

            return Ok();
        }
    }
}
