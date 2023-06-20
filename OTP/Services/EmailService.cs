
using Microsoft.AspNetCore.Identity;
using MimeKit;
using OTP.Data;
using OTP.Models;
using OTP.Models.DTO;
using StudentForm.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace OTP.Services
{
    public class EmailService : IEmailService

    {
        private readonly SMTPConfigModel _smtpConfig;
        private const string templatePath = @"EmailTemplate/{0}.html";
       

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
            
        }
        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello,This mail is from management", userEmailOptions.PlaceHolders);

            //"this a test email subject"; 
            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("textEmail"), userEmailOptions.PlaceHolders);
            // GetEmailBody("textEmail");
            await SendEmail(userEmailOptions);
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_smtpConfig.SenderDisplayNames, _smtpConfig.SenderAddress);
                    emailMessage.From.Add(emailFrom);

                    foreach (var toEmail in userEmailOptions.toEmails)
                    {
                        MailboxAddress emailTo = new MailboxAddress(toEmail, toEmail);
                        emailMessage.To.Add(emailTo);
                    }



                    emailMessage.Subject = userEmailOptions.Subject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();

                    emailBodyBuilder.HtmlBody = userEmailOptions.Body;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (MailKit.Net.Smtp.SmtpClient mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        mailClient.Connect(_smtpConfig.Host, _smtpConfig.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_smtpConfig.UserName, _smtpConfig.Password);
                        await mailClient.SendAsync(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //This get the html template
        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }


        private string UpdatePlaceHolders(String text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }
            return text;
        }

       



       async Task IEmailService.SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions)
        {
            { 
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}},Confirm your email", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }
    }

        
    }

   
}


