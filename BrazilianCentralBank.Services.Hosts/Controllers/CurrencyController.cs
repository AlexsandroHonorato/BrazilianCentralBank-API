using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using System.Threading.Tasks;

using AutoMapper;

using BrazilianCentralBank.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using BrazilianCentralBank.Services.Hosts.Command;
using BrazilianCentralBank.Application.ViewModels;
using BrazilianCentralBank.Application.Helper;

namespace BrazilianCentralBank.Services.Hosts.Controllers {
    /// <summary>
    /// 
    /// </summary>

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]  
    public class CurrencyController : ControllerBase {

        private readonly ICurrencyServices _currencyServices;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencyServices"></param>
        /// <param name="mapper"></param>
        public CurrencyController(ICurrencyServices currencyServices, IMapper mapper) {
            this._currencyServices = currencyServices;
            this._mapper = mapper;
        }
        /// <summary>
        /// Metodo responsável para gerar o Token de Acesso
        /// </summary>
        /// <param name="Simbolo">USD</param>
        /// <returns></returns>
        [HttpGet("AccessToken")]     
        [AllowAnonymous]
        public async Task<IActionResult> AccessToken(string Simbolo) {
            try {

                CurrencyViewModel currencyModel = this._mapper.Map<CurrencyViewModel>(new CurrencyCommand { Simbolo = Simbolo });

                AuthResponseObjHelper authResult = await this._currencyServices.AuthenticateSevice(currencyModel);

                if (authResult.status_code == 200)
                    return StatusCode(authResult.status_code, new { StatusDescription = authResult.http_message, AccessToken = authResult.Token, TipoModeda = authResult.currency });

                return StatusCode(authResult.status_code, new { StatusDescription = authResult.http_message });


            } catch (Exception error) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"ERROR {error.Message}");
            }
        }

        /// <summary>
        /// Metodo responsável por trazer as informações das moedas 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Moedas")]
        [Authorize]     
        public async Task<IActionResult> CurrencyType() {
            try { 

                AuthResponseObjHelper authResult = await this._currencyServices.SelectCurrencyService();

                if (authResult.status_code == 200)
                    return StatusCode(authResult.status_code, new { StatusDescription = authResult.http_message, AccessToken = authResult.currencyList });

                return StatusCode(authResult.status_code, new { StatusDescription = authResult.http_message });


            } catch (Exception error) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"ERROR {error.Message}");
            }
        }

    }
}
