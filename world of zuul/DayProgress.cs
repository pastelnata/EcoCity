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
            WriteLine($"A day has passed. Current day: {currentDay}");
            ENERGY.EnergyIsEnough();
            ColorReset();
            SetColor(Blue);
            WriteLine($"You now have: {Game.currentMoney} euros.");
            WriteLine();
            ColorReset();
            SetColor(DarkGreen);
            Write($"New day: |  {currentDay}  |  ");  
            Write("Pollution:");
            Pollution.DisplayPollution();
            Write(" | ");
            Population.IncreasePopulation();
            ColorReset();
        }
        timer.Stop();
        timer.Start();
    }
}