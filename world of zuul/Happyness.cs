using System;
using System.Diagnostics;

namespace WorldOfZuul
{
    public static class Happyness
    {
        public static int happyness;
        public static int happynessLimit;

        public static int HappynessLimit()
        {
            int[] happynessArray = {300, 600, 1200, 1800, 3600};
            for(int i = Game.dayCounter.currentDay; i-1 >= 0 ; i--)
            {
                if(happyness> happynessArray[i-1])
                {
                    return i-1;
                }
            }
            return -1;
        }
        
        public static bool IsHappynessEnough() 
        {
            if(happyness < happynessLimit) 
            {
                return false;
            }
            return true;
        }
    }
}
