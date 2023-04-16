using NFLDepthChart.Domain;
using NFLDepthChart.Repositories;
using System.Text;

namespace NFLDepthChart.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Dictionary<string, List<Player>>?> AddPlayerToChart(string position, Player player, int depth = -1)
        {
            var playerAdded = await _gameRepository.AddPlayer(position, player, depth);
            return playerAdded;
        }

        public async Task<string> RemovePlayerFromChart(string position, Player playerToRemove)
        {
            var playerRemoved = await _gameRepository.RemovePlayer(position, playerToRemove);
            if(playerRemoved != null)
            {
                return $"{playerRemoved.PlayerNum} - {playerRemoved.PlayerName}";
            }
            else
            {
                return "No List";
            }
        }

        public async Task<string> GetBackupPlayers(string position, Player currentPlayer)
        {
            var backupPlayers = await _gameRepository.GetBackups(position, currentPlayer);
            if(backupPlayers != null && backupPlayers.Count > 0)
            {
                StringBuilder supportPlayers = new StringBuilder();
                foreach (var item in backupPlayers)
                {
                    supportPlayers.AppendLine($"{item.PlayerNum}-{item.PlayerName}");
                }

                return supportPlayers.ToString();
            }
            else
            {
                return "No List";
            }
        } 

        public async Task<string?> GetFullDepthChart()
        {
            var fullDepthChart = await _gameRepository.GetFullDepthChart();

            if(fullDepthChart != null)
            {
                StringBuilder depthChart = new StringBuilder();

                foreach (var item in fullDepthChart)
                {
                    depthChart.AppendLine($"{item.Key}: [{string.Join(",", item.Value.Select(s => s.PlayerNum + "-" + s.PlayerName))}]");
                }

                return depthChart.ToString();
            }
            return null;
        }

        public async Task<Dictionary<string, List<Player>>> RawDepthChart()
        {
            var getFullChart = await _gameRepository.GetFullDepthChart();
            return getFullChart;
        }
    }
}
