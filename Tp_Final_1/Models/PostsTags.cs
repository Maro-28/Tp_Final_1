using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tp_Final_1.Data;


namespace Tp_Final_1.Models
{
    public class PostsTags
    {
        public int idPost { get; set; }
        public Post Post { get; set; }
        public int idTag { get; set; }
        public Tag Tag { get; set; }
        public PostsTags() { }

        public void relPostTag(int p, int t)
        {
            MyContext _context = new MyContext();
            PostsTags pt = new PostsTags();
            pt.idPost = p;
            pt.idTag = t;
            _context.PostsTags.Add(pt);
            _context.SaveChanges();
        }

    }
}
