import apiClient from './apiClient';

export interface SaleItem {
  id: number;
  saleId: number;
  productId: number;
  productName: string;
  quantity: number;
  price: number;
  total: number;
  createdAt: string;
  updatedAt: string;
}

export interface Sale {
  id: number;
  date: string;
  total: number;
  items: SaleItem[];
}

export interface CreateSaleItemRequest {
  productId: number;
  quantity: number;
}

export interface CreateSaleRequest {
  items: CreateSaleItemRequest[];
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export const saleService = {
  getAll: async (): Promise<Sale[]> => {
    const response = await apiClient.get<ApiResponse<Sale[]>>('/sales');
    return response.data.data;
  },

  getById: async (id: number): Promise<Sale> => {
    const response = await apiClient.get<ApiResponse<Sale>>(`/sales/${id}`);
    return response.data.data;
  },

  create: async (sale: CreateSaleRequest): Promise<Sale> => {
    const response = await apiClient.post<ApiResponse<Sale>>('/sales', sale);
    return response.data.data;
  },

  delete: async (id: number): Promise<Sale> => {
    const response = await apiClient.delete<ApiResponse<Sale>>(`/sales/${id}`);
    return response.data.data;
  },
};

