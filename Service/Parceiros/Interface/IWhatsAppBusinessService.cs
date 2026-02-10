using ERP_API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface IWhatsAppBusinessService
    {
        Task<WhatsAppApiResponse> SendTextMessageAsync(string to, string message, bool previewUrl = false);
        Task<WhatsAppApiResponse> SendTemplateMessageAsync(string to, string templateName, List<string> parameters = null);
        Task<WhatsAppApiResponse> SendImageAsync(string to, string imageUrl, string caption = null);
        Task<WhatsAppApiResponse> SendDocumentAsync(string to, string documentUrl, string filename, string caption = null);
        Task<WhatsAppApiResponse> SendInteractiveButtonsAsync(string to, string bodyText, List<(string id, string title)> buttons);
        Task<WhatsAppApiResponse> SendInteractiveListAsync(string to, string bodyText, List<(string title, List<(string id, string title, string description)> rows)> sections);
        Task<bool> MarkMessageAsReadAsync(string messageId);
    }
}
