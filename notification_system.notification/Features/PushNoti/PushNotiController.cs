using Microsoft.AspNetCore.Mvc;

namespace notification_system.notification.Features.PushNoti;

[Route("api/v1/[controller]")]
[ApiController]
public class PushNotiController : BaseController
{
    private readonly BL_PushNoti _bL_PushNoti;

    public PushNotiController(BL_PushNoti bL_PushNoti)
    {
        _bL_PushNoti = bL_PushNoti;
    }

    [HttpPost]
    public async Task<IActionResult> PushNoti(PushNotiRequest request, CancellationToken cs)
    {
        var result = await _bL_PushNoti.PushNotiAsync(request, cs);
        return Content(result);
    }
}
