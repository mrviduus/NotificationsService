using Microsoft.Exchange.WebServices.Data;
using NotificationsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsService
{
    public class ExchangeWebServicesNotificationDeliveryProvider
    {
        private readonly string userMail;

        public readonly string userPass;

        public ExchangeWebServicesNotificationDeliveryProvider(string userMail, string userPass)
        {           

            if (userMail == null || userPass == null)
                throw new ArgumentNullException();

            this.userMail = userMail;
            this.userPass = userPass;
        }

        public void SendEmail(IEmailNotification mail)
        {
            try
            {
                var message = BuildMail(mail);

                SendEmailInternal(message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, ex.StackTrace);
                throw;
            }
        }

        public void SendEmail(IEmailNotification mail, IEnumerable<IEmailAttachment> emailAttachments)
        {
            try
            {
                var message = BuildMail(mail);

                if (emailAttachments.Any())
                {
                    foreach (var emailAttachment in emailAttachments)
                    {
                        message.Attachments.AddFileAttachment(emailAttachment.FileName, emailAttachment.ContentBytes);
                    }
                }

                SendEmailInternal(message);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message, ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Build a EmailMessage
        /// </summary>
        /// <param name="mail">notification</param>
        /// <returns></returns>
        private EmailMessage BuildMail(IEmailNotification mail)
        {
            if (mail == null)
            {
                throw new ArgumentNullException(nameof(mail));
            }

            if (mail.To == null || !mail.To.Any())
                throw new InvalidOperationException("there are no recipients");

            var exchangeService = new ExchangeService(ExchangeVersion.Exchange2016);
            exchangeService.Credentials = new WebCredentials(userMail, userPass);
            exchangeService.TraceEnabled = true;
            exchangeService.TraceFlags = TraceFlags.All;
            exchangeService.AutodiscoverUrl(userMail, RedirectionUrlValidationCallback);
            exchangeService.Timeout = 50000;


            var message = new EmailMessage(exchangeService)
            {
                Subject = mail.Subject,
                Body = new MessageBody(mail.IsHtml ? BodyType.HTML : BodyType.Text, mail.Body),
            };
            message.ToRecipients.AddRange(
                mail.To.Select(to => new EmailAddress(to))
            );

            if (mail.Cc != null && mail.Cc.Any())
            {
                message.CcRecipients.AddRange(
                    mail.Cc.Select(cc => new EmailAddress(cc))
                );
            }

            return message;
        }

        /// <summary>
        /// Send Notification via Exchange Web Services
        /// </summary>
        /// <param name="message">notification</param>
        /// <returns></returns>
        private void SendEmailInternal(EmailMessage message)
        {
            message.Save();
            message.SendAndSaveCopy(WellKnownFolderName.SentItems);
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            return new Uri(redirectionUrl).Scheme == Uri.UriSchemeHttps;
        }

    }
}
