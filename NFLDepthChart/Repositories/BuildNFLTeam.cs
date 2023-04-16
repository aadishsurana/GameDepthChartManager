using NFLDepthChart.Domain;
using System.Collections;
using System.Text;

namespace NFLDepthChart.Repositories
{
    public static class BuildNFLTeam
    {
        public static Dictionary<string, List<Player>> BuildTeam()
        {

            var allPositions = new List<Positions>
            {
                new Positions { positionName = "LWR"},
                new Positions {positionName = "RWR"},
                new Positions {positionName = "LT"},
                new Positions {positionName = "LG"},
                new Positions {positionName = "C"},
                new Positions {positionName = "RG"},
                new Positions {positionName = "RT"}
            };

            var players = new List<Player>
            {
                new Player { PlayerNum = 1, PlayerName = "Mike Evans" },
                new Player { PlayerNum = 2, PlayerName = "Ali Marpet" },
                new Player { PlayerNum = 3, PlayerName = "Alex Cappa" },
                new Player { PlayerNum = 4, PlayerName = "Tom Brady" },
                new Player { PlayerNum = 5, PlayerName = "Josh Wells" },
                new Player { PlayerNum = 6, PlayerName = "Kyle Trask" },
                new Player { PlayerNum = 7, PlayerName = "Scott Miller" }
            };

            var playerPositions = new Dictionary<string, List<Player>>
            {
                { allPositions[0].positionName, new List<Player> { players[0], players[1] } },
                { allPositions[1].positionName, new List<Player> { players[1] } },
                { allPositions[2].positionName, new List<Player> { players[2], players[3] } },
                { allPositions[3].positionName, new List<Player> { players[4] } },
                { allPositions[4].positionName, new List<Player> { players[3], players[4] } },
                { allPositions[5].positionName, new List<Player> { players[5] } },
                { allPositions[6].positionName, new List<Player> { players[6] } }
            };

            var league = new League
            {
                Id = Guid.NewGuid(),
                Name = "NFL",
                Teams = new List<Team>
                {
                    new Team {Id =  Guid.NewGuid(), Name = "Tampa Bay Buccaneer", Players = players}
                }
            };

            return playerPositions;
        }
    }
}
