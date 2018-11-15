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

        public Message SortMessage(string header, string sender, string messageBody, string subject)
        {

            //Console.WriteLine("Message Processor: " + header + sender + messageBody + subject);

            if (header.ToUpper().Contains("S"))
            {
                return new SMSMessage(header, sender, messageBody);
            }
            else if (header.ToUpper().Contains("E"))
            {
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
                return new TwitterMessage(header, sender, messageBody);
            }

            else
            {
                return null;
            }
        }

    }
}
