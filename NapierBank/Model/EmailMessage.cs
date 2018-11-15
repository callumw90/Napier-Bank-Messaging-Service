using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NapierBank.Model
{
    public class EmailMessage : Message
    {
        //protected string subject;
        private List<string> urlList = new List<string>(); //list for quarantined urls (needs to be returned and saved to txt file)


        //regex for urls
        private string regex = @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)";

        public EmailMessage(string header, string sender, string messageBody, string subject) : base(header, sender, messageBody)
        {
            Subject = subject;
            ProcessMessage();
        }
        
        protected override void ProcessMessage()
        {
            processedMessage = messageBody;

            //checks message for regex and replaces with <URL Quarantined> then adding URL to list
            foreach (Match match in Regex.Matches(messageBody, regex))
            {
                processedMessage = processedMessage.Replace(match.Value, "<URL Quarantined>");
                urlList.Add(match.Value);
            }
        }

        #region getters & setters
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public List<string> UrlList
        {
            get { return urlList; }
            set { urlList = value; }
        }
        #endregion
    }
}
