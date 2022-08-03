using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Tp_Final_1.Models;

namespace Tp_Final_1.Data
{
    public class MyContext : DbContext
    {
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<Post> post { get; set; }
        public DbSet<Comentario> comentarios { get; set; }        
        public DbSet<Reaccion> reacciones { get; set; }
        public DbSet<Tag> tags { get; set; }

        public MyContext() { }
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=DESKTOP-I98M29K\SQLEXPRESS;initial catalog=tp_final_1;trusted_connection=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nombre de la tabla
            modelBuilder.Entity<Usuario>().ToTable("Usuarios").HasKey(u => u.id);
            modelBuilder.Entity<Post>().ToTable("Post").HasKey(p => p.id);
            modelBuilder.Entity<Comentario>().ToTable("Comentario").HasKey(c => c.id);
            modelBuilder.Entity<Reaccion>().ToTable("Reaccion").HasKey(r => r.id);
            modelBuilder.Entity<Tag>().ToTable("Tag").HasKey(k => k.id);
            modelBuilder.Entity<UsuarioAmigo>().ToTable("Usuario_Amigo").HasKey(k => new { k.idAmigo, k.idUser });
            modelBuilder.Entity<PostsTags>().ToTable("Posts_Tags").HasKey(k => new { k.idPost, k.idTag });

