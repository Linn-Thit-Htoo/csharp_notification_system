
namespace notification_system.notification.Features.Email.Core;

[Route("api/v1/[controller]")]
[ApiController]
public class EmailController : BaseController
{
    private readonly BL_SendEmail _bL_SendEmail;

    public EmailController(BL_SendEmail bL_SendEmail)
    {
        _bL_SendEmail = bL_SendEmail;
    }

    [HttpPost("SendSingleEmail")]
    public async Task<IActionResult> SendSingleEmail(SendEmailRequest request, CancellationToken cs)
    {
        var result = await _bL_SendEmail.SendSingleEmailAsync(request, cs);
        return Content(result);
    }

    [HttpPost(("SendMultipleEmail"))]
    public async Task<IActionResult> SendMultipleEmail(
        SendMultipleEmailRequest request,
        CancellationToken cs
    )
    {
        var result = await _bL_SendEmail.SendMultipleEmailAsync(request, cs);
        return Content(result);
    }
}
