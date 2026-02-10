using ERP.Infra;
using ERP_API.Domain.Entidades;
using ERP_API.Models;
using ERP_API.Service.Parceiros.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros
{
    public class WebhookPluggyService : IWebhookPluggyService
    {
        private readonly Context _context;
        public WebhookPluggyService(Context context)
        {
            _context = context;
        }

        public async Task CriarLogWebHookAsync(CriarLogWebHookPluggyModel request)
        {
          
                var retorno = new LogWebhookPluggyResponseModel();

                var logWebHook = new WebhookPluggy(DateTime.Now, JsonSerializer.Serialize(request.ObjJson));

                _context.Add(logWebHook);
                await _context.SaveChangesAsync();


                return;
            
        }

    }
}