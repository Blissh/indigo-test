import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { saleService, type Sale } from '../../api/saleService';
import LoadingSpinner from '../../components/LoadingSpinner';
import { getErrorMessage } from '../../utils/errorHandler';
import './SaleDetail.css';

const SaleDetailPage = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [sale, setSale] = useState<Sale | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchSale = async () => {
      if (!id) return;

      try {
        const data = await saleService.getById(parseInt(id));
        setSale(data);
      } catch (err) {
        setError(getErrorMessage(err));
      } finally {
        setLoading(false);
      }
    };

    fetchSale();
  }, [id]);

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  if (loading) {
    return <LoadingSpinner />;
  }

  if (error || !sale) {
    return (
      <div className="error-container">
        {error || 'Venta no encontrada'}
      </div>
    );
  }

  return (
    <div className="sale-detail-page">
      <div className="page-header">
        <h1>Detalle de Venta #{sale.id}</h1>
        <button onClick={() => navigate('/sales')} className="btn-secondary">
          Volver
        </button>
      </div>

      <div className="sale-detail-card">
        <div className="sale-info">
          <div className="info-item">
            <span className="info-label">Fecha:</span>
            <span className="info-value">{formatDate(sale.date)}</span>
          </div>
          <div className="info-item">
            <span className="info-label">Total:</span>
            <span className="info-value total">${sale.total.toFixed(2)}</span>
          </div>
        </div>

        <div className="sale-items">
          <h2>Productos</h2>
          <table className="items-table">
            <thead>
              <tr>
                <th>Producto</th>
                <th>Cantidad</th>
                <th>Precio Unitario</th>
                <th>Subtotal</th>
              </tr>
            </thead>
            <tbody>
              {sale.items.map((item) => (
                <tr key={item.id}>
                  <td>{item.productName}</td>
                  <td>{item.quantity}</td>
                  <td>${item.price.toFixed(2)}</td>
                  <td>${item.total.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default SaleDetailPage;

