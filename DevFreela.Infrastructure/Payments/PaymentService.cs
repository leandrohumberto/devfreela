﻿using DevFreela.Core.DTOs;
using DevFreela.Core.Services;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace DevFreela.Infrastructure.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IMessageBusService _messageBusService;
        private const string QUEUE_NAME = "Payments";

        public PaymentService(IMessageBusService messageBusService)
        {
            _messageBusService = messageBusService;
        }

        public void ProcessPayment(PaymentInfoDTO dto)
        {
            var paymentInfoJson = JsonSerializer.Serialize(dto);
            var paymentInfoBytes = Encoding.UTF8.GetBytes(paymentInfoJson);
            _messageBusService.Publish(QUEUE_NAME, paymentInfoBytes);
        }

        //
        // Implementação do serviço utilizando chamada síncrona ao microsserviço
        /*
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _paymentsBaseUrl;

        public PaymentService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _paymentsBaseUrl = configuration.GetRequiredSection("Services:Payments").Value;
        }

        public async Task<bool> ProcessPayment(PaymentInfoDTO dto)
        {
            var url = $"{_paymentsBaseUrl}/api/payments";
            var paymentInfoJson = JsonSerializer.Serialize(dto);
            using var paymentInfoContent = new StringContent(
                paymentInfoJson,
                Encoding.UTF8,
                "application/json"
            );
            using var httpClient = _httpClientFactory.CreateClient("Payments");
            var response = await httpClient.PostAsync(url, paymentInfoContent);

            return response.IsSuccessStatusCode;
        }
        */
    }
}
