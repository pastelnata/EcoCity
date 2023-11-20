using System;
using System.Text;
using System.Timers;
using world_of_zuul;
using WorldOfZuul;

public class DayProgress
{
    
    public int currentDay { get; private set; }
    private System.Timers.Timer timer = new System.Timers.Timer(100);

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
            Population population = new Population();
            DayProgress dayCounter = new DayProgress(0);
            for (int i = 0; i < currentDay; i++)
            {
                population.startingPopulation += 50;
            }
     
            currentDay++;
            Console.WriteLine();
            Console.Write($"New day: | {currentDay}  | population: | {population.startingPopulation}");
            Console.Write($" | pollution: | ");
            Pollution.DisplayPollution();

        }
        timer.Stop();
        timer.Start();
    }
}