using CoolSms;
using LittleBakery.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LittleBakery.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SmsController : ControllerBase
    {
        private SmsApi _smsApi;

        public SmsController(IOptions<CoolSmsConfig> config)
        {
            _smsApi = new SmsApi(new SmsApiOptions
            {
                ApiKey = config.Value.ApiKey,
                ApiSecret = config.Value.ApiSecret,
                DefaultSenderId = config.Value.SenderId
            });
        }

        [HttpPost]
        public IActionResult SendVerificationNumber(VerificationRequest verificationRequest)
        {
            return new ObjectResult(verificationRequest);
            // SendMessageResponse sendMessageResponse = await _smsApi.SendMessageAsync(phoneNumber, "Hello");
            // 
            // return new ObjectResult(sendMessageResponse);
        }
    }
}
