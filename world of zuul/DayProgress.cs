using System;
using System.Timers;
using WorldOfZuul;

class DayProgress
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
        if (currentDay <= 5)
        {
            currentDay++;
            Building.DailyMoneyManager();
            Console.WriteLine($"A day has passed. Current day: {currentDay}");
            Console.WriteLine($"You now have: {Game.currentMoney} euros.");
        }
        timer.Stop();
        timer.Start();
    }
}