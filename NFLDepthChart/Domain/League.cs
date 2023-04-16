namespace NFLDepthChart.Domain
{
    public class League
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
    }
}
