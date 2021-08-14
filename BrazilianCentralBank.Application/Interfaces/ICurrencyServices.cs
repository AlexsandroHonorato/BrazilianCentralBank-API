using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

using BrazilianCentralBank.Application.Helper;
using BrazilianCentralBank.Application.ViewModels;

namespace BrazilianCentralBank.Application.Interfaces {
    public interface ICurrencyServices {
        Task<AuthResponseObjHelper> SelectCurrencyService();
        Task<AuthResponseObjHelper> AuthenticateSevice(CurrencyViewModel currency);
    }
}
