using DevFreela.Core.DTOs;
using DevFreela.Core.Repositories;
using DevFreela.Core.Services;
using MediatR;

namespace DevFreela.Application.Commands.FinishProject
{
    public class FinishProjectCommandHandler : IRequestHandler<FinishProjectCommand, bool>
    {

        private readonly IProjectRepository _repository;
        private readonly IPaymentService _paymentService;

        public FinishProjectCommandHandler(IProjectRepository repository, IPaymentService paymentService)
        {
            _repository = repository;
            _paymentService = paymentService;
        }

        public async Task<bool> Handle(FinishProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (project != null)
            {
                var paymentInfoDto = new PaymentInfoDTO(request.Id, request.CreditCardNumber, request.Cvv,
                    request.ExpiresAt, request.FullName, project?.TotalCost ?? 0.0M);
                
                _paymentService.ProcessPayment(paymentInfoDto);

                project?.SetPaymentPending();
                 
                await _repository.SaveChangesAsync(cancellationToken);

                return true;
            }

            return false;
        }

        /*
         * Implementação do método Handle com chamada síncrona ao microsserviço
         * 
        public async Task<bool> Handle(FinishProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (project != null)
            {
                project?.Finish();

                var paymentInfoDto = new PaymentInfoDTO(request.Id, request.CreditCardNumber, request.Cvv,
                    request.ExpiresAt, request.FullName, project?.TotalCost ?? 0.0M);
                var result = await _paymentService.ProcessPayment(paymentInfoDto);

                if (!result)
                {
                    project?.SetPaymentPending();
                }

                await _repository.SaveChangesAsync(cancellationToken);

                return result;
            }

            return false;
        } 
        */
    }
}
