import { useState } from 'react';

function SearchBar({ onSearch, placeholder = 'Buscar jogo por categoria...' }) {
  const [category, setCategory] = useState('');

  const handleChange = (event) => {
    const value = event.target.value;
     setCategory(value);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    onSearch?.(category);
  };

  const handleClear = () => {
    setCategory('');
    onSearch?.('');
  };

  return (
    <form onSubmit={handleSubmit} style={{ display: 'flex', gap: '8px' }}>
      <input
        type="text"
        value={category}
        onChange={handleChange}
        placeholder={placeholder}
        aria-label="Campo de busca"
      />
      {category && (
        <button type="button" onClick={handleClear}>
          ✕
        </button>
      )}
      <button type="submit">Buscar</button>
    </form>
  );
}

export default SearchBar;