import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { saleService, type CreateSaleItemRequest } from '../../api/saleService';
import { productService, type Product } from '../../api/productService';
import LoadingSpinner from '../../components/LoadingSpinner';
import { getErrorMessage } from '../../utils/errorHandler';
import './CreateSale.css';

const CreateSalePage = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [selectedItems, setSelectedItems] = useState<Map<number, number>>(new Map());
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const data = await productService.getAll();
        setProducts(data.filter(p => p.stock > 0)); // Solo productos con stock
      } catch (err) {
        setError(getErrorMessage(err));
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

  const handleQuantityChange = (productId: number, quantity: number) => {
    const newItems = new Map(selectedItems);
    if (quantity > 0) {
      newItems.set(productId, quantity);
    } else {
      newItems.delete(productId);
    }
    setSelectedItems(newItems);
  };

  const getProductQuantity = (productId: number) => {
    return selectedItems.get(productId) || 0;
  };

  const getTotal = () => {
    let total = 0;
    selectedItems.forEach((quantity, productId) => {
      const product = products.find(p => p.id === productId);
      if (product) {
        total += product.price * quantity;
      }
    });
    return total;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (selectedItems.size === 0) {
      setError('Debes seleccionar al menos un producto');
      return;
    }

    setSaving(true);
    setError('');

    try {
      const items: CreateSaleItemRequest[] = Array.from(selectedItems.entries()).map(
        ([productId, quantity]) => ({
          productId,
          quantity,
        })
      );

      await saleService.create({ items });
      navigate('/sales');
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return <LoadingSpinner />;
  }

  return (
    <div className="create-sale-page">
      <div className="page-header">
        <h1>Nueva Venta</h1>
        <button onClick={() => navigate('/sales')} className="btn-secondary">
          Cancelar
        </button>
      </div>

      <form onSubmit={handleSubmit} className="sale-form">
        <div className="products-selection">
          <h2>Seleccionar Productos</h2>
          {products.length === 0 ? (
            <div className="empty-state">
              <p>No hay productos disponibles con stock</p>
            </div>
          ) : (
            <div className="products-list">
              {products.map((product) => (
                <div key={product.id} className="product-select-item">
                  <div className="product-select-info">
                    {product.image && (
                      <img src={product.image} alt={product.name} className="product-select-image" />
                    )}
                    <div>
                      <h3>{product.name}</h3>
                      <p className="product-price">${product.price.toFixed(2)}</p>
                      <p className="product-stock">Stock disponible: {product.stock}</p>
                    </div>
                  </div>
                  <div className="quantity-control">
                    <button
                      type="button"
                      onClick={() => {
                        const current = getProductQuantity(product.id);
                        if (current > 0) {
                          handleQuantityChange(product.id, current - 1);
                        }
                      }}
                      disabled={getProductQuantity(product.id) === 0}
                      className="qty-btn"
                    >
                      -
                    </button>
                    <input
                      type="number"
                      min="0"
                      max={product.stock}
                      value={getProductQuantity(product.id)}
                      onChange={(e) => {
                        const value = parseInt(e.target.value) || 0;
                        if (value <= product.stock) {
                          handleQuantityChange(product.id, value);
                        }
                      }}
                      className="qty-input"
                    />
                    <button
                      type="button"
                      onClick={() => {
                        const current = getProductQuantity(product.id);
                        if (current < product.stock) {
                          handleQuantityChange(product.id, current + 1);
                        }
                      }}
                      disabled={getProductQuantity(product.id) >= product.stock}
                      className="qty-btn"
                    >
                      +
                    </button>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        <div className="sale-summary">
          <h2>Resumen</h2>
          <div className="summary-items">
            {Array.from(selectedItems.entries()).map(([productId, quantity]) => {
              const product = products.find(p => p.id === productId);
              if (!product) return null;
              return (
                <div key={productId} className="summary-item">
                  <span>{product.name} x{quantity}</span>
                  <span>${(product.price * quantity).toFixed(2)}</span>
                </div>
              );
            })}
          </div>
          <div className="summary-total">
            <span>Total:</span>
            <span>${getTotal().toFixed(2)}</span>
          </div>
        </div>

        {error && <div className="error-message">{error}</div>}

        <div className="form-actions">
          <button type="submit" disabled={saving || selectedItems.size === 0} className="btn-primary">
            {saving ? 'Guardando...' : 'Crear Venta'}
          </button>
        </div>
      </form>
    </div>
  );
};

export default CreateSalePage;

