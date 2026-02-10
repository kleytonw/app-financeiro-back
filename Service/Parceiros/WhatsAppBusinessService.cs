using ERP_API.Models;
using ERP_API.Service.Parceiros.Interface;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ERP_API.Service.Parceiros
{
    public class WhatsAppBusinessService : IWhatsAppBusinessService
    {
        private readonly HttpClient _httpClient;
        private readonly string _accessToken;
        private readonly string _phoneNumberId;
        private readonly string _baseUrl;

        public WhatsAppBusinessService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _accessToken = configuration["WhatsApp:AccessToken"];
            _phoneNumberId = configuration["WhatsApp:PhoneNumberId"];
            _baseUrl = $"https://graph.facebook.com/v22.0/{_phoneNumberId}";

            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);
        }

        public async Task<WhatsAppApiResponse> SendTextMessageAsync(string to, string message, bool previewUrl = false)
        {
            var whatsAppMessage = new WhatsAppMessage
            {
                To = to,
                Type = "text",
                Text = new WhatsAppText
                {
                    Body = message,
                    PreviewUrl = previewUrl
                }
            };

            return await SendMessageAsync(whatsAppMessage);
        }

        public async Task<WhatsAppApiResponse> SendTemplateMessageAsync(string to, string templateName, List<string> parameters = null)
        {
            var components = new List<WhatsAppComponent>();

            if (parameters?.Any() == true)
            {
                components.Add(new WhatsAppComponent
                {
                    Type = "body",
                    Parameters = parameters.Select(p => new WhatsAppParameter
                    {
                        Type = "text",
                        Text = p
                    }).ToList()
                });
            }

            var whatsAppMessage = new WhatsAppMessage
            {
                To = to,
                Type = "template",
                Template = new WhatsAppTemplate
                {
                    Name = templateName,
                    Language = new WhatsAppLanguage { Code = "pt_BR" },
                    Components = components
                }
            };

            return await SendMessageAsync(whatsAppMessage);
        }

        public async Task<WhatsAppApiResponse> SendImageAsync(string to, string imageUrl, string caption = null)
        {
            var whatsAppMessage = new WhatsAppMessage
            {
                To = to,
                Type = "image",
                Image = new WhatsAppMedia
                {
                    Link = imageUrl,
                    Caption = caption
                }
            };

            return await SendMessageAsync(whatsAppMessage);
        }

        public async Task<WhatsAppApiResponse> SendDocumentAsync(string to, string documentUrl, string filename, string caption = null)
        {
            var whatsAppMessage = new WhatsAppMessage
            {
                To = to,
                Type = "document",
                Document = new WhatsAppDocument
                {
                    Link = documentUrl,
                    Filename = filename,
                    Caption = caption
                }
            };

            return await SendMessageAsync(whatsAppMessage);
        }

        public async Task<WhatsAppApiResponse> SendInteractiveButtonsAsync(string to, string bodyText, List<(string id, string title)> buttons)
        {
            var whatsAppMessage = new WhatsAppMessage
            {
                To = to,
                Type = "interactive",
                Interactive = new WhatsAppInteractive
                {
                    Type = "button",
                    Body = new WhatsAppInteractiveBody { Text = bodyText },
                    Action = new WhatsAppAction
                    {
                        Buttons = buttons.Select(b => new WhatsAppButton
                        {
                            Reply = new WhatsAppReply
                            {
                                Id = b.id,
                                Title = b.title
                            }
                        }).ToList()
                    }
                }
            };

            return await SendMessageAsync(whatsAppMessage);
        }

        public async Task<WhatsAppApiResponse> SendInteractiveListAsync(string to, string bodyText, List<(string title, List<(string id, string title, string description)> rows)> sections)
        {
            var whatsAppMessage = new WhatsAppMessage
            {
                To = to,
                Type = "interactive",
                Interactive = new WhatsAppInteractive
                {
                    Type = "list",
                    Body = new WhatsAppInteractiveBody { Text = bodyText },
                    Action = new WhatsAppAction
                    {
                        Sections = sections.Select(s => new WhatsAppSection
                        {
                            Title = s.title,
                            Rows = s.rows.Select(r => new WhatsAppRow
                            {
                                Id = r.id,
                                Title = r.title,
                                Description = r.description
                            }).ToList()
                        }).ToList()
                    }
                }
            };

            return await SendMessageAsync(whatsAppMessage);
        }

        public async Task<bool> MarkMessageAsReadAsync(string messageId)
        {
            var payload = new
            {
                messaging_product = "whatsapp",
                status = "read",
                message_id = messageId
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/messages", content);
            return response.IsSuccessStatusCode;
        }

        private async Task<WhatsAppApiResponse> SendMessageAsync(WhatsAppMessage message)
        {
            try
            {
                var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_baseUrl}/messages", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<WhatsAppApiResponse>(responseContent);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"WhatsApp API Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao enviar mensagem WhatsApp: {ex.Message}", ex);
            }
        }
    }
}

