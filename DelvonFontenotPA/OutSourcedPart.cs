using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelvonFontenotPA
{
    class OutSourcedPart:Part
    {
        private readonly int partIDCount;
        private static int numberOfParts;
        public new int PartIDCount { get { return partIDCount; } }
        public string CompanyName { get; set; }

        // Constructors for OutSourced Parts.
        public OutSourcedPart(string partName, int inStock, double priceCost, int max, int min, string companyName) : base(partName, inStock, priceCost, max, min)
        {
            CompanyName = companyName;
        }
        public OutSourcedPart(int partID, string partName, int inStock, double priceCost, int max, int min, string companyName) : base(partID,partName, inStock, priceCost, max, min)
        {
            CompanyName = companyName;
        }
        public OutSourcedPart():base()
        {
            partIDCount = ++numberOfParts;
        }

    }
}

/**************************************
* Author:               Delvon Fontenot
* Notes:                none
**************************************/
