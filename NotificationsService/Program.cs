using Microsoft.Extensions.Configuration;
using NotificationsService.Entities;
using NotificationsService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace NotificationsService
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Press any key to send message");
            Console.ReadKey();
            try
            {
                SentEmailWithoutAttachmentsPass();
                SentEmailWithAttachmentsPass();
                Console.WriteLine("Success!");
                Console.ReadKey();

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message, ex.StackTrace);
            }
        }

        private static void SentEmailWithoutAttachmentsPass()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var mail = config.GetSection("NotificationServiceAccountEmail").Value;
            var pass = config.GetSection("NotificationServiceAccountPassword").Value;
            var to = config.GetSection("SendTo").Value;


            var notificationProvider = new ExchangeWebServicesNotificationDeliveryProvider(mail, pass);


            Console.WriteLine("Sent Email Without Attachments Pass");

            var expectedNotification = new EmailNotification
            {
                To = new List<string> { to },

                Subject = "Expected subject",
                Body = "Expected body",
                Cc = new List<string>(),
                IsHtml = false
            };

            notificationProvider.SendEmail(expectedNotification);

            Console.WriteLine("Done");

        }

        private static void SentEmailWithAttachmentsPass()
        {

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var mail = config.GetSection("NotificationServiceAccountEmail").Value;
            var pass = config.GetSection("NotificationServiceAccountPassword").Value;
            var to = config.GetSection("SendTo").Value;


            var notificationProvider = new ExchangeWebServicesNotificationDeliveryProvider(mail, pass);


            Console.WriteLine("Sent Email Attachments");

            var expectedAttachmentId = "expectedAttachmentId";
            var expectedAttachmentName = "expectedAttachmentName.html";
            var expectedAttachmentType = ".html";
            var expectedAttachmentContent = new byte[] { 233, 122, 23 };

            var expectedNotification = new EmailNotification
            {
                To = new List<string> { to },
                Subject = "Expected subject with file",
                Body = "Expected body with file",
                Cc = new List<string>(),
                IsHtml = false,
                AttachmentIds = new List<string> { expectedAttachmentId }
            };

            var emailAttachment = new EmailAttachment
            {
                FileName = expectedAttachmentName,
                ContentBytes = expectedAttachmentContent,
                ContentType = expectedAttachmentType
            };

            List<IEmailAttachment> emailAttachments = new List<IEmailAttachment>();
            emailAttachments.Add(emailAttachment);

            notificationProvider.SendEmail(expectedNotification, emailAttachments);

            Console.WriteLine("Done");

        }

    }
}
