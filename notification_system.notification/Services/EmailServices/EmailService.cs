using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using notification_system.notification.Configurations;
using notification_system.notification.Features.Email.SendEmail;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;

namespace notification_system.notification.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly AppSetting _setting;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<AppSetting> setting, ILogger<EmailService> logger)
        {
            _setting = setting.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(SendEmailRequest request, CancellationToken cs = default)
        {
            try
            {
                var client = new SendGridClient(_setting.SendGrid.API_KEY);
                var from = new EmailAddress(_setting.SendGrid.FROM_EMAIL, _setting.SendGrid.FROM_NAME);
                var msg = MailHelper.CreateSingleEmail(from, new EmailAddress(request.ToEmail), request.Subject, null, request.HtmlContent);

                if (request.CcEmail is not null)
                {
                    msg.AddCc(request.CcEmail);
                }

                if (request.BccEmail is not null)
                {
                    msg.AddBcc(request.BccEmail);
                }

                if (request.Files is not null && request.Files.Count > 0)
                {
                    foreach (var file in request.Files)
                    {
                        msg.AddAttachment(new Attachment
                        {
                            Content = Convert.ToBase64String(file.Content),
                            Filename = file.FileName,
                            Type = file.ContentType,
                            Disposition = "attachment"
                        });
                    }
                }

                var response = await client.SendEmailAsync(msg, cs);
            }
            catch (Exception ex)
            {

            }
        }

        public Task SendEmailAysnc(SendEmailMultipleRequest request, CancellationToken cs = default)
        {
            throw new NotImplementedException();
        }
    }
}
