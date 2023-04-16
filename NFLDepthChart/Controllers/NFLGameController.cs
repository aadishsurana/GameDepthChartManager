using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFLDepthChart.Domain;
using NFLDepthChart.Domain.Requests;
using NFLDepthChart.Services;

namespace NFLDepthChart.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class NFLGameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public NFLGameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet(Name = "GetFullDepthChart")]
        public async Task<ActionResult<string>> GetFullDepthChart()
        {
            var depthChart = await _gameService.GetFullDepthChart();
            return depthChart;
        }

        [HttpPost]
        public async Task<IActionResult> AddPlayer([FromBody] AddPlayerRequest request)
        {
            var playerToAdd = new Player { PlayerNum = request.PlayerNumber, PlayerName = request.PlayerName };
            var addedPlayerResponse = await _gameService.AddPlayerToChart(request.Position, playerToAdd, request.Depth);

            if(addedPlayerResponse == null)
            {
                return BadRequest();
            }

            return new OkObjectResult(addedPlayerResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePlayer([FromBody] DeletePlayerRequest request)
        {
            var playerToDelete = new Player { PlayerNum = request.PlayerNumber, PlayerName = request.PlayerName };
            string playerDeleted = await _gameService.RemovePlayerFromChart(request.Position, playerToDelete);
            return Ok(playerDeleted);
        }

        [HttpGet(Name ="GetBackupPlayers")]
        public async Task<ActionResult<string>> GetBackups(string position, [FromQuery]  Player playerBackup)
        {
            var backupPlayers = await _gameService.GetBackupPlayers(position, playerBackup);
            return backupPlayers;
        }
    }
}
