using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRaChat
{
    public class Message
    {
        public string username { get; set; }
        public long timestamp { get; set; }
        public string message { get; set; }
        
        public override string ToString()
        {
            return String.Format("{0}: {1}", username, message);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Message other = obj as Message;
            if (other == null) return false;
            else
            {
                if (other.username == username && other.timestamp == timestamp && other.message == message) return true;
            }
            return false;
        }
    }
}
