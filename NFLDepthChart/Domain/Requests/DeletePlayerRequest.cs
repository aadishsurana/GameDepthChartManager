namespace NFLDepthChart.Domain.Requests
{
    public class DeletePlayerRequest
    {
        public string Position { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerName { get; set; }
    }
}
