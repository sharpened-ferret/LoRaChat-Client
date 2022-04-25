using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using System.Windows.Forms;
using LoRaChat.Properties;

namespace LoRaChat
{
    public partial class ChatWindow : Form
    {
        // Stores data for the WebSocket connection with the Endpoint
        private readonly ClientWebSocket _client = new ClientWebSocket();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly Uri _websocketUri;
        private Task _receive;

        // Stores message history from the current session
        private readonly List<Message> _messages = new List<Message>();
        
        // Initialises the chat window
        public ChatWindow()
        {
            InitializeComponent();

            // Initialises the WebSocket using the address stored in the program properties, if valid.
            try
            {
                _websocketUri = new Uri(Properties.Settings.Default.ws_path);
            }
            // If the stored URI is invalid, a default URI is used instead, and the properties are updated
            // This is set as the default Endpoint IP address
            catch (UriFormatException)
            {
                _websocketUri = new Uri("ws://192.168.4.1:80/ws");
                Properties.Settings.Default.ws_path = "ws://192.168.4.1:80/ws";
                Properties.Settings.Default.Save();
            }
        }

        // Handles connecting to the Endpoint using WebSockets
        private async Task ConnectToServerAsync(Uri uri, CancellationToken token)
        {
            var keepAlive = new TimeSpan(0, 30, 0);
            _client.Options.KeepAliveInterval = keepAlive;
            await _client.ConnectAsync(uri, token);
        }

        // Sends a message over the WebSocket connection in JSON format
        private async Task SendMessageAsync(string message, CancellationToken token)
        {
            // Creates temporary Message object
            var msg = new Message
            {
                username = Properties.Settings.Default["username"].ToString(),
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                message = message
            };

            // Converts the Message to JSON and sends to the Endpoint
            var serialisedMsg = JsonConvert.SerializeObject(msg);
            var byteMsg = Encoding.UTF8.GetBytes(serialisedMsg);
            var segment = new ArraySegment<byte>(byteMsg);
            await _client.SendAsync(segment, WebSocketMessageType.Text, true, token);
        }

        // Handles receiving messages from the Endpoint
        private async Task ReceiveMessageAsync(CancellationToken token)
        {
            while (true)
            {
                WebSocketReceiveResult result;
                var msgArray = new ArraySegment<byte>(new byte[4096]);

                do
                {
                    // Receives data from the WebSocket connection
                    result = await _client.ReceiveAsync(msgArray, token);
                    var msgBytes = msgArray.Skip(msgArray.Offset).Take(result.Count).ToArray();
                    var serialisedMsg = Encoding.UTF8.GetString(msgBytes);

                    // Converts the received message to a Message object
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
                    // If the message is invalid, it is ignored and an message is logged
                    catch (Exception ex)
                    {
                        Console.WriteLine(Resources.Invalid_Message_Error);
                    }
                } while (!result.EndOfMessage);
            }
        }

        // Handles button click to send a message to the Endpoint
        private async void SendButton_Click(object sender, EventArgs e)
        {
            // If the Endpoint WebSocket connection is not open, a connection is opened
            if (_client.State != WebSocketState.Open)
            {
                try
                {
                    await ConnectToServerAsync(_websocketUri, _cts.Token);

                    // GUI is updated to reflect the new connection status
                    connectionStatusLabel.Text = _client.State.ToString();
                    connectionStatusLabel.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(_client.State);
                }
            }

            // Attempts to send a message to the Endpoint
            try
            {
                // Sends the message-box contents to the Endpoint, and empties the message-box
                await SendMessageAsync(messageInputBox.Text, _cts.Token);
                messageInputBox.Text = string.Empty;
                if (_receive == null)
                {
                    _receive = ReceiveMessageAsync(_cts.Token);
                }
            }
            // If the message can't be sent, an error pop-up is displayed to the user
            catch (InvalidOperationException)
            {
                MessageBox.Show(Resources.Disconnected_Error_Message_Text, Resources.Disconnected_Error_Message_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Closes WebSocket connection on application close
        private async void ChatWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_client.State == WebSocketState.Open)
            {
                await _client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            Properties.Settings.Default.Save();
        }

        // Handles opening the settings window
        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = new Settings();
            settings.ShowDialog();
        }
    }
}
