using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Tp_Final_1.Models
{
    public class Post
    {
        public int id { get; set; }
        public int idUser { get; set; }
        public Usuario user { get; set; }
        public string contenido { get; set; }
        public List<Comentario> comentarios { get; set; } = new List<Comentario>();
        public List<Reaccion> reacciones { get; set; } = new List<Reaccion>();
        public DateTime fecha { get; set; }
        public ICollection<Tag> Tag { get; } = new List<Tag>();
        public List<PostsTags> PostsTags { get; set; }

        public Post() { }

        public Post(int id,Usuario user, string contenido)
        {
            this.id =id ;
            this.user = user;
            this.contenido = contenido;
            reacciones = new List<Reaccion>();
            comentarios = new List<Comentario>();
            //tags = new List<Tag>();
            this.fecha = DateTime.Now;
        }

        public Post(int id, Usuario user, string contenido, List<Tag> tag)
        {
            this.id = id;
            this.user = user;
            this.contenido = contenido;
            reacciones = new List<Reaccion>();
            comentarios = new List<Comentario>();
            List<Tag> tags = new List<Tag>();
            this.fecha = DateTime.Now;
        }


        public Post(Usuario user, string contenido)
        {
            this.id = id;
            this.user = user;
            this.contenido = contenido;
            reacciones = new List<Reaccion>();
            comentarios = new List<Comentario>();
            //tags = new List<Tag>();
            this.fecha = DateTime.Now;
        }

        public Post postear(Post post, int id)
        {
            
            DateTime fecha = DateTime.Now;
            post.fecha = fecha;
            post.idUser = id;
            return post;
        }

        public void agregarTags(int p, string t)
        {
            List<string> palabras = t.Split(' ').ToList();
            foreach (string palabra in palabras)
            {
                Tag tag = new Tag();
                tag.agregarTag(palabra, p);
            }

        }
    }
}
