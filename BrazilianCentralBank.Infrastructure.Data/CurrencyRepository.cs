using System;
using System.Text;
using System.Collections.Generic;

using Dapper;

using System.Threading.Tasks;
using System.Data.SqlClient;

using BrazilianCentralBank.Domain.Entities;
using BrazilianCentralBank.Infrastructure.Data.Interfaces;

namespace BrazilianCentralBank.Infrastructure.Data {
    public class CurrencyRepository : ICurrencyRepository {
        private readonly IRepositoryBase _repositoryBase;
        public CurrencyRepository(IRepositoryBase repositoryBase) {
            this._repositoryBase = repositoryBase;
        }

        public async Task<Currency> Authenticate(Currency currency) {
            try {
                using (var connection = new SqlConnection(this._repositoryBase.ConnectionString)) {
                    return await connection.QuerySingleAsync<Currency>(
                            "Select                                             " + Environment.NewLine +
                            "convert(nvarchar(50), C.[Id])  as Id,              " + Environment.NewLine +
                            "C.[Simbolo]                    as Simbolo,         " + Environment.NewLine +
                            "C.[NomeFormatado]              as NomeFormatado,   " + Environment.NewLine +
                            "C.[TipoMoeda]                  as TipoMoeda        " + Environment.NewLine +
                            "from[Currency] C                                   " + Environment.NewLine +
                            "where C.[Simbolo] = @simbolo                       ", 
                            new { simbolo = currency.Simbolo });
                }
            } catch (Exception error) {

                throw error;
            }
        }

        public async Task<List<Currency>> SelectCurrency() {
            try {
                using (var connection = new SqlConnection(this._repositoryBase.ConnectionString)) {
                    return (List<Currency>)await connection.QueryAsync<Currency>(
                            "Select                                             " + Environment.NewLine +
                            "convert(nvarchar(50), C.[Id])  as Id,              " + Environment.NewLine +
                            "C.[Simbolo]                    as Simbolo,         " + Environment.NewLine +
                            "C.[NomeFormatado]              as NomeFormatado,   " + Environment.NewLine +
                            "C.[TipoMoeda]                  as TipoMoeda        " + Environment.NewLine +
                            "from[Currency] C ");
                }
            } catch (Exception error) {

                throw error;
            }
        }
    }
}
