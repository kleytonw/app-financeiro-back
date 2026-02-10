using ERP_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Service.Parceiros.Interface
{
    public interface IWebhookPluggyService
    {
        Task CriarLogWebHookAsync(CriarLogWebHookPluggyModel request);

    }
}
