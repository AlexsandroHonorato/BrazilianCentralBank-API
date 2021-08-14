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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CurrencyController : ControllerBase {
        private readonly ICurrencyServices _currencyServices;
        private readonly IMapper _mapper;

        public CurrencyController(ICurrencyServices currencyServices, IMapper mapper) {
            this._currencyServices = currencyServices;
            this._mapper = mapper;
        }

        [HttpPost("AccessToken")]
        [AllowAnonymous]
        public async Task<IActionResult> AccessToken([FromBody] CurrencyCommand currencyCommand) {
            try {

                CurrencyViewModel currencyModel = this._mapper.Map<CurrencyViewModel>(currencyCommand);

                AuthResponseObjHelper authResult = await this._currencyServices.AuthenticateSevice(currencyModel);

                if (authResult.status_code == 200)
                    return StatusCode(authResult.status_code, new { StatusDescription = authResult.http_message, AccessToken = authResult.Token, TipoModeda = authResult.currency });

                return StatusCode(authResult.status_code, new { StatusDescription = authResult.http_message });


            } catch (Exception error) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"ERROR {error.Message}");
            }
        }

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
