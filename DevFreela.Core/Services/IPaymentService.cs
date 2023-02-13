using DevFreela.Core.DTOs;

namespace DevFreela.Core.Services
{
    public interface IPaymentService
    {
        void ProcessPayment(PaymentInfoDTO dto);
        //Task<bool> ProcessPayment(PaymentInfoDTO dot);
    }
}
