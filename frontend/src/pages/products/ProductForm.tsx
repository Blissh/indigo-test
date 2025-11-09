import { useState, useEffect } from 'react';
import { productService, type Product, type CreateProductRequest, type UpdateProductRequest } from '../../api/productService';
import ImageUpload from '../../components/ImageUpload';
import { getErrorMessage } from '../../utils/errorHandler';
import './ProductForm.css';

interface ProductFormProps {
  product: Product | null;
  onClose: () => void;
  onSuccess: () => void;
}

const ProductForm = ({ product, onClose, onSuccess }: ProductFormProps) => {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [price, setPrice] = useState('');
  const [stock, setStock] = useState('');
  const [imageUrl, setImageUrl] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (product) {
      setName(product.name);
      setDescription(product.description);
      setPrice(product.price.toString());
      setStock(product.stock.toString());
      setImageUrl(product.image);
    }
  }, [product]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const productData = {
        name,
        description,
        price: parseFloat(price),
        stock: parseInt(stock),
        image: imageUrl,
      };

      if (product) {
        await productService.update(product.id, productData as UpdateProductRequest);
      } else {
        await productService.create(productData as CreateProductRequest);
      }

      onSuccess();
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{product ? 'Editar Producto' : 'Nuevo Producto'}</h2>
          <button onClick={onClose} className="btn-close">×</button>
        </div>

        <form onSubmit={handleSubmit} className="product-form">
          <div className="form-group">
            <label htmlFor="name">Nombre *</label>
            <input
              id="name"
              type="text"
              value={name}
              onChange={(e) => setName(e.target.value)}
              required
              placeholder="Ej: Laptop Dell"
            />
          </div>

          <div className="form-group">
            <label htmlFor="description">Descripción</label>
            <textarea
              id="description"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              rows={4}
              placeholder="Descripción del producto"
            />
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="price">Precio *</label>
              <input
                id="price"
                type="number"
                step="0.01"
                min="0.01"
                value={price}
                onChange={(e) => setPrice(e.target.value)}
                required
                placeholder="0.00"
              />
            </div>

            <div className="form-group">
              <label htmlFor="stock">Stock *</label>
              <input
                id="stock"
                type="number"
                min="0"
                value={stock}
                onChange={(e) => setStock(e.target.value)}
                required
                placeholder="0"
              />
            </div>
          </div>

          <div className="form-group">
            <label>Imagen</label>
            {product ? (
              <div className="image-display">
                {imageUrl && (
                  <div className="current-image">
                    <img src={imageUrl} alt="Producto" />
                    <p className="image-note">La imagen no se puede modificar</p>
                  </div>
                )}
              </div>
            ) : (
              <ImageUpload
                onImageUploaded={setImageUrl}
                disabled={loading}
              />
            )}
            {imageUrl && !product && (
              <div className="image-preview-url">
                <img src={imageUrl} alt="Preview" />
              </div>
            )}
          </div>

          {error && <div className="error-message">{error}</div>}

          <div className="form-actions">
            <button type="button" onClick={onClose} className="btn-secondary">
              Cancelar
            </button>
            <button type="submit" disabled={loading} className="btn-primary">
              {loading ? 'Guardando...' : product ? 'Actualizar' : 'Crear'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default ProductForm;

