import { apiRequest } from './client';
import type { Payment } from '../types';

export async function getAllPayments(): Promise<Payment[]> {
  return apiRequest<Payment[]>('/payments/api/payments');
}

export async function completePayment(id: number): Promise<void> {
  await apiRequest<void>(`/payments/api/payments/${id}/complete`, {
    method: 'POST',
  });
}
