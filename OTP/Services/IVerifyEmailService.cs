using Microsoft.AspNetCore.Identity;
using OTP.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OTP.Services
{
    public interface IVerifyEmailService
    {
        Task<IdentityResult> SignUpAsync(SignUp signup);
        //Task<IdentityResult> ConfirmEmailAsync(string email, string token);
        Task<string> VerifyEmailAsync(string email, string code);
    }
}
