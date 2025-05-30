using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using notification_system.notification.Features.Otp.RequestOtp;

namespace notification_system.notification.Features.Otp.Core
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OtpController : BaseController
    {
        private readonly BL_RequestOtp _bL_RequestOtp;

        public OtpController(BL_RequestOtp bL_RequestOtp)
        {
            _bL_RequestOtp = bL_RequestOtp;
        }

        [HttpPost("RequestOtp")]
        public async Task<IActionResult> RequestOtp(RequestOtpRequest request, CancellationToken cs)
        {
            var result = await _bL_RequestOtp.RequestOtp(request, cs);
            return Content(result);
        }
    }
}
