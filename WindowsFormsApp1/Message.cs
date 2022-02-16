using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
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
    }
}
