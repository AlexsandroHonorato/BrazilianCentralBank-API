using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrazilianCentralBank.Services.Hosts.Command {
    public class CurrencyQuotationCommand {
        public string Simbolo { get; set; }   
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
    }
}
