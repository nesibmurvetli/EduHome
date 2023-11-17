﻿using System.Net.Mail;
using System.Net;

namespace EduHome.Helpers
{
    public  static class Helper
    {
        public static async Task SendMailAsync(string messageSubject, string messageBody, string mailTo)
        {
            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("nasib.m@itbrains.edu.az", "rcjjpnyjxivvtmqa");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage message = new MailMessage("nasib.m@itbrains.edu.az", mailTo);
            message.Subject = messageSubject;
            message.Body = messageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            await client.SendMailAsync(message);
        }
        public enum Roles
        {
            SuperAdmin,
            Admin,
            Member
        }
    }
}
