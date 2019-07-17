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
    public partial class IMS : BaseForm
    {
        public static int indexOfSelectedPart;
        public static int indexOfSelectedProduct;

        public IMS()
            
        {
            InitializeComponent();
            dataGridViewProducts.DataSource = Inventory.Products;
            dataGridViewParts.DataSource = Inventory.AllParts;
            indexOfSelectedPart = -1;
            indexOfSelectedProduct = -1;
            FormatDataGridView(dataGridViewParts);
            FormatDataGridView(dataGridViewProducts);
            CheckProducts();
            CheckAllParts();
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

        /*------------------------
        |   Part event handlers  |
        ------------------------*/

        // Add New Part.
        private void AddPartButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.dataGridViewProducts.Refresh();
            PartScreen addPartScreen = new PartScreen();

            // Change addModifyPartLabel text to Add Part.
            addPartScreen.addModifyPartLabel.Text = "Add Part";

            // Change text color to sliver.
            addPartScreen.partNametextBox.ForeColor = Color.Silver;
            addPartScreen.inventoryTextBox.ForeColor = Color.Silver;
            addPartScreen.priceCostTextBox.ForeColor = Color.Silver;
            addPartScreen.maxTextBox.ForeColor = Color.Silver;
            addPartScreen.minTextBox.ForeColor = Color.Silver;
            addPartScreen.compMachTextBox.ForeColor = Color.Silver;
            addPartScreen.ShowDialog();           
        }

        // Modify part.
        private void ModifyPartButton_Click(object sender, EventArgs e)
        {
            if (indexOfSelectedPart >= 0)
            {
                this.Hide();
                PartScreen modifyPartScreen = new PartScreen();

                // Change addModifyPartLabel text to Modify Part.
                modifyPartScreen.addModifyPartLabel.Text = "Modify Part";
                modifyPartScreen.partIDTextBox.Text = Inventory.CurrentPart.PartID.ToString();
                modifyPartScreen.partNametextBox.Text = Inventory.CurrentPart.PartName;
                modifyPartScreen.inventoryTextBox.Text = Inventory.CurrentPart.InStock.ToString();
                modifyPartScreen.priceCostTextBox.Text = Inventory.CurrentPart.PriceCost.ToString();
                modifyPartScreen.maxTextBox.Text = Inventory.CurrentPart.Max.ToString();
                modifyPartScreen.minTextBox.Text = Inventory.CurrentPart.Min.ToString();
                modifyPartScreen.Show();
            }
            else
            {
                MessageBox.Show("Select a Part to modify!");
            }
        }

        // Parts Datagridview click event.
        private void DataGridViewParts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            indexOfSelectedPart = e.RowIndex;
            dataGridViewProducts.ClearSelection();
            dataGridViewParts.DataSource = Inventory.AllParts;

            if(indexOfSelectedPart >= 0)
            {
                // Get part id from Cell 0 in Rows.
                Inventory.CurrentPartID = (int)dataGridViewParts.Rows[indexOfSelectedPart].Cells[0].Value;
            }
           else
            {
               MessageBox.Show("Please Select a Part!");
            }
            // Lookup part in inventory.
            Inventory.CurrentPart = Inventory.LookupPart(Inventory.CurrentPartID);
            dataGridViewParts.DefaultCellStyle.SelectionBackColor = Color.Lime;
            dataGridViewParts.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
        }

        // Delete part button click event.
        private void DeleteButton_Click_1(object sender, EventArgs e)
        {
            // If part is selected, delete part.
            if (indexOfSelectedPart >= 0) 
            {
                for (int j = 0; j < Inventory.AllParts.Count; j++)
                {
                    // Get part id from Cell 0 in Rows.
                    if (Inventory.AllParts[j].PartID == (int)dataGridViewParts.Rows[indexOfSelectedPart].Cells[0].Value)
                    {
                        // Prompt for deletion.
                        DialogResult result = MessageBox.Show($"Are you sure you " +
                        $"want to delete {Inventory.AllParts[j].PartName}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        // Delete Part. 
                        if (result == DialogResult.Yes)
                        {                            
                            Inventory.AllParts.RemoveAt(j);
                            MessageBox.Show("Part Removed!");
                            CheckAllParts();
                        }
                        else // nothinng was removed
                        {
                            MessageBox.Show("Nothing Removed");
                        }
                    }
                }
                DisplayParts();
                indexOfSelectedPart = -1;
            }
            // Select a part.
            else
            {
                MessageBox.Show("Select a Part!");
            }
        }

        // Search Part Button Click event.
        private void SearchPartsButton_Click(object sender, EventArgs e)
        {
            // Search for Part by Name.
            dataGridViewParts.ClearSelection();
            dataGridViewProducts.ClearSelection();
            dataGridViewParts.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            bool found = false;

            // If searchbox is not blank, select parts.
            if (searchBoxParts.Text != "") 
            {
                // Loop through parts.
                for (int i = 0; i < Inventory.AllParts.Count; i++) 
                {
                    if (Inventory.AllParts[i].PartName.ToUpper().Contains(searchBoxParts.Text.ToUpper()))
                    {
                        // Select parts.
                        dataGridViewParts.Rows[i].Selected = true;
                        found = true;
                    }
                }
            }

            // If no parts found.
            if (!found) 
            {
                MessageBox.Show("No Parts Found!");
            }
        }

        // Search part box text change event.
        private void SearchBoxParts_TextChanged(object sender, EventArgs e)
        {
            dataGridViewParts.ClearSelection(); 
        }

        // Display Parts.
        private void DisplayParts()
        {
            dataGridViewParts.AutoGenerateColumns = false;
            dataGridViewParts.DataSource = Inventory.AllParts;
            dataGridViewParts.ClearSelection();
        }

        /*------------------------
        | Products event handlers |
        -------------------------*/

        // Products datagridview Cell click event
        private void DataGridViewProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            indexOfSelectedProduct = e.RowIndex;
            dataGridViewParts.ClearSelection();

            if (indexOfSelectedProduct >= 0)
            {
                // Get Currentproduct ID from Cell 0 of Row.
                Inventory.CurrentProductID = (int)dataGridViewProducts.Rows[indexOfSelectedProduct].Cells[0].Value;
            }
            else
            {
                MessageBox.Show("Select a Product!");
            }

            // Lookup product using Product ID.
            Inventory.CurrentProduct = Inventory.LookupProduct(Inventory.CurrentProductID);

            //Add current products associated parts to PartList.
            Inventory.LoadPartList();

            dataGridViewProducts.DefaultCellStyle.SelectionBackColor = Color.Lime;
            dataGridViewProducts.DefaultCellStyle.SelectionForeColor = Color.DarkBlue;
        }

        // Search products Button Click event.
        private void SearchProductsButton_Click(object sender, EventArgs e)
        {
            dataGridViewProducts.ClearSelection();
            dataGridViewParts.ClearSelection();
            dataGridViewProducts.DefaultCellStyle.SelectionBackColor = Color.Yellow;
            bool found = false;

            // If seachbox is not blank, select products.
            if (searchBoxProducts.Text != "") 
            {
                for (int i = 0; i < Inventory.Products.Count; i++)
                {
                    if (Inventory.Products[i].ProductName.ToUpper().Contains(searchBoxProducts.Text.ToUpper()))
                    {
                        // select products.
                        dataGridViewProducts.Rows[i].Selected = true; 
                        found = true;
                    }
                }
            }
            // If no products found.
            if (!found) 
            {
                MessageBox.Show("No Product(s) Found!");
            }
        }

        // Search products Text box text change event.
        private void SearchBoxProducts_TextChanged(object sender, EventArgs e)
        {
            dataGridViewProducts.ClearSelection();
        }

        // Add Product Button Click event.
        private void AddProductButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            indexOfSelectedPart = -1;
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ProductScreen addProductScreen = new ProductScreen();

            // Change addModifyProductLabel Text to Add Product.
            addProductScreen.addModifyProductLabel.Text = "Add Product";
            addProductScreen.productIDTextBox.Text = Inventory.CurrentProduct.ProductIDCount.ToString();

            // Change text colors to silver.
            addProductScreen.productNameTextBox.ForeColor = Color.Silver;
            addProductScreen.inventoryTextBox.ForeColor = Color.Silver;
            addProductScreen.priceCostTextBox.ForeColor = Color.Silver;
            addProductScreen.maxTextBox.ForeColor = Color.Silver;
            addProductScreen.minTextBox.ForeColor = Color.Silver;
            addProductScreen.ShowDialog();
        }

        private void ModifyProductsButton_Click(object sender, EventArgs e)
        {
            // Modify Existing Product.
            if (indexOfSelectedProduct >= 0)
            {
                this.Hide();
                ProductScreen modifyProductScreen = new ProductScreen();

                // Change addModifyProductLabel Text to Modify Product.
                modifyProductScreen.addModifyProductLabel.Text = "Modify Product";
                modifyProductScreen.productNameTextBox.Text = Inventory.CurrentProduct.ProductName;
                modifyProductScreen.inventoryTextBox.Text = Inventory.CurrentProduct.InStock.ToString();
                modifyProductScreen.priceCostTextBox.Text = Inventory.CurrentProduct.ProductPrice.ToString();
                modifyProductScreen.maxTextBox.Text = Inventory.CurrentProduct.Max.ToString();
                modifyProductScreen.minTextBox.Text = Inventory.CurrentProduct.Min.ToString();
                modifyProductScreen.Show();
            }
            // Select a product to Modify.
            else
            {
                MessageBox.Show("Select a Product to modify!");
            }
        }

        // Delete product Button click event.
        private void DeleteProductsButton_Click(object sender, EventArgs e)
        {
            if (indexOfSelectedProduct >= 0)
            {
                try
                {
                    // Loop through Products.
                    for (int j = 0; j < Inventory.Products.Count; j++)
                    {
                        // If the selected Products PartList is blank
                        // Throw DeletionException.
                        if (Inventory.PartList.Count != 0)
                            throw new DeletionException();

                        // Else Find the product by Product ID and Delete the Product
                        else
                        {
                            if (Inventory.Products[j].ProductID == (int)dataGridViewProducts.Rows[indexOfSelectedProduct].Cells[0].Value)
                            {
                                // Prompt user for deletion.
                                DialogResult result = MessageBox.Show($"{Inventory.PartList.Count.ToString()} part(s) Associated with " +
                                    $"this Product, are you sure you " +
                                    $"want to delete {Inventory.Products[j].ProductName}?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    // Delete the Product and prompt confirmation.
                                    Inventory.CurrentPartIndex = j;
                                    Inventory.RemoveProduct(Inventory.CurrentProductIndex);
                                    MessageBox.Show("Product Removed!");
                                }
                                // If User selects No, Prompt that nothing was removed.
                                else
                                {
                                    MessageBox.Show("Nothing Removed");
                                }
                            }
                        }
                    }                    
                }
                // Catch the Deletion Exception and display message.
                catch(DeletionException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CheckProducts();
                DisplayProducts();
                indexOfSelectedProduct = -1;
            }
            // If product is not selected, prompt to select a product.
            else
            {
                MessageBox.Show("Select a Product!");
            }
        }

        // Display Products.
        private void DisplayProducts()
        {
            dataGridViewProducts.AutoGenerateColumns = false;
            dataGridViewProducts.DataSource = Inventory.Products;
            dataGridViewProducts.ClearSelection();
        }

        // Deletion Exception for Product deletion
        class DeletionException : Exception
        {
            public DeletionException() : base($"Cannot Delete, Product is still associated with Parts!")
            {
                MessageBox.Show($"{Inventory.PartList.Count.ToString()} Part(s) still Associated with this Product");
            }
        }

        // Check to see if any parts are in All Parts, if not disable deleting and modifying.
        public void CheckAllParts()
        {
            if (Inventory.AllParts.Count == 0)
            {
                DeleteButton.Enabled = false;
                modifyPartButton.Enabled = false;
            }
        }

        // Check to see if any products are in Products, if not disable deleting and modifying.
        public void CheckProducts()
        {
            if (Inventory.Products.Count == 0)
            {
                deleteProductsButton.Enabled = false;
                modifyProductsButton.Enabled = false;
            }
        }

        // Exit program.
        private void ExitButton_Click(object sender, EventArgs e)
        { 
            this.Close();
            Application.Exit();
        }
    }
}
