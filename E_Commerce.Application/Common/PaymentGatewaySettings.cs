using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class PaymentGatewaySettings
    {
        public string ClientSecret { get; set; } = default!;
        public string DefaultCurrancy { get; set; } = default!;

    }
}
