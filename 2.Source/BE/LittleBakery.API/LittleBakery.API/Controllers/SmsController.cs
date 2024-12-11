using CoolSms;
using LittleBakery.API.Models;
using LittleBakery.API.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;

namespace LittleBakery.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SmsController : ControllerBase
    {
        private SmsApi _smsApi;
        private Dictionary<string, VerificationNumber> _verificationDic = new Dictionary<string, VerificationNumber>();

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
        public async Task<IActionResult> SendVerificationNumber(VerificationRequest verificationRequest)
        {
            Random rand = new Random();
            string randVal = rand.Next(10_000_000, 100_000_000).ToString();

            VerificationNumber verificationNumber = new VerificationNumber(randVal);

            VerificationRepository.Upsert(verificationRequest.UUID, verificationNumber);

            // return new ObjectResult(verificationRequest);
            SendMessageResponse sendMessageResponse = await _smsApi.SendMessageAsync(verificationRequest.PhoneNumber, $"인증번호는 [{verificationNumber.Number}]입니다. 정확히 입력해주세요.");
            
            return new ObjectResult(sendMessageResponse);
        }

        [HttpPost]
        public IActionResult VerifyNumber(Verification verification)
        {
            if (!VerificationRepository.Verify(verification.UUID, verification.Number))
                return NotFound();
                        
            VerificationRepository.Delete(verification.UUID);

            string guid = Guid.NewGuid().ToString();

            // TODO: GUID로 유저 정보 DB에 저장

            Response.Cookies.Append("USER", guid, new CookieOptions { Expires = DateTimeOffset.MaxValue});

            return Ok();
        }
    }
}
