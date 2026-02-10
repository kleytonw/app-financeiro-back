using System;

namespace ERP.Models
{
    public class BlogResponse
    {
        public int IdBlog { get; set; }         
        public int? IdEmpresa { get; set; }        
        public string Autor { get; set; }          
        public string Titulo { get; set; }         
        public string Subtitulo { get; set; }      
        public string Descricao { get; set; }      
        public DateTime? Data { get; set; }        
        public string LinkFoto { get; set; }       
        public string Situacao { get; set; }
    }

    public class BlogRequest
    {
        public int IdBlog { get; set; }
        public int? IdEmpresa { get; set; }
        public string Autor { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? Data { get; set; }
        public string LinkFoto { get; set; }
        public string Situacao { get; set; }
    }
}