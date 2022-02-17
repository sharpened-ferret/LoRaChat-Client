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


        public override int GetHashCode()
        {
            int hashCode = -475375544;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(username);
            hashCode = hashCode * -1521134295 + timestamp.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(message);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is Message message &&
                   username == message.username &&
                   timestamp == message.timestamp &&
                   this.message == message.message;
        }
    }
}
