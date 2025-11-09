import { type ReactNode } from 'react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import './Layout.css';

interface LayoutProps {
  children: ReactNode;
}

const Layout = ({ children }: LayoutProps) => {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  // Función para determinar si un link está activo
  const isActive = (path: string) => {
    if (path === '/products') {
      return location.pathname === '/products';
    }
    if (path === '/sales') {
      return location.pathname === '/sales' || 
             location.pathname.startsWith('/sales/');
    }
    if (path === '/reports') {
      return location.pathname === '/reports';
    }
    return false;
  };

  return (
    <div className="layout">
      <nav className="navbar">
        <div className="nav-brand">
          <h2>Papulandia Store</h2>
        </div>
        <div className="nav-links">
          <Link 
            to="/products" 
            className={isActive('/products') ? 'active' : ''}
          >
            Productos
          </Link>
          <Link 
            to="/sales" 
            className={isActive('/sales') ? 'active' : ''}
          >
            Ventas
          </Link>
          <Link 
            to="/reports" 
            className={isActive('/reports') ? 'active' : ''}
          >
            Reportes
          </Link>
        </div>
        <div className="nav-user">
          <div className="user-info">
            <svg 
              className="user-icon" 
              width="20" 
              height="20" 
              viewBox="0 0 24 24" 
              fill="none" 
              stroke="currentColor" 
              strokeWidth="2"
            >
              <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
              <circle cx="12" cy="7" r="4"></circle>
            </svg>
            <span className="user-name">{user?.name}</span>
          </div>
          <button onClick={handleLogout} className="btn-logout">
            Cerrar Sesión
          </button>
        </div>
      </nav>
      <main className="main-content">
        {children}
      </main>
    </div>
  );
};

export default Layout;

