using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using notification_system.notification.Features.Otp.RequestOtp;
using notification_system.notification.Features.Otp.VerifyOtp;

namespace notification_system.notification.Features.Otp.Core
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OtpController : BaseController
    {
        private readonly BL_RequestOtp _bL_RequestOtp;
        private readonly BL_VerifyOtp _bL_VerifyOtp;

        public OtpController(BL_RequestOtp bL_RequestOtp, BL_VerifyOtp bL_VerifyOtp)
        {
            _bL_RequestOtp = bL_RequestOtp;
            _bL_VerifyOtp = bL_VerifyOtp;
        }

        [HttpPost("RequestOtp")]
        public async Task<IActionResult> RequestOtp(RequestOtpRequest request, CancellationToken cs)
        {
            var result = await _bL_RequestOtp.RequestOtp(request, cs);
            return Content(result);
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request, CancellationToken cs)
        {
            var result = await _bL_VerifyOtp.VerifyOtpAsync(request, cs);
            return Content(result);
        }
    }
}
