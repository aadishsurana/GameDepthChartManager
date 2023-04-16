using NFLDepthChart.Domain;
using System.Text;

namespace NFLDepthChart.Repositories
{
    public class GameRepository : IGameRepository
    {
        private Dictionary<string, List<Player>> nflTeam;

        public GameRepository()
        {
            nflTeam = BuildNFLTeam.BuildTeam();
        }

        public async Task<Dictionary<string, List<Player>>> AddPlayer(string position, Player player, int depth = -1)
        {
            var currentTeam = nflTeam;

            if (currentTeam.TryGetValue(position, out List<Player> existingPlayers))
            {
                if(!existingPlayers.Any(x => x.PlayerNum == player.PlayerNum))
                {
                    if (depth >= 0)
                    {
                        existingPlayers.Insert(depth, player);
                    }
                    else
                    {
                        existingPlayers.Add(player);
                    }
                    nflTeam[position] = existingPlayers;
                    return nflTeam;
                }
            }
            return currentTeam;
        }

        public async Task<Player?> RemovePlayer(string position, Player player)
        {
            var currentTeam = nflTeam;

            if (currentTeam.TryGetValue(position, out List<Player> existingPlayers))
            {
                var playerToRemove = existingPlayers.FirstOrDefault(p => p.PlayerNum == player.PlayerNum && p.PlayerName == player.PlayerName);

                if (playerToRemove != null)
                {
                    if (existingPlayers.Remove(playerToRemove))
                    {
                        nflTeam[position] = existingPlayers;
                        return player;
                    }
                }
            }
            return null;
        }

        public async Task<List<Player>?> GetBackups(string position, Player player)
        {
            var currentTeam = nflTeam;

            if (currentTeam.TryGetValue(position, out List<Player> existingPlayers))
            {
                var currentPlayer = existingPlayers.FirstOrDefault(p => p.PlayerNum == player.PlayerNum && p.PlayerName == player.PlayerName);
                int currentPlayerIndex = existingPlayers.FindIndex(x => x == currentPlayer);
                if (currentPlayer != null && currentPlayerIndex >= 0)
                {
                    var backupPlayers = existingPlayers.Skip(currentPlayerIndex + 1).ToList<Player>();
                    return backupPlayers;
                }
            }
            return null;
        }

        public async Task<Dictionary<string, List<Player>>> GetFullDepthChart()
        {
            return nflTeam;
        }

    }
}
