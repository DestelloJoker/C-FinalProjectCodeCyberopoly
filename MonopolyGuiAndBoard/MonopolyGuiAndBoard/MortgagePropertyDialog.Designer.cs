namespace MonopolyGuiAndBoard
{
    partial class MortgagePropertyDialog
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
            this.MortgageInstructions = new System.Windows.Forms.Label();
            this.MortgageCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.MortgageButton = new System.Windows.Forms.Button();
            this.UnmortgageButton = new System.Windows.Forms.Button();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.UnmortgageCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // MortgageInstructions
            // 
            this.MortgageInstructions.AutoSize = true;
            this.MortgageInstructions.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MortgageInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MortgageInstructions.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.MortgageInstructions.Location = new System.Drawing.Point(115, 9);
            this.MortgageInstructions.Name = "MortgageInstructions";
            this.MortgageInstructions.Size = new System.Drawing.Size(551, 31);
            this.MortgageInstructions.TabIndex = 0;
            this.MortgageInstructions.Text = "Select properties to mortgage or unmortgage";
            // 
            // MortgageCheckedListBox
            // 
            this.MortgageCheckedListBox.FormattingEnabled = true;
            this.MortgageCheckedListBox.Location = new System.Drawing.Point(81, 92);
            this.MortgageCheckedListBox.Name = "MortgageCheckedListBox";
            this.MortgageCheckedListBox.Size = new System.Drawing.Size(200, 229);
            this.MortgageCheckedListBox.TabIndex = 1;
            // 
            // MortgageButton
            // 
            this.MortgageButton.Location = new System.Drawing.Point(95, 354);
            this.MortgageButton.Name = "MortgageButton";
            this.MortgageButton.Size = new System.Drawing.Size(146, 30);
            this.MortgageButton.TabIndex = 3;
            this.MortgageButton.Text = "Mortgage";
            this.MortgageButton.UseVisualStyleBackColor = true;
            this.MortgageButton.Click += new System.EventHandler(this.MortgageButton_Click);
            // 
            // UnmortgageButton
            // 
            this.UnmortgageButton.Location = new System.Drawing.Point(520, 354);
            this.UnmortgageButton.Name = "UnmortgageButton";
            this.UnmortgageButton.Size = new System.Drawing.Size(120, 30);
            this.UnmortgageButton.TabIndex = 4;
            this.UnmortgageButton.Text = "Unmortgage";
            this.UnmortgageButton.UseVisualStyleBackColor = true;
            this.UnmortgageButton.Click += new System.EventHandler(this.UnmortgageButton_Click);
            // 
            // ReturnButton
            // 
            this.ReturnButton.Location = new System.Drawing.Point(12, 12);
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.Size = new System.Drawing.Size(72, 38);
            this.ReturnButton.TabIndex = 5;
            this.ReturnButton.Text = "Cancel Mortgaging";
            this.ReturnButton.UseVisualStyleBackColor = true;
            this.ReturnButton.Click += new System.EventHandler(this.ReturnButton_Click);
            // 
            // UnmortgageCheckedListBox
            // 
            this.UnmortgageCheckedListBox.FormattingEnabled = true;
            this.UnmortgageCheckedListBox.Location = new System.Drawing.Point(485, 92);
            this.UnmortgageCheckedListBox.Name = "UnmortgageCheckedListBox";
            this.UnmortgageCheckedListBox.Size = new System.Drawing.Size(172, 229);
            this.UnmortgageCheckedListBox.TabIndex = 6;
            // 
            // MortgagePropertyDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.UnmortgageCheckedListBox);
            this.Controls.Add(this.ReturnButton);
            this.Controls.Add(this.UnmortgageButton);
            this.Controls.Add(this.MortgageButton);
            this.Controls.Add(this.MortgageCheckedListBox);
            this.Controls.Add(this.MortgageInstructions);
            this.Name = "MortgagePropertyDialog";
            this.Text = "MortgagePropertyDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MortgageInstructions;
        private System.Windows.Forms.CheckedListBox MortgageCheckedListBox;
        private System.Windows.Forms.Button MortgageButton;
        private System.Windows.Forms.Button UnmortgageButton;
        private System.Windows.Forms.Button ReturnButton;
        private System.Windows.Forms.CheckedListBox UnmortgageCheckedListBox;
    }
}