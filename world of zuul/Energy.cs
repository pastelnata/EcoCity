namespace WorldOfZuul
{
    public class Energy
    {
        public double Income;
        public double DailyPrice;
        public double Price;
        public void IncomeCalc(double income)
        {
            Income = income;
        }

        public void DailyPriceCalc(double dailyPrice)
        {
            DailyPrice=dailyPrice;
        }

        public void BuildPrice(double price)
        {
            Price = price;
        }
    }

}