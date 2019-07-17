using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelvonFontenotPA
{

    class InHousePart:Part
    {
        private readonly int partIDCount;
        private static int numberOfParts;
        public new int PartIDCount { get { return partIDCount; } }
        public int MachineID { get; set; }

        // Constructor for Inhouse Part
        public InHousePart(string partName, int inStock, double priceCost, int max, int min, int machineID) : base(partName, inStock, priceCost, max, min)
        {
            MachineID = machineID;
        }
        public InHousePart(int partID, string partName, int inStock, double priceCost, int max, int min, int machineID) : base(partID, partName, inStock, priceCost, max, min)
        {
            MachineID = machineID;
        }
        public InHousePart():base()
        {
            partIDCount = ++numberOfParts;
        }

    }
}

/**************************************
* Author:               Delvon Fontenot
* Notes:                none
**************************************/
