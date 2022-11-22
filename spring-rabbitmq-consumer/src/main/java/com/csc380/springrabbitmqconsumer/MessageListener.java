package com.csc380.springrabbitmqconsumer;

import net.minidev.json.JSONObject;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;
import javax.mail.*;
import javax.mail.internet.*;

import java.nio.charset.StandardCharsets;
import java.util.Properties;

@Component
public class MessageListener {

    @RabbitListener(queues = MQConfig.QUEUE)
    public void listener(String string)throws MessagingException {
        System.out.println("test");
        System.out.println(string);
//        String emailReceiver = object.get("email").toString();
//        String emailSubject = object.get("subject").toString();
//        String emailBody = object.get("body").toString();
//
//        Properties prop = new Properties();
//        prop.put("mail.smtp.auth", true);
//        prop.put("mail.smtp.starttls.enable", "true");
//        prop.put("mail.smtp.ssl.trust", "*");
//        prop.put("mail.smtp.ssl.protocols", "TLSv1.2");
//        prop.put("mail.smtp.host", "smtp.gmail.com");
//        prop.put("mail.smtp.port", "587");
//
//        Session session = Session.getInstance(prop, new Authenticator() {
//            @Override
//            protected PasswordAuthentication getPasswordAuthentication() {
//                return new PasswordAuthentication("noreply.tkahrs.automatic@gmail.com", "ckfwuswhxifckvzp");
//            }
//        });
//
//        Message message = new MimeMessage(session);
//        message.setFrom(new InternetAddress("noreply.tkahrs.automatic@gmail.com"));
//        message.setRecipients(
//                Message.RecipientType.TO, InternetAddress.parse(emailReceiver));
//        message.setSubject(emailSubject);
//
//
//        MimeBodyPart mimeBodyPart = new MimeBodyPart();
//        mimeBodyPart.setContent(emailBody, "text/html; charset=utf-8");
//
//        Multipart multipart = new MimeMultipart();
//        multipart.addBodyPart(mimeBodyPart);
//
//        message.setContent(multipart);
//
//        Transport.send(message);

    }
}
