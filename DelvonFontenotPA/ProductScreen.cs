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
    
    public partial class ProductScreen : BaseForm
    {
        bool isNewProduct = true;
        public static int indexOfSelectedPart;
        int count;

        public ProductScreen()
        {
            InitializeComponent();
            availablePartsDGV.DataSource = Inventory.AllParts;
            indexOfSelectedPart = -1;
            Product iProduct = Inventory.CurrentProduct;
            FormatDataGridView(productPartsDGV);
            FormatDataGridView(availablePartsDGV);
            DisplayParts();
            CheckPartlist();
        }

        // Format datagridview.
        private void FormatDataGridView(DataGridView view)
        {
            view.ClearSelection();
            view.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            view.RowHeadersVisible = false;
            view.DefaultCellStyle.SelectionBackColor = view.DefaultCellStyle.BackColor;
            view.DefaultCellStyle.SelectionForeColor = view.DefaultCellStyle.ForeColor;
            view.RowsDefaultCellStyle.BackColor = Color.LightSkyBlue;
            view.AlternatingRowsDefaultCellStyle.BackColor = Color.Silver;
        }

        // AddModifyProductLabel text changed event. 
        private void AddModifyProductLabel_TextChanged(object sender, EventArgs e)
        {
            // If text = Add Product, set isNewProduct to true.
            if (addModifyProductLabel.Text == "Add Product")
            {
                productPartsDGV.Rows.Clear();
                isNewProduct = true;

                // Create new Product
                if (isNewProduct)
                {
                    Product.GetNumberOfProducts();
                    Inventory.CurrentProduct = Inventory.AddProduct(new Product());
                    Inventory.Products.RemoveAt(Inventory.Products.Count - 1);
                }
            }
            // If text = Modify Product, set isNewProduct to false. Set ProductIDTextBox 
            // to that of current Product.
            else
            {
                isNewProduct = false;
                productIDTextBox.Text = Inventory.CurrentProduct.ProductID.ToString();              
            }
            DisplayParts();
        }

        // AvailableParts datagridview cell click event.
        private void AvailablePartsDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexOfSelectedPart = e.RowIndex;
            productPartsDGV.ClearSelection();
            deleteButton.Enabled = false;
            availablePartsDGV.DataSource = Inventory.AllParts;
            if(indexOfSelectedPart >= 0)
            {
                // Set CurrentPartID to the selected Row, cell 0 value.
                Inventory.CurrentPartID = (int)availablePartsDGV.Rows[indexOfSelectedPart].Cells[0].Value;
                // Lookup CurrentPart by CurrentPartID.
                Inventory.CurrentPart = Inventory.LookupPart(Inventory.CurrentPartID);
                availablePartsDGV.DefaultCellStyle.SelectionBackColor = Color.Lime;
                availablePartsDGV.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
            // Else, promp user to select a part.
            else
            {
                MessageBox.Show("Select a Part!");
            }
        }

        // ProductParts datagridview Cell click event.
        private void ProductPartsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexOfSelectedPart = e.RowIndex;
            availablePartsDGV.ClearSelection();
            deleteButton.Enabled = true;
            if(indexOfSelectedPart >= 0)
            {
                // Set AssociatedPartID to the selected row, cell 0 value.
                Product.AssociatedPartID = (int)productPartsDGV.Rows[indexOfSelectedPart].Cells[0].Value;
                // Lookup the AssociatedPart by CurrentPartID
                Product.AssociatedPart = Product.LookupAssociatedPart(Inventory.CurrentProductID);
                productPartsDGV.DefaultCellStyle.SelectionBackColor = Color.Lime;
                productPartsDGV.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
            }
            // Else, promp user to select a part.
            else
            {
                MessageBox.Show("Select a Part!");
            }
        }

        // ProductNameTextBox text changed event.
        private void ProductNameTextBox_TextChanged(object sender, EventArgs e)
        {
            productNameTextBox.ForeColor = Color.Black;
            if (string.IsNullOrWhiteSpace(productNameTextBox.Text))
            {
                productNameTextBox.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                productNameTextBox.BackColor = System.Drawing.Color.White;
            }
            saveButton.Enabled = AllowProductSave();
        }

        // InventoryTextBox text changed event.
        private void InventoryTextBox_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            inventoryTextBox.ForeColor = Color.Black;

            // If textbox is blank, flag error.
            if (string.IsNullOrWhiteSpace(inventoryTextBox.Text))
            {
                inventoryTextBox.BackColor = System.Drawing.Color.Yellow;
            }
            // If texbox has values.
            else
            {
                // If textbox is not null.
                if (!string.IsNullOrWhiteSpace(inventoryTextBox.Text))
                {
                    // Verify textbox is a number. IF so, allow entry.
                    try
                    {
                        Int32.Parse(inventoryTextBox.Text);
                        inventoryTextBox.BackColor = System.Drawing.Color.White;
                    }
                    // Catch FormatException and display a message. Flag an error.
                    catch (FormatException)
                    {
                        inventoryTextBox.Focus();
                        MessageBox.Show($"{inventoryTextBox.Text} is not an integer!", inventoryTextBox.Text);
                        inventoryTextBox.BackColor = System.Drawing.Color.Yellow;
                    }
                }
            }
            saveButton.Enabled = AllowProductSave();
        }

        // PriceCostTextBox text changed event.
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
            saveButton.Enabled = AllowProductSave();
        }

        // MaxTextBox text changed event.
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
            saveButton.Enabled = AllowProductSave();
        }

        // MinTextBox text changed event.
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
            saveButton.Enabled = AllowProductSave();
        }

        // Save button click event.
        private void SaveButton_Click(object sender, EventArgs e)
        {           
            if((MinMaxvalidation() == true) && (InventoryValidation() == true)) /* && (validation3() == true))*/
            {
                errorProvider1.Clear();
                if (isNewProduct)
                {
                    Product product = new Product(Convert.ToInt32(productIDTextBox.Text), productNameTextBox.Text.ToString(),
                    Convert.ToInt32(inventoryTextBox.Text), Convert.ToDouble(priceCostTextBox.Text), Convert.ToInt32(maxTextBox.Text),
                    Convert.ToInt32(minTextBox.Text), Inventory.PartList);
                    Inventory.InsertProduct(product);
                    MessageBox.Show("Product Added!");

                    //Create new PartList for CurrentProducts AssociatedParts.
                    Inventory.Restore();
                }
                else
                {
                    Product product = new Product(Convert.ToInt32(productIDTextBox.Text), productNameTextBox.Text.ToString(),
                    Convert.ToInt32(inventoryTextBox.Text), Convert.ToDouble(priceCostTextBox.Text), Convert.ToInt32(maxTextBox.Text),
                    Convert.ToInt32(minTextBox.Text), Inventory.PartList);
                    Inventory.SwapProduct(product);
                    MessageBox.Show("Product Updated!");

                    //Create new PartList for CurrentProducts AssociatedParts.
                    Inventory.Restore();
                }

                // Copy Current PartList to AssociatedParts
                Inventory.AssociatedPartsReStore();

                this.Hide();
                IMS updatedMainScreen = new IMS();
                updatedMainScreen.Show();
            }
        }
        
        // AddPartButton Click event.
        private void AddPartButton_Click(object sender, EventArgs e)
        {
            int contains = 0;

            if (Inventory.CurrentPart == null)
            {
                MessageBox.Show(" Select a Part to Add!");
            }
            else
            {
                for (int j = 0; j < Inventory.PartList.Count; j++)
                {

                    if (Inventory.CurrentPart.PartID == Inventory.PartList[j].PartID)
                    {
                        contains++;
                    }

                }
                if (contains == 0)
                {
                    Product.AddAssociatedPart(Inventory.CurrentPart);
                    count++;
                    productPartsDGV.Refresh();
                }
                else
                {
                    MessageBox.Show("Duplicate Part");
                }
                Inventory.CurrentPart = null;
                availablePartsDGV.ClearSelection();
            }
        }

        // Display Parts.
        public void DisplayParts()
        {
            productPartsDGV.AutoGenerateColumns = false;
            productPartsDGV.DataSource = Inventory.PartList;
            productPartsDGV.ClearSelection();
            productPartsDGV.Refresh();
        }
        
        // DeleteButton click event.
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (indexOfSelectedPart >= 0)
            {
                // Attempt to delete part if user acknowledge.
                try
                {
                    //Loop through Partlist.
                    for (int j = 0; j < Inventory.PartList.Count; j++)
                    {
                        // Check for PartID in Partlist.
                        if (Inventory.PartList[j].PartID == (int)productPartsDGV.Rows[indexOfSelectedPart].Cells[0].Value)
                        {
                            // Prompt user for acknowledgement.
                            DialogResult result = MessageBox.Show($"Are you sure you " +
                            $"want to delete {Inventory.PartList[j].PartName}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                Inventory.PartList.RemoveAt(j);
                                MessageBox.Show("Part will be Removed if Saved!","",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                // Check Inventory.Partlist for any parts left. If no parts, disable delete button.
                                CheckPartlist();
                            }
                            // IF user selects No.
                            else
                            {
                                MessageBox.Show("Not deleted");
                            }
                        }
                    }
                    indexOfSelectedPart = -1;
                    productPartsDGV.ClearSelection();
                }
                // Catch ArgumentOutOFRangeException, propmt user to select a part.
                catch (System.ArgumentOutOfRangeException)
                {
                    MessageBox.Show($"Select a Part");
                }
                productPartsDGV.Refresh();
            }
            else
            {
                MessageBox.Show(" Select a Part to delete!");
            }
        }

        // CancelButton click event. Cancel changes to Products partlist and do not save.
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
            IMS updatedMain = new IMS();
            updatedMain.Show();
            Inventory.PartList.Clear();
        }

        // Condition for product save button to be enabled
        private bool AllowProductSave()
        {
            if (((!string.IsNullOrWhiteSpace(productNameTextBox.Text)) &&
                        (!(string.IsNullOrWhiteSpace(maxTextBox.Text) || (!Int32.TryParse(maxTextBox.Text, out int number)))) &&
                        (!(string.IsNullOrWhiteSpace(minTextBox.Text) || (!Int32.TryParse(inventoryTextBox.Text, out number)))) &&
                        (!(string.IsNullOrWhiteSpace(priceCostTextBox.Text) || (!Double.TryParse(priceCostTextBox.Text, out double dNumber))))))
            {
                return true;
            }
            return false;
        }

        // MinMax Validation for saving product.
        public bool MinMaxvalidation()
        {
            bool test = true;
            if(Convert.ToInt32(maxTextBox.Text) < Convert.ToInt32(minTextBox.Text))
            {
                 maxTextBox.Focus();
                 errorProvider1.SetError(maxTextBox, MessageBox.Show("Max must be greater than or equal to Min!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error).ToString());
                 test = false;

            }
             return test;
        }


        // Inventory Validation for saving product.
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

        // NoParts Validation for saving products(Not currently used).
        public bool NoPartsValidation()
        {
            bool test = true;
            if (productPartsDGV.Rows.Count == 0)
            {
                errorProvider1.SetError(productPartsDGV, MessageBox.Show("Product must contatin at least" +
                    " one part", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error).ToString());
                test = false;
            }
            return test;
        }
        
        // MaxTextBox click event. Clear error.
        private void MaxTextBox_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        // InventoryTextBox click event. Clear error.
        private void InventoryTextBox_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        // MinTextBox click event. Clear error.
        private void MinTextBox_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }

        // SearchPartsButton click event. Search parts by name
        private void SearchPartsButton_Click(object sender, EventArgs e)
        {
            availablePartsDGV.ClearSelection();
            availablePartsDGV.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            bool found = false;
            if (searchBoxParts.Text != "")
            {
                for (int i = 0; i < Inventory.AllParts.Count; i++)
                {
                    if (Inventory.AllParts[i].PartName.ToUpper().Contains(searchBoxParts.Text.ToUpper()))
                    {
                        availablePartsDGV.Rows[i].Selected = true;
                        found = true;
                    }
                }
            }
            if (!found)
            {
                MessageBox.Show("No Part(s) Found!");
            }
        }

        // Check Inventory.Partlist for any parts. If no parts, disable delete button.
         public void CheckPartlist()
        {
            if (Inventory.PartList.Count == 0)
            {
                deleteButton.Enabled = false;
            }
        }
}
}
