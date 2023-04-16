using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NFLDepthChart.Domain;
using NFLDepthChart.Repositories;
using NFLDepthChart.Services;

namespace NFLDepthChart.Tests
{
    public class GameServiceTests
    {
        private readonly IFixture _fixture;
        private IGameService _sut;

        public GameServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

        }

        [Fact]
        public async Task Test_GetFullDepthChart()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);

            //Act
            string expectedChart = @"LWR: [1-Mike Evans,2-Ali Marpet]
RWR: [2-Ali Marpet]
LT: [3-Alex Cappa,4-Tom Brady]
LG: [5-Josh Wells]
C: [4-Tom Brady,5-Josh Wells]
RG: [6-Kyle Trask]
RT: [7-Scott Miller]";

            var responseChart = await _sut.GetFullDepthChart();

            //Assert
            Assert.Contains(expectedChart, responseChart);
        }

        [Fact]
        public async Task Test_AddPlayerTo_LastPosition_When_DepthNotSpecified()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "RWR";
            var playerToAdd = _fixture.Create<Player>();

            //Act
            var newTeam = await _sut.AddPlayerToChart(position, playerToAdd);
            var newPlayerAdded = newTeam[position].Last();

            //Assert
            Assert.True(originalTeam[position].Last() != newPlayerAdded);
            Assert.True(originalTeam[position].Count() < newTeam[position].Count());
            Assert.True(newPlayerAdded.PlayerName == playerToAdd.PlayerName);
        }

        [Fact]
        public async Task Test_AddPlayerTo_SpecifiedPosition_When_DepthSpecified()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "LT";
            var playerToAdd = _fixture.Create<Player>();

            //Act
            var newTeam = await _sut.AddPlayerToChart(position, playerToAdd, 1);
            var newPlayerAdded = newTeam[position][1];

            //Assert
            Assert.True(originalTeam[position].Last() != newPlayerAdded);
            Assert.True(originalTeam[position].Count() < newTeam[position].Count());
            Assert.True(newPlayerAdded.PlayerName == playerToAdd.PlayerName);
            Assert.True(newTeam[position].Count() == 3);
        }

        [Fact]
        public async Task Test_DontAddPlayer_IfPosition_Invalid()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "LTR";
            var playerToAdd = _fixture.Create<Player>();

            //Act
            var newTeam = await _sut.AddPlayerToChart(position, playerToAdd, 1);

            //Assert
            Assert.Equivalent(originalTeam, newTeam);
            Assert.Equal(originalTeam.Count(), newTeam.Count());
        }

        [Fact]
        public async Task Test_DontAddPlayer_InSamePosition_IfSamePlayerAlreadyExists()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "LT";
            var playerToAdd = new Player { PlayerNum=4, PlayerName="Tom Brady"};

            //Act
            var newTeam = await _sut.AddPlayerToChart(position, playerToAdd, 0);

            //Assert
            Assert.Equal(originalTeam[position].Count(), newTeam[position].Count());
        }

        [Fact]
        public async Task Test_RemovePlayer_FromSpecifiedPosition()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "LT";
            var playerToRemove = new Player { PlayerNum = 4, PlayerName = "Tom Brady" };

            //Act
            var newTeam = await _sut.RawDepthChart();
            var playerRemoved = await _sut.RemovePlayerFromChart(position, playerToRemove);

            //Assert
            Assert.NotEqual(originalTeam[position].Count(), newTeam[position].Count());
            Assert.True(originalTeam[position].Count() > newTeam[position].Count());
            Assert.Contains(playerToRemove.PlayerName, playerRemoved);
            Assert.Equal("4 - Tom Brady", playerRemoved);
        }

        [Fact]
        public async Task Test_RemovePlayer_ReturnsEmptyString_WhenPosition_IsInvalid()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "LTR";
            var playerToRemove = new Player { PlayerNum = 4, PlayerName = "Tom Brady" };

            //Act
            var newTeam = await _sut.RawDepthChart();
            var playerRemoved = await _sut.RemovePlayerFromChart(position, playerToRemove);

            //Assert
            Assert.Equal("No List", playerRemoved);
            Assert.Equal(originalTeam.Count(), newTeam.Count());
        }

        [Fact]
        public async Task Test_RemovePlayer_ReturnsEmptyString_WhenPlayerIsInvalid()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "LT";
            var playerToRemove = new Player { PlayerNum = 5, PlayerName = "Josh Wells" };

            //Act
            var newTeam = await _sut.RawDepthChart();
            var playerRemoved = await _sut.RemovePlayerFromChart(position, playerToRemove);

            //Assert
            Assert.Equal("No List", playerRemoved);
            Assert.Equal(originalTeam.Count(), newTeam.Count());
        }

        [Fact]
        public async Task Test_GetBackupPlayers_ForSpecifiedPlayer()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "LT";
            var currentPlayer = new Player { PlayerNum = 3, PlayerName = "Alex Cappa" };

            //Act
            var backupPlayers = await _sut.GetBackupPlayers(position, currentPlayer);

            //Assert
            Assert.Contains("4-Tom Brady", backupPlayers);
        }

        [Fact]
        public async Task Test_GetBackupPlayers_ReturnsEmptyString_WhenSpecifiedPlayer_LastInList()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "RWR";
            var currentPlayer = new Player { PlayerNum = 2, PlayerName = "Ali Marpet" };

            //Act
            var backupPlayers = await _sut.GetBackupPlayers(position, currentPlayer);

            //Assert
            Assert.Contains("No List", backupPlayers);
        }

        [Fact]
        public async Task Test_GetBackupPlayers_ReturnsEmptyString_WhenPosition_IsInvalid()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "XX";
            var currentPlayer = new Player { PlayerNum = 2, PlayerName = "Ali Marpet" };

            //Act
            var backupPlayers = await _sut.GetBackupPlayers(position, currentPlayer);

            //Assert
            Assert.Contains("No List", backupPlayers);
        }

        [Fact]
        public async Task Test_GetBackupPlayers_ReturnsEmptyString_WhenPlayer_IsInvalid()
        {
            //Arrange
            var gameRepo = new Mock<GameRepository>();
            _sut = new GameService(gameRepo.Object);
            var originalTeam = BuildNFLTeam.BuildTeam();

            string position = "RWR";
            var currentPlayer = new Player { PlayerNum = 23, PlayerName = "XX" };

            //Act
            var backupPlayers = await _sut.GetBackupPlayers(position, currentPlayer);

            //Assert
            Assert.Contains("No List", backupPlayers);
        }
    }
}