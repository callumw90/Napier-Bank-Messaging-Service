using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NapierBank.Model
{
    //process SMS message
    class SMSMessage : Message
    {

        private string filePath = @"D:/University/Year 3/TR1/Software Engineering/Napier Bank C#/NapierBank/NapierBank/resources/textwords.csv";

        public SMSMessage(string header, string sender, string messageBody) : base(header, sender, messageBody)
        {     
            ProcessMessage();
        }


        //Checks message for txt speak and replaces with regular expression. writes to processed message
        protected override void ProcessMessage()
        {
            StreamReader sr = new StreamReader(File.OpenRead(filePath));

            processedMessage = messageBody;

            while (!sr.EndOfStream)
            {
                string[] txtSpeak = sr.ReadLine().Split(',');
                string replace = @"\b" + txtSpeak[0] + @"\b";

                processedMessage = Regex.Replace(processedMessage, replace, delegate(Match m){
                    return m.Value + " <" + txtSpeak[1] + ">";
                }, RegexOptions.IgnoreCase);
            }
        }
    }
}
