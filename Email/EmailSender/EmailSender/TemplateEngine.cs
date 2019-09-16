using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSender.Modal;
using System.Text.RegularExpressions;

namespace EmailSender
{
    public class TemplateEngine<T> where T : EmailModalBase
    {
        private T _templateModal;
        private string _template;
        private const string _mustachPattern = "{{([a-zA-Z0-9])*}}";

        public TemplateEngine(T modal, string template)
        {
            _template = template;
            _templateModal = modal;
        }

        public virtual string GetTemplate()
        {
            Regex rgx = new Regex(_mustachPattern);

            foreach (Match match in rgx.Matches(_template))
            {
                string field = match.Value;
                string prop = field.Substring(2, field.Length - 4);
                _template = _template.Replace(field, GetPropValue(prop));
            }

            return _template;
        }

        private string GetPropValue(string prop)
        {
            return _templateModal.GetType().GetProperty(prop)?.GetValue(_templateModal)?.ToString() ?? string.Empty;
        }
    }
}
