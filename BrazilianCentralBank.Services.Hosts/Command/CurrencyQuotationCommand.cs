using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrazilianCentralBank.Services.Hosts.Command {
    public class CurrencyQuotationCommand {
        public string Id { get; set; }
        public string Simbolo { get; set; }
        public float ParidadeCompra { get; set; }
        public float ParidadeVenda { get; set; }
        public float CotacaoCompra { get; set; }
        public float CotacaoVenda { get; set; }
        public DateTime DataHoraCotacao { get; set; }
        public string TipoBoletim { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
    }
}
