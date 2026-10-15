using ElAmal.Core.Entities.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Contract.Services.Mail
{
    public interface IMailSettingsService
    {
        Task SendEmailAsync(Email email);
    }
}
