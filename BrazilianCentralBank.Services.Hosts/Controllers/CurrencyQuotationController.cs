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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CurrencyQuotationController : ControllerBase {
        ICurrencyQuotationServices _currencyQuotationServices;
        IMapper _mapper;

        public CurrencyQuotationController(ICurrencyQuotationServices currencyQuotationServices, IMapper mapper) {
            this._currencyQuotationServices = currencyQuotationServices;
            this._mapper = mapper;
        }

        [HttpPost("Cambio")]
        [Authorize]
        public async Task<IActionResult> CurrencyType(CurrencyQuotationCommand quotationCommand) {
            try {

                CurrencyQuotationViewModel currencyQuotationModel = this._mapper.Map<CurrencyQuotationViewModel>(quotationCommand);

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
