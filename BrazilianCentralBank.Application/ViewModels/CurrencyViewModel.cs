using System;
using System.Collections.Generic;
using System.Text;

namespace BrazilianCentralBank.Application.ViewModels {
    public class CurrencyViewModel {
        public string Id { get; set; }
        public string Simbolo { get; set; }
        public string NomeFormatado { get; set; }
        public string TipoMoeda { get; set; }
    }
}
