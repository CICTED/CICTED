using CICTED.Domain.Infrastucture.Services.Interfaces;
using CICTED.Domain.Models.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CICTED.Domain.Infrastucture.Services
{
    public class SmsService : ISmsService
    {
        #region Constructors and injections
        
        private CustomSettings _settings;
        public SmsService(IOptions<CustomSettings> settings)
        {
            _settings = settings.Value;
        }

        #endregion

        public async Task SendAccountConfirmation(string phone, string code)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = $"{_settings.TwillioURL}/Accounts/{_settings.TwillioAccountSID}/Messages.json";

                    var byteArray = Encoding.ASCII.GetBytes($"{_settings.TwillioAccountSID}:{_settings.TwillioToken}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    var content = new FormUrlEncodedContent(new[]
                    {
             new KeyValuePair<string, string>("To", phone),
             new KeyValuePair<string, string>("From", _settings.TwillioNumber),
             new KeyValuePair<string, string>("Body", $"Seu código de confirmação do CICTED é {code}")
        });

                    await client.PostAsync(url, content);
                }
            }
            catch(Exception ex)
            {
                var a = ex.Message;
            }
        }
    }
}
