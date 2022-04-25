using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoRaChat
{
    // Defines a Message object to store communications
    public class Message
    {
        // Stores message data
        public string username { get; set; }
        public long timestamp { get; set; }
        public string message { get; set; }
        
        // ToString for representing the object
        public override string ToString()
        {
            return $"{username}: {message}";
        }

        // Defines Hash code for hash sets
        public override int GetHashCode()
        {
            int hashCode = -475375544;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(username);
            hashCode = hashCode * -1521134295 + timestamp.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(message);
            return hashCode;
        }

        // Defines equality for comparison operators
        // A message is equal if and only if all properties are equal
        public override bool Equals(object obj)
        {
            return obj is Message message &&
                   username == message.username &&
                   timestamp == message.timestamp &&
                   this.message == message.message;
        }
    }
}
