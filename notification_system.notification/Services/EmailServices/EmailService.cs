using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using notification_system.notification.Configurations;
using notification_system.notification.Constants;
using notification_system.notification.Entities;
using notification_system.notification.Features.Email.SendEmail;
using notification_system.notification.Persistence.Wrapper;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;

namespace notification_system.notification.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly AppSetting _setting;
        private readonly ILogger<EmailService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public EmailService(IOptions<AppSetting> setting, ILogger<EmailService> logger, IUnitOfWork unitOfWork)
        {
            _setting = setting.Value;
            _logger = logger;
            _unitOfWork = unitOfWork;
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

                var notiLog = new TblNotificationLog()
                {
                    LogId = Ulid.NewUlid().ToString(),
                    LogType = NotificationTypeConstant.Email,
                    ToEmailList = JsonConvert.SerializeObject(request.ToEmail),
                    CcEmailList = request.CcEmail is not null ? JsonConvert.SerializeObject(request.CcEmail) : null,
                    BccEmailList = request.BccEmail is not null ? JsonConvert.SerializeObject(request.BccEmail) : null,
                    ToPhoneList = null,
                    Payload = JsonConvert.SerializeObject(request),
                    CreatedAt = DateTime.Now,
                    ResponseAt = null,
                    IsSuccess = null,
                    ResponseMessage = null
                };

                var response = await client.SendEmailAsync(msg, cs);

                notiLog.ResponseAt = DateTime.Now;
                notiLog.IsSuccess = response.IsSuccessStatusCode;
                notiLog.ResponseMessage = response.Body is not null ? await response.Body.ReadAsStringAsync(cs) : null;

                await _unitOfWork.NotificationLogRepository.AddAsync(notiLog, cs);
                await _unitOfWork.SaveChangesAsync(cs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Sending Single Notification Error: {ex.ToString()}");
                throw;
            }
        }

        public async Task SendMultipleEmailAysnc(SendEmailMultipleRequest request, CancellationToken cs = default)
        {
            try
            {
                var client = new SendGridClient(_setting.SendGrid.API_KEY);
                var from = new EmailAddress(_setting.SendGrid.FROM_EMAIL, _setting.SendGrid.FROM_NAME);
                var msg = new SendGridMessage
                {
                    From = from,
                    Subject = request.Subject,
                    HtmlContent = request.HtmlContent
                };

                if (request.ToEmails is not null && request.ToEmails.Count > 0)
                {
                    msg.AddTos(request.ToEmails.Select(x => new EmailAddress(x)).ToList());
                }

                if (request.CcEmails is not null && request.CcEmails.Count > 0)
                {
                    msg.AddCcs(request.CcEmails.Select(x => new EmailAddress(x)).ToList());
                }

                if (request.BccEmails is not null)
                {
                    msg.AddBccs(request.BccEmails.Select(x => new EmailAddress(x)).ToList());
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

                var notiLog = new TblNotificationLog()
                {
                    LogId = Ulid.NewUlid().ToString(),
                    LogType = NotificationTypeConstant.Email,
                    ToEmailList = JsonConvert.SerializeObject(request.ToEmails),
                    CcEmailList = request.CcEmails is not null ? JsonConvert.SerializeObject(request.CcEmails) : null,
                    BccEmailList = request.BccEmails is not null ? JsonConvert.SerializeObject(request.BccEmails) : null,
                    ToPhoneList = null,
                    Payload = JsonConvert.SerializeObject(request),
                    CreatedAt = DateTime.Now,
                    ResponseAt = null,
                    IsSuccess = null,
                    ResponseMessage = null
                };

                var response = await client.SendEmailAsync(msg, cs);

                notiLog.ResponseAt = DateTime.Now;
                notiLog.IsSuccess = response.IsSuccessStatusCode;
                notiLog.ResponseMessage = response.Body is not null ? await response.Body.ReadAsStringAsync(cs) : null;

                await _unitOfWork.NotificationLogRepository.AddAsync(notiLog, cs);
                await _unitOfWork.SaveChangesAsync(cs);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Sending Single Notification Error: {ex.ToString()}");
                throw;
            }
        }
    }
}
