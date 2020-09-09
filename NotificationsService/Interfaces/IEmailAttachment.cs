using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationsService.Interfaces
{
    /// <summary>
    /// Attachment interface
    /// </summary>
    public interface IEmailAttachment
    {
        string FileName { get; set; }

        byte[] ContentBytes { get; set; }

        string ContentType { get; set; }
    }
}
