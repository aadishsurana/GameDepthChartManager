using NFLDepthChart.Domain;

namespace NFLDepthChart.Repositories
{
    public interface IGameRepository
    {
        Task<Dictionary<string, List<Player>>> AddPlayer(string position, Player player, int depth = -1);
        Task<Player?> RemovePlayer(string position, Player player);
        Task<List<Player>?> GetBackups(string position, Player player);
        Task<Dictionary<string, List<Player>>> GetFullDepthChart();

    }
}
