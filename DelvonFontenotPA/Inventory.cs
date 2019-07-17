using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelvonFontenotPA
{
   static class Inventory
    {

        private static BindingList<Part> allParts = new BindingList<Part>();
        private static BindingList<Part> partList = new BindingList<Part>();
        private static BindingList<Product> products = new BindingList<Product>();

        // Partlists bindinglist.
        public static BindingList<Part> AllParts { get { return allParts; } set { allParts = value; } }
        public static BindingList<Part> PartList { get { return partList; } set { partList = value; } }

        // Products bindingList.
        public static BindingList<Product> Products { get { return products; } set { products = value; } }


        // Product AssociatedParts preloaded bindingLists.
        public static BindingList<Part> googlePhoneList = new BindingList<Part>();
        public static BindingList<Part> samsungPhoneList = new BindingList<Part>();
        public static BindingList<Part> iPhoneList = new BindingList<Part>();
        public static BindingList<Part> lGPhoneList = new BindingList<Part>();
        public static BindingList<Part> motorolaPhoneList = new BindingList<Part>();
        public static BindingList<Part> newPhoneList = new BindingList<Part>();
        public static BindingList<Part> onePlusPhoneList = new BindingList<Part>();

        // Propeties.
        public static Part CurrentPart { get; set; }
        public static Product CurrentProduct { get; set; }
        public static int CurrentPartID { get; set; }
        public static int CurrentPartIndex { get; set; }             
        public static int CurrentProductID { get; set; }
        public static int CurrentProductIndex { get; set; }
        public static int CurrentProductName { get; set; }

        // Lookup Part by PartID.
        public static Part LookupPart(int i)
        {
            for (int j = 0; j < AllParts.Count; j++)
            {
                if (AllParts[j].PartID.Equals(i))
                {
                    CurrentPartIndex = j;
                    return AllParts[j];
                }
            }
            CurrentPartIndex = -1;
            return null;
        }

        // Lookup Product by ProductName.
        public static Product LookupProduct(string name)
        {
            for (int j = 0; j < Products.Count; j++)
            {
                if (Products[j].ProductName.Equals(name))
                {
                    CurrentProductName = j;
                    return Products[j];
                }
            }
            CurrentProductIndex = -1;
            return null;
        }

        // Lookup Product by ProductID.
        public static Product LookupProduct(int i)
        {
            for (int j = 0; j < Products.Count; j++)
            {
                if (Products[j].ProductID.Equals(i))
                {
                    CurrentProductIndex = j;
                    return Products[j];
                }
            }
            CurrentProductIndex = -1;
            return null;
        }

        // SwapParts.
        internal static void SwapPart(Part part)
        {
            AllParts.Insert(CurrentPartIndex, part);
            AllParts.RemoveAt(CurrentPartIndex + 1);
        }


        //SwapProducts.
        internal static void SwapProduct(Product product)
        {
            Products.Insert(CurrentProductIndex, product);
            Products.RemoveAt(CurrentProductIndex + 1);
        }

        // Insert Part.
        internal static void InsertPart(Part part)
        {
            AllParts.Insert(CurrentPartIndex, part);
        }

        // Insert Product.
        internal static void InsertProduct(Product product)
        {
            Products.Insert(CurrentProductName, product);
        }

        // Remove Part by Part.
        internal static void RemovePart(Part part)
        {
            AllParts.RemoveAt(CurrentPartIndex);
        }

        // Remove Product.
        internal static bool RemoveProduct(int productIDX)
        {
            Products.RemoveAt(productIDX);
            return true;
        }

        // Add Part.
        internal static Part AddPart(Part part)
        {
            AllParts.Add(part);
            return part;
        }

        // Add Product.
        internal static Product AddProduct(Product product)
        {
            Products.Add(product);
            return product;
        }

        // Create new Partlist of AssociatedParts and assign to CurrentProduct.
        public static void Restore()
        {
            PartList = new BindingList<Part>(CurrentProduct.AssociatedParts);
            CurrentProduct.AssociatedParts = PartList;
        }

        // Copy Current Partlist to AssociatedParts
        public static void AssociatedPartsReStore()
        {
            CurrentProduct.AssociatedParts.Clear();
            foreach (Part i in PartList)
            {
                CurrentProduct.AssociatedParts.Add(i);
            }
        }

        // Add current products associated parts to partlist.
        public static void LoadPartList()
        {
            PartList.Clear();
            foreach (Part i in CurrentProduct.AssociatedParts)
            {
                PartList.Add(i);
            }
        }

        // Add items to lists.
        internal static void Build()
        {
            AllParts.Clear();
            AllParts.Add(new InHousePart(1, "LCD Screen", 12, 49.25, 25, 25, 2525));
            Inventory.AllParts.Add(new InHousePart(2, "Microphone", 25, 6.32, 50, 0, 9874));
            Inventory.AllParts.Add(new InHousePart(3, "Front Camera", 25, 8.99, 25, 25, 7898));
            Inventory.AllParts.Add(new InHousePart(4, "Rear Camera", 25, 9.99, 50, 0, 8748));
            Inventory.AllParts.Add(new InHousePart(5, "Touch Sensor", 25, 9.99, 50, 0, 1974));
            Inventory.AllParts.Add(new OutSourcedPart(6, "GPS", 25, 15.65, 25, 25, "ABC Tecknology"));
            Inventory.AllParts.Add(new OutSourcedPart(7, "Battery", 25, 24.65, 50, 0, "Duracell Lithium LLC"));
            Inventory.AllParts.Add(new OutSourcedPart(8, "Phone Speaker", 25, 24.65, 50, 0, "Motorola"));
            Inventory.AllParts.Add(new OutSourcedPart(9, "Processor", 25, 24.65, 50, 0, "Intel"));
            Inventory.AllParts.Add(new OutSourcedPart(10, "Processor", 25, 24.65, 50, 0, "AMD"));

            // products partslist
            Inventory.Products.Clear();
            Inventory.Products.Add(new Product(101, "OnePlus", 25, 624.65, 50, 0, Inventory.onePlusPhoneList));
            Inventory.Products.Add(new Product(102, "Moto", 25, 624.65, 50, 0, Inventory.motorolaPhoneList));
            Inventory.Products.Add(new Product(103, "Google Phone", 30, 895.99, 50, 25, Inventory.googlePhoneList));
            Inventory.Products.Add(new Product(104, "Samsung Galaxy", 25, 955.25, 50, 0, Inventory.samsungPhoneList));
            Inventory.Products.Add(new Product(105, "iPhone", 25, 1125.95, 25, 25, Inventory.iPhoneList));
            Inventory.Products.Add(new Product(106, "LG View", 25, 599.65, 50, 0, Inventory.lGPhoneList));

            Inventory.googlePhoneList.Clear();
            Inventory.googlePhoneList.Add(new InHousePart(1, "LCD Screen", 12, 49.25, 25, 25, 2525));
            Inventory.googlePhoneList.Add(new InHousePart(3, "Front Camera", 25, 8.99, 25, 25, 7898));


            // samsung phone partslist
            Inventory.samsungPhoneList.Clear();
            Inventory.samsungPhoneList.Add(new InHousePart(3, "Front Camera", 25, 8.99, 25, 25, 7898));
            Inventory.samsungPhoneList.Add(new InHousePart(4, "Rear Camera", 25, 9.99, 50, 0, 8748));
            Inventory.samsungPhoneList.Add(new OutSourcedPart(6, "GPS", 25, 15.65, 25, 25, "ABC Tecknology"));
            Inventory.samsungPhoneList.Add(new OutSourcedPart(7, "Battery", 25, 24.65, 50, 0, "Duracell Lithium LLC"));
            Inventory.samsungPhoneList.Add(new OutSourcedPart(9, "Processor", 25, 24.65, 50, 0, "Intel"));

            // iPhone partslist
            Inventory.iPhoneList.Clear();
            Inventory.iPhoneList.Add(new InHousePart(1, "LCD Screen", 12, 49.25, 25, 25, 2525));
            Inventory.iPhoneList.Add(new InHousePart(4, "Rear Camera", 25, 9.99, 50, 0, 8748));
            Inventory.iPhoneList.Add(new OutSourcedPart(6, "GPS", 25, 15.65, 25, 25, "ABC Tecknology"));
            Inventory.iPhoneList.Add(new OutSourcedPart(7, "Battery", 25, 24.65, 50, 0, "Duracell Lithium LLC"));
            Inventory.iPhoneList.Add(new OutSourcedPart(10, "Processor", 25, 24.65, 50, 0, "AMD"));

            // LG phone partslist
            Inventory.lGPhoneList.Clear();
            Inventory.lGPhoneList.Add(new InHousePart(1, "LCD Screen", 12, 49.25, 25, 25, 2525));
            Inventory.lGPhoneList.Add(new InHousePart(3, "Front Camera", 25, 8.99, 25, 25, 7898));
            Inventory.lGPhoneList.Add(new InHousePart(4, "Rear Camera", 25, 9.99, 50, 0, 8748));
            Inventory.lGPhoneList.Add(new OutSourcedPart(7, "Battery", 25, 24.65, 50, 0, "Duracell Lithium LLC"));
            Inventory.lGPhoneList.Add(new OutSourcedPart(10, "Processor", 25, 24.65, 50, 0, "AMD"));

            // motorola phone partslist
            Inventory.motorolaPhoneList.Clear();
            Inventory.motorolaPhoneList.Add(new InHousePart(1, "LCD Screen", 12, 49.25, 25, 25, 2525));

            // onePlus phone partlist
            Inventory.onePlusPhoneList.Clear();
            Inventory.onePlusPhoneList.Add(new InHousePart(4, "Rear Camera", 25, 9.99, 50, 0, 8748));
        }

    }

}

/**************************************
* Author:               Delvon Fontenot
* Notes:                none
**************************************/
