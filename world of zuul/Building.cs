class Building
{
    int cost;
    int pollution;
    int population;
    int energyConsumption;

    class Residence : Building
    {
        List<string> residenceBuildings = new List<string> {""};
    }

    class Energy : Building
    {

    }

    class Commercial : Building
    {

    }

    

}


