using System.Net;
using System.Net.Mail;
using System.Text;
using ETicaretAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;

namespace ETicaretAPI.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
    {
        await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
    }

    public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
    {
        MailMessage mail = new();
        mail.IsBodyHtml = isBodyHtml;
        foreach (var to in tos)
        {
            mail.To.Add(to);
        }

        mail.Subject = subject;
        mail.Body = body;
        mail.From = new MailAddress(_configuration["Mail:Username"], "ERYA", System.Text.Encoding.UTF8);

        SmtpClient smtp = new();
        smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
        smtp.Port = 587;
        smtp.EnableSsl = true;
        smtp.Host = _configuration["Mail:Host"];
        await smtp.SendMailAsync(mail);
    }

    public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
    {
        var link = $"{_configuration["AngularClientUrl"]}/update-password/{userId}/{resetToken}";

        
        var mail = new StringBuilder();
        mail.AppendLine("Hello,<br> You have requested a password reset. Please click the link below to reset your password.");
        mail.AppendLine($"<br><strong><a target=\"_blank\" href=\"{link}\">Click, for the new password request...</a></strong>");
        mail.AppendLine("<br><br><span style=\"color:red;\">This link will expire in 1 hour.</span>");
        mail.AppendLine("<br>Best Regards,<br>Team ERYA");

        
        await SendMailAsync(to, "Password Reset Request", mail.ToString());
        
    }
}



// StringBuilder mail = new StringBuilder();
// mail.AppendLine("Hello,<br> You have requested a password reset." +
//                 " Please click the link below to reset your password." +
//                 "<br><strong><a target=\"_blank\"href=\"");
// mail.AppendLine(_configuration["AngularClientUrl"]);
// mail.AppendLine(_configuration["/update-password/"]);
// mail.AppendLine(userId);
// mail.AppendLine("/");
// mail.AppendLine(resetToken);
// mail.AppendLine("\">Click, for the new password request...</a></strong><br><br><span " +
//                 "style=\"color:red;\">" +
//                 "This link will expire in 1 hour.</span><br>Best Regards,<br>Team ERYA");
//
// var link = mail.ToString();
// await SendMailAsync(to, "Password Reset Request", mail.ToString());