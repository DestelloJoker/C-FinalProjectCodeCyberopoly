using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MonopolyGuiAndBoard.CyberopolyGameBoardForm;

namespace MonopolyGuiAndBoard
{
    // Called MortgagePropertyDialog because this wasn't supposed to be another form but a different
    // pop up window, found a form was much easier to make and handle with my current knowledge level
    public partial class MortgagePropertyDialog : Form
    {
        private List<BoardSpace> ownedProperties;
        private List<BoardSpace> selectedProperties = new List<BoardSpace>(); 
        private int currentPlayerIndex;
        private List<Player> players;
        private CyberopolyGameBoardForm gameBoardForm;
        public MortgagePropertyDialog(int currentPlayerIndex, List<Player> players, List<BoardSpace> ownedProperties, CyberopolyGameBoardForm gameBoardForm)
        {
            // Initializes the UI data 
            InitializeComponent();
            this.currentPlayerIndex = currentPlayerIndex;
            this.players = players;
            this.ownedProperties = ownedProperties;
            this.gameBoardForm = gameBoardForm;
            PopulateMortgageList(currentPlayerIndex, ownedProperties);
            PopulateUnmortgageList(currentPlayerIndex, ownedProperties);
            
        }
        // Get the selected properties from the Game Board form data
        public List<BoardSpace> SelectedProperties { get { return selectedProperties; } }
        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void UpdatePropertyStatus(BoardSpace property, bool isMortgaged)
        {
            // Updates the mortgage status of the property
            property.IsMortgaged = isMortgaged;
        }
        // Checks for properties by their name
        private BoardSpace FindPropertyByName(string propertyName, List<BoardSpace> properties)
        {
            foreach (BoardSpace property in properties)
            {
                if (property.Name == propertyName)
                {
                    return property;
                }
            }
            // If the property with the given name is not found, return null or throw an exception
            return null; 
        }
        private int CalculateMortgageValue(BoardSpace property)
        {
            // Return the mortgage value of the property
            return property.MortgageValue;
        }


        private void MortgageButton_Click(object sender, EventArgs e)
        {
            // Get the current player using the currentPlayerIndex
            Player currentPlayer = players[currentPlayerIndex];
            int totalMortgageValue = 0;

            // Iterate through all checked items for mortgage
            foreach (string propertyName in MortgageCheckedListBox.CheckedItems)
            {
                // Find the corresponding property(s) checked
                BoardSpace property = FindPropertyByName(propertyName, currentPlayer.OwnedProperties);
                if (property != null)
                {
                    // Calculate mortgage value of all selected properties and add to player's cash
                    int mortgageValue = CalculateMortgageValue(property);
                    totalMortgageValue += mortgageValue;
                    currentPlayer.TotalCash += mortgageValue;

                    // Mark property as mortgaged
                    UpdatePropertyStatus(property, true);
                    // Refresh the unmortgage and mortgage lists
                   
                    PopulateUnmortgageList(currentPlayerIndex, ownedProperties);
                }
            }

            // Update player's cash
            gameBoardForm.UpdateCashLabel(currentPlayer, totalMortgageValue);

            Close();
        }

        // Populate the MortgageCheckedListBox with all owned properties of the current Player
        private void PopulateMortgageList(int currentPlayerIndex, List<BoardSpace> ownedProperties)
        {
            MortgageCheckedListBox.Items.Clear();
            // Get the current player using the currentPlayerIndex
            Player currentPlayer = players[currentPlayerIndex];
            foreach (BoardSpace property in ownedProperties)
            {
                if (property.OwnerIndex == currentPlayer.PlayerIndex && !property.IsMortgaged
                    && !MortgageCheckedListBox.Items.Contains(property.Name))
                {
                    MortgageCheckedListBox.Items.Add(property.Name);
                }
            }
        }

        private void PopulateUnmortgageList(int currentPlayerIndex, List<BoardSpace> ownedProperties)
        {
            UnmortgageCheckedListBox.Items.Clear();
            // Get the current player using the currentPlayerIndex
            Player currentPlayer = players[currentPlayerIndex];
            foreach (BoardSpace property in ownedProperties)
            {
                if (property.OwnerIndex == currentPlayer.PlayerIndex && property.IsMortgaged
                    && !UnmortgageCheckedListBox.Items.Contains(property.Name))
                {
                    UnmortgageCheckedListBox.Items.Add(property.Name);
                }
            }
        }
        private void UnmortgageButton_Click(object sender, EventArgs e)
        {
            // Get the current player using the currentPlayerIndex
            Player currentPlayer = players[currentPlayerIndex];
            int totalUnmortgageValue = 0;

            // Iterate through all checked items
            foreach (string propertyName in UnmortgageCheckedListBox.CheckedItems)
            {
                // Find the corresponding property(s) checked
                BoardSpace property = FindPropertyByName(propertyName, currentPlayer.OwnedProperties);
                if (property != null)
                {
                    // Calculate the Unmortgage value and deduct from player's cash
                    // Unmortgage value is the same as mortgage value
                    int unmortgageValue = CalculateMortgageValue(property);
                    totalUnmortgageValue += unmortgageValue;
                    currentPlayer.TotalCash -= unmortgageValue;

                    // Mark property as unmortgaged
                    UpdatePropertyStatus(property, false);
                }
            }

            // Update player's cash total by subtracting from cash as it's an unmortgage
            gameBoardForm.UpdateCashLabel(currentPlayer, -totalUnmortgageValue);
            // Refresh the mortgage and unmortgage lists
            PopulateMortgageList(currentPlayerIndex, ownedProperties);

            Close();

        }
    }
}
