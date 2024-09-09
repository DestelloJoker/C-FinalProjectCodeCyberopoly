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
    public partial class PlayerSelectForm : Form
    {
        // Data structure to store selected icons for each player
        private List<Image>[] selectedIcons; 

        // Used to check number of players to hide or enable the appropriate amount of buttons and icons
        private int numberOfPlayers;

        // Tracks whether each player has selected an icon
        private bool[] playerIconSelected = new bool[4]; 

        // Declare image arrays for each player with the possible options included
        Image[] p1Images = {
            Properties.Resources.IconP1Controller,
            Properties.Resources.IconP1Keyboard,
            Properties.Resources.IconP1Mouse,
            Properties.Resources.IconP1Phone,
            Properties.Resources.IconP1Robot
        };

        Image[] p2Images = {
            Properties.Resources.IconP2Controller,
            Properties.Resources.IconP2Keyboard,
            Properties.Resources.IconP2Mouse,
            Properties.Resources.IconP2Phone,
            Properties.Resources.IconP2Robot
        };

        Image[] p3Images = {
            Properties.Resources.IconP3Controller,
            Properties.Resources.IconP3Keyboard,
            Properties.Resources.IconP3Mouse,
            Properties.Resources.IconP3Phone,
            Properties.Resources.IconP3Robot
        };

        Image[] p4Images = {
            Properties.Resources.IconP4Controller,
            Properties.Resources.IconP4Keyboard,
            Properties.Resources.IconP4Mouse,
            Properties.Resources.IconP4Phone,
            Properties.Resources.IconP4Robot
        };

        // Stores the arrays of player images in a dictionary for easy access
        Dictionary<string, Image[]> imagesDictionary;
        // Dictionary to store the current index for each PictureBox
        Dictionary<string, int> currentIndexes;

        public PlayerSelectForm(int numberOfPlayers)
        {
            StartPosition = FormStartPosition.CenterScreen;

            this.numberOfPlayers = numberOfPlayers;

            InitializeComponent();

            InitializePlayerControls(numberOfPlayers);


            // Initialize all player icon dictionaries
            imagesDictionary = new Dictionary<string, Image[]>()
            {
                { "P1IconPictureBox", p1Images },
                { "P2IconPictureBox", p2Images },
                { "P3IconPictureBox", p3Images },
                { "P4IconPictureBox", p4Images }
            };

            currentIndexes = new Dictionary<string, int>()
            {
                { "P1IconPictureBox", 0 },
                { "P2IconPictureBox", 0 },
                { "P3IconPictureBox", 0 },
                { "P4IconPictureBox", 0 }
            };

            // Button click events for each player up to a max of 4
            P1NextIcon.Click += P1NextIcon_Click;
            P1PreviousIcon.Click += P1PreviousIcon_Click;

            P2NextIcon.Click += P2NextIcon_Click;
            P2PreviousIcon.Click += P2PreviousIcon_Click;

            P3NextIcon.Click += P3NextIcon_Click;
            P3PreviousIcon.Click += P3PreviousIcon_Click;

            P4NextIcon.Click += P4NextIcon_Click;
            P4PreviousIcon.Click += P4PreviousIcon_Click;

            selectedIcons = new List<Image>[4]; // Assumes a max of 4 players
                                                // Initializes the list for each player
            for (int i = 0; i < 4; i++)
            {
                selectedIcons[i] = new List<Image>();
            }

        }

        // Method to handle saving icons for each player
        private void SaveIcons(int playerIndex)
        {
            // Disable the "Previous" and "Next" icon buttons for the specified player
            DisableIconButtons(playerIndex);

            // Disable the "Save Icons" button for the specified player
            Button saveButton = (Button)this.Controls.Find($"P{playerIndex + 1}SaveIcon", true)[0];
            saveButton.Enabled = false;
            playerIconSelected[playerIndex] = true;

        }

        private bool AllPlayersSelectedIcons()
        {
            // Check if all players have selected an icon based on the number of players
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (!playerIconSelected[i])
                    return false;
            }
            return true;
        }

        private void CheckEnableNextButton()
        {
            // Enable or disable the Play Game button based on whether all players have selected icons or not
            button2.Enabled = AllPlayersSelectedIcons();
        }

        // Method to disable the "Previous" and "Next" icon buttons for the specified player once their piece
        // has been chosen
        private void DisableIconButtons(int playerIndex)
        {
            // Disable the "Previous" and "Next" icon buttons for the specified player after they have chosen
            // Their icon 
            Button previousButton = (Button)this.Controls.Find($"P{playerIndex + 1}PreviousIcon", true)[0];
            Button nextButton = (Button)this.Controls.Find($"P{playerIndex + 1}NextIcon", true)[0];
            previousButton.Enabled = false;
            nextButton.Enabled = false;
        }

        // Method to handle saving icons for player 1
        private void SaveIconsPlayer1()
        {
            SaveIcons(0); // Player 1 index is 0
        }

        // Method to handle saving icons for player 2
        private void SaveIconsPlayer2()
        {
            SaveIcons(1); // Player 2 index is 1
        }

        // Method to handle saving icons for player 3
        private void SaveIconsPlayer3()
        {
            SaveIcons(2); // Player 3 index is 2
        }

        // Method to handle saving icons for player 4
        private void SaveIconsPlayer4()
        {
            SaveIcons(3); // Player 4 index is 3
        }

        // Event handler for the SaveIconsbutton click for player 1
        private void P1SaveIcon_Click(object sender, EventArgs e)
        {
            SaveIconsPlayer1();
            CheckEnableNextButton();
        }

        // Event handler for the SaveIconsbutton click for player 2
        private void P2SaveIcon_Click(object sender, EventArgs e)
        {
            SaveIconsPlayer2();
            CheckEnableNextButton();
        }

        // Event handler for the SaveIconsbutton click for player 3
        private void P3SaveIcon_Click(object sender, EventArgs e)
        {
            SaveIconsPlayer3();
            CheckEnableNextButton();
        }

        // Event handler for the SaveIconsbutton click for player 4
        private void P4SaveIcon_Click(object sender, EventArgs e)
        {
            SaveIconsPlayer4();
            CheckEnableNextButton();
        }

        // return button that allows user to return to the start screen 
        private void button1_Click(object sender, EventArgs e)
        {
            // Suspends layout to prevent flickering
            SuspendLayout();

            // Hides the current form (PlayerSelectForm)
            Hide();

            // Show Form1
            Form1 form1 = new Form1();
            form1.Show();

            // Resumes layout after showing the next form 
            ResumeLayout();
        }

        //PlayGame button called button2 due to a mistake
        private void button2_Click(object sender, EventArgs e)
        {
            
            if (AllPlayersSelectedIcons())
            {
                // Suspends layout to prevent flickering
                SuspendLayout();
                // Show GameBoardForm
                List<Image>[] selectedIcons = GetSelectedIcons();
                CyberopolyGameBoardForm CyberopolyGameBoard = new CyberopolyGameBoardForm(selectedIcons);
                CyberopolyGameBoard.Show();
                // Hides the current form (PlayerSelectForm)
                Hide();
                // Resumes layout after showing the next form
                ResumeLayout();
            }
            else
            {
                MessageBox.Show("Please ensure all players have selected icons.", "Incomplete Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void P4IconPictureBox_Click(object sender, EventArgs e)
        {

        }
        // Each button named P'x'Next/PreviousIcon is passing the PictureBox name and then incrementing the value
        // of the array for the Icons by 1 or -1 to let the user cycle through all possible pieces
        private void P1NextIcon_Click(object sender, EventArgs e)
        {
            string pictureBoxName = "P1IconPictureBox";
            CycleImages(pictureBoxName, 1); 
        }

        private void P1PreviousIcon_Click(object sender, EventArgs e)
        {
            string pictureBoxName = "P1IconPictureBox";
            CycleImages(pictureBoxName, -1);
        }

        // Method used to allow the used to Cycle between the many potential icons they can use
        private void CycleImages(string pictureBoxName, int increment)
        {
            // Starts the current Index of the current image displayed
            int currentIndex = currentIndexes[pictureBoxName];
            Image[] images = imagesDictionary[pictureBoxName];
            //increment is always by 1 or -1 for proper icon cycling from being called in the icon_Click methods
            currentIndex = (currentIndex + increment + images.Length) % images.Length;
            // Updates current image being displayed
            currentIndexes[pictureBoxName] = currentIndex;

            // Find the PictureBox control by name within the form's Controls collection
            PictureBox pictureBox = Controls[pictureBoxName] as PictureBox;
            if (pictureBox != null)
            {
                pictureBox.Image = images[currentIndex];
            }
            else
            {
                // Handle the case where the control with the specified name is not found
                MessageBox.Show("PictureBox not found.");
            }
        }

        private void P2PreviousIcon_Click(object sender, EventArgs e)
        {
            string pictureBoxName = "P2IconPictureBox";
            CycleImages(pictureBoxName, -1);
        }

        private void P2NextIcon_Click(object sender, EventArgs e)
        {
            string pictureBoxName = "P2IconPictureBox";
            CycleImages(pictureBoxName, 1); // Pass the PictureBox name and the increment value
        }

        // Checks how many players are playing and hides the appropriate amount of player buttons
        private void InitializePlayerControls(int numberOfPlayers)
        {
            // Debug statement to check the number of players but can be kept
            MessageBox.Show($"Number of Players: {numberOfPlayers}"); 

            // Hides/disables player 3 and 4 controls based on the number of players
            if (numberOfPlayers == 2)
            {
                // Debug statement to check if this block is executed but can be kept for now hidden
               // MessageBox.Show("Hiding controls for 3 and 4 players"); 

                // Hide/disable player 3 and 4 controls
                P3PreviousIcon.Visible = false;
                P3NextIcon.Visible = false;
                P3IconPictureBox.Visible = false;
                P3SaveIcon.Visible = false;
                P4PreviousIcon.Visible = false;
                P4NextIcon.Visible = false;
                P4IconPictureBox.Visible = false;
                P4SaveIcon.Visible = false;
               
            }
            else if (numberOfPlayers == 3)
            {
                //same thing, debug statement to check if only 3 players are playing and hides player 4 stuff, hidden for now
               // MessageBox.Show("Hiding controls for 4 players"); 

                // Hide/disable player 4 controls
                P4PreviousIcon.Visible = false;
                P4NextIcon.Visible = false;
                P4IconPictureBox.Visible = false;
                P4SaveIcon.Visible = false;
            }
        }

        private void P3PreviousIcon_Click(object sender, EventArgs e)
        {
            string pictureBoxName = "P3IconPictureBox";
            CycleImages(pictureBoxName, -1);
        }

        private void P3NextIcon_Click(object sender, EventArgs e)
        {
            string pictureBoxName = "P3IconPictureBox";
            CycleImages(pictureBoxName, 1);
        }

        private void P4PreviousIcon_Click(object sender, EventArgs e)
        {
            string pictureBoxName = "P4IconPictureBox";
            CycleImages(pictureBoxName, -1);
        }

        private void P4NextIcon_Click(object sender, EventArgs e)
        {
            string pictureBoxName = "P4IconPictureBox";
            CycleImages(pictureBoxName, 1);
        }

        private List<Image>[] GetSelectedIcons()
        {
            // Initialize a list to store selected icons for each player
            List<Image>[] selectedIcons = new List<Image>[4];

            // Assumes having controls for 4 total players 
            for (int i = 0; i < 4; i++)
            {
                // Initialize the list for the current player
                selectedIcons[i] = new List<Image>();

                // Get the selected icon from the PictureBox control
                PictureBox pictureBox = this.Controls.Find($"P{i + 1}IconPictureBox", true).FirstOrDefault() as PictureBox;
                if (pictureBox != null && pictureBox.Image != null)
                {
                    // Add the selected icon to the list for the current player
                    selectedIcons[i].Add(pictureBox.Image);
                }
            }

            return selectedIcons;
        }

        //don't worry about this, made a mistake
        private void PlayerSelectForm_Load(object sender, EventArgs e)
        {

        }
    }
}
