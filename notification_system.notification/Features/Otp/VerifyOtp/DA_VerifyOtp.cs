using Microsoft.EntityFrameworkCore;
using notification_system.notification.Persistence.Wrapper;
using notification_system.notification.Utils;

namespace notification_system.notification.Features.Otp.VerifyOtp
{
    public class DA_VerifyOtp
    {
        private readonly IUnitOfWork _unitOfWork;

        public DA_VerifyOtp(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<VerifyOtpResponse>> VerifyOtpAsync(VerifyOtpRequest request, CancellationToken cs = default)
        {
            BaseResponse<VerifyOtpResponse> result;

            var item = await _unitOfWork.OtpRepository
                .GetByCondition(x => x.OtpId == request.RefId && x.OtpValue == request.OtpValue && x.ExpiredAt > DateTime.Now && !x.IsDeleted)
                .SingleOrDefaultAsync(cs);
            if (item is null)
            {
                result = BaseResponse<VerifyOtpResponse>.NotFound("No data found.");
                goto result;
            }

            item.IsDeleted = true;
            _unitOfWork.OtpRepository.Update(item);
            await _unitOfWork.SaveChangesAsync(cs);

            result = BaseResponse<VerifyOtpResponse>.Success();

        result:
            return result;
        }
    }
}
