using MediatR;

namespace DevFreela.Application.Commands.FinishProject
{
    public class FinishProjectCommand : IRequest<bool>
    {
        public FinishProjectCommand(int id, string creditCardNumber, string cvv, string expiresAt, string fullName)
        {
            if (string.IsNullOrWhiteSpace(creditCardNumber))
            {
                throw new ArgumentException($"'{nameof(creditCardNumber)}' cannot be null or whitespace.", nameof(creditCardNumber));
            }

            if (string.IsNullOrWhiteSpace(cvv))
            {
                throw new ArgumentException($"'{nameof(cvv)}' cannot be null or whitespace.", nameof(cvv));
            }

            if (string.IsNullOrWhiteSpace(expiresAt))
            {
                throw new ArgumentException($"'{nameof(expiresAt)}' cannot be null or whitespace.", nameof(expiresAt));
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException($"'{nameof(fullName)}' cannot be null or whitespace.", nameof(fullName));
            }

            Id = id;
            CreditCardNumber = creditCardNumber;
            Cvv = cvv;
            ExpiresAt = expiresAt;
            FullName = fullName;
        }

        public int Id { get; set; }
        public string CreditCardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpiresAt { get; set; }
        public string FullName { get; set; }
    }
}
