import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import Layout from './components/Layout';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ProductsPage from './pages/products/ProductsPage';
import SalesPage from './pages/sales/SalesPage';
import CreateSalePage from './pages/sales/CreateSalePage';
import SaleDetailPage from './pages/sales/SaleDetailPage';
import ReportsPage from './pages/reports/ReportsPage';

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route
            path="/*"
            element={
              <ProtectedRoute>
                <Layout>
                  <Routes>
                    <Route path="/products" element={<ProductsPage />} />
                    <Route path="/sales" element={<SalesPage />} />
                    <Route path="/sales/create" element={<CreateSalePage />} />
                    <Route path="/sales/:id" element={<SaleDetailPage />} />
                    <Route path="/reports" element={<ReportsPage />} />
                    <Route path="/" element={<Navigate to="/products" replace />} />
                    <Route path="*" element={<Navigate to="/products" replace />} />
                  </Routes>
                </Layout>
              </ProtectedRoute>
            }
          />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;
