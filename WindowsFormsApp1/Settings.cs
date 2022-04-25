using System;
using System.Windows.Forms;

namespace LoRaChat
{
    public partial class Settings : Form
    {
        // Initialises the settings window
        public Settings()
        {
            InitializeComponent();
        }

        // Saves username changes to non-volatile settings file
        private void username_box_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.username = username_box.Text;
            Properties.Settings.Default.Save();
        }

        // Saves endpoint WebSocket address changes to non-volatile settings file
        private void node_address_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ws_path = node_address.Text;
            Properties.Settings.Default.Save();
        }
    }
}
