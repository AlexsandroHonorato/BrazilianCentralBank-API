
using System;
using System.Collections.Generic;
using System.Text;

using BrazilianCentralBank.Application.ViewModels;

namespace BrazilianCentralBank.Application.Helper {
    public class AuthResponseObjHelper {
        public int status_code { get; set; }
        public string http_message { get; set; }  
        public string Token { get; set; }
        public CurrencyViewModel currency { get; set; }
        public List<CurrencyViewModel> currencyList {get; set;}
        public List<CurrencyQuotationViewModel> currencyQuotationsList { get; set; }
    }
}
