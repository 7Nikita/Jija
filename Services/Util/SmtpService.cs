using System.Threading;
using Jija.Services.Background;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace Jija.Services
{
    public class SmtpService : IMailing
    {
        private readonly ILogger<SmtpService> _logger;
        private readonly IConfiguration _configuration;
        private readonly CancellationToken _cancellationToken;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public SmtpService(IConfiguration configuration, ILogger<SmtpService> logger, IBackgroundTaskQueue taskQueue,
            IHostApplicationLifetime applicationLifetime)
        {
            _configuration = configuration;
            _logger = logger;
            _backgroundTaskQueue = taskQueue;
            _cancellationToken = applicationLifetime.ApplicationStopping;
        }

        public void Send(string to, string subject, string body)
        {
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                var message = new MimeMessage
                {
                    Sender = new MailboxAddress(_configuration["Mailing:From"], _configuration["Mailing:Email"]),
                    Subject = subject,
                    Body = new TextPart(TextFormat.Html) {Text = body},
                };
                message.To.Add(new MailboxAddress(to));

                using var emailClient = new SmtpClient();
                await emailClient.ConnectAsync(_configuration["Mailing:Host"], 465, true);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                await emailClient.AuthenticateAsync(_configuration["Mailing:Email"],
                    _configuration["Mailing:Pass"]);
                await emailClient.SendAsync(message);
                
                _logger.LogInformation($"Sent message to {to}");
                
                await emailClient.DisconnectAsync(true);
            });
        }
    }
}