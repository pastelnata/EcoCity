using System.Diagnostics;

namespace WorldOfZuul
{
    public static class Happyness
    {
        public static int happyness;
        public static int happynessLimit;

        public static void HappynessLimit()
        {
            switch (Game.dayCounter.currentDay) 
            {
                case 1:
                    happynessLimit = 300;
                    break;
                case 2:
                    happynessLimit = 600;
                    break;
                case 3:
                    happynessLimit = 1200;
                    break;
                case 4:
                    happynessLimit = 1800;
                    break;
                case 5:
                    happynessLimit = 3600;
                    break;
            }
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
