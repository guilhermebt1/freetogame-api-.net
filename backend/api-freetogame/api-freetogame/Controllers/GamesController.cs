using api_freetogame.DTOs;
using api_freetogame.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api_freetogame.Controllers;

[ApiController]
[Route("api/[controller]")]
//[controller] ajusta automaticamente para o nome da classe
public class GamesController : ControllerBase
{
    private readonly IFreeToGameService _freeToGameService;
    
    public GamesController(IFreeToGameService freeToGameService)
    {
        _freeToGameService = freeToGameService;
    }

    [HttpGet]
    public async Task<ActionResult<List<GameDto>>> GetGamesAsync([FromQuery] string category)
    {
        if (string.IsNullOrWhiteSpace(category))
        {
            return BadRequest("Busca Inválida");
        }
        category = category.Trim().ToLower();
        var result = await _freeToGameService.GetGamesByCategoryAsync(category);
        return Ok(result);
    }
}