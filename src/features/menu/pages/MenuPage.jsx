import { useState } from "react";
import { useDebounce } from "../hooks/useDebounce";
import { useFetch } from "../hooks/useFetch";
import MenuList from "../components/MenuList";

function MenuPage() {
  const [query, setQuery] = useState("");
  const debounced = useDebounce(query, 400);

  const apiUrl = import.meta.env.VITE_API_URL
    ? `${import.meta.env.VITE_API_URL}/api/menu`
    : "/menu.json";
  const { data: dishes, isLoading, error } = useFetch(apiUrl);

  if (isLoading) return <p>Loading menu...</p>;
  if (error) return <p>Could not load menu: {error}</p>;

  const filtered = dishes.filter((d) =>
    d.name.toLowerCase().includes(debounced.toLowerCase()),
  );

  return (
    <div className="page">
      <h2 className="page-title">Our Menu</h2>
      <input
        className="search-input"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
        placeholder="Search dishes..."
      />

      <MenuList dishes={filtered} />
    </div>
  );
}

export default MenuPage;
