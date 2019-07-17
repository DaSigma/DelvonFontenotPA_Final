using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelvonFontenotPA
{
    public partial class PartScreen : BaseForm
    {
        bool isInHouse;
        bool isNewPart;
        public PartScreen()
        {
            InitializeComponent();

            // If the current part is an Inhouse part
            // set compMachTextBox to Inhouse parts Machine ID
            // and check the inhouseRadioButton
            if (Inventory.CurrentPart is InHousePart)
            {
                InHousePart iPart = (InHousePart)Inventory.CurrentPart;
                compMachTextBox.Text = iPart.MachineID.ToString();
                isInHouse = true;
                inHouseRadioButton.Checked = true;
            }
            // If the current part is an Outsourced part
            // and check the outsourcedRadioButton
            else
            {
                OutSourcedPart oPart = (OutSourcedPart)Inventory.CurrentPart;
                compMachTextBox.Text = oPart.CompanyName;
                isInHouse = false;
                outsourcedRadioButton.Checked = true;
            }
        }

        // InHouseRadioButton_CheckChanged Event
        // If InHousePartRadioButton is sellected
        // Change the changeLabel text to Machine ID and
        // the compMachTextBox text to a number
        private void InHouseRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            changeLabel.Text = "      Machine ID";
            //compMachTextBox.Text = "1234";
            isInHouse = true;
            CheckOnRadioButtonSwitch();
        }

        // OutSourcedRadioButton_CheckChanged Event
        // If OutsourcedRadioButton is sellected
        // Change the changeLabel text to Company Name and
        // the compMachTextBox text to a string.
        private void OutsourcedRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            changeLabel.Text = "Company Name";
            Part.GetNumberOfParts();
            //compMachTextBox.Text = "Company Name"; 
            isInHouse = false;
            CheckOnRadioButtonSwitch();
        }

        // Add ModifyPartLable Text Changed Event.
        private void AddModifyPartLabel_TextChanged(object sender, EventArgs e)
        {
            // If the text is Add Part, isNewPart is true, Create new part
            if (addModifyPartLabel.Text == "Add Part")
            {
                isNewPart = true;

                if (isNewPart)
                {            
                    isInHouse = true;
                    inHouseRadioButton.Checked = true;
                    Part.GetNumberOfParts();
                    Inventory.CurrentPart = Inventory.AddPart(new InHousePart());
                    partIDTextBox.Text = Inventory.CurrentPart.PartIDCount.ToString();
                    Inventory.AllParts.RemoveAt(Inventory.AllParts.Count - 1);
                }
            }
            // isNewPart is false
            else
            {
                isNewPart = false;
                if (Inventory.CurrentPart is OutSourcedPart)
                {
                    outsourcedRadioButton.Checked = true;
                    Part.GetNumberOfParts();
                    CheckOnRadioButtonSwitch();
                }
                if (Inventory.CurrentPart is InHousePart)
                {
                    inHouseRadioButton.Checked = true;
                    Part.GetNumberOfParts();
                    CheckOnRadioButtonSwitch();
                }
            }
        }

        // Part ID Text box text changed event.
        private void PartIDTextBox_TextChanged(object sender, EventArgs e)
        {
            partIDTextBox.BackColor = System.Drawing.Color.Silver;         
        }

        // Part Name text box text changed event.
        private void PartNametextBox_TextChanged(object sender, EventArgs e)
        {
            partNametextBox.ForeColor = Color.Black;
            if (string.IsNullOrWhiteSpace(partNametextBox.Text))
            {
                partNametextBox.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                partNametextBox.BackColor = System.Drawing.Color.White;
            }
        }

        // Inventory text box text changed event.
        private void InventoryTextBox_TextChanged(object sender, EventArgs e)
        {
            inventoryTextBox.ForeColor = Color.Black;
            if (string.IsNullOrWhiteSpace(inventoryTextBox.Text))
            {
                inventoryTextBox.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(inventoryTextBox.Text))
                {
                    // Check if the char input is a number.
                    try
                    {
                        Int32.Parse(inventoryTextBox.Text);
                        inventoryTextBox.BackColor = System.Drawing.Color.White;
                    }
                    // Catch Format exception if char is not a number. Display message.
                    catch (FormatException)
                    {                       
                        inventoryTextBox.Focus();
                        MessageBox.Show($"{inventoryTextBox.Text} is not an integer!", inventoryTextBox.Text);
                        inventoryTextBox.BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }           
            saveButton.Enabled = AllowPartSave();
        }

        // PriceCost text box text changed event.
        private void PriceCostTextBox_TextChanged(object sender, EventArgs e)
        {
            priceCostTextBox.ForeColor = Color.Black;
            if (string.IsNullOrWhiteSpace(priceCostTextBox.Text) || (!Double.TryParse(priceCostTextBox.Text, out double number)))
            {
                priceCostTextBox.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                priceCostTextBox.BackColor = System.Drawing.Color.White;
            }
            saveButton.Enabled = AllowPartSave();
        }

        // Max text box text changed event.
        private void MaxTextBox_TextChanged(object sender, EventArgs e)
        {
            maxTextBox.ForeColor = Color.Black;
            if (string.IsNullOrWhiteSpace(maxTextBox.Text) || (!Int32.TryParse(maxTextBox.Text, out int number)))
            {
                maxTextBox.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                maxTextBox.BackColor = System.Drawing.Color.White;
            }
            saveButton.Enabled = AllowPartSave();
        }

        // Min text box text changed event.
        private void MinTextBox_TextChanged(object sender, EventArgs e)
        {
            minTextBox.ForeColor = Color.Black;
            if (string.IsNullOrWhiteSpace(minTextBox.Text) || (!Int32.TryParse(minTextBox.Text, out int number)))
            {
                minTextBox.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                minTextBox.BackColor = System.Drawing.Color.White;
            }
            saveButton.Enabled = AllowPartSave();
        }

        // CompMach text box text changed event.
        private void CompMachTextBox_TextChanged(object sender, EventArgs e)
        {
            compMachTextBox.ForeColor = Color.Black;
            CheckOnRadioButtonSwitch();
        }

        // Save Button click Event.
        private void SaveButton_Click(object sender, EventArgs e)
        {

            // if Min and Max is validated and Inventory is validated Clear error provider and allow save.
            if ((MinMaxValidation() == true) && (InventoryValidation() == true))
            {
                errorProvider1.Clear();
                if (isInHouse)
                {
                    // Save inhouse part.
                    Part part = new InHousePart(Convert.ToInt32(partIDTextBox.Text), partNametextBox.Text.ToString(), Convert.ToInt32(inventoryTextBox.Text), Convert.ToDouble(priceCostTextBox.Text), Convert.ToInt32(maxTextBox.Text),
                    Convert.ToInt32(minTextBox.Text), Convert.ToInt32(compMachTextBox.Text));

                    // If isNewPart true - Add part.
                    if (isNewPart)
                    {
                        Inventory.AddPart(part);
                        MessageBox.Show("In-House Part Added!");
                    }
                    // If isNewPart is false - Swap part.
                    else
                    {
                        Inventory.SwapPart(part);
                        MessageBox.Show("Part Updated!");
                    }                   
                }
                // Else Save Outsourced part.
                else
                {
                    Part part = new OutSourcedPart(Convert.ToInt32(partIDTextBox.Text), partNametextBox.Text.ToString(), Convert.ToInt32(inventoryTextBox.Text), Convert.ToDouble(priceCostTextBox.Text), Convert.ToInt32(maxTextBox.Text),
                    Convert.ToInt32(minTextBox.Text), compMachTextBox.Text.ToString());

                    // If isNewPart true - Add part.
                    if (isNewPart)
                    {
                        Inventory.InsertPart(part);
                        MessageBox.Show("Out-Sourced Part Added!");
                    }
                    // If isNewPart is false - Swap part.
                    else
                    {
                        Inventory.SwapPart(part);
                        MessageBox.Show("Part Updated!");
                    }                   
                }
                this.Hide();
                IMS updatedMain = new IMS();
                updatedMain.Show();
            }
        }

        // Cancel button click event.
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            IMS updatedMain = new IMS();
            updatedMain.Show();
        }

        // Allow save conditions method.
        private bool AllowPartSave()
        {
            return (!string.IsNullOrWhiteSpace(partNametextBox.Text)) &&
                (!(string.IsNullOrWhiteSpace(maxTextBox.Text) || (!Int32.TryParse(maxTextBox.Text, out int number)))) &&
                (!(string.IsNullOrWhiteSpace(minTextBox.Text) || (!Int32.TryParse(inventoryTextBox.Text, out number)))) &&
                (!(string.IsNullOrWhiteSpace(priceCostTextBox.Text) || (!Double.TryParse(priceCostTextBox.Text, out double dNumber)))) &&
                (!(string.IsNullOrWhiteSpace(compMachTextBox.Text) || (isInHouse && !Int32.TryParse(compMachTextBox.Text, out number) ||
                (!isInHouse && Int32.TryParse(compMachTextBox.Text, out number)))));
        }

        // Check conditions when radio button is switched.
        private void CheckOnRadioButtonSwitch()
        {
            if (string.IsNullOrWhiteSpace(compMachTextBox.Text) || (isInHouse && !Int32.TryParse(compMachTextBox.Text, out int number)) ||
                (!isInHouse && Int32.TryParse(compMachTextBox.Text, out number)))
            {
                compMachTextBox.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                compMachTextBox.BackColor = System.Drawing.Color.White;
            }
            saveButton.Enabled = AllowPartSave();
        }

        // Min and Max validation. Return false if Min is greater than max.
        public bool MinMaxValidation()
        {
            bool test = true;
            if (Convert.ToInt32(maxTextBox.Text) < Convert.ToInt32(minTextBox.Text))
            {
                maxTextBox.Focus();
                errorProvider1.SetError(maxTextBox, MessageBox.Show("Max must be greater than or equal to Min!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error).ToString());
                test = false;
            }
            return test;
        }

        // Inventory validation. Return false if Inventory is not between min and max.
        public bool InventoryValidation()
        {
            bool test = true;
            if ((Convert.ToInt32(inventoryTextBox.Text) > Convert.ToInt32(maxTextBox.Text)) ||
                (Convert.ToInt32(inventoryTextBox.Text) < Convert.ToInt32(minTextBox.Text)))
            {
                inventoryTextBox.Focus();
                errorProvider1.SetError(inventoryTextBox, MessageBox.Show("Inventory Must be " +
                    "between Max and Min!", "Inventory Error", MessageBoxButtons.OK, MessageBoxIcon.Error).ToString());
                test = false;
            }
            return test;
        }

        // Inventory text box click event.
        private void InventoryTextBox_Click(object sender, EventArgs e)
        {
            // Clear Error.
            errorProvider1.Clear();
        }

        // Max text box click event.
        private void maxTextBox_Click(object sender, EventArgs e)
        {
            // Clear Error.
            errorProvider1.Clear();
        }

        // Min text box click event.
        private void minTextBox_Click(object sender, EventArgs e)
        {
            // Clear Error.
            errorProvider1.Clear();
        }
    }
}
