using NotificationsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationsService.Entities
{
    public class EmailNotification : IEmailNotification
    {
        public IEnumerable<string> To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsHtml { get; set; }
        public IEnumerable<string> Cc { get; set; }
        public IEnumerable<string> AttachmentIds { get; set; }
    }
}
