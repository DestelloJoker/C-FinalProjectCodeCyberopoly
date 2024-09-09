namespace MonopolyGuiAndBoard
{
    partial class CyberopolyGameBoardForm
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
            this.RollDiceButton = new System.Windows.Forms.Button();
            this.MortgageButton = new System.Windows.Forms.Button();
            this.PayJailFee = new System.Windows.Forms.Button();
            this.BuyPropertyButton = new System.Windows.Forms.Button();
            this.ListAllProperties = new System.Windows.Forms.Button();
            this.EndTurnButton = new System.Windows.Forms.Button();
            this.Player1CashLabel = new System.Windows.Forms.Label();
            this.Player2CashLabel = new System.Windows.Forms.Label();
            this.Player3CashLabel = new System.Windows.Forms.Label();
            this.Player4CashLabel = new System.Windows.Forms.Label();
            this.Player4Box = new System.Windows.Forms.PictureBox();
            this.Player3Box = new System.Windows.Forms.PictureBox();
            this.Player2Box = new System.Windows.Forms.PictureBox();
            this.Player1Box = new System.Windows.Forms.PictureBox();
            this.CyberopolyBoard = new System.Windows.Forms.PictureBox();
            this.allPropertiesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.Player4Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player3Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player2Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player1Box)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CyberopolyBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // RollDiceButton
            // 
            this.RollDiceButton.Location = new System.Drawing.Point(1308, 63);
            this.RollDiceButton.Name = "RollDiceButton";
            this.RollDiceButton.Size = new System.Drawing.Size(141, 28);
            this.RollDiceButton.TabIndex = 0;
            this.RollDiceButton.Text = "Roll Dice";
            this.RollDiceButton.UseVisualStyleBackColor = true;
            this.RollDiceButton.Click += new System.EventHandler(this.RollDiceButton_Click);
            // 
            // MortgageButton
            // 
            this.MortgageButton.Location = new System.Drawing.Point(1308, 111);
            this.MortgageButton.Name = "MortgageButton";
            this.MortgageButton.Size = new System.Drawing.Size(141, 29);
            this.MortgageButton.TabIndex = 1;
            this.MortgageButton.Text = "Mortgage/Unmortgage";
            this.MortgageButton.UseVisualStyleBackColor = true;
            this.MortgageButton.Click += new System.EventHandler(this.MortgageButton_Click);
            // 
            // PayJailFee
            // 
            this.PayJailFee.Location = new System.Drawing.Point(1308, 163);
            this.PayJailFee.Name = "PayJailFee";
            this.PayJailFee.Size = new System.Drawing.Size(141, 29);
            this.PayJailFee.TabIndex = 2;
            this.PayJailFee.Text = "Pay Jail Fee";
            this.PayJailFee.UseVisualStyleBackColor = true;
            this.PayJailFee.Click += new System.EventHandler(this.PayJailFee_Click);
            // 
            // BuyPropertyButton
            // 
            this.BuyPropertyButton.Location = new System.Drawing.Point(1308, 213);
            this.BuyPropertyButton.Name = "BuyPropertyButton";
            this.BuyPropertyButton.Size = new System.Drawing.Size(141, 30);
            this.BuyPropertyButton.TabIndex = 3;
            this.BuyPropertyButton.Text = "Buy Property";
            this.BuyPropertyButton.UseVisualStyleBackColor = true;
            this.BuyPropertyButton.Click += new System.EventHandler(this.BuyPropertyButton_Click);
            // 
            // ListAllProperties
            // 
            this.ListAllProperties.Location = new System.Drawing.Point(1308, 266);
            this.ListAllProperties.Name = "ListAllProperties";
            this.ListAllProperties.Size = new System.Drawing.Size(141, 29);
            this.ListAllProperties.TabIndex = 4;
            this.ListAllProperties.Text = "List Properties";
            this.ListAllProperties.UseVisualStyleBackColor = true;
            this.ListAllProperties.Click += new System.EventHandler(this.ListAllProperties_Click);
            // 
            // EndTurnButton
            // 
            this.EndTurnButton.Location = new System.Drawing.Point(1308, 317);
            this.EndTurnButton.Name = "EndTurnButton";
            this.EndTurnButton.Size = new System.Drawing.Size(141, 27);
            this.EndTurnButton.TabIndex = 5;
            this.EndTurnButton.Text = "Pass Turn";
            this.EndTurnButton.UseVisualStyleBackColor = true;
            this.EndTurnButton.Click += new System.EventHandler(this.EndTurnButton_Click);
            // 
            // Player1CashLabel
            // 
            this.Player1CashLabel.AutoSize = true;
            this.Player1CashLabel.BackColor = System.Drawing.Color.Black;
            this.Player1CashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1CashLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Player1CashLabel.Location = new System.Drawing.Point(174, 1125);
            this.Player1CashLabel.Name = "Player1CashLabel";
            this.Player1CashLabel.Size = new System.Drawing.Size(86, 31);
            this.Player1CashLabel.TabIndex = 11;
            this.Player1CashLabel.Text = "label1";
            // 
            // Player2CashLabel
            // 
            this.Player2CashLabel.AutoSize = true;
            this.Player2CashLabel.BackColor = System.Drawing.Color.Black;
            this.Player2CashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2CashLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Player2CashLabel.Location = new System.Drawing.Point(478, 1125);
            this.Player2CashLabel.Name = "Player2CashLabel";
            this.Player2CashLabel.Size = new System.Drawing.Size(86, 31);
            this.Player2CashLabel.TabIndex = 12;
            this.Player2CashLabel.Text = "label2";
            // 
            // Player3CashLabel
            // 
            this.Player3CashLabel.AutoSize = true;
            this.Player3CashLabel.BackColor = System.Drawing.Color.Black;
            this.Player3CashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player3CashLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Player3CashLabel.Location = new System.Drawing.Point(774, 1125);
            this.Player3CashLabel.Name = "Player3CashLabel";
            this.Player3CashLabel.Size = new System.Drawing.Size(86, 31);
            this.Player3CashLabel.TabIndex = 13;
            this.Player3CashLabel.Text = "label3";
            // 
            // Player4CashLabel
            // 
            this.Player4CashLabel.AutoSize = true;
            this.Player4CashLabel.BackColor = System.Drawing.Color.Black;
            this.Player4CashLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player4CashLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Player4CashLabel.Location = new System.Drawing.Point(1057, 1125);
            this.Player4CashLabel.Name = "Player4CashLabel";
            this.Player4CashLabel.Size = new System.Drawing.Size(86, 31);
            this.Player4CashLabel.TabIndex = 14;
            this.Player4CashLabel.Text = "label4";
            // 
            // Player4Box
            // 
            this.Player4Box.BackColor = System.Drawing.Color.Gold;
            this.Player4Box.Location = new System.Drawing.Point(1197, 1019);
            this.Player4Box.Name = "Player4Box";
            this.Player4Box.Size = new System.Drawing.Size(63, 65);
            this.Player4Box.TabIndex = 10;
            this.Player4Box.TabStop = false;
            // 
            // Player3Box
            // 
            this.Player3Box.BackColor = System.Drawing.Color.Green;
            this.Player3Box.Location = new System.Drawing.Point(1119, 1019);
            this.Player3Box.Name = "Player3Box";
            this.Player3Box.Size = new System.Drawing.Size(63, 65);
            this.Player3Box.TabIndex = 9;
            this.Player3Box.TabStop = false;
            // 
            // Player2Box
            // 
            this.Player2Box.BackColor = System.Drawing.Color.Red;
            this.Player2Box.Location = new System.Drawing.Point(1197, 948);
            this.Player2Box.Name = "Player2Box";
            this.Player2Box.Size = new System.Drawing.Size(63, 65);
            this.Player2Box.TabIndex = 8;
            this.Player2Box.TabStop = false;
            // 
            // Player1Box
            // 
            this.Player1Box.BackColor = System.Drawing.Color.RoyalBlue;
            this.Player1Box.Location = new System.Drawing.Point(1119, 948);
            this.Player1Box.Name = "Player1Box";
            this.Player1Box.Size = new System.Drawing.Size(63, 65);
            this.Player1Box.TabIndex = 7;
            this.Player1Box.TabStop = false;
            // 
            // CyberopolyBoard
            // 
            this.CyberopolyBoard.Image = global::MonopolyGuiAndBoard.Properties.Resources.CyberopolyBoardFinished;
            this.CyberopolyBoard.Location = new System.Drawing.Point(180, 10);
            this.CyberopolyBoard.Name = "CyberopolyBoard";
            this.CyberopolyBoard.Size = new System.Drawing.Size(1100, 1100);
            this.CyberopolyBoard.TabIndex = 6;
            this.CyberopolyBoard.TabStop = false;
            // 
            // allPropertiesCheckedListBox
            // 
            this.allPropertiesCheckedListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.allPropertiesCheckedListBox.FormattingEnabled = true;
            this.allPropertiesCheckedListBox.Location = new System.Drawing.Point(180, 1173);
            this.allPropertiesCheckedListBox.Name = "allPropertiesCheckedListBox";
            this.allPropertiesCheckedListBox.Size = new System.Drawing.Size(1100, 140);
            this.allPropertiesCheckedListBox.TabIndex = 15;
            // 
            // CyberopolyGameBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.RoyalBlue;
            this.ClientSize = new System.Drawing.Size(1471, 1389);
            this.Controls.Add(this.allPropertiesCheckedListBox);
            this.Controls.Add(this.Player4CashLabel);
            this.Controls.Add(this.Player3CashLabel);
            this.Controls.Add(this.Player2CashLabel);
            this.Controls.Add(this.Player1CashLabel);
            this.Controls.Add(this.Player4Box);
            this.Controls.Add(this.Player3Box);
            this.Controls.Add(this.Player2Box);
            this.Controls.Add(this.Player1Box);
            this.Controls.Add(this.CyberopolyBoard);
            this.Controls.Add(this.EndTurnButton);
            this.Controls.Add(this.ListAllProperties);
            this.Controls.Add(this.BuyPropertyButton);
            this.Controls.Add(this.PayJailFee);
            this.Controls.Add(this.MortgageButton);
            this.Controls.Add(this.RollDiceButton);
            this.Name = "CyberopolyGameBoardForm";
            this.Text = "CyberopolyGameBoard";
            this.Load += new System.EventHandler(this.CyberopolyGameBoardForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Player4Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player3Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player2Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Player1Box)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CyberopolyBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RollDiceButton;
        private System.Windows.Forms.Button MortgageButton;
        private System.Windows.Forms.Button PayJailFee;
        private System.Windows.Forms.Button BuyPropertyButton;
        private System.Windows.Forms.Button ListAllProperties;
        private System.Windows.Forms.Button EndTurnButton;
        private System.Windows.Forms.PictureBox CyberopolyBoard;
        private System.Windows.Forms.PictureBox Player1Box;
        private System.Windows.Forms.PictureBox Player2Box;
        private System.Windows.Forms.PictureBox Player3Box;
        private System.Windows.Forms.PictureBox Player4Box;
        private System.Windows.Forms.Label Player1CashLabel;
        private System.Windows.Forms.Label Player2CashLabel;
        private System.Windows.Forms.Label Player3CashLabel;
        private System.Windows.Forms.Label Player4CashLabel;
        private System.Windows.Forms.CheckedListBox allPropertiesCheckedListBox;
    }
}