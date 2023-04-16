using System.ComponentModel;

namespace NFLDepthChart.Domain.Requests
{
    public class AddPlayerRequest
    {
        public string Position { get; set; }
        public int PlayerNumber { get; set; }
        public string PlayerName { get; set; }

        [DefaultValue(-1)]
        public int Depth { get; set; }
    }
}
