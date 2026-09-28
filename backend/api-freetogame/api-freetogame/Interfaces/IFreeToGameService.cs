using api_freetogame.DTOs;

namespace api_freetogame.Interfaces;


public interface IFreeToGameService
{
    Task<List<GameDto>> GetGamesByCategoryAsync (string category);
}