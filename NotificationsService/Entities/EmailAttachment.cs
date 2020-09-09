using NotificationsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationsService.Entities
{
    public class EmailAttachment : IEmailAttachment
    {
        public string FileName { get; set; }

        public byte[] ContentBytes { get; set; }

        public string ContentType { get; set; }
    }
}
