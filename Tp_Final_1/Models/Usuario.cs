using Tp_Final_1.Data;




namespace Tp_Final_1.Models
{
    public class Usuario
    {
        public int id { get; set; }
        public int dni { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int intentosFallidos { get; set; }
        public bool bloqueado { get; set; }
        public bool isAdm { get; set; }
        public virtual ICollection<UsuarioAmigo> misAmigos { get; set; }
        public virtual ICollection<UsuarioAmigo> amigosMios { get; set; }
        public List<Post> misPosts { get; } = new List<Post>();
        public List<Comentario> misComentarios { get; set; }
        public List<Reaccion> misReacciones { get; set; }        
        

        public Usuario() { }

        public Usuario(string nombre, string apellido, string mail, int dni, string pass, bool isAdm)
        {
            this.nombre = nombre;
            this.password = pass;
            this.apellido = apellido;
            this.email = mail;
            this.dni = dni;            
            this.intentosFallidos = 0;
            this.isAdm = isAdm;
            this.bloqueado = false;
        }

        public Usuario usuarioExistente(Usuario usuario, Usuario userPassCorrecto, Usuario[] userCorrecto)
        {

            if (userPassCorrecto != null)
            {
                userPassCorrecto.intentosFallidos = 0;  
                                                        
                return userPassCorrecto;
            }
            else
            {
                if (userCorrecto.Length > 0)
                {
                    userCorrecto.First().intentosFallidos++;

                    if (userCorrecto.First().intentosFallidos >= 3)
                    {
                        userCorrecto.First().bloqueado = true;
                    }
                    return userCorrecto.First();
                }
                else
                {
                    usuario.email = "";
                    return usuario;
                }
            }            
        }
    
        public Usuario crearUsuario(Usuario usuario,int mailExiste)
        {
            MyContext _context = new MyContext();
            
            if (mailExiste > 0)
            {
                usuario.email = "";
                return usuario;
            }
            else
            {
                return usuario;
            }
        }
    
        public Usuario editarUsuario(Usuario usuario,string passwordNueva, Usuario[] userList)
        {
            if (userList.Length > 0 )
            {
                _ = usuario.isAdm == true ? usuario.isAdm = true : usuario.isAdm = false;
                _ = usuario.intentosFallidos == 0 ? usuario.intentosFallidos = 0 : usuario.intentosFallidos = 1;
                _ = usuario.bloqueado == true ? usuario.bloqueado = true : usuario.bloqueado = false;
                if(usuario.password == passwordNueva)
                {
                    return usuario;
                }
                else
                {
                    usuario.password = "@*@";
                    return usuario;
                }
            }
            else
            {
                usuario.password = "";
                return usuario;
            }
        }
    }
}
