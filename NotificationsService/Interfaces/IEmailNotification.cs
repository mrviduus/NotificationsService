using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationsService.Interfaces
{
    /// <summary>
    /// Email interface
    /// </summary>
    public interface IEmailNotification
    {
        IEnumerable<string> To { get; set; }
        IEnumerable<string> Cc { get; set; }
        string Body { get; set; }
        string Subject { get; set; }
        bool IsHtml { get; set; }
    }
}
