using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using notification_system.notification.Features.SMS.SendSMS;

namespace notification_system.notification.Features.SMS.Core
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SMSController : BaseController
    {
        private readonly BL_SendSMS _bL_SendSMS;

        public SMSController(BL_SendSMS bL_SendSMS)
        {
            _bL_SendSMS = bL_SendSMS;
        }

        [HttpPost("SendSingleSMS")]
        public async Task<IActionResult> SendSingleSMS(SendSingleSMSRequest request, CancellationToken cs)
        {
            var result = await _bL_SendSMS.SendSingleSMSAsync(request, cs);
            return Content(result);
        }
    }
}
