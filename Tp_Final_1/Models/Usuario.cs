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

        private readonly MyContext _context;
        

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
            misPosts = new List<Post>();
            misComentarios = new List<Comentario>();

        }

        public Usuario usuarioExistente(Usuario usuario)
        {
            MyContext _context = new MyContext();
            if (_context.usuarios != null)
            {
                var user = _context.usuarios.Where(x => x.email == usuario.email && x.password == usuario.password).SingleOrDefault();

                string[] userEmail = (from Usuario in _context.usuarios
                                      where Usuario.email == usuario.email
                                      select Usuario.email).ToArray();

                if (user != null)
                {
                    IEnumerable<Usuario> userList = _context.usuarios.Where(x => x.email == usuario.email && x.password == usuario.password);

                    userList.First().intentosFallidos = 0;
                    _context.Update(userList.First());
                    _context.SaveChanges();
                    usuario = userList.First();
                    return usuario;
                }
                else
                {
                    if (userEmail.Length > 0)
                    {
                        
                        Usuario usuarioIncorrecto = _context.usuarios.Where(x => x.email == usuario.email).First();
                        usuarioIncorrecto.intentosFallidos++;

                        if (usuarioIncorrecto.intentosFallidos >= 3)
                        {

                            usuarioIncorrecto.bloqueado = true;

                        }
                        _context.Update(usuarioIncorrecto);
                        _context.SaveChanges();
                        return usuarioIncorrecto;
                    }
                    else
                    {
                        usuario.email = "";
                        return usuario;
                    }
                }
            }
            else
            {
                usuario.email = "";
                return usuario;
            }
        }
    
        public Usuario crearUsuario(Usuario usuario)
        {
            MyContext _context = new MyContext();
            int mailExiste = _context.usuarios.Where(x => x.email == usuario.email).Count();
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
    }

}
