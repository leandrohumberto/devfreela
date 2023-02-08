using DevFreela.Core.DTOs;
using DevFreela.Core.Services;

namespace DevFreela.Infrastructure.Payments
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> ProcessPayment(PaymentInfoDTO dto)
        {
            // Implementar lógica de pagamento com gateway de pagamento

            return Task.FromResult(true);
        }
    }
}
