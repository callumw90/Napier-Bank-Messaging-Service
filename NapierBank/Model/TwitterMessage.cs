using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NapierBank.Model
{
    internal class TwitterMessage : Message
    {
        //Dictionaries to store mentions and hashtags counting instances of each one
        private Dictionary<string, int> trendingList = new Dictionary<string, int>();
        private Dictionary<string, int> mentions = new Dictionary<string, int>();

        //regex string used to find hashtags and mentions
        private string hashtagSort = @"#[A-z0-9-_]+";
        private string mentionSort = @"@[A-z0-9_]{1,15}";

        //filePath hard coded now needs to be more dynamic
        private string filePath = @"D:/University/Year 3/TR1/Software Engineering/Napier Bank C#/NapierBank/NapierBank/resources/textwords.csv";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="sender"></param>
        /// <param name="messageBody"></param>
        /// 
        public TwitterMessage(string header, string sender, string messageBody) : base(header, sender, messageBody)
        {           
            ProcessMessage();
            //Console.WriteLine("Twitter Message: " + header + sender + processedMessage + subject);
        }

        protected override void ProcessMessage()
        {
            StreamReader sr = new StreamReader(File.OpenRead(filePath));

            processedMessage = messageBody;

            //checks for text speak and appends the evaluated string
            while (!sr.EndOfStream)
            {
                string[] txtSpeak = sr.ReadLine().Split(',');
                string replace = @"\b" + txtSpeak[0] + @"\b";

                processedMessage = Regex.Replace(processedMessage, replace, delegate (Match m)
                {
                    return m.Value + " <" + txtSpeak[1] + ">";
                }, RegexOptions.IgnoreCase);
            }

            //checks message for strings that begin with # saving to dictionary
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

            //checks message for strings that begin with @ & are within size restriction and saves to dictionary
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

        #region getters & setters
        public Dictionary<string, int> TrendingList
            {
                 get { return trendingList; }
            }

            public Dictionary<string, int> Mentions
            {
                 get { return mentions; }
            }
        #endregion
    }
}