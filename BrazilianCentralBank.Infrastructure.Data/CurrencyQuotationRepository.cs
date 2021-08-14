using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

using Dapper;

using BrazilianCentralBank.Domain.Entities;
using BrazilianCentralBank.Infrastructure.Data.Interfaces;


namespace BrazilianCentralBank.Infrastructure.Data {
    public class CurrencyQuotationRepository : ICurrencyQuotationRepository {
        private readonly IRepositoryBase _repositoryBase;

        public CurrencyQuotationRepository(IRepositoryBase repositoryBase) {
            this._repositoryBase = repositoryBase;
        }   

        public async Task<List<CurrencyQuotation>> SelectCurrencyQuotation(CurrencyQuotation currencyQuotation) {
            try {
                string lstrSelect = string.Empty;
                string lstrWhere = string.Empty;

                using (var connection = new SqlConnection(this._repositoryBase.ConnectionString)) {
                    lstrSelect = $" select                                              " + Environment.NewLine +
                                    "convert(nvarchar(50), CQ.[Id]) as Id,              " + Environment.NewLine +
                                    "CQ.[Simbolo]                   as Simbolo,         " + Environment.NewLine +
                                    "CQ.[ParidadeCompra]            as ParidadeCompra,  " + Environment.NewLine +
                                    "CQ.[CotacaoVenda]              as CotacaoVenda,    " + Environment.NewLine +
                                    "CQ.[CotacaoCompra]             as CotacaoCompra,   " + Environment.NewLine +
                                    "CQ.[CotacaoVenda]              as CotacaoVenda,    " + Environment.NewLine +
                                    "CQ.[DataHoraCotacao]           as DataHoraCotacao, " + Environment.NewLine +
                                    "CQ.[TipoBoletim]               as TipoBoletim      " + Environment.NewLine +
                                    "from [CurrencyQuotation] CQ                        " + Environment.NewLine;

                    lstrWhere = getWhereFilter(currencyQuotation);

                    if (lstrWhere != string.Empty)
                        lstrSelect += " WHERE " + Environment.NewLine + lstrWhere;

                    return (List<CurrencyQuotation>)await connection.QueryAsync<CurrencyQuotation>(lstrSelect);
                }

            } catch (Exception error) {

                throw error;
            }
        }

        private string getWhereFilter(CurrencyQuotation currencyQuotation) {
            try {
                string lstrWhere = string.Empty;

                if (currencyQuotation.Simbolo != String.Empty)
                    lstrWhere = $" CQ.[Simbolo] = '{currencyQuotation.Simbolo}'" + Environment.NewLine;

                if (currencyQuotation.DataInicial != DateTime.MinValue && currencyQuotation.DataFinal != DateTime.MinValue) {
                    if (lstrWhere != string.Empty)
                        lstrWhere += " and ";

                    lstrWhere += $"CQ.[DataHoraCotacao] BETWEEN " + Environment.NewLine;
                    lstrWhere += $"convert(datetime,  '{currencyQuotation.DataInicial.ToString("yyyy-MM-dd 00:00:00")}',101)  " + Environment.NewLine;
                    lstrWhere += $"and convert(datetime,  '{currencyQuotation.DataFinal.ToString("yyyy-MM-dd 23:59:59")}',101)" + Environment.NewLine;
                }

                return lstrWhere;

            } catch (Exception error) {

                throw error;
            }
        }
    }
}
