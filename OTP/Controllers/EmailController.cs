using Microsoft.AspNetCore.Mvc;
using OTP.Data;
using OTP.Models;
using OTP.Models.DTO;
using OTP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTP.Controllers
{
    public class EmailController : Controller
    {
        private readonly IVerifyEmailService _verifyEmail ;
        private readonly DataContext _ctx;
        private readonly IEmailService _emailService;
        public EmailController(IVerifyEmailService verifyEmail, DataContext ctx, IEmailService emailService)
        {
            _verifyEmail = verifyEmail;
            _ctx = ctx;
            _emailService = emailService;

        }


        [HttpPost]
        [Route("SendMail")]
        public async Task<IActionResult> EmailSend()
        {
            UserEmailOptions options = new UserEmailOptions
            {
                toEmails = new List<string>() { "compliance@hydeenergyltd.com" },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", "{{UserName}}")
                }
            };
            await _emailService.SendTestEmail(options);
            return Ok();
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(SignUp signup)
        {
            if (ModelState.IsValid)
            {
                var result = await _verifyEmail.SignUpAsync(signup);

                if (result.Succeeded)
                {
                    return Ok(signup);
                }
            }
            return BadRequest();

        }
    

        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string code, string email)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(code))
            {

                var result = await _verifyEmail.VerifyEmailAsync(email, code);
                return Ok(result);
            }

            return BadRequest("Email and code are required");


        }


    }
}
