import apiClient from './apiClient';

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  image: string;
}

export interface CreateProductRequest {
  name: string;
  description: string;
  price: number;
  stock: number;
  image: string;
}

export interface UpdateProductRequest {
  name: string;
  description: string;
  price: number;
  stock: number;
  image: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export const productService = {
  getAll: async (): Promise<Product[]> => {
    const response = await apiClient.get<ApiResponse<Product[]>>('/products');
    return response.data.data;
  },

  getById: async (id: number): Promise<Product> => {
    const response = await apiClient.get<ApiResponse<Product>>(`/products/${id}`);
    return response.data.data;
  },

  create: async (product: CreateProductRequest): Promise<Product> => {
    const response = await apiClient.post<ApiResponse<Product>>('/products', product);
    return response.data.data;
  },

  update: async (id: number, product: UpdateProductRequest): Promise<Product> => {
    const response = await apiClient.put<ApiResponse<Product>>(`/products/${id}`, product);
    return response.data.data;
  },

  delete: async (id: number): Promise<Product> => {
    const response = await apiClient.delete<ApiResponse<Product>>(`/products/${id}`);
    return response.data.data;
  },

  uploadImage: async (file: File): Promise<string> => {
    const formData = new FormData();
    formData.append('image', file);
    
    const response = await apiClient.post<ApiResponse<string>>('/products/upload-image', formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    });
    return response.data.data;
  },
};

