using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NapierBank.Model
{

    public abstract class Message
    {
        protected string header;
        protected string sender;
        protected string messageBody;
        protected string subject;
        protected string processedMessage;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="sender"></param>
        /// <param name="messageBody"></param>
        public Message(string header, string sender, string messageBody)
        {
            Header = header;
            MessageBody = messageBody;
            Sender = sender;

            //Console.WriteLine("Message.cs: " + header + sender + processedMessage + subject);
        }


        protected abstract void ProcessMessage();

        #region Getters & Setters
        public string Header
        {
            get { return header; }
            set { this.header = value; }
        }

        public string MessageBody
        {
            get { return messageBody; }
            set { this.messageBody = value; }
        }

        public string Sender
        {
            get { return sender; }
            set { this.sender = value; }
        }

        public string ProcessedMessage
        {
            get { return processedMessage; }
        }
        #endregion
    }
}
