using Microsoft.AspNetCore.Identity;
using OTP.Models;
using OTP.Models.DTO;
using StudentForm.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTP.Services
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);
        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);
        //Task<IdentityResult> SignUpAsync(SignUp signup);
        //Task<IdentityResult> ConfirmEmailAsync(string email, string token);
        //bool ConfirmEmail(string email, string token);
      //  Task SendEmailConfirmationEmail(ApplicationUser user, string token);

    }
}
