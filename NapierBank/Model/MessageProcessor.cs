using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NapierBank.Model
{
    class MessageProcessor
    {
        /// <summary>
        /// Sorts messages into categories and creates them, then returns the object to be added to a list
        /// </summary>
        /// <param name="header"></param>
        /// <param name="sender"></param>
        /// <param name="messageBody"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public Message SortMessage(string header, string sender, string messageBody, string subject)
        {

            //Console.WriteLine("Message Processor: " + header + sender + messageBody + subject);
            //Ensures header is 9 characters long
            if (header.Length != 9)
            {
                MessageBox.Show("Invalid Header Length");
            }
            else
            {
                header = header.Substring(0, 9);

            }

            
            if (header.ToUpper().Contains("S"))
            {
                //cuts off the message length if its over 140
                if (messageBody.Length > 140)
                {
                    messageBody = messageBody.Substring(0, 140);
                    return new SMSMessage(header, sender, messageBody);
                }
                return new SMSMessage(header, sender, messageBody);
            }

            else if (header.ToUpper().Contains("E"))
            {
                //cuts message off at 1028 characters
                if (messageBody.Length > 1028)
                {
                    messageBody = messageBody.Substring(0, 1028);
                }

                //cuts subject to 20 characters
                if (subject.Length > 20)
                {
                    subject = subject.Substring(0, 20);
                }

                //check subject if its SIR or Standard message
                if (subject.ToUpper().Contains("SIR"))
                {
                    return new SIR_Email(header, sender, messageBody, subject);
                }
                else
                {
                    return new EmailMessage(header, sender, messageBody, subject);
                }
            }

            else if (header.ToUpper().Contains("T"))
            {
                //keeps sender to 15 characters 
                if (sender.Length > 15)
                {
                    sender = sender.Substring(0,15);
                }

                //limits message to 140 chars
                if (messageBody.Length > 140)
                {
                    messageBody = messageBody.Substring(0, 140);
                }

                return new TwitterMessage(header, sender, messageBody);
            }

            else
            {
                return null;
            }
        }

    }
}
