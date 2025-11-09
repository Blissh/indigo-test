import { useState } from 'react';
import { useSales } from '../../hooks/useSales';
import { saleService } from '../../api/saleService';
import { useNavigate } from 'react-router-dom';
import LoadingSpinner from '../../components/LoadingSpinner';
import { getErrorMessage } from '../../utils/errorHandler';
import '../Sales.css';

const SalesPage = () => {
  const { sales, loading, error, refetch } = useSales();
  const [deletingId, setDeletingId] = useState<number | null>(null);
  const navigate = useNavigate();

  const handleDelete = async (id: number) => {
    if (!confirm('¿Estás seguro de que quieres eliminar esta venta?')) {
      return;
    }

    setDeletingId(id);
    try {
      await saleService.delete(id);
      refetch();
    } catch (err) {
      alert(getErrorMessage(err));
    } finally {
      setDeletingId(null);
    }
  };

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

  const handleCreateSale = () => {
    navigate('/sales/create');
  };

  return (
    <div className="sales-page">
      {error && sales.length > 0 && (
        <div className="error-container">{error}</div>
      )}

      {sales.length === 0 ? (
        <div className="sales-grid">
          {/* Card para crear nueva venta - visible cuando no hay ventas */}
          <div className="sale-card create-sale-card" onClick={handleCreateSale}>
            <div className="create-sale-content">
              <div className="create-sale-icon">+</div>
              <h3>Crear Primera Venta</h3>
              <p>No hay ventas registradas</p>
            </div>
          </div>
        </div>
      ) : (
        <>
          <div className="page-header">
            <button onClick={handleCreateSale} className="btn-primary">
              + Nueva Venta
            </button>
          </div>

          <div className="sales-table-container">
            <table className="sales-table">
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Fecha</th>
                  <th>Total</th>
                  <th>Items</th>
                  <th>Acciones</th>
                </tr>
              </thead>
              <tbody>
                {sales.map((sale) => (
                  <tr key={sale.id}>
                    <td>#{sale.id}</td>
                    <td>{formatDate(sale.date)}</td>
                    <td className="total-cell">${sale.total.toFixed(2)}</td>
                    <td>{sale.items.length} producto(s)</td>
                    <td>
                      <div className="table-actions">
                        <button
                          onClick={() => navigate(`/sales/${sale.id}`)}
                          className="btn-view"
                        >
                          Ver
                        </button>
                        <button
                          onClick={() => handleDelete(sale.id)}
                          disabled={deletingId === sale.id}
                          className="btn-delete"
                        >
                          {deletingId === sale.id ? 'Eliminando...' : 'Eliminar'}
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </>
      )}
    </div>
  );
};

export default SalesPage;

