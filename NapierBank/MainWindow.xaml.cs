using Microsoft.Win32;
using NapierBank.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NapierBank
{
    /// <summary>
    /// Interaction logic for AddMessage.xaml
    /// </summary>
    public partial class NewMessage : Window
    {
        private Message message;
        private List<Message> msgList = new List<Message>();
        private string filePath;
        private Dictionary<string, int> trendingList = null;
        private Dictionary<string, int> mentions = null;


        public NewMessage()
        {
            InitializeComponent();
            txt_subject.IsEnabled = false;
        }

        /*
         *
         * Clears all text fields
         *
         */
        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            txt_header.Clear();
            txt_sender.Clear();
            txt_subject.Clear();
            txt_msgBox.Clear();            
        }


        /*
         *
         * Enables the subject field if email
         *
         */
        private void txt_header_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_header.Text.ToUpper().Contains("E"))
            {
                txt_subject.IsEnabled = true;
            }
            else
            {
                txt_subject.IsEnabled = false;
            }
        }


        /*
         *
         * Gets contents of text field and creates a new message
         *
         */
        private void btn_process_Click(object sender, RoutedEventArgs e)
        {

            //Console.WriteLine(txt_header.Text.Trim() + txt_sender.Text.Trim() + txt_msgBox.Text.Trim());

            try
            {
                MessageProcessor mp = new MessageProcessor();

                if (txt_header.Text.ToUpper().Contains("E"))
                {
                    message = mp.SortMessage(txt_header.Text.Trim(), txt_sender.Text.Trim(), txt_msgBox.Text.Trim(), txt_subject.Text.Trim());
                    msgList.Add(message);
                    saveFile(message.Header);
                }
                else
                {                   
                    message = mp.SortMessage(txt_header.Text.Trim(), txt_sender.Text.Trim(), txt_msgBox.Text.Trim(), null);
                    msgList.Add(message);
                    saveFile(message.Header);
                }

                MessageBox.Show("Header: " + message.Header + "\n" + "Sender: " + message.Sender + "\n" + "Message: " + message.ProcessedMessage);            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btn_getStats_Click(object sender, RoutedEventArgs e)
        {
            trendingList = new Dictionary<string, int>();
            mentions = new Dictionary<string, int>();
            txt_stats.Items.Clear();

            foreach (Message m in msgList)
            { 
                if (m is TwitterMessage)
                {
                    TwitterMessage tweet = (TwitterMessage)m;
                    sortTwitterStats(tweet);
                }                
            }

            foreach (KeyValuePair<string, int> key in trendingList)
            {
                txt_stats.Items.Add(key);
            }

            foreach (KeyValuePair<string, int> key in mentions)
            {
                txt_stats.Items.Add(key);
            }
        }


        /*
         *
         * gets twitter stats and displays them in list
         *
         */
        private void sortTwitterStats(TwitterMessage tweet)
        {
                foreach (KeyValuePair<string, int> keyValue in tweet.TrendingList)
                {
                    foreach (KeyValuePair<string, int> item in txt_stats.Items)
                    {
                        var selectedItem = (dynamic)item;
                        trendingList.Add(selectedItem.Key, Convert.ToInt32(selectedItem.Value));
                    }

                    if (trendingList.ContainsKey(keyValue.Key))
                    {
                        trendingList[keyValue.Key] += keyValue.Value;
                    }
                    else
                    {
                        trendingList.Add(keyValue.Key, keyValue.Value);
                    }
                }

                foreach (KeyValuePair<string, int> keyValue in tweet.Mentions)
                {
                    foreach (KeyValuePair<string, int> item in txt_stats.Items)
                    {
                        var selectedItem = (dynamic)item;
                        mentions.Add(selectedItem.Key, Convert.ToInt32(selectedItem.Value));
                    }

                    if (mentions.ContainsKey(keyValue.Key))
                    {
                        mentions[keyValue.Key] += keyValue.Value;
                    }
                    else
                    {
                        mentions.Add(keyValue.Key, keyValue.Value);
                    }
                }    
        }

        /*
         *
         * gets SIR stats (incident and sort code) and displays in a list
         *
         */
        private void btn_getSIR_Click(object sender, RoutedEventArgs e)
        {
            txt_stats.Items.Clear();

            foreach(Message m in msgList)
            {
                if(m is SIR_Email)
                {
                    SIR_Email email = (SIR_Email)m;
                    txt_stats.Items.Add(email.Incident);
                    txt_stats.Items.Add(email.SortCode);
                }
            }
        }

        /*
         *
         * saves file to json file
         *
         */
        private void saveFile(string fileName)
        {
            string path = @"D:/University/Year 3/TR1/Software Engineering/Napier Bank C#/NapierBank/NapierBank/resources/"; //default file path
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "JSON Files|*.json";
            saveFile.AddExtension = true;
            saveFile.DefaultExt = ".json";
            string saveName = path + fileName + ".json"; //appends path to the filename which is from header

            if (!File.Exists(saveName))
            {
                var newFile = File.Create(saveName);
                newFile.Close();
            }

            StreamWriter sW = new StreamWriter(saveName);

            foreach (Message m in msgList)
            {
                sW.WriteLine(JSONSerializer.Serializer(m));
            }

                sW.Flush();
                sW.Close();

            MessageBox.Show("Processed Message saved as: " + saveName); //on save displays filepath
        }


        /*
         * 
         * Opens file and add lines to relevant txt fields
         * 
         */
    
        private void btn_openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog browser = new OpenFileDialog();
            browser.Filter = "Message Files| *.mes";
            browser.Title = "Open Message File";
            browser.InitialDirectory = System.IO.Directory.GetCurrentDirectory();

           Nullable<bool> result = browser.ShowDialog();

            try
            {
                if (result == true)
                {
                    filePath = browser.FileName;
                }

                using (var reader = new StreamReader(filePath))
                {
                    string tempHeader = reader.ReadLine();
                    string tempSender = reader.ReadLine();

                    txt_header.Text = tempHeader;
                    txt_sender.Text = tempSender;

                    if (tempHeader.ToUpper().Contains("E"))
                    {
                        string tempSubject = reader.ReadLine();
                        txt_subject.Text = tempSubject;
                        string tempMessage = reader.ReadToEnd();
                        txt_msgBox.Text = tempMessage;
                    }
                    else
                    {
                        string tempMessage = reader.ReadToEnd();
                        txt_msgBox.Text = tempMessage;
                    }
                }

            }
            catch (FileNotFoundException error)
            {
                MessageBox.Show(error.Message, "Error");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void btn_viewMessage_Click(object sender, RoutedEventArgs e)
        {
            ViewMessages view = new ViewMessages(msgList);
            view.Show();
        }

    }
}