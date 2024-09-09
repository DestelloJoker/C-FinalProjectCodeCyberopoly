using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MonopolyGuiAndBoard.CyberopolyGameBoardForm;
using static System.Windows.Forms.AxHost;

namespace MonopolyGuiAndBoard
{
    public partial class CyberopolyGameBoardForm : Form
    {
        // List to store player instances
        private List<Player> players = new List<Player>(); 
        // array representing the game board
        private BoardSpace[] gameBoard = new BoardSpace[40];     
        // Index of the current player in the players list
        private int currentPlayerIndex = 0;
        // Getter method for currentPlayerIndex
        public int CurrentPlayerIndex { get { return currentPlayerIndex; } }
        // Array to store the position of each player
        private int[] playerPositions;
        // Total roll of two dice
        private int TotalRoll = 0;
        // Assumes there are 4 players
        private int[] totalRollAmounts = new int[4]; 
        //start freeParking to 0
        public int freeParkingPool = 0;

        // Array to store all dice faces for later use
        private Image[] diceFaces = new Image[] {
        Properties.Resources.DiceFace1,
        Properties.Resources.DiceFace2,
        Properties.Resources.DiceFace3,
        Properties.Resources.DiceFace4,
        Properties.Resources.DiceFace5,
        Properties.Resources.DiceFace6
    };
        // for the most part outside of the board initialization, this goes mostly unused due to the fact color sets are not used
        // only actually used on 3 total lines since I wanted to use it still
        public enum PropertyCategory
        {
            Brown,
            LightBlue,
            Pink,
            Orange,
            Red,
            Yellow,
            DarkGreen,
            DarkBlue,
            Railroad,
            Utility,
            // Umbrella category for special spaces like tax, jail, GO, chance, community chest.
            Special
        }
        // for the most part outside of the board initialization, this goes mostly unused due to the fact color sets are not used
        // only actually used on 3 total lines since I wanted to use it still
        public static class ColorMapper
        {
            public static Color GetColorForCategory(PropertyCategory category)
            {
                switch (category)
                {
                    // Default property colors
                    case PropertyCategory.Brown:
                        return Color.Brown;
                    case PropertyCategory.LightBlue:
                        return Color.LightBlue;
                    case PropertyCategory.Pink:
                        return Color.Pink;
                    case PropertyCategory.Orange:
                        return Color.Orange;
                    case PropertyCategory.Red:
                        return Color.Red;
                    case PropertyCategory.Yellow:
                        return Color.Yellow;
                    case PropertyCategory.DarkGreen:
                        return Color.DarkGreen;
                    case PropertyCategory.DarkBlue:
                        return Color.DarkBlue;
                    case PropertyCategory.Railroad:
                        // Custom color for railroad
                        return Color.Black; 
                    case PropertyCategory.Utility:
                        // Custom color for utility
                        return Color.Gray; 
                    case PropertyCategory.Special:
                        // Custom color for special spaces
                        return Color.DarkGray; 
                    default:
                        // Default color
                        return Color.White; 
                }
            }
        }
        // Class player with the many different Player specific variables that need to be tracked or used 
        public class Player
        {
            // Current position of the player
            public int Position { get; set; }
            // Icon for the player
            public Image Icon { get; set; }
            public object Tag { get; set; }

            // Add a property to store the starting cash
            public double StartingCash { get; set; }
            public int PlayerIndex { get; set; }
            public int TotalCash { get; set; }
            // Property to track the number of railroads owned by the player
            public int NumberOfRailroadsOwned { get; set; }
            // Flag to indicate if the player is in jail
            public bool OwnsBothUtilities { get; set; }
            public int TurnsInJail { get; set; }
            public bool InJail { get; set; }

            //get out of jail free card
            public bool GoojfCard { get; set; }
            public bool PassedGo { get; set; }
            // Flag to indicate if the player paid the jail fee
            public bool PaidJailFee { get; set; }

            // Flag to indicate if the player rolled doubles in the current turn
            public bool RolledDoubles { get; set; }

            // Counter to track consecutive times the player rolled doubles
            public int ConsecutiveDoublesCount { get; set; }

            public List<BoardSpace> OwnedProperties { get; private set; }

            // Constructor for player variables
            public Player()
            {

                Position = 0;
                StartingCash = 0;
                TotalCash = 0;
                NumberOfRailroadsOwned = 0;
                TurnsInJail = 0;
                InJail = false;
                PassedGo = false;
                GoojfCard = false;
                PaidJailFee = false;
                RolledDoubles = false;
                ConsecutiveDoublesCount = 0;
                OwnedProperties = new List<BoardSpace>();
            }

            // Method to add a property to the player's owned properties
            public void AddOwnedProperty(BoardSpace property)
            {
                OwnedProperties.Add(property);
            }

            // Method to remove a property from the player's owned properties
            public void RemoveOwnedProperty(BoardSpace property)
            {
                OwnedProperties.Remove(property);
            }
        }

        // Class BoardSpace with the many different BoardSpace specific variables that need to be tracked or used 
        public class BoardSpace
        {
            // BoardSpace variables

            // Space position and size are unused and need to be removed when delivering the final project
            // Goal was to track the position where the picture box would go to and set it then use size to dynamically set buttons to pop up 
            // individual property data and not make a button with all property data listed
            public string Name { get; set; } 
            public Color Color { get; set; }
            public int Price { get; set; }
            public int RentCost { get; set; }
            // Rent cost for railroads
            public int RailRoadRentCost { get; set; } 
            // Rent cost for utilities
            public int UtilityRentCost { get; set; } 

            public bool IsOwned { get; set; }
            // Index of the player who owns the space (-1 if not owned)
            public bool IsMortgaged { get; set; }
            public int OwnerIndex { get; set; } 
            public int MortgageValue { get; set; }
            // Coordinates of the space UNUSED
            public Point Position { get; set; }
            // Size of the space UNUSED
            public Size Size { get; set; } 
            // Checks if a property is able to be bought
            public bool IsPurchasable { get; set; }
            // Flag to indicate if a player owns both utilities
            public bool OwnsBothUtilities { get; set; }

            // Method to update the utility rent based on ownership
            public void UpdateUtilityRent(bool ownsBothUtilities, int roll)
            {
                OwnsBothUtilities = ownsBothUtilities;
                if (OwnsBothUtilities)
                {
                    // If the player owns both utilities, set the rent to total roll times 10
                    UtilityRentCost = 10 * roll;
                }
                else
                {
                    // If the player owns only one utility, set the rent to total roll times 4
                    UtilityRentCost = 4 * roll;
                }
            }
            // Method to update RailRoadrent based on how many a single player owns
            public void UpdateRailroadRent(int numberOfRailroadsOwned)
            {
                // Calculate the rent based on the number of railroads owned by a single player
                switch (numberOfRailroadsOwned)
                {
                    case 1:
                        RailRoadRentCost = 25;
                        break;
                    case 2:
                        RailRoadRentCost = 50;
                        break;
                    case 3:
                        RailRoadRentCost = 100;
                        break;
                    case 4:
                        RailRoadRentCost = 200;
                        break;
                    // Default rent cost if no players own any railroads
                    default:
                        RailRoadRentCost = 0; 
                        break;
                }
            }

            // Method to double the rent cost
            public void DoubleRentCost()
            {
                // Double the rent cost
                 RentCost *= 2;
            }

        }


        public CyberopolyGameBoardForm(List<Image>[] playerIcons)
        {
            InitializeComponent();
            // Place the icons on the game board
            // Pass playerIcons to the method and initialize players with starting positions and icons
            InitializePlayers(playerIcons);
            PlacePlayerIcons(playerIcons);
            // Initialize the game board
            InitializeBoard();
            MovePlayerIcons();
            allPropertiesCheckedListBox.Visible = false;
            StartPosition = FormStartPosition.CenterScreen;
        }
        
