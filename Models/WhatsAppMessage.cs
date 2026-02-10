using Newtonsoft.Json;
using System.Collections.Generic;

namespace ERP_API.Models
{
    public class WhatsAppMessage
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; } = "whatsapp";

        [JsonProperty("recipient_type")]
        public string RecipientType { get; set; } = "individual";

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public WhatsAppText Text { get; set; }

        [JsonProperty("template")]
        public WhatsAppTemplate Template { get; set; }

        [JsonProperty("image")]
        public WhatsAppMedia Image { get; set; }

        [JsonProperty("document")]
        public WhatsAppDocument Document { get; set; }

        [JsonProperty("interactive")]
        public WhatsAppInteractive Interactive { get; set; }
    }

    public class WhatsAppText
    {
        [JsonProperty("preview_url")]
        public bool PreviewUrl { get; set; } = false;

        [JsonProperty("body")]
        public string Body { get; set; }
    }

    public class WhatsAppTemplate
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("language")]
        public WhatsAppLanguage Language { get; set; }

        [JsonProperty("components")]
        public List<WhatsAppComponent> Components { get; set; }
    }

    public class WhatsAppLanguage
    {
        [JsonProperty("code")]
        public string Code { get; set; } = "pt_BR";
    }

    public class WhatsAppComponent
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("parameters")]
        public List<WhatsAppParameter> Parameters { get; set; }
    }

    public class WhatsAppParameter
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("image")]
        public WhatsAppMedia Image { get; set; }

        [JsonProperty("document")]
        public WhatsAppDocument Document { get; set; }
    }

    public class WhatsAppMedia
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }
    }

    public class WhatsAppDocument
    {
        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("caption")]
        public string Caption { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }
    }

    public class WhatsAppInteractive
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("body")]
        public WhatsAppInteractiveBody Body { get; set; }

        [JsonProperty("action")]
        public WhatsAppAction Action { get; set; }
    }

    public class WhatsAppInteractiveBody
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class WhatsAppAction
    {
        [JsonProperty("buttons")]
        public List<WhatsAppButton> Buttons { get; set; }

        [JsonProperty("sections")]
        public List<WhatsAppSection> Sections { get; set; }
    }

    public class WhatsAppButton
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "reply";

        [JsonProperty("reply")]
        public WhatsAppReply Reply { get; set; }
    }

    public class WhatsAppReply
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class WhatsAppSection
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("rows")]
        public List<WhatsAppRow> Rows { get; set; }
    }

    public class WhatsAppRow
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    // Modelos para recebimento de webhooks
    public class WhatsAppWebhook
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("entry")]
        public List<WhatsAppEntry> Entry { get; set; }
    }

    public class WhatsAppEntry
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changes")]
        public List<WhatsAppChange> Changes { get; set; }
    }

    public class WhatsAppChange
    {
        [JsonProperty("value")]
        public WhatsAppValue Value { get; set; }

        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public class WhatsAppValue
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("metadata")]
        public WhatsAppMetadata Metadata { get; set; }

        [JsonProperty("contacts")]
        public List<WhatsAppContact> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<WhatsAppIncomingMessage> Messages { get; set; }

        [JsonProperty("statuses")]
        public List<WhatsAppStatus> Statuses { get; set; }
    }

    public class WhatsAppMetadata
    {
        [JsonProperty("display_phone_number")]
        public string DisplayPhoneNumber { get; set; }

        [JsonProperty("phone_number_id")]
        public string PhoneNumberId { get; set; }
    }

    public class WhatsAppContact
    {
        [JsonProperty("profile")]
        public WhatsAppProfile Profile { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class WhatsAppProfile
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class WhatsAppIncomingMessage
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("text")]
        public WhatsAppText Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("interactive")]
        public WhatsAppInteractiveResponse Interactive { get; set; }
    }

    public class WhatsAppInteractiveResponse
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("button_reply")]
        public WhatsAppButtonReply ButtonReply { get; set; }

        [JsonProperty("list_reply")]
        public WhatsAppListReply ListReply { get; set; }
    }

    public class WhatsAppButtonReply
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class WhatsAppListReply
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class WhatsAppStatus
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipientId { get; set; }
    }

    // Response models
    public class WhatsAppApiResponse
    {
        [JsonProperty("messaging_product")]
        public string MessagingProduct { get; set; }

        [JsonProperty("contacts")]
        public List<WhatsAppContactResponse> Contacts { get; set; }

        [JsonProperty("messages")]
        public List<WhatsAppMessageResponse> Messages { get; set; }
    }

    public class WhatsAppContactResponse
    {
        [JsonProperty("input")]
        public string Input { get; set; }

        [JsonProperty("wa_id")]
        public string WaId { get; set; }
    }

    public class WhatsAppMessageResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
