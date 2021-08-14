using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using System.Threading.Tasks;

using BrazilianCentralBank.Application.Helper;
using BrazilianCentralBank.Application.Interfaces;
using BrazilianCentralBank.Application.ViewModels;
using BrazilianCentralBank.Infrastructure.Data.Interfaces;
using BrazilianCentralBank.Domain.Entities;

namespace BrazilianCentralBank.Application {
    public class CurrencyQuotationServices : ICurrencyQuotationServices {
        private readonly ICurrencyQuotationRepository _CurrencyQuotationRepository;
        private readonly IMapper _mapper;

        public CurrencyQuotationServices(ICurrencyQuotationRepository currencyQuotationRepository, IMapper mapper) {
            this._CurrencyQuotationRepository = currencyQuotationRepository;
            this._mapper = mapper;
        }

        public async Task<AuthResponseObjHelper> SelectCurrencyQuotationSevice(CurrencyQuotationViewModel curruncyViewModel) {
            try {

                CurrencyQuotation currencyQuotation = this._mapper.Map<CurrencyQuotation>(
                    new CurrencyQuotationViewModel { Simbolo = curruncyViewModel.Simbolo, DataInicial = Convert.ToDateTime(curruncyViewModel.DataInicial),
                                                        DataFinal = Convert.ToDateTime(curruncyViewModel.DataFinal)});

                List<CurrencyQuotationViewModel> currencyQuotations = this._mapper.Map<List<CurrencyQuotationViewModel>>(
                    await this._CurrencyQuotationRepository.SelectCurrencyQuotation(currencyQuotation));

                if (currencyQuotations != null)
                    return new AuthResponseObjHelper() {status_code = 200, http_message = $"Dados das cotações retornados com sucesso.", currencyQuotationsList = currencyQuotations };

                return new AuthResponseObjHelper() { status_code = 501, http_message = $"Erro ao carregar os dados.", currencyQuotationsList = currencyQuotations };

            } catch (Exception error) {

                return new AuthResponseObjHelper() { status_code = 500, http_message = $"{error.Message}" };
            }
        }
    }
}