        private void PlacePlayerIcons(List<Image>[] playerIcons)
        {
            // Hide Player3 and Player4 components initially
            Player3Box.Visible = false;
            Player3CashLabel.Visible = false;
            Player4Box.Visible = false;
            Player4CashLabel.Visible = false;



            // Update positions of existing PictureBox controls for player icons
            for (int i = 0; i < playerIcons.Length; i++)
            {
                if (playerIcons[i].Count > 0)
                {
                    // Assign the icon to the respective PictureBox
                    if (i == 0)
                    {
                        Player1Box.Tag = 0;
                        Player1Box.Image = ResizeImage(playerIcons[i][0], 0.3f);
                    }
                    else if (i == 1)
                    {
                        Player2Box.Tag = 1;
                        Player2Box.Image = ResizeImage(playerIcons[i][0], 0.3f);
                    }
                    else if (i == 2)
                    {
                        // Show Player3 components if there are 3 or more players
                        Player3Box.Visible = true;
                        Player3CashLabel.Visible = true;
                        Player3Box.Tag = 2;
                        if (playerIcons.Length >= 3)
                            Player3Box.Image = ResizeImage(playerIcons[i][0], 0.3f);
                    }
                    else if (i == 3)
                    {
                        // Show Player4 components if there are 4 players
                        Player4Box.Visible = true;
                        Player4CashLabel.Visible = true;
                        Player4Box.Tag = 3;
                        if (playerIcons.Length == 4)
                            Player4Box.Image = ResizeImage(playerIcons[i][0], 0.3f);
                    }
                }
            }

            // Update labels for player cash
            for (int i = 0; i < playerIcons.Length; i++)
            {
                // Assuming players list is sorted by player index
                if (i < players.Count)
                {
                    // Update the corresponding cash label
                    if (i == 0)
                    {
                        Player1CashLabel.Text = $"Player 1: ${players[i].TotalCash}";
                    }
                    else if (i == 1)
                    {
                        Player2CashLabel.Text = $"Player 2: ${players[i].TotalCash}";
                    }
                    else if (i == 2)
                    {
                        // Update Player3 cash label if there are 3 or more players
                        if (playerIcons.Length >= 3)
                            Player3CashLabel.Text = $"Player 3: ${players[i].TotalCash}";
                    }
                    else if (i == 3)
                    {
                        // Update Player4 cash label if there are 4 players
                        if (playerIcons.Length == 4)
                            Player4CashLabel.Text = $"Player 4: ${players[i].TotalCash}";
                    }
                }
            }
        }
        // Method Used to properly size the player images to be set on the board 
        private Image ResizeImage(Image originalImage, float scaleFactor)
        {
            int newWidth = (int)(originalImage.Width * scaleFactor);
            int newHeight = (int)(originalImage.Height * scaleFactor);
            return new Bitmap(originalImage, new Size(newWidth, newHeight));
        }

        private void InitializePlayers(List<Image>[] playerIcons)
        {
            // Clear any existing players if there are any
            players.Clear();

            // Initialize playerPositions array to store player positions
            // Use playerIcons.Length instead of players.Count
            playerPositions = new int[playerIcons.Length];
            // Loop through playerIcons.Length
            for (int i = 0; i < playerIcons.Length; i++) 
            {
                if (playerIcons[i].Count > 0)
                {
                    // Create a new player instance
                    Player player = new Player();
                    // Start position at space 0
                    player.Position = 0;
                    // Set the icon for the player
                    player.Icon = playerIcons[i][0]; 

                    // Tag the player with its index
                    player.Tag = i;

                    // Set the player's total cash to 1500
                    player.TotalCash = 1500;

                    // Add the player to the players list
                    players.Add(player);

                    // Initialize player position
                    playerPositions[i] = 0;
                }
            }
        }

