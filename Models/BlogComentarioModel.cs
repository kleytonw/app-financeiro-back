namespace ERP.Models
{
    public class BlogComentarioResponse
    {
        public int IdBlogComentario { get; set; }     
        public int? IdBlog { get; set; }               
        public string Nome { get; set; } 
        public string Titulo { get; set; }
        public string Email { get; set; }             
        public string Comentario { get; set; }        
        public string Situacao { get; set; }         
    }

    public class BlogComentarioRequest
    {
        public int IdBlogComentario { get; set; }     
        public int? IdBlog { get; set; }               
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public string Email { get; set; }             
        public string Comentario { get; set; }        
        public string Situacao { get; set; }         
    }
}
