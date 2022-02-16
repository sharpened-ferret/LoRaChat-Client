using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        ClientWebSocket client = new ClientWebSocket();
        List<Message> messages = new List<Message>();
        CancellationTokenSource cts = new CancellationTokenSource();
        Uri uri = new Uri("ws://localhost:8765");
        Task recieve;

        public Form1()
        {
            InitializeComponent();
        }

        private async Task ConnectToServerAsync(Uri uri, CancellationToken token)
        {
            var keepAlive = new TimeSpan(0, 30, 0);
            client.Options.KeepAliveInterval = keepAlive;
            await client.ConnectAsync(uri, token);
        }

        private async Task SendMessageAsync(string message, CancellationToken token)
        {
            Message msg = new Message
            {
                username = Properties.Settings.Default["username"].ToString(),
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                message = textBox1.Text
            };
            string serialisedMsg = JsonConvert.SerializeObject(msg);

            var byteMsg = Encoding.UTF8.GetBytes(serialisedMsg);
            var segment = new ArraySegment<byte>(byteMsg);

            await client.SendAsync(segment, WebSocketMessageType.Text, true, token);
        }

        private async Task ReceiveMessageAsync(CancellationToken token)
        {
            while (true)
            {
                WebSocketReceiveResult result;
                var msgArray = new ArraySegment<byte>(new byte[4096]);

                do
                {
                    result = await client.ReceiveAsync(msgArray, token);
                    var msgBytes = msgArray.Skip(msgArray.Offset).Take(result.Count).ToArray();
                    String serialisedMsg = Encoding.UTF8.GetString(msgBytes);

                    try
                    {
                        var msgObj = JsonConvert.DeserializeObject<Message>(serialisedMsg);
                        if (!messages.Contains(msgObj))
                        {
                            messages.Add(msgObj);
                            string messageFormat = String.Format("{0}: {1}", msgObj.username, msgObj.message);
                            listBox1.Items.Add(messageFormat);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                } while (!result.EndOfMessage);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (client.State != WebSocketState.Open)
            {
                await ConnectToServerAsync(uri, cts.Token);
                
            }

            await SendMessageAsync("Test message", cts.Token);
            if (recieve == null)
            {
                recieve = ReceiveMessageAsync(cts.Token);
            }
            
            //SendMessageAsync("Test 2");

        }
 

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client.State == WebSocketState.Open)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            Properties.Settings.Default.Save();
            Console.WriteLine("Closing");
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["username"] = toolStripTextBox1.Text.ToString();
            Properties.Settings.Default.Save();
        }
    }
}
