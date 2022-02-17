using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoRaChat
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void username_box_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["username"] = username_box.Text.ToString();
            Properties.Settings.Default.Save();
        }

        private void node_address_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["ws_path"] = node_address.Text.ToString();
            Properties.Settings.Default.Save();
        }
    }
}
