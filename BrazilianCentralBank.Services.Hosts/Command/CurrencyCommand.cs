using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrazilianCentralBank.Services.Hosts.Command {
    public class CurrencyCommand {
        public string Id { get; set; }
        public string Simbolo { get; set; }
        public string NomeFormatado { get; set; }
        public string TipoMoeda { get; set; }
    }
}
