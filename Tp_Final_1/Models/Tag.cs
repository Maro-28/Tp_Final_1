using System.Collections.Generic;
using Tp_Final_1.Data;

namespace Tp_Final_1.Models
{
    public class Tag
    {
        public int id { get; set; }
        public string palabra { get; set; }
        public ICollection<Post> Post { get; } = new List<Post>();
        public List<PostsTags> PostsTags { get; set; }

        public Tag() { }
        public Tag(string palabra)
        {
            this.id = id;
            this.palabra = palabra;
        }
        public Tag(int id,string palabra, List<Post> posts)
        {
            this.id = id;
            this.palabra = palabra;
        }

        public void agregarTag(string t, int idP)
        {
            MyContext _context = new MyContext();
            PostsTags pt = new PostsTags();

            Tag[] tagExist= _context.tags.Where(x => x.palabra == t).ToArray();
            if (tagExist.Length > 0)
            {
                pt.relPostTag(idP, tagExist[0].id);
            }
            else
            {
                Tag tag = new Tag(t);
                _context.tags.Add(tag);
                _context.SaveChanges();
                pt.relPostTag(idP, tag.id);
            }
        }
    }
}
