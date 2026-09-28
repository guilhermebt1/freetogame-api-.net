import { useState } from "react";
import { getGamesByCategory } from "./services/gameService";
import SearchBar from "./components/SearchBar";
import GameList from "./components/GameList";
import LoadingSpinner from "./components/LoadingSpinner";
import "./App.css";

function App() {
  const [category, setCategory] = useState("");
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  async function handleSearch(newCategory) {
    setCategory(newCategory);
    setLoading(true);
    setError(null);

    try {
      const result = await getGamesByCategory(newCategory);
      setGames(result);
    } catch (err) {
      setError("Não foi possível buscar os jogos. Tente novamente.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <>
      <SearchBar onSearch={handleSearch} />

      {loading && <LoadingSpinner />}

      {!loading && error && <p>{error}</p>}

      {!loading && !error && games.length === 0 && (
        <p>Nenhum resultado encontrado.</p>
      )}

      {!loading && !error && games.length > 0 && <GameList games={games} />}
    </>
  );
}

export default App;
