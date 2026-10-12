import { useParams, Link } from "react-router-dom";

function DishDetailPage() {
  const { id } = useParams();    
  return (
    <div className="page">
      <h2 className="page-title">Dish Details</h2>
      <p>Showing details for dish #{id}</p>
      <Link className="back-link" to="/">
        Back to menu
      </Link>
    </div>
  );
}
export default DishDetailPage;