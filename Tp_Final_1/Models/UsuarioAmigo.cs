using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tp_Final_1.Models
{
    public class UsuarioAmigo
    {
        public int idUser { get; set; }
        public Usuario user { get; set; }
        public int idAmigo { get; set; }
        public Usuario amigo { get; set; }

        public UsuarioAmigo() { }

        public UsuarioAmigo(Usuario ppal, Usuario segundo)
        {
            user = ppal;
            amigo = segundo;
        }

        public UsuarioAmigo(int ppal, int segundo)
        {
            idUser = ppal;
            idAmigo = segundo;
        }

        public UsuarioAmigo agregarAmigoUsuario(UsuarioAmigo segundo)
        {
            int idU = segundo.idUser;
            int idAmigo = segundo.idAmigo;

            segundo.idAmigo = idU;
            segundo.idUser = idAmigo;

            return segundo;
        }
        public UsuarioAmigo EliminarAmigo(UsuarioAmigo segundo)
        {
            int idU = segundo.idUser;
            int idAmigo = segundo.idAmigo;

            segundo.idAmigo = idU;
            segundo.idUser = idAmigo;

            return segundo;
        }
        
        }
}
