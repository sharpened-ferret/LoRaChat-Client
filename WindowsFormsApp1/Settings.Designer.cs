namespace LoRaChat
{
    partial class Settings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.node_address = new System.Windows.Forms.TextBox();
            this.username_box = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Node Address:";
            // 
            // node_address
            // 
            this.node_address.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LoRaChat.Properties.Settings.Default, "ws_path", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.node_address.Location = new System.Drawing.Point(109, 64);
            this.node_address.Name = "node_address";
            this.node_address.Size = new System.Drawing.Size(117, 20);
            this.node_address.TabIndex = 3;
            this.node_address.Text = global::LoRaChat.Properties.Settings.Default.ws_path;
            this.node_address.TextChanged += new System.EventHandler(this.node_address_TextChanged);
            // 
            // username_box
            // 
            this.username_box.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::LoRaChat.Properties.Settings.Default, "username", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.username_box.Location = new System.Drawing.Point(109, 38);
            this.username_box.Name = "username_box";
            this.username_box.Size = new System.Drawing.Size(117, 20);
            this.username_box.TabIndex = 1;
            this.username_box.Text = global::LoRaChat.Properties.Settings.Default.username;
            this.username_box.TextChanged += new System.EventHandler(this.username_box_TextChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.node_address);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.username_box);
            this.Controls.Add(this.label1);
            this.Name = "Settings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox username_box;
        private System.Windows.Forms.TextBox node_address;
        private System.Windows.Forms.Label label2;
    }
}