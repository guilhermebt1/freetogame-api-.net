using System.Text.Json;
using api_freetogame.DTOs;
using api_freetogame.Interfaces;

namespace api_freetogame.Services;

public class FreeToGameService : IFreeToGameService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FreeToGameService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<GameDto>> GetGamesByCategoryAsync(string category)
        {
            var httpClient = _httpClientFactory.CreateClient("FreeToGame");
            string escapedCategory = Uri.EscapeDataString(category);

            try
            {
                var response = await httpClient.GetAsync($"games?category={escapedCategory}");
                response.EnsureSuccessStatusCode();
                List<GameDto> resultado = await response.Content.ReadFromJsonAsync<List<GameDto>>();
                return resultado;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Erro na requisição: {e.Message}");
                return new List<GameDto>(); 
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Erro ao deserializar o JSON: {e.Message}");
                return new List<GameDto>();
            }
        }
    }