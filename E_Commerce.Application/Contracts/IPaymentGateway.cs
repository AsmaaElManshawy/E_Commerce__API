using E_Commerce.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IPaymentGateway
    {
        // create PaymentIntent
        // Amount + Currancy => PaymentIntentId + ClientSecret

        Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount , string currancy , CancellationToken ct = default);

        // Update PaymentIntent
        // PaymentIntentId + Amount => new PaymentIntentId + ClientSecret
        Task<PaymentIntentResult> UpdatePaymentIntentAsync(decimal amount , string paymentIntentId, CancellationToken ct = default);
    }
}
