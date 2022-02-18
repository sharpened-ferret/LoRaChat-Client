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

namespace LoRaChat
{
    public partial class ChatWindow : Form
    {
        private readonly ClientWebSocket _client = new ClientWebSocket();
        private readonly List<Message> _messages = new List<Message>();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Uri _uri = new Uri("ws://192.168.0.76:80/ws");
        private Task _receive;

        public ChatWindow()
        {
            InitializeComponent();
        }

        private async Task ConnectToServerAsync(Uri uri, CancellationToken token)
        {
            var keepAlive = new TimeSpan(0, 30, 0);
            _client.Options.KeepAliveInterval = keepAlive;
            await _client.ConnectAsync(uri, token);
        }

        private async Task SendMessageAsync(string message, CancellationToken token)
        {
            var msg = new Message
            {
                username = Properties.Settings.Default["username"].ToString(),
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                message = textBox1.Text
            };
            var serialisedMsg = JsonConvert.SerializeObject(msg);

            var byteMsg = Encoding.UTF8.GetBytes(serialisedMsg);
            var segment = new ArraySegment<byte>(byteMsg);

            await _client.SendAsync(segment, WebSocketMessageType.Text, true, token);
        }

        private async Task ReceiveMessageAsync(CancellationToken token)
        {
            while (true)
            {
                WebSocketReceiveResult result;
                var msgArray = new ArraySegment<byte>(new byte[4096]);

                do
                {
                    result = await _client.ReceiveAsync(msgArray, token);
                    var msgBytes = msgArray.Skip(msgArray.Offset).Take(result.Count).ToArray();
                    var serialisedMsg = Encoding.UTF8.GetString(msgBytes);

                    try
                    {
                        var msgObj = JsonConvert.DeserializeObject<Message>(serialisedMsg);
                        if (!_messages.Contains(msgObj))
                        {
                            if (msgObj != null)
                            {
                                _messages.Add(msgObj);
                                var messageFormat = $"{msgObj.username}: {msgObj.message}";
                                chat_log.Items.Add(messageFormat);
                            }
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
            if (_client.State != WebSocketState.Open)
            {
                await ConnectToServerAsync(_uri, _cts.Token);
                connectionStatusLabel.Text = "Connected";
                connectionStatusLabel.ForeColor = Color.Green;
            }

            await SendMessageAsync("Test message", _cts.Token);
            if (_receive == null)
            {
                _receive = ReceiveMessageAsync(_cts.Token);
            }
        }
 

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_client.State == WebSocketState.Open)
            {
                await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            Properties.Settings.Default.Save();
            Console.WriteLine("Closing");
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.ShowDialog();
        }
    }
}
