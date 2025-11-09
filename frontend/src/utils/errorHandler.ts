import { AxiosError } from 'axios';

export interface ApiErrorResponse {
  success?: boolean;
  message?: string;
  errors?: string[];
  error?: string;
}

/**
 * Extrae el mensaje de error de una respuesta de Axios
 */
export const getErrorMessage = (error: unknown): string => {
  if (error instanceof AxiosError) {
    // Si hay una respuesta del servidor
    if (error.response?.data) {
      const data = error.response.data as ApiErrorResponse;
      
      // Intentar obtener el mensaje del formato ApiResponse
      if (data.message) {
        return data.message;
      }
      
      // Intentar obtener el mensaje del campo error
      if (data.error) {
        return data.error;
      }
      
      // Si hay un array de errores
      if (data.errors && Array.isArray(data.errors) && data.errors.length > 0) {
        return data.errors.join(', ');
      }
      
      // Si el data es un string directamente
      if (typeof data === 'string') {
        return data;
      }
    }
    
    // Si hay un mensaje en el error
    if (error.message) {
      return error.message;
    }
  }
  
  // Si es un Error estándar
  if (error instanceof Error) {
    return error.message;
  }
  
  // Mensaje por defecto
  return 'Ha ocurrido un error desconocido';
};

