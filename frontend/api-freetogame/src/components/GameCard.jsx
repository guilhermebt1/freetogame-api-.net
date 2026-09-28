function GameCard({ game }) {
  return (
    <div>
      <h3>{game.title}</h3>
      <img src={game.thumbnail} alt={game.title} />
      <h3>{game.genre}</h3>
      <h3>{game.platform}</h3>
      <h3>{game.publisher}</h3>
    </div>
  );
}

export default GameCard;