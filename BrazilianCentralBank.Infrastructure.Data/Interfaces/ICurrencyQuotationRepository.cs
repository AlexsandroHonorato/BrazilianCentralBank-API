using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using BrazilianCentralBank.Domain.Entities;



namespace BrazilianCentralBank.Infrastructure.Data.Interfaces {
    public interface ICurrencyQuotationRepository {
        Task<List<CurrencyQuotation>> SelectCurrencyQuotation(CurrencyQuotation currencyQuotation);
        Task<List<Currency>> SelectCurrency(Currency currency);
    }
}
