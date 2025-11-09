import { useState } from "react";
import { useProducts } from "../../hooks/useProducts";
import { productService, type Product } from "../../api/productService";
import ProductForm from "./ProductForm";
import LoadingSpinner from "../../components/LoadingSpinner";
import { getErrorMessage } from "../../utils/errorHandler";
import "../Products.css";

const ProductsPage = () => {
  const { products, loading, error, refetch } = useProducts();
  const [showForm, setShowForm] = useState(false);
  const [editingProduct, setEditingProduct] = useState<Product | null>(null);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  const handleCreate = () => {
    setEditingProduct(null);
    setShowForm(true);
  };

  const handleEdit = (product: Product) => {
    setEditingProduct(product);
    setShowForm(true);
  };

  const handleDelete = async (id: number) => {
    if (!confirm("¿Estás seguro de que quieres eliminar este producto?")) {
      return;
    }

    setDeletingId(id);
    try {
      await productService.delete(id);
      refetch();
    } catch (err) {
      alert(getErrorMessage(err));
    } finally {
      setDeletingId(null);
    }
  };

  const handleFormClose = () => {
    setShowForm(false);
    setEditingProduct(null);
    refetch();
  };

  if (loading) {
    return <LoadingSpinner />;
  }

  return (
    <div className="products-page">
      {showForm && (
        <ProductForm
          product={editingProduct}
          onClose={handleFormClose}
          onSuccess={handleFormClose}
        />
      )}

      {error && products.length > 0 && (
        <div className="error-container">{error}</div>
      )}

      <div className="products-grid">
        {/* Card para crear nuevo producto - siempre visible */}
        <div className="product-card create-product-card" onClick={handleCreate}>
          <div className="create-product-content">
            <div className="create-product-icon">+</div>
            <h3>
              {products.length === 0
                ? "Crear Primer Producto"
                : "Nuevo Producto"}
            </h3>
            {products.length === 0 && (
              <p>No hay productos registrados</p>
            )}
          </div>
        </div>

        {/* Lista de productos existentes */}
        {products.map((product) => (
          <div key={product.id} className="product-card">
            {product.image && (
              <div className="product-image">
                <img src={product.image} alt={product.name} />
              </div>
            )}
            <div className="product-info">
              <h3>{product.name}</h3>
              <p className="product-description">{product.description}</p>
              <div className="product-details">
                <div className="detail-item">
                  <span className="label">Precio:</span>
                  <span className="value">${product.price.toFixed(2)}</span>
                </div>
                <div className="detail-item">
                  <span className="label">Stock:</span>
                  <span
                    className={`value ${
                      product.stock === 0 ? "out-of-stock" : ""
                    }`}
                  >
                    {product.stock}
                  </span>
                </div>
              </div>
              <div className="product-actions">
                <button
                  onClick={() => handleEdit(product)}
                  className="btn-edit"
                >
                  Editar
                </button>
                <button
                  onClick={() => handleDelete(product.id)}
                  disabled={deletingId === product.id}
                  className="btn-delete"
                >
                  {deletingId === product.id ? "Eliminando..." : "Eliminar"}
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ProductsPage;
