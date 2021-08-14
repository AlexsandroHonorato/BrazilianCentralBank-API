using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrazilianCentralBank.Services.Hosts.Command {
    /// <summary>
    /// Camando para pegar os dados das moedas 
    /// </summary>
    public class CurrencyCommand {     
        /// <summary>
        /// Simbolo da moeda
        /// </summary>
        public string Simbolo { get; set; }
   
    }
}
