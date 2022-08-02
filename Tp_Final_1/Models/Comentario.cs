using System;
using System.Collections.Generic;
using System.Text;
using Tp_Final_1.Data;


namespace Tp_Final_1.Models
{
    public class Comentario
    {
        public int id { get; set; }
        public int idPost { get; set; }
        public Post post { get; set; }
        public int idUser { get; set; }
        public Usuario usuario { get; set; }
        public string contenido { get; set; }
        public DateTime fecha { get; set; }        

        public Comentario() { }

        public Comentario(Post post, Usuario usuario, string contenido)
        {
            this.id = id;
            this.post = post;
            this.usuario = usuario;
            this.contenido = contenido;
            this.fecha = DateTime.Now;
        }

        public Comentario(int id, Post post, Usuario usuario, string contenido)
        {
            this.id = id;
            this.post = post;
            this.usuario = usuario;
            this.contenido = contenido;
            this.fecha = DateTime.Now;
        }


        public Comentario agregarComentario (int idP, Comentario c)
        {
            MyContext _context = new MyContext();
            int post = _context.post.Where(x => x.id == idP).Count();
            if(post > 0)
            {    
               c.idPost= idP;
            }
            c.fecha = DateTime.Now;
            return c;
        }
    }
}
