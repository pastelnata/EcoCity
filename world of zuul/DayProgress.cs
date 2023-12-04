using System;
using System.Text;
using System.Timers;
using WorldOfZuul;

public class DayProgress
{
    
    public int currentDay { get; private set; }
    private System.Timers.Timer timer = new System.Timers.Timer(300000);

    public DayProgress(int day)
    {
        currentDay = day;
    }
    public void InitializeTimer()
    {
        timer.Elapsed += TimerElapsedEventHandler;
        timer.Start();
    }

    private void TimerElapsedEventHandler(object? sender, ElapsedEventArgs e)
    {
        UpdateDay();
    }   

    public void UpdateDay()
    {
 
        if (currentDay < 5)
        {
            DayProgress dayCounter = new DayProgress(0);
            currentDay++;
            Console.WriteLine();
            Console.Write($"New day: |  {currentDay}  |  ");
            Population.increasePopulation();
        }
        timer.Stop();
        timer.Start();
    }
}