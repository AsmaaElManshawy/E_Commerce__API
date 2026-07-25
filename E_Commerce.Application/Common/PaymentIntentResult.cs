using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public sealed class PaymentIntentResult
    {
        public PaymentIntentResult(string payIntentId , string clientSecret)
        {
            PaymentIntentId = payIntentId;
            ClientSecret = clientSecret;
        }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
    }
}