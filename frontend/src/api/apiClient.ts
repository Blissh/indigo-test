import axios, { AxiosError, type InternalAxiosRequestConfig } from 'axios';

// En producción (Docker), usar ruta relativa que será proxeada por nginx
// En desarrollo, usar la variable de entorno o el valor por defecto
const API_URL = import.meta.env.VITE_API_URL || (import.meta.env.PROD ? '/api/v1' : 'http://localhost:5000/api/v1');

const apiClient = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor para agregar token JWT a las peticiones
apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = localStorage.getItem('token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Interceptor para manejar errores de respuesta
apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    // Si hay un error de red o el servidor no responde, también limpiar el token si es 401
    if (error.response?.status === 401 || (error.code === 'ERR_NETWORK' && error.config?.url?.includes('/products'))) {
      // Token inválido o expirado, o error de red en petición autenticada
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      // Solo redirigir si no estamos ya en la página de login
      if (window.location.pathname !== '/login' && window.location.pathname !== '/register') {
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

export default apiClient;

