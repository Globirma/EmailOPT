using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OTP.Data;
using OTP.Models;
using OTP.Models.DTO;
using StudentForm.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTP.Services
{
    public class VerifyEmailService: IVerifyEmailService

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _ctx;
        private readonly IEmailService _emailService;
        public VerifyEmailService(IOptions<SMTPConfigModel> smtpConfig, IConfiguration configuration,
            UserManager<ApplicationUser> userManager, DataContext ctx, IEmailService emailService)
        {
         
            _userManager = userManager;
            _configuration = configuration;
            _ctx = ctx;
            _emailService = emailService;
        }
        public async Task<IdentityResult> SignUpAsync(SignUp signup)
        {
            var user = new ApplicationUser()
            {

                Email = signup.Email,
                UserName = signup.Email
            };

            Random random = new Random();
            string _code = random.Next(100000, 999999).ToString();
            user.Otp = _code;





            var result = await _userManager.CreateAsync(user, signup.Password);
            if (result.Succeeded)
            {

                if (!string.IsNullOrEmpty(_code))
                {
                    
                    await SendEmailConfirmationEmail(user, _code);
                }
            }
            return result;
        }

        //public async Task<IdentityResult> ConfirmEmailAsync(string email, string code)
        //{
        //    return await _userManager.ConfirmEmailAsync(await _userManager.FindByEmailAsync(email), code);

        //}




        public async Task<string>VerifyEmailAsync(string email, string code)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user.Otp != code)
                return "Invalid Otp";

            user.EmailConfirmed = true;        

            var result =  await _userManager.UpdateAsync(user);

            if (result.Succeeded)
                return "Success";

            return "network error";
        }


        private async Task SendEmailConfirmationEmail(ApplicationUser user, string code)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;
            UserEmailOptions options = new UserEmailOptions
            {
                toEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                      {
                          new KeyValuePair<string, string>("{{UserName}}",/*user.FirstName*/ "Glory"),
                          new KeyValuePair<string, string>("{{Link}}",
                          string.Format(appDomain + confirmationLink, user.Id, code))
                      }

            };
            await _emailService.SendEmailForEmailConfirmation(options);
        }
    }


}
    

