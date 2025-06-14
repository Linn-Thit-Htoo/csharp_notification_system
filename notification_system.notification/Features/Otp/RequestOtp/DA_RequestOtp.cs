using System.Security.Cryptography;

namespace notification_system.notification.Features.Otp.RequestOtp;

public class DA_RequestOtp
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppSetting _setting;

    public DA_RequestOtp(IUnitOfWork unitOfWork, IOptions<AppSetting> setting)
    {
        _unitOfWork = unitOfWork;
        _setting = setting.Value;
    }

    public async Task<BaseResponse<RequestOtpResponse>> RequestOtp(
        RequestOtpRequest request,
        CancellationToken cs = default
    )
    {
        int otpValue = GenerateSixDigitNumber();
        var item = new TblOtp()
        {
            OtpId = Ulid.NewUlid().ToString(),
            CreatedAt = DateTime.Now,
            Email = request.Email,
            ExpiredAt = DateTime.Now.AddMinutes(_setting.OtpConfig.ExpireInMinutes),
            IsDeleted = false,
            OtpValue = otpValue,
        };

        await _unitOfWork.OtpRepository.AddAsync(item, cs);
        await _unitOfWork.SaveChangesAsync(cs);

        return BaseResponse<RequestOtpResponse>.Success(
            new RequestOtpResponse { ExpiredAt = item.ExpiredAt, Otp = otpValue }
        );
    }

    private int GenerateSixDigitNumber()
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] bytes = new byte[4];
        rng.GetBytes(bytes);
        int randomValue = BitConverter.ToInt32(bytes, 0) & int.MaxValue;
        return (randomValue % 900000) + 100000;
    }
}
