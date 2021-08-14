using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

using BrazilianCentralBank.Application.Helper;
using BrazilianCentralBank.Application.Interfaces;
using BrazilianCentralBank.Infrastructure.Data.Interfaces;
using BrazilianCentralBank.Application.ViewModels;
using BrazilianCentralBank.Domain.Entities;



namespace BrazilianCentralBank.Application {
    public class CurrencyServices : ICurrencyServices {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CurrencyServices(ICurrencyRepository currencyRepository, IMapper mapper, IConfiguration configuration) {
            this._currencyRepository = currencyRepository;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public async Task<AuthResponseObjHelper> AuthenticateSevice(CurrencyViewModel currencymodel) {
            try {

                Currency currency = this._mapper.Map<Currency>(currencymodel);

                CurrencyViewModel autenticate = this._mapper.Map<CurrencyViewModel>( await this._currencyRepository.Authenticate(currency));

                if (autenticate != null)
                    return new AuthResponseObjHelper() {status_code = 200, Token = GenerateJWTToken(autenticate), http_message = $"Token gerado com sucesso.", currency = autenticate };

                return new AuthResponseObjHelper() { status_code = 501, http_message = $"Erro ao carregar os dados.", currency = autenticate };

            } catch (Exception error) {

                return new AuthResponseObjHelper() { status_code = 500, http_message = $"{error.Message}" };
            }
        }

        public async Task<AuthResponseObjHelper> SelectCurrencyService() {
            try {
                List<CurrencyViewModel> currencies = this._mapper.Map<List<CurrencyViewModel>>( await this._currencyRepository.SelectCurrency()); 

                if(currencies != null)
                    return new AuthResponseObjHelper(){ status_code = 200, http_message = $"Dados das moedas retornados com sucesso.", currencyList = currencies};

                return new AuthResponseObjHelper() { status_code = 501, http_message = $"Erro ao carregar os dados.", currencyList = currencies };

            } catch (Exception error) {

                return new AuthResponseObjHelper() { status_code = 500, http_message = $"{error.Message}" };
            }
        }

        private string GenerateJWTToken(CurrencyViewModel currency) {
            string issuer = this._configuration["AppSettings:Issuer"];
            string audience = this._configuration["AppSettings:Audience"];

            //Aqui cria as credenciais (Roles)
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, currency.Simbolo)           
            };
            //Aqui cria uma criptografia com base no token do AppSetting
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this._configuration["AppSettings:Token"]));
            //Aqui cria a Credencial, como base no tipo da chave especificada (a Key + HmacSha512Signature)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            //Aqui gera a descrição do Token
            var tokenDrescription = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = creds,
                Issuer = issuer,
                Audience = audience

            };
            //Aqui gera o Token
            var tokenHandler = new JwtSecurityTokenHandler();
            //Aqui cria o Token
            var token = tokenHandler.CreateToken(tokenDrescription);

            return tokenHandler.WriteToken(token);
        }

    }
}
