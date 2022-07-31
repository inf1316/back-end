﻿using Microsoft.Extensions.Options;
using QvaCar.Application.Services;
using QvaCar.Infraestructure.Identity.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace QvaCar.Infraestructure.Identity.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpOptions options;
        public EmailService(IOptions<SmtpOptions> options)
        {
            this.options = options.Value;
        }
        public async Task SendEmailAsync(string fromAddress, string toAddress, string subject, string body)
        {
            var mailMessage = new MailMessage(fromAddress, toAddress, subject, body);
            using (var client = new SmtpClient(options.Host, options.Port)
            {
                Credentials = new NetworkCredential(options.Username, options.Password)
            })
            {
                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