        // Space position and size are unused and need to be removed when delivering the final project
        // Goal was to track the position where the picture box would go to and set it then use size to dynamically set buttons to pop up 
        // individual property data and not make a button with all property data listed
        private void InitializeBoard()
        {
            // Initialize the game board with empty spaces Starting at GO!!!
            gameBoard[0] = new BoardSpace
            {
                Name = "GO!!!",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(1150, 1010), // Space Coords
                Size = new Size(145, 145), //Space size
                IsPurchasable = false
            };
            //Mediterranean Avenue
            gameBoard[1] = new BoardSpace
            {
                Name = "Mediterranean Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Brown),
                Price = 60,
                RentCost = 2,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 30,
                Position = new Point(1036, 1024), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = true
            };
            //Community Chest space 1
            gameBoard[2] = new BoardSpace
            {
                Name = "Community Chest",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 30,
                Position = new Point(946, 1013), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = false
            };
            //Baltic Avenue
            gameBoard[3] = new BoardSpace
            {
                Name = "Baltic Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Brown),
                Price = 60,
                RentCost = 4,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 30,
                Position = new Point(851, 946), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = true
            };
            //Income Tax
            gameBoard[4] = new BoardSpace
            {
                Name = "Income Tax",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(763, 946), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = false
            };
            //Reading Railroad
            gameBoard[5] = new BoardSpace
            {
                Name = "Reading Railroad",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Railroad),
                Price = 200,
                RailRoadRentCost = 0, //25 times number of owned railroads, will add later
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 100,
                Position = new Point(674, 948), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = true
            };
            //Oriental Avenue
            gameBoard[6] = new BoardSpace
            {
                Name = "Oriental Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.LightBlue),
                Price = 100,
                RentCost = 6,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 50,
                Position = new Point(588, 947), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = true
            };
            //Chance Space 1
            gameBoard[7] = new BoardSpace
            {
                Name = "Chance Space",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(499, 948), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = false
            };
            //Vermont Avenue
            gameBoard[8] = new BoardSpace
            {
                Name = "Vermont Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.LightBlue),
                Price = 100,
                RentCost = 6,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 50,
                Position = new Point(412, 947), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = true
            };
            //Connecticut Avenue
            gameBoard[9] = new BoardSpace
            {
                Name = "Connecticut Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.LightBlue),
                Price = 120,
                RentCost = 8,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 60,
                Position = new Point(323, 948), // Space Coords
                Size = new Size(88, 140), //Space size
                IsPurchasable = true
            };
            // Just visiting
            gameBoard[10] = new BoardSpace
            {
                Name = "Just visiting",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(180, 947), // Space Coords
                Size = new Size(145, 145), //Space size
                IsPurchasable = false
            };
            //St. Charles Avenue
            gameBoard[11] = new BoardSpace
            {
                Name = "St. Charles Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Pink),
                Price = 140,
                RentCost = 10,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 70,
                Position = new Point(180, 853), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = true
            };
            //Electric Company
            gameBoard[12] = new BoardSpace
            {
                Name = "Electric Company",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Utility),
                Price = 150,
                UtilityRentCost = 0, // will fix later
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 75,
                Position = new Point(180, 765), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = true
            };
            //States Avenue
            gameBoard[13] = new BoardSpace
            {
                Name = "States Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Pink),
                Price = 140,
                RentCost = 10,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 70,
                Position = new Point(180, 680), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = true
            };
            //Virginia Avenue
            gameBoard[14] = new BoardSpace
            {
                Name = "Virginia Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Pink),
                Price = 160,
                RentCost = 12,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 80,
                Position = new Point(180, 593), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = true
            };
            //Pennsylvania RailRoad
            gameBoard[15] = new BoardSpace
            {
                Name = "Pennsylvania RailRoad",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Railroad),
                Price = 200,
                RailRoadRentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 100,
                Position = new Point(180, 505), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = true
            };
            //St. James Avenue
            gameBoard[16] = new BoardSpace
            {
                Name = "St. James Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Orange),
                Price = 180,
                RentCost = 14,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 90,
                Position = new Point(180, 417), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = true
            };
            //Community Chest space 2
            gameBoard[17] = new BoardSpace
            {
                Name = "Community Chest",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(180, 328), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = false
            };
            //Tennessee Avenue
            gameBoard[18] = new BoardSpace
            {
                Name = "Tennessee Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Orange),
                Price = 180,
                RentCost = 14,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 90,
                Position = new Point(180, 240), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = true
            };
            //New York Avenue
            gameBoard[19] = new BoardSpace
            {
                Name = "New York Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Orange),
                Price = 200,
                RentCost = 16,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 100,
                Position = new Point(180, 152), // Space Coords
                Size = new Size(141, 94), //Space size
                IsPurchasable = true
            };
            //Free parking
            gameBoard[20] = new BoardSpace
            {
                Name = "Free parking",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(180, 12), // Space Coords
                Size = new Size(145, 145), //Space size
                IsPurchasable = false
            };
            //Kentucky Avenue
            gameBoard[21] = new BoardSpace
            {
                Name = "Kentucky Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Red),
                Price = 220,
                RentCost = 18,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 110,
                Position = new Point(321, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = true
            };
            //Chance space 2
            gameBoard[22] = new BoardSpace
            {
                Name = "Chance space",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(408, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = false
            };
            //Indiana Avenue
            gameBoard[23] = new BoardSpace
            {
                Name = "Indiana Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Red),
                Price = 220,
                RentCost = 18,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 110,
                Position = new Point(497, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = true
            };
            //Illinois Avenue
            gameBoard[24] = new BoardSpace
            {
                Name = "Illinois Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Red),
                Price = 240,
                RentCost = 20,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 120,
                Position = new Point(584, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = true
            };
            //B.&.O RailRoad
            gameBoard[25] = new BoardSpace
            {
                Name = "B.&.O RailRoad",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Railroad),
                Price = 200,
                RailRoadRentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 100,
                Position = new Point(673, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = true
            };
            //Atlantic Avenue
            gameBoard[26] = new BoardSpace
            {
                Name = "Atlantic Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Yellow),
                Price = 260,
                RentCost = 22,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 130,
                Position = new Point(761, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = true
            };
            //Ventnor Avenue
            gameBoard[27] = new BoardSpace
            {
                Name = "Ventnor Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Yellow),
                Price = 260,
                RentCost = 22,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 130,
                Position = new Point(849, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = true
            };
            //Electric Company
            gameBoard[28] = new BoardSpace
            {
                Name = "Electric Company",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Utility),
                Price = 150,
                UtilityRentCost = 0, // will fix later
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 75,
                Position = new Point(936, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = true
            };
            //Marvin Gardens
            gameBoard[29] = new BoardSpace
            {
                Name = "Marvin Gardens",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Yellow),
                Price = 280,
                RentCost = 24,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 140,
                Position = new Point(1026, 12), // Space Coords
                Size = new Size(92, 142), //Space size
                IsPurchasable = true
            };
            //Go To Jail
            gameBoard[30] = new BoardSpace
            {
                Name = "Go To Jail",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(1116, 12), // Space Coords
                Size = new Size(142, 142), //Space size
                IsPurchasable = false
            };
            //Pacific Avenue
            gameBoard[31] = new BoardSpace
            {
                Name = "Pacific Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.DarkGreen),
                Price = 300,
                RentCost = 26,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 150,
                Position = new Point(1115, 152), // Space Coords
                Size = new Size(142, 91), //Space size
                IsPurchasable = true
            };
            //North Carolina Avenue
            gameBoard[32] = new BoardSpace
            {
                Name = "North Carolina Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.DarkGreen),
                Price = 300,
                RentCost = 26,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 150,
                Position = new Point(1115, 240), // Space Coords
                Size = new Size(142, 90), //Space size
                IsPurchasable = true
            };
            //Community Chest space 3
            gameBoard[33] = new BoardSpace
            {
                Name = "Community Chest space",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(1115, 329), // Space Coords
                Size = new Size(142, 90), //Space size
                IsPurchasable = false
            };
            //Pennsylvania Avenue
            gameBoard[34] = new BoardSpace
            {
                Name = "Pennsylvania Avenue",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.DarkGreen),
                Price = 320,
                RentCost = 28,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 160,
                Position = new Point(1116, 416), // Space Coords
                Size = new Size(142, 90), //Space size
                IsPurchasable = true
            };
            //Short Line
            gameBoard[35] = new BoardSpace
            {
                Name = "Short Line",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Railroad),
                Price = 200,
                RailRoadRentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 100,
                Position = new Point(1116, 506), // Space Coords
                Size = new Size(142, 90), //Space size
                IsPurchasable = true
            };
            //Chance space 3
            gameBoard[36] = new BoardSpace
            {
                Name = "Chance space",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(1114, 593), // Space Coords
                Size = new Size(142, 90), //Space size
                IsPurchasable = false
            };
            //Park Place
            gameBoard[37] = new BoardSpace
            {
                Name = "Park Place",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.DarkBlue),
                Price = 350,
                RentCost = 35,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 175,
                Position = new Point(1116, 680), // Space Coords
                Size = new Size(142, 90), //Space size
                IsPurchasable = true
            };
            //Luxuary Tax
            gameBoard[38] = new BoardSpace
            {
                Name = "Luxuary Tax",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.Special),
                Price = 0,
                RentCost = 0,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 0,
                Position = new Point(1115, 770), // Space Coords
                Size = new Size(142, 90), //Space size
                IsPurchasable = false
            };
            //BoardWalk
            gameBoard[39] = new BoardSpace
            {
                Name = "BoardWalk",
                Color = ColorMapper.GetColorForCategory(PropertyCategory.DarkBlue),
                Price = 400,
                RentCost = 50,
                IsOwned = false,
                OwnerIndex = -1,
                IsMortgaged = false,
                MortgageValue = 200,
                Position = new Point(1115, 857), // Space Coords
                Size = new Size(142, 90), //Space size
                IsPurchasable = true
            };
        }

        //New Move Player Icons functions to drag and move each piece manually
        // 'e' is used to quickly create these playe dragging functions, e just means event for the most part
        private void MovePlayerIcons()
        {
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox && pictureBox.Tag != null)
                {
                    // Attach event handlers for mouse events
                    pictureBox.MouseDown += PictureBox_MouseDown;
                    pictureBox.MouseMove += PictureBox_MouseMove;
                    pictureBox.MouseUp += PictureBox_MouseUp;
                }
            }
        }
        // Creates variables to check the previous location before dragging a player icon, and checks if it is being dragged
        private Point lastLocation;
        private bool isDragging = false;

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastLocation = e.Location;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (isDragging && pictureBox != null)
            {
                // Calculates the new location based on the mouse movement
                int newX = pictureBox.Left + e.X - lastLocation.X;
                int newY = pictureBox.Top + e.Y - lastLocation.Y;

                // Ensures the new location where the player drags an icon stays within the boundaries of the form
                newX = Math.Max(0, Math.Min(newX, this.ClientSize.Width - pictureBox.Width));
                newY = Math.Max(0, Math.Min(newY, this.ClientSize.Height - pictureBox.Height));

                // Updates the just dragged PictureBox location
                pictureBox.Left = newX;
                pictureBox.Top = newY;
            }
        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }
        // Function used to update the current player's position on the board after rolling
        private void MovePlayer(Player player, int totalRoll)
        {
            // Finds the PictureBox corresponding to the current player's icon
            foreach (Control control in Controls)
            {
                if (control is PictureBox pictureBox && pictureBox.Tag is Player pictureBoxPlayer && pictureBoxPlayer == player)
                {
                    // Calculates the new position based on the total roll and the player's current position
                    int currentPositionIndex = player.Position;
                    int newPositionIndex = (currentPositionIndex + totalRoll) % gameBoard.Length;

                    // Update the player's position variable based on where they should currently be
                    player.Position = newPositionIndex;

                    // Checks the board space the player landed on
                    CheckBoardSpace(player);

                    // Exit the function after moving the player icon
                    return;
                }
            }
        }
        // Method used to update the utility rent during chance and community chest
        private int CalculateUtilityRentPrice(Player owner, int roll)
        {
            // Calculate the rent price for the utility based on the total roll of the dice times 10
            // as the card intends
            int rentPrice = 10 * roll;

            // If the owner has both utilities, double the rent
            if (owner.OwnsBothUtilities == true)
            {
                rentPrice *= 2;
            }

            return rentPrice;
        }
        // Method used to try and locate the nearest utility and move the player towards it
        private void AdvanceToNearestUtility(Player currentPlayer)
        {
            int currentPosition = currentPlayer.Position;

            // Define the positions of the utility spaces on the board
            int[] utilityPositions = { 12, 28 };

            // Calculate the distance to each utility space from the current position
            Dictionary<int, int> distancesToUtilities = new Dictionary<int, int>();
            foreach (int utilityPosition in utilityPositions)
            {
                int distance = (utilityPosition - currentPosition + 40) % 40;
                distancesToUtilities[utilityPosition] = distance;
            }

            // Finds the nearest utility space
            int nearestUtilityPosition = distancesToUtilities.OrderBy(kv => kv.Value).First().Key;

            // Moves the player to the nearest utility space
            currentPlayer.Position = nearestUtilityPosition;

            // Checks if the nearest utility is owned by another player
            BoardSpace nearestUtility = gameBoard[nearestUtilityPosition];
            if (nearestUtility.IsOwned && nearestUtility.OwnerIndex != currentPlayer.PlayerIndex)
            {
                // Get the owner of the utility
                Player owner = players[nearestUtility.OwnerIndex];

                // Calculates the rent price of the utility
                int rentPrice = CalculateUtilityRentPrice(owner, 0); // Pass 0 for the roll as it's not relevant

                // Pay the chance/com chest based rent to the owner
                PayRent(currentPlayer, owner, rentPrice);
            }
        }
        // Anything said in the CalcUtilityPrice and AdvanceToNearestUtility can be said here
        private int CalculateRailRoadRentPrice( Player owner)
        {
            // Define the rent prices based on the number of railroad spaces owned by the same player
            int[] rentPrices = { 25, 50, 100, 200 };

            // Get the number of railroads owned by the player
            int numberOfRailroadsOwned = owner.NumberOfRailroadsOwned;

            // Return the corresponding rent price based on the count
            // Subtract 1 to not go out of bounds
            return rentPrices[numberOfRailroadsOwned - 1]; 
        }

        private void AdvanceToNearestRailroad(Player currentPlayer)
        {
            int currentPosition = currentPlayer.Position;

            // Define the positions of the railroad spaces on the board
            int[] railroadPositions = { 5, 15, 25, 35 };

            // Calculates the distance to each railroad space from the current position
            Dictionary<int, int> distancesToRailroads = new Dictionary<int, int>();
            foreach (int railroadPosition in railroadPositions)
            {
                int distance = (railroadPosition - currentPosition + 40) % 40;
                distancesToRailroads[railroadPosition] = distance;
            }

            // Finds the nearest railroad space
            int nearestRailroadPosition = distancesToRailroads.OrderBy(kv => kv.Value).First().Key;

            // Moves the player to the nearest railroad space
            currentPlayer.Position = nearestRailroadPosition;

            // Checks if the nearest railroad is owned by another player
            BoardSpace nearestRailroad = gameBoard[nearestRailroadPosition];
            if (nearestRailroad.IsOwned && nearestRailroad.OwnerIndex != currentPlayer.PlayerIndex)
            {
                // Gets the owner of the railroad
                Player owner = players[nearestRailroad.OwnerIndex];

                // Calculates the chance/Com Chest empowered rent price of the railroad
                int rentPrice = CalculateRailRoadRentPrice(owner);

                // Pay rent to the owner of the railroad
                PayRent(currentPlayer, owner, rentPrice);
            }
        }
        //Used to pay each player an amount from the current players total cash
        private void PayEachPlayer(Player currentPlayer, int amount)
        {
            int numPlayers = players.Count - 1; // Exclude the current player
            int totalPayment = amount * numPlayers;

            // Deduct total payment from the current player's total cash
            currentPlayer.TotalCash -= totalPayment;

            // Adds totalPayment to each other player's total cash
            foreach (Player player in players)
            {
                if (player != currentPlayer)
                {
                    player.TotalCash += amount;
                    MessageBox.Show($"You paid $50 to Player {player}.", "Money Lost", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Update cash labels for all players
            foreach (Player player in players)
            {
                UpdateCashLabel(player, 0);
            }
        }

        // chance function with the many possible chance effects 
        private void ChanceTimeChanceSpace(Player currentPlayer)
        {
            Random random = new Random();
            int actionNumber = random.Next(1, 15); // Adjusted to include 14 as upper bound

            switch (actionNumber)
            {
                case 1:
                    currentPlayer.Position = 39; // Move player to Boardwalk
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to BoardWalk!", "Chance Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    currentPlayer.Position = 0; // Move player to Go
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to GO!", "Chance Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 3:
                    currentPlayer.Position = 24; // Move player to Illinois Avenue
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to Illinois!", "Chance Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 4:
                    currentPlayer.Position = 11; // Move player to St. Charles Place
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to St. Charles Place!", "Chance Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 5:
                    AdvanceToNearestRailroad(currentPlayer);
                    break;
                case 6:
                    AdvanceToNearestUtility(currentPlayer);
                    break;
                case 7:
                    currentPlayer.TotalCash += 50; // Pay player $50
                    UpdateCashLabel(currentPlayer, 50);
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've received $50!", "Chance Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 8:
                    currentPlayer.Position -= 3; // Move player back 3 spaces
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved back by three spaces", "Chance Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Check the board space the player landed on
                    CheckBoardSpace(currentPlayer);
                    break;
                case 9:
                    currentPlayer.InJail = true;
                    currentPlayer.PaidJailFee = false;
                    currentPlayer.Position = 10; // Move player to Jail
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you are in jail!", "In Jail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 10:
                    currentPlayer.TotalCash -= 15; // Pay a $15 fine
                    UpdateCashLabel(currentPlayer, -15);
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been fined $15 for jaywalking!", "Money Lost", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 11:
                    currentPlayer.Position = 5; // Move player to Reading Railroad
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to Reading Railroad!", "Chance Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 12:
                    PayEachPlayer(currentPlayer, 50); // Pay each player $50
                    break;
                case 13:
                    currentPlayer.TotalCash += 150; // Receive $150
                    UpdateCashLabel(currentPlayer, 150);
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've received $150!", "Money Gained", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 14:
                    currentPlayer.GoojfCard = true; // Receive a 'Get Out of Jail Free' card
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've received a 'Get Out of Jail Free' card!", "Chance Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    // Do nothing
                    break;
            }
        }


        //comChest function
        private void CommunityChestChanceTime(Player currentPlayer)
        {
            Random random = new Random();
            int actionNumber = random.Next(1, 15); // Adjusted to include 14 as upper bound

            switch (actionNumber)
            {
                case 1:
                    currentPlayer.Position = 37; // Move player to ParkPlace
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to Park Place!", "Chest Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 2:
                    currentPlayer.Position = 0; // Move player to Go
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to Go!", "Chest Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 3:
                    currentPlayer.Position = 16; // Move player to St. James Avenue
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to St. James Avenue!", "Chest Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 4:
                    currentPlayer.Position = 29; // Move player to Marvin Gardens
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to Marvin Gardens!", "Chest Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 5:
                    AdvanceToNearestRailroad(currentPlayer);
                    break;
                case 6:
                    AdvanceToNearestUtility(currentPlayer);
                    break;
                case 7:
                    currentPlayer.TotalCash += 75; // Pay player $75
                    UpdateCashLabel(currentPlayer, 75);
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've received $75!", "Money Gained", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 8:
                    currentPlayer.Position += 3; // Move player forward 3 spaces
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved forward by three spaces", "Chest Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Check the board space the player landed on
                    CheckBoardSpace(currentPlayer);
                    break;
                case 9:
                    currentPlayer.InJail = true;
                    currentPlayer.PaidJailFee = false;
                    currentPlayer.Position = 10; // Move player to Jail
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you are in jail!", "In Jail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 10:
                    currentPlayer.TotalCash -= 45; // Player lost $45
                    UpdateCashLabel(currentPlayer, -45);
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've lost $45 in a bad stock exchange!", "Money Lost", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 11:
                    currentPlayer.Position = 25; // Move player to B. & O. RailRoad
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've been moved to B.& O. RailRoad!", "Chest Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 12:
                    PayEachPlayer(currentPlayer, 50); // Pay each player $50
                    break;
                case 13:
                    currentPlayer.TotalCash += 100; // Receive $100
                    UpdateCashLabel(currentPlayer, 100);
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've received $100!", "Money Gained", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 14:
                    currentPlayer.GoojfCard = true; // Receive a 'Get Out of Jail Free' card
                    MessageBox.Show($"Player {currentPlayer.PlayerIndex + 1}, you've received a 'Get Out of Jail Free' card!", "Chest Action", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                default:
                    // Do nothing
                    break;
            }
        }

        //used to only check if a properly is a utility or railroad space for rent
        private bool IsUtilityOrRailroad(BoardSpace space)
        {
            // Check if the space color corresponds to a railroad or utility
            return space.Color == Color.Black || space.Color == Color.Gray;
        }
        // Method used to check the board space the player landed on
        private BoardSpace CheckBoardSpace(Player currentPlayer)
        {
            // Get the current player's position on the game board
            int playerPosition = currentPlayer.Position;

            // Check if the player's position is within the bounds of the game board
 
            if (playerPosition >= 0 && playerPosition < gameBoard.Length)
            {
                // Retrieve the corresponding property from the game board based on the player's position
                BoardSpace currentSpace = gameBoard[playerPosition];

                // Check if the current space is not a utility or railroad and is owned by another player
                if (!IsUtilityOrRailroad(currentSpace) && currentSpace.IsOwned && currentSpace.OwnerIndex != currentPlayer.PlayerIndex)
                {
                    // Get the owner of the property
                    Player owner = players[currentSpace.OwnerIndex];

                    // Pay rent to the owner by passing current player and space to PayRent
                    PayRent(currentPlayer, owner, currentSpace.RentCost);
                }

                // Save the player's previous position before updating it
                int previousPosition = currentPlayer.Position;

                // Used to get the index/location for where the current player will be then sets 
                // currentPlayer.Position to it 
                int newPositionIndex = (currentPlayer.Position + TotalRoll) % gameBoard.Length;
                currentPlayer.Position = newPositionIndex;

                // Check if the player has passed "Go" after updating their position
                if (newPositionIndex < previousPosition)
                {
                    // Award the player $200 for passing "Go"
                    currentPlayer.TotalCash += 200;
                    // Update UI to reflect the change in player's cash
                    UpdateCashLabel(currentPlayer, 200);
                    currentPlayer.PassedGo = true;
                }
                // Income tax space 
                else if (playerPosition == 4)
                {
                    // Calculate tax amount
                    double taxAmount;
                    if (currentPlayer.TotalCash >= 2000)
                    {
                        taxAmount = 200;
                    }
                    else
                    {
                        taxAmount = currentPlayer.TotalCash * 0.1;
                    }

                    // Round the 10% tax amount to the nearest value
                    int roundedTaxAmount = (int)Math.Round(taxAmount);

                    // Deduct the tax from the player's total cash
                    currentPlayer.TotalCash -= roundedTaxAmount;

                    // Update the cash label
                    UpdateCashLabel(currentPlayer, roundedTaxAmount);
                }

                //chance space code if a player lands on a chance space run the function called ChanceTimeChanceSpace
                else if (playerPosition == 7)
                {
                    ChanceTimeChanceSpace(currentPlayer);
                    
                }
                //community chest space code if a player lands on a comm chest space run the function CommunityChestChanceTime
                else if (playerPosition == 2)
                {
                    CommunityChestChanceTime(currentPlayer);
                    
                }
                // Add similar conditions for the other chance and community chest positions
                else if (playerPosition == 22)
                {
                    ChanceTimeChanceSpace(currentPlayer);
                   
                }
                else if (playerPosition == 17)
                {
                    CommunityChestChanceTime(currentPlayer);
                    
                }
                else if (playerPosition == 36)
                {
                    ChanceTimeChanceSpace(currentPlayer);
                    
                }
                else if (playerPosition == 33)
                {
                    CommunityChestChanceTime(currentPlayer);
                    
                }
                // Free Parking space 
                else if (playerPosition == 20)
                {
                    // Player obtains freeParking cash
                    currentPlayer.TotalCash += freeParkingPool;

                    // Update the cash label
                    UpdateCashLabel(currentPlayer, freeParkingPool);

                    // Reset the Free Parking pool to zero
                    freeParkingPool = 0;
                }
                // Go to Jail space
                else if (playerPosition == 30)
                {
                    // Logic for when the player lands on "Go to Jail" space
                    // For example, move the player to the Jail space
                    currentPlayer.InJail = true;
                    currentPlayer.PaidJailFee = false;
                    // RolledDoubles = false;
                    currentPlayer.Position = 10; // Assuming Jail space is at index 10
                    MessageBox.Show($"You are in jail. You've broken the law player {currentPlayerIndex}", "In Jail", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                // Luxury Tax space
                else if (playerPosition == 38)
                {
                    int LuxuryTaxAmount = 75;
                    currentPlayer.TotalCash -= LuxuryTaxAmount;
                    freeParkingPool += 75;
                    UpdateCashLabel(currentPlayer, LuxuryTaxAmount);
                }
                currentPlayer.PassedGo = false;
                return currentSpace;
            }
            else
            {
                // Player is out of bounds of the game board
                return null;
            }
        }

        // Method to remove all player controls after they lose
        public void RemovePlayerControls(Player player)
        {
            // Finds the PictureBox and Label associated with a player that goes bankrupt
            PictureBox playerPictureBox = Controls.Find($"Player{player.PlayerIndex + 1}Box", true).FirstOrDefault() as PictureBox;
            Label playerCashLabel = Controls.Find($"Player{player.PlayerIndex + 1}CashLabel", true).FirstOrDefault() as Label;

            // Removes their PictureBox
            if (playerPictureBox != null)
            {
                Controls.Remove(playerPictureBox);
                playerPictureBox.Dispose(); // Dispose PictureBox = release resources
            }

            // Removes their Label
            if (playerCashLabel != null)
            {
                Controls.Remove(playerCashLabel);
                playerCashLabel.Dispose(); // Dispose Label = release resources
            }
        }
        // Method used to check if any players have gone Bankrupt
        public void CheckPlayerBankruptcies()
        {
            // Creates a new list to store all players that go bankrupt
            List<Player> bankruptPlayers = new List<Player>();

            foreach (var player in players)
            {
                // Checks each player's total cash amount and checks if they need to be removed due to going BankRupt
                if (player.TotalCash <= 0)
                {
                    MessageBox.Show($"Player {player.PlayerIndex + 1} has gone bankrupt!", "Bankruptcy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Add the bankrupt player to the list of players to be removed
                    bankruptPlayers.Add(player);
                    RemovePlayerControls(player);
                }
            }

            // Remove the bankrupt players from the list of players
            foreach (var bankruptPlayer in bankruptPlayers)
            {
                players.Remove(bankruptPlayer);
            }
        }
        // Method used to end the game and allow the player to restart or close the program
        private void EndGame()
        {
            // Declares and makes a new int for totalTurns
            int totalTurns = 0;

            // Increment totalTurns each time EndGame is called in endturn
            totalTurns++;

            // Check if it's time to double property rent by checking if 20 player turns aka 5 total turns has passed
            if (totalTurns % 20 == 0)
            {
                // Doubles property rent for all properties
                DoublePropertyRent();

                // Displays a popup indicating that all rent prices have been doubled
                MessageBox.Show("All property rents have been doubled due to inflation!", "Rent Doubled", MessageBoxButtons.OK);
            }

            // checks if all players have clicked the end turn button 20 times to initiate
            // the alternate win option that checks which player has the most cash
            if (totalTurns >= 80)
            {
                Dictionary<Player, int> cashAmounts = new Dictionary<Player, int>();

                // Gathers the total cash amounts for each player
                foreach (Player player in players)
                {
                    cashAmounts[player] = player.TotalCash;
                }

                // Sorts each player by total cash in descending order
                var sortedPlayers = cashAmounts.OrderByDescending(kv => kv.Value).ToList();

                // Checks to see if any player owns the most money
                if (sortedPlayers.Count == 1 || sortedPlayers[0].Value != sortedPlayers[1].Value)
                {
                    Player winner = sortedPlayers[0].Key;
                    MessageBox.Show($"Player {winner.PlayerIndex + 1} wins with ${winner.TotalCash} left!", "Game Over", MessageBoxButtons.OK);
                }
                else
                {
                    // If 2 or more players have = cash, resolves the tie by rolling dice
                    List<Player> tiedPlayers = sortedPlayers.Where(kv => kv.Value == sortedPlayers[0].Value).Select(kv => kv.Key).ToList();
                    Dictionary<Player, int> rollResults = new Dictionary<Player, int>();

                    // Rolls a dice for each tied player
                    Random random = new Random();
                    foreach (Player player in tiedPlayers)
                    {
                        // Rolls a dice once for each player and stores it to then be checked
                        int roll = random.Next(1, 7); 
                        rollResults[player] = roll;
                    }

                    // Sort tied players by their roll results in descending order
                    var sortedRollResults = rollResults.OrderByDescending(kv => kv.Value).ToList();

                    // Declare the winner based on the highest roll between the players that tied
                    Player winner = sortedRollResults[0].Key;
                    MessageBox.Show($"Player {winner.PlayerIndex + 1} wins with a roll of {sortedRollResults[0].Value}!", "Game Over", MessageBoxButtons.OK);
                }

                // Ask if the players want to play again
                DialogResult result = MessageBox.Show("Would you like to play again?", "Play Again?", MessageBoxButtons.YesNo);

                // Checks if you answered yes to play again or no to stop playing
                if (result == DialogResult.Yes)
                {
                    // Restart the game by reopening Form1
                    Form1 newGameForm = new Form1();
                    newGameForm.Show();
                    // Closes Cyberopoly form
                    Close();
                }
                else
                {
                    // Close the program
                    Application.Exit();
                }
            }
        }
        // Function to double property rent for all properties
        private void DoublePropertyRent()
        {
            foreach (BoardSpace property in gameBoard)
            {
                // Checks if the space is a purchasable property
                if (property.IsPurchasable)
                {
                    // Doubles the rent cost for the space
                    property.DoubleRentCost();
                }
            }
        }
            //accidentally made, do not remove it will mess form up
            private void CyberopolyGameBoardForm_Load(object sender, EventArgs e)
        {

        }
        //player 4 picture box //accidentally made, do not remove it will mess form up
        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        // Price goes unused, but don't worry about this I forgot to fix this due to time crunches
        // this would of made the code a lot more neat and less ugly if properly implemented
        public void UpdateCashLabel(Player currentPlayer, double price)
        {
            // Find the label associated with the current player's cash
            Control[] controls = Controls.Find($"Player{currentPlayerIndex + 1}CashLabel", true);

            // Checks if the current player's label is found
            if (controls.Length > 0)
            {
                Label cashLabel = controls[0] as Label;
                if (cashLabel != null)
                {
                    cashLabel.Text = $"Player {currentPlayer.PlayerIndex + 1}: ${currentPlayer.TotalCash}";
                } 
            }
        }
        private void UpdatePropertyOwnership(BoardSpace property, int currentPlayerIndex)
        {
            try
            {
                // Check if the purchased property is one of the railroad properties
                if (property.Position.X % 10 == 5) // Check if the property index ends with 5 (railroad properties)
                {
                    // Get the current player
                    Player currentPlayer = players[currentPlayerIndex];

                    // Update the rent for all railroad properties based on the current player's ownership
                    foreach (BoardSpace boardSpace in gameBoard)
                    {
                        if (boardSpace.Color == ColorMapper.GetColorForCategory(PropertyCategory.Railroad))
                        {
                            boardSpace.UpdateRailroadRent(currentPlayer.NumberOfRailroadsOwned);
                        }
                    }
                }

                // Check if the purchased property is a utility
                if (property.Color == ColorMapper.GetColorForCategory(PropertyCategory.Utility))
                {
                    // Get the current player
                    Player currentPlayer = players[currentPlayerIndex];

                    // Update the rent for the utility based on the current player's ownership
                    bool ownsBothUtilities = currentPlayer.OwnedProperties.Count(p => p.Color == ColorMapper.GetColorForCategory(PropertyCategory.Utility)) == 2;
                    property.UpdateUtilityRent(ownsBothUtilities, TotalRoll);
                }

                // Update the property ownership based on the current player
                property.OwnerIndex = currentPlayerIndex;

                // Update property ownership on the game board
                // Add the property to the player's owned properties list
                players[currentPlayerIndex].AddOwnedProperty(property);
            }
            catch (Exception ex)
            {
                // Handle the error
                MessageBox.Show($"An error occurred while updating property ownership: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BuyProperty(BoardSpace property, int currentPlayerIndex)
        {
            // Buy the property
            property.IsOwned = true;
            property.OwnerIndex = currentPlayerIndex;

            // Update property ownership on the game board
            UpdatePropertyOwnership(property, currentPlayerIndex);

            // Add the property to the player's owned properties list
            players[currentPlayerIndex].AddOwnedProperty(property);
        }

        private void BuyPropertyButton_Click(object sender, EventArgs e)
        {
            // Get the current player
            Player currentPlayer = players[currentPlayerIndex];

            // Get the current property
            BoardSpace currentProperty = CheckBoardSpace(currentPlayer); 

            // Check if the property is purchasable and not already owned
            if (currentProperty.IsPurchasable && !currentProperty.IsOwned)
            {
                // Check if the player can afford the property
                if (currentPlayer.TotalCash >= currentProperty.Price)
                {
                    // Debug: Output the property being bought
                    MessageBox.Show($"Player {currentPlayerIndex + 1} is buying property {currentProperty.Name} for ${currentProperty.Price}");

                    // Buy the property
                    BuyProperty(currentProperty, currentPlayerIndex);

                    // Deduct the property price from the player's cash
                    currentPlayer.TotalCash -= currentProperty.Price;

                    // Update the player's cash label
                    UpdateCashLabel(currentPlayer, currentProperty.Price);

                    // Display a message box indicating the current player's total cash after the purchase
                    MessageBox.Show($"Player {currentPlayerIndex + 1} now has ${currentPlayer.TotalCash}", "Total Cash", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("You don't have enough cash to purchase this property.", "Insufficient Funds", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("This property cannot be purchased or is already owned.", "Invalid Property", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void HandleJail(Player currentPlayer)
        {
            if (currentPlayer.InJail)
            {
                // Increment the number of turns spent in jail
                currentPlayer.TurnsInJail++;

                // Check if the player has spent 3 turns in jail
                if (currentPlayer.TurnsInJail == 3)
                {
                    // Check if the player has a "Get Out of Jail Free" card
                    if (currentPlayer.GoojfCard)
                    {
                        // Use the "Get Out of Jail Free" card
                        currentPlayer.GoojfCard = false; // Player uses the card
                        currentPlayer.InJail = false; // Player is released from jail

                        // Inform the player that they used the card to get out of jail
                        MessageBox.Show("You used a 'Get Out of Jail Free' card to get out of jail.", "Jail Time Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Force the player to pay $50
                        currentPlayer.TotalCash -= 50;

                        // Add the fee to the Free Parking pool
                        freeParkingPool += 50;

                        // Reset the turns spent in jail
                        currentPlayer.TurnsInJail = 0;

                        // Inform the player about paying the fee
                        MessageBox.Show("You've spent 3 turns in jail. Pay $50 to get out.", "Jail Time Over", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Update UI to reflect the deduction of $50
                        UpdateCashLabel(currentPlayer, 50);
                    }
                }
                else
                {
                    // Inform the player about their current situation
                    MessageBox.Show($"You are in jail. You've spent {currentPlayer.TurnsInJail} turns in jail.", "In Jail", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        //All rent code for updating the cash labels for rent, and the rent function to call below
        public void UpdateCashLabelRent(Player payer, Player recipient, int rentAmount)
        {
            try
            {
                // Deduct the rent amount from the payer's total cash
                payer.TotalCash -= rentAmount;

                // Add the rent amount to the recipient's total cash
                recipient.TotalCash += rentAmount;

                // Output debug 
                MessageBox.Show($"Player {payer.PlayerIndex + 1} paid ${rentAmount} in rent to Player {recipient.PlayerIndex + 1}");

                // Find the label associated with the payer's cash
                Control[] payerControls = Controls.Find($"Player{payer.PlayerIndex + 1}CashLabel", true);

                // Check if the label for the payer is found
                if (payerControls.Length > 0)
                {
                    Label payerCashLabel = payerControls[0] as Label;
                    if (payerCashLabel != null)
                    {
                        payerCashLabel.Text = $"${payer.TotalCash}";
                    }
                }

                // Find the label associated with the recipient's cash
                Control[] recipientControls = Controls.Find($"Player{recipient.PlayerIndex + 1}CashLabel", true);

                // Check if the label for the recipient is found
                if (recipientControls.Length > 0)
                {
                    Label recipientCashLabel = recipientControls[0] as Label;
                    if (recipientCashLabel != null)
                    {
                        recipientCashLabel.Text = $"${recipient.TotalCash}";
                    }
                }

                // Inform the players about the rent payment
                MessageBox.Show($"Player {payer.PlayerIndex + 1} paid ${rentAmount} in rent to Player {recipient.PlayerIndex + 1}.", "Rent Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Handle the error
                MessageBox.Show($"An error occurred while updating cash labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PayRent(Player payer, Player recipient, int rentAmount)
        {
            try
            {
                // Deduct the rent amount from the payer's total cash
                payer.TotalCash -= rentAmount;

                // Add the rent amount to the recipient's total cash
                recipient.TotalCash += rentAmount;

                // Find the label associated with the payer's cash
                Control[] payerControls = Controls.Find($"Player{payer.PlayerIndex + 1}CashLabel", true);

                // Check if the label for the payer is found
                if (payerControls.Length > 0)
                {
                    Label payerCashLabel = payerControls[0] as Label;
                    if (payerCashLabel != null)
                    {
                        payerCashLabel.Text = $"${payer.TotalCash}";
                    }
                }

                // Find the label associated with the recipient's cash
                Control[] recipientControls = Controls.Find($"Player{recipient.PlayerIndex + 1}CashLabel", true);

                // Check if the label for the recipient is found
                if (recipientControls.Length > 0)
                {
                    Label recipientCashLabel = recipientControls[0] as Label;
                    if (recipientCashLabel != null)
                    {
                        recipientCashLabel.Text = $"${recipient.TotalCash}";
                    }
                }

                // Inform the players about the rent payment
                MessageBox.Show($"Player {payer.PlayerIndex + 1} paid ${rentAmount} in rent to Player {recipient.PlayerIndex + 1}.", "Rent Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                // Handle the error
                MessageBox.Show($"An error occurred while updating cash labels: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void RollDiceButton_Click(object sender, EventArgs e)
        {
            // Disable RollDiceButton
            RollDiceButton.Enabled = false;
            // Update the position of the current player based on the total roll
            Player currentPlayer = players[currentPlayerIndex];

            //BoardSpace currentProperty = gameBoard[]

            // Handle jail-related logic
            HandleJail(currentPlayer);

            // Create PictureBox for each dice
            PictureBox diceBox1 = new PictureBox();
            PictureBox diceBox2 = new PictureBox();

            // Set properties for the dice boxes
            diceBox1.BackColor = Color.Transparent;
            diceBox2.BackColor = Color.Transparent;
            diceBox1.Size = new Size(400, 400); // Set size to 400x400
            diceBox2.Size = new Size(400, 400); // Set size to 400x400
            diceBox1.Location = new Point(300, 400);
            diceBox2.Location = new Point(750, 400);

            // Generate random numbers for each dice roll
            Random random = new Random();
            int roll1 = random.Next(1, 7); // Random number between 1 and 6
            int roll2 = random.Next(1, 7); // Random number between 1 and 6

            // Load and display the corresponding dice face images
            diceBox1.Image = diceFaces[roll1 - 1]; // Subtract 1 because array indexes start from 0
            diceBox2.Image = diceFaces[roll2 - 1]; // Subtract 1 because array indexes start from 0

            // Add the dice boxes to the form's controls
            Controls.Add(diceBox1);
            Controls.Add(diceBox2);

            // Ensure that the dice boxes are brought to the front
            diceBox1.BringToFront();
            diceBox2.BringToFront();

            // Await for 3 seconds
            await Task.Delay(3000);

            // Hide the dice boxes after 3 seconds
            Controls.Remove(diceBox1);
            Controls.Remove(diceBox2);

            // Calculate the total roll
            TotalRoll = roll1 + roll2;

            // Used to get the index/location for where the current player will be then sets 
            // currentPlayer.Position to it 
            int newPositionIndex = (currentPlayer.Position + TotalRoll) % gameBoard.Length;
            // currentPlayer.Position = newPositionIndex;

            // Create the instance of boardSpace to properly output the name of the board space later 
            // Through a message box also didn't want to move the current structure due to time crunch
            BoardSpace boardSpace = gameBoard[newPositionIndex];
            // Update the player's total roll amount for the turn
            totalRollAmounts[currentPlayerIndex] += TotalRoll;

            // Check if the player rolled doubles
            if (roll1 == roll2 && newPositionIndex != 30) 
            {
                // Increment the consecutive doubles count for the player
                currentPlayer.ConsecutiveDoublesCount++;

                // Check if the player rolled doubles 3 times in a turn
                if (currentPlayer.ConsecutiveDoublesCount == 3)
                {
                    // Send the player to jail
                    currentPlayer.InJail = true;
                    currentPlayer.PaidJailFee = false;
                    // Reset the consecutive doubles count
                    currentPlayer.ConsecutiveDoublesCount = 0;

                    // Show a message box informing the player they are in jail for rolling too many doubles
                    MessageBox.Show("You rolled doubles 3 times in a row and are sent to jail!", "Rolled Doubles 3 Times", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // enable EndTurnButton and disable RollDiceButton
                    RollDiceButton.Enabled = false;
                    EndTurnButton.Enabled = true;

                    // Exit the function
                    return;
                }

                // Re-enable RollDiceButton and disable EndTurnButton
                RollDiceButton.Enabled = true;
                EndTurnButton.Enabled = false;

                // Show popup window informing the player that they rolled doubles
                MessageBox.Show("You rolled doubles! Roll again.", "Doubles Rolled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Reset the consecutive doubles count if the player didn't roll doubles
                currentPlayer.ConsecutiveDoublesCount = 0;

                // Disable RollDiceButton and enable EndTurnButton
                RollDiceButton.Enabled = false;
                EndTurnButton.Enabled = true;
            }
            // Checks if the player is in jail and skips movement if they are
            if (currentPlayer.InJail)
            {
                MessageBox.Show($"Player {currentPlayerIndex + 1} cannot move spaces while in jail");
            }
            else
            {
                // Move the player
                MovePlayer(players[currentPlayerIndex], TotalRoll);

                // Check the board space the player landed on
                CheckBoardSpace(currentPlayer);

                // Lets the player know where they currently are on the board
                MessageBox.Show($"Player {currentPlayerIndex + 1} moved to space {newPositionIndex}: {boardSpace.Name}");
            }
        }

        private void EndTurnButton_Click(object sender, EventArgs e)
        {
            // Checks if a player went bankrupt
            CheckPlayerBankruptcies();

            // Checks if the game should end
            EndGame(); 

            // Move to the next player's turn

            currentPlayerIndex++;

            // Resets to the first player if currentPlayerIndex exceeds the maximum index
            if (currentPlayerIndex >= players.Count)
            {
                currentPlayerIndex = 0;
            }

            // Used to change the background color based on the current player's turn
            switch (currentPlayerIndex)
            {
                case 0:
                    this.BackColor = Color.RoyalBlue;
                    break;
                case 1:
                    this.BackColor = Color.Red;
                    break;
                case 2:
                    this.BackColor = Color.Green;
                    break;
                case 3:
                    this.BackColor = Color.Yellow;
                    break;
                default:
                    this.BackColor = Color.RoyalBlue; // Resets to default color
                    break;
            }

            // Displays a message indicating whose turn it is
            MessageBox.Show($"It's now Player {currentPlayerIndex + 1}'s turn.");

            // Enables RollDiceButton and disable EndTurnButton
            RollDiceButton.Enabled = true;
            EndTurnButton.Enabled = false;
        }

        private void PayJailFee_Click(object sender, EventArgs e)
        {
            Player currentPlayer = players[currentPlayerIndex];

            // Checks if the current player is in jail and has not yet paid the fee
            if (currentPlayer.InJail && !currentPlayer.PaidJailFee)
            {
                // Checks if the player has a "Get Out of Jail Free" card
                if (currentPlayer.GoojfCard)
                {
                    // Use the "Get Out of Jail Free" card
                    currentPlayer.GoojfCard = false; // Player uses the Get out of jail free card
                    currentPlayer.InJail = false; // Player is released from jail

                    // Informs the player that they used the card to get out of jail
                    MessageBox.Show("You used a 'Get Out of Jail Free' card to get out of jail.", "Jail Fee Paid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Deducts the fee from the player's total cash when paid
                    currentPlayer.TotalCash -= 50;

                    // Adds the fee to the Free Parking pool
                    freeParkingPool += 50;

                    // Resets the turns spent in jail when added to jail
                    currentPlayer.TurnsInJail = 0;

                    // Set paid jail fee to true so the player can exit jail
                    currentPlayer.PaidJailFee = true;
                    // Set in jail to false so they are out of jail
                    currentPlayer.InJail = false;

                    // Updates the player's cash total for who paid the jail fee
                    UpdateCashLabel(currentPlayer, 50);

                    // Lets the player know they paid the jail fee
                    MessageBox.Show("You've paid $50 to get out of jail.", "Jail Fee Paid", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // Informs the player that they cannot pay a jail fee when they are not in jail
                MessageBox.Show("You cannot pay the jail fee at this time.", "Cannot Pay Jail Fee", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void MortgageButton_Click(object sender, EventArgs e)
        {
            int currentIndex = CurrentPlayerIndex;
            Player currentPlayer = players[currentPlayerIndex];

            // Checks if the current player owns any properties
            if (currentPlayer.OwnedProperties.Count > 0)
            {
                // Display a new form to select properties for mortgage called dialog
                // had a different way i wanted to implement this function but just made it a form and forgot to change the name 
                MortgagePropertyDialog dialog = new MortgagePropertyDialog(currentPlayerIndex, players, currentPlayer.OwnedProperties, this);
                dialog.ShowDialog();

                // Shows the dialog box
                DialogResult result = dialog.ShowDialog();

                // Checks if the player selected properties and clicked OK
                if (result == DialogResult.OK)
                {
                    // Get the list of properties selected for mortgage
                    List<BoardSpace> selectedProperties = dialog.SelectedProperties;

                    // Calculates the total mortgage value
                    int totalMortgageValue = selectedProperties.Sum(property => property.MortgageValue);

                    // Adds the total mortgage value to the player's total cash
                    currentPlayer.TotalCash += totalMortgageValue;

                    // Updates the UI to reflect the increase or decrease of their total cash
                    UpdateCashLabel(currentPlayer, totalMortgageValue);

                    // Tells the player about their mortgage transaction
                    MessageBox.Show($"You've mortgaged {selectedProperties.Count} properties for a total of ${totalMortgageValue}.", "Properties Mortgaged", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                // If the player doesn't own a property, lets the player know that they can't mortgage
                MessageBox.Show("You do not own any properties to mortgage.", "No Properties Owned", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void PopulatePropertiesList()
        {
            // Clears any existing items in the checked list box if any exists
            allPropertiesCheckedListBox.Items.Clear();

            // Iterates through all properties in the game board array
            foreach (BoardSpace property in gameBoard)
            {
                // Checks if the property is purchasable so certain spaces aren't shown IE Chance or CommunityChest spaces
                if (property.IsPurchasable)
                {
                    // Creates a string to hold all the property information
                    string propertyInfo = $"Name: {property.Name}, " +
                                          $"Color: {property.Color}, " +
                                          $"Price: ${property.Price}, " +
                                          $"RentCost: ${property.RentCost}, " +
                                          $"IsOwned: {property.IsOwned}, " +
                                          $"OwnerIndex: {property.OwnerIndex + 1}, " +
                                          $"IsMortgaged: {property.IsMortgaged}, " +
                                          $"MortgageValue: ${property.MortgageValue}, ";
                    //+ $"IsPurchasable: {property.IsPurchasable}"
                    // Adds the property information to the checked list box
                    allPropertiesCheckedListBox.Items.Add(propertyInfo);
                }
            }
        }

        private void ListAllProperties_Click(object sender, EventArgs e)
        {
            // Toggles the visibility of the AllPropertiesCheckedListBox when the button is clicked
            allPropertiesCheckedListBox.Visible = !allPropertiesCheckedListBox.Visible;

            // If the properties list is visible, populate it with the property data
            if (allPropertiesCheckedListBox.Visible)
            {
                // Clears any existing items in the checked list box if any exist
                allPropertiesCheckedListBox.Items.Clear();
                PopulatePropertiesList();
            }

        }

    }
}