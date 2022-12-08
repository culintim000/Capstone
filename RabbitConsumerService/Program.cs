using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using MailKit.Net.Smtp;
using MimeKit;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare(queue: "hello",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine(" [x] Received {0}", message);
        
    var messageSplit = message.Split(' ');
    var code = messageSplit[1];
    var email = messageSplit[0];

    var customEmail = new MimeMessage();
    customEmail.From.Add(MailboxAddress.Parse("e21350538@gmail.com")); //EMAIL FROM WHERE WE SEND
        
    customEmail.To.Add(MailboxAddress.Parse(email)); //EMAIL WHERE ITS GOING TO
        
    customEmail.Subject = "Password Recovery for Paws in Good Hands.";
    customEmail.Body = new TextPart(MimeKit.Text.TextFormat.Html) {Text = "Your verification code is: " + code};
        
    using var smtp = new SmtpClient();
    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
    //email: e21350538@gmail.com
    //password: zyhWb4ypXsetfCcFYS
    //password2: pmlovovxulermzzf
    //passwordForCapstone: xsmqvykpipkjvggd
    smtp.Authenticate("e21350538@gmail.com", "xsmqvykpipkjvggd"); //EMAIL FROM WHERE WE SEND
    smtp.Send(customEmail);
    Console.WriteLine("Email sent");
    smtp.Disconnect(true);
};
channel.BasicConsume(queue: "hello",
    autoAck: true,
    consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();