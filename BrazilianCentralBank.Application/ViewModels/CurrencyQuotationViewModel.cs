using System;
using System.Collections.Generic;
using System.Text;

namespace BrazilianCentralBank.Application.ViewModels {
    public class CurrencyQuotationViewModel {
        public string Id { get; set; }
        public string Simbolo { get; set; }
        public float ParidadeCompra { get; set; }
        public float ParidadeVenda { get; set; }
        public float CotacaoCompra { get; set; }
        public float CotacaoVenda { get; set; }
        public DateTime DataHoraCotacao { get; set; }
        public string TipoBoletim { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
    }
}
