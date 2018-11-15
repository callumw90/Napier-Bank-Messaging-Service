using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NapierBank.Model
{
    class SIR_Email : EmailMessage
    {

        private string incident;
        private string sortCode;

        //not needed currently
        // private string srtCodeRegex = @"^[0-9]{2}-[0-9]{2}-[0-9]{2}"; 

        //regex for urls
        private string regex = @"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="sender"></param>
        /// <param name="messageBody"></param>
        /// <param name="subject"></param>
        public SIR_Email(string header, string sender, string messageBody, string subject) : base(header, sender, messageBody, subject)
        {
            ProcessMessage();
        }

        protected override void ProcessMessage()
        {
            processedMessage = messageBody;
            string[] temp = processedMessage.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            SortCode = temp[0].Trim().Substring(11);
            Incident = temp[1].Trim().Substring(20);

            // for debug
            //MessageBox.Show(sortCode);
            //MessageBox.Show(sortCode);

            //checks for sort code - not needed currently
            /*foreach (Match match in Regex.Matches(messageBody, srtCodeRegex))
            {
                sortCode = match.Value;
            }*/

            //checks message for regex and replaces with <URL Quarantined> then adding URL to list
            foreach (Match match in Regex.Matches(messageBody, regex)) //checks message for urls replaces them and adds it to a list
            {
                processedMessage = processedMessage.Replace(match.Value, "<URL Quarantined>");
                base.UrlList.Add(match.Value);
            }
        }


        #region getters & setters
        public string SortCode
        {            
            get { return sortCode; }
            set { sortCode = value; }
        }

        public string Incident
        {
            get { return incident; }
            set { incident = value; }
        }
        #endregion
    }
}
