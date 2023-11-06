using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace world_of_zuul
{
    public class Buildings
    {
        public static void PlaceBuilding(string buildingName)
        {
            Console.WriteLine($"you have successfuly built {buildingName}");
        }
        public Buildings()
        {
            Pollution pollution = new Pollution();
            while (pollution.StartPollution <  pollution.MaxPollution)
            {
                
            }
        }        
    }
}