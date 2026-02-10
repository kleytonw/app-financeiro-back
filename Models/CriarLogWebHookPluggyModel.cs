using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_API.Models
{
    public class CriarLogWebHookPluggyModel
    {
        public object ObjJson { get; set; }

    }

    public class PluggyItemUpdatedEventDto
    {
        public string? ItemId { get; set; }
        public string? Event { get; set; }
        public string? Id { get; set; }
        public string? EventId { get; set; }
        public string? ClientUserId { get; set; }
        public string? TriggeredBy { get; set; }
    }
}