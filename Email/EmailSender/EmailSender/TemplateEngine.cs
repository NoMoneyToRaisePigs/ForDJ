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
        private const string _braketPattern = @"\[\[([^[])*\]\]";
        private const string _dollarPatter = @"\$\$([^$])*\$\$";

        public TemplateEngine(T modal, string template)
        {
            _template = template;
            _templateModal = modal;
        }

        public virtual string GetTemplate()
        {

            _template = FlattenTemplate(_template);
            _template = ResolveTemplateValues(_templateModal, _template, _mustachPattern);
            _template = ResolveTemplateLoopings(_templateModal, _template);

            //foreach (Match match in rgx.Matches(_template))
            //{
            //    string field = match.Value;
            //    string expression = field.Substring(2, field.Length - 4);

            //    string loopExpression = expression.Split('@')[0];
            //    string loopContent = expression.Split('@')[1];
            //    //string loopItem = loopExpression.Split(new string[] { "of" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
            //    //object loopItemValue = GetPropValue(_templateModal, loopItem);               
            //    //string prop = loopExpression.Split(new string[] { "of" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim();
            //    string x = GetFinalLoopingContent(loopExpression, loopContent);

            //}

            return _template;
        }

        //private string GetFinalLoopingContent(string loopExpression, string loopContent)
        //{
        //    string[] expressions = loopExpression.Split(new string[] { "of" }, StringSplitOptions.RemoveEmptyEntries);
        //    if (expressions.Length != 2) { throw new Exception($"invalid expression: {loopExpression}"); }
        //    string loopItem = expressions[0].Trim();        
        //    string prop = expressions[1].Trim();
        //    Array array = (Array)GetPropObject(prop);

        //    //IEnumerable<T> enumerable = (IEnumerable<T>)array;
        //    //var x = array.GetValue(0);
        //    StringBuilder builder = new StringBuilder(string.Empty);

        //    foreach (var item in array)
        //    {
        //        string content = Resolve(loopContent, item, _dollarPatter, loopItem);
        //        builder.Append(content);
        //    }

        //    return builder.ToString();
        //}



        //private string Resolve(string template, object modal, string pattern)
        //{
        //    Regex rgx = new Regex(pattern);

        //    foreach (Match match in rgx.Matches(template))
        //    {
        //        string field = match.Value;
        //        template = template.Replace(field, GetPropValue(modal,field));
        //    }


        //    return template;
        //}

        private string FlattenTemplate(string template)
        {
            template = template.Replace("\n", "");
            template = template.Replace("\r", "");

            return template;
        }

        private string ResolveTemplateLoopings(object modal, string template)
        {
            Regex rgx = new Regex(_braketPattern);

            foreach (Match match in rgx.Matches(template))
            {
                string field = match.Value;
                string resolved = ResolveLoopingExpression(modal, field);
                template = template.Replace(field, resolved);
            }

            return template;
        }

        private string ResolveTemplateValues(object modal, string template, string pattern, string alias = null)
        {
            Regex rgx = new Regex(pattern);

            foreach (Match match in rgx.Matches(template))
            {
                string field = match.Value;
                string resolved = ResolveValueExpression(modal, field, alias)?.ToString();
                template = template.Replace(field, resolved);
            }


            return template;
        }
       
        private object GetPropertyValue(object modal, string prop)
        {
            if (modal == null) throw new Exception($"Can't Resolve : {prop} with modal value null");

            return modal.GetType().GetProperty(prop)?.GetValue(modal);
        }

        private object ResolveValueExpression(object modal, string expression, string alias = null)
        {
            if (modal == null) throw new Exception($"Can't Resolve : {expression} with modal value null");

            expression = expression.Substring(2, expression.Length - 4).Trim();
            expression = expression.Replace($"{alias}.", "");
            string[] valueItems = expression.Split(new[] { '.' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (valueItems.Length == 1)
            {
                return GetPropertyValue(modal, valueItems[0]);
            }
            else if (valueItems.Length == 2)
            {
                return ResolveValueExpression(GetPropertyValue(modal, valueItems[0]), valueItems[1]);
            }
            else
            {
                return expression;
            }
        }

        private string ResolveLoopingExpression(object modal, string expression)
        {
            if (modal == null) throw new Exception($"Can't Resolve : {expression} with modal value null");

            expression = expression.Substring(2, expression.Length - 4).Trim();
            string[] expressionItems = expression.Split(new[] { '@' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if(expressionItems.Length != 2) throw new Exception($"Invaid Looping Expression: {expression}");

            string loopExpression = expressionItems[0];
            string loopContent = expressionItems[1];
            string[] loopItems = loopExpression.Split(new string[] { "of" }, 2, StringSplitOptions.RemoveEmptyEntries);


            StringBuilder builder = new StringBuilder(string.Empty);

            if (loopItems.Length == 1)
            {
                object times = GetPropertyValue(modal, loopItems[0].Trim());
                if (times.GetType().IsPrimitive && times.GetType() == typeof(int))
                {
                    int number = (int)times;

                    for (int i = 0; i < number; i++)
                    {
                        builder.Append(loopContent);
                    }
                }
                else
                {
                    throw new Exception($"Invaid Looping Expression: {expression}");
                }
            }
            else if (loopItems.Length == 2)
            {
                string loopAlias = loopItems[0].Trim();
                string loopProperty = loopItems[1].Trim();
                Array array = (Array)GetPropertyValue(modal, loopProperty);

                //ToDo: check Array, throw Exception here

                foreach (var item in array)
                {
                    string content = ResolveTemplateValues(item, loopContent, _dollarPatter, loopAlias);
                    builder.Append(content);
                }
            }
            else
            {
                throw new Exception($"Invaid Looping Expression: {expression}");
            }

            return builder.ToString();
        }
    }
}
