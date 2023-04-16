using NFLDepthChart.Domain;

namespace NFLDepthChart.Services
{
    public interface IGameService
    {
        Task<Dictionary<string, List<Player>>> AddPlayerToChart(string position, Player player, int depth = -1);
        Task<string> RemovePlayerFromChart(string position, Player playerToRemove);
        Task<string> GetBackupPlayers(string position, Player currentPlayer);
        Task<string> GetFullDepthChart();
        Task<Dictionary<string, List<Player>>> RawDepthChart();
    }
}
