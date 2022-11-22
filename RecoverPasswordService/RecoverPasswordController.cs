using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Steeltoe.Discovery.Client;

namespace RecoverPasswordService;

[ApiController]
[Route("pass")]
public class RecoverPasswordController : ControllerBase
{
    [HttpPost]
    [Route("sendEmail")]
    public IActionResult SendEmail([FromBody] string email)
    {
        var customEmail = new MimeMessage();
        customEmail.From.Add(MailboxAddress.Parse("e21350538@gmail.com")); //EMAIL FROM WHERE WE SEND

        customEmail.To.Add(MailboxAddress.Parse(order.Account.Email)); //EMAIL WHERE ITS GOING TO
    
        customEmail.Subject = "Thank you for your order: " + order.Account.Name + "!";
        customEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) {Text = order.ToString()};

        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
        //email: e21350538@gmail.com
        //password: zyhWb4ypXsetfCcFYS
        //password2: pmlovovxulermzzf
        smtp.Authenticate("e21350538@gmail.com", "pmlovovxulermzzf"); //EMAIL FROM WHERE WE SEND
        smtp.Send(customEmail);
        smtp.Disconnect(true);
        return Results.Ok("Sent Email");
    }
}