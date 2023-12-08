using System;
using System.Drawing;
using System.Text;
using System.Timers;
using world_of_zuul;
using WorldOfZuul;
using static world_of_zuul.VisualTextWriter;
using static System.ConsoleColor;

public class DayProgress
{
    public int currentDay;
    private System.Timers.Timer timer = new System.Timers.Timer(300000);

    private static List<List<string>> newBuildingsAccordingToDay = new List<List<string>>
    {
        new List<string>(),
        new List<string> {"food supply", "hospital", "oil supply", "eco house"},
        new List<string> {"wind energy", "solar energy", "community center", "shops"},
        new List<string> {"luxury house", "public transport", "fission energy"},
        new List<string>(),
        new List<string> {"fusion energy"} 
    };
    
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
        UpdateDay(); //event that occurs when timer ends
    }

    void BuildingsDailyUpdater()
    {
        foreach (var room in Game.rooms) // makes sure to update buildings in every room
        {
            foreach (var newBuilding in newBuildingsAccordingToDay[currentDay])
            {
                if (room.buildings.ContainsKey(newBuilding)) 
                {
                    room.buildings[newBuilding] = 0; 
                }
            }
        }
    }

    public void UpdateDay()
    {
 
        if (currentDay < 5)
        {
            SetColor(Red);
            DayProgress dayCounter = new DayProgress(0);
            currentDay++;
            BuildingsDailyUpdater();
            Building.DailyMoneyManager();
            Console.WriteLine($"A day has passed. Current day: {currentDay}");
            ENERGY.EnergyIsEnough();
            ColorReset();
            SetColor(Blue);
            Console.WriteLine($"You now have: {Game.currentMoney} euros.");
            Console.WriteLine();
            ColorReset();
            SetColor(DarkGreen);
            Console.Write($"New day: |  {currentDay}  |  ");  
            Console.Write("Pollution:");
            Pollution.DisplayPollution();
            Console.Write(" | ");
            Population.IncreasePopulation();
            ColorReset();
        }
        timer.Stop();
        timer.Start();
    }
}