using System;
using System.Collections.Generic;

namespace ERP_API.Models
{
    public class ListagemArquivosNexxeraRequest
    {
        public string? InitialDate { get; set; } 
        public string? FinalDate { get; set; }  
    }

    public class RedisponibilizarArquivoNexxeraRequest
    {
        public List<string> Filenames { get; set; } = new();
    }

    public class DownloadNexxeraRequest
    {
        public string Filename { get; set; } = string.Empty;
    }

    public class UploadNexxeraRequest
    {
        public string Receiver { get; set; } = string.Empty;
    }

    public class RedisponibilizarArquivoNexxeraResponse
    {
        public List<ArquivoRedisponibilizadoNexxera> Files { get; set; } = new();
        public string Message { get; set; } = string.Empty;
    }

    public class ArquivoRedisponibilizadoNexxera
    {
        public string Filename { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // exemplo: "UNREAD", "AVAILABLE"
    }


    public class ArquivoListagemNexxeraResponse
    {
        public string Filename { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public long Size { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class ArquivoNexxeraResponse
    {
        public string Filename { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public long Tamanho { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class DownloadNexxeraResponse
    {
        public string Filename { get; set; } = string.Empty;
        public string DownloadUrl { get; set; } = string.Empty;
    }

    public class UploadNexxeraResponse
    {
        public string Receiver { get; set; } = string.Empty;
        public string UploadUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }


}
