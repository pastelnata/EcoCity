using System;
using System.Threading;

class GameDayProgress
{
    public static int currentDay = 0;
    public static int totalDays = 5;


//TODO IMPLEMENT AND FIX
    static void Days() 
    {
        DaysTimer();
    }

    static void DaysTimer()
    {
        // Timer timer = new Timer(DisplayMessage, null, 0, 300000); // 5 minutes in milliseconds
    }

    static void DisplayMessage(object state)
    {
        while (currentDay <= totalDays)
        {
            currentDay++;
            Console.WriteLine("a day has gone by. Check to see if more buildings are available");
        }
        if (currentDay > totalDays)
        {
            
        }
        
    }
}