namespace DevFreela.Core.IntegrationEvents
{
    public class PaymentApprovedIntegrationEvent
    {
        public int IdProject { get; private set; }

        public PaymentApprovedIntegrationEvent(int idProject)
        {
            IdProject = idProject;
        }
    }
}
