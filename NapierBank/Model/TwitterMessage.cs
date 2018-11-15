using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NapierBank.Model
{
    internal class TwitterMessage : Message
    {
        private Dictionary<string, int> trendingList = new Dictionary<string, int>();
        private Dictionary<string, int> mentions = new Dictionary<string, int>();

        private string hashtagSort = @"#[A-z0-9-_]+";
        private string mentionSort = @"@[A-z0-9_]{1,15}";
        private string filePath = @"D:/University/Year 3/TR1/Software Engineering/Napier Bank C#/NapierBank/NapierBank/resources/textwords.csv";

        public TwitterMessage(string header, string sender, string messageBody) : base(header, sender, messageBody)
        {           
            ProcessMessage();
            //Console.WriteLine("Twitter Message: " + header + sender + processedMessage + subject);
        }

        protected override void ProcessMessage()
        {
            StreamReader sr = new StreamReader(File.OpenRead(filePath));

            processedMessage = messageBody;

            while (!sr.EndOfStream)
            {
                string[] txtSpeak = sr.ReadLine().Split(',');
                string replace = @"\b" + txtSpeak[0] + @"\b";

                processedMessage = Regex.Replace(processedMessage, replace, delegate (Match m)
                {
                    return m.Value + " <" + txtSpeak[1] + ">";
                }, RegexOptions.IgnoreCase);
            }

            foreach (Match match in Regex.Matches(messageBody, hashtagSort))
            {
                if (trendingList.ContainsKey(match.Value))
                {
                    trendingList[match.Value]++;
                }
                else
                {
                    trendingList.Add(match.Value, 1);
                }
            }

            foreach (Match match in Regex.Matches(messageBody, mentionSort))
            {
                if (mentions.ContainsKey(match.Value))
                {
                    mentions[match.Value]++;
                }
                else
                {
                    mentions.Add(match.Value, 1);
                }
            }
        }

            public Dictionary<string, int> TrendingList
            {
                 get { return trendingList; }
            }

            public Dictionary<string, int> Mentions
            {
                 get { return mentions; }
            }
    }
}