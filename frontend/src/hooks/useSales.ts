import { useState, useEffect } from 'react';
import { saleService, type Sale } from '../api/saleService';
import { getErrorMessage } from '../utils/errorHandler';

export const useSales = () => {
  const [sales, setSales] = useState<Sale[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchSales = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await saleService.getAll();
      setSales(data);
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSales();
  }, []);

  return { sales, loading, error, refetch: fetchSales };
};

