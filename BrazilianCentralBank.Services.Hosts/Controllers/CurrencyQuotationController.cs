using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;

using BrazilianCentralBank.Application.Helper;
using BrazilianCentralBank.Application.Interfaces;
using BrazilianCentralBank.Services.Hosts.Command;
using BrazilianCentralBank.Application.ViewModels;

namespace BrazilianCentralBank.Services.Hosts.Controllers {
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CurrencyQuotationController : ControllerBase {
        ICurrencyQuotationServices _currencyQuotationServices;
        IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencyQuotationServices"></param>
        /// <param name="mapper"></param>
        public CurrencyQuotationController(ICurrencyQuotationServices currencyQuotationServices, IMapper mapper) {
            this._currencyQuotationServices = currencyQuotationServices;
            this._mapper = mapper;
        }

        /// <summary>
        /// Metodo responsável por trazer a taxa de câmbio, filtrando por tipo da moeda "USD" e Data da pesquisa "14-08-2021"
        /// É necessário o Tokem de Acesso para essa consulta
        /// </summary>
        /// <param name="Simbolo">USD</param>
        /// <param name="DataInicial">14-08-2021</param>
        /// <param name="DataFinal">14-08-2021</param>
        /// <returns></returns>
        [HttpGet("Cambio")]
        [Authorize]
        public async Task<IActionResult> CurrencyType(string Simbolo, string DataInicial, string DataFinal) {
            try {

                CurrencyQuotationViewModel currencyQuotationModel = this._mapper.Map<CurrencyQuotationViewModel>(
                    new CurrencyQuotationCommand { Simbolo = Simbolo, DataInicial = DataInicial, DataFinal = DataFinal });

                AuthResponseObjHelper authResult = await this._currencyQuotationServices.SelectCurrencyQuotationSevice(currencyQuotationModel);

                if (authResult.status_code == 200)
                    return StatusCode(authResult.status_code, new { StatusDescription = authResult.http_message, AccessToken = authResult.currencyQuotationsList });

                return StatusCode(authResult.status_code, new { StatusDescription = authResult.http_message });


            } catch (Exception error) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"ERROR {error.Message}");
            }
        }
    }
}
