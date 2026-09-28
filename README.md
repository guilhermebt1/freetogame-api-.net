# FreeToGame Finder 🎮

Aplicação full stack para buscar jogos gratuitos por categoria, usando a [FreeToGame API](https://www.freetogame.com/api/games). O projeto integra um frontend em React com uma API REST em ASP.NET Core, que consulta o serviço externo e devolve ao frontend somente os dados necessários para exibir os jogos.

![Preview da aplicação FreeToGame Finder](https://i.ibb.co/DPYN2y0F/APIGames.png)

Este repositório faz parte do meu aprendizado e portfólio com o ecossistema .NET. O foco é demonstrar integração com API externa, DTOs, injeção de dependência, `IHttpClientFactory`, configuração via `appsettings`, CORS e deploy full stack.

**Acesso pelo link do GitHub Pages** https://guilhermebt1.github.io/freetogame-api-.net/

## O que a aplicação faz hoje

- Busca jogos gratuitos a partir de uma categoria, como `shooter`, `mmorpg` ou `strategy`. SOMENTE EM INGLÊS
- Consulta a FreeToGame API sem expor diretamente o contrato externo ao frontend.
- Exibe os jogos retornados em cartões, com título, capa, gênero, plataforma e publicadora.
- Exibe estados de carregamento, erro e lista vazia na interface.
- Trata termos vazios com uma resposta `400 Bad Request`.

## Tecnologias e por quê

| Camada | Tecnologia | Motivo da escolha |
|---|---|---|
| Frontend | React 19 + Vite | Interface reativa e leve para praticar componentes e consumo de API com `fetch`. |
| Backend | ASP.NET Core 10 | Plataforma para criar a API REST, com injeção de dependência e middleware. |
| Integração externa | `IHttpClientFactory` | Centraliza a comunicação assíncrona com a FreeToGame API em um cliente nomeado. |
| Contratos | DTOs com `JsonPropertyName` | Mapeia o JSON da API externa diretamente para um contrato próprio da aplicação. |
| Documentação local | OpenAPI + Scalar | Interface para explorar a API durante o desenvolvimento. |
| Deploy | GitHub Pages + Render | Hospedagem do frontend React e da API .NET containerizada. |

## Como as partes se conectam

```text
GitHub Pages (React + Vite)
              │ fetch / JSON
              ▼
API ASP.NET Core no Render
              │
              │ IHttpClientFactory (cliente nomeado "FreeToGame")
              ▼
FreeToGame API
```

O controller recebe a requisição do frontend. O `FreeToGameService` chama a API externa usando o cliente nomeado configurado no `Program.cs` e trata falhas de rede e de desserialização. O `GameDto` representa o contrato da aplicação, mapeado por atributos para o formato do JSON externo.

## Estrutura do repositório

```text
frontend/api-freetogame/            # frontend React + Vite
  src/
    components/                   # SearchBar, GameList, GameCard, LoadingSpinner
    services/gameService.js       # chamadas fetch à API
    App.jsx                       # estado da busca e composição da tela
  dist/                           # build publicado no GitHub Pages
  vite.config.js                  # base do deploy no GitHub Pages

backend/api-freetogame/           # API ASP.NET Core
  Controllers/                    # endpoints HTTP
  DTOs/                           # GameDto (contrato interno mapeado do externo)
  Interfaces/                     # IFreeToGameService
  Services/                       # FreeToGameService (cliente da API externa)
  appsettings.json                # configuração da BaseUrl da FreeToGame
  Dockerfile                      # imagem usada pelo Render
```

## Decisões de implementação

### 1. Centralizar a configuração da API externa

A URL base da FreeToGame fica no `appsettings.json`, sob a chave `FreeToGameApi:BaseUrl`, e é lida na hora de registrar o cliente HTTP. Se a configuração estiver ausente, a aplicação falha rápido na inicialização com uma exceção clara, em vez de quebrar silenciosamente na primeira requisição.

```csharp
builder.Services.AddHttpClient("FreeToGame", client =>
{
    string? baseUrl = builder.Configuration.GetValue<string>("FreeToGameApi:BaseUrl");
    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException("A configuração 'FreeToGameApi:BaseUrl' não foi encontrada ou está vazia.");
    }
    client.BaseAddress = new Uri(baseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
```

### 2. Mapear o contrato externo com `JsonPropertyName`

O `GameDto` usa atributos para mapear os nomes do JSON da FreeToGame (`short_description`, `game_url`, `release_date`) em propriedades em PascalCase, sem precisar de código de conversão manual.

```text
FreeToGame API → GameDto → FreeToGameService → Frontend
```

### 3. Depender da interface, não da implementação

O `FreeToGameService` implementa `IFreeToGameService` e é registrado com `AddScoped`. O `GamesController` injeta a interface, o que facilita trocar ou testar a implementação sem alterar o controller.

### 4. Tratar falhas externas sem derrubar a requisição

O serviço captura `HttpRequestException` (falha de rede ou status de erro) e `JsonException` (resposta inesperada), loga no console e retorna uma lista vazia. O frontend, por sua vez, diferencia erro de busca de lista vazia e exibe a mensagem adequada para cada caso.

### 5. Limitar origens permitidas com CORS

O frontend roda em domínio diferente da API. Por isso, o backend libera somente as origens do Vite em desenvolvimento (`5173`) e o GitHub Pages, em vez de usar `AllowAnyOrigin()`.

```csharp
policy.WithOrigins(
        "http://127.0.0.1:5173",
        "http://localhost:5173",
        "https://guilhermebt1.github.io")
    .WithMethods("GET")
    .AllowAnyHeader();
```

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/api/games?category={categoria}` | Retorna os jogos gratuitos da categoria informada. |

Exemplo:

```text
GET /api/games?category=shooter
```

## Como rodar localmente

Backend (porta configurada pelo `launchSettings.json`):

```bash
cd backend/api-freetogame/api-freetogame
dotnet run
```

Frontend (necessário um arquivo `.env` com `VITE_API_URL` apontando para a API):

```bash
cd frontend/api-freetogame
npm install
npm run dev
```

A documentação interativa (Scalar) fica disponível em `/scalar/v1` quando a API roda em ambiente de desenvolvimento.

## Meus próximos passos com o projeto

- Adicionar busca com múltiplas categorias e ordenação.
- Exibir mais detalhes do jogo em um modal ou página própria.
- Padronizar falhas externas com `ProblemDetails` em vez de lista vazia.
- Criar testes automatizados para o `FreeToGameService`.
- Adicionar cache para buscas repetidas.

## Aprendizados demonstrados 👨🏻‍💻

Com esse projeto exercitei conceitos como: consumo de API externa, operações assíncronas, DTOs com mapeamento por atributos, injeção de dependência, `IHttpClientFactory`, configuração via `appsettings`, camadas de serviço, CORS, tratamento de erros, Docker e deploy de uma aplicação full stack.

Desenvolvido como projeto de estudo e portfólio.
