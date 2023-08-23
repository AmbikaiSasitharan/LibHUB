using LibHub.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibHub.Web.Services.Contracts
{
    public interface IMailService
    {
        Task SendEmailAsync(string ToEmail, string Subject, string HTMLBody);
    }
}
