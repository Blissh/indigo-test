import apiClient from './apiClient';

export interface TopProduct {
  productId: number;
  productName: string;
  productImage: string;
  quantitySold: number;
  revenue: number;
  ranking: number;
}

export interface SalesSummary {
  date: string;
  totalSales: number;
  totalRevenue: number;
  totalProducts: number;
}

export interface SalesReport {
  startDate: string;
  endDate: string;
  totalSales: number;
  totalRevenue: number;
  averageTicket: number;
  topProducts: TopProduct[];
  dailySummary: SalesSummary[];
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
}

export const reportService = {
  getSalesReport: async (startDate?: Date, endDate?: Date): Promise<SalesReport> => {
    const params = new URLSearchParams();
    if (startDate) {
      params.append('startDate', startDate.toISOString());
    }
    if (endDate) {
      params.append('endDate', endDate.toISOString());
    }
    
    const queryString = params.toString();
    const url = `/reports/sales${queryString ? `?${queryString}` : ''}`;
    
    const response = await apiClient.get<ApiResponse<SalesReport>>(url);
    return response.data.data;
  },
};

