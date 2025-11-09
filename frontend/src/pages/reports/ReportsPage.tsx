import { useState, useEffect } from 'react';
import { reportService, type SalesReport } from '../../api/reportService';
import { LineChart, Line, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';
import LoadingSpinner from '../../components/LoadingSpinner';
import { getErrorMessage } from '../../utils/errorHandler';
import './Reports.css';

const ReportsPage = () => {
  const [report, setReport] = useState<SalesReport | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [startDate, setStartDate] = useState(() => {
    const date = new Date();
    date.setMonth(date.getMonth() - 1);
    return date.toISOString().split('T')[0];
  });
  const [endDate, setEndDate] = useState(() => {
    return new Date().toISOString().split('T')[0];
  });

  const fetchReport = async () => {
    setLoading(true);
    setError('');
    try {
      const start = new Date(startDate);
      const end = new Date(endDate);
      end.setHours(23, 59, 59, 999);
      const data = await reportService.getSalesReport(start, end);
      setReport(data);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchReport();
  }, []);

  const handleDateChange = () => {
    if (new Date(startDate) <= new Date(endDate)) {
      fetchReport();
    }
  };

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('es-ES', {
      style: 'currency',
      currency: 'COP',
      minimumFractionDigits: 0
    }).format(value);
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', { day: 'numeric', month: 'short' });
  };

  if (loading) {
    return <LoadingSpinner />;
  }

  if (error) {
    return <div className="error-container">{error}</div>;
  }

  if (!report) {
    return <div className="error-container">No hay datos disponibles</div>;
  }

  return (
    <div className="reports-page">
      <div className="page-header">
        <h1>Reporte de Ventas</h1>
        <div className="date-filters">
          <div className="date-input-group">
            <label htmlFor="startDate">Desde:</label>
            <input
              id="startDate"
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              onBlur={handleDateChange}
            />
          </div>
          <div className="date-input-group">
            <label htmlFor="endDate">Hasta:</label>
            <input
              id="endDate"
              type="date"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              onBlur={handleDateChange}
            />
          </div>
          <button onClick={fetchReport} className="btn-primary">
            Actualizar
          </button>
        </div>
      </div>

      <div className="metrics-grid">
        <div className="metric-card">
          <div className="metric-label">Total de Ventas</div>
          <div className="metric-value">{report.totalSales}</div>
        </div>
        <div className="metric-card">
          <div className="metric-label">Ingresos Totales</div>
          <div className="metric-value">{formatCurrency(report.totalRevenue)}</div>
        </div>
        <div className="metric-card">
          <div className="metric-label">Promedio por Orden</div>
          <div className="metric-value">{formatCurrency(report.averageTicket)}</div>
        </div>
      </div>

      <div className="charts-grid">
        <div className="chart-card">
          <h2>Ventas Diarias</h2>
          <ResponsiveContainer width="100%" height={300}>
            <LineChart data={report.dailySummary} margin={{left: 45}}>
              <CartesianGrid strokeDasharray="3 3" stroke="#334155" />
              <XAxis
                dataKey="date"
                stroke="#94a3b8"
                tickFormatter={formatDate}
              />
              <YAxis stroke="#94a3b8" scale="auto" />
              <Tooltip
                contentStyle={{
                  backgroundColor: '#1e293b',
                  border: '1px solid #334155',
                  color: '#e2e8f0',
                }}
                labelFormatter={(value) => formatDate(value)}
                formatter={(value: number) => formatCurrency(value)}
              />
              <Legend />
              <Line
                type="monotone"
                dataKey="totalRevenue"
                stroke="#3b82f6"
                strokeWidth={2}
                name="Ingresos"
              />
              <Line
                type="monotone"
                dataKey="totalSales"
                stroke="#10b981"
                strokeWidth={2}
                name="Ventas"
              />
            </LineChart>
          </ResponsiveContainer>
        </div>

        <div className="chart-card">
          <h2>Top 10 Productos</h2>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={report.topProducts.slice(0, 10)} margin={{left: 35}}>
              <CartesianGrid strokeDasharray="3 3" stroke="#334155" />
              <XAxis
                dataKey="productName"
                stroke="#94a3b8"
                angle={-45}
                textAnchor="end"
                height={100}
              />
              <YAxis stroke="#94a3b8" />
              <Tooltip
                contentStyle={{
                  backgroundColor: '#1e293b',
                  border: '1px solid #334155',
                  color: '#e2e8f0',
                }}
                formatter={(value: number) => formatCurrency(value)}
              />
              <Legend />
              <Bar dataKey="revenue" fill="#3b82f6" name="Ingresos" />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </div>

      <div className="summary-table-card">
        <h2>Resumen Diario</h2>
        <div className="table-container">
          <table className="summary-table">
            <thead>
              <tr>
                <th>Fecha</th>
                <th>Ventas</th>
                <th>Ingresos</th>
                <th>Productos Vendidos</th>
              </tr>
            </thead>
            <tbody>
              {report.dailySummary.map((day, index) => (
                <tr key={index}>
                  <td>{formatDate(day.date)}</td>
                  <td>{day.totalSales}</td>
                  <td>{formatCurrency(day.totalRevenue)}</td>
                  <td>{day.totalProducts}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default ReportsPage;

