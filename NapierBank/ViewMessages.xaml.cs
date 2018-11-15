using NapierBank.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ViewMessages.xaml
    /// </summary>
    public partial class ViewMessages : Window
    {
        private List<Message> msgList = null;
        private Message tempMessage;
        private int i = 0;

        #region Constructor
        public ViewMessages(List<Message> msgList) //takes in list of Messages
        {
            this.msgList = msgList;
            i = 0;
            InitializeComponent();
            try
            {
                if (msgList.ElementAt(i) != null)
                {
                    txt_Msg.Text = "Header: " + msgList.ElementAt(i).Header + "\n" + "Sender: " + msgList.ElementAt(i).Sender + "Processed Message: " + msgList.ElementAt(i).ProcessedMessage;
                }
                else
                {
                    MessageBox.Show("No Messages", "Error");
                }
            }
            catch (ArgumentOutOfRangeException ex) //catches if msgList is empty
            {
                System.Console.Write(ex.Message);
                System.Console.Read();
                MessageBox.Show("No Messages", "Error");
            }
        }
        #endregion

        private void btn_Next_Click(object sender, RoutedEventArgs e)
        {
            i += 1;
            try
            {
                if (msgList.ElementAt(i) != null)
                {
                    tempMessage = msgList.ElementAt(i);
                    txt_Msg.Text = "Header: " + tempMessage.Header + "\n" + "Sender: " + tempMessage.Sender + "Processed Message: " + tempMessage.ProcessedMessage;
                }
                else
                {
                    MessageBox.Show("End of Messages", "Error");
                }
            }
            catch (ArgumentOutOfRangeException ex) //catches if at the end of the list
            {
                System.Console.Write(ex.Message);
                System.Console.Read();
                MessageBox.Show("No More Messages", "Error");
            }
        }

        private void btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            i -= 1;
            try
            {
                if (msgList.ElementAt(i) != null)
                {
                    tempMessage = msgList.ElementAt(i);
                    txt_Msg.Text = "Header: " + tempMessage.Header + "\n" + "Sender: " + tempMessage.Sender + "\n" + "Processed Message: " + tempMessage.ProcessedMessage;
                }
                else
                {
                    MessageBox.Show("No More Messages", "Error");
                }
            }
            catch (ArgumentOutOfRangeException ex) //catches if at the end of the list
            {
                System.Console.Write(ex.Message);
                System.Console.Read();
                MessageBox.Show("No More Messages", "Error");
            }
        }


        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
