using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DelvonFontenotPA
{

    class Product

    {
        private BindingList<Part> associatedParts = new BindingList<Part>();
        
        public BindingList<Part> AssociatedParts { get { return associatedParts; } set { associatedParts = value; } }
        
        public static Part AssociatedPart { get; set; }
        public static int AssociatedPartID { get; set; }
        public static int AssociatedPartIndex { get; set; }
        private static int numberOfProducts;
        private static int productIDCount;
        public int ProductIDCount { get { return productIDCount; } }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int InStock { get; set; } 
        public int Min { get; set; }
        public int Max { get; set; }

        // Constructors for Products.     
        public Product(int productID, string productName, int inStock, double productPrice, int max, int min, BindingList<Part> associatedParts)
        {
            ProductID = productID;
            ProductName = productName;
            InStock = inStock;
            ProductPrice = productPrice;
            Max = max;
            Min = min;
            AssociatedParts = associatedParts;
        }
        public Product(string productName, int inStock, double productPrice, int max, int min, BindingList<Part> associatedParts)
        {
            ProductName = productName;
            InStock = inStock;
            ProductPrice = productPrice;
            Max = max;
            Min = min;
            AssociatedParts = associatedParts;
        }
        public Product()
        {
            productIDCount = 100 + (++numberOfProducts);
        }
        
        // Get number of Products.
        public static int GetNumberOfProducts()
        {
            return numberOfProducts = Inventory.Products.Count();
        }

        // Add associated Part.
        internal static void AddAssociatedPart(Part associatedPart)
        {
            Inventory.PartList.Add(associatedPart);
        }

        // Remove Associated Part.
        internal static void RemoveAssociatedPart(Part associatedPart)
        {
            Inventory.PartList.RemoveAt(AssociatedPartIndex);
        }

        // Lookup Associated Part by PartID.
        public static Part LookupAssociatedPart(int i)
        {
            for (int j = 0; j < Inventory.CurrentProduct.AssociatedParts.Count; j++)
            {
                if (Inventory.CurrentProduct.AssociatedParts[j].PartID.Equals(i))
                {
                    AssociatedPartID = j;
                    return Inventory.CurrentProduct.AssociatedParts[j];
                }
            }
            AssociatedPartIndex = -1;
            return null;
        }

    }
}

/**************************************
* Author:               Delvon Fontenot
* Notes:                none
**************************************/
