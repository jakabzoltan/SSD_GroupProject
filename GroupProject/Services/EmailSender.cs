using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GroupProject.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public IConfiguration Configuration { get; set; }

        public EmailSender(IConfiguration configuration){
            Configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress(Configuration["Email"]);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                // Plug in your email service here to send an email.
                using (var smpt = new SmtpClient())
                {
                    var credential = new NetworkCredential()
                    {
                        UserName = Configuration["Email"],
                        Password = Configuration["PasswordField"]
                    };
                    smpt.Credentials = credential;
                    smpt.Host = "smtp.gmail.com";
                    smpt.Port = 587;
                    smpt.EnableSsl = true;
                    smpt.Send(mail);
                }
                return Task.CompletedTask;
                //SSD.GroupProject.Z.I.M.C@gmail.com
                /*SmtpClient client = new SmtpClient("smtp.gmail.com", 465);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(Configuration["Email"], Configuration["PasswordField"]);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(Configuration["Email"]);
                mailMessage.To.Add(new MailAddress(email));
                mailMessage.Body = message;
                mailMessage.Subject = subject;
                client.Send(mailMessage);

                return Task.CompletedTask;*/
            } catch(Exception e)
            {
                return Task.CompletedTask;
            }
            
        }
    }
}
