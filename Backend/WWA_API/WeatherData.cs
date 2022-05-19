namespace WWA_API
{
    public class WeatherData
    {
        public int Id { get; set; }
        public string? Location { get; set; }
        public double Degree { get; set; }

        public DateTime Date { get; set; }
        public double RainProbability { get; set; }
        public double SunHours { get; set; }
    }
}
