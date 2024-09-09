namespace MonopolyGuiAndBoard
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.AboutGameButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.NumberOfPlayersTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InputPlayersButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(369, 311);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(164, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "Play Game";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AboutGameButton
            // 
            this.AboutGameButton.Location = new System.Drawing.Point(400, 382);
            this.AboutGameButton.Name = "AboutGameButton";
            this.AboutGameButton.Size = new System.Drawing.Size(100, 55);
            this.AboutGameButton.TabIndex = 1;
            this.AboutGameButton.Text = "About Cyberopoly";
            this.AboutGameButton.UseVisualStyleBackColor = true;
            this.AboutGameButton.Click += new System.EventHandler(this.AboutGameButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MonopolyGuiAndBoard.Properties.Resources.CyberOPOLY;
            this.pictureBox1.Location = new System.Drawing.Point(167, 90);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(547, 124);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // NumberOfPlayersTextBox
            // 
            this.NumberOfPlayersTextBox.Location = new System.Drawing.Point(614, 325);
            this.NumberOfPlayersTextBox.Name = "NumberOfPlayersTextBox";
            this.NumberOfPlayersTextBox.Size = new System.Drawing.Size(65, 20);
            this.NumberOfPlayersTextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(576, 291);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "How many players will play?";
            // 
            // InputPlayersButton
            // 
            this.InputPlayersButton.Location = new System.Drawing.Point(579, 364);
            this.InputPlayersButton.Name = "InputPlayersButton";
            this.InputPlayersButton.Size = new System.Drawing.Size(135, 23);
            this.InputPlayersButton.TabIndex = 5;
            this.InputPlayersButton.Text = "Set Player Count";
            this.InputPlayersButton.UseVisualStyleBackColor = true;
            this.InputPlayersButton.Click += new System.EventHandler(this.InputPlayersButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(899, 587);
            this.Controls.Add(this.InputPlayersButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NumberOfPlayersTextBox);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.AboutGameButton);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button AboutGameButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox NumberOfPlayersTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button InputPlayersButton;
    }
}

