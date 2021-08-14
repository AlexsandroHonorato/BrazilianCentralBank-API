
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using BrazilianCentralBank.Domain.Entities;

namespace BrazilianCentralBank.Infrastructure.Data.Interfaces {
    public interface ICurrencyRepository {
        Task<Currency> Authenticate(Currency currency);
        Task<List<Currency>> SelectCurrency();
    }
}
