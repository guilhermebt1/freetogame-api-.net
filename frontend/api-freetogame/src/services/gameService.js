async function getGamesByCategory(category) {

  const categoryEscape = encodeURIComponent(category);
  const url = `${import.meta.env.VITE_API_URL}/api/games?category=${categoryEscape}`;

  const response = await fetch(url);

  if (!response.ok) {
    throw new Error(`Erro na busca: ${response.status}`);
  }

  const games = await response.json(); 
  return games;
}

export { getGamesByCategory };