            //relaciones
            modelBuilder.Entity<Post>()
                .HasOne(P => P.user)
                .WithMany(U => U.misPosts)
                .HasForeignKey(P => P.idUser)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Comentario>()
                .HasOne(C => C.usuario)
                .WithMany(U => U.misComentarios)
                .HasForeignKey(C => C.idUser)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Comentario>()
                .HasOne(C => C.post)
                .WithMany(P => P.comentarios)
                .HasForeignKey(C => C.idPost)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaccion>()
                .HasOne(R => R.usuario)
                .WithMany(U => U.misReacciones)
                .HasForeignKey(R => R.idUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reaccion>()
                .HasOne(R => R.post)
                .WithMany(P => P.reacciones)
                .HasForeignKey(R => R.idPost)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UsuarioAmigo>()
               .HasOne(UA => UA.user)
               .WithMany(U => U.misAmigos)
               .HasForeignKey(u => u.idAmigo)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UsuarioAmigo>()
                .HasOne(UA => UA.amigo)
                .WithMany(U => U.amigosMios)
                .HasForeignKey(u => u.idUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>()
                .HasMany(T => T.Tag)
                .WithMany(P => P.Post)
                .UsingEntity<PostsTags>(
                    etp => etp.HasOne(tp => tp.Tag).WithMany(p => p.PostsTags).HasForeignKey(t => t.idTag),
                    etp => etp.HasOne(tp => tp.Post).WithMany(p => p.PostsTags).HasForeignKey(t => t.idPost),
                    etp => etp.HasKey(k => new { k.idPost, k.idTag })
                );

            modelBuilder.Entity<Usuario>(
                usr => 
                {
                    usr.Property(u => u.dni).HasColumnType("int");
                    usr.Property(u => u.dni).IsRequired(true);
                    usr.Property(u => u.nombre).HasColumnType("varchar(200)");
                    usr.Property(u => u.apellido).HasColumnType("varchar(200)");
                    usr.Property(u => u.email).HasColumnType("varchar(200)");
                    usr.Property(u => u.password).HasColumnType("varchar(200)");
                    usr.Property(u => u.intentosFallidos).HasColumnType("int");
                    usr.Property(u => u.bloqueado).HasColumnType("bit");
                    usr.Property(u => u.isAdm).HasColumnType("bit");
                });

            //Datos de prueba
            modelBuilder.Entity<Usuario>().HasData(
                new {id = 1, dni = 111, nombre = "Mariano", apellido = "Rojas", email = "Mariano@mail.com", password = "111", intentosFallidos = 0, bloqueado = false , isAdm = true});
            modelBuilder.Entity<Usuario>().HasData(
                new { id = 2, dni = 222, nombre = "Alan", apellido = "Carballal", email = "Alan@mail.com", password = "222", intentosFallidos = 0, bloqueado = false, isAdm = false });
            modelBuilder.Entity<Usuario>().HasData(
                new { id = 3, dni = 333, nombre = "Manuel", apellido = "Fraga", email = "Manuel@mail.com", password = "333", intentosFallidos = 0, bloqueado = false, isAdm = false });
            modelBuilder.Entity<Usuario>().HasData(
                new { id = 4, dni = 444, nombre = "Paula", apellido = "Lezcano", email = "Paula@mail.com", password = "444", intentosFallidos = 0, bloqueado = true, isAdm = false });
            modelBuilder.Entity<Usuario>().HasData(
                new { id = 5, dni = 555, nombre = "Mariano", apellido = "Ghislanzoni", email = "Marianog@mail.com", password = "555", intentosFallidos = 2, bloqueado = false, isAdm = false });
            
            modelBuilder.Entity<Post>().HasData(
                new { id = 1, idUser = 1, contenido = "Como estan?", fecha = DateTime.Now });
            modelBuilder.Entity<Post>().HasData(
                new { id = 2, idUser = 2, contenido = "Todo bien por suerte", fecha = DateTime.Now });
            modelBuilder.Entity<Post>().HasData(
                new { id = 3, idUser = 3, contenido = "Hola", fecha = DateTime.Now });
            modelBuilder.Entity<Post>().HasData(
                new { id = 4, idUser = 4, contenido = "De donde son?", fecha = DateTime.Now });
            modelBuilder.Entity<Post>().HasData(
                new { id = 5, idUser = 5, contenido = "Hola", fecha = DateTime.Now });


            modelBuilder.Entity<Comentario>().HasData(
                new { id = 1, idPost = 1, idUser= 2,  contenido = "Hola todo bien y vos?", fecha = DateTime.Now });
            modelBuilder.Entity<Comentario>().HasData(
                new { id = 2, idPost = 3, idUser = 2, contenido = "Hola como estas?", fecha = DateTime.Now });
            modelBuilder.Entity<Comentario>().HasData(
                new { id = 3, idPost = 3, idUser = 3, contenido = "Hola!!!", fecha = DateTime.Now });
            modelBuilder.Entity<Comentario>().HasData(
                new { id = 4, idPost = 5, idUser = 1, contenido = "Buenas!", fecha = DateTime.Now });
            modelBuilder.Entity<Comentario>().HasData(
                new { id = 5, idPost = 4, idUser = 3, contenido = "Argentina", fecha = DateTime.Now });

            modelBuilder.Entity<Tag>().HasData(
                new { id = 1, palabra = "Bienvenida" });
            modelBuilder.Entity<Tag>().HasData(
                new { id = 2, palabra = "love" }); 
            modelBuilder.Entity<Tag>().HasData(
                new { id = 3, palabra = "instagood" }); 
            modelBuilder.Entity<Tag>().HasData(
                new { id = 4, palabra = "fashion" }); 
            modelBuilder.Entity<Tag>().HasData(
                new { id = 5, palabra = "photooftheday" }); 
            modelBuilder.Entity<Tag>().HasData(
                new { id = 6, palabra = "art" }); 
            modelBuilder.Entity<Tag>().HasData(
                new { id = 7, palabra = "photography" }); 
            modelBuilder.Entity<Tag>().HasData(
                new { id = 8, palabra = "instagram" }); 
            modelBuilder.Entity<Tag>().HasData(
                new { id = 9, palabra = "beautiful" }); 
            modelBuilder.Entity<Tag>().HasData(
                new { id = 10, palabra = "nature" });

            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 1, idPost = 1, idTag = 5 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 2, idPost = 1, idTag = 8 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 3, idPost = 1, idTag = 9 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 4, idPost = 1, idTag = 3 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 5, idPost = 2, idTag = 10 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 6, idPost = 2, idTag = 5 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 7, idPost = 2, idTag = 9 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 8, idPost = 2, idTag = 3 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 9, idPost = 3, idTag = 2 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 10, idPost = 3, idTag = 5 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 11, idPost = 3, idTag = 6 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 12, idPost = 3, idTag = 8 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 13, idPost = 4, idTag = 8 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 14, idPost = 4, idTag = 5 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 15, idPost = 4, idTag = 2 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 16, idPost = 4, idTag = 1 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 17, idPost = 5, idTag = 10 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 18, idPost = 5, idTag = 2 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 19, idPost = 5, idTag = 5 }); 
            modelBuilder.Entity<PostsTags>().HasData(
                new { id = 20, idPost = 5, idTag = 6 });

            modelBuilder.Entity<UsuarioAmigo>().HasData(
                new { id = 1, idUser = 2, idAmigo = 3 });
            modelBuilder.Entity<UsuarioAmigo>().HasData(
                new { id = 2, idUser = 3, idAmigo = 2 });

            modelBuilder.Ignore<RedSocial>();
        }

        public DbSet<Tp_Final_1.Models.UsuarioAmigo>? UsuarioAmigo { get; set; }

        public DbSet<Tp_Final_1.Models.PostsTags>? PostsTags { get; set; }
    }
}
