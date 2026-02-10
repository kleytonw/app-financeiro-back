using ERP_API.Models;
using ERP_API.Service.Parceiros.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [AllowAnonymous]
    public class WhatsAppController : ControllerBase
    {
        private readonly IWhatsAppBusinessService _whatsAppService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WhatsAppController> _logger;

        public WhatsAppController(
            IWhatsAppBusinessService whatsAppService,
            IConfiguration configuration,
            ILogger<WhatsAppController> logger)
        {
            _whatsAppService = whatsAppService;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("send-text")]
        public async Task<IActionResult> SendText([FromBody] SendTextRequest request)
        {
            try
            {
                var result = await _whatsAppService.SendTextMessageAsync(request.To, request.Message, request.PreviewUrl);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar mensagem de texto");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("send-template")]
        public async Task<IActionResult> SendTemplate([FromBody] SendTemplateRequest request)
        {
            try
            {
                var result = await _whatsAppService.SendTemplateMessageAsync(
                    request.To,
                    request.TemplateName,
                    request.Parameters);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar template");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("send-image")]
        public async Task<IActionResult> SendImage([FromBody] SendImageRequest request)
        {
            try
            {
                var result = await _whatsAppService.SendImageAsync(request.To, request.ImageUrl, request.Caption);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar imagem");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("send-buttons")]
        public async Task<IActionResult> SendButtons([FromBody] SendButtonsRequest request)
        {
            try
            {
                var buttons = request.Buttons.Select(b => (b.Id, b.Title)).ToList();
                var result = await _whatsAppService.SendInteractiveButtonsAsync(request.To, request.Text, buttons);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar botões");
                return BadRequest(new { error = ex.Message });
            }
        }

        // Webhook para receber mensagens
        [HttpGet("webhook")]
        public IActionResult VerifyWebhook([FromQuery(Name = "hub.verify_token")] string verifyToken,
                                         [FromQuery(Name = "hub.challenge")] string challenge,
                                         [FromQuery(Name = "hub.mode")] string mode)
        {
            var expectedToken = _configuration["WhatsApp:WebhookVerifyToken"];

            if (mode == "subscribe" && verifyToken == expectedToken)
            {
                _logger.LogInformation("Webhook verificado com sucesso");
                return Ok(challenge);
            }

            return Forbid();
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WhatsAppWebhook webhook)
        {
            try
            {
                // Verificar assinatura do webhook (recomendado para produção)
                if (!VerifyWebhookSignature())
                {
                    return Unauthorized();
                }

                _logger.LogInformation($"Webhook recebido: {System.Text.Json.JsonSerializer.Serialize(webhook)}");

                foreach (var entry in webhook.Entry ?? new List<WhatsAppEntry>())
                {
                    foreach (var change in entry.Changes ?? new List<WhatsAppChange>())
                    {
                        if (change.Field == "messages")
                        {
                            await ProcessIncomingMessages(change.Value);
                        }
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar webhook");
                return StatusCode(500);
            }
        }

        private bool VerifyWebhookSignature()
        {
            // Implementar verificação da assinatura do webhook
            // Documentação: https://developers.facebook.com/docs/graph-api/webhooks/getting-started#verification-requests
            return true; // Simplificado para exemplo
        }

        private async Task ProcessIncomingMessages(WhatsAppValue value)
        {
            if (value.Messages == null) return;

            foreach (var message in value.Messages)
            {
                _logger.LogInformation($"Mensagem recebida de {message.From}: {message.Text?.Body}");

                // Marcar mensagem como lida
                await _whatsAppService.MarkMessageAsReadAsync(message.Id);

                // Processar diferentes tipos de mensagem
                switch (message.Type)
                {
                    case "text":
                        await HandleTextMessage(message);
                        break;
                    case "interactive":
                        await HandleInteractiveMessage(message);
                        break;
                }
            }
        }

        private async Task HandleTextMessage(WhatsAppIncomingMessage message)
        {
            // Lógica para processar mensagens de texto
            var response = $"Olá! Recebi sua mensagem: {message.Text.Body}";
            await _whatsAppService.SendTextMessageAsync(message.From, response);
        }

        private async Task HandleInteractiveMessage(WhatsAppIncomingMessage message)
        {
            // Lógica para processar respostas interativas
            string responseText = "";

            if (message.Interactive.ButtonReply != null)
            {
                responseText = $"Você clicou no botão: {message.Interactive.ButtonReply.Title}";
            }
            else if (message.Interactive.ListReply != null)
            {
                responseText = $"Você selecionou: {message.Interactive.ListReply.Title}";
            }

            await _whatsAppService.SendTextMessageAsync(message.From, responseText);
        }
    }

    // DTOs para requests
    public class SendTextRequest
    {
        public string To { get; set; }
        public string Message { get; set; }
        public bool PreviewUrl { get; set; } = false;
    }

    public class SendTemplateRequest
    {
        public string To { get; set; }
        public string TemplateName { get; set; }
        public List<string> Parameters { get; set; }
    }

    public class SendImageRequest
    {
        public string To { get; set; }
        public string ImageUrl { get; set; }
        public string Caption { get; set; }
    }

    public class SendButtonsRequest
    {
        public string To { get; set; }
        public string Text { get; set; }
        public List<ButtonOption> Buttons { get; set; }
    }

    public class ButtonOption
    {
        public string Id { get; set; }
        public string Title { get; set; }
    }
}

