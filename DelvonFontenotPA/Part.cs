using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelvonFontenotPA
{
     abstract class Part
    {
        public int PartID { get; set; }
        private static int partIDCount;
        private static int numberOfParts;      
        public int PartIDCount { get { return partIDCount; } }
        public string PartName { get; set; }
        public int InStock { get; set; }
        public double PriceCost { get; set; }
        public int Max { get; set; }
        public int Min { get; set; }

        // Constructors for Part
        public Part(int partID, string partName, int inStock, double priceCost, int max, int min)
        {
            PartID = partID;
            PartName = partName;
            InStock = inStock;
            PriceCost = priceCost;
            Max = max;
            Min = min;
        }
        public Part(string partName, int inStock, double priceCost, int max, int min )
        {
            PartName = partName;
            InStock = inStock;
            PriceCost = priceCost;
            Max = max;
            Min = min;
        }
        public Part()
        {
            partIDCount = ++numberOfParts;
        }

        // Get the number of parts.
        public static int GetNumberOfParts()
        {
            return numberOfParts = Inventory.AllParts.Count();
        }
    }
}

/**************************************
* Author:               Delvon Fontenot
* Notes:                none
**************************************/
