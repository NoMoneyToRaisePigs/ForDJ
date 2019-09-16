using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.Modal
{
    public class EmailModalBase
    {
        public string UserName { get; set; }
        public string FooterTitle { get; set; }
        public string FooterEmail { get; set; }
        public string LogoImg { get; set; }
        public string HeartImg { get; set; }
    }
}
