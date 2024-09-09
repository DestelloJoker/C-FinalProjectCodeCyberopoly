using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace MonopolyGuiAndBoard
{
    public partial class Form1 : Form
    {
        // Int to store the input for the number of players which will be used later
        private int numberOfPlayers; 
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            // Initially disables the playgame button
            button1.Enabled = false; 
            AboutGameButton.Visible = false;  
        }

        private void button1_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Select your piece!");

            // Suspends layout to prevent flickering
            SuspendLayout();
            
            // Hides the current form (Form1)
            Hide();

            // Show playerSelectForm
            PlayerSelectForm playerSelectForm = new PlayerSelectForm(numberOfPlayers);
            playerSelectForm.Show();

            // Resume layout after showing the next form
            ResumeLayout();

        }
        //Button meant to open the word doc that explains all of cyberopoly currently not working
        private void AboutGameButton_Click(object sender, EventArgs e)
        {
            // Get the directory where the executable is located
            string executableDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // This line Combines the executable directory with the Word document's filename
            string documentPath = Path.Combine(executableDirectory, "AboutCyberopoly.docx");

            // Check if the document exists
            if (File.Exists(documentPath))
            {
                // Open the about cyberopoly word document
                Process.Start("WINWORD.EXE", documentPath);
            }
            else
            {
                // If the document doesn't exist or cannot be found, display an error message
                MessageBox.Show("The document is missing.");
            }
        }
        private void InputPlayersButton_Click(object sender, EventArgs e)
        {
            // Validates the user input
            bool isValidInput = int.TryParse(NumberOfPlayersTextBox.Text, out numberOfPlayers) &&
                                (numberOfPlayers >= 2 && numberOfPlayers <= 4);

            if (isValidInput)
            {
                // If input is a valid int, enable the "Play Game" button
                button1.Enabled = true;
            }
            else
            {
                // If input is an invalid variable, display an error message
                MessageBox.Show("Please enter a valid number of players (2, 3, or 4).", "Invalid Input",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